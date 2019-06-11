using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.SqlServer.Management.Smo;

namespace AUM.Core.Helper
{
    public static class BackupHelper
    {
        static BackupHelper ()
        {
            //TODO ��������� �� ���?
            if (ZipConstants.DefaultCodePage == 1)
                ZipConstants.DefaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;
        }

        /// <summary>
        /// ���������� ����� ���� ������
        /// </summary>
        /// <param name="server"></param>
        /// <param name="filename">������ ���� � ����� ������</param>
        /// <param name="mainDatabaseSMO"></param>
        public static void BackupDatabase(Server server, Database mainDatabaseSMO, string filename)
        {
            var backup = new Backup {Action = BackupActionType.Database};
            var now = DateTime.Now;
            backup.BackupSetDescription = string.Format("Full backup of '{0}'; created by AUM at '{1}'", mainDatabaseSMO.Name, now);
            backup.BackupSetName = string.Format("AUM-{0}-{1}", mainDatabaseSMO.Name, now);
            backup.Database = mainDatabaseSMO.Name;
            backup.Devices.AddDevice(filename, DeviceType.File);
            backup.Incremental = false;
            backup.SqlBackup(server);
            //���������� �����
            var fZip = UpdHelper.CreateZipper();
            var dirName = Path.GetDirectoryName(filename);
            if (dirName != null)
            {
                var zipFilename = Path.Combine(dirName, string.Format("{0}.zip", Path.GetFileNameWithoutExtension(filename)));
                FileHelper.DeleteFile(zipFilename);
                fZip.CreateZip(zipFilename, dirName, false, UpdHelper.BakFilename, string.Empty);
                FileHelper.DeleteFile(filename);
            }
        }

        /// <summary>
        /// ��������� ��������� ���������
        /// </summary>
        /// <param name="copyDirsList">�������� ���������, ���������� ���������</param>
        /// <param name="rootFolder">�������, ��� ����� eidss.main.exe</param>
        /// <param name="tempDir">��������� �������, ���� ���� ��������� ���������������� ��������</param>
        public static void BackupDirs(List<CopyDirData> copyDirsList, string rootFolder, string tempDir)
        {
            foreach (var dir in copyDirsList)
            {
                var destinationDir = FileHelper.GetDestinationPath(dir.DestinationDir);

                //�������� �������� �������� ������ �� ����������� Updates � Backups, ������� ����� ������������ ��� ������
                var filterDirs = new List<string>();
                var directoryInfo = new DirectoryInfo(rootFolder);
                foreach (var di in directoryInfo.GetDirectories())
                {
                    if ((!di.Name.Equals(FileHelper.AUMUpdateDirName) && !di.Name.Equals(FileHelper.BackupsDirName)))
                    {
                        filterDirs.Add(di.Name);
                    }
                }

                var filterStr = FileHelper.GetFilterString(filterDirs);
                var fZip = UpdHelper.CreateZipper();
                fZip.CreateZip(string.Format("{0}.zip", Path.Combine(tempDir, dir.SourceDir))
                                , destinationDir
                                , true, filterStr);
            }
        }

        /// <summary>
        /// �������� ��������� ����� ������-������
        /// </summary>
        /// <param name="configFilesList">�������� �������� ��� ��������</param>
        /// <param name="tempDir">��������� �������</param>
        public static void BackupConfigs(List<ChangeConfigData> configFilesList, string tempDir)
        {
            foreach (var cfg in configFilesList)
            {
                var destination = FileHelper.GetDestinationPath(cfg.FileName);
                if (!File.Exists(destination)) continue;
                //���������������
                File.Copy(destination, Path.ChangeExtension(destination, "configold"), true);
                //���������� ����� ������� ������ ����� � �������� �������
                File.Copy(destination, Path.Combine(tempDir, cfg.FileName), true);
            }
        }

        /// <summary>
        /// �������� �������� ������� � ����� � ������ ������������� ���������
        /// </summary>
        /// <param name="rootFolder">����� ������� ����� �����������</param>
        /// <param name="tempDir">���� ����� �������� ����� � ���� ���������</param>
        /// <param name="filename">��� ����� ������</param>
        public static string BackupRootDir(string rootFolder, string tempDir, string filename)
        {
            var destinationDir = FileHelper.GetDestinationPath(rootFolder);

            //������ �� ���������
            var filterDirs = new List<string>();
            var directoryInfo = new DirectoryInfo(rootFolder);
            foreach (var di in directoryInfo.GetDirectories())
            {
                if ((!di.Name.Equals(FileHelper.AUMUpdateDirName) && !di.Name.Equals(FileHelper.BackupsDirName) && !di.Name.Equals(FileHelper.AUMLogDirName)))
                {
                    filterDirs.Add(di.Name);
                }
            }

            var backupFileName = Path.Combine(tempDir, filename);
            FileHelper.DeleteFile(backupFileName);
            var fZip = UpdHelper.CreateZipper();
            fZip.CreateZip(backupFileName
                            , destinationDir
                            , true, null, FileHelper.GetFilterString(filterDirs));
            return backupFileName;
        }

        /// <summary>
        /// ���������� ��������� �������� �� programmability �� � ��������� �������
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="appDir">�������, ������ ����������� AUM</param>
        /// <param name="outputDir">�������, ���� ��������� ����� ��������</param>
        public static void BackupDBIntoScripts(Server server, Database database, string appDir, string outputDir)
        {
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            string loginSqlAdmin;
            string passwordSqlAdmin;
            UpdHelper.GetSqlAdminSettings(string.Empty, string.Empty, out loginSqlAdmin, out passwordSqlAdmin);

            //��������� ��������������� ��� ������� ��� �������� � ��� ������ �������));
            for (var i = 1; i <= 3; i++)
            {
                var args = string.Format("-S {0} -d {1} -U {2} -P {3} -i \"{4}\" -o \"{5}\" -y 0"
                                            , server.Name
                                            , database.Name
                                            , loginSqlAdmin
                                            , passwordSqlAdmin
                                            , Path.Combine(appDir, string.Format("InputScript_{0}.sql", i))
                                            , Path.Combine(outputDir, string.Format("OutputScript_{0}.sql", i))
                    );

                //���� ���� ���� �� �������� �� ����������, �� �������
              if (UpdHelper.RunProcess("sqlcmd", args, true, false).Length > 0)
              {
                break;
              }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="scriptsBackupPath"></param>
        public static string RestoreDB(Database database, string scriptsBackupPath)
        {
            var errorMessage = string.Empty;

            if (!Directory.Exists(scriptsBackupPath)) return errorMessage;
            //��������� ������������ programmability �� ��������
            var scripts = Directory.GetFiles(scriptsBackupPath, "*.sql");
            try
            {
                foreach (var scriptFilename in scripts)
                {
                    using (var sr = (new FileInfo(Path.Combine(scriptsBackupPath, scriptFilename)).OpenText()))
                    {
                        database.ExecuteNonQuery(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception exc)
            {
                errorMessage = UpdHelper.GetErrorMessage(exc);
            }

            return errorMessage;
        }

        /// <summary>
        /// ���������� ����� �������� ������� bvupdater.config
        /// </summary>
        public static void BackupMainConfig(string appPath)
        {
            try
            {
                var filepath = Path.Combine(appPath, FileHelper.BVUpdaterConfigFileName);
                if (!File.Exists(filepath)) return;
                var files = Directory.GetFiles(appPath, "bvupdater*.config");
                var count = files.Length;
                var newFilename = Path.Combine(appPath, string.Format("bvupdater{0}.config", count));
                while (File.Exists(newFilename))
                {
                    count++;
                    newFilename = Path.Combine(appPath, string.Format("bvupdater{0}.config", count));
                }

                FileHelper.RemoveReadonlyAttribute(filepath);
                File.Copy(filepath, newFilename, true);
            }
            catch (Exception exc)
            {
              SecurityLog.WriteToEventLogWindows(EventLogEntryType.Warning, exc.Message);
            }
        }

        /// <summary>
        /// ����� ���������� ���� aum.log
        /// </summary>
        /// <param name="appPath"></param>
        public static void BackupAumLog(string appPath)
        {
            try
            {
                AUMLog.WriteInLog("=== backup aum.log===");
                var logFileName = Path.Combine(appPath, AUMLog.AUMLogFileName);
                AUMLog.WriteInLog("Try to find {0}", logFileName);
                if (!File.Exists(logFileName))
                {
                    AUMLog.WriteInLog("File is not found. Exit backup operation.");
                    return;
                }
                AUMLog.WriteInLog("Success");
                var backupDir = Path.Combine(appPath, "AUMlogBackup");
                if (!Directory.Exists(backupDir))
                {
                    AUMLog.WriteInLog("Try to create dir {0}", backupDir);
                    Directory.CreateDirectory(backupDir);
                    AUMLog.WriteInLog("Success");
                }
                //���� ������ ������ 1 ������ � ���� �������� �����, �� ���� ��� ����������, �������� �������
                var dateCreation = File.GetCreationTime(logFileName);
                var dtNow = DateTime.Now;
                if ((dtNow - dateCreation).TotalDays > 31)
                {
                    //������ ����� � ��������� �������
                    var fZip = UpdHelper.CreateZipper();
                    var zipFileName = string.Format("aumlog_{0}_{1}_{2}.zip", dtNow.Month, dtNow.Year, dtNow.Ticks);
                    var zipFullPath = Path.Combine(backupDir, zipFileName);
                    AUMLog.WriteInLog("Try to zip backup {0}", zipFullPath);
                    fZip.CreateZip(zipFullPath, appPath, false, "aum.log");
                    AUMLog.WriteInLog("Success");
                    //������� ���
                    AUMLog.WriteInLog("Try to delete current aum.log");
                    if (!FileHelper.DeleteFile(logFileName))
                    {
                        AUMLog.WriteInLog("Can't delete current aum.log");
                    }
                    else
                    {
                        //�������� ����-��������, ����� ���������� �� �������� create date
                        File.Create(logFileName).Dispose();
                        File.SetCreationTime(logFileName, DateTime.Now);
                    }
                    AUMLog.WriteInLog("Backup aum.log completed successful. Old log file into: {0}.", zipFullPath);
                }
                else
                {
                    AUMLog.WriteInLog("Backup is not required.");
                }
            }
            catch (Exception exc)
            {
                var errorString = string.Format("Can't backup aum.log. Error = {0}", exc.Message);
                SecurityLog.WriteToEventLogWindows(EventLogEntryType.Warning, errorString);
            }
        }
    }
}
