using System;
using System.ComponentModel;
using bv.model.BLToolkit;
using eidss.model.Reports.KZ;
using EIDSS.Reports.BaseControls.Filters;
using EIDSS.Reports.BaseControls.Keeper;
using EIDSS.Reports.BaseControls.Report;

namespace EIDSS.Reports.Parameterized.Veterinary.KZ.Keepers
{
    public partial class VetPlannedDiagnosticKeeper : BaseIntervalKeeper
    {
        private readonly ComponentResourceManager m_Resources =
            new ComponentResourceManager(typeof (VetPlannedDiagnosticKeeper));

        private string[] m_Diagnosis;
        private string[] m_InvestigationType;
        private string[] m_Species;
        private long? m_Region;

        internal VetPlannedDiagnosticKeeper()
        {
            InitializeComponent();
        }

        public VetPlannedDiagnosticKeeper(Type reportType) : base(reportType)
        {
            InitializeComponent();
            dtEnd.EditValue = TruncateDate(DateTime.Now);
            dtStart.EditValue = TruncateDate(DateTime.Now.AddMonths(-1));

            if (ReportType == typeof (VetCountryPlannedDiagnostic))
            {
                regionFilter1.Hide();
            }
            regionFilter1.SetMandatory();
        }

        protected override BaseReport GenerateReport(DbManagerProxy manager, DbManagerProxy archiveManager)
        {
            var model = new DiagnosticInvestigationModel(CurrentCulture.ShortName, StartDateTruncated, EndDateTruncated, m_Region,
                m_Diagnosis, m_Species, m_InvestigationType, UseArchive);
            dynamic report = CreateReportObject();
            report.SetParameters( model,manager,archiveManager);
            return report;
        }

        protected internal override void ApplyResources(DbManagerProxy manager)
        {
            base.ApplyResources(manager);
            m_Resources.ApplyResources(pnlSettings, "pnlSettings");
            m_Resources.ApplyResources(this, "$this");

            regionFilter1.DefineBinding();
            DiagnosisMatrixFilter1.DefineBinding();
            investigationTypeFilter1.DefineBinding();
            speciesFilter1.DefineBinding();

            m_Resources.ApplyResources(DiagnosisMatrixFilter1, "DiagnosisMatrixFilter1");
            m_Resources.ApplyResources(regionFilter1, "regionFilter1");
            m_Resources.ApplyResources(speciesFilter1, "speciesFilter1");
            m_Resources.ApplyResources(investigationTypeFilter1, "investigationTypeFilter1");
        }

        private void regionFilter1_ValueChanged(object sender, SingleFilterEventArgs e)
        {
            m_Region = regionFilter1.RegionId > 0 ? (long?) regionFilter1.RegionId : null;
        }

        private void DiagnosisMatrixFilter1_ValueChanged(object sender, MultiFilterEventArgs e)
        {
            m_Diagnosis = e.KeyArray;
        }

        private void investigationTypeFilter1_ValueChanged(object sender, MultiFilterEventArgs e)
        {
            m_InvestigationType = e.KeyArray;
        }

        private void speciesFilter1_ValueChanged(object sender, MultiFilterEventArgs e)
        {
            m_Species = e.KeyArray;
        }
    }
}