﻿using System;
using System.IO;
using System.Reflection;
using bv.common.Configuration;
using bv.common.Core;
using bv.model.BLToolkit;
using bv.model.Model.Core;
using EIDSS;
using eidss.avr.db.CacheReceiver;
using eidss.avr.db.Common;
using eidss.avr.db.DBService.Access;
using eidss.model.Core;

namespace eidss.avr.export
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Init();

                AvrAccessExportResult result = ParceArgs(args);

                if (result.IsOk)
                {
                    var exportedMessage = ExportToAccess(result);
                    if (!string.IsNullOrEmpty(exportedMessage))
                    {
                        result = new AvrAccessExportResult(exportedMessage);
                    }
                }


                Console.WriteLine(result.Serialize());
            }
            catch (Exception ex)
            {
                var result = new AvrAccessExportResult("Export error", ex.ToString());
                Console.WriteLine(result.Serialize());
            }
            //Console.ReadLine();
            Environment.Exit(0);
        }

        private static void Init()
        {
            Config.DefaultGlobalConfigFileName = Constants.GlobalEidssConfigName;
            DbManagerFactory.SetSqlFactory(new ConnectionCredentials().ConnectionString);
            EidssUserContext.Init();
            EIDSS_LookupCacheHelper.Init();
        }

        private static string ExportToAccess(AvrAccessExportResult result)
        {
            //using (new CultureInfoTransaction(new CultureInfo(CustomCultureHelper.SupportedLanguages[lang])))
            ModelUserContext.CurrentLanguage = result.Language;
            CachedQueryResult queryResult = ServiceClientHelper.ExecQuery(result.QueryId, false, true);
            if (string.IsNullOrEmpty(queryResult.ErrorMessage))
            {
                AccessDataAdapter.QueryLineListToAccess(result.ResultFilePath, queryResult.QueryTable);
            }
            return queryResult.ErrorMessage;
        }

        private static AvrAccessExportResult ParceArgs(string[] args)
        {
            long queryId;
            if (args.Length == 0)
            {
                return new AvrAccessExportResult("Query id, language and exported file name is not passed");
            }

            if (!long.TryParse(args[0], out queryId))
            {
                return new AvrAccessExportResult(string.Format("Cannot parce given query id: '{0}'", args[0]));
            }
            if (args.Length < 2)
            {
                return new AvrAccessExportResult("Language and exported file name is not passed");
            }
            string lang = args[1];
            if (!Localizer.AllSupportedLanguages.ContainsKey(lang))
            {
                return new AvrAccessExportResult(string.Format("Language '{0}' is not supported", lang));
            }
            if (args.Length < 3)
            {
                return new AvrAccessExportResult("Exported file name is not passed");
            }

            string fileName = args[2];

            Assembly asm = Assembly.GetExecutingAssembly();
            string templatePath = Utils.GetFilePathNearAssembly(asm, @"\AccessTemplate\template.mdb");
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.Copy(templatePath, fileName);
            }
            catch (Exception)
            {
                return new AvrAccessExportResult(string.Format("Could not create Microsoft Access file '{0}'", fileName));
            }

            //bool isWeb = (args.Length > 2);
            return new AvrAccessExportResult(queryId, lang, fileName);
        }
    }
}