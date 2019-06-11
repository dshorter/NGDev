using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using bv.model.BLToolkit;
using DevExpress.XtraPrinting;
using eidss.model.Reports;
using eidss.model.Reports.AZ;
using eidss.model.Reports.Common;
using EIDSS.Reports.BaseControls.Report;
using EIDSS.Reports.Parameterized.Human.AJ.DataSets;

namespace EIDSS.Reports.Parameterized.Human.AJ.Reports
{
    [CanWorkWithArchive]
    public sealed partial class FormVet1 : BaseReport
    {
        private bool m_IsFirstRow = true;
        private bool m_IsPrintGroup = true;
        private int m_DiagnosisCounter;

        public FormVet1()
        {
            InitializeComponent();
        }

        public void SetParameters(VetCasesSurrogateModel model, DbManagerProxy manager, DbManagerProxy archiveManager)
        {
            SetLanguage(model, manager);

            DateTimeCell.Text = ReportRebinder.ToDateTimeString(DateTime.Now);

            ForReportPeriod.Text = VetCaseReport.GetPeriodText(model);

            var location = AddressModel.GetLocation(model.Language,
                m_BaseDataSet.sprepGetBaseParameters[0].CountryName,
                model.RegionId, model.RegionName,
                model.RayonId, model.RayonName);
            SentBy.Text = model.OrganizationEnteredById.HasValue 
                ? string.Format("{0} ({1})", model.OrganizationEnteredByName, location) 
                : location;

            m_DiagnosisCounter = 1;

            VetForm1ReportDataSet.spRepVetForm1ReportAZDataTable dataTable = m_DataSet.spRepVetForm1ReportAZ;
            Action<SqlConnection, SqlTransaction> action = ((connection, transaction) =>
            {
                m_DataAdapter.Connection = connection;
                m_DataAdapter.Transaction = transaction;
                m_DataAdapter.CommandTimeout = CommandTimeout;

                m_DataSet.EnforceConstraints = false;

                m_DataAdapter.Fill(dataTable, model.Language,
                    model.StartYear, model.EndYear,
                    model.StartMonth, model.EndMonth,
                    model.RegionId, model.RayonId, model.OrganizationEnteredById, model.OrganizationId, model.UserId);
            });
            FillDataTableWithArchive(action, RemoveEmptyRowsIfRealDataExists,
                manager, archiveManager,
                dataTable,
                model.Mode,
                new[] {"strDiagnosisSpeciesKey", "strOIECode", "idfsDiagnosis"},
                new[] {"DiagnosisOrderColumn", "SpeciesOrderColumn"});

            dataTable.DefaultView.Sort = " strDiagnosisName, SpeciesOrderColumn, strSpecies";
            if (dataTable.Count > 0)
            {
                //SentBy.Text = dataTable[0].strHeaderSentBy;
                PerformerCell.Text = dataTable[0].strFooterPerformer;
                if (dataTable.FirstOrDefault(r => r.idfsDiagnosis < 0) == null)
                {
                    RowNumberCell.Text = 1.ToString();
                }
            }
        }

        private void RemoveEmptyRowsIfRealDataExists(DataTable archiveData, DataTable sourceData)
        {
            VetForm1ReportDataSet.spRepVetForm1ReportAZRow archiveRow =
                ((VetForm1ReportDataSet.spRepVetForm1ReportAZDataTable) archiveData).FirstOrDefault(r => r.idfsDiagnosis < 0);
            VetForm1ReportDataSet.spRepVetForm1ReportAZRow sourceRow =
                ((VetForm1ReportDataSet.spRepVetForm1ReportAZDataTable) sourceData).FirstOrDefault(r => r.idfsDiagnosis < 0);
            if (archiveRow == null && sourceRow != null)
            {
                sourceData.Rows.Remove(sourceRow);
            }
            if (archiveRow != null && sourceRow == null)
            {
                archiveData.Rows.Remove(archiveRow);
            }
        }

        private void GroupFooterDiagnosis_BeforePrint(object sender, PrintEventArgs e)
        {
            m_IsPrintGroup = true;

            m_DiagnosisCounter++;
            RowNumberCell.Text = m_DiagnosisCounter.ToString();
        }

        private void RowNumberCell_BeforePrint(object sender, PrintEventArgs e)
        {
            VetCaseReport.AjustGroupBorders(RowNumberCell, m_IsPrintGroup, m_DiagnosisCounter > 1);
            VetCaseReport.AjustGroupBorders(DiseaseCell, m_IsPrintGroup, m_DiagnosisCounter > 1);
            VetCaseReport.AjustGroupBorders(OIECell, m_IsPrintGroup, m_DiagnosisCounter > 1);

            m_IsPrintGroup = false;
        }

        private void SpeciesCell_BeforePrint(object sender, PrintEventArgs e)
        {
            if (!m_IsFirstRow)
            {
                SpeciesCell.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                NumberSpeciesCell.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                UnhealthyStations.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                NumberSick.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                NumberDead.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                NumberVaccinated.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                OtherMeasures.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                Annihilated.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                Slaughtered.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                UnhealthyStationsLeft.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
                NumberDiseased.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Right;
            }
            m_IsFirstRow = false;
        }
    }
}