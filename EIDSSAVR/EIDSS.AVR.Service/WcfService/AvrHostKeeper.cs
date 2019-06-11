﻿using System;
using eidss.avr.service.WcfFacade;
using eidss.model.AVR.DataBase;
using eidss.model.Trace;
using eidss.model.WcfService;
using EIDSS.AVR.Service.Scheduler;
using EIDSS.AVR.Service.WcfFacade;
using StructureMap;

namespace EIDSS.AVR.Service.WcfService
{
    public class AvrHostKeeper : ServiceHostKeeper
    {
        private SchedulerRunner m_Scheduler;
        private readonly IContainer m_Container;

        public AvrHostKeeper(IContainer container)
        {
            m_Container = container;
        }

        protected override Type ServiceType
        {
            get { return typeof (AVRFacade); }
        }

        protected override string DefaultServiceHostURL
        {
            get { return "http://localhost:8071/"; }
        }

        protected override string ServiceHostURLConfigName
        {
            get { return "AVRServiceHostURL"; }
        }

        protected override string TraceCategory
        {
            get { return TraceHelper.AVRCategory; }
        }

        protected override void InitEidssCore()
        {
            EidssAvrServiceInitializer.InitEidssCore();
        }

        protected override void InitServiceHost()
        {
            base.InitServiceHost();

            m_Scheduler = m_Container.GetInstance<SchedulerRunner>();

            Trace.TraceInfo(TraceTitle, string.Format(@"AVR Scheduler started with configuration: {0}{1} ",
                Environment.NewLine, m_Scheduler.Configuration));
            try
            {
                DatabaseNames names = m_Scheduler.Facade.GetDatabaseName();
                Trace.TraceInfo(TraceTitle, string.Format(@"AVR Service use 
                    EIDSS Database '{0}'
                    EIDSS Archive Database '{1}'
                    AVR Database '{2}'",
                    names.EidssActualDbName, names.EidssArchiveDbName, names.AvrDbName));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex);
            }
        }

        protected override void OpenExtender()
        {
            m_Scheduler.Start();
            Trace.TraceInfo(TraceTitle, @"Scheduler started.");
        }

        protected override void CloseExtender()
        {
            if (m_Scheduler == null)
            {
                throw new ApplicationException(@"Scheduler already stopped.");
            }

            m_Scheduler.Dispose();
            m_Scheduler = null;

            Trace.TraceInfo(TraceTitle, @"Scheduler stopped.");
        }
    }
}