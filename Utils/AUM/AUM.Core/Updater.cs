namespace AUM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Transactions;
    using db;
    using Enums;
    using Helper;
    using ICSharpCode.SharpZipLib.Zip;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;
    using Transactions;
    using UpdateStatus = Enums.UpdateStatus;


    public class Updater
    {
        protected readonly ConfigSettings Settings;
        public const int EmptyIdAdminRecord = -1;


        public string WorkingDir { get; private set; }

        public Updater(ConfigSettings settings)
        {
            if (null == settings)
            {
                throw new ArgumentNullException("settings");
            }

            Settings = settings;
            SetWorkingDirectory(settings);
        }

        private void SetWorkingDirectory(ConfigSettings settings)
        {
            if (ProductHelper.IsEIDSSProgram(settings.ProgramId))
            {
                WorkingDir = UpdHelper.EIDSSPath;
            }
            else if (ProductHelper.IsNSProgram(settings.ProgramId))
            {
                WorkingDir = UpdHelper.NSPath;
            }
            else if (ProductHelper.IsAUMUpdate(settings.ProgramId))
            {
                WorkingDir = UpdHelper.AUMPath;
            }
            else if (ProductHelper.IsDBUpdate(settings.ProgramId)
                     || ProductHelper.IsDBaUpdate(settings.ProgramId)
                     || ProductHelper.IsAvrServiceDbProgram(settings.ProgramId))
            {
                WorkingDir = FileHelper.GetExecutingPath();
            }
            else if (ProductHelper.IsAvrServiceProgram(settings.ProgramId))
            {
                WorkingDir = UpdHelper.AvrServicePath;
            }
        }

        static Updater()
        {
            Report = new StringBuilder();
            //TODO ��������� �� ���?
            if (ZipConstants.DefaultCodePage == 1)
                ZipConstants.DefaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;
        }

        public static void SetUpdateState(
          string message,
          string programId,
          Version version,
          MachineLevel.Level machineLevel,
          bool success = true)
        {
            AUMLog.WriteInLog(message);
            const string UpdateMessage = "This component needs no update";
            //var updates = DatabaseHelper.GetUpdatesInfo(Environment.MachineName, );
            //if (!updates.Any(c => (c.Alias == programId) && (c.VersionFinish == version)))

            var id = AdminHelper.WriteUpdateStartedToAdminDB(programId, version, version);
            AdminHelper.WriteUpdateFinishedToAdminDB(id, version, success, UpdateMessage);

            FileHelper.WriteLogFile(
              FileHelper.GetExecutingPath(),
              success,
              UpdateMessage,
              version.ToString(),
              programId,
              machineLevel,
              message);
        }

        /// <summary>
        /// ����������� ������������� ������ ������
        /// </summary>
        /// <param name="updatePackageFileName">������ ���� � ������ �������</param>
        /// <param name="productId">����������� �������</param>
        /// <returns></returns>
        private bool IsThisUpdateRequired(string updatePackageFileName, string productId)
        {
            if (MutexHelper.MutexExists(MutexType.DbUpdate))
            {
                AUMLog.WriteInLog("Check update is skipped: db update is currently running");
                return false;
            }
            AUMLog.WriteInLog("Check update");

            var fZip = UpdHelper.CreateZipper();
            fZip.ExtractZip(updatePackageFileName, UpdHelper.TempDirPath, FileHelper.FileNameUpdate);
            //���� ������� ����������� ����, �� ��������� ������ ������� �� ����
            var updFileName = Path.Combine(UpdHelper.TempDirPath, FileHelper.FileNameUpdate);
            if (!File.Exists(updFileName))
            {
                return false;
            }

            var updateSettings = UpdHelper.ReadUpdateFile(updFileName);

            if (DoesUpdateFailedByCortege(updateSettings))
            {
                return false;
            }

            Settings.VersionOfUpdate = updateSettings.Version;

            //��������, ��� ������ �� ��������� ���� ������
            if (!updateSettings.ProgramID.Equals(productId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //�������� ������
            //��������� ���� ������ ��������, ���� ������ ��
            if (ProductHelper.IsDBUpdate(productId) && (updateSettings.SqlScriptsList.Count > 0))
            {
                #region �������� ������ ��

                //��������� ������� ������ ��
                var currentDbVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbMain);

                if (!UpdHelper.CompareDbVersion(Settings.VersionOfUpdate, currentDbVersion))
                {
                    SetUpdateState(
                      "Check update failed: database Version is newest or equal than in update",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                #endregion
            }
            else if (ProductHelper.IsDBaUpdate(productId) && (updateSettings.SqlScriptsList.Count > 0))
            {
                #region �������� ������ �� (��������)

                //��������� ������� �������� �� �� ���� �����
                if (RegistryHelper.ReadFromRegistry(RegistryHelper.ArchiveServerKey).Length == 0)
                {
                    SetUpdateState(
                      "Check update failed: Archive Database is not deployed at this machine",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                //��������� ������� ������ ��
                var currentDBaVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbArchive);

                if (!UpdHelper.CompareDbVersion(Settings.VersionOfUpdate, currentDBaVersion))
                {
                    SetUpdateState(
                      "Check update failed: Archive Database version is newest or equal than in update",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                #endregion
            }
            else if (ProductHelper.IsAvrServiceDbProgram(productId))
            {
                #region �������� ������ AVR Service ��

                //��������� ������� AVR Service
                if (RegistryHelper.ReadFromRegistry(RegistryHelper.AvrServiceServerKey).Length == 0)
                {
                    SetUpdateState(
                      "Check update failed: AVR Service (and its database) is not installed at this machine",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                //��������� ������� ������ ��
                var currentAvrDbVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.AVRServiceDb);

                if (!UpdHelper.CompareDbVersion(Settings.VersionOfUpdate, currentAvrDbVersion))
                {
                    SetUpdateState(
                      "Check update failed: AVR Service database version is newest or equal than in update",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                #endregion
            }
            else if (ProductHelper.IsEIDSSProgram(productId)
                     || ProductHelper.IsNSProgram(productId)
                     || ProductHelper.IsAvrServiceProgram(productId))
            {
                #region �������� ������ ��������

                var currentVersion = VersionFactory.EmptyVersion;
                if (ProductHelper.IsEIDSSProgram(productId))
                {
                    currentVersion = FileHelper.GetEIDSSVersion(WorkingDir);
                }
                else if (ProductHelper.IsNSProgram(productId))
                {
                    currentVersion = FileHelper.GetNSVersion(WorkingDir);
                }
                else if (ProductHelper.IsAvrServiceProgram(productId))
                {
                    currentVersion = FileHelper.GetAvrServiceVersion(WorkingDir);
                }
                if (currentVersion == VersionFactory.EmptyVersion)
                {
                    AUMLog.WriteInLog("Check update failed: can't get current Version");
                    return false;
                }
                if (Settings.VersionOfUpdate <= currentVersion)
                {
                    SetUpdateState(
                      "Check update failed: installed version is equal or newer than update version",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                #endregion
            }
            else if (ProductHelper.IsExecuteUpdate(productId))
            {
                #region ��������� �� �������, ���������� �� ����� ������

                //���� ��� ���� ����� ������ (������ ����������), �� �������
                if (RegistryHelper.DoesExecuteRegistryKeyExist(Settings.VersionOfUpdate))
                {
                    SetUpdateState(
                      "Check update failed: this update is already applied",
                      productId,
                      Settings.VersionOfUpdate,
                      Settings.Level);
                    return false;
                }

                #endregion
            }

            if (!IsCRCCorrect(updatePackageFileName))
            {
                return false;
            }
            AUMLog.WriteInLog("Update is correct and required");
            return true;
        }
        private bool IsThisAUMUpdateRequired(string updatePackageFileName, string productId)
        {
            AUMLog.WriteInLog("Check update");

            var fZip = UpdHelper.CreateZipper();
            fZip.ExtractZip(updatePackageFileName, UpdHelper.TempDirPath, FileHelper.FileNameUpdate);
            //���� ������� ����������� ����, �� ��������� ������ ������� �� ����
            var updFileName = Path.Combine(UpdHelper.TempDirPath, FileHelper.FileNameUpdate);
            if (!File.Exists(updFileName))
            {
                return false;
            }

            var updateSettings = UpdHelper.ReadUpdateFile(updFileName);

            if (DoesUpdateFailedByCortege(updateSettings))
            {
                return false;
            }

            Settings.VersionOfUpdate = updateSettings.Version;

            if (!updateSettings.ProgramID.Equals(productId, StringComparison.OrdinalIgnoreCase)
              || !ProductHelper.IsAUMUpdate(productId))
            {
                return false;
            }

            var currentVersion = VersionFactory.EmptyVersion;
            if (ProductHelper.IsAUMUpdate(productId))
            {
                currentVersion = FileHelper.GetAUMVersion(WorkingDir);
            }
            if (currentVersion == VersionFactory.EmptyVersion)
            {
                AUMLog.WriteInLog("Check update failed: can't get current Version");
                return false;
            }
            if (Settings.VersionOfUpdate <= currentVersion)
            {
                SetUpdateState(
                  "Check update failed: installed version is equal or newer than update version",
                  productId,
                  Settings.VersionOfUpdate,
                  Settings.Level);
                return false;
            }

            if (!IsCRCCorrect(updatePackageFileName))
            {
                return false;
            }
            AUMLog.WriteInLog("Update is correct and required");
            return true;
        }

        protected static bool IsCRCCorrect(string updatePackageFileName)
        {
            #region �������� ����������� �����

            long updatePackageCRC;
            //�������� ����������� �����
            var crcFileInfo = new FileInfo(FileHelper.GetCRCFilename(updatePackageFileName));
            if (!crcFileInfo.Exists)
            {
                AUMLog.WriteInLog("Check update failed: crc is not exists");
                return false;
            }
            using (var sr = crcFileInfo.OpenText())
            {
                if (!long.TryParse(sr.ReadLine(), out updatePackageCRC))
                {
                    sr.Close();
                    AUMLog.WriteInLog("Check update failed: can't read crc");
                    return false;
                }
            }

            if (updatePackageCRC != FileHelper.CaclulateCRC(updatePackageFileName))
            {
                SecurityLog.WriteToEventLogWindows(
                  EventLogEntryType.Error,
                  ResourceHelper.Get("CRCIsInvalidError"), updatePackageFileName);
                AUMLog.WriteInLog("Check update failed: crc is wrong");
                return false;
            }

            #endregion

            return true;
        }

        private static bool DoesUpdateFailedByCortege(UpdateSettings updateSettings)
        {
            var isValidVersionByCortege = true;

            //�������, ��� ����� ������ total/small ����� �� ����� ��������� ����� ������� ������
            if (!updateSettings.IsSmallUpdate || string.IsNullOrEmpty(updateSettings.CortegeVersion))
            {
                return false;
            }

            AUMLog.WriteInLog("Cortege check");
            foreach (var cortege in UpdHelper.ParseUpdateVersions(updateSettings.CortegeVersion))
            {
                if (ProductHelper.IsDBUpdate(cortege.ProductID))
                {
                    //��������� ������� ������ ��
                    var currentVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbMain);
                    AUMLog.WriteInLog("Current DB Version: {0}", currentVersion);
                    isValidVersionByCortege = cortege.IsValidVersion(currentVersion);
                }
                else if (ProductHelper.IsDBaUpdate(cortege.ProductID))
                {
                    //��������� ������� ������ �� (��������)
                    var currentVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbArchive);
                    AUMLog.WriteInLog("Current DBa Version: {0}", currentVersion);
                    isValidVersionByCortege = cortege.IsValidVersion(currentVersion);
                }
                else if (ProductHelper.IsAUMUpdate(cortege.ProductID))
                {
                    var currentVersion = FileHelper.GetAUMVersion(UpdHelper.AUMPath); //���� � AUM
                    AUMLog.WriteInLog("Current AUM Version: {0}", currentVersion);
                    isValidVersionByCortege = cortege.IsValidVersion(currentVersion);
                }
                if (ProductHelper.IsEIDSSProgram(cortege.ProductID))
                {
                    //���� � EIDSS
                    var eidssPath = UpdHelper.EIDSSPath;
                    if (!string.IsNullOrEmpty(eidssPath))
                    {
                        var currentVersion = FileHelper.GetEIDSSVersion(eidssPath);
                        AUMLog.WriteInLog("Current EIDSS Version: {0}", currentVersion);
                        isValidVersionByCortege = cortege.IsValidVersion(currentVersion);
                    }
                    else
                    {
                        isValidVersionByCortege = false;
                    }
                }
                else if (ProductHelper.IsNSProgram(cortege.ProductID))
                {
                    //���� � NS
                    var nsPath = UpdHelper.NSPath;
                    if (!string.IsNullOrEmpty(nsPath))
                    {
                        var currentVersion = FileHelper.GetNSVersion(nsPath);
                        AUMLog.WriteInLog("Current NS Version: {0}", currentVersion);
                        isValidVersionByCortege = cortege.IsValidVersion(currentVersion);
                    }
                    else
                    {
                        isValidVersionByCortege = false;
                    }
                }
                if (!isValidVersionByCortege)
                {
                    AUMLog.WriteInLog("Check update failed: current Version failed by cortege");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programUpdate">�������� ������� ��� AUM</param>
        /// <param name="idAdminRecord"></param>
        public void RunAUMUpdate(string programUpdate, long idAdminRecord)
        {
            AUMLog.WriteInLog("AUM self-update is started");

            //��� 1. ���� ���������� �������� �� ��������� �������� ����������� ��������� � � ������ ������, ��� ��, ��� � ������ ����������,
            //�� ��������� ����� �� ����������� AUM �� ��������� �������, � ������� AUM.exe ������. ���������� �������.
            //���� ������ � ��������� ������, �� ������ ���� ���� ��������.

            //��� 2. ���� ���������� �������� �� �� ��������� �������� ����������� ���������, �� ��� "���������" ������,
            //������� ������������ ��� ���������� ������ AUM. ��������� ����� ������ AUM � �������� ������� ����������� ��������� 
            //� �������� AUM.Launcher.exe �� ��������� �������� ���������, ���� ��� �� ��������� �����, � ������ AUM.Service.exe, ���� ���������.
            //��� ���� ������ �� ���������!

            //��� 3. ���� ���������� �������� �� ��������� �������� ����������� ��������� � � ������ �� ������ ���, ��� � ������ ����������
            //�� ������ ������ �� ���� --> AUM ��� �������.

            var appDir = FileHelper.GetExecutingPath();

            var appDirLocal = appDir.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? appDir : appDir + "\\";
            AUMLog.WriteInLog("AppPath = {0}", appDirLocal);
            var rootDirLocal = WorkingDir.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? WorkingDir : WorkingDir + "\\";
            AUMLog.WriteInLog("RootDir = {0}", rootDirLocal);
            var arePathsEqual = appDirLocal.Equals(rootDirLocal, StringComparison.OrdinalIgnoreCase);
            AUMLog.WriteInLog("Is equal = {0}", arePathsEqual);

            if (arePathsEqual)
            {
                if (!IsThisAUMUpdateRequired(programUpdate, Settings.ProgramId))
                {
                    return;
                }
                AUMUpdateStep1(appDirLocal, rootDirLocal, programUpdate);
            }
            else
            {
                AUMUpdateStep2(appDirLocal, rootDirLocal, idAdminRecord);
            }
        }

        private void AUMUpdateStep1(string appDir, string rootDir, string programUpdate)
        {
            AUMLog.WriteInLog("AUM self-update 1 step");

            //������� ���� ��������� ���������, ������� ����� �������� � ���������� ��������������
            var oldDirs = Directory.GetDirectories(UpdHelper.TempDirPath, "AUM*");
            foreach (var oldDir in oldDirs)
            {
                FileHelper.DeleteDir(oldDir, true); //��� ��������� ������� ���� �������
            }

            //������ ����������� AUM.exe
            var exePath = Path.Combine(appDir, UpdHelper.AUMFileName);
            var versionThisUpdater = VersionFactory.NewVersion(FileVersionInfo.GetVersionInfo(exePath).FileVersion);
            //������� �������� ����� ������������
            var tempDir = Path.Combine(UpdHelper.TempDirPath, string.Format("AUM{0}", DateTime.Now.Ticks));
            FileHelper.DeleteDir(tempDir, true);
            Thread.Sleep(2000);
            var tempDirName = UpdHelper.ExtractDirFromUpdate(programUpdate, tempDir, string.Empty);
            if (tempDirName.Length <= 0)
            {
                return;
            }

            var tempFileName = Path.Combine(tempDirName, UpdHelper.AUMFileName);
            var versionTempUpdater = VersionFactory.NewVersion(FileVersionInfo.GetVersionInfo(tempFileName).FileVersion);
            if (versionThisUpdater >= versionTempUpdater)
            {
                return;
            }

            AUMLog.WriteInLog("AUM self-update stop all AUM programs");
            //AUM ��������� � ����������
            var progList = new List<string>
      {
        UpdHelper.AUMAdministratorFileName
      };
            foreach (var prname in progList)
            {
                string errorMessage;
                string processFileName;
                FileHelper.StopProcess(prname, out errorMessage, out processFileName);
                if (errorMessage.Length <= 0)
                {
                    continue;
                }

                var errStr = string.Format("Can't unload {0}. Error: {1}", prname, errorMessage);
                AUMLog.WriteInLog(errStr);
                SecurityLog.WriteToEventLogWindows(EventLogEntryType.Error, errStr);
                //����� �������� ���
                FileHelper.WriteLogFile(
                  rootDir,
                  false,
                  errStr,
                  FileVersionInfo.GetVersionInfo(exePath).FileVersion,
                  ProductHelper.AumProgramId,
                  Settings.Level,
                  string.Empty);
                return;
            }

            ServiceHelper.AUMServiceChangeState(false);
            AUMLog.WriteInLog("AUM self-update all AUM programs is stoped");

            //��������� � ��� ������� �� �������� �������� ������ bvupdater.config � ����������� �� ������� ������
            var bvupdaterConfigTempFileName = Path.Combine(tempDirName, FileHelper.BVUpdaterConfigFileName);
            File.Copy(Path.Combine(appDir, FileHelper.BVUpdaterConfigFileName), bvupdaterConfigTempFileName, true);

            var idAdminRecord = WriteAUMUpdateStartedToAdminDB(versionThisUpdater, versionTempUpdater);

            //����� ���������� AUM.exe
            //���������� ����������
            var aumCommandLineArguments = string.Format(
              CultureInfo.CurrentCulture,
              "/f{0} /a{1} /s:{2}",
              Path.GetFileNameWithoutExtension(programUpdate),
              idAdminRecord,
              bvupdaterConfigTempFileName);
            AUMLog.WriteInLog("AUM self-update start {0} {1}", UpdHelper.AUMFileName, aumCommandLineArguments);
            UpdHelper.RunProcess(tempFileName, aumCommandLineArguments, false, true);

            AUMLog.WriteInLog("AUM self-update 1 step finished");
        }

        private void AUMUpdateStep2(string appDir, string rootDir, long idAdminRecord)
        {
            AUMLog.WriteInLog("AUM self-update 2 step");

            var success = false;
            var dtStart = DateTime.Now;
            var waitTime = new TimeSpan(0, 5, 0);
            AUMLog.WriteInLog("AUM self-update 2 step files copy started");

            var wasError = false;
            var fileManager = new TxFileManager();
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    while (!success && ((DateTime.Now - dtStart) <= waitTime))
                    {
                        //�������� ��������� ����� ��� ������, ������� ����� ����������
                        //���������� ������ ������, ������� ����� ��������
                        //���� ������ ����� ������ ��� (�� ��������������� ����������) � ����� �����
                        var allFiles = Directory.GetFiles(appDir, "*.*", SearchOption.AllDirectories).ToList();
                        //��������� ������ ������������� ���� � ������
                        SetRelativePaths(ref allFiles, appDir);

                        //���������� ����� �����
                        var newFiles =
                          (from file in allFiles
                           let targetFile =
                    Path.Combine(rootDir, file)
                           where !File.Exists(targetFile)
                           select file).ToList();

                        var aumFiles = Directory.GetFiles(appDir, "*AUM*.exe", SearchOption.AllDirectories).ToList();
                        aumFiles.AddRange(Directory.GetFiles(appDir, "*AUM*.dll", SearchOption.AllDirectories).ToList());
                        aumFiles.AddRange(Directory.GetFiles(appDir, "*bvupdate*.exe", SearchOption.AllDirectories).ToList());
                        aumFiles.AddRange(Directory.GetFiles(appDir, "*bvupdate*.dll", SearchOption.AllDirectories).ToList());

                        SetRelativePaths(ref aumFiles, appDir);

                        //����� �������� ������ ��� ����������
                        var updateFiles = new List<string>();
                        updateFiles.AddRange(newFiles);
                        foreach (var aumFile in aumFiles.Where(aumFile => !updateFiles.Contains(aumFile)))
                        {
                            updateFiles.Add(aumFile);
                        }

                        //��������� �������� ������, ������� �������������� ��������
                        AUMLog.WriteInLog("AUM Files for update:");
                        for (var i = 0; i < updateFiles.Count - 1; i++)
                        {
                            AUMLog.WriteInLog("#{0} {1}", i, updateFiles[i]);
                        }

                        foreach (var targetFile in updateFiles.Select(updateFile => Path.Combine(rootDir, updateFile)).Where(File.Exists))
                        {
                            AUMLog.WriteInLog("Deleting {0}", targetFile);
                            fileManager.Delete(targetFile);
                            //FileHelper.DeleteFile(targetFile);
                            AUMLog.WriteInLog("{0} deleted!", targetFile);
                        }

                        //����� ��� ����� �������, ����� ���������� �� �� ����� ����� ������
                        //��������������� ��� ��������, ���� �� ������� �����������
                        //�������� ����� ������ �����
                        foreach (var updateFile in updateFiles)
                        {
                            var targetFile = Path.Combine(rootDir, updateFile);
                            var sourceFile = Path.Combine(appDir, updateFile);
                            AUMLog.WriteInLog("Copyng {0} to {1}", sourceFile, targetFile);
                            fileManager.Copy(sourceFile, targetFile, true);
                            AUMLog.WriteInLog("{0} copied", sourceFile);
                        }
                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception exc)
            {
                AUMLog.WriteInLog("AUM self-update error: {0}", exc.Message);
                wasError = true;
            }
            success = true;
            var exePath = Path.Combine(appDir, UpdHelper.AUMFileName);
            if (!wasError)
            {
                AUMLog.WriteInLog("AUM self-update 2 step files copy finished");
                //������� ���� run.upd, ���� ������� ����� � ������� (�� ������������ ��������� ����)
                FileHelper.DeleteRunUpd(rootDir);
            }
            //����� � ��� � ���������� ����������
            AUMLog.WriteInLog("AUM self-update 2 step Settins level = {0}", Settings.Level);
            FileHelper.WriteLogFile(rootDir, success, "AUM updated",
              FileVersionInfo.GetVersionInfo(exePath).FileVersion,
              ProductHelper.AumProgramId, Settings.Level, string.Empty);

            //We excluded Aum launcher from aum components and shall remove from autos start 
            //Delete all possible AUM.Launcher records from auto run, if autorun record exists
            RegistryHelper.DeleteFromAutorun("EIDSS AUM60 launcher");
            RegistryHelper.DeleteFromAutorun("EIDSS AUM launcher");
            //AUM is always managed by service now and we shall make aum service start mode to automatic if this was not done yet
            var err = ServiceHelper.ChangeStartModeWithoutThrow(UpdHelper.AUMServiceName, System.ServiceProcess.ServiceStartMode.Automatic, false);
            if (err != null)
                FileHelper.WriteLogFile(rootDir, success, "Switch AUM Service to autostart mode failed",
                  FileVersionInfo.GetVersionInfo(exePath).FileVersion,
                  ProductHelper.AumProgramId, Settings.Level, err);

            //���������� ���������� ������
            AUMLog.WriteInLog("AUM self-update 2 step restart service");
            ServiceHelper.AUMServiceChangeState(true);
            AUMLog.WriteInLog("AUM self-update 2 step service restarted");
            //����� � �� Admin
            var versionThisUpdater = VersionFactory.NewVersion(FileVersionInfo.GetVersionInfo(exePath).FileVersion);
            WriteAUMUpdateFinishedToAdminDB(idAdminRecord, versionThisUpdater, success);
            //TODO ��� ����� ���-�� �������� ������ ������?
            AUMLog.WriteInLog("AUM self-update 2 step finished");
        }

        protected virtual long WriteAUMUpdateStartedToAdminDB(Version versionThisUpdater, Version targetVersion)
        {
            var idAdminRecord = AdminHelper.WriteUpdateStartedToAdminDB(ProductHelper.AumProgramId, versionThisUpdater, targetVersion);
            return idAdminRecord;
        }

        protected virtual void WriteAUMUpdateFinishedToAdminDB(long idAdminRecord, Version versionThisUpdater, bool success)
        {
            if (idAdminRecord > EmptyIdAdminRecord)
            {
                AdminHelper.WriteUpdateFinishedToAdminDB(idAdminRecord, versionThisUpdater, success, "AUM updated");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allFiles"></param>
        /// <param name="appDir"></param>
        private static void SetRelativePaths(ref List<string> allFiles, string appDir)
        {
            var l = appDir.Length;
            for (var i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Substring(l);
            }
        }

        public delegate void AddLogStringHandler(string message);

        /// <summary>
        /// ������� �����������, ����� ����������� ����� ������ � ���
        /// </summary>
        public event AddLogStringHandler AddLogString;

        public delegate void SetMaxProgressBarHandler();

        /// <summary>
        /// ����������� ������������� �������� ��� Progress Bar
        /// </summary>
        public event SetMaxProgressBarHandler SetMaxProgressBar;

        public delegate void MoveNextProgressBarHandler();

        /// <summary>
        /// �������� �������� ���� �� �������
        /// </summary>
        public event MoveNextProgressBarHandler MoveNextProgressBar;

        public delegate void SetProgressbarMaxValueHandler(int maxValue);

        /// <summary>
        /// ����������� ������ (������������) ������� ������������
        /// </summary>
        public event SetProgressbarMaxValueHandler SetProgressbarMaxValue;

        /// <summary>
        /// ����������� ������ (������������) ������� ������������
        /// </summary>
        protected void InvokeSetProgressbarMaxValue(int maxValue)
        {
            if (SetProgressbarMaxValue != null)
                SetProgressbarMaxValue(maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void InvokeAddLogString(string message)
        {
            if (AddLogString == null)
                return;
            AddLogString(message);
        }

        /// <summary>
        /// ����������� ������������� �������� ��� Progress Bar
        /// </summary>
        protected void InvokeSetMaxProgressBar()
        {
            if (SetMaxProgressBar != null)
                SetMaxProgressBar();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeMoveNextProgressBar()
        {
            if (MoveNextProgressBar != null)
            {
                MoveNextProgressBar();
            }
        }

        /// <summary>
        /// ���� ���������� ��� ���������, ������� ��������� �� ����� ����������
        /// </summary>
        protected static StringBuilder Report { get; set; }

        protected void AddReportString(string format, params object[] args)
        {
            var message = string.Format(CultureInfo.InvariantCulture, format, args);
            Report.AppendLine(message);
            InvokeAddLogString(message);
        }

        protected void AddReportStringEx(string format, params object[] args)
        {
            var message = string.Format(CultureInfo.InvariantCulture, format, args);
            AddReportString(message);
            AUMLog.WriteInLog(message);
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="updateFilename"></param>
        /// <param name="eidssVersion">(�����������) ������ EIDSS, �� ������� � ���� ������ �� ����� �������</param>
        /// <returns></returns>
        public virtual void RunUpdate(string updateFilename, Version eidssVersion)
        {
            if (!IsThisUpdateRequired(updateFilename, Settings.ProgramId))
            {
                return;
            }

            //����� ����� ��������� � �������� backups\Update<������>
            var tempDir = Path.Combine(Settings.BackupPath,
                                          Path.GetFileNameWithoutExtension(updateFilename) ?? string.Empty);
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            var conn = OpenDbMainConnection();
            if (null == conn)
            {
                return;
            }

            SecurityLog.WriteToEventLogDB(conn, Operations.Update, true,
                                            string.Empty,
                                            string.Format(ResourceHelper.Get("UpdateProcessStarted"), DateTime.Now));

            var updateMessenger = new UpdateMessenger(conn);

            AddReportString(string.Format(ResourceHelper.Get("UpdateProcessStarted"), DateTime.Now));

            //����� ���-�� ������������ ������
            InvokeSetProgressbarMaxValue(10);

            var workDir = Path.Combine(UpdHelper.TempDirPath, Path.GetFileNameWithoutExtension(updateFilename) ?? string.Empty);

            //����������� ����� � �������� �������
            var fZip = ExtractUpdateFiles(updateFilename, workDir);

            InvokeMoveNextProgressBar();

            var updFileName = Path.Combine(workDir, FileHelper.FileNameUpdate);
            //������� ��������� �������
            var settings = UpdHelper.ReadUpdateFile(updFileName);

            //��� ���������� �� � AUM �� ����� clientID
            var clientId = UpdHelper.GetClientID(settings.ProgramID);

            CreateUpdateBlock(clientId, settings, updateMessenger);

            #region ��������� ��������� �� ������ ������

            //��������, ������� ����� ����������
            var processes = new List<string> { UpdHelper.ClientAgentProcessName };
            //������������ "������������� �����", �� ���� ��� ����������
            if (
                ProductHelper.IsEIDSSProgram(settings.ProgramID)
                || ProductHelper.IsDBUpdate(settings.ProgramID)
                || ProductHelper.IsDBaUpdate(settings.ProgramID)
                || ProductHelper.IsAvrServiceProgram(settings.ProgramID)
                || ProductHelper.IsAvrServiceDbProgram(settings.ProgramID)
                )
            {
                if (FileHelper.IsProcessExists(UpdHelper.EIDSSFileName))
                {
                    AddReportStringEx(string.Format("Try to stop '{0}'", UpdHelper.EIDSSFileName));

                    AddReportStringEx(string.Format("Wait for '{0}' will close", UpdHelper.EIDSSFileName));
                    var terminateEIDSSHelper = new TerminateEIDSSHelper();
                    while (!terminateEIDSSHelper.CanClose)
                    {
                        Thread.Sleep(1000);
                    }
                }

                processes.Add(UpdHelper.EIDSSFileName);
            }

            FileHelper.StopProcesses(processes, conn);

            //���� ��������� ������� NS, �� ��� ������������� ��� ������
            var nsWasStopped = false;
            if (ProductHelper.IsNSProgram(settings.ProgramID)
                || ProductHelper.IsDBUpdate(settings.ProgramID)
                || ProductHelper.IsDBaUpdate(settings.ProgramID))
            {
                ServiceHelper.NotificationServiceChangeState(false);
                nsWasStopped = true;
            }

            //���� AVR Service �����������, �� �������� ���
            var avrServiceWasStopped = false;
            if (
                ProductHelper.IsAvrServiceProgram(settings.ProgramID)
                ||
                ProductHelper.IsAvrServiceDbProgram(settings.ProgramID)
                )
            {
                ServiceHelper.AvrServiceChangeState(false);
                avrServiceWasStopped = true;
            }

            //������� ����������� ����� ����������
            DeleteRunningApps(updateMessenger, clientId, settings);

            #endregion

            var success = false;
            var errorMessage = string.Empty; //���� ����� ��� ��������� �� �������
            var message = string.Empty;

            var idAdminRecord = RegisterUpdateStart(WorkingDir, settings);
            var appPath = FileHelper.GetExecutingPath();

            //���������� AUM ������ � Program.cs
            if (ProductHelper.IsDBUpdate(settings.ProgramID)
                || ProductHelper.IsDBaUpdate(settings.ProgramID)
                || ProductHelper.IsAvrServiceDbProgram(settings.ProgramID)
                )
            {
                #region ���������� ��

                AddReportStringEx(string.Format("{0} update started", settings.ProgramID));
                var updateDatabase = DatabaseHelper.GetUpdateDatabase(settings.ProgramID);
                var sqlConnection = DatabaseHelper.GetConnection(updateDatabase);
                var serverConnection = new ServerConnection(sqlConnection);
                var server = new Server(serverConnection);
                var mainDatabaseSMO = server.Databases[sqlConnection.Database];
                AddReportStringEx(string.Format("Server name={0}; Database name={1}", server.Name, mainDatabaseSMO.Name));

                //�������� ������� ���������� ����� �� ����� (��������������)
                try
                {
                    var disk = tempDir.Substring(0, 1).ToUpperInvariant();
                    var freeSpace = FileHelper.GetDiskFreeSpace(disk) / (1024 * 1024);
                    mainDatabaseSMO.RecalculateSpaceUsage();
                    AddReportStringEx(string.Format("Free space={0}; DB SpaceUsage={1}", freeSpace, mainDatabaseSMO.Size));
                    if (freeSpace < mainDatabaseSMO.Size * 0.7)
                    //�������, ��� ����� ������ ���� �� ����� 70% �� ��������� ������� ��
                    {
                        AddReportStringEx("Low free space on disk!");
                        SecurityLog.WriteToEventLogWindows(
                          EventLogEntryType.Warning,
                          ResourceHelper.Get("LowFreeSpace"),
                          disk);
                    }
                }
                catch (Exception exc)
                {
                    AddReportStringEx(string.Format("Error while getting free disk space: {0}.", exc.Message));
                }

                //������� ���� ��� �� ���������� ��� �� ������� �����, ����� �������� ����� ����������� ����������
                //��� ����� ������ ���� ����������� ��
                AddReportString(ResourceHelper.Get("WaitingForAnotherClientsExit"));

                var needCheckRunningApps = !(ProductHelper.IsAvrServiceDbProgram(settings.ProgramID) || ProductHelper.IsAvrServiceProgram(settings.ProgramID));

                //if (!m_settings.IsServiceMode && (formTerminate != null))
                //{
                //    if (needCheckRunningApps)
                //    {
                //        formTerminate.StartCheckConnections();
                //    }
                //    else
                //    {
                //        formTerminate.CanClose = true;
                //    }
                //    if (formTerminate.NeedShowForm())
                //    {
                //        if (formTerminate.ShowDialog().Equals(DialogResult.Cancel))
                //        {
                //            AddReportString(ResourceHelper.Get("UpdateInterruptedByUser"));
                //            Application.DoEvents();
                //            //������� ���������� �������
                //          DeleteUpdateBlock(updateMessenger);
                //            //��������� ��� � ����
                //            FileHelper.WriteLogFile(WorkingDir, false, Report.ToString(), settings.Version,
                //                                    settings.ProgramID, m_settings.Level,
                //                                    ResourceHelper.Get("UpdateInterruptedByUser"));
                //            AUMLog.WriteInLog("Interupted by user.");
                //            return;
                //        }
                //        if (formTerminate.WasError)
                //        {
                //            //������� ���������� �������
                //          DeleteUpdateBlock(updateMessenger);
                //            //��������� ��� � ����
                //            FileHelper.WriteLogFile(WorkingDir, false, Report.ToString(), settings.Version,
                //                                    settings.ProgramID, m_settings.Level, formTerminate.ErrorString);
                //            return;
                //        }
                //    }
                //}
                //else
                //{
                AddReportStringEx("Terminate connections...");

                var terminateDbHelper = new TerminateDbHelper(true, updateDatabase);
                if (needCheckRunningApps)
                {
                    terminateDbHelper.StartCheckConnections();
                }
                else
                {
                    terminateDbHelper.CanClose = true;
                }
                AddReportStringEx(string.Format("Open connection count={0}", terminateDbHelper.OpenConnectionsCount));
                while (!terminateDbHelper.CanClose)
                {
                    Thread.Sleep(1000);
                }
                if (terminateDbHelper.WasError)
                {
                    //������� ���������� �������
                    DeleteUpdateBlock(updateMessenger);
                    //��������� ��� � ����
                    FileHelper.WriteLogFile(WorkingDir, false, Report.ToString(), settings.Version.ToString(),
                                            settings.ProgramID, Settings.Level, terminateDbHelper.ErrorString);
                    return;
                }
                //}
                AddReportString(ResourceHelper.Get("ThereAreNoClientsMore"));

                var scriptsBackupPath = Path.Combine(tempDir, "scriptsbackupdatabase");
                AddReportStringEx(string.Format("Scripts backup path={0}", scriptsBackupPath));

                //���������� ����� �� � ���� .bak ����� � � ���� ������ ��������
                AddReportString(ResourceHelper.Get("BackupDB"));

                var bakFile = Path.Combine(tempDir, UpdHelper.BakFilename);
                var backupFileForDB = Path.Combine(UpdHelper.TempDirPath, "scriptsBackup.zip");
                var backupFileForDB2 = Path.Combine(tempDir, "scriptsBackup.zip");

                AddReportStringEx(string.Format("Bak File={0}; backupFileForDB={1}; backupFileForDB2={2}", bakFile,
                                                backupFileForDB, backupFileForDB2));

                try
                {
                    //bak
                    BackupHelper.BackupDatabase(server, mainDatabaseSMO, bakFile);
                    //�������
                    if (!Directory.Exists(scriptsBackupPath))
                        Directory.CreateDirectory(scriptsBackupPath);
                    BackupHelper.BackupDBIntoScripts(server, mainDatabaseSMO, WorkingDir, scriptsBackupPath);
                    //������� ���������� � ����� �����, ����� ��� ����� ���� ���������� ������ � ������� �������� � ����� ����� ������
                    FileHelper.DeleteFile(backupFileForDB);
                    fZip.CreateZip(backupFileForDB, scriptsBackupPath, false, string.Empty);
                    //�������� �������
                    File.Move(backupFileForDB, backupFileForDB2);
                    AddReportString(ResourceHelper.Get("BackupDBComplete"));
                }
                catch (Exception exc)
                {
                    AddReportStringEx(string.Format("Backup DB failed. Error: {0}", exc.Message));
                    SecurityLog.WriteToEventLogWindows(
                      EventLogEntryType.Warning,
                      ResourceHelper.Get("BackupFailed"),
                      exc.Message);
                    FileHelper.DeleteFile(bakFile);
                    FileHelper.DeleteFile(backupFileForDB);
                    FileHelper.DeleteFile(backupFileForDB2);
                }
                try
                {
                    AddReportString(ResourceHelper.Get("RunningScripts"));
                    serverConnection.StatementTimeout = 0;
                    bool created;
                    using (var mutex = MutexHelper.CreateMutex(MutexType.DbUpdate, out created))
                    {

                        serverConnection.BeginTransaction();

                        foreach (var script in settings.SqlScriptsList)
                        {
                            AUMLog.WriteInLog("Run script: {0}", script.FileName);
                            AddReportString(string.Format(ResourceHelper.Get("RunScriptMessage"), script.FileName));
                            using (var sr = (new FileInfo(Path.Combine(workDir, script.FileName)).OpenText()))
                            {
                                mainDatabaseSMO.ExecuteNonQuery(sr.ReadToEnd());
                            }
                        }

                        serverConnection.CommitTransaction();

                        AddReportString(ResourceHelper.Get("ScriptsSuccessfullyExecuted"));
                        mutex.ReleaseMutex();
                    }
                    //������� ����� ������ � �� �� ��� ������ �������

                    #region Version update

                    AddReportStringEx(string.Format("Try to lift db version to '{0}'...", settings.Version));

                    var parameters = new List<object> { settings.Version.ToString() };

                    var cmdtext = DatabaseHelper.CreateStoredProc("[dbo].[spAUMLocalSiteOptions_Post]", parameters);
                    if (!DatabaseHelper.ExecuteStoredProc(mainDatabaseSMO, cmdtext))
                    {
                        AddReportString("Can't lift db version. See Windows Log and aum.log for details");
                        //������� ���������� �������
                        DeleteUpdateBlock(updateMessenger);
                        //��������� ��� � ����
                        FileHelper.WriteLogFile(WorkingDir, false, Report.ToString(), settings.Version.ToString(),
                                                settings.ProgramID, Settings.Level, Report.ToString());
                        return;
                    }
                    AUMLog.WriteInLog("[dbo].[spAUMLocalSiteOptions_Post] executed without errors");

                    //������ ��� CDR, Stand alone � Web ��������� ������� tstVersionCompare
                    if (MachineLevel.IsCdr(Settings.Level) || MachineLevel.IsWeb(Settings.Level))
                    {
                        //� ��� ������� ���� ������ ������ ��� ����� ������
                        var versionDatabaseShort = string.Empty;
                        var dbVersion = settings.Version;
                        if (!dbVersion.Equals(VersionFactory.EmptyVersion))
                        {
                            versionDatabaseShort = string.Format("{0}.{1}.{2}", dbVersion.Major, dbVersion.Minor, dbVersion.Build);
                        }

                        var eidssVersionShort = string.Empty;
                        if (!eidssVersion.Equals(VersionFactory.EmptyVersion))
                        {
                            eidssVersionShort = string.Format("{0}.{1}.{2}", eidssVersion.Major, eidssVersion.Minor, eidssVersion.Build);
                        }

                        if (versionDatabaseShort.Length > 0 && eidssVersionShort.Length > 0)
                        {
                            AddReportStringEx(string.Format("Write in tstVersionCompare. eidssVersion='{0}', databaseVersion='{1}' ...", eidssVersion, versionDatabaseShort));
                            parameters.Clear();
                            parameters.Add(eidssVersionShort);
                            parameters.Add(versionDatabaseShort);
                            cmdtext = DatabaseHelper.CreateStoredProc("[dbo].[spAUMVersionCompare_Post]", parameters);

                            if (!DatabaseHelper.ExecuteStoredProc(mainDatabaseSMO, cmdtext))
                            {
                                AddReportString("Can't set Version Compare Info. See Windows Log and aum.log for details");
                                //������� ���������� �������
                                DeleteUpdateBlock(updateMessenger);
                                //��������� ��� � ����
                                FileHelper.WriteLogFile(WorkingDir, false, Report.ToString(), settings.Version.ToString(),
                                                        settings.ProgramID, Settings.Level, Report.ToString());
                                return;
                            }
                            AUMLog.WriteInLog("[dbo].[spAUMVersionCompare_Post] executed without errors");
                        }
                    }

                    #endregion

                    success = true;
                }
                catch (Exception exc)
                {
                    errorMessage = UpdHelper.GetErrorMessage(exc);
                    //������� ����������
                    serverConnection.RollBackTransaction();
                    //��������� ������������ programmability �� ��������
                    errorMessage += BackupHelper.RestoreDB(mainDatabaseSMO, scriptsBackupPath);
                }
                finally
                {
                    //��������� ���� ����������� � ��
                    updateMessenger.ManageConnections(true);
                    if ((serverConnection != null) && serverConnection.IsOpen)
                        serverConnection.Disconnect();
                }

                #endregion
            }
            else if (ProductHelper.IsEIDSSProgram(settings.ProgramID)
                     || ProductHelper.IsNSProgram(settings.ProgramID)
                     || ProductHelper.IsAvrServiceProgram(settings.ProgramID)
                )
            {
                #region ���������� ���������� EIDSS, NS, AVR Service

                success = RunApplicationUpdate(updateFilename, tempDir, workDir, out errorMessage);

                #endregion
            }
            else if (ProductHelper.IsExecuteUpdate(settings.ProgramID))
            {
                success = RunXPatch(settings, workDir, out errorMessage);
            }

            //���� ���� ������, �� ������� ���
            if (errorMessage.Length > 0)
            {
                AddReportString(errorMessage);
                SecurityLog.WriteToEventLogDB(conn, Operations.Update, success, errorMessage,
                                              string.Empty);
            }

            message +=
                string.Format(
                    success ? ResourceHelper.Get("UpdateSuccessfullyComplete") : ResourceHelper.Get("UpdateFailed"),
                    DateTime.Now);
            SecurityLog.WriteToEventLogDB(conn, Operations.Update, success, string.Empty, message);

            AddReportString(message);
            InvokeSetMaxProgressBar();

            try
            {
                var description =
                            string.Format("Date Time = {0}; Result = {1}; Updated to Version = {2}; Computer Name = {3}",
                                  DateTime.Now
                                  , success ? "success" : "fail"
                                  , settings.Version
                                  , Environment.MachineName
                        );

                //������� ���������� �������
                DeleteUpdateBlock(updateMessenger);

                //��������� ������ � ��������� ��� (��, ����� ���� �� ����������, �� � ������ Windows)
                SecurityLog.WriteToEventLogDB(conn, Operations.Update, success, errorMessage, description);

                //������ ���� � ��������� �������
                AddReportString(ResourceHelper.Get("TryCopyLogs"));

                var fullReportString = Report.ToString();
                FileHelper.WriteLogFile(appPath, success, fullReportString, settings.Version.ToString(),
                                        settings.ProgramID, Settings.Level, message);

                //����� � Admin ��������� ����������
                if (idAdminRecord > EmptyIdAdminRecord)
                {
                    AdminHelper.WriteUpdateFinishedToAdminDB(idAdminRecord, settings.Version, success, fullReportString);
                }
                else
                {
                    AUMLog.WriteInLog("Can't write into Admin DB ({0})", EmptyIdAdminRecord);
                }

                //���������� NS//���� ��������� ������� NS, �� ��� ������������� ���
                //EIDSS �� �������������, ������ ��� � ��� ����� ������ ����� ������ ������ EIDSS
                if (nsWasStopped)
                    ServiceHelper.NotificationServiceChangeState(true);

                //���������� AVR Service
                if (avrServiceWasStopped)
                    ServiceHelper.AvrServiceChangeState(true);

                // ���������� ������ � ���� �����
                string tmp;
                var backupFilename = ArchiveBackups(tempDir, fZip, out tmp);
                InvokeMoveNextProgressBar();

                //������� ������
                AddReportString(ResourceHelper.Get("DeleteGarbageBackups"));
                ClearBackup(
                  tempDir,
                  ProductHelper.IsEIDSSProgram(settings.ProgramID) ? UpdHelper.EIDSSPath : UpdHelper.NSPath,
                  Settings.BackupPath, out errorMessage);
                AddReportString(
                  errorMessage.Length == 0
                  ? ResourceHelper.Get("GarbageBackupsSuccessfullyDeleted")
                  : string.Format(CultureInfo.InvariantCulture, ResourceHelper.Get("GarbageBackupsFailedDeleted"), errorMessage));
                File.Move(tmp, backupFilename);

                #region �������� ���������� ������ ������

                //��� ������� �������� ����� ���� ������ ���� �����, ��������� �������
                var dirs = Directory.GetDirectories(Settings.BackupPath,
                                                    string.Format("update_{0}_*", Settings.ProgramId));
                foreach (var dir in dirs)
                {
                    if (dir.ToUpperInvariant().Equals(tempDir.ToUpperInvariant()))
                        continue;
                    FileHelper.DeleteDir(dir, true);
                }

                #endregion
                if (File.Exists(UpdHelper.ClientAgentPath) && !FileHelper.IsProcessExists(UpdHelper.ClientAgentProcessName))
                {
                    CreateProcessAsUserWrapper.LaunchChildProcess(UpdHelper.ClientAgentPath);
                }

                //������ ���������
                AddReportString(ResourceHelper.Get("WorkComplete"));
            }
            catch (Exception exc)
            {
                errorMessage = UpdHelper.GetErrorMessage(exc);
                AddReportString(errorMessage);
                SecurityLog.WriteToEventLogWindows(EventLogEntryType.Error, errorMessage);
            }

            conn.Close();
        }

        protected static SqlConnection OpenDbMainConnection()
        {
            try
            {
                var conn = DatabaseHelper.GetConnection(UpdateDatabases.DbMain);
                if ((conn != null) && (conn.State != ConnectionState.Open))
                {
                    conn.Open();
                }
                return conn;
            }
            catch (Exception e)
            {
                SecurityLog.WriteToEventLogWindows(EventLogEntryType.Error, e.Message);
                return null;
            }
        }

        protected FastZip ExtractUpdateFiles(string updateFileName, string workDir)
        {
            AddReportString(ResourceHelper.Get("ExtractingFiles"));

            if (!Directory.Exists(workDir))
            {
                Directory.CreateDirectory(workDir);
            }

            var fZip = UpdHelper.CreateZipper();
            fZip.ExtractZip(updateFileName, workDir, string.Empty);

            AddReportString(ResourceHelper.Get("FilesSuccessfullyExtracted"));
            return fZip;
        }

        protected string ArchiveBackups(string tempDir, FastZip fZip, out string tmp)
        {
            AddReportString(ResourceHelper.Get("FullBackup"));

            var filename = FileHelper.GetBackupFilename(Settings.VersionOfUpdate.ToString());
            var backupFilename = Path.Combine(tempDir, filename);
            FileHelper.DeleteFile(backupFilename);
            //�������� ����� �� ��������� �������, ����� ������ �������� ����� � ��������� ����� �������
            tmp = Path.Combine(UpdHelper.TempDirPath, filename);
            //����� ������������ ������ ���������������� ��������, ����������� � �������, ����� ��������, ������� �����������, � ����� ��
            //������� � ���� run.upd, ������� ����������� ��� �������������� ���� ������
            var files = FileHelper.GetFiles(tempDir, "*.zip", false);
            files.AddRange(FileHelper.GetFiles(tempDir, "*.config", false));
            files.AddRange(FileHelper.GetFiles(tempDir, "*.bak", false));
            files.AddRange(FileHelper.GetFiles(tempDir, "*.bat", false));
            files.AddRange(FileHelper.GetFiles(tempDir, "*.cmd", false));
            files.AddRange(FileHelper.GetFiles(tempDir, "*.vbs", false));
            files.AddRange(FileHelper.GetFiles(tempDir, "run.upd", false));
            var filter = FileHelper.GetFilesFilter(files);
            //��� �������� ����� �� �������� ������������
            try
            {
                fZip.CreateZip(tmp, tempDir, true, filter, string.Empty);
            }
            catch (Exception exc)
            {
                SecurityLog.WriteToEventLogWindows(
                  EventLogEntryType.Warning,
                  ResourceHelper.Get("BackupFailed"),
                  exc.Message);
            }
            return backupFilename;
        }

        /// <summary>
        /// ����� � �� Admin � ������ ����������
        /// </summary>
        /// <param name="workingDir"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected virtual long RegisterUpdateStart(string workingDir, UpdateSettings settings)
        {
            var currentVersion = VersionFactory.EmptyVersion;
            if (ProductHelper.IsDBUpdate(settings.ProgramID))
            {
                currentVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbMain);
            }
            else if (ProductHelper.IsDBaUpdate(settings.ProgramID))
            {
                currentVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbArchive);
            }
            else if (ProductHelper.IsAvrServiceDbProgram(settings.ProgramID))
            {
                currentVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.AVRServiceDb);
            }
            else if (ProductHelper.IsAvrServiceProgram(settings.ProgramID))
            {
                currentVersion = FileHelper.GetAvrServiceVersion(workingDir);
            }
            else if (ProductHelper.IsEIDSSProgram(settings.ProgramID))
            {
                currentVersion = FileHelper.GetEIDSSVersion(workingDir);
            }
            else if (ProductHelper.IsNSProgram(settings.ProgramID))
            {
                currentVersion = FileHelper.GetNSVersion(workingDir);
            }
            else if (ProductHelper.IsAUMUpdate(settings.ProgramID))
            {
                currentVersion = FileHelper.GetAUMVersion(workingDir);
            }
            var idAdminRecord = AdminHelper.WriteUpdateStartedToAdminDB(settings.ProgramID, currentVersion, settings.Version);
            AddReportStringEx(string.Format("Write into Admin DB. idAdminRecord = {0}", idAdminRecord));
            return idAdminRecord;
        }

        protected virtual void DeleteUpdateBlock(UpdateMessenger updateMessenger)
        {
            updateMessenger.DeleteUpdateBlock();
        }

        protected virtual void DeleteRunningApps(UpdateMessenger updateMessenger, string clientId, UpdateSettings settings)
        {
            updateMessenger.DeleteRunningApps(clientId, settings.ProgramID);
        }

        protected virtual void CreateUpdateBlock(string clientId, UpdateSettings settings, UpdateMessenger updateMessenger)
        {
            //�������� ������� EIDSSUpdate.Updateblock, ����� �� ����������� ����� ������� � ������������ ������� ��������� ����������
            AddReportStringEx(string.Format(
              CultureInfo.InvariantCulture,
              "Create update block for Client ID = {0}, Program ID={1}", clientId,
              settings.ProgramID));
            updateMessenger.CreateUpdateBlock(
              clientId,
              settings.ProgramID,
              ProductHelper.IsDBUpdate(settings.ProgramID) || ProductHelper.IsDBaUpdate(settings.ProgramID));
        }


        ///// <summary>
        ///// ���������� ���������������� ������ ��������� ������� ����������
        ///// </summary>
        ///// <param name="settings"></param>
        //internal static void ExecuteRegime(ConfigSettings settings)
        //{

        //}

        public static void SystemOperations(string appPath)
        {
            //����� aum.log
            BackupHelper.BackupAumLog(appPath);
        }

        /// <summary>
        /// ����������� �� ����� ����������� ������
        /// </summary>
        /// <param name="fileFrom"></param>
        /// <param name="fileTo"></param>
        protected void OnFileHelperOnCopyFiles(string fileFrom, string fileTo)
        {
            AddReportString(string.Format(ResourceHelper.Get("CopyFiles"), fileFrom, fileTo));
        }

        /// <summary>
        /// �������� �������� �������
        /// </summary>
        /// <param name="tempDir"></param>
        /// <param name="rootPath"></param>
        /// <param name="backupDir"></param>
        /// <param name="errStr"></param>
        public static void ClearBackup(string tempDir, string rootPath, string backupDir, out string errStr)
        {
            errStr = string.Empty;
            try
            {
                FileHelper.DeleteDir(tempDir, false);
                //������� ��� ������������� ������� � �������� ����������
                FileHelper.DeleteFiles(FileHelper.GetFiles(rootPath, "*.configold", true));
                FileHelper.DeleteFiles(FileHelper.GetFiles(backupDir, "*.zip", false));
            }
            catch (Exception exc)
            {
                errStr = exc.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static string GetProductFilter(string productId, Version version)
        {
            return string.Format("_{0}_{1}", productId, version);
        }

        private static bool IsDbUpdateCorrect(
          Version dbVersion,
          Version currentDbMainVersion,
          Version productVersion)
        {
            return dbVersion != VersionFactory.EmptyVersion
              && UpdHelper.CheckDbUpdateExecution(dbVersion, currentDbMainVersion, ProductHelper.DbProgramId, productVersion);
        }

        private static bool IsDbaUpdateCorrect(
          Version dbaVersion,
          Version currentDbArchiveVersion,
          Version productVersion)
        {
            return dbaVersion != VersionFactory.EmptyVersion
              && UpdHelper.CheckDbUpdateExecution(dbaVersion, currentDbArchiveVersion, ProductHelper.DbaProgramId, productVersion);
        }

        private static bool IsAvrDbUpdateCorrect(
          Version avrDbVersion,
          Version currentAvrDbVersion,
          Version productVersion)
        {
            return avrDbVersion != VersionFactory.EmptyVersion
              && UpdHelper.CheckDbUpdateExecution(avrDbVersion, currentAvrDbVersion, ProductHelper.AvrServiceDbProgramId, productVersion);
        }

        /// <summary>
        /// ��������� ������ ���������� �� ������ ������
        /// </summary>
        /// <param name="settings"></param>
        public static void RunUpdateForAllProducts(ConfigSettings settings)
        {
            AUMLog.WriteInLog("==========================");

            FileHelper.DeleteWrongLogFiles(settings.LogLocal);

            FileHelper.DeleteDir(UpdHelper.TempDirPath, false);

            var updateArchives = FileHelper.CollectUpdates(settings.UpdateLocal);
            if (!updateArchives.Any())
            {
                return;
            }

            var versionList = ExtractUpdates(updateArchives, settings.UpdateLocal);
            var versionLowest = versionList.Min();

            var productArchivesUnsorted = RemoveFullUpdatesFromList(Directory.GetFiles(settings.UpdateLocal, "update_*_*.zip")
              .Where(f => FileHelper.GetVersionFromArchive(f) >= versionLowest).ToList());
            if (!productArchivesUnsorted.Any())
            {
                return;
            }

            // this block of variables is really required outside foreach
            var hasDbUpdate = false;
            var hasDbaUpdate = false;
            var hasAvrDbUpdate = false;
            var dbVersion = VersionFactory.EmptyVersion;
            var dbaVersion = VersionFactory.EmptyVersion;
            var avrDbVersion = VersionFactory.EmptyVersion;
            foreach (var updateFile in SortAndLogUpdatesByExecutionOrder(versionList, productArchivesUnsorted))
            {
                AUMLog.WriteInLog("===== run {0} =====", updateFile);

                var programId = FileHelper.ExtractProgramID(updateFile);
                var archiveVersion = FileHelper.GetVersionFromArchive(updateFile);

                AUMLog.WriteInLog("programId='{0}'; version='{1}'", programId, archiveVersion);
                AUMLog.WriteInLog("Try to get Version from run.upd for this update");

                var productVersion = UpdHelper.GetUpdateVersion(updateFile);
                if (productVersion == VersionFactory.EmptyVersion)
                {
                    productVersion = archiveVersion;
                    AUMLog.WriteInLog("Getting Version failed! File 'run.upd' not found. Set Version='{0}'", productVersion);
                }
                else
                {
                    AUMLog.WriteInLog("Version='{0}'", productVersion);
                }

                var eidssUpdateVersion = VersionFactory.EmptyVersion;

                // The next if block checks if previous updates in this patch succeeded - if not we can't go on
                // nothing but checks, no regcords in db, nothing
                if (!MachineLevel.IsWebWks(settings.Level) && !MachineLevel.IsClientWks(settings.Level))
                {
                    if (ProductHelper.IsDBUpdate(programId)
                        || ProductHelper.IsDBaUpdate(programId)
                        || ProductHelper.IsAvrServiceDbProgram(programId)
                        || ProductHelper.IsAvrServiceProgram(programId)
                        || ProductHelper.IsEIDSSProgram(programId)
                        || ProductHelper.IsNSProgram(programId))
                    {
                        var currentDbMainVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbMain);
                        var currentDbArchiveVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.DbArchive);
                        var currentAvrDbVersion = DatabaseHelper.GetCurrentDBVersion(UpdateDatabases.AVRServiceDb);


                        if (ProductHelper.IsDBUpdate(programId))
                        {
                            hasDbUpdate = true;
                            dbVersion = productVersion;
                        }
                        else if (ProductHelper.IsDBaUpdate(programId))
                        {
                            if (hasDbUpdate && !IsDbUpdateCorrect(dbVersion, currentDbMainVersion, productVersion))
                            {
                                continue;
                            }
                            if (!DbExist(UpdateDatabases.DbArchive))
                            {
                                MakeFakeLog(
                                  "This machine hasn't Archive database. This update is ignored.",
                                  productVersion,
                                  programId,
                                  settings.Level);
                                continue;
                            }

                            hasDbaUpdate = true;
                            dbaVersion = productVersion;
                        }
                        else if (ProductHelper.IsAvrServiceDbProgram(programId))
                        {
                            if (hasDbUpdate && !IsDbUpdateCorrect(dbVersion, currentDbMainVersion, productVersion)
                              || hasDbaUpdate && !IsDbaUpdateCorrect(dbaVersion, currentDbArchiveVersion, productVersion))
                            {
                                continue;
                            }

                            if (!DbExist(UpdateDatabases.DbArchive))
                            {
                                MakeFakeLog(
                                  "This machine hasn't AVR Service database. This update is ignored.",
                                  productVersion,
                                  programId,
                                  settings.Level);
                                continue;
                            }

                            hasAvrDbUpdate = true;
                            avrDbVersion = productVersion;
                        }
                        else if (ProductHelper.IsAvrServiceProgram(programId))
                        {
                            if (hasDbUpdate && !IsDbUpdateCorrect(dbVersion, currentDbMainVersion, productVersion)
                                || hasDbaUpdate && !IsDbaUpdateCorrect(dbaVersion, currentDbArchiveVersion, productVersion)
                                || hasAvrDbUpdate && !IsAvrDbUpdateCorrect(avrDbVersion, currentAvrDbVersion, productVersion))
                            {
                                continue;
                            }
                        }
                        else if (ProductHelper.IsEIDSSProgram(programId))
                        {
                            if (hasDbUpdate && !IsDbUpdateCorrect(dbVersion, currentDbMainVersion, productVersion)
                                || hasDbaUpdate && !IsDbaUpdateCorrect(dbaVersion, currentDbArchiveVersion, productVersion))
                            {
                                continue;
                            }
                        }
                        else if (ProductHelper.IsNSProgram(programId))
                        {
                            if (hasDbUpdate && !IsDbUpdateCorrect(dbVersion, currentDbMainVersion, productVersion)
                                || hasDbaUpdate && !IsDbaUpdateCorrect(dbaVersion, currentDbArchiveVersion, productVersion))
                            {
                                continue;
                            }
                        }
                    }

                    //���� ���� ���������� �� ��� ���, �� ������� ������ ������ EIDSS �� ���� �� ������ ����������
                    if (ProductHelper.IsDBUpdate(programId) || ProductHelper.IsDBaUpdate(programId))
                    {
                        var eidssUpdateFile = updateFile.Replace(
                          string.Format(CultureInfo.InvariantCulture, "_{0}_", programId),
                          string.Format(CultureInfo.InvariantCulture, "_{0}_", ProductHelper.EidssProgramId));
                        if (File.Exists(eidssUpdateFile))
                        {
                            eidssUpdateVersion = UpdHelper.GetUpdateVersion(eidssUpdateFile);
                        }
                    }
                }

                var fn = Path.GetFileNameWithoutExtension(updateFile);
                var args = string.Format(CultureInfo.InvariantCulture, "/f{0} ", fn);
                if (eidssUpdateVersion != VersionFactory.EmptyVersion)
                {
                    args += string.Format(CultureInfo.InvariantCulture, "/e{0}", eidssUpdateVersion);
                }

                var regimeDescription = UpdHelper.GetRegimeCaption(programId);

                SecurityLog.WriteToEventLogWindows(
                  EventLogEntryType.Information,
                  "Task: {0} (Version {1}); Started: {2}",
                  regimeDescription,
                  archiveVersion,
                  DateTime.Now);

                var errorMessage = UpdHelper.RunProcess(
                  Path.Combine(UpdHelper.AUMPath, UpdHelper.AUMFileName),
                  args,
                  true,
                  string.Empty,
                  string.Empty);

                SecurityLog.WriteToEventLogWindows(
                  EventLogEntryType.Information,
                  "Task: {0} Version {1}); Finished: {2}; Error (exec process only): {3}",
                  regimeDescription,
                  archiveVersion,
                  DateTime.Now,
                  errorMessage.Length > 0 ? errorMessage : "none");

                //����� �� ���� ��������� ������� � Windows Log, ������� ��������� � ���� � �� �� �������
                Thread.Sleep(TimeSpan.FromSeconds(1));

                FileHelper.DeleteDir(UpdHelper.TempDirPath, false);
            }
        }

        private static void MakeFakeLog(string message, Version productVersion, string programId, MachineLevel.Level level)
        {
            AUMLog.WriteInLog(message);

            //������ �������� �������� ����
            var logfilename = FileHelper.WriteLogFile(
              UpdHelper.AUMPath,
              true,
              message,
              productVersion.ToString(),
              programId,
              level,
              string.Empty);

            AUMLog.WriteInLog("Empty log file '{0}' was created.", logfilename);
        }

        private static bool DbExist(UpdateDatabases database)
        {
            var conn = DatabaseHelper.GetConnection(database);
            if (conn == null)
            {
                return false;
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return true;
        }


        private static IEnumerable<string> SortAndLogUpdatesByExecutionOrder(
          IEnumerable<Version> versionList,
          List<string> productArchivesUnsorted)
        {
            var productArchives = SortUpdatesByExecutionOrder(versionList, productArchivesUnsorted);
            AUMLog.WriteInLog("Patches in the execution order:");
            for (var index = 0; index < productArchives.Count; ++index)
            {
                AUMLog.WriteInLog("#{0} {1}", index, Path.GetFileName(productArchives[index]));
            }
            return productArchives;
        }

        private static List<string> SortUpdatesByExecutionOrder(IEnumerable<Version> versionList, List<string> productArchivesUnsorted)
        {
            var productArchives = new List<string>();
            foreach (var version in versionList)
            {
                var file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.AumProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.CustomTasksProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.DbProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.DbaProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.AvrServiceDbProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.AvrServiceProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.EidssProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
                file = productArchivesUnsorted.FirstOrDefault(f => f.Contains(GetProductFilter(ProductHelper.NsProgramId, version)));
                if (file != null)
                {
                    productArchives.Add(file);
                }
            }
            return productArchives;
        }

        private static List<string> RemoveFullUpdatesFromList(List<string> archives)
        {
            for (var i = archives.Count - 1; i >= 0; i--)
            {
                var filename = Path.GetFileNameWithoutExtension(archives[i]);
                if (filename != null && (filename.Contains("small") || filename.Contains("total")))
                {
                    archives.RemoveAt(i);
                }
            }
            return archives;
        }

        private static List<Version> ExtractUpdates(IEnumerable<string> updateArchives, string dirUpdatePath)
        {
            var versionList = new List<Version>();
            foreach (var updateArchive in updateArchives)
            {
                UpdHelper.ExtractDirFromUpdate(updateArchive, dirUpdatePath, string.Empty);
                versionList.Add(FileHelper.GetVersionFromArchive(updateArchive));
            }
            versionList.Sort();
            return versionList;
        }


        private bool RunApplicationUpdate(string updateFileName, string tempDir, string workDir, out string errorMessage)
        {
            return RunEidssUpdate(updateFileName, tempDir, workDir, out errorMessage);
        }

        protected bool RunEidssUpdate(string updateFileName, string tempDir, string workDir, out string errorMessage)
        {
            errorMessage = string.Empty;
            Mutex mutex = null;
            try
            {
                //���������� mutex, ����� ����� �� ��� ��������� ����������� ����������
                bool mutexCreated;
                mutex = MutexHelper.CreateMutex(MutexType.EidssUpdate, out mutexCreated);
                //������� �������, � ������� ��������� ����������. ��������� ��������� ��������.
                AddReportString(ResourceHelper.Get("BackupProgramDirectories"));

                var backupFileName = BackupHelper.BackupRootDir(
                  WorkingDir,
                  tempDir,
                  string.Format(CultureInfo.InvariantCulture, "{0}.zip", Path.GetFileNameWithoutExtension(updateFileName) ?? string.Empty));

                InvokeMoveNextProgressBar();
                AddReportString(ResourceHelper.Get("BackupProgramDirectoriesSuccessfullyComplete"));

                try
                {
                    #region �������� ��������

                    AddReportString(ResourceHelper.Get("CopyFiles2"));
                    FileHelper.OnCopyFiles += OnFileHelperOnCopyFiles;
                    var errStr = FileHelper.CopyDirs(workDir, WorkingDir);
                    if (errStr.Length > 0)
                    {
                        throw new Exception(errStr);
                    }
                    //������� ���� run.upd, ���� ������� ����� � ������� (�� ������������ ��������� ����)
                    FileHelper.DeleteRunUpd(WorkingDir);

                    InvokeMoveNextProgressBar();
                    AddReportString(ResourceHelper.Get("CopyFilesSuccessfullyComplete"));

                    #endregion

                    InvokeMoveNextProgressBar();

                    return true;
                }
                catch (Exception exc)
                {
                    errorMessage = UpdHelper.GetErrorMessage(exc);

                    //������������� ������� �������� �������
                    if (File.Exists(backupFileName))
                    {
                        FileHelper.RestoreRootDir(WorkingDir, backupFileName);
                    }
                }
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                }
                FileHelper.OnCopyFiles -= OnFileHelperOnCopyFiles;
            }

            return false;
        }

        protected bool RunXPatch(UpdateSettings settings, string workDir, out string errorMessage)
        {
            AddReportString(ResourceHelper.Get("ExecuteFilesStarted"));

            errorMessage = string.Empty;

            if (!File.Exists(UpdHelper.XPatchLauncher))
            {
                AddReportString(string.Format(CultureInfo.InvariantCulture, ResourceHelper.Get("FileNotFound"), UpdHelper.XPatchLauncher));
                return false;
            }

            AddReportString(string.Format(CultureInfo.InvariantCulture, ResourceHelper.Get("ExecuteFile"), UpdHelper.XPatchLauncher));

            try
            {
                errorMessage = UpdHelper.RunProcess(UpdHelper.XPatchLauncher, workDir, true, string.Empty, string.Empty);
            }
            catch (Exception exc)
            {
                errorMessage += exc.Message;
            }

            if (errorMessage.Length > 0)
            {
                AddReportStringEx(string.Format(CultureInfo.InvariantCulture, "error = {0}", errorMessage));
            }

            //����, ������������ � XPatchesLauncher.exe, ���������� � ���� update_customtasks_COMPNAME_customtasks.log
            //��������������� ����� ���������� �� ������ ����� �������
            var updatesLogPath = Path.Combine(FileHelper.GetExecutingPath(), FileHelper.AUMLogDirName);
            updatesLogPath = Path.Combine(updatesLogPath, Environment.MachineName);
            var logFilename = Path.Combine(updatesLogPath, FileHelper.GetLogFileName("customtasks", "customtasks"));
            Report.AppendLine(FileHelper.GetTextFromLogFile(logFilename));
            var result = FileHelper.GetResultFromLogFile(logFilename) == UpdateStatus.Success;

            RegisterXPatch(result, settings.Version);
            return result;
        }

        /// Writes update version to regisrty - mark for future that update was applied
        protected void RegisterXPatch(bool success, Version version)
        {
            if (!success)
            {
                return;
            }
            AddReportString(ResourceHelper.Get("ExecuteFilesFinished"));
            RegistryHelper.WriteExistExecuteRegistryKey(version);
        }
    }
}