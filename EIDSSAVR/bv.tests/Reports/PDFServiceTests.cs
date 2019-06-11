﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using bv.common.Configuration;
using bv.model.BLToolkit;
using bv.tests.ReportPdfService;
using eidss.model.Core;
using eidss.model.Enums;
using eidss.model.Reports.ARM;
using eidss.model.Reports.AZ;
using eidss.model.Reports.Common;
using eidss.model.Reports.GG;
using eidss.model.Reports.IQ;
using eidss.model.Reports.KZ;
using eidss.model.Reports.TH;
using eidss.model.Trace;
using eidss.webclient.Controllers;
using EIDSS.Reports.Service.WcfService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bv.tests.Reports
{
    [TestClass]
    public class PDFServiceTests
    {
        private static readonly object m_SyncRoot = new object();
        private static int m_TotalCount;
        private static ReportHostKeeper m_HostKeeper;
        private static TraceHelper m_Trace;

        public class ReportClientWrapper : IDisposable
        {
            private readonly ReportFacadeClient m_Client;

            public ReportClientWrapper()
            {
                string address = Config.GetSetting("ReportServiceHostURL", "http://localhost:8097/");
                m_Client = new ReportFacadeClient("BasicHttpBinding_IReportFacade", address);
            }

            public ReportFacadeClient Client
            {
                get { return m_Client; }
            }

            public void Dispose()
            {
                m_Client.Close();
            }
        }

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            m_Trace = new TraceHelper(TraceHelper.ReportsCategory);
            m_HostKeeper = new ReportHostKeeper();
            m_HostKeeper.Open();
        }

        [ClassCleanup]
        public static void MyClassCleanup()
        {
            try
            {
                m_HostKeeper.Close();
            }
            catch (Exception ex)
            {
                m_Trace.TraceError(ex);
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            DbManagerFactory.SetSqlFactory(new ConnectionCredentials().ConnectionString);
            EidssUserContext.Init();
        }

        #region PDF Human reports

        [TestMethod] 
        public void ServiceVersionTest()
        {
            using (var wrapper = new ReportClientWrapper())
            { 
                Version serviceVersion = wrapper.Client.GetServiceVersion();

                Assembly asm = Assembly.GetExecutingAssembly();
                var version = asm.GetName().Version;

                Assert.AreEqual(version, serviceVersion);
            }
        }

        [TestMethod] 
        public void PdfHumAggregateCaseTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumAggregateCase(new AggregateModel("en", 1, 6707800000000, 2, false)));
            }
        }

        [TestMethod] 
        public void PdfHumAggregateCaseSummaryTest()
        {
            const string aggrXml =
                @"<?xml version=""1.0"" encoding=""UTF-16""?><ROOT><AdminLevel AreaType=""satCountry""><AdminUnit AdminUnitID=""170000000"" /></AdminLevel><TimeInterval PeriodType=""sptYear""><TimeIntervalUnit StartDate=""2009-01-01"" FinishDate=""2011-12-31"" /><TimeIntervalUnit StartDate=""2008-01-01"" FinishDate=""2008-12-31"" /></TimeInterval></ROOT>";
            var observations = new List<long> {1, 2};
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumAggregateCaseSummary(
                    new AggregateSummaryModel("en", aggrXml, observations, false)));
            }
        }

        [TestMethod] 
        public void PdfHumUrgentyNotificationTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumUrgentyNotification(new BaseIdModel("en", 1, false)));
            }
        }

        [TestMethod] 
        public void PdfHumCaseInvestigationTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumCaseInvestigation(new HumIdModel("en", 1, 2, 3, 4, false)));
            }
        }

        [TestMethod] 
        public void PdfHumDiagnosisToChangedDiagnosistTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new DToChangedDSurrogateModel("en", 2010, 1, null, null, null, "", new string[0], new string[0], 0, null,
                    new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumDiagnosisToChangedDiagnosis(model));
            }
        }

        [TestMethod] 
        public void PdfHumMonthlyMorbidityAndMortalityTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumMonthlyMorbidityAndMortality(
                    new BaseDateModel("en", 2012, 1, false)));
            }
        }

        [TestMethod] 
        public void PdfHumFormN1Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new FormNo1SurrogateModel("en", 2012, 1, 10, true, null, null, null, null, null, null, null,
                    new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumFormN1(model));
                model.IsA3Format = false;
                AssertPDF(wrapper.Client.ExportHumFormN1(model));
            }
        }

        [TestMethod] 
        public void PdfHumDataQualityIndicatorsTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new DataQualityIndicatorsSurrogateModel("en", new[] {"2955880000000", "784620000000"}, "xxx",2012, 1, 2,
                    null, null, "", "", true, null, null, false);
                AssertPDF(wrapper.Client.ExportHumDataQualityIndicators(model));
            }
        }

        [TestMethod] 
        public void PdfHumDataQualityIndicatorsRayonsTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new DataQualityIndicatorsSurrogateModel("en", new[] {"2955880000000", "784620000000"}, "yy",2012, 0, 0,
                    null, null, "", "", true, null, null, false);
                AssertPDF(wrapper.Client.ExportHumDataQualityIndicatorsRayons(model));
            }
        }

        [TestMethod] 
        public void PdfExportHumSummaryByRayonDiagnosisAgeGroupsTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new SummaryByRayonDiagnosisModel("en", DateTime.Now.AddMonths(-1), DateTime.Now,  false);
                AssertPDF(wrapper.Client.ExportHumSummaryByRayonDiagnosisAgeGroups(model));
            }
        }

        [TestMethod] 
        public void PdfHumComparativeAZReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumComparativeAZReport(
                    new ComparativeSurrogateModel("en", 1, 1, 10, 2010, 2012,
                        null, null, null, null, 1, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod] 
        public void PdfHumExternalComparativeReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumExternalComparativeReport(
                    new ExternalComparativeSurrogateModel("en", null, null,null, null, 2010,
                        2012,  2, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod] 
        public void PdfHumMainIndicatorsOfAFPSurveillanceTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new AFPModel("en", 2012, 0, 0, false);
                AssertPDF(wrapper.Client.ExportHumMainIndicatorsOfAFPSurveillance(model));
            }
        }

        [TestMethod] 
        public void PdfHumAdditionalIndicatorsOfAFPSurveillanceTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new AFPModel("en", 2012, 0, 0, false);
                AssertPDF(wrapper.Client.ExportHumAdditionalIndicatorsOfAFPSurveillance(model));
            }
        }

        [TestMethod] 
        public void PdfHumWhoMeaslesRubellaReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumWhoMeaslesRubellaReport(new WhoMeaslesRubellaModel("en", 2012, 1,
                        WhoMeaslesRubellaModel.MeaslesId, false)));
            }
        }

        [TestMethod] 
        public void PdfHumComparativeReportOfTwoYearsTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumComparativeReportOfTwoYears(new ComparativeTwoYearsSurrogateModel("en", 2010, 2011, null, null,
                        null, null, null, null, null, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod] 
        public void PdfVeterinaryCasesReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new VetCasesSurrogateModel("en", null, null, "", "", null, "", 2010, 2012, 1, 2, false);
                AssertPDF(wrapper.Client.ExportVeterinaryCasesReport(model));
            }
        }

        [TestMethod] 
        public void PdfVeterinaryLabReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new VetLabSurrogateModel("en", 2013, 1, 2, null, "", null, null, "", "", null, new List<PersonalDataGroup>(),
                    false);
                AssertPDF(wrapper.Client.ExportVeterinaryLaboratoriesAZReport(model));
            }
        }

        [TestMethod] 
        public void PdfVeterinaryFormVet1Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new VetCasesSurrogateModel("en", null, null, "", "", null, "", 2010, 2012, 1, 2, false);
                AssertPDF(wrapper.Client.ExportVeterinaryFormVet1(model));
            }
        }

        [TestMethod] 
        public void PdfVeterinaryFormVet1ATest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new VetCasesSurrogateModel("en", null, null, "", "", null, "", 2010, 2012, 1, 2, false);
                AssertPDF(wrapper.Client.ExportVeterinaryFormVet1A(model));
            }
        }

        [TestMethod] 
        public void PdfExportVetSummaryTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new VetSummaryModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, 
                    VetSummarySurveillanceType.None, "",-1, "", -1, "",new string[0],false);

                AssertPDF(wrapper.Client.ExportVeterinarySummaryAZ(model));
            }
        }

        [TestMethod] 
        public void PdfHumSerologyResearchCardTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumSerologyResearchCard(
                    new HumanLabSampleModel("en", "123", "", "", false)));
            }
        }

        [TestMethod] 
        public void PdfHumMicrobiologyResearchCardTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumMicrobiologyResearchCard(
                    new HumanLabSampleModel("en", "123", "", "", false)));
            }
        }

        [TestMethod] 
        public void PdfHumAntibioticResistanceCardTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumAntibioticResistanceCard(
                    new HumanLabSampleModel("en", "123", "", "", false)));
            }
        }

        [TestMethod] 
        public void PdfHumAntibioticResistanceCardLMATest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumAntibioticResistanceCardLma(
                    new VetLabSampleModel("en", "123", false)));
            }
        }

        [TestMethod] 
        public void PdfLabTestingResultsAZTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLabTestingResultsAZ(
                    new LabTestingTesultsModel("en", "123", 1234, false)));
            }
        }

        [TestMethod] 
        public void PdfAssignmentLabDiagnosticAZTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportAssignmentLabDiagnosticAZ(
                    new AssignmentLabDiagnosticModel("en", "123", 123, false)));
            }
        }

        [TestMethod] 
        public void PdfHum60BJournalTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new Hum60BJournalModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, false);
                AssertPDF(wrapper.Client.ExportHum60BJournal(model));
            }
        }

        [TestMethod] 
        public void PdfHumMonthInfectiousDiseaseV4Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new MonthInfectiousDiseaseModel("en", 2012, 1, true, false);
                AssertPDF(wrapper.Client.ExportHumMonthInfectiousDiseaseV4(model));
            }
        }

        [TestMethod] 
        public void PdfHumMonthInfectiousDiseaseV5Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new MonthInfectiousDiseaseModel("en", 2012, 1, true, false);
                AssertPDF(wrapper.Client.ExportHumMonthInfectiousDiseaseV5(model));
            }
        }

        [TestMethod]
        public void PdfHumMonthInfectiousDiseaseV6Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new MonthInfectiousDiseaseModel("en", 2012, 1, true, false);
                AssertPDF(wrapper.Client.ExportHumMonthInfectiousDiseaseV6(model));
            }
        }
    
        [TestMethod]
        public void PdfHumMonthInfectiousDiseaseV61Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new MonthInfectiousDiseaseModel("en", 2012, 1, true, false);
                AssertPDF(wrapper.Client.ExportHumMonthInfectiousDiseaseV61(model));
            }
        }



        [TestMethod] 
        public void PdfHumIntermediateMonthInfectiousDiseaseV4Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new IntermediateInfectiousDiseaseSurrogateModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, null, null, "",
                    "", null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumIntermediateMonthInfectiousDiseaseV4(model));
            }
        }

        [TestMethod] 
        public void PdfHumIntermediateMonthInfectiousDiseaseV5Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new IntermediateInfectiousDiseaseSurrogateModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, null, null, "",
                    "", null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumIntermediateMonthInfectiousDiseaseV5(model));
            }
        }

        [TestMethod]
        public void PdfHumIntermediateMonthInfectiousDiseaseV6Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new IntermediateInfectiousDiseaseSurrogateModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, null, null, "",
                    "", null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumIntermediateMonthInfectiousDiseaseV6(model));
            }
        }

        [TestMethod]
        public void PdfHumIntermediateMonthInfectiousDiseaseV61Test()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new IntermediateInfectiousDiseaseSurrogateModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, null, null, "",
                    "", null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumIntermediateMonthInfectiousDiseaseV61(model));
            }
        }
        [TestMethod] 
        public void PdfHumAnnualInfectiousDiseaseTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new AnnualInfectiousDiseaseModel("en", 2012, false);
                AssertPDF(wrapper.Client.ExportHumAnnualInfectiousDisease(model));
            }
        }

        [TestMethod] 
        public void PdfHumIntermediateAnnualInfectiousDiseaseTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new IntermediateInfectiousDiseaseSurrogateModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, null, null, "",
                    "", null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumIntermediateAnnualInfectiousDisease(model));
            }
        }

        [TestMethod]
        public void PdfHumComparativeGGReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new ComparativeGGSurrogateModel("en", null,null,null,null, 2015,2016, 1,2, null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumComparativeGGReport(model));
            }
        }

        [TestMethod]
        public void PdfExportHumComparativeSeveralYearsGGReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new ComparativeGGSeveralYearsSurrogateModel("en", 2015,2016, new []{"1","2"}, new string[0], "xxx", null, null, null,null, null, new List<PersonalDataGroup>(), false);
                AssertPDF(wrapper.Client.ExportHumComparativeSeveralYearsGGReport(model));
            }
        }


        [TestMethod]
        public void PdfHumBorderRayonsComparativeTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumBorderRayonsComparativeReport(new BorderRayonsSurrogateModel("en", 2015, null, null, null, null,
                        null, null, new string[0], new string[0], null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod]
        public void PdfHumTuberculosisCasesTestedTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumTuberculosisCasesTested(new TuberculosisSurrogateModel("en", null, null, new[] {"2014"}, null,
                        null, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod]
        public void PdfComparativeSeveralYearsTHReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumComparativeReportOfSeveralYearsTH(new ComparativeSeveralYearsTHSurrogateModel("en", 2014, 2015,
                        new string[0], 0, null, null, null, null, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod]
        public void PdfNumberOfCasesDeathsMorbidityMortalityTHTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumNumberOfCasesDeathsMorbidityMortalityTH(new NumberOfCasesDeathsMorbidityMortalityTHModel("en",
                        DateTime.Now.AddYears(-1), DateTime.Now, null, null, null, null, null, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod]
        public void PdfNumberOfCasesDeathsMonthTHTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumNumberOfCasesDeathsMonthTH(new NumberOfCasesDeathsMonthTHModel("en", 2015, null, null, null,
                        null, null, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod]
        public void PdfExportHumFormN85ARMTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportHumFormN85ARM(new FormN85SurrogateModel("en", 2015, 1, null, null, "xx", "", "xxx", null,
                        new List<PersonalDataGroup>(), false)));
            }
        }

        #endregion

        #region PDF Zoonotic reports

        [TestMethod]
        public void PdfZoonoticComparativeAZTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportZoonoticComparativeAZ(new ZoonoticSurrogateModel("en", 2014, null, null, null, null, null, null,
                        null, new List<PersonalDataGroup>(), false)));
            }
        }

        #endregion

        #region PDF Veterinary reports

        [TestMethod] 
        public void PdfVetAggregateCaseTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetAggregateCase(new AggregateModel("en", 1, 6707800000000, 2, false)));
            }
        }

        [TestMethod] 
        public void PdfVetAggregateCaseSummaryTest()
        {
            const string aggrXml =
                @"<?xml version=""1.0"" encoding=""UTF-16""?><ROOT><AdminLevel AreaType=""satCountry""><AdminUnit AdminUnitID=""170000000"" /></AdminLevel><TimeInterval PeriodType=""sptYear""><TimeIntervalUnit StartDate=""2009-01-01"" FinishDate=""2011-12-31"" /><TimeIntervalUnit StartDate=""2008-01-01"" FinishDate=""2008-12-31"" /></TimeInterval></ROOT>";
            var observations = new List<long> {1, 2};
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetAggregateCaseSummary(
                    new AggregateSummaryModel("en", aggrXml, observations, false)));
            }
        }

        [TestMethod] 
        public void PdfVetAggregateCaseActionsTest()
        {
            var labels = new Dictionary<string, string>
            {
                {"@diagnosticTexten", "x"},
                {"@sanitaryTexten", "y"},
                {"@prophylacticTexten", "z"}
            };
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetAggregateCaseActions(
                    new AggregateActionsModel("en", 1, 6707800000000, 3, 6707800000000, 5, 6707800000000, 7, labels, false)));
            }
        }

        [TestMethod] 
        public void PdfVetAggregateCaseActionsSummaryTest()
        {
            const string aggrXml =
                @"<?xml version=""1.0"" encoding=""UTF-16""?><ROOT><AdminLevel AreaType=""satCountry""><AdminUnit AdminUnitID=""170000000"" /></AdminLevel><TimeInterval PeriodType=""sptYear""><TimeIntervalUnit StartDate=""2009-01-01"" FinishDate=""2011-12-31"" /><TimeIntervalUnit StartDate=""2008-01-01"" FinishDate=""2008-12-31"" /></TimeInterval></ROOT>";
            var observations = new long[] {1, 2};
            var labels = new Dictionary<string, string>
            {
                {"@diagnosticTexten", "x"},
                {"@sanitaryTexten", "y"},
                {"@prophylacticTexten", "z"}
            };
            using (var wrapper = new ReportClientWrapper())
            {
                var ps = new AggregateActionsSummaryModel("en", aggrXml, observations, observations, observations, labels, false);
                AssertPDF(wrapper.Client.ExportVetAggregateCaseActionsSummary(ps));
            }
        }

        [TestMethod] 
        public void PdfVetAvianInvestigationTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetAvianInvestigation(new VetIdModel("en", 1, 2, false, false)));
            }
        }

        [TestMethod] 
        public void PdfVetLivestockInvestigationTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetLivestockInvestigation(new VetIdModel("en", 1, 2, false, false)));
            }
        }

        [TestMethod] 
        public void PdfVetActiveSurveillanceSampleCollectedTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetActiveSurveillanceSampleCollected(new BaseIdModel("en", 1, false)));
            }
        }

        [TestMethod] 
        public void PdfVetSamplesCountTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetSamplesCount(new BaseYearModel("en", 2012, false)));
            }
        }

        [TestMethod] 
        public void PdfExportVetSamplesBySampleTypeTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetSamplesBySampleType(new BaseYearModel("en", 2012, false)));
            }
        }

        [TestMethod] 
        public void PdfExportVetSamplesBySampleTypesWithinRegionsTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetSamplesBySampleTypesWithinRegions(
                    new BaseYearModel("en", 2012, false)));
            }
        }

        [TestMethod] 
        public void PdfVetYearlySituationTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetYearlySituation(new BaseYearModel("en", 2012, false)));
            }
        }

        [TestMethod] 
        public void PdfVetRabiesBulletinEuropeTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(
                    wrapper.Client.ExportVetRabiesBulletinEurope(new RBESurrogateModel("en", 2012, new QuartersModel(), new string[0],
                        new string[0], true, false)));
            }
        }

        [TestMethod] 
        public void PdfInfectiousParasiticKZTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new InfectiousParasiticKZSurrogateModel("en", 2012, 1, 10, null, null, null, null, null,
                    new List<PersonalDataGroup>(), false);

                AssertPDF(wrapper.Client.ExportInfectiousParasiticKZReport(model));
            }
        }

        [TestMethod] 
        public void PdfComparativeIQReportTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportHumComparativeIQReport(
                    new ComparativeSurrogateModel("en", 1, 1, 10, 2010, 2012,
                        null, null, null, null, 1, null, new List<PersonalDataGroup>(), false)));
            }
        }

        [TestMethod] 
        public void PdfWeeklySituationDiseasesByAgeGroupAndSexIQTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new SituationOnInfectiousDiseasesSurrogateModel("en",
                    2012, StatisticPeriodType.Week, 1,
                    new DateTime(2015, 01, 12), new DateTime(2015, 01, 18),
                    39520000000, "Baghdad", false);

                AssertPDF(wrapper.Client.ExportWeeklySituationDiseasesByAgeGroupAndSex(model));
            }
        }

        [TestMethod] 
        public void PdfWeeklySituationDiseasesByDistrictsIQTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new SituationOnInfectiousDiseasesSurrogateModel("en",
                    2012, StatisticPeriodType.Week, 1,
                    new DateTime(2015, 01, 12), new DateTime(2015, 01, 18),
                    39520000000, "Baghdad", false);

                AssertPDF(wrapper.Client.ExportWeeklySituationDiseasesByDistricts(model));
            }
        }

        /*
        [TestMethod] 
        public void PdfVetPlannedDiagnosticTest()
        {
            var kzPdDiagnosis = new[] {"6618180000000"};
            var kzPdSpecies = new[] {"6619290000000", "952120000000"};
            var kzPdInvestigation = new[] {"837780000000", "837840000000"};

            using (var wrapper = new ReportClientWrapper())
            {
                var model = new DiagnosticInvestigationModel("ru", DateTime.Now.AddMonths(-1), DateTime.Now, null,
                                                             kzPdDiagnosis, kzPdInvestigation, kzPdSpecies, false);
                AssertPDF(wrapper.Client.ExportVetCountryPlannedDiagnosticTests(model));

                model.RegionFilter.RegionId = 41180000000;
                AssertPDF(wrapper.Client.ExportVetCountryPlannedDiagnosticTests(model));
            }
        }

        [TestMethod] 
        public void PdfVetPreventiveMeasuresTest()
        {
            var kzPmDiagnosis = new[] {"6618460000000"};
            var kzPmSpecies = new[] {"838110000000"};
            var kzPmMeasures = new[] {"952180000000"};
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new ProphylacticModel("ru", DateTime.Now.AddMonths(-1), DateTime.Now, null,
                                                  kzPmDiagnosis, kzPmSpecies, kzPmMeasures, false);

                AssertPDF(wrapper.Client.ExportVetCountryPreventiveMeasures(model));

                model.RegionFilter.RegionId = 41180000000;
                AssertPDF(wrapper.Client.ExportVetCountryPreventiveMeasures(model));
            }
        }

        [TestMethod] 
        public void PdfVetSanitaryMeasuresTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new SanitaryModel("ru", DateTime.Now.AddMonths(-1), DateTime.Now, null, new[] {"952220000000"}, false);

                AssertPDF(wrapper.Client.ExportVetCountrySanitaryMeasures(model));

                model.RegionFilter.RegionId = 41180000000;
                AssertPDF(wrapper.Client.ExportVetCountrySanitaryMeasures(model));
            }
        }
        */

        [TestMethod] 
        public void PdfVetActiveSurveillanceTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportVetActiveSurveillance(new BaseYearModel("en", 2012, false)));
            }
        }

        #endregion

        #region  Lab module

        [TestMethod] 
        public void PdfLimTestResultTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimTestResult(new LimTestResultModel("en", 1, 2, 3, false)));
            }
        }

        [TestMethod] 
        public void PdfLimSampleTransferTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimSampleTransfer(new BaseIdModel("en", 1, false)));
            }
        }

        [TestMethod] 
        public void PdfLimTestTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimTest(new LimTestModel("en", 1, true, false)));
            }
        }

        [TestMethod] 
        public void PdfLimBatchTestTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimBatchTest(new LimBatchTestModel("en", 1, 2, false)));
            }
        }

        [TestMethod] 
        public void PdfLimContainerDetailsTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimSample(new BaseIdModel("en", 1, false)));
            }
        }

        [TestMethod] 
        public void PdfLimContainerContentPdfTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimContainerContent(
                    new LimContainerContentModel("en", 1, null, false)));
            }
        }

        [TestMethod] 
        public void PdfLimAccessionInTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimAccessionIn(new BaseIdModel("en", 1, false)));
            }
        }

        [TestMethod] 
        public void PdfLimSampleDestructionTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportLimSampleDestruction(new IdListModel("en", new long[] {1, 2}, false)));
            }
        }

        [TestMethod] 
        public void PdfLaboratoryResearchResultTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                var model = new VetLaboratoryResearchResultModel("en", "xxx", null, "aaa", "bbb", "ccc", false);
                AssertPDF(wrapper.Client.ExportVetLaboratoryResearchResult(model));
            }
        }

        #endregion

        [TestMethod] 
        public void ExportNewBarcodesPdfTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportNewBarcodes((long) NumberingObjectEnum.HumanCase, 5));
            }
        }

        [TestMethod] 
        public void ExportExistingBarcodesPdfTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportExistingBarcodes((long) NumberingObjectEnum.Sample, new[] {50856020000000}));
            }
        }

        [TestMethod] 
        public void UniOutbreakPdfTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportOutbreak(new BaseIdModel("en", 1, false)));
            }
        }

        [TestMethod] 
        public void AdmEventLogPdfTest()
        {
            using (var wrapper = new ReportClientWrapper())
            {
                AssertPDF(wrapper.Client.ExportAdmEventLog(
                    new BaseIntervalModel("en", DateTime.Now.AddMonths(-1), DateTime.Now, false)));
            }
        }

        [TestMethod] 
        public void PdfParallelAccessTest()
        {
            List<Task> tasks = CreateTasks();
            foreach (Task task in tasks)
            {
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
        }

        [TestMethod] 
        public void ReportControllerMethodChainTests()
        {
            var controller = new ReportController();

            List<MethodInfo> methodInfos =
                controller.GetType().GetMethods().Where(
                    m => !m.GetParameters().Any() && typeof(ActionResult).IsAssignableFrom(m.ReturnType)
                    ).ToList();

            Console.WriteLine("Test of {0} Actions of ReportController started", methodInfos.Count);
            foreach (MethodInfo info in methodInfos)
            {
                info.Invoke(controller, new object[0]);
                Assert.IsNotNull(controller.ViewBag.Title, string.Format("Report title not set in mehtod {0}", info.Name));
                Assert.IsNotNull(controller.ViewBag.GetReportActionName, string.Format("Report name not set in mehtod {0}", info.Name));

                string actionName = controller.ViewBag.GetReportActionName.ToString();
                MethodInfo action = controller.GetType().GetMethod(actionName);
                Assert.IsNotNull(action, string.Format("Controller hasn't action {0}", actionName));

                ParameterInfo[] parameterInfos = action.GetParameters();
                Assert.IsTrue(parameterInfos.Length == 1, string.Format("Controller action {0} should have 1 parameter", actionName));

                var parameter = Activator.CreateInstance(parameterInfos[0].ParameterType) as BaseModel;
                Assert.IsNotNull(parameter,
                    string.Format("parameter of mehtod {0} has wrong type {1}", actionName, parameterInfos[0].ParameterType));
                parameter.Language = "en";
                parameter.ExportFormat = "Pdf";
                object result = action.Invoke(controller, new[] { (object)parameter });
                Assert.IsNotNull(result, string.Format("Controller action {0} return null value", actionName));
            }
        }

        private static List<Task> CreateTasks()
        {
            const string aggrXml =
                @"<?xml version=""1.0"" encoding=""UTF-16""?><ROOT><AdminLevel AreaType=""satCountry""><AdminUnit AdminUnitID=""170000000"" /></AdminLevel><TimeInterval PeriodType=""sptYear""><TimeIntervalUnit StartDate=""2009-01-01"" FinishDate=""2011-12-31"" /><TimeIntervalUnit StartDate=""2008-01-01"" FinishDate=""2008-12-31"" /></TimeInterval></ROOT>";
            var observations = new List<long> {1, 2};
            var labels = new Dictionary<string, string>
            {
                {"@diagnosticTexten", "x"},
                {"@sanitaryTexten", "y"},
                {"@prophylacticTexten", "z"}
            };

            Action humAggregateCase =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportHumAggregateCase(new AggregateModel("en", 1, 6707800000000, 2, false)),
                        "PdfHumAggregateCase");

            Action humAggregateCaseSummary =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportHumAggregateCaseSummary(
                                new AggregateSummaryModel("en", aggrXml, observations, false)),
                        "PdfHumAggregateCaseSummary");
            Action humUrgentyNotification =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportHumUrgentyNotification(new BaseIdModel("en", 45820000633, false)),
                        "PdfHumUrgentyNotification");

            Action humCaseInvestigation =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportHumCaseInvestigation(new HumIdModel("en", 1, 2, 3, 4, false)),
                        "PdfHumCaseInvestigation");

            Action vetAggregateCase =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportVetAggregateCase(new AggregateModel("en", 1, 6707800000000, 2, false)),
                        "PdfVetAggregateCase");

            Action vetAggregateCaseActions =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportVetAggregateCaseActions(
                                new AggregateActionsModel("en", 1, 45820000633, 3, 45820000633, 5, 45820000633, 7, labels, false)),
                        "PdfVetAggregateCaseActions");

            Action vetAggregateCaseSummary =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportVetAggregateCaseSummary(
                                new AggregateSummaryModel("en", aggrXml, observations, false)),
                        "PdfVetAggregateCaseSummary");

            var asm = new AggregateActionsSummaryModel("en", aggrXml, observations, observations, observations, labels, false);
            Action vetAggregateCaseActionsSummary =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportVetAggregateCaseActionsSummary(asm),
                        "PdfVetAggregateCaseActionsSummary");

            Action vetAvianInvestigation =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportVetAvianInvestigation(new VetIdModel("en", 1, 2, false, false)),
                        "PdfVetAvianInvestigation");

            Action vetLivestockInvestigation =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportVetLivestockInvestigation(new VetIdModel("en", 1, 2, false, false)),
                        "PdfVetLivestockInvestigation");

            Action vetActiveSurveillanceSampleCollected =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportVetActiveSurveillanceSampleCollected(new BaseIdModel("en", 1, false)),
                        "PdfVetActiveSurveillanceSampleCollected");

            Action limTestResult =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportLimTestResult(new LimTestResultModel("en", 1, 2, 3, false)),
                        "PdfLimTestResult");

            Action limSampleTransfer =
                () =>
                    AssertPdfAction(wrapper => wrapper.Client.ExportLimSampleTransfer(new BaseIdModel("en", 1, false)),
                        "PdfLimSampleTransfer");

            Action limTest =
                () =>
                    AssertPdfAction(wrapper => wrapper.Client.ExportLimTest(new LimTestModel("en", 1, true, false)),
                        "PdfLimTest");

            Action batchTest =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportLimBatchTest(new LimBatchTestModel("en", 1, 2, false)),
                        "PdfLimBatchTest");

            Action containerDetails =
                () =>
                    AssertPdfAction(
                        wrapper => wrapper.Client.ExportLimSample(new BaseIdModel("en", 1, false)),
                        "PdfLimSample");

            Action containerContent =
                () =>
                    AssertPdfAction(
                        wrapper =>
                            wrapper.Client.ExportLimContainerContent(new LimContainerContentModel("en", 1, null, false)),
                        "PdfLimContainerContent");

            Action limAccessionIn =
                () =>
                    AssertPdfAction(wrapper => wrapper.Client.ExportLimAccessionIn(new BaseIdModel("en", 1, false)),
                        "PdfLimAccessionIn");

            Action outbreak =
                () =>
                    AssertPdfAction(wrapper => wrapper.Client.ExportOutbreak(new BaseIdModel("en", 1, false)),
                        "PdfOutbreak");

            var tasks = new List<Task>
            {
                new Task(outbreak),
                new Task(humAggregateCase),
                new Task(humAggregateCaseSummary),
                new Task(humUrgentyNotification),
                new Task(humCaseInvestigation),
                new Task(vetAggregateCase),
                new Task(vetAggregateCaseSummary),
                new Task(vetAggregateCaseActions),
                new Task(vetAggregateCaseActionsSummary),
                new Task(vetAvianInvestigation),
                new Task(vetLivestockInvestigation),
                new Task(vetActiveSurveillanceSampleCollected),
                new Task(limTestResult),
                new Task(limSampleTransfer),
                new Task(limTest),
                new Task(batchTest),
                new Task(containerDetails),
                new Task(containerContent),
                new Task(limAccessionIn)
            };

            return tasks;
        }

        private static void AssertPdfAction(Func<ReportClientWrapper, byte[]> pdfReportAction, string reportName)
        {
            int count;
            lock (m_SyncRoot)
            {
                m_TotalCount++;
                count = m_TotalCount;
            }
            DateTime oldTime = DateTime.Now;
            Console.WriteLine("->{0} {1} called.", count, reportName);
            using (var wrapper = new ReportClientWrapper())
            {
                byte[] pdfHumUrgentyNotification = pdfReportAction(wrapper);
                AssertPDF(pdfHumUrgentyNotification);
            }
            Console.WriteLine("<-{0} {1} received. Executing time: {2}", count, reportName, DateTime.Now - oldTime);
        }

        public static void AssertPDF(byte[] bytes)
        {
            ExportTests.AssertPDF(bytes);
        }
    }
}