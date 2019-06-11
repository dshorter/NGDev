﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bv.model.Model.Core;
using bv.model.Model.Validators;
using bv.model.BLToolkit;
using bv.model.Helpers;

namespace eidss.model.Schema
{
    public partial class AsCampaign
    {
        public bool ValidateSessionAssignment(AsSession session)
        {
            if (session.idfCampaign.HasValue)
            {
                if (session.idfCampaign != this.idfCampaign)
                {
                    throw new ValidationModelException("msgASSessionAlreadyBelongsToCampaign", "idfCampaign", "idfCampaign", new string[] { "Error" }, typeof(PredicateValidator), ValidationEventType.Error, session);
                }
            }

            if (!DateHelper.ValidateDateInRange(session.datStartDate, datCampaignDateStart, datCampaignDateEnd) ||
                !DateHelper.ValidateDateInRange(session.datEndDate, datCampaignDateStart, datCampaignDateEnd))
            {
                throw new ValidationModelException("msgCampaignSessionDatesConflict", "idfCampaign", "idfCampaign", new object[] { session.datStartDate, session.datEndDate, datCampaignDateStart, datCampaignDateEnd, "Error" }, typeof(PredicateValidator), ValidationEventType.Error, session);
            }

            if (!AsSession.ValidateCheckAnimalSampleList(session, this))
            {
                throw new ValidationModelException("msgCampaignDoesntContainDiagnosis", "idfCampaign", "idfCampaign", new object[] { "Error" }, typeof(PredicateValidator), ValidationEventType.Error, session);
            }

            if (!AsSession.ValidateCheckToDeleteDiagnosis(session, this))
            {
                throw new ValidationModelException("msgCampaignDoesntContainDiagnosis", "idfCampaign", "idfCampaign", new object[] { "Error" }, typeof(PredicateValidator), ValidationEventType.Error, session);
            }

            //if session contains diagnosis which are not in the campaign, error
            if (!AsSession.ValidateCampaignToSessionDiseasesRules(session,this))            
            {
                throw new ValidationModelException("errNotMatchSessionDiagnosis", "idfCampaign", "idfCampaign", new string[] { "Confirmation" }, typeof(PredicateValidator), ValidationEventType.Error, session);
            }           
           

            return true;
        }

        private static bool NewDiseaseValidation(AsCampaign campaign, AsDisease item)
        {
            //check duplicates
            if (campaign.Diseases.Count(x => x.idfCampaignToDiagnosis != item.idfCampaignToDiagnosis && x.idfsDiagnosis == item.idfsDiagnosis && x.idfsSpeciesType == item.idfsSpeciesType && x.idfsSampleType == item.idfsSampleType && !x.IsMarkedToDelete) > 0)
                throw new ValidationModelException("AsSession.Diseases.Duplicate_msgId", "idfsSpeciesType", "idfsSpeciesType", null, typeof(PredicateValidator), ValidationEventType.Error, item);

            return true;
        }

        public static bool AssignCampaignToSession(AsCampaign campaign, long idfMonitoringSession)
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(eidss.model.Core.EidssUserContext.Instance))
            {
                AsSession session = AsSession.Accessor.Instance(null).SelectByKey(manager, idfMonitoringSession);
                if (session != null && campaign.Sessions.Count(s => s.idfMonitoringSession == session.idfMonitoringSession && !s.IsMarkedToDelete) == 0)
                {

                    if (campaign.ValidateSessionAssignment(session))
                    {
                        //session.CampaignInRamOnly = campaign;
                        session.SetCampaignForTest(campaign);
                        session.idfCampaign = campaign.idfCampaign;
                        var temp = session.CampaignInRamOnly;
                        
                        campaign.Sessions.Add(AsMonitoringSession.CreateFromASSession(manager, campaign, session));
                    }

                }
            }
            return true;
        }

        public static bool AssignCampaignToSession(AsCampaign campaign, AsSession session)
        {
            if (session == null)
                return false;

            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(eidss.model.Core.EidssUserContext.Instance))
            {
                if (campaign.ValidateSessionAssignment(session))
                {
                    //session.CampaignInRamOnly = campaign;
                    session.SetCampaignForTest(campaign);
                    session.idfCampaign = campaign.idfCampaign;
                    var temp = session.CampaignInRamOnly;

                    campaign.Sessions.Add(AsMonitoringSession.CreateFromASSession(manager, campaign, session));
                }

            }

            return true;
        }

        public static void RemoveSessionFromCampaign(AsCampaign campaign, long idfMonitoringSession)
        {
            AsMonitoringSession mssession = campaign.Sessions.Single(s => s.idfMonitoringSession == idfMonitoringSession);
            mssession.idfCampaign = null;

        }


    }


    public partial class AsMonitoringSession
    {

        public static void UpdateFromASSession(AsMonitoringSession mssession, AsSession original, long idfCampaign)
        {
            string[] populateSession = new string[] {                                            
                      "datStartDate",
                      "datEndDate",
                      "strMonitoringSessionID",
                      };

            foreach (var prop in populateSession)
            {
                if (original.GetValue(prop) != null)
                    mssession.SetValue(prop, original.GetValue(prop).ToString());
            }

            mssession.idfCampaign = idfCampaign;
            mssession.strRegion = original.Region.ToString();
            mssession.strRayon = original.Rayon == null ? String.Empty : original.Rayon.ToString();
            mssession.strSettlement = original.Settlement == null ? String.Empty : original.Settlement.ToString();
            mssession.strStatus = original.MonitoringSessionStatus.ToString();
            //mssession.FullSession = original;
        }

        public static AsMonitoringSession CreateFromASSession(DbManagerProxy manager, AsCampaign campaign, AsSession original)
        {

            AsMonitoringSession mssession = AsMonitoringSession.Accessor.Instance(null).CreateNewT(manager, campaign);


            string[] populateSession = new string[] {
                      "idfMonitoringSession",
                      "idfCampaign",                      
                      "datStartDate",
                      "datEndDate",
                      "strMonitoringSessionID",
                      };

            foreach (var prop in populateSession)
            {
                if (original.GetValue(prop) != null)
                    mssession.SetValue(prop, original.GetValue(prop).ToString());
            }

            mssession.strRegion = original.Region == null ? String.Empty : original.Region.ToString();
            mssession.strRayon = original.Rayon == null ? String.Empty : original.Rayon.ToString();
            mssession.strSettlement = original.Settlement == null ? String.Empty : original.Settlement.ToString();
            mssession.strStatus = original.MonitoringSessionStatus.ToString();
            //mssession.FullSession = original;

            return mssession;
        }
    }
}
