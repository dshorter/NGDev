﻿using eidss.model.Schema;

namespace eidss.winclient.Administration
{
    partial class CaseClassificationMasterDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseClassificationMasterDetail));
            this.matrixDetail = new eidss.winclient.Administration.CaseClassificationDetail();
            this.SuspendLayout();
            bv.common.Resources.BvResourceManagerChanger.GetResourceManager(typeof(CaseClassificationMasterDetail), out resources);
            // Form Is Localizable: True
            // 
            // matrixDetail
            // 
            resources.ApplyResources(this.matrixDetail, "matrixDetail");
            this.matrixDetail.FormID = "";
            this.matrixDetail.HelpTopicID = "";
            this.matrixDetail.Icon = null;
            this.matrixDetail.idfsVectorType = null;
            this.matrixDetail.InlineMode = bv.winclient.BasePanel.InlineMode.UseNewRow;
            this.matrixDetail.Name = "matrixDetail";
            this.matrixDetail.Sizable = true;
            // 
            // CaseClassificationMasterDetail
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.matrixDetail);
            this.Icon = global::eidss.winclient.Properties.Resources.Reference_Book__large__41_;
            this.Name = "CaseClassificationMasterDetail";
            this.ResumeLayout(false);

        }

        #endregion

        private CaseClassificationDetail matrixDetail;
    }
}
