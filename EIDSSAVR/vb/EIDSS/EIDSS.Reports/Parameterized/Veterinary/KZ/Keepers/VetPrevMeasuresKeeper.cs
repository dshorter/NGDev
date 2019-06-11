using System;
using bv.model.BLToolkit;
using eidss.model.Reports.KZ;
using EIDSS.Reports.BaseControls.Filters;
using EIDSS.Reports.BaseControls.Keeper;
using EIDSS.Reports.BaseControls.Report;

namespace EIDSS.Reports.Parameterized.Veterinary.KZ.Keepers
{
    public partial class VetPrevMeasuresKeeper : BaseIntervalKeeper
    {
        private long? m_Region;

        private string[] m_Diagnosis;
        private string[] m_MeasureType;
        private string[] m_Species;

        public VetPrevMeasuresKeeper()
        {
            InitializeComponent();
        }

        public VetPrevMeasuresKeeper(Type reportType)
            : base(reportType)
        {
            InitializeComponent();
            dtEnd.EditValue = TruncateDate(DateTime.Now);
            dtStart.EditValue = TruncateDate(DateTime.Now.AddMonths(-1));

            if (ReportType == typeof (VetCountryPreventiveMeasures))
            {
                regionFilter1.Hide();
            }
            regionFilter1.SetMandatory();
        }

        protected override BaseReport GenerateReport(DbManagerProxy manager, DbManagerProxy archiveManager)
        {
            var model = new ProphylacticModel(CurrentCulture.ShortName, StartDateTruncated, EndDateTruncated, m_Region,
                m_Diagnosis, m_Species, m_MeasureType, UseArchive);
            dynamic report = CreateReportObject();
            report.SetParameters( model,manager,archiveManager);
            return report;
        }

        protected internal override void ApplyResources(DbManagerProxy manager)
        {
            base.ApplyResources(manager);

            regionFilter1.DefineBinding();
            DiagnosisMatrixFilter1.DefineBinding();
            measureTypeFilter1.DefineBinding();
            speciesFilter1.DefineBinding();
        }

        private void regionFilter1_ValueChanged(object sender, SingleFilterEventArgs e)
        {
            m_Region = regionFilter1.RegionId > 0 ? (long?) regionFilter1.RegionId : null;
        }

        private void DiagnosisMatrixFilter1_ValueChanged(object sender, MultiFilterEventArgs e)
        {
            m_Diagnosis = e.KeyArray;
        }

        private void measureTypeFilter1_ValueChanged(object sender, MultiFilterEventArgs e)
        {
            m_MeasureType = e.KeyArray;
        }

        private void speciesFilter1_ValueChanged(object sender, MultiFilterEventArgs e)
        {
            m_Species = e.KeyArray;
        }
    }
}