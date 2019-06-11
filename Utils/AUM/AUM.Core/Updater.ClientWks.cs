namespace AUM.Core
{
  using System;
  using System.Collections.Generic;
  using System.Data.SqlClient;
  using System.Diagnostics;
  using System.Globalization;
  using System.IO;
  using System.Threading;
  using db;
  using Enums;
  using Helper;


  public sealed class UpdaterClientWks : Updater
  {
    private const string NoDbUpdateRequiredMessage = "This is WKS (level {0}). Database Update is ignored.";
    private const string NoDbaUpdateRequiredMessage = "This is WKS (level {0}). Archive Database Update is ignored.";
    private const string NoAvrDbUpdateRequiredMessage = "This is WKS (level {0}). AVR Service Database Update is ignored.";
    private const string NoNsUpdateRequiredMessage = "This is WKS (level {0}). Notification Service Update is ignored.";
    private const string NoAvrServiceUpdateRequiredMessage = "This is WKS (level {0}). AVR Service Update is ignored.";

    public UpdaterClientWks(ConfigSettings settings)
      : base(settings)
    {
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
      if (ProductHelper.IsDBUpdate(productId))
      {
        #region �������� ������ ��

        SetUpdateState(
          string.Format(CultureInfo.InvariantCulture, NoDbUpdateRequiredMessage, Settings.Level),
          productId,
          Settings.VersionOfUpdate,
          Settings.Level);
          return false;

        #endregion
      }
      if (ProductHelper.IsDBaUpdate(productId))
      {
        #region �������� ������ �� (��������)

        SetUpdateState(
          string.Format(CultureInfo.InvariantCulture, NoDbaUpdateRequiredMessage, Settings.Level),
          productId,
          Settings.VersionOfUpdate,
          Settings.Level);
        return false;

        #endregion
      }
      if (ProductHelper.IsAvrServiceDbProgram(productId))
      {
        #region �������� ������ AVR Service ��

        SetUpdateState(
          string.Format(CultureInfo.InvariantCulture, NoAvrDbUpdateRequiredMessage, Settings.Level),
          productId,
          Settings.VersionOfUpdate,
          Settings.Level);
        return false;

        #endregion
      }
      if (ProductHelper.IsNSProgram(productId))
      {
        SetUpdateState(
          string.Format(CultureInfo.InvariantCulture, NoNsUpdateRequiredMessage, Settings.Level),
          productId,
          Settings.VersionOfUpdate,
          Settings.Level);
        return false;
      }
      if (ProductHelper.IsAvrServiceProgram(productId))
      {
        SetUpdateState(
          string.Format(CultureInfo.InvariantCulture, NoAvrServiceUpdateRequiredMessage, Settings.Level),
          productId,
          Settings.VersionOfUpdate,
          Settings.Level);
        return false;
      }
      if (ProductHelper.IsEIDSSProgram(productId))
      {
        var currentVersion = FileHelper.GetEIDSSVersion(WorkingDir);
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

    public override void RunUpdate(string updateFilename, Version eidssVersion)
    {
      if (!IsThisUpdateRequired(updateFilename, Settings.ProgramId))
      {
        return;
      }

      //����� ����� ��������� � �������� backups\Update<������>
      var tempDir = Path.Combine(Settings.BackupPath, Path.GetFileNameWithoutExtension(updateFilename) ?? string.Empty);
      if (!Directory.Exists(tempDir))
      {
        Directory.CreateDirectory(tempDir);
      }

      var conn = OpenDbMainConnection();
      if (null == conn)
      {
        return;
      }

      SecurityLog.WriteToEventLogDB(
        conn,
        Operations.Update,
        true,
        string.Empty,
        string.Format(ResourceHelper.Get("UpdateProcessStarted"), DateTime.Now));

      var updateMessenger = new UpdateMessenger(conn);

      AddReportString(string.Format(ResourceHelper.Get("UpdateProcessStarted"), DateTime.Now));

      InvokeSetProgressbarMaxValue(10);

      var workDir = Path.Combine(UpdHelper.TempDirPath, Path.GetFileNameWithoutExtension(updateFilename) ?? string.Empty);
      var fZip = ExtractUpdateFiles(updateFilename, workDir);

      InvokeMoveNextProgressBar();

      var updFileName = Path.Combine(workDir, FileHelper.FileNameUpdate);
      //������� ��������� �������
      var settings = UpdHelper.ReadUpdateFile(updFileName);

      //��� ���������� �� � AUM �� ����� clientID
      var clientId = UpdHelper.GetClientID(settings.ProgramID);

      CreateUpdateBlock(clientId, settings, updateMessenger);

      bool nsWasStopped;
      bool avrServiceWasStopped;
      StopProcessess(settings, conn, updateMessenger, clientId, out nsWasStopped, out avrServiceWasStopped);

      var success = false;
      var errorMessage = string.Empty; //���� ����� ��� ��������� �� �������
      var message = string.Empty;

      var idAdminRecord = RegisterUpdateStart(WorkingDir, settings);

      if (ProductHelper.IsDBUpdate(settings.ProgramID))
      {
        success = RunDbUpdate(settings, workDir, out errorMessage);
      }
      else if (ProductHelper.IsDBaUpdate(settings.ProgramID))
      {
        success = RunDbaUpdate(settings, workDir, out errorMessage);
      }
      else if (ProductHelper.IsNSProgram(settings.ProgramID))
      {
        success = RunNsUpdate(settings, workDir, out errorMessage);
      }
      else if (ProductHelper.IsAvrServiceDbProgram(settings.ProgramID))
      {
        success = RunAvrServiceDbUpdate(settings, workDir, out errorMessage);
      }
      else if (ProductHelper.IsAvrServiceProgram(settings.ProgramID))
      {
        success = RunAvrServiceUpdate(settings, workDir, out errorMessage);
      }
      else if (ProductHelper.IsEIDSSProgram(settings.ProgramID))
      {
        success = RunEidssUpdate(updateFilename, tempDir, workDir, out errorMessage);
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

      message += string.Format(
        CultureInfo.InvariantCulture,
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
        FileHelper.WriteLogFile(
          FileHelper.GetExecutingPath(),
          success,
          fullReportString,
          settings.Version.ToString(),
          settings.ProgramID,
          Settings.Level,
          message);

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
        {
          ServiceHelper.NotificationServiceChangeState(true);
        }

        //���������� AVR Service
        if (avrServiceWasStopped)
        {
          ServiceHelper.AvrServiceChangeState(true);
        }

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
          {
            continue;
          }
          FileHelper.DeleteDir(dir, true);
        }

        #endregion

        //������� ������������� Client Agent
        //�� ����� � �������� EIDSS
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

    // ��������� ��������� �� ������ ������
    private void StopProcessess(
      UpdateSettings settings,
      SqlConnection conn,
      UpdateMessenger updateMessenger,
      string clientId,
      out bool nsWasStopped,
      out bool avrServiceWasStopped)
    {
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
          AddReportStringEx("Try to stop '{0}'", UpdHelper.EIDSSFileName);

          AddReportStringEx("Wait for '{0}' will close", UpdHelper.EIDSSFileName);
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
      nsWasStopped = false;
      if (ProductHelper.IsNSProgram(settings.ProgramID)
          || ProductHelper.IsDBUpdate(settings.ProgramID)
          || ProductHelper.IsDBaUpdate(settings.ProgramID))
      {
        ServiceHelper.NotificationServiceChangeState(false);
        nsWasStopped = true;
      }

      //���� AVR Service �����������, �� �������� ���
      avrServiceWasStopped = false;
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
    }

    protected bool RunDbUpdate(UpdateSettings settings, string workDir, out string errorMessage)
    {
      errorMessage = string.Empty;
      AddReportString(NoDbUpdateRequiredMessage, Settings.Level);
      return true;
    }

    protected bool RunDbaUpdate(UpdateSettings settings, string workDir, out string errorMessage)
    {
      errorMessage = string.Empty;
      AddReportString(NoDbaUpdateRequiredMessage, Settings.Level);
      return true;
    }

    protected bool RunNsUpdate(UpdateSettings settings, string workDir, out string errorMessage)
    {
      errorMessage = string.Empty;
      AddReportString(NoNsUpdateRequiredMessage, Settings.Level);
      return true;
    }

    protected bool RunAvrServiceDbUpdate(UpdateSettings settings, string workDir, out string errorMessage)
    {
      errorMessage = string.Empty;
      AddReportString(NoAvrDbUpdateRequiredMessage, Settings.Level);
      return true;
    }

    protected bool RunAvrServiceUpdate(UpdateSettings settings, string workDir, out string errorMessage)
    {
      errorMessage = string.Empty;
      AddReportString(NoAvrServiceUpdateRequiredMessage, Settings.Level);
      return true;
    }
  }
}