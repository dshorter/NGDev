﻿using System;
using System.Collections.Generic;
using System.Linq;
using bv.common.Configuration;
using eidss.avr.db.AVRCacheService;
using eidss.model.AVR.DataBase;
using eidss.model.AVR.ServiceData;
using IAVRFacade = eidss.model.WindowsService.IAVRFacade;

namespace eidss.avr.db.CacheReceiver
{
    public class AvrServiceClientWrapper : IDisposable, IAVRFacade
    {
        private const string MethodFromSchedulerOnly =
            "This method should be called from AVR scheduler (which is part of AVR service) only ";

        private readonly AVRFacadeClient m_Client;

        public AvrServiceClientWrapper()
        {
            string address = BaseSettings.AvrServiceHostURL;
            m_Client = new AVRFacadeClient("BasicHttpBinding_IAVRFacade", address);
        }

        public AvrServiceClientWrapper(string address)
        {
            m_Client = new AVRFacadeClient("BasicHttpBinding_IAVRFacade", address);
        }

        #region IAVRFacade members

        public ChartExportDTO ExportChartToJpg(ChartTableDTO zippedData)
        {
            //return null;
            return m_Client.ExportChartToJpg(zippedData);
        }

        public ViewDTO GetCachedView(string sessionId, long layoutId, string lang)
        {
            return m_Client.GetCachedView(sessionId, layoutId, lang);
        }

        public void InvalidateViewCache(long layoutId)
        {
           m_Client.InvalidateViewCache(layoutId);
        }

        public QueryTableHeaderDTO GetCachedQueryTableHeader(long queryId, string lang, bool isArchive)
        {
            return m_Client.GetCachedQueryTableHeader(queryId, lang, isArchive);
        }


        public bool DoesCachedQueryExists(long queryId, string lang, bool isArchive)
        {
            return m_Client.DoesCachedQueryExists(queryId, lang, isArchive);
        }

        public void RefreshCachedQueryTableByScheduler(long queryId, string lang, bool isArchive)
        {
            throw new InvalidOperationException(MethodFromSchedulerOnly);
        }

        public QueryTableHeaderDTO GetConcreteCachedQueryTableHeader(long queryCacheId, long queryId, string lang, bool isArchive)
        {
            return m_Client.GetConcreteCachedQueryTableHeader(queryCacheId, queryId, lang, isArchive);
        }

        public QueryTablePacketDTO GetCachedQueryTablePacket(long queryCacheId, int packetNumber, int totalPacketCount)
        {
            return m_Client.GetCachedQueryTablePacket(queryCacheId, packetNumber, totalPacketCount);
        }

        public void InvalidateQueryCacheForLanguage(long queryId, string lang)
        {
            m_Client.InvalidateQueryCacheForLanguage(queryId, lang);
        }

        public void InvalidateQueryCache(long queryId)
        {
            m_Client.InvalidateQueryCache(queryId);
        }

        public void DeleteQueryCacheForLanguage(long queryId, string lang, bool leaveLastRecord)
        {
            throw new InvalidOperationException(MethodFromSchedulerOnly);
        }

        public DateTime? GetsQueryCacheUserRequestDate(long queryId)
        {
            throw new InvalidOperationException(MethodFromSchedulerOnly);
        }

        public Version GetServiceVersion()
        {
            return m_Client.GetServiceVersion();
        }

        public DatabaseNames GetDatabaseName()
        {
            return m_Client.GetDatabaseName();
        }

        public DateTime GetQueryRefreshDateTime(long queryId, string lang)
        {
            return m_Client.GetQueryRefreshDateTime(queryId, lang);
        }

        public List<long> GetQueryIdList()
        {
            return m_Client.GetQueryIdList().ToList();
        }

        public List<long> GetLayoutIdList()
        {
            
            return m_Client.GetLayoutIdList().ToList();
        }

        public long CopyLayout(long layoutId, string lang)
        {
            return m_Client.CopyLayout(layoutId, lang);
        }

        #endregion

        public void Dispose()
        {
            m_Client.Close();
        }
    }
}