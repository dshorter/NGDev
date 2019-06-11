﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using bv.model.BLToolkit;
using bv.winclient.Layout;
using DevExpress.XtraEditors.Controls;
using eidss.model.Reports.AZ;
using eidss.model.Reports.Common;
using eidss.model.Reports.OperationContext;
using EIDSS.Reports.BaseControls.Filters;
using EIDSS.Reports.BaseControls.Keeper;
using EIDSS.Reports.BaseControls.Report;
using EIDSS.Reports.Parameterized.Human.AJ.Reports;

namespace EIDSS.Reports.Parameterized.Human.AJ.Keepers
{
    public partial class VetKeeper : BaseReportKeeper
    {
        private readonly ComponentResourceManager m_Resources = new ComponentResourceManager(typeof (VetKeeper));

        private List<ItemWrapper> m_MonthCollection;

        internal VetKeeper(VetReportType vetReportType)
            : base(new Dictionary<string, string>())
        {
            switch (vetReportType)
            {
                case VetReportType.Case:
                    ReportType = typeof (VetCaseReport);
                    break;
                case VetReportType.FormVet1:
                    ReportType = typeof (FormVet1);
                    break;
                case VetReportType.FormVet1A:
                    ReportType = typeof (FormVet1A);
                    break;
                default:
                    throw new ApplicationException(string.Format("Not supported report type : {0}", vetReportType));
            }
            InitializeComponent();
            
            using (ContextKeeper.CreateNewContext(ContextValue.ReportFilterLoading))

            {
                organizationFilter.ReportType = vetReportType;
                LayoutCorrector.SetStyleController(FromYearSpinEdit, LayoutCorrector.MandatoryStyleController);
                LayoutCorrector.SetStyleController(ToYearSpinEdit, LayoutCorrector.MandatoryStyleController);

                FromYearSpinEdit.Value = DateTime.Now.Year;
                ToYearSpinEdit.Value = DateTime.Now.Year;

                FromYearSpinEdit.Properties.MinValue = 2000;
                ToYearSpinEdit.Properties.MinValue = 2000;
                FromYearSpinEdit.Properties.MaxValue = DateTime.Now.Year;
                ToYearSpinEdit.Properties.MaxValue = DateTime.Now.Year;

                BindLookup(FromMonthLookup, MonthCollection, FromMonthLabel.Text);
                FromMonthLookup.EditValue = MonthCollection[DateTime.Now.Month - 1];

                BindLookup(ToMonthLookup, MonthCollection, ToMonthLabel.Text);
                ToMonthLookup.EditValue = MonthCollection[DateTime.Now.Month - 1];
            }
            m_HasLoad = true;
        }

        #region Properties

        [Browsable(false)]
        protected long? RegionID
        {
            get { return regionFilter.RegionId > 0 ? (long?) regionFilter.RegionId : null; }
        }

        [Browsable(false)]
        protected long? RayonID
        {
            get { return rayonFilter.RayonId > 0 ? (long?) rayonFilter.RayonId : null; }
        }

        [Browsable(false)]
        protected long? OrganizationID
        {
            get { return organizationFilter.EditValueId > 0 ? (long?) organizationFilter.EditValueId : null; }
        }

        [Browsable(false)]
        protected int FromYearParam
        {
            get { return (int) FromYearSpinEdit.Value; }
        }

        [Browsable(false)]
        protected int ToYearParam
        {
            get { return (int) ToYearSpinEdit.Value; }
        }

        [Browsable(false)]
        protected int? FromMonthParam
        {
            get
            {
                return (FromMonthLookup.EditValue == null)
                    ? (int?) null
                    : ((ItemWrapper) FromMonthLookup.EditValue).Number;
            }
        }

        [Browsable(false)]
        protected int? ToMonthParam
        {
            get
            {
                return (ToMonthLookup.EditValue == null)
                    ? (int?) null
                    : ((ItemWrapper) ToMonthLookup.EditValue).Number;
            }
        }

        private List<ItemWrapper> MonthCollection
        {
            get { return m_MonthCollection ?? (m_MonthCollection = FilterHelper.GetWinMonthList()); }
        }

        #endregion

