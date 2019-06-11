namespace AUM.Core.Helper
{
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Data;
  using System.Data.SqlClient;
  using System.Diagnostics;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Management;
  using System.Net;
  using System.Net.NetworkInformation;
  using System.Security;
  using System.Text;
  using System.Threading;
  using System.Windows.Forms;
  using System.Xml;
  using Enums;
  using db;
  using ICSharpCode.SharpZipLib.Zip;


    /// <summary>
    /// 
    /// </summary>
    public class UpdatesFileNamesComparer : IComparer<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename1"></param>
        /// <param name="filename2"></param>
        /// <returns></returns>
        public int Compare(string filename1, string filename2)
        {
            var result = String.CompareOrdinal(filename1, filename2);
            try
            {
                var v1 = FileHelper.GetVersionFromArchive(filename1);
                var v2 = FileHelper.GetVersionFromArchive(filename2);
                result = v1.CompareTo(v2);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
            return result;
        }
    }

    /// <summary>
    /// ����� ��� ��������������� ������ *.upd
    /// </summary>
    public static class UpdHelper
    {
      public const string BakFilename = "databasebackup.bak";
      public const string AUMProsessName = "AUM60";
      public const string AUMFileName = "AUM60.exe";
      public const string EIDSSFileName = "eidss.main.exe";
      public const string ClientAgentProcessName = "EIDSS_ClientAgent.exe";
      public const string NSFileName = "EIDSS60_Ntfy.exe";
      public const string AUMServiceName = "AUM.Service60";
      public const string NSServiceName2 = "EIDSS.NotificationService_v6.0";
      public const string AUMAdministratorFileName = "AUM.Administrator60.exe";
      public const string AUMServiceFileName = "AUM.Service60.exe";
      public const string AVRServiceFileName = "eidss.avr.service.exe";
      private const string XPatchLauncherFileName = "XPatchesLauncher.exe";
      private const string ConnStrMask = "Persist Security Info=False;User ID={0};Password={1};Initial Catalog={2};Data Source={3};Asynchronous Processing=True;";

      public static readonly string EIDSSPath = RegistryHelper.ReadFromRegistry("epath");
      public static readonly string ClientAgentPath = Path.Combine(EIDSSPath, ClientAgentProcessName);
      public static readonly string NSPath = RegistryHelper.ReadFromRegistry("nspath");
      public static readonly string AUMPath = GetAumPath();
      public static readonly string AvrServicePath = RegistryHelper.ReadFromRegistry("avrspath");
      public static readonly string AVRServiceName = RegistryHelper.ReadFromRegistry("avrsname");
      private static readonly string s_windowsDirPath = GetWindowsPath();
      private static readonly string s_programFilesDirPath = GetProgramFilesPath();
      public static readonly string TempDirPath = Path.Combine(FileHelper.GetExecutingPath(), FileHelper.AUMTempDirName);
      public static readonly string NSServiceName = Path.GetFileNameWithoutExtension(NSFileName);
      public static readonly string XPatchLauncher = Path.Combine(GetAumPath(), XPatchLauncherFileName);

      /// <summary>
      /// �������������� ��������
      /// </summary>
      private static readonly Dictionary<string, CultureInfo> s_cultures = GetCultures();

      private static string GetWindowsPath()
      {
        var windir = Environment.GetEnvironmentVariable("windir");
        if (string.IsNullOrEmpty(windir))
        {
          windir = Environment.GetEnvironmentVariable("SystemRoot");
        }
        return windir;
      }

      private static string GetProgramFilesPath()
      {
        var programFilesDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        if (programFilesDirPath.Length == 0)
        {
          programFilesDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }
        return programFilesDirPath;
      }

      public static bool IsUpdateSuccess(string logFilename)
      {
        var result = false;

        try
        {
          if (File.Exists(logFilename))
          {
            var successString = XmlHelper.ReadAttributeFromXML(logFilename, "status", "success");
            result = successString.Equals("1");
          }
        }
        catch (Exception exc)
        {
          AUMLog.WriteInLog("Error while reading log file '{0}'. Error='{1}'.", logFilename, exc.Message);
          result = false;
        }
        return result;
      }

        public static void AppendInnerException(Exception exc, StringBuilder sb)
        {
            if (exc.InnerException != null)
            {
                sb.AppendFormat("{0};", exc.InnerException.Message);
                AppendInnerException(exc.InnerException, sb);
            }
        }

        public static string GetErrorMessage(Exception exc)
        {
            var sb = new StringBuilder();
            sb.Append(String.Format(ResourceHelper.Get("UpdateExecuteWithError"), exc.Message));
            AppendInnerException(exc, sb);
            return sb.ToString();
        }

      private static string GetAumPath()
      {
        var registeredPath = RegistryHelper.ReadFromRegistry("aumpath");
        return registeredPath.Length != 0 ? registeredPath : Application.StartupPath;
      }

        public static Dictionary<string, CultureInfo> GetCultures()
        {
            var  cultures = new Dictionary<string, CultureInfo>();
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (!cultures.ContainsKey(lang.Culture.TwoLetterISOLanguageName))
                    cultures.Add(lang.Culture.TwoLetterISOLanguageName, lang.Culture);
            }
            if (!cultures.ContainsKey("az-L")) cultures.Add("az-L", new CultureInfo("az-Latn-AZ"));
            return cultures;
        }


        public static SqlConnection GetSqlConnection(string server, string database, string user, string password, string connectionStringMask = "")
        {
            if (connectionStringMask.Length == 0) connectionStringMask = ConnStrMask;
            return new SqlConnection(String.Format(connectionStringMask, user, password, database, server));
        }

        /// <summary>
        /// ��������� ClienID ������������ ����������
        /// </summary>
        /// <returns></returns>
        /// <param name="programID"></param>
        public static string GetClientID(string programID)
        {
            //���� � ������-����� ������������ ����������
            var file = String.Empty;
            if (ProductHelper.IsEIDSSProgram(programID))
            {
                file = Path.Combine(EIDSSPath, EIDSSFileName);
            }
            else if (ProductHelper.IsNSProgram(programID))
            {
                file = Path.Combine(NSPath, NSFileName);
            }

            var clientID = String.Empty;

            if ((file.Length > 0) && File.Exists(file))
            {
                //������� �������� ClientID �� ������-����� ������������ ����������

                string exe = Path.Combine(EIDSSPath, EIDSSFileName);
                if (File.Exists(exe))
                {
                    var configuration = ConfigurationManager.OpenExeConfiguration(exe);
                    if (configuration.AppSettings.Settings["ClientID"] != null)
                    {
                        clientID = configuration.AppSettings.Settings["ClientID"].Value;
                    }
                }

                //XmlDocument document = new XmlDocument();
                //document.Load(file);
                //XmlNode node = document.DocumentElement;
                //XmlNode appSettingsNode = (node != null)
                //                              ? node.SelectSingleNode("/configuration/appSettings")
                //                              : null;
                //XmlNode neededNode = ((appSettingsNode != null) && (appSettingsNode.OwnerDocument != null))
                //                         ? appSettingsNode.OwnerDocument.SelectSingleNode(
                //                             "descendant::add[@key='ClientID']")
                //                         : null;
                //if ((neededNode != null) && (neededNode.Attributes != null))
                //    clientID = neededNode.Attributes["value"].InnerText;

            }
            else
            {
                //���� �� ����������, �� ��� MAC-����� �����
                clientID = GetMACAddress();
            }

            return clientID;
        }

        /// <summary>
        /// ��������� MAC-������ �����
        /// </summary>
        /// <returns></returns>
        private static string GetMACAddress()
        {
            string macAddress = String.Empty;

            try
            {
                var managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                var managementObjectCollection = managementClass.GetInstances();
                foreach (var managementBaseObject in managementObjectCollection)
                {
                    if (Convert.ToBoolean(managementBaseObject["IPEnabled"]))
                    {
                        macAddress = managementBaseObject["MacAddress"].ToString();
                        break;
                    }
                }
                macAddress = macAddress.Replace(":", "");
                // ReSharper disable EmptyGeneralCatchClause
            }
            catch
            {
            }
            // ReSharper restore EmptyGeneralCatchClause

            return macAddress;
        }

        /// <summary>
        /// ������������� �� ��������� ������� ��������� ���� � ���������� ���� � ����
        /// </summary>
        /// <param name="updatePackageFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ExtractFileFromUpdate(string updatePackageFileName, string fileName)
        {
            var fullPath = String.Empty;

            var fZip = CreateZipper();
            fZip.ExtractZip(updatePackageFileName, TempDirPath, fileName);
            var name = Path.Combine(TempDirPath, fileName);
            if (File.Exists(name)) fullPath = name;

            return fullPath;
        }

        /// <summary>
        /// ������������� �� ��������� ������� ��������� ������� � ���������� ���� � ����
        /// </summary>
        /// <param name="updatePackageFileName"></param>
        /// <param name="subDirName">���������� TEMP ��������, ���� ���� �����������</param>
        /// <returns></returns>
        public static string ExtractDirFromUpdate(string updatePackageFileName, string subDirName)
        {
            return ExtractDirFromUpdate(updatePackageFileName, TempDirPath, subDirName);
        }

        /// <summary>
        /// ������������� �� ��������� ������� ��������� ������� � ���������� ���� � ����
        /// </summary>
        /// <param name="updatePackageFileName"></param>
        /// <param name="subDirName">���������� TEMP ��������, ���� ���� �����������</param>
        /// <param name="destinationDir"></param>
        /// <returns></returns>
        public static string ExtractDirFromUpdate(string updatePackageFileName, string destinationDir, string subDirName)
        {
          var fZip = CreateZipper();
          var dir = Path.Combine(destinationDir, subDirName);
          if (!Directory.Exists(destinationDir))
          {
            Directory.CreateDirectory(destinationDir);
          }

          try
          {
            s_toUnsip = updatePackageFileName;
            fZip.ExtractZip(s_toUnsip, dir, FastZip.Overwrite.Prompt, ConfirmOverwriteDelegate, string.Empty, string.Empty, false);
          }
          finally
          {
            s_toUnsip = string.Empty;
          }
          return dir;
        }


      private static string s_toUnsip;

      private static bool ConfirmOverwriteDelegate(string fileName)
      {
        return File.GetCreationTime(s_toUnsip) >= File.GetLastWriteTime(fileName);
      }

      /// <summary>
        /// ������� ����� � ������������ �����
        /// </summary>
        /// <param name="dirPath">� ����� ���������� �������� ��������</param>
        public static void DeleteFilesWithErrors(string dirPath)
        {
            if (!Directory.Exists(dirPath)) return;
            bool hasCorruptedFile = false;
            try
            {
                string[] files = Directory.GetFiles(dirPath, "*.zip", SearchOption.TopDirectoryOnly);
                if (files.Length == 0) return;
                for (int i = files.Length - 1; i >= 0; i--)
                {
                    string file = files[i];
                    try
                    {
                        using (var zipFile = new ZipFile(file))
                        {
                            if (!zipFile.TestArchive(true))
                            {
                                FileHelper.DeleteFile(file);
                                hasCorruptedFile = true;
                            }
                        }
                    }
                    catch
                    {
                        FileHelper.DeleteFile(file);
                        hasCorruptedFile = true;
                    }
                }
            }
            catch (Exception e)
            {
                SecurityLog.WriteToEventLogWindows(EventLogEntryType.Warning, "Can't clear error files: {0}", e.Message);
                hasCorruptedFile = true;
            }
            if (hasCorruptedFile)
            {
              SecurityLog.WriteToEventLogWindows(EventLogEntryType.Warning, ResourceHelper.Get("PatchesCorrupted"));
            }
        }

        /// <summary>
        /// ���������� ������� � ������ ������� ���������� AUM'a
        /// </summary>
        /// <param name="updatePackageFileName">������ ���� � ������ �������</param>
        /// <returns></returns>
        public static bool IsUpdaterInclude(string updatePackageFileName)
        {
            var result = false;
           
            //���� ������� ����������� ����, �� ��������� ������ ������� �� ����
            var updFileName = ExtractFileFromUpdate(updatePackageFileName,FileHelper.FileNameUpdate);

            var exist = XmlHelper.ReadAttributeFromXML(updFileName, "update", "bvupdaterinclude");
            if (exist != null) result = exist.Equals("1");

            return result;
        }

        /// <summary>
        /// ������ ���� ��������� ������� � �������� ������� ��������
        /// </summary>
        /// <param name="updFileName">������ ���� � ����� run.upd</param>
        public static UpdateSettings ReadUpdateFile(string updFileName)
        {
          var updateSettings = new UpdateSettings
          {
            Version = VersionFactory.EmptyVersion,
            ProgramID = string.Empty,
            //CopyDirsList = new List<CopyDirData>(),
            SqlScriptsList = new List<RunScriptData>(),
            ConfigFilesList = new List<ChangeConfigData>(),
            DeleteFilesList = new List<string>()
          };

          using (var reader = new XmlTextReader(updFileName))
          {
            while (reader.Read())
            {
              if (reader.NodeType != XmlNodeType.Element)
              {
                continue;
              }
              if (reader.Name.Equals("update"))
              {
                var version = reader.GetAttribute("version");
                updateSettings.Version = string.IsNullOrEmpty(version) ? VersionFactory.EmptyVersion : VersionFactory.NewVersion(version);
                updateSettings.ProgramID = reader.GetAttribute("program");
                updateSettings.IsSmallUpdate = reader.GetAttribute("issmallupdate") == "1";
                updateSettings.CortegeVersion = reader.GetAttribute("cortegeversion");
              }
              else if (reader.Name.Equals("runscript"))
              {
                updateSettings.SqlScriptsList.Add(new RunScriptData { FileName = reader.GetAttribute("filename") });
              }
              else if (reader.Name.Equals("changeconfigvalue"))
              {
                var changeConfigData = new ChangeConfigData
                {
                  FileName = reader.GetAttribute("filename"),
                  Path = reader.GetAttribute("path"),
                  AttributeKey = reader.GetAttribute("attributekey"),
                  AttributeValue = reader.GetAttribute("attributevalue"),
                  Key = reader.GetAttribute("key"),
                  Value = reader.GetAttribute("value"),
                  ValueIsRegularExpression =
                    reader.GetAttribute("valueisregularexpression") == "1",
                  DeleteThisNode = reader.GetAttribute("deletethisnode") == "1"
                };
                updateSettings.ConfigFilesList.Add(changeConfigData);
              }
              else if (reader.Name.Equals("deletefiles"))
              {
                object fname = reader.GetAttribute("filename");
                if (fname != null)
                {
                  updateSettings.DeleteFilesList.Add(fname.ToString());
                }
              }
              else if (reader.Name.Equals("execute"))
              {
                var executeData = new ExecuteData
                {
                  FileName = reader.GetAttribute("filename"),
                  NeedWait = reader.GetAttribute("needwait") == "1"
                };
                updateSettings.ExecuteList.Add(executeData);
              }
            }
            }

            return updateSettings;
        }
        
        public static bool GetSqlAdminSettings(string loginKey, string passwordKey, out string loginDecrypted, out string passwordDecrypted)
        {
            if (loginKey.Length == 0) loginKey = "MaintSqlLogin";
            if (passwordKey.Length == 0) passwordKey = "MaintSqlPassword";
            loginDecrypted = passwordDecrypted = String.Empty;
            //��� ��������� ����� �������� �� ���������� �������
            var loginCrypted = RegistryHelper.ReadFromRegistry(loginKey);
            var passwordCrypted = RegistryHelper.ReadFromRegistry(passwordKey);
            if ((loginCrypted.Length > 0) && (passwordCrypted.Length > 0))
            {
                loginDecrypted = Cryptor.Decrypt(loginCrypted);
                passwordDecrypted = Cryptor.Decrypt(passwordCrypted, loginDecrypted);
            }
            return ((loginDecrypted.Length > 0) && (passwordDecrypted.Length > 0));
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFileName"></param>
        public static string ReadLogsPathBVUpdaterConfig(string configFileName)
        {
            var result = String.Empty;
            if (!File.Exists(configFileName)) return result;

            using (var reader = new XmlTextReader(configFileName))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element) continue;
                    if (reader.Name.Equals("logremote"))
                    {
                        result = reader.GetAttribute("path");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <param name="needWait"></param>
        /// <param name="loginAdmin"></param>
        /// <param name="passwordAdmin"></param>
        /// <returns></returns>
        public static string RunProcess(string filename, string arguments, bool needWait, string loginAdmin, string passwordAdmin)
        {
            return RunProcess(filename, arguments, needWait, false);
        }

        /// <summary>
        /// �������� ������ ������� ������
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCountry()
        {
            var result = String.Empty;
            try
            {
                var conn = DatabaseHelper.GetConnection(UpdateDatabases.DbMain);
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    var cmd = new SqlCommand("dbo.fnCurrentCountry", conn) {CommandType = CommandType.StoredProcedure};
                    cmd.Parameters.Add("@retval", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    var ret = cmd.Parameters["@retval"];
                    if (ret != null)
                    {
                        var idCountry = Int64.Parse(ret.Value.ToString());
                        switch (idCountry)
                        {
                            case 780000000:
                                result = "ge";
                                break;
                            case 170000000:
                                result = "az";
                                break;
                            case 1240000000:
                                result = "kz";
                                break;
                            case 80000000:
                                result = "am";
                                break;
                            case 2260000000:
                                result = "ua";
                                break;
                            case 2360000000:
                                result = "uz";
                                break;
                            case 1880000000:
                                result = "ru";
                                break;
                        }
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (Exception e)
            {
              SecurityLog.WriteToEventLogWindows(EventLogEntryType.Error, e.Message);
                return e.Message;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <param name="needWait"></param>
        /// <param name="loginAdmin"></param>
        /// <param name="passwordAdmin"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public static string RunProcess(string filename, string arguments, bool needWait, bool visible)
        {
            return RunProcess(filename, arguments, needWait, visible, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <param name="needWait"></param>
        /// <param name="loginAdmin"></param>
        /// <param name="passwordAdmin"></param>
        /// <param name="visible">������ �� ������� ���� �������</param>
        /// <param name="useShallExecute"></param>
        /// <param name="workingDir"></param>
        /// <returns>�����, ���� ������ �� ����, � ����� ������, ���� ��� ����</returns>
        public static string RunProcess(string filename, string arguments, bool needWait, bool visible, bool? useShallExecute, string workingDir)
        {
            if (!File.Exists(filename)) return String.Empty;

            var errorMessage = String.Empty;

            var process = new Process {StartInfo = {FileName = filename, Arguments = arguments}};
          process.StartInfo.WorkingDirectory = string.IsNullOrEmpty(workingDir)
            ? (Path.GetDirectoryName(filename) ?? string.Empty)
            : workingDir;

            
            process.StartInfo.WindowStyle = !visible ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal;
            if (useShallExecute.HasValue)
            {
                process.StartInfo.UseShellExecute = useShallExecute.Value;
                if (!process.StartInfo.UseShellExecute)
                {
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                }
            }

          AUMLog.WriteInLog("Run process. Filename={0}; Args={1}{2}",
            process.StartInfo.FileName,
            process.StartInfo.Arguments,
            string.IsNullOrEmpty(process.StartInfo.UserName)
              ? string.Empty
              : string.Format(CultureInfo.InvariantCulture, "User={0}{1}",
                string.IsNullOrEmpty(process.StartInfo.Domain)
                  ? string.Empty
                  : process.StartInfo.Domain + "\\", process.StartInfo.UserName));

            try
            {
                process.Start();
                if (needWait) process.WaitForExit();//process.WaitForExit(60 * 60 * 1000); //������ ����������� �� ���������� ����� ������� � 1 �.
            }
            catch (Exception exc)
            {
                errorMessage = String.Format("Message='{0}'; filename='{1}'; agrs='{2}'; UserName='{3}';"
                                             , exc.Message
                                             , filename
                                             , arguments
                    );
                AUMLog.WriteInLog(errorMessage);
                SecurityLog.WriteToEventLogWindows(EventLogEntryType.Error, errorMessage);
            }
            if (process.HasExited) AUMLog.WriteInLog("Process Exit code='{0}'", process.ExitCode);
            if ((errorMessage.Length == 0) && process.HasExited)
            {
                if (process.ExitCode == 8) //0,1,2,8,200
                {
                    errorMessage = (process.ExitCode == 8)
                                       ? ResourceHelper.Get("ReasonConnectionLost")
                                       : ResourceHelper.Get("UnknownError"); //TODO ����������� � ������ ������
                    if (process.StartInfo.RedirectStandardError && process.StartInfo.RedirectStandardOutput)
                    {
                        var output = process.StandardOutput.ReadToEnd();
                        var error = process.StandardError.ReadToEnd();
                        errorMessage = String.Format("Output: {0}, Error: {1}"
                                                     , String.IsNullOrEmpty(output) ? "unknown" : output
                                                     , String.IsNullOrEmpty(error) ? "unknown" : error);
                    }
                }
                else if (process.ExitCode == 2)
                {
                    if (process.StartInfo.RedirectStandardError && process.StartInfo.RedirectStandardOutput)
                    {
                        var output = process.StandardOutput.ReadToEnd();
                        var error = process.StandardError.ReadToEnd();
                        errorMessage = String.Format("Output: {0}, Error: {1}"
                                                     , String.IsNullOrEmpty(output) ? "unknown" : output
                                                     , String.IsNullOrEmpty(error) ? "unknown" : error);
                    }

                    if (errorMessage.Length == 0) errorMessage = "Possibly incorrect path";
                }
            }

            return errorMessage;
        }

        /// <summary>
        /// ������ ������
        /// </summary>
        /// <param name="regKey"></param>
        /// <param name="filename"></param>
        public static void FixRegistry(string regKey, string filename)
        {
            try
            {
                var pathRegistry = RegistryHelper.ReadFromRegistryAutorun(regKey).ToUpperInvariant();
                var realPath = String.Format("{0}{1}{0}", "\"", filename); //���� � ������� ������ ���� � ������� ��������
                if ((pathRegistry.Length > 0) && !pathRegistry.Equals(realPath.ToUpperInvariant()) && File.Exists(filename))
                {
                    RegistryHelper.WriteToRegistryAutorun(regKey, realPath);
                }
            }
            catch (Exception e)
            {
                //���� �� ������� ��������� ������, �� ������� ������
                //��� �� ����������� ������ � ����� ���������� ����������
              SecurityLog.WriteToEventLogWindows(EventLogEntryType.Warning, "Can't fix registry errors: {0}; reg key={1}", e.Message, regKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="domain"> </param>
        /// <returns></returns>
        public static bool IsValidLogin(string login, string password, string domain)
        {
            IntPtr userHandle;
            return IsValidLogin(login, password, domain, out userHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="domain"> </param>
        /// <param name="userHandle"></param>
        /// <returns></returns>
        public static bool IsValidLogin(string login, string password, string domain, out IntPtr userHandle)
        {
            //������� �������� ����� �� �������
            //var domain = RegistryHelper.GetDomain();

            AUMLog.WriteInLog("Check valid login. Login='{0}', domain='{1}'", login, domain);
            userHandle = IntPtr.Zero;

            var result = Win32Helper.LogonUser(
                login
                , domain  //Environment.MachineName
                , password
                , (int)Win32Helper.LogonType.LOGON32_LOGON_INTERACTIVE
                , (int)Win32Helper.LogonProvider.LOGON32_PROVIDER_DEFAULT
                , ref userHandle);

            AUMLog.WriteInLog("Result='{0}' Userhandle='{1}'", result, userHandle);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SecureString GetSecureString(string password)
        {
            var result = new SecureString();
            foreach (var c in password)
                result.AppendChar(c);
            result.MakeReadOnly();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirUpdateServer"></param>
        /// <param name="loginDecrypted"></param>
        /// <param name="passwordDecrypted"></param>
        /// <param name="successfullyConnect"></param>
        /// <param name="standartError"></param>
        public static string ConnectToUpdateServer(string dirUpdateServer, string loginDecrypted, string passwordDecrypted, out bool successfullyConnect, out string standartError)
        {
            standartError = RunProcess(
                Path.Combine(s_windowsDirPath, @"system32\net.exe")
                , String.Format("use {0} {1} /USER:{2}", dirUpdateServer, passwordDecrypted,loginDecrypted) //String.Concat("\"", loginDecrypted, "\"")
                , true
                , String.Empty
                , String.Empty
                );
            successfullyConnect = standartError.Length == 0;

            return standartError;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearAllConnectionToWebFolder(out string standartError)
        {
            standartError = RunProcess(
                Path.Combine(s_windowsDirPath, @"system32\net.exe")
                , "use /del * /yes"
                , true
                , String.Empty
                , String.Empty
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirUpdateRemote"></param>
        /// <param name="programID"></param>
        /// <param name="needCloseConnect"></param>
        /// <returns></returns>
        public static bool OpenWebFolder(string dirUpdateRemote, string programID, out bool needCloseConnect)
        {
            needCloseConnect = false;
            //����������, ���� �� ������ � ������� ����������
            //���� ��� �� ��������� ����, �� ������������ ����� �����
            //������� ������� ��� ������ � ������ �� ���� �� �������������
            //if (!dirUpdateRemote[0].ToString().Equals("\\"))
            //{
            //    try
            //    {
            //        //������ �������� �������� �����, ��� ������ ��� ������, ���� ���������� �������
            //        Directory.GetFiles(dirUpdateRemote);
            //    }
            //    catch (Exception exc)
            //    {
            //        //����� ������������� ���������� � �������� ����������
            //        SecurityLog.WriteToEventLogWindows(exc.ToString(), EventLogEntryType.Error);
            //        needCloseConnect = true;
            //    }
            //}
            //else
            //{
            //    needCloseConnect = true;
            //}

            //if (needCloseConnect)
            //{

            #region ��������� ������ � ������ ��� ������� � ������� ����������

            //��� ��������� ����� �������� �� ���������� �������
            var loginCrypted = RegistryHelper.ReadFromRegistry("UpdateServerLogin");
            var passwordCrypted = RegistryHelper.ReadFromRegistry("UpdateServerPassword");
            //if ((loginCrypted.Length == 0) || (passwordCrypted.Length == 0))
            //{
            //    //��� ������������ ������ ������ � �����
            //    if (FormsHelper.ShowUpdateServerWindow() == DialogResult.Cancel) return false;

            //    //�������� ������
            //    loginCrypted = ReadFromRegistry("UpdateServerLogin");
            //    passwordCrypted = ReadFromRegistry("UpdateServerPassword");
            //}

            var loginDecrypted = loginCrypted.Length > 0 ? Cryptor.Decrypt(loginCrypted) : String.Empty;
            var passwordDecrypted = passwordCrypted.Length > 0
                                           ? Cryptor.Decrypt(passwordCrypted, loginDecrypted)
                                           : String.Empty;

            #endregion

            //���� ��� ����, �� ���������� ������� ����������
            try
            {
                string standartError;
                bool successfullyConnect;

                //�������� ������ �������������
                ClearAllConnectionToWebFolder(out standartError);
                if (standartError.Length > 0)
                {
                    //SecurityLog.WriteToEventLogWindows(
                    //    String.Format("Can't connect To Update Server. UpdateServer='{0}', Error='{1}'", dirUpdateRemote, standartError)
                    //    , EventLogEntryType.Error);
                    throw new Exception(standartError);
                }

                //������������
                ConnectToUpdateServer(dirUpdateRemote, loginDecrypted, passwordDecrypted, out successfullyConnect,
                                      out standartError);
                needCloseConnect = successfullyConnect;

                //if (standartError.Length > 0)
                //    SecurityLog.WriteToEventLogWindows(
                //        String.Format("Can't Connect To Update Server. UpdateServer='{0}', Error='{1}'", dirUpdateRemote,
                //                      standartError), EventLogEntryType.Error);
                if (!successfullyConnect || (standartError.Length > 0)) throw new Exception(standartError);
            }
            catch (Exception exc)
            {
                SecurityLog.WriteToEventLogWindows(
                  EventLogEntryType.Error,
                  "Can't Connect To Update Server. UpdateServer='{0}', Error='{1}'", dirUpdateRemote, exc.Message);
                return false;
            }
            //}

            return true;
        }

        /// <summary>
        /// ���������� ������ �� ������� � ����������� ���������� ������������
        /// </summary>
        /// <param name="loginkey"></param>
        /// <param name="passwordkey"></param>
        /// <param name="loginDecrypted"></param>
        /// <param name="passwordDecrypted"></param>
        /// <returns>true ���� ������� ������������</returns>
        public static bool DecryptUser(string loginkey, string passwordkey, out string loginDecrypted, out string passwordDecrypted)
        {
            bool result = false;
            loginDecrypted = passwordDecrypted = String.Empty;
            try
            {
                string loginCrypted = RegistryHelper.ReadFromRegistry(loginkey);
                string passwordCrypted = RegistryHelper.ReadFromRegistry(passwordkey);
                if ((loginCrypted.Length > 0) && (passwordCrypted.Length > 0))
                {
                    loginDecrypted = Cryptor.Decrypt(loginCrypted);
                    passwordDecrypted = Cryptor.Decrypt(passwordCrypted, loginDecrypted);
                    result = true;
                }
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckUrlAccess(string url)
        {
            AUMLog.WriteInLog("Try to ping...");
            //��������� 5 ���
            var i = 1;
            var pingSuccessful = false;
            while (i <= 5)
            {
                pingSuccessful = Ping(url);
                AUMLog.WriteInLog("Try #{0}; Result={1}", i, pingSuccessful);
                if (pingSuccessful) break;
                i++;
            }
            return pingSuccessful;
        }

        /// <summary>
        /// ����������� � ���������� � VPN
        /// </summary>
        /// <param name="connectionName">������������ VPN �������</param>
        ///// <param name="url">ip-����� ��� �������� ����������� �������� ������</param>
        /// <param name="connect">true - ���������� �����������, false - ���������� ����������</param>
        public static string ConnectToVPN(string connectionName, /*string url, */bool connect = true)
        {
            //����������� ����� ����������� ������ � ��� ������, ���� ���������� ������ �� �����
            //if (connect && (url.Length > 0))
            //{
            //    AUMLog.WriteInLog("Try to ping...");
            //    //��������� 5 ���
            //    var i = 1;
            //    while (i <= 5)
            //    {
            //        var pingSuccessful = Ping(url);
            //        AUMLog.WriteInLog("Try #{0}; Result={1}", i, pingSuccessful));
            //        if (pingSuccessful) break;
            //        i++;
            //    }
            //}

            var args = connect ? String.Format("connect {0}", connectionName) : "disconnect";
            AUMLog.WriteInLog("Args={0}", args);
            
            var fullPath = Path.Combine(s_programFilesDirPath, @"Cisco Systems\VPN Client\vpnclient.exe");
            AUMLog.WriteInLog("Full path to VPN Client={0}", fullPath);
            var errorString = RunProcess(
                fullPath
                , args
                , true
                , String.Empty
                , String.Empty
                );
            
            return errorString;
        }

        /// <summary>
        /// ��������� �� ������ ���� ip-�����. ���� �� �������, �� ������ ������ ������.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetUrl(string path)
        {
            var ip = String.Empty;
            var pos = path.IndexOf("\\", 3);
            if ((path.IndexOf("\\", 0, 1, StringComparison.Ordinal) >= 0) && (pos > 0))
            {
                ip = path.Substring(2, pos - 2);
            }
            return ip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirUpdateServer"></param>
        public static void DisconnectToUpdateServer(string dirUpdateServer)
        {
            RunProcess(
                Path.Combine(s_windowsDirPath, @"system32\net.exe")
                , String.Format("use /DELETE {0}", dirUpdateServer)
                , true
                ,String.Empty,String.Empty
                );
        }

        /// <summary>
        /// ��������� ����������� ��������� �������
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool Ping(string url)
        {
            var result = false;
            try
            {
                using (var pingSender = new Ping())
                {
                    var reply = pingSender.Send(url, 1000); //���� 1 ���
                    if (reply != null) result = reply.Status == IPStatus.Success;
                }
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
            }

            return result;
        }
        
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programID">��������� ��������� ������</param>
        /// <returns></returns>
        public static string GetRegimeCaption(string programID)
        {
            var regimeDescription =  ResourceHelper.Get("Unknown");
            if (programID.Equals(Args.Sync)) regimeDescription = ResourceHelper.Get("synchronization");
            else if (programID.Equals(ProductHelper.AumProgramId)) regimeDescription = ResourceHelper.Get("updatingAum");
            else if (programID.Equals(ProductHelper.EidssProgramId)) regimeDescription = ResourceHelper.Get("updatingEIDSS");
            else if (programID.Equals(ProductHelper.NsProgramId)) regimeDescription = ResourceHelper.Get("updatingNotificationService");
            else if (programID.Equals(ProductHelper.DbProgramId)) regimeDescription = ResourceHelper.Get("updatingDatabase");
            else if (programID.Equals(ProductHelper.DbaProgramId)) regimeDescription = ResourceHelper.Get("updatingDatabaseArchive");
            else if (programID.Equals(ProductHelper.CustomTasksProgramId)) regimeDescription = ResourceHelper.Get("executingfiles");
            else if (programID.Equals(ProductHelper.AvrServiceDbProgramId)) regimeDescription = ResourceHelper.Get("AvrServiceDb");
            else if (programID.Equals(ProductHelper.AvrServiceProgramId)) regimeDescription = ResourceHelper.Get("AvrServiceProgram");
            return regimeDescription;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayToString(string[] array)
        {
            var sb = new StringBuilder();
            foreach (var s in array)
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool NotEmpty(object o)
        {
            return ((o != null) && (o != DBNull.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="dtEvent"></param>
        /// <param name="dtSecurityAudit"></param>
        /// <param name="language"></param>
        public static void GetLanguage(string clientID, out DateTime dtEvent, out DateTime dtSecurityAudit, out string language)
        {
            dtEvent = dtSecurityAudit = DateTime.MinValue;
            language = String.Empty;
            try
            {
                var conn = DatabaseHelper.GetConnection(UpdateDatabases.DbMain);
                if (conn != null)
                {
                    var updateMessenger = new UpdateMessenger(conn);
                    var dataTable = updateMessenger.GetCurrentLanguageInfo(clientID);
                    if (dataTable.Rows.Count == 1)
                    {
                        var oEvent = dataTable.Rows[0]["Event"];
                        var oSecurityAudit = dataTable.Rows[0]["SecurityAudit"];
                        var oLanguage = dataTable.Rows[0]["Language"];

                        dtEvent = NotEmpty(oEvent) ? Convert.ToDateTime(oEvent) : DateTime.MinValue;
                        dtSecurityAudit = NotEmpty(oSecurityAudit)
                                              ? Convert.ToDateTime(dataTable.Rows[0]["SecurityAudit"])
                                              : DateTime.MinValue;

                        if ((dtEvent > dtSecurityAudit.ToLocalTime()) && (NotEmpty(oLanguage)))
                            language = oLanguage.ToString();
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetClientIDTotal()
        {
            string clientID = GetClientID("e");
            if (clientID.Length == 0) clientID = GetClientID("ns");
            if (clientID.Length == 0) clientID = GetClientID(String.Empty); //��� MAC-�����
            return clientID;
        }

        /// <summary>
        /// ���������� ������� ��������
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetCurrentCulture()
        {
            //������� �������� �������� �� ��
            //���� �� ���������, �������� �� ������� EIDSS

            string lang = String.Empty;

            string clientID = GetClientIDTotal();
            if (clientID.Length > 0)
            {
                try
                {
                    DateTime dtEvent;
                    DateTime dtSecurityAudit;
                    string language;
                    GetLanguage(clientID, out dtEvent, out dtSecurityAudit, out language);
                    lang = language;
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                {
                }
                // ReSharper restore EmptyGeneralCatchClause
            }

            if (lang.Length == 0)
            {
                //������� ������� �� ������� EIDSS
                lang = GetEIDSSCultureLetters();
            }

            lang = lang.Length > 0 ? lang : "en";
            return (s_cultures.ContainsKey(lang)) ? s_cultures[lang]  : new CultureInfo("en-US");
        }

        /// <summary>
        /// ��������� �������� �� eidss.config
        /// </summary>
        /// <returns></returns>
        public static string GetEIDSSCultureLetters()
        {
            var keyLang = String.Empty;
            if (Directory.Exists(EIDSSPath))
            {
                var exe = Path.Combine(EIDSSPath, EIDSSFileName);
                if (File.Exists(exe))
                {
                    var configuration = ConfigurationManager.OpenExeConfiguration(exe); 
                    if (configuration.AppSettings.Settings["DefaultLanguage"] != null)
                    {
                        keyLang = configuration.AppSettings.Settings["DefaultLanguage"].Value;
                    }
                }
            }

            return keyLang;
        }

        /// <summary>
        /// ���������� ������ ������� ������
        /// </summary>
        /// <param name="dir">�������, ��� �������� ����� �� ���������</param>
        /// <param name="updates">�������� ��� ������ ���������</param>
        /// <param name="isSmall">�������� �� ��� ��������� ������</param>
        /// <param name="version">������ ������� �����</param>
        /// <returns>��� ����� ������� ������</returns>
        public static string CreateTotalArchive(string dir, List<string> updates, bool isSmall, string version)
        {
            var files = new List<FileInfo>();
            foreach (var update in updates)
            {
                var patchFilename = Path.GetFileName(update);
                var crcFilename = Path.GetFileName(Path.ChangeExtension(update, "crc"));
                files.AddRange(FileHelper.GetFiles(dir, patchFilename, false));
                files.AddRange(FileHelper.GetFiles(dir, crcFilename, false));
            }
            var filter = FileHelper.GetFilesFilter(files);
            var fZip = CreateZipper();
            //������ ���� �������� � ��� �� �������, ��� ��� ����� ���������
            var updateFile = Path.Combine(dir, FileHelper.GetTotalUpdateFileName(version, isSmall));
            fZip.CreateZip(updateFile, dir, true, filter, String.Empty);
            return updateFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool CheckUpdateVersions(string version)
        {
            return ParseUpdateVersions(version).Count > 0;
        }

      public static List<UpdateVersion> ParseUpdateVersions(string version)
      {
        var result = new List<UpdateVersion>();
        var corteges = version.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        if (corteges.Length > 0)
        {
          result.AddRange(from cortege in corteges let v = new UpdateVersion() where v.ExtractFromString(cortege) select v);
        }
        return result;
      }

      public static bool CompareDbVersion(Version versionUpdate, Version currentDbVersion)
      {
        //��� ���������� �� ���������, ����� ������ �� � ������� ���� ������, ��� ������� ������ ��, � ����� ���� �������, ������� ���� ���������
        AUMLog.WriteInLog("CheckDbVersion. versionUpdate='{0}'; currentDBVersion='{1}';", versionUpdate, currentDbVersion);
        if ((versionUpdate == null) || (currentDbVersion == null))
        {
          AUMLog.WriteInLog("Warning! version is null!");
          return false;
        }
        return versionUpdate > currentDbVersion;
      }

        /// <summary>
        /// Check results of db update
        /// </summary>
        /// <param name="versionDb">��������� ������ ��</param>
        /// <param name="currentDbVersion">�������� ������ ��</param>
        /// <param name="programId"> </param>
        /// <param name="versionProduct"> </param>
        /// <returns></returns>
        internal static bool CheckDbUpdateExecution(
          Version versionDb,
          Version currentDbVersion,
          string programId,
          Version versionProduct)
        {
          AUMLog.WriteInLog("Checking for previous db patch complete...");
          if (currentDbVersion == VersionFactory.EmptyVersion)
          {
            AUMLog.WriteInLog("Check failed: can't get current db version");
            return false;
          }
          //�����, ����� ������ ���� ��. ���� �� �� ������, �� ������ ��������� ��� ����������.
          //�������� ��������� ���������� ����� ��
          var logFilename = Path.Combine(
            AUMPath,
            FileHelper.AUMLogDirName,
            Environment.MachineName,
            FileHelper.GetLogFileName(programId, versionDb.ToString()));
          //���������� ����� ���� �������� ������
          if (!File.Exists(logFilename))
          {
            AUMLog.WriteInLog("Can't find file '{0}'. This is error, can't execute next update", logFilename);
            return false;
          }

          //�������� ������� ���������� �� ��� ����� ���������� ��
          if (!IsUpdateSuccess(logFilename))
          {
            AUMLog.WriteInLog(
              "Log file '{0}' checked. {1} update was not successful. This is error, can't execute '{2}' update",
              logFilename,
              programId,
              programId);
            return false;
          }
          //����� ����, �������� �������������� ������ �� � ����������
          try
          {
            if (versionDb < currentDbVersion)
            {
              AUMLog.WriteInLog(
                "Real database Version ('{0}') is newer than declared database Version ('{1}'). Update '{2}' was not required",
                currentDbVersion,
                versionDb,
                versionProduct);
              return true;
            }
          }
          catch (Exception exc)
          {
            AUMLog.WriteInLog(
              "Can't compare real database Version with declared database Version. Error='{0}'. Can't execute '{1}' update",
              exc.Message,
              programId);
            return false;
          }
          return true;
        }

        public static Version GetUpdateVersion(string updateFile)
        {
          var fZip = CreateZipper();
          fZip.ExtractZip(updateFile, TempDirPath, FileHelper.FileNameUpdate);
          
          var updFileName = Path.Combine(TempDirPath, FileHelper.FileNameUpdate);
          if (!File.Exists(updFileName))
          {
            return VersionFactory.EmptyVersion;
          }
          var updateSettings = ReadUpdateFile(updFileName);
          return updateSettings.Version;
        }

        public static void SetCorrectCodePage()
        {
            if (ZipConstants.DefaultCodePage == 1)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(String.Empty);
                AUMLog.WriteInLog("Current Culture is {0}/{1}", Thread.CurrentThread.CurrentCulture.DisplayName, Thread.CurrentThread.CurrentCulture.EnglishName);
                if (ZipConstants.DefaultCodePage == 1)
                {
                    if (Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage != 1)
                    {
                        ZipConstants.DefaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;
                    }
                    else if (Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage != 1)
                    {
                        ZipConstants.DefaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
                    }
                    else
                    {
                        ZipConstants.DefaultCodePage = 437;
                    }
                }
                AUMLog.WriteInLog("ZipConstants Code Page changed to {0}", ZipConstants.DefaultCodePage);
            }
        }

        public static FastZip CreateZipper()
        {
            SetCorrectCodePage();
            return new FastZip();
        }
    }
}
