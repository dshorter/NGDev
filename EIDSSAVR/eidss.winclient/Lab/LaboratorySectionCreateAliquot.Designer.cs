﻿namespace eidss.winclient.Lab
{
    partial class LaboratorySectionCreateAliquot
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaboratorySectionCreateAliquot));
            this.lbNumber = new DevExpress.XtraEditors.LabelControl();
            this.seNumber = new DevExpress.XtraEditors.SpinEdit();
            this.dtAccessionDate = new DevExpress.XtraEditors.DateEdit();
            this.lblDateAccession = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.seNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAccessionDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAccessionDate.Properties)).BeginInit();
            this.SuspendLayout();
            bv.common.Resources.BvResourceManagerChanger.GetResourceManager(typeof(LaboratorySectionCreateAliquot), out resources);
            // Form Is Localizable: True
            // 
            // lbNumber
            // 
            this.lbNumber.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lbNumber.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            resources.ApplyResources(this.lbNumber, "lbNumber");
            this.lbNumber.Name = "lbNumber";
            // 
            // seNumber
            // 
            resources.ApplyResources(this.seNumber, "seNumber");
            this.seNumber.Name = "seNumber";
            this.seNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seNumber.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.seNumber.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dtAccessionDate
            // 
            resources.ApplyResources(this.dtAccessionDate, "dtAccessionDate");
            this.dtAccessionDate.Name = "dtAccessionDate";
            this.dtAccessionDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("dtAccessionDate.Properties.Buttons"))))});
            this.dtAccessionDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // lblDateAccession
            // 
            this.lblDateAccession.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblDateAccession.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            resources.ApplyResources(this.lblDateAccession, "lblDateAccession");
            this.lblDateAccession.Name = "lblDateAccession";
            // 
            // LaboratorySectionCreateAliquot
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtAccessionDate);
            this.Controls.Add(this.lblDateAccession);
            this.Controls.Add(this.seNumber);
            this.Controls.Add(this.lbNumber);
            this.FormID = "L33";
            this.HelpTopicID = "labs_aliquot_create";
            this.Icon = global::eidss.winclient.Properties.Resources.Aliquots_Derivatives__large_;
            this.Name = "LaboratorySectionCreateAliquot";
            ((System.ComponentModel.ISupportInitialize)(this.seNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAccessionDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAccessionDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbNumber;
        private DevExpress.XtraEditors.SpinEdit seNumber;
        private DevExpress.XtraEditors.DateEdit dtAccessionDate;
        private DevExpress.XtraEditors.LabelControl lblDateAccession;
    }
}
