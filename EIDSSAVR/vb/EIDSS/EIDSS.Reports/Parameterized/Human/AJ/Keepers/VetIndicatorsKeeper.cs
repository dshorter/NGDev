﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using bv.model.BLToolkit;
using bv.winclient.Layout;
using DevExpress.XtraEditors.Controls;
using eidss.model.Reports.AZ;
using eidss.model.Reports.Common;
using eidss.model.Reports.OperationContext;
using EIDSS.Reports.BaseControls.Keeper;
using EIDSS.Reports.BaseControls.Report;
using EIDSS.Reports.Parameterized.Human.AJ.Reports;

namespace EIDSS.Reports.Parameterized.Human.AJ.Keepers
{
    public partial class VetIndicatorsKeeper : BaseReportKeeper
    {
        private readonly ComponentResourceManager m_Resources = new ComponentResourceManager(typeof (VetIndicatorsKeeper));

        private List<ItemWrapper> m_MonthCollection;

        internal VetIndicatorsKeeper(): base(new Dictionary<string, string>())
        {
            ReportType = typeof(VetIndicatorsSummaryReport);
            InitializeComponent();
            using (ContextKeeper.CreateNewContext(ContextValue.ReportFilterLoading))
            {
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

                organizationFilter.ReportType = VetReportType.Indicators;
            }
            m_HasLoad = true;
        }

        protected override BaseReport GenerateReport(DbManagerProxy manager, DbManagerProxy archiveManager)
        {
            ReportType = OrganizationID.HasValue 
                ? typeof(VetIndicatorsDetailReport)
                : typeof(VetIndicatorsSummaryReport);

            var model = new VetIndicatorsSurrogateModel(CurrentCulture.ShortName,
                OrganizationID, organizationFilter.SelectedText,
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

                //m_Resources.ApplyResources(FromMonthLookup, "FromMonthLookup");
                //m_Resources.ApplyResources(ToMonthLookup, "ToMonthLookup");
                m_Resources.ApplyResources(FromMonthLabel, "FromMonthLabel");
                m_Resources.ApplyResources(ToMonthLabel, "ToMonthLabel");
                m_Resources.ApplyResources(FromYearLabel, "FromYearLabel");
                m_Resources.ApplyResources(ToYearLabel, "ToYearLabel");
                //m_Resources.ApplyResources(FromYearSpinEdit, "FromYearSpinEdit");
                //m_Resources.ApplyResources(ToYearSpinEdit, "ToYearSpinEdit");

                ApplyLookupResources(FromMonthLookup, MonthCollection, FromMonthParam, FromMonthLabel.Text);
                ApplyLookupResources(ToMonthLookup, MonthCollection, ToMonthParam, ToMonthLabel.Text);

                organizationFilter.DefineBinding();
            }
            finally
            {
                m_HasLoad = true;
                IsResourceLoading = false;
            }
        }

        #region Properties

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
    }
}