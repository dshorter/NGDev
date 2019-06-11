﻿using EIDSS.Reports.BaseControls.Filters;
namespace EIDSS.Reports.Parameterized.AberrationAnalysis.Keepers
{
    partial class VetAberrationKeeper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VetAberrationKeeper));
            this.caseReportTypeLookupFilter = new EIDSS.Reports.BaseControls.Filters.CaseReportTypeLookupFilter();
            this.diagnosisFilter = new EIDSS.Reports.BaseControls.Filters.VetDiagnosisMultiFilter();
            this.caseClassificationMultiFilter = new EIDSS.Reports.BaseControls.Filters.CaseClassificationMultiFilter();
            this.vetCaseTypeLookupFilter = new EIDSS.Reports.BaseControls.Filters.VetCaseTypeLookupFilter();
            ((System.ComponentModel.ISupportInitialize)(this.TimeUnitsLookUp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).BeginInit();
            this.pnlSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceUseArchiveData.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // TimeUnitsLookUp
            // 
            // 
            // DateFieldsLookUp
            // 
            this.DateFieldsLookUp.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("DateFieldsLookUp.Appearance.Font")));
            this.DateFieldsLookUp.Appearance.Options.UseFont = true;
            // 
            // regionFilter
            // 
            this.regionFilter.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("regionFilter.Appearance.Font")));
            this.regionFilter.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.regionFilter, "regionFilter");
            // 
            // rayonFilter
            // 
            this.rayonFilter.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.rayonFilter, "rayonFilter");
            // 
            // settlementFilter
            // 
            resources.ApplyResources(this.settlementFilter, "settlementFilter");
            // 
            // dtStart
            // 
            resources.ApplyResources(this.dtStart, "dtStart");
            // 
            // dtEnd
            // 
            resources.ApplyResources(this.dtEnd, "dtEnd");
            // 
            // pnlSettings
            // 
            this.pnlSettings.Controls.Add(this.vetCaseTypeLookupFilter);
            this.pnlSettings.Controls.Add(this.caseClassificationMultiFilter);
            this.pnlSettings.Controls.Add(this.diagnosisFilter);
            this.pnlSettings.Controls.Add(this.caseReportTypeLookupFilter);
            this.pnlSettings.Controls.SetChildIndex(this.GenerateReportButton, 0);
            this.pnlSettings.Controls.SetChildIndex(this.TimeUnitsLookUp, 0);
            this.pnlSettings.Controls.SetChildIndex(this.DateFieldsLookUp, 0);
            this.pnlSettings.Controls.SetChildIndex(this.lblStart, 0);
            this.pnlSettings.Controls.SetChildIndex(this.lblEnd, 0);
            this.pnlSettings.Controls.SetChildIndex(this.dtStart, 0);
            this.pnlSettings.Controls.SetChildIndex(this.dtEnd, 0);
            this.pnlSettings.Controls.SetChildIndex(this.ceUseArchiveData, 0);
            this.pnlSettings.Controls.SetChildIndex(this.rayonFilter, 0);
            this.pnlSettings.Controls.SetChildIndex(this.regionFilter, 0);
            this.pnlSettings.Controls.SetChildIndex(this.settlementFilter, 0);
            this.pnlSettings.Controls.SetChildIndex(this.caseReportTypeLookupFilter, 0);
            this.pnlSettings.Controls.SetChildIndex(this.diagnosisFilter, 0);
            this.pnlSettings.Controls.SetChildIndex(this.caseClassificationMultiFilter, 0);
            this.pnlSettings.Controls.SetChildIndex(this.vetCaseTypeLookupFilter, 0);
            // 
            // ceUseArchiveData
            // 
            resources.ApplyResources(this.ceUseArchiveData, "ceUseArchiveData");
            this.ceUseArchiveData.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("ceUseArchiveData.Properties.Appearance.Font")));
            this.ceUseArchiveData.Properties.Appearance.Options.UseFont = true;
            this.ceUseArchiveData.Properties.AppearanceDisabled.Font = ((System.Drawing.Font)(resources.GetObject("ceUseArchiveData.Properties.AppearanceDisabled.Font")));
            this.ceUseArchiveData.Properties.AppearanceDisabled.Options.UseFont = true;
            this.ceUseArchiveData.Properties.AppearanceFocused.Font = ((System.Drawing.Font)(resources.GetObject("ceUseArchiveData.Properties.AppearanceFocused.Font")));
            this.ceUseArchiveData.Properties.AppearanceFocused.Options.UseFont = true;
            this.ceUseArchiveData.Properties.AppearanceReadOnly.Font = ((System.Drawing.Font)(resources.GetObject("ceUseArchiveData.Properties.AppearanceReadOnly.Font")));
            this.ceUseArchiveData.Properties.AppearanceReadOnly.Options.UseFont = true;
            // 
            // GenerateReportButton
            // 
            resources.ApplyResources(this.GenerateReportButton, "GenerateReportButton");
            bv.common.Resources.BvResourceManagerChanger.GetResourceManager(typeof(VetAberrationKeeper), out resources);
            // Form Is Localizable: True
            // 
            // caseReportTypeLookupFilter
            // 
            resources.ApplyResources(this.caseReportTypeLookupFilter, "caseReportTypeLookupFilter");
            this.caseReportTypeLookupFilter.Name = "caseReportTypeLookupFilter";
            // 
            // diagnosisFilter
            // 
            this.diagnosisFilter.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("diagnosisFilter.Appearance.Font")));
            this.diagnosisFilter.Appearance.Options.UseFont = true;
            this.diagnosisFilter.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            resources.ApplyResources(this.diagnosisFilter, "diagnosisFilter");
            this.diagnosisFilter.Name = "diagnosisFilter";
            this.diagnosisFilter.ValueChanged += new System.EventHandler<EIDSS.Reports.BaseControls.Filters.MultiFilterEventArgs>(this.diagnosisFilter_ValueChanged);
            // 
            // caseClassificationMultiFilter
            // 
            this.caseClassificationMultiFilter.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("caseClassificationMultiFilter.Appearance.Font")));
            this.caseClassificationMultiFilter.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.caseClassificationMultiFilter, "caseClassificationMultiFilter");
            this.caseClassificationMultiFilter.Name = "caseClassificationMultiFilter";
            this.caseClassificationMultiFilter.ValueChanged += new System.EventHandler<EIDSS.Reports.BaseControls.Filters.MultiFilterEventArgs>(this.caseClassificationMultiFilter_ValueChanged);
            // 
            // vetCaseTypeLookupFilter
            // 
            resources.ApplyResources(this.vetCaseTypeLookupFilter, "vetCaseTypeLookupFilter");
            this.vetCaseTypeLookupFilter.Name = "vetCaseTypeLookupFilter";
            this.vetCaseTypeLookupFilter.ValueChanged += new System.EventHandler<EIDSS.Reports.BaseControls.Filters.SingleFilterEventArgs>(this.vetCaseTypeLookupFilter_ValueChanged);
            // 
            // VetAberrationKeeper
            // 
            this.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("VetAberrationKeeper.Appearance.Font")));
            this.Appearance.Options.UseFont = true;
            resources.ApplyResources(this, "$this");
            this.Name = "VetAberrationKeeper";
            ((System.ComponentModel.ISupportInitialize)(this.TimeUnitsLookUp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).EndInit();
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceUseArchiveData.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VetDiagnosisMultiFilter diagnosisFilter;
        private CaseClassificationMultiFilter caseClassificationMultiFilter;
        private CaseReportTypeLookupFilter caseReportTypeLookupFilter;
        private VetCaseTypeLookupFilter vetCaseTypeLookupFilter;
    }
}