        protected override BaseReport GenerateReport(DbManagerProxy manager, DbManagerProxy archiveManager)
        {
            var organization = GetOrganization(manager);

            var model = new VetCasesSurrogateModel(CurrentCulture.ShortName,
                RegionID, RayonID, regionFilter.SelectedText, rayonFilter.SelectedText,
                OrganizationID, organization,
                FromYearParam, ToYearParam, FromMonthParam, ToMonthParam,
                UseArchive);

            dynamic vetCaseReport = CreateReportObject();
            vetCaseReport.SetParameters(model, manager, archiveManager);
            return (BaseReport) vetCaseReport;
        }

        protected internal override void ApplyResources(DbManagerProxy manager)
        {
            try
            {
                IsResourceLoading = true;
                m_MonthCollection = null;

                m_HasLoad = false;

                base.ApplyResources(manager);

                m_Resources.ApplyResources(FromMonthLabel, "FromMonthLabel");
                m_Resources.ApplyResources(ToMonthLabel, "ToMonthLabel");
                m_Resources.ApplyResources(FromYearLabel, "FromYearLabel");
                m_Resources.ApplyResources(ToYearLabel, "ToYearLabel");
                ApplyLookupResources(FromMonthLookup, MonthCollection, FromMonthParam, FromMonthLabel.Text);
                ApplyLookupResources(ToMonthLookup, MonthCollection, ToMonthParam, ToMonthLabel.Text);

                regionFilter.DefineBinding();
                rayonFilter.DefineBinding();
                organizationFilter.DefineBinding();
                

                if (ContextKeeper.ContainsContext(ContextValue.ReportKeeperFirstLoading))
                {
                    LocationHelper.SetDefaultFilters(manager, ContextKeeper, regionFilter, rayonFilter);
                }
            }
            finally
            {
                m_HasLoad = true;
                IsResourceLoading = false;
            }
        }

        private void YearSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            CorrectSpinEditRange(sender, FromYearSpinEdit, ToYearSpinEdit);
            if (FromYearParam == ToYearParam && FromMonthParam > ToMonthParam)
            {
                var buttonsArgs = new ButtonPressedEventArgs(new EditorButton(ButtonPredefines.Delete));
                ClearLookUpButtonClick(FromMonthLookup, buttonsArgs, FromMonthLookup, ToMonthLookup);
            }
        }

        private void FromMonthLookup_EditValueChanged(object sender, EventArgs e)
        {
            MonthLookupEnum lookupEnum = FromYearParam == ToYearParam ? MonthLookupEnum.Start : MonthLookupEnum.None;
            CorrectLookupRange(FromMonthLookup, ToMonthLookup, lookupEnum);
        }

        private void ToMonthLookup_EditValueChanged(object sender, EventArgs e)
        {
            MonthLookupEnum lookupEnum = FromYearParam == ToYearParam ? MonthLookupEnum.End : MonthLookupEnum.None;
            CorrectLookupRange(FromMonthLookup, ToMonthLookup, lookupEnum);
        }

        private void MonthLookup_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ClearLookUpButtonClick(sender, e, FromMonthLookup, ToMonthLookup);
        }

        private void Region_ValueChanged(object sender, SingleFilterEventArgs e)
        {
            using (ContextKeeper.CreateNewContext(ContextValue.ReportFilterResetting))
            {
                LocationHelper.RegionFilterValueChanged(regionFilter, rayonFilter, e);
            }
        }

        private void Rayon_ValueChanged(object sender, SingleFilterEventArgs e)
        {
            if (ContextKeeper.ContainsContext(ContextValue.ReportFilterResetting))
            {
                return;
            }
            using (ContextKeeper.CreateNewContext(ContextValue.ReportFilterResetting))
            {
                LocationHelper.RayonFilterValueChanged(regionFilter, rayonFilter, e);
            }
        }

        private string GetOrganization(DbManagerProxy manager)
        {
            var ps = new List<object>
            {
                manager.Parameter("@LangID", CurrentCulture.ShortName),
                manager.Parameter("@ID", OrganizationID ?? -1)
            };
            var command = manager.SetSpCommand("dbo.spRepVetOrganizationSelectLookup", ps.ToArray());
            DataTable lookup = command.ExecuteDataTable();
            string organization = lookup.Rows.Count == 0
                ? String.Empty
                : lookup.Rows[0]["FullName"].ToString();
            return organization;
        }
    }
}