using System.Collections.Generic;
using bv.model.BLToolkit;
using eidss.model.Enums;
using eidss.model.Reports;
using EIDSS.Reports.BaseControls.FlexFormIntegration;
using EIDSS.Reports.BaseControls.Report;
using EIDSS.Reports.Factory;
using EIDSS.Reports.Flexible;

namespace EIDSS.Reports.Document.Veterinary.Aggregate
{
    [NullableAdapters]
    public sealed partial class VetAggregateReport : BaseDocumentReport
    {
        private const int DeltaHeight = 12;
        private const int DeltaWidth = 8;

        public VetAggregateReport()
        {
            InitializeComponent();
        }

        public override void SetParameters
            (string lang, Dictionary<string, string> parameters, DbManagerProxy manager, DbManagerProxy archiveManager)
        {
            tableInterval.Left = lblReportName.Left;
            tableInterval.Width = lblReportName.Width;
            lblUnit.Left = lblReportName.Left;
            lblUnit.Width = lblReportName.Width;

            base.SetParameters(lang, parameters, manager, archiveManager);

            var sourceTable = new FlexParamDataSet();
            FlexConverter.FillCaseTable(manager, sourceTable, parameters, lang);

            bool hasData = sourceTable.tblCase.Rows.Count > 0;
            if (hasData)
            {
                FlexParamDataSet.tblCaseRow firstRow = sourceTable.tblCase[0];
                cellInputStartDate.Text = (firstRow.datStartDate).ToShortDateString();
                cellInputEndDate.Text = (firstRow.datFinishDate).ToShortDateString();
                lblUnit.Text = firstRow.strAdmUnitName;

                CaseIdCell.Text = firstRow.strCaseID;
                CaseIdBarcodeCell.Text = m_BarCode.Code128(firstRow.strCaseID);
                PlaceCell.Text = GetFullLocationFromAdmUnitId(firstRow.idfsAdministrativeUnit, (GisReferenceType) firstRow.idfsAdmUnitType);
            }
            PageHeader.Visible = false;
            tableInterval.Visible = hasData;
            lblUnit.Visible = hasData;
            CaseIdCell.Visible = hasData;
            CaseIdBarcodeCell.Visible = hasData;
            PlaceCell.Visible = hasData;

            AjustLeftHeaderHeight(DeltaHeight);

            long observationId = (GetLongParameter(parameters, "@observationId"));
            long idFormTemplate = (GetLongParameter(parameters, "@idFormTemplate"));
            FlexFactory.CreateVetAggregateReport(FlexSubreport, idFormTemplate, observationId, tableBaseHeader.Width - DeltaWidth);

            ReportRebinder.RebindDateAndFontForReport();
            ReportRtlHelper.SetRTL(this);
        }
    }
}