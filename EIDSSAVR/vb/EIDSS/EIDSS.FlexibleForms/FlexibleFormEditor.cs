using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using bv.common;
using bv.common.Configuration;
using bv.common.Core;
using bv.common.db.Core;
using bv.common.Objects;
using bv.common.win;
using bv.winclient.BasePanel;
using bv.winclient.Core;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraNavBar;
using DevExpress.XtraNavBar.ViewInfo;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraVerticalGrid.Rows;
using eidss.model.Core;
using eidss.model.Enums;
using eidss.model.Resources;
using EIDSS.FlexibleForms.Components;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.DesignerHosting;
using EIDSS.FlexibleForms.Helpers;
using BaseReferenceType = bv.common.db.BaseReferenceType;
using FFDecorElementTypes = bv.common.FFDecorElementTypes;
using FFDeterminantTypes = bv.common.FFDeterminantTypes;
using FFEditModes = bv.common.FFEditModes;
using FFParameterEditors = bv.common.FFParameterEditors;
using FFParameterTypes = bv.common.FFParameterTypes;
using FFRuleCheckPointType = bv.common.FFRuleCheckPointType;
using FFRuleFunctions = bv.common.FFRuleFunctions;
using FixedStyle = DevExpress.XtraGrid.Columns.FixedStyle;
using Label = EIDSS.FlexibleForms.Components.Label;
using Localizer = bv.common.Core.Localizer;

namespace EIDSS.FlexibleForms
{
    /// <summary>
    /// �������� ������������ ���� (Flexible Forms)
    /// </summary>
    public partial class FlexibleFormEditor : BaseDetailList
    {
        public override bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = value;

                //���� ��������� �����������
                if (value)
                {
                    propSection.Enabled =
                        propParameter.Enabled =
                        pnlParameterDesignHost.Enabled =
                        standaloneBarDockControl3.Enabled =
                        standaloneBarDockControl1.Enabled =
                        pnlTemplateDesignHost.Enabled =
                        tbcTemplateComponentsProperties.Enabled =
                        false;

                }
                miTemplateLanguage.Enabled = true;
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        public FlexibleFormEditor()
        {
            InitializeComponent();
            //
            if (IsDesignMode())
                return;
            nbcTemplatesEditor.View = new StandardSkinNavigationPaneViewInfoRegistrator(BaseSettings.SkinName);

            DbService = new DbService();
            baseDataSet = MainDbService.MainDataSet;
            MainDbService.OnAfterPost += OnMainDbServiceOnAfterPost;
            Load += OnFlexibleFormEditorLoad;
            PermissionObject = EIDSSPermissionObject.FlexibleForm;

            ffRender = new FFRender((DbService)DbService, pnlTemplateDesignHost);
            ffRender.TemplateDesignerHost.RightClick += OnTemplateDesignerHostRightClick;
            ffRender.EditValueChanging += OnFlexibleFormEditorEditValueChanging;
            ffRender.EditValueChanged += OnFlexibleFormEditorEditValueChanged;
            ffRender.ParameterIsMandatory += SetControlMandatoryState;
            ffRender.SectionSelect += OnSectionSelect;
            ffRender.SectionResize += OnSectionSizeChanged;
            ffRender.ParameterSelect += OnParameterSelect;
            ffRender.ParameterResize += OnParameterSizeChanged;
            ffRender.LabelSelect += OnLabelSelect;
            ffRender.LabelResize += OnLabelSizeChanged;
            ffRender.LineSelect += OnLineSelect;
            ffRender.LineResize += OnLineSizeChanged;
            ffRender.ParameterHostUnSelect += OnParameterHostOnUnSelect;
            ffRender.TemplateMouseDown += OnTemplateMouseDown;
            ffRender.OnSelectedBandChanged += OnSelectedBandChanged;
            cbTemplateUNI.EditValueChanged += OnCbTemplateUNIOnEditValueChanged;
            //��������� ���������
            tbcRedParamProperties.ShowTabHeader = tbcTemplateComponentsProperties.ShowTabHeader = DefaultBoolean.False;
            tbcRedParamProperties.SelectedTabPage = tbpRedFormType;
            //
            tbcMain.SelectedTabPage = tbpParameters;

            //������ �������� ����� �������� ��� ������
            nbcTemplatesEditor.ActiveGroup = nbgTemplates;

            //�������� � �������
            repPopupRuleActions.Popup += OnPopupRuleActionsPopup;
            repPopupRuleActions.CloseUp += OnPopupRuleActionsCloseUp;

            popupRuleActions.QueryPopUp += OnRuleActionsQueryPopUp;
            popupRuleActions.Popup += OnRuleActionsPopup;
            popupRuleActions.CloseUp += OnRuleActionsCloseUp;

            //��������� � �������
            repPopupRuleConstants.Popup += OnPopupRuleConstantsPopup;
            repPopupRuleConstants.CloseUp += OnPopupRuleConstantsCloseUp;

            //���� ��� ����������� � ������� ������ � ����������
            popupParameters.Popup += OnPopupParametersPopup;
            //���� ��� ����������� �������� ��. ������ ����
            //
            constantsFixedPreset.EditValueChanged += OnConstantsFixedPresetEditValueChanged;
            //
            barManagerMain.SetPopupContextMenu(treeTemplates, popupTemplates);
            barManagerMain.SetPopupContextMenu(treeParameters, popupParameters);
            //
            OnAfterPost += OnFlexibleFormEditorOnAfterPost;

            erParameterID.Visible = erSectionID.Visible = erTemplateID.Visible =
                                                          erTemplateParameterID.Visible = erTemplateSectionID.Visible = erLabelID.Visible
                                                                                          = false;//true; 

            StrEmpty = EidssMessages.Get("strEmpty");
            StrPointer = EidssMessages.Get("strPointer");
            delimiterPath = EidssMessages.Get("delimiterPath");

            nationalTemplateName = EidssMessages.Get("defaultTemplateName");
            nationalRuleName = EidssMessages.Get("defaultRuleName");
            nationalSectionName = EidssMessages.Get("defaultSectionName");
            nationalParameterName = EidssMessages.Get("defaultParameterName");
            nationalLabelName = EidssMessages.Get("defaultLabelName");
            if (Localizer.IsRtl)
            {
                pParametersPanel.Dock = DockStyle.Right;
                splitterControl1.Dock = DockStyle.Right;
                nbcTemplatesEditor.Dock = DockStyle.Right;
                nbcTemplatesEditor.RightToLeft = RightToLeft.Yes;
                splitterControl4.Dock = DockStyle.Right;
                barManagerMain.RightToLeft = DefaultBoolean.True;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedBand"></param>
        void OnSelectedBandChanged(GridBand selectedBand)
        {
            if (selectedBand == null) return;
            btnFreezeSection.Down = selectedBand.RootBand.Fixed.Equals(FixedStyle.Left);
        }

        //private bool selectPermission;
        private bool UpdatePermission { get; set; }
        private bool DeletePermission { get; set; }
        private bool InsertPermission { get; set; }

        //private CriticalObjectsHelper CriticalObjectsHelper { get; set; }

        /// <summary>
        /// ���� �� ������������� UNI/�� UNI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCbTemplateUNIOnEditValueChanged(object sender, EventArgs e)
        {
            var templatesRow = GetActiveTemplate();
            if (templatesRow == null) return;
            var checkEdit = sender as CheckEdit;
            if (checkEdit == null) return;

            if (checkEdit.Checked)
            {
                //���� UNI-������� ��������� ������ ������� �� ����� FF-����, �� ��������� �������
                //����� ������ ��������
                var templatesRows = MainDbService.GetTemplatesByFormType(templatesRow.idfsFormType);
                foreach (var row in templatesRows)
                {
                    if (row.blnUNI)
                    {
                        if (
                            !WinUtils.ConfirmMessage(
                                String.Format(EidssMessages.Get("Question_Change_UNI"), row.NationalName,
                                              templatesRow.NationalName), String.Empty))
                        {
                            templatesRow.blnUNI = false;
                            checkEdit.Checked = false;
                            return;
                        }
                        row.blnUNI = false;
                        break;
                    }
                }
            }
            else
            {
                //������ ������� ��������� UNI. � ���� ����� ������ ���� ���� �� ���� UNI (10366)
                var templatesRows = MainDbService.GetTemplatesByFormType(templatesRow.idfsFormType);
                var hasAnotherUni = false;
                foreach (var row in templatesRows)
                {
                    if (!row.IsRowAlive()) continue;
                    if (row.idfsFormTemplate == templatesRow.idfsFormTemplate) continue;
                    if (row.blnUNI)
                    {
                        hasAnotherUni = true;
                        break;
                    }
                }
                if (!hasAnotherUni)
                {
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("TemplateUNI_Mustbe"));
                    templatesRow.blnUNI = true;
                    cbTemplateUNI.EditValueChanged -= OnCbTemplateUNIOnEditValueChanged;
                    cbTemplateUNI.ValueChecked = true;
                    cbTemplateUNI.EditValueChanged += OnCbTemplateUNIOnEditValueChanged;
                }
            }

            #region ������� ������-������

            /*
            //���� � ������� ������� ��� ������������� � � ������� UNI ���� ��� �������������, �� ��������� ����� ������� UNI ������
            if (checkEdit.Checked)
            {
                bool wasError = false;
                //���� � ����� ������� ��������� ������������� ���� ������, �� �� � �������� �� ����� ���� UNI
                if (MainDbService.GetTemplateDeterminants(templatesRow.idfsFormTemplate, FFDeterminantTypes.Country).Length > 1)
                {
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("Determinants_Already_Have_Country"));
                    wasError = true;
                }

                //���� � ����� ������� ��� ������������� � ���� ������ ������ ���� ��� �������������, ��  ����� ���������� ����������� UNI � ���� �� ���� ������
                if (MainDbService.GetTemplateDeterminants(templatesRow.idfsFormTemplate).Length == 0)
                {
                    var templatesRows = MainDbService.GetTemplatesByFormType(templatesRow.idfsFormType);
                    foreach (var row in templatesRows)
                    {
                        if (!row.blnUNI) continue;
                        if (row.idfsFormTemplate.Equals(templatesRow.idfsFormTemplate)) continue;
                        //������� ������ ��������, � ������� ��� �������������
                        if (MainDbService.GetTemplateDeterminants(row.idfsFormTemplate).Length == 0)
                        {
                            //���� ������������ �� ����� ������� ������� UNI � ���������� �������, ����� ������� ������ �� ���� �������, ������ �� ���� ������ �� ������ �����������-������
                            if (!WinUtils.ConfirmMessage(String.Format(EidssMessages.Get("Determinants_Question_Remove_UNI2"), row.NationalName), String.Empty))
                            {
                                wasError = true;
                                break;
                            }
                            row.blnUNI = false;

                            //ErrorForm.ShowMessageDirect(String.Format(EidssMessages.Get("TemplateUNI_Cant_Set", null, null), row.NationalName));
                            //wasError = true;
                            //break;
                        }
                    }
                }

                var determinants =
                    MainDbService.GetTemplateDeterminants(templatesRow.idfsFormTemplate, FFDeterminantTypes.Country);
                if (determinants.Length == 1)
                {
                    //���� �� ���������, �� ���� ��� ������ � ����� �� �������, �� ��������� �������
                    var templates =
                        MainDbService.GetTemplateWithSameDeterminant(determinants[0].IsidfsBaseReferenceNull() ? determinants[0].idfsGISBaseReference : determinants[0].idfsBaseReference, templatesRow.idfsFormType, true);
                    foreach (var templateDeterminantValuesRow in templates)
                    {
                        var template = MainDbService.GetTemplateRow(templateDeterminantValuesRow.idfsFormTemplate);
                        if (template == null) return;
                        if (templatesRow.idfsFormTemplate.Equals(template.idfsFormTemplate)) continue;
                        //���� ������������ �� ����� ������� ������� UNI � ���������� �������, ����� ������� ������ �� ���� �������, ������ �� ���� ������ �� ������ �����������-������
                        if (!WinUtils.ConfirmMessage(String.Format(EidssMessages.Get("Determinants_Question_Remove_UNI"), template.NationalName), String.Empty))
                        {
                            wasError = true;
                            //break;
                        }
                        else
                        {
                            template.blnUNI = false;
                        }
                    }
                }
                if (wasError)
                {
                    templatesRow.blnUNI = false;
                    checkEdit.Checked = false;
                }
            }

            //��������� ������� UNI, ���� �� ���������, ������ ��� ������� ��� ����� ������
            erTemplateUNI.Enabled = !templatesRow.blnUNI;
            */
            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnConstantsFixedPresetEditValueChanged(object sender, EventArgs e)
        {
            var row = grdvRuleConstants.GetFocusedRow() as FlexibleFormsDS.RuleConstantRow;
            if (row == null) return;
            var lookUpEdit = ((LookUpEdit)sender);
            row.varConstant = lookUpEdit.EditValue;
        }

        private string StrEmpty { get; set; }
        private const string StrEmptyKey = "emptykey";
        private string StrPointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFlexibleFormEditorEditValueChanged(object sender, EventArgs e)
        {
            if (EditValueChanged != null) EditValueChanged(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        private FFRender ffRender { get; set; }

        /// <summary>
        /// ���������� ����������� ������, �������� �� ���������������� �����
        /// </summary>
        /// <param name="button"></param>
        /// <param name="type">true -- ������ �� ����������, false -- ������ �� ��������</param>
        private void SetButtonEnabled(BarButtonItem button, bool type)
        {
            if (type)
            {
                button.Enabled = InsertPermission || ReadOnly;
            }
            else
            {
                button.Enabled = DeletePermission || ReadOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFlexibleFormEditorLoad(object sender, EventArgs e)
        {
            #region ����������� ���� ������� � ����������� ���������

            AuditObject = new AuditObject((long)EIDSSAuditObject.daoFlexibleFormDesigner, (long)AuditTable.ffParameter);
            PermissionObject = EIDSSPermissionObject.FlexibleForm;
            //���������� ����������� �� ������
            //selectPermission = eidss.model.Core.EidssUserContext.User.HasPermission(PermissionHelper.SelectPermission(PermissionObject));
            UpdatePermission = EidssUserContext.User.HasPermission(PermissionHelper.UpdatePermission(PermissionObject));
            InsertPermission = EidssUserContext.User.HasPermission(PermissionHelper.InsertPermission(PermissionObject));
            DeletePermission = EidssUserContext.User.HasPermission(PermissionHelper.DeletePermission(PermissionObject));
            
            //��������� ����� � ����������
            btnAddDeterminant.Enabled =
                btnAddLabelToTemplate.Enabled = btnAddLineToTemplate.Enabled = btnAddParameter.Enabled =
                                                                               btnAddRule.Enabled =
                                                                               btnAddSection.Enabled =
                                                                               btnRuleAddConstant.Enabled =
                                                                               btnAddTemplate.Enabled =
                                                                               InsertPermission && !ReadOnly;
            btnFreezeSection.Enabled = UpdatePermission || InsertPermission || !ReadOnly;

            btnDeleteDeterminant.Enabled = btnDeleteParameter.Enabled = btnDeleteRule.Enabled =
                                                                        btnDeleteSection.Enabled =
                                                                        btnDeleteTemplate.Enabled =
                                                                        btnRuleDeleteConstant.Enabled =
                                                                        btnTemplateRemoveObject.Enabled =
                                                                        DeletePermission && !ReadOnly;
            barTemplateManagement.Visible = 
                standaloneBarDockControl3.Visible =
                barTemplateItems.Visible = 
                barTemplateCommands.Visible = !ReadOnly;

            ffRender.ReadOnly = ReadOnly;

            #endregion

            
        }

        public override bool LoadData(ref object id)
        {
            var result = base.LoadData(ref id);

            #region �������� ��������� �������

            //�������, ��� ���������� �� ������� + �� ��� ���� �����
            long idfsFormTemplate = 0;
            
            if ((StartUpParameters != null) && StartUpParameters.ContainsKey("t"))
            {
                idfsFormTemplate = (long) StartUpParameters["t"];
            }
            
            if (idfsFormTemplate > 0)
            {
                //�������� ��� �����
                var templatesRow = MainDbService.LoadTemplate(idfsFormTemplate);
                if (templatesRow != null)
                {
                    //���������� ���� � ������ ��������
                    TreeListNode formTemplateNode = null;
                    foreach (TreeListNode node in treeTemplates.Nodes)
                    {
                        var ftRow = node.Tag as FlexibleFormsDS.FormTypesRow;
                        if ((ftRow != null) && (ftRow.idfsFormType == templatesRow.idfsFormType))
                        {
                            formTemplateNode = node;
                            break;
                        }
                    }
                    if (formTemplateNode != null)
                    {
                        formTemplateNode.ExpandAll();
                        //������ ����������� �����������. ���������� ���� ��������. ���� ����� ��� ������.
                        foreach (TreeListNode node in formTemplateNode.Nodes)
                        {
                            var tRow = node.Tag as FlexibleFormsDS.TemplatesRow;
                            if ((tRow != null) && (tRow.idfsFormTemplate == idfsFormTemplate))
                            {
                                tbcMain.SelectedTabPage = tbpTemplates;
                                treeTemplates.FocusedNode = node;
                                OnTreeTemplatesClick(this, EventArgs.Empty);
                                break;
                            }
                        }
                    }
                    
                }
            }


            //OnTemplateMouseDown

            #endregion

            return result;
        }

        /// <summary>
        /// �������� ����� ��������� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMainDbServiceOnAfterPost(object sender, PostEventArgs e)
        {
            var listDelete = new List<long>();
            var dictionary = new Dictionary<long, Line>();
            //��������� � ������� ID �� ��������
            foreach (var key in ffRender.LineList.Keys)
            {
                if (key > 0) continue;
                var line = ffRender.LineList[key];
                var linesRow = (FlexibleFormsDS.LinesRow)line.Tag;
                listDelete.Add(key);
                dictionary.Add(linesRow.idfDecorElement, line);
            }
            foreach (var key in listDelete)
            {
                ffRender.LineList.Remove(key);
            }
            foreach (var key in dictionary.Keys)
            {
                ffRender.LineList.Add(key, dictionary[key]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRuleActionsPopup(object sender, EventArgs e)
        {
            var rowParameterTemplate = (FlexibleFormsDS.ParameterTemplateRow)treeRuleActionParameters.Selection[0].Tag;
            //������ ���� ������� �������� �������
            var rowRules = GetActiveRule();
            if (rowRules == null) return;

            //���������� ����� �������� ������� ��� ����� ���������
            var rows =
                    (FlexibleFormsDS.RuleParameterForActionRow[])
                    MainDbService.MainDataSet.RuleParameterForAction.Select(
                        DataHelper.Filter("idfsRule", rowRules.idfsRule, "idfsParameter", rowParameterTemplate.idfsParameter));

            //������� ���������
            foreach (Control control in popupRuleActions.PopupControl.Controls)
            {
                ((CheckEdit)control).Checked = false;
            }

            //����������� ��������� ���, ��� ����� ���� ���-�� �������
            foreach (var row in rows)
            {
                if (!row.IsRowAlive()) continue;

                foreach (Control control in popupRuleActions.PopupControl.Controls)
                {
                    if (control.Tag == null) continue;
                    long id;
                    if (!Int64.TryParse(control.Tag.ToString(), out id)) continue;
                    if (id.Equals(row.idfsRuleAction))
                        ((CheckEdit)control).Checked = true;
                }
            }
        }

        /// <summary>
        /// ����� ���������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFlexibleFormEditorOnAfterPost(object sender, EventArgs e)
        {
            if (parEditingParameter != null)
                parEditingParameter.RefreshSelectListInControl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPopupRuleActionsCloseUp(object sender, CloseUpEventArgs e)
        {
            //�������� �������� ���������� �������
            propTemplateRule.SelectedObject = null;
            propTemplateRule.SelectedObject = GetActiveRule();
        }

        /// <summary>
        /// ������, ���������� ��� �������� ��� �����������
        /// </summary>
        private FlexibleFormsDS.SectionsRow RowCopiedSection { get; set; }
        /// <summary>
        /// ��������, ���������� ��� �������� ��� �����������
        /// </summary>
        private FlexibleFormsDS.ParametersRow RowCopiedParameter { get; set; }
        /// <summary>
        /// ������, ���������� ��� �������� ��� �����������
        /// </summary>
        private FlexibleFormsDS.TemplatesRow RowCopiedTemplate { get; set; }

        /// <summary>
        /// ����������� ���� �� ������ ���������� � ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPopupParametersPopup(object sender, EventArgs e)
        {
            if (treeParameters.Selection.Count == 0)
            {
                btnCopySectionOrParameter.Enabled = btnPasteSectionOrParameter.Enabled = false;
                return;
            }
            //����������� �������� ������ ��� ��������� ������� �� ��������� ���� ������
            //������� �������� ����� ������ �� ���� ����� ��� ������. ����� ������ ���� ������� ��������������� �����������.
            //������� ������ ���������� � ���� ���� � � ������ �������-������
            object tag = treeParameters.Selection[0].Tag;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            var rowSections = tag as FlexibleFormsDS.SectionsRow;
            var rowParameters = tag as FlexibleFormsDS.ParametersRow;
            bool isFormType = rowFormType != null;
            bool isSection = rowSections != null;
            bool isParameter = rowParameters != null;
            bool existCopiedObject = !((RowCopiedSection == null) && (RowCopiedParameter == null));
            //���� ���������� ������, �� �������������� �������� �� �����, ���� � ���������
            if ((RowCopiedSection != null) && isSection)
            {
                existCopiedObject = !MainDbService.IsChildSection(RowCopiedSection.idfsSection, rowSections.idfsSection);
            }

            btnCopySectionOrParameter.Enabled = isSection || isParameter;
            btnPasteSectionOrParameter.Enabled = (isFormType || isSection) && existCopiedObject;
        }

        /// <summary>
        /// �������� ���� ����� ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPopupRuleConstantsCloseUp(object sender, CloseUpEventArgs e)
        {
            //�������� �������� ���������� �������
            var rowRules = GetActiveRule();
            if (grdvRuleConstants.EditingValue != null)
            {
                var row = grdvRuleConstants.GetFocusedRow() as FlexibleFormsDS.RuleConstantRow;
                if (row != null)
                    row.varConstant = grdvRuleConstants.EditingValue;
            }

            if (rowRules != null)
                rowRules.ConstantsText = MainDbService.GetRuleConstantsText(rowRules);

            propTemplateRule.SelectedObject = null;
            propTemplateRule.SelectedObject = rowRules; 

        }

        /// <summary>
        /// �������� ���� ����� ����������������� �������� ��� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPopupRuleConstantsPopup(object sender, EventArgs e)
        {
            //������ ����� �������
            if (grdRuleConstants.Parent == null)
                grdRuleConstants.Parent = popupRuleConstants;
            if (standaloneBarDockControlRuleConstants.Parent == null)
                standaloneBarDockControlRuleConstants.Parent = popupRuleConstants;
            //�������� ������ ��������, ��������� � ��������� ��������, ������� �� ������������� ����
            RefreshRuleConstants();
        }

        /// <summary>
        /// ��������� ������ �������� � ������� ��� �������
        /// </summary>
        private void RefreshRuleConstants()
        {
            //�������� ������ ��������, ��������� � ��������� ��������, ������� �� ������������� ����
            //������ ���� ������� �������� �������
            var rowRules = GetActiveRule();

            if (rowRules == null) return;
            //���������� ����� ��������� ������� ��� ����� �������
            grdRuleConstants.DataSource = MainDbService.MainDataSet.RuleConstant.Select(DataHelper.Filter("idfsRule", rowRules.idfsRule));
        }

        /// <summary>
        /// ������� ������� ���� ������ �������� ��� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRuleActionsQueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //���� � ������� ���� ��������� �� ��������, �� �������� �������� ����
            if (  treeRuleActionParameters.Selection.Count == 0 ||
                !(treeRuleActionParameters.Selection[0].Tag is FlexibleFormsDS.ParameterTemplateRow))
                e.Cancel = true;
        }

        /// <summary>
        /// ���������� �������, ������� ������ �������
        /// </summary>
        /// <returns></returns>
        private FlexibleFormsDS.RulesRow GetActiveRule()
        {
            FlexibleFormsDS.RulesRow result = null;

            if (treeRules.Selection.Count > 0 && 
                treeRules.Selection[0].Tag != null)
            {
                result = (FlexibleFormsDS.RulesRow)treeRules.Selection[0].Tag;
            }
            return result;
        }

        /// <summary>
        /// ���������� ������, ������� ������ ������ 
        /// <returns></returns>
        /// </summary>
        private FlexibleFormsDS.TemplatesRow GetActiveTemplate()
        {
            FlexibleFormsDS.TemplatesRow result = null;

            if (treeTemplates.Selection.Count > 0 &&
                treeTemplates.Selection[0].Tag is FlexibleFormsDS.TemplatesRow)
            {
                result = (FlexibleFormsDS.TemplatesRow)treeTemplates.Selection[0].Tag;
            }
            return result;
        }

        /// <summary>
        /// �������� ���� ������ �������� ��� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRuleActionsCloseUp(object sender, CloseUpEventArgs e)
        {
            if (treeRuleActionParameters.Selection.Count == 0 ||
                !(treeRuleActionParameters.Selection[0].Tag is FlexibleFormsDS.ParameterTemplateRow))
                return;

            var rowParameterTemplate = (FlexibleFormsDS.ParameterTemplateRow)treeRuleActionParameters.Selection[0].Tag;
            //������ ������������ ������������ ��������
            if (rowParameterTemplate.idfsParameter < 0)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("Rule_Cant_Add_Unsaved_Parameter"));
                return;
            }

            //������ ���� ������� �������� �������
            var rowRules = GetActiveRule();
            if (rowRules == null)
                return;

            //�������� ��������� �������� ��� �������
            var idsActions = new List<long>();
            foreach (Control control in popupRuleActions.PopupControl.Controls)
            {
                var checkEdit = control as CheckEdit;
                if (checkEdit != null &&
                    checkEdit.Tag != null &&
                    checkEdit.Checked)
                    idsActions.Add(Convert.ToInt64(checkEdit.Tag));
                }

            //���������� �������� �������
            //��������� ������ �������� ��� ������� ����� ����������, �� ������ ��������� � ������ �� ������
            var rows =
                    (FlexibleFormsDS.RuleParameterForActionRow[])
                    MainDbService.MainDataSet.RuleParameterForAction.Select(
                    DataHelper.Filter("idfsRule", rowRules.idfsRule, "idfsParameter", rowParameterTemplate.idfsParameter));
            foreach (var row in rows)
            {
                row.Delete();
            }

            //���������� ����� �������� ��������
            for (int i = 0; i < idsActions.Count; i++)
            {
                #region ������ ����� ������

                var rowParameterForAction = MainDbService.MainDataSet.RuleParameterForAction.NewRuleParameterForActionRow();
                rowParameterForAction.idfsFormTemplate = rowRules.idfsFormTemplate;
                rowParameterForAction.idfsRule = rowRules.idfsRule;
                rowParameterForAction.idfsParameter = rowParameterTemplate.idfsParameter;
                rowParameterForAction.idfsRuleAction = idsActions[i];
                MainDbService.MainDataSet.RuleParameterForAction.AddRuleParameterForActionRow(rowParameterForAction);

                #endregion
            }

            #region ���������� ������ � ��������� ��������

            if (treeRuleActionParameters.Nodes.Count > 0)
            {
                var templateNode = treeRuleActionParameters.Nodes.FirstNode;
                var node = templateNode.GetNodeMatch(rowParameterTemplate);
                if (node != null) node[tlcAction] = MainDbService.GetRuleActionsText(rowRules, rowParameterTemplate);
            }

            #endregion

            rowRules.ActionText = String.Format("{0}{1}", MainDbService.MainDataSet.RuleParameterForAction.Select(DataHelper.Filter("idfsRule", rowRules.idfsRule)).Length, EidssMessages.Get("actions"));
        }




        /// <summary>
        /// �������� ���� ����� �������� ��� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPopupRuleActionsPopup(object sender, EventArgs e)
        {
            //������ ����� �������
            if (treeRuleActionParameters.Parent == null)
                treeRuleActionParameters.Parent = popupRuleActionParameters;
            //
            RefreshRuleActionParameters();
        }

        /// <summary>
        /// ������ ������ ���������� ��� ������ ���� �������� � �������
        /// </summary>
        private void RefreshRuleActionParameters()
        {
            //������ ������ ����� ��, � �� �����������
            treeRuleActionParameters.Nodes.Clear();
            //������� ����� �������� ��� ������ (������ ��� �� ������ ������ �� ��� �����������)
            var templatesRow = GetActiveTemplate();
            if (templatesRow == null)
                return;

            var node = treeRuleActionParameters.AppendNode(null, null, templatesRow);
            node[trlcTreeListParametersColumn] = templatesRow.NationalName;

            //�������� ��� ������ � ���������, ��� �� ������ ������ ���� � �������
            treeRuleActionParameters.AppendSectionNodes(MainDbService, node, templatesRow.idfsFormTemplate, null, GetActiveRule());
            treeRuleActionParameters.AppendParameterNodes(MainDbService, node, templatesRow.idfsFormTemplate, null, GetActiveRule());
        }

        /// <summary>
        /// ������ ���� �� �������
        /// </summary>
        /// <param name="sender"></param>
        void OnTemplateDesignerHostRightClick(object sender)
        {
            //������ ���� ������� �������� �������, ����� ����� ���� ��������� �������
            if (treeRules.Selection.Count == 0)
            {
                //������� �����������
                barManagerMain.SetPopupContextMenu((Control)sender, null);
                return;
            }
            //
            if (ReadOnly) return;

            //���������, ��� �������
            int selectedCount = ffRender.TemplateDesignerHost.SelectedParameterHostsCount();

            if (selectedCount < 1 || selectedCount > 2) return;
            //�� ��� �������� �������� � �������
            if (!(ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) is Parameter))
            {
                //������� �����������
                barManagerMain.SetPopupContextMenu((Control)sender, null);
                return;
            }
            var parameterTemplateRow1 = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0).GetParameterTemplateRow();
            if (parameterTemplateRow1 == null)
                return;

            FlexibleFormsDS.ParameterTemplateRow parameterTemplateRow2 = null;

            if (selectedCount == 2)
            {
                parameterTemplateRow2 = ffRender.TemplateDesignerHost.GetSelectedParameterHost(1).GetParameterTemplateRow();
                if (parameterTemplateRow2 == null)
                    return;

                //��������, ����� ��������� ���� ������ ���� ������
                if (parameterTemplateRow1.idfsEditor != parameterTemplateRow2.idfsEditor)
                {
                    //������ ��� �������� ������
                    //TODO ���� ���-�� ���� �� �������������� ����������
                    for (var i = 0; i < 5; i++)
                    {
                        barManagerMain.Items[String.Format("btnRuleFunction{0}", i + 1)].Visibility = BarItemVisibility.Never;
                    }
                    return;
                }
            }
            //�������� ��������� ���� �������
            MainDbService.LoadRuleFunctions(selectedCount);
            barManagerMain.SetPopupContextMenu((Control)sender, popupRules);
            //TODO ����� ��������� ����� ���������� ������ � ������������� ����������
            for (int i = 0; i < MainDbService.MainDataSet.RuleFunctions.Count; i++)
            {
                var row = MainDbService.MainDataSet.RuleFunctions[i];
                //���� �������� �� ����� ��� ����, �� ��� �� �������� ������� ��� ��������� � ������� �����
                if (row.idfsRuleFunction.Equals((long)FFRuleFunctions.CurrentDate))
                {
                    if (!parameterTemplateRow1.idfsParameterType.Equals((long)FFParameterTypes.Date) && !parameterTemplateRow1.idfsParameterType.Equals((long)FFParameterTypes.DateTime))
                    {
                        barManagerMain.Items[String.Format("btnRuleFunction{0}", i + 1)].Visibility = BarItemVisibility.Never;
                        continue;
                    }
                }

                //�������� ������� ������ �������� � ��������� ������ ����
                var buttonItem = (BarButtonItem)popupRules.LinksPersistInfo[i].Item;
                //buttonItem.Caption = String.Format(row.strMask, parameterTemplateRow1.NationalName, null);
                buttonItem.Caption = String.Format(row.strMaskNational, parameterTemplateRow1.NationalName, null);
                //�������� ��� � ��� "id �������;��������1;��������2"
                var tag = new StringBuilder(String.Format("{0};{1};", row.idfsRuleFunction, parameterTemplateRow1.idfsParameter));
                if (parameterTemplateRow2 != null)
                {
                    buttonItem.Caption = String.Format(row.strMaskNational, parameterTemplateRow1.NationalName, parameterTemplateRow2.NationalName);
                    tag.Append(parameterTemplateRow2.idfsParameter);
                }
                buttonItem.Tag = tag;
                //������� ���� ������� ������
                barManagerMain.Items[String.Format("btnRuleFunction{0}", i + 1)].Visibility = BarItemVisibility.Always;
            }
            for (int i = MainDbService.MainDataSet.RuleFunctions.Count; i < 5; i++)
            {
                barManagerMain.Items[String.Format("btnRuleFunction{0}", i + 1)].Visibility = BarItemVisibility.Never;
            }
        }

        /// <summary>
        /// ��������� ��������� �������� � ����� �� ��������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFlexibleFormEditorEditValueChanging(object sender, ChangingEventArgs e)
        {
            if (EditValueChanging != null)
                EditValueChanging(sender, e);
        }

        public delegate void EditValueChangingDelegate(object sender, ChangingEventArgs e);

        /// <summary>
        /// ������� �����������, ����� �������� �������� � ������������ �������� ������ �� ������� ����������
        /// </summary>
        public event EditValueChangingDelegate EditValueChanging;

        public delegate void EditValueChangedDelegate(object sender, EventArgs e);

        /// <summary>
        /// ������� �����������, �����  �������� � ������ ����������
        /// </summary>
        public event EditValueChangedDelegate EditValueChanged;

        /// <summary>
        /// ����� ������ ������ ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLabelSizeChanged(object sender, EventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1)
                return;

            var label = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Label;
            if (label == null)
                return;

            var labelsRow = ((FlexibleFormsDS.LabelsRow)label.Tag);
            //���� ������ ���, ����� �� ����������� ����������
            propTemplateLabel.SelectedObject = null;
            propTemplateLabel.SelectedObject = labelsRow;
        }

        /// <summary>
        /// ����� ����� ������ �� �������
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnLabelSelect(ParameterHost parameterHost)
        {
            var label = (Label)parameterHost;
            var labelsRow = (FlexibleFormsDS.LabelsRow)label.Tag;
            if (labelsRow.IsColorNull()) labelsRow.Color = Color.FromArgb(labelsRow.intColor);
            //�������� ������, ��������� ��� �� ������ ���� �������� ��� ��������� ����� �������
            btnFit.Enabled = false;
            //������������ ������
            propTemplateLabel.SelectedObject = null;
            propTemplateLabel.SelectedObject = labelsRow;

            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateLabel;
        }

        /// <summary>
        /// ��������� �������� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLineSizeChanged(object sender, EventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1)
                return;

            var line = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Line;
            if (line == null)
                return;

            var linesRow = ((FlexibleFormsDS.LinesRow)line.Tag);
            //���� ������ ���, ����� �� ����������� ����������
            propTemplateLine.SelectedObject = null;
            propTemplateLine.SelectedObject = linesRow;
        }

        /// <summary>
        /// ����� ������� ����� �� ������� ����� ��������
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnLineSelect(ParameterHost parameterHost)
        {
            var line = (Line)parameterHost;
            var linesRow = (FlexibleFormsDS.LinesRow)line.Tag;
            if (linesRow.IsColorNull()) linesRow.Color = Color.FromArgb(linesRow.intColor);
            if (linesRow.IsintOrientationNull())
            {
                if (linesRow.IsblnOrientationNull()) linesRow.intOrientation = -1;
                else linesRow.intOrientation = linesRow.blnOrientation ? 1 : 0;
            }
            //�������� ������, ��������� ��� �� ������ ���� �������� ��� ��������� ����� �������
            btnFit.Enabled = false;

            linesRow.Color = Color.FromArgb(linesRow.intColor);

            //������������ ������
            propTemplateLine.SelectedObject = null;
            propTemplateLine.SelectedObject = linesRow;

            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateLine;
        }

        /// <summary>
        /// ���� �� �����, ������� �������� ����-����������� ��� �������
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        private void OnTemplateMouseDown(object sender, MouseEventArgs e)
        {
            //���� �� ������ �������� ������, �� �������
            FlexibleFormsDS.TemplatesRow rowTemplate = GetActiveTemplate();
            if (rowTemplate == null) return;

            //���������� ����������� ������
            List<ParameterHost> list = ffRender.TemplateDesignerHost.GetSelectedParameterHosts();
            btnFit.Enabled = (list.Count == 0 ||
                              (list.Count == 1
                              && list[0].Tag is FlexibleFormsDS.SectionTemplateRow
                              && !((FlexibleFormsDS.SectionTemplateRow)list[0].Tag).blnGrid)
                             );

            //���� �� ������ ������ �� ���������� ����������, �� �������
            if (SelectedObjectInParametersLibrary == null || ReadOnly)
                return;

            //� ������ ������ ��������� � ������� ��������� � ������, ���� � ���� ��� ���� observation
            if (MainDbService.HasObservationsForTemplate(rowTemplate.idfsFormTemplate))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("AddInTemplate_Template_Has_Observations"));
                return;
            }

            #region ���� ��� ������

            if (SelectedObjectInParametersLibrary is FlexibleFormsDS.SectionsRow)
            {
                var sectionRow = (FlexibleFormsDS.SectionsRow)SelectedObjectInParametersLibrary;
                //��������, �� ������� �� ���� ������ �����������
                if (ffRender.CriticalObjects.Sections.ContainsKey(sectionRow.idfsSection))
                {
                    var co = ffRender.CriticalObjects.Sections[sectionRow.idfsSection];
                    if (!co.CanAddToTemplate)
                    {
                        ErrorForm.ShowMessageDirect(EidssMessages.Get("msgCantAddCriticalObjectToTemplate"));
                        return;
                    }
                }

                //����������, �� �������� �� ��� ������ ��� � �������� ������ ��� ������������. ������������ ������ � ��������� ������ ��������� � ������.
                if (MainDbService.CheckSectionForNew(sectionRow))
                {
                    //��������� ��������� ������
                    bool result = WinUtils.ConfirmMessage(EidssMessages.Get("AddSectionInTemplate_Cant_Add_Unsaved_Section"), String.Empty);
                    if (!result) return;
                    result = Post();
                    if (!(result && (m_PromptResult == DialogResult.Yes))) return;
                }
                //����������, �� ��������� �� ��� ��� ������
                if (ffRender.SectionAlreadyAdded(sectionRow.idfsSection)) return;
                ffRender.CreateSectionInTemplateHost(sectionRow, e.Location, rowTemplate.idfsFormTemplate);
            }

            #endregion

            #region ���� ��� ��������

            else if (SelectedObjectInParametersLibrary is FlexibleFormsDS.ParametersRow)
            {
                var rowParameter = (FlexibleFormsDS.ParametersRow)SelectedObjectInParametersLibrary;
                //��������, �� ������� �� ���� ������ �����������
                if (ffRender.CriticalObjects.Parameters.ContainsKey(rowParameter.idfsParameter))
                {
                    var co = ffRender.CriticalObjects.Parameters[rowParameter.idfsParameter];
                    if (!co.CanAddToTemplate)
                    {
                        ErrorForm.ShowMessageDirect(EidssMessages.Get("msgCantAddCriticalObjectToTemplate"));
                        return;
                    }
                }
                //����������, �� �������� �� ���� �������� ��� ��� �������� ������ ��� ������������. ������������ ������ � ��������� ������ ��������� � ������.
                if (MainDbService.CheckParameterForNew(rowParameter))
                {
                    bool result = WinUtils.ConfirmMessage(EidssMessages.Get("AddParameterInTemplate_Cant_Add_Unsaved_Parameter"), String.Empty);
                    if (!result) return;
                    result = Post();
                    if (!(result && (m_PromptResult == DialogResult.Yes))) return;
                }
                //����������, �� �������� �� ��� ���� ��������
                if (ffRender.ParameterAlreadyAdded(rowParameter.idfsParameter)) return;
                ffRender.CreateParameterInTemplateHost(rowParameter, e.Location, rowTemplate.idfsFormTemplate, false);
            }

            #endregion

            //������� ��������� ������, ����� �������� ����������� ���������� ��� ����� ������
            SelectedObjectInParametersLibrary = null;
            //������� �� ������ ����������, ����� ���������������� � �����������
            OnTreeParametersClick(sender, null);
        }

        /// <summary>
        /// �����������, ����� ��������� ����� �� ������
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnParameterHostOnUnSelect(ParameterHost parameterHost)
        {
            tbcTemplateComponentsProperties.SelectedTabPage = null; //������ �� �������
        }

        /// <summary>
        /// ��������� �������� ���������� ���������-�������� �� ������� ����� ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnParameterSizeChanged(object sender, EventArgs e)
        {
            //���������� ����� �������� ������
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1)
                return;

            var parameter = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Parameter;
            if (parameter == null || parameter.NowLoading)
                return;

            var rowParameterTemplate = ((FlexibleFormsDS.ParameterTemplateRow)parameter.Tag);
            //���� ������ ���, ����� �� ����������� ����������
            propTemplateParameter.SelectedObject = null;
            propTemplateParameter.SelectedObject = rowParameterTemplate;
        }

        /// <summary>
        /// ��������� �������� ��������� ������-�������� �� ������� ����� ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSectionSizeChanged(object sender, EventArgs e)
        {
            //���������� ����� �������� ������
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1)
                return;

            var section = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Section;
            if (section == null || section.NowLoading)
                return;

            var rowSectionTemplate = ((FlexibleFormsDS.SectionTemplateRow)section.Tag);
            //���� ������ ���, ����� �� ����������� ����������
            propTemplateSection.SelectedObject = null;
            propTemplateSection.SelectedObject = rowSectionTemplate;
        }

        /// <summary>
        /// ������ �����-�� �������� (� ���� ��������) �� ������� ����� ��������
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnParameterSelect(ParameterHost parameterHost)
        {
            if (parameterHost.NowLoading)
                return;

            var parameter = parameterHost as Parameter;
            if (parameter == null)
                return;

            //�������� ������, ��������� ��� �� ������ ���� �������� ��� ��������� ����� �������
            btnFit.Enabled = false;

            //������������ ������
            propTemplateParameter.SelectedObject = null;
            propTemplateParameter.SelectedObject = parameter.Tag;

            erTemplateParameterEditMode.Visible = parameter.Editor != FFParameterEditors.CheckBox;

            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateParameter;
        }

        /// <summary>
        /// ������� �����-�� ������ (� ���� ��������) �� ������� ����� ��������
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnSectionSelect(ParameterHost parameterHost)
        {
            if (parameterHost.NowLoading)
                return;

            var section = parameterHost as Section;
            if (section == null)
                return;
            //������������ ������
            propTemplateSection.SelectedObject = section.Tag;

            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateSection;
        }

        /// <summary>
        /// ����������� ������ ���������
        /// </summary>
        /// <param name="parentControl"></param>
        public static void Register(Control parentControl)
        {
            var category = MenuActionManager.Instance.FindAction("MenuConfiguration", MenuActionManager.Instance.System, 960);
            new MenuAction(ShowMe, MenuActionManager.Instance, category, "MenuCustomForms", 1005, false, (int)eidss.model.Enums.MenuIconsSmall.FFDesigner)
                                {
                                    SelectPermission =
                                        PermissionHelper.SelectPermission(
                                            eidss.model.Enums.EIDSSPermissionObject.FlexibleForm)
                                };
        }

        /// <summary>
        /// ������ ���������
        /// </summary>
        public static void ShowMe()
        {
            BaseFormManager.ShowNormal(new FlexibleFormEditor(), null);
        }

        /// <summary>
        /// ������ ���������
        /// </summary>
        /// <param name="ff"></param>
        public static void ShowMe(FlexibleFormEditor ff)
        {
            object id = null;
            BaseFormManager.ShowClient(ff, BaseFormManager.MainForm, ref id);
        }

        /// <summary>
        /// ���������� ������ ��������� �������� ��� ������
        /// </summary>
        private void BindingRuleActions()
        {
            var popup = new PopupContainerControl();
            popupRuleActions.PopupControl = popup;
            var dv = LookupCache.Get(BaseReferenceType.rftFFRuleAction);
            popup.Controls.Clear();
            var popupHeight = 0;
            var popupWidth = 0;
            foreach (DataRowView row in dv)
            {
                var idfsReference = long.Parse(row["idfsReference"].ToString());
                if (idfsReference <= 0) continue;
                var checkEdit = new CheckEdit
                                    {
                                        Text = row["Name"].ToString(),
                                        Checked = false,
                                        Tag = Convert.ToInt64(row["idfsReference"]),
                                        Top = popupHeight
                                    };
                checkEdit.Width = checkEdit.CalcBestSize().Width;
                if (checkEdit.Width > popupWidth)
                {
                    popupWidth = checkEdit.Width;
                }
                popupHeight += checkEdit.Height;

                popup.Controls.Add(checkEdit);
            }
            popup.AutoScroll = true;
            popup.AutoScrollMinSize = new Size(popupWidth, popupHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableLanguages"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private DataRow CreateRowForLang(DataTable tableLanguages, string lang)
        {
            var row = tableLanguages.NewRow();
            row["ID"] = lang;
            row["Name"] = bv.common.Core.Localizer.GetMenuLanguageName(lang);
            return row;
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        protected override void DefineBinding()
        {
            base.DefineBinding();

            //��������� ������ HACode � ���� ��������
            popupHACodeParameter.BindReprositoryHACodeLookup(MainDbService.LoadHACodeList());
            popupHACodeParameter.QueryResultValue += OnPopupHACodeParameterQueryResultValue;
            BindingRuleActions();

            //��������� ���� ���� � ������
            LoadFormTypes();

            #region ������������� ������
            //��������� ������ ParameterTypeReference
            rleParameterTypeReferenceEditor.DataSource = LookupCache.Get(LookupTables.ParameterTypeReferenceEditor);
            //rleParameterTypeReferenceEditor.EditValueChanged += OnChangeParameterType;
            rleParameterTypeReferenceEditor.Closed += OnChangeParameterType;
            rleParameterTypeReferenceEditor.ButtonClick += OnRleParameterTypeReferenceEditorButtonClick;

            //��������� ������ ��������� ��� Editor ����������
            MainDbService.LoadParameterEditors();
            leParameterControlType.DataSource = MainDbService.MainDataSet.ParameterEditors;
            leParameterControlType.Closed += OnLeParameterControlTypeClosed;
            icbSectionType.EditValueChanged += OnSectionTypeChanged;

            #region �������� ���������� � ������� ������, �������������� ��������

            //Localizer.SupportedLanguages
            LanguageTemplate = bv.common.Core.Localizer.lngEn;

            var tableLanguages = new DataTable();
            tableLanguages.Columns.Add("ID", typeof(string));
            tableLanguages.Columns.Add("Name", typeof(string));
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                DataRow row = null;

                switch (lang.Culture.TwoLetterISOLanguageName)
                {
                    case "uz":
                        row = CreateRowForLang(tableLanguages, lang.Culture.Name.IndexOf("Cyrl") > 0 ? bv.common.Core.Localizer.lngUzCyr : bv.common.Core.Localizer.lngUzLat);
                        break;
                    case "az":
                        row = CreateRowForLang(tableLanguages, bv.common.Core.Localizer.lngAzLat);
                        break;
                    default:
                        row = CreateRowForLang(tableLanguages, lang.Culture.TwoLetterISOLanguageName);
                        break;
                }

                if (row != null)
                    tableLanguages.Rows.Add(row);
            }

            leTemplateLanguage.DataSource = tableLanguages;
            leTemplateLanguage.ButtonPressed += OnLeTemplateLanguageButtonPressed;
            //���������� ��� ����, � ������� �������� �������
            miTemplateLanguage.EditValue = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            leTemplateLanguage.Buttons[1].Visible = false;

            #endregion

            #endregion
        }

        /// <summary>
        /// � ������� �������������� ������� ��������� �������������� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRleParameterTypeReferenceEditorButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                var form = new FFParameterTypesEditor();
                BaseFormManager.ShowModal(form, FindForm());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPopupHACodeParameterQueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            if (parEditingParameter.ParametersRow != null && e.Value != null)
            {
            parEditingParameter.ParametersRow.intHACode = Convert.ToInt32(e.Value);
            parEditingParameter.RefreshSelectListInControl();
        }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLeParameterControlTypeClosed(object sender, ClosedEventArgs e)
        {
            propParameter.PostEditor();
        }

        /// <summary>
        /// ������� ������ � �������� ������ ����� ��� ����������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLeTemplateLanguageButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (treeTemplates.Selection.Count == 0)
                return;
            if (Utils.IsEmpty(miTemplateLanguage.EditValue))
                return;
            var template = GetActiveTemplate();
            if (template == null)
                return;
            //��������� ���������� � �������� �������� ��������� ����� ��� ����� ������, ����� �����������
            if (miTemplateLanguage.EditValue.Equals(bv.common.Core.Localizer.lngEn))
                return;

            var langid = miTemplateLanguage.EditValue.ToString();

            switch (e.Button.Kind)
            {
                case ButtonPredefines.Undo:
                    //���� ��� ���������� ����� ��� ����������, �� �������
                    if (!HasLanguageTemplate(template.idfsFormTemplate, langid))
                        return;

                    /*
                    //������� ��������� ������
                    MainDbService.DeleteSectionTemplate(template.idfsFormTemplate, langid);
                    //������� ��������� ���������
                    MainDbService.DeleteParameterTemplate(template.idfsFormTemplate, langid);
                    //������� ��������� ������
                    MainDbService.DeleteLabelTemplate(template.idfsFormTemplate, langid);
                    //������� ��������� �����
                    MainDbService.DeleteLineTemplate(template.idfsFormTemplate, langid);
                    */

                    //������ �� ����� ������� ����, � ����� ����������� ���� �������� �� ����������� ����
                    CopyHelper.RestoreLanguageLayout(MainDbService
                        , template.idfsFormTemplate
                        , langid);

                    //����������� ���� �� ����������
                    //miTemplateLanguage.EditValue = bv.common.Core.Localizer.lngEn;
                    OnTreeTemplatesClick(sender, EventArgs.Empty);

                    break;
            }
        }

        /// <summary>
        /// �������� ��� ������ (������� ��� ���������)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSectionTypeChanged(object sender, EventArgs e)
        {
            //�������� �� �� � � �������� �������, ����� � �� ���
            var isGrid = Convert.ToBoolean(((ImageComboBoxEdit)sender).EditValue);
            var node = treeParameters.Selection[0];
            if (isGrid)
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                SetSectionType(node, isGrid);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            else
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                SetImageForSectionType(isGrid, node);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);
        }

        /// <summary>
        /// ����������� ������ 
        /// </summary>
        /// <param name="idEditor"></param>
        private void SetFilterControlTypeEditors(long idEditor)
        {
            string filter;
            if (!Utils.IsEmpty(idEditor))
            {
                switch (idEditor)
                {
                    case (long)FFParameterTypes.Boolean/*"parBoolean"*/:
                        filter = "idfsEditor=10067001";//'editCheck'
                        break;
                    case (long)FFParameterTypes.Numeric/*"fptNumeric"*/:
                    case (long)FFParameterTypes.NumericNatural:
                    case (long)FFParameterTypes.NumericPositive:
                    case (long)FFParameterTypes.NumericInteger:
                        filter = "idfsEditor=10067008 or idfsEditor=10067009";//'editText''editUpDown'
                        break;
                    case (long)FFParameterTypes.String/*"parString"*/:
                        filter = "idfsEditor=10067008 or idfsEditor=10067006";//'editText''editMemo'
                        break;
                    case (long)FFParameterTypes.DateTime /*"parDatetime"*/:
                        filter = "idfsEditor=10067004";//'editDateTime'
                        break;
                    case (long)FFParameterTypes.Date /*"parDate"*/:
                        filter = "idfsEditor=10067003";//'editDate'
                        break;
                    default:
                        filter = "idfsEditor=10067002";//'editCombo'
                        break;
                }
            }
            else
            {
                //��� ����� �� BaseReference � rftParametersFixedPresetValue
                filter = "idfsEditor=10067002";//'editCombo'
            }

            if (filter.Length > 0)
            {
            var editors = (FlexibleFormsDS.ParameterEditorsRow[])MainDbService.MainDataSet.ParameterEditors.Select(filter);
            leParameterControlType.DataSource = editors;

            //��� ���� ���������, ������� ������ � ������
            var rowParameter = (FlexibleFormsDS.ParametersRow)treeParameters.Selection[0].Tag;
            //������������� �������� ����� ��������, ��� ����������� ������� ��� �������
            rowParameter.idfsParameterType = idEditor;
            //���� ����� ����� �������� ������� ���� ��, ��� ��� ���� � ���������, �� ������� ���
            //���� ��� ������ ��������, �� ������� ������ ����������

                if (editors.Length > 0)
                {
            var finded = false;
            foreach (var row in editors)
            {
                if (row.RowState != DataRowState.Deleted &&
                    row["idfsEditor"].Equals(rowParameter.idfsEditor))
                {
                finded = true;
                break;
            }
            }
            rowParameter.idfsEditor = finded ? rowParameter.idfsEditor : Convert.ToInt64(editors[0].idfsEditor);
            parEditingParameter.Editor = ParameterControlTypeHelper.ConvertToParameterControlType(rowParameter.idfsEditor);
            propParameter.SelectedObject = null;
            propParameter.SelectedObject = rowParameter;
        }
            }
        }

        /// <summary>
        /// ����� ���� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChangeParameterType(object sender, EventArgs e)
        {
            propParameter.PostEditor();
            var leSender = (sender as LookUpEdit);
            if (leSender != null)
            //���������� ���
            SetFilterControlTypeEditors(Convert.ToInt64(leSender.EditValue));
        }

        /// <summary>
        /// ��������� ������ � ���������� ����������
        /// (�� ���� ������-������ ��� ������-��������)
        /// </summary>
        private object SelectedObjectInParametersLibrary { get; set; }

        /// <summary>
        /// ������ �������� ���� ���� �� �� � ��������� �� � �������
        /// </summary>
        public void LoadFormTypes()
        {
            //������������ ������ �� ����� ����
            MainDbService.LoadFormTypes();
            //��������� �� ����� � ��������� ��������
            treeParameters.ClearNodes();
            treeTemplates.ClearNodes();
            foreach (FlexibleFormsDS.FormTypesRow row in MainDbService.MainDataSet.FormTypes.Rows)
            {
                if (row.IsRowAlive())
                {
                AppendFormTypeNode(treeParameters, row, true);
                AppendFormTypeNode(treeTemplates, row, false);
            }
        }
        }

        /// <summary>
        /// ��������������� ����� ���������� ���� ���� ����� � ������
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="row"></param>
        /// <param name="isParameterTree"></param>
        public void AppendFormTypeNode(TreeList tree, FlexibleFormsDS.FormTypesRow row, bool isParameterTree)
        {
            var node = tree.AppendNode(null, null, row);
            node[trlcTreeListParametersColumn] = row.Name;
            node.ImageIndex = node.SelectImageIndex = 0;
            //���� � ���� ����� ���� ������ ��� ���������, �� ������� ��� ��������� ����
            //��������� ���� ����� ���������� � ����� ������ ������ ���������, �� ����� ��������� ������������ ��� ��������
            if (isParameterTree)
            {
                if (row.HasNestedSections || row.HasParameters)
                {
                    var nodeEmpty = tree.AppendNode(null, node, StrEmptyKey);
                    nodeEmpty[trlcTreeListParametersColumn] = StrEmpty;
                    node.Expanded = false;
                }
            }
            else
            {
                if (row.HasTemplates)
                {
                    var nodeEmpty = tree.AppendNode(null, node, StrEmptyKey);
                    nodeEmpty[trlcTreeListParametersColumn] = StrEmpty;
                    node.Expanded = false;
                }
            }
        }

        /// <summary>
        /// ���������� ������ ���� � ���� � ������
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetFullPathForNode(TreeListNode node)
        {
            return CollectPath(node, new StringBuilder());
        }

        /// <summary>
        /// ����������� ������� ��� ����� ���� �� ����
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sbPath"></param>
        /// <returns></returns>
        private string CollectPath(TreeListNode node, StringBuilder sbPath)
        {
            sbPath.Insert(0, node[trlcTreeListParametersColumn].ToString(), 1);
            return node.ParentNode != null ? CollectPath(node.ParentNode, sbPath.Insert(0, delimiterPath, 1)) : sbPath.ToString();
        }


        /// <summary>
        /// ������ �� ������� ��� Flexible Forms
        /// </summary>
        private DbService MainDbService { get { return (DbService)DbService; } }

        /// <summary>
        /// ������������ ���� ������ ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersBeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            var parentTree = (TreeList)sender;
            var parentNode = e.Node;
            var parentObject = e.Node.Tag;
            //���� � ���� ��� ��������� �������� ����, �� �������� �� �� �����������
            if ((parentNode.FirstNode != null) && (!parentNode.FirstNode.Tag.Equals(StrEmptyKey))) return;

            //������������� ����� ���� ��� �����, ���� ������
            //�������� �� ����� ��������� ��������� ����
            Cursor = Cursors.WaitCursor;

            //���� �������� ������ ���������� ��� ������������� � �������, �� ����� ������������ ������ ������������ ������
            var useCache = (parentTree == treeParametersLibrary);

            //���� ������������ ��� �����
            if (parentObject is FlexibleFormsDS.FormTypesRow)
            {
                #region �������� �����������

                //�������� ������, ����������� � ����� ���� ����
                var row = (FlexibleFormsDS.FormTypesRow)parentObject;

                if (!useCache)
                {
                    //������������ ���������� ������
                    MainDbService.LoadSections(row.idfsFormType, null, null);
                    //������������ ���������� ����������
                    //����� ������ ��, ��� �� ��������� � �����-���� ������, � ������ � ���� �����
                    MainDbService.LoadParameters(row.idfsFormType, null);
                }

                //���������, ��� �� ���� ����� �������, � ��� ���
                if (MainDbService.HasNestedSections(row.idfsFormType))
                {
                    //��������� ������ � ������
                    MainDbService.AppendSectionNodes(parentTree, parentNode, MainDbService.GetSectionsByFormType(row.idfsFormType), useCache, true);
                }
                if (MainDbService.HasNestedParameters(row.idfsFormType))
                {
                    //��������� ��������� � ������ (�������� ������ ��, ������� ��������� � ���� �����, �� �� ����� ������� ������)
                    parentTree.AppendParameterNodes(parentNode, MainDbService.GetParametersByFormType(row.idfsFormType), false);
                }

                #endregion
            }

            //���� ������������ ������
            if (parentObject is FlexibleFormsDS.SectionsRow)
            {
                #region �������� �����������

                //�������� ������, ��� ������� ��� ������ �������� ������������
                var row = (FlexibleFormsDS.SectionsRow)parentObject;

                if (!useCache)
                {
                    //������������ ���������� ������
                    MainDbService.LoadSections(row.idfsFormType, null, row.idfsSection);
                    //������������ ���������� ����������
                    //����� ������ ��, ��� �� ��������� � �����-���� ������, � ������ � ���� �����
                    MainDbService.LoadParameters(row.idfsFormType, row.idfsSection);
                }

                if (MainDbService.HasNestedSections(row))
                {
                    //��������� ������ � ������
                    MainDbService.AppendSectionNodes(parentTree, parentNode, MainDbService.GetSectionChildrenRows(row.idfsSection), useCache, true);
                }
                if (MainDbService.HasNestedParameters(row))
                {

                    //��������� ������ � ������
                    parentTree.AppendParameterNodes(parentNode, (FlexibleFormsDS.ParametersRow[])MainDbService.MainDataSet.Parameters.Select(DataHelper.Filter("idfsSection", row.idfsSection)), false);

                }

                #endregion
            }

            Cursor = Cursors.Default;
        }

        private const int IndexSectionDefaultImage = 1;
        private const int IndexSectionTableImage = 6;

        /// <summary>
        /// ����������� ������ ���� ������
        /// </summary>
        /// <param name="isTable"></param>
        /// <param name="node"></param>
        private static void SetImageForSectionType(bool isTable, TreeListNode node)
        {
            node.ImageIndex = isTable
                                  ? (node.SelectImageIndex = IndexSectionTableImage)
                                  : (node.SelectImageIndex = IndexSectionDefaultImage);
        }

        /// <summary>
        /// ����� �������� �������������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnParEditingParameterSizeChanged(object sender, EventArgs e)
        {
            if ((treeParameters.Selection.Count > 0) && (treeParameters.Selection[0].Tag is FlexibleFormsDS.ParametersRow))
            {
                var rowParameter = ((FlexibleFormsDS.ParametersRow)treeParameters.Selection[0].Tag);
                rowParameter.intWidth = parEditingParameter.Width;
                rowParameter.intHeight = parEditingParameter.Height;
                //� ����������� �� ����� ��������� �������� �������� ������
                rowParameter.intLabelSize = parEditingParameter.LabelSize;
                //���� ������ ���, ����� �� ����������� ����������
                propParameter.SelectedObject = null;
                propParameter.SelectedObject = rowParameter;
            }
        }

        /// <summary>
        /// ���������� ������������� ���������, ������� ������ �������������
        /// </summary>
        private Parameter parEditingParameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool HasChanges()
        {
            //return true;//MainDbService.MainDataSet.HasChanges();
            return ((InsertPermission || UpdatePermission || DeletePermission)
                    &&
                    (
                    MainDbService.MainDataSet.Sections.GetChanges() != null ||
                    MainDbService.MainDataSet.Parameters.GetChanges() != null ||
                    MainDbService.MainDataSet.Templates.GetChanges() != null ||
                    MainDbService.MainDataSet.SectionTemplate.GetChanges() != null ||
                    MainDbService.MainDataSet.ParameterTemplate.GetChanges() != null ||
                    MainDbService.MainDataSet.Rules.GetChanges() != null ||
                    MainDbService.MainDataSet.RuleParameterForFunction.GetChanges() != null ||
                    MainDbService.MainDataSet.RuleConstant.GetChanges() != null ||
                    MainDbService.MainDataSet.RuleParameterForAction.GetChanges() != null ||
                    MainDbService.MainDataSet.Lines.GetChanges() != null ||
                    MainDbService.MainDataSet.TemplateDeterminantValues.GetChanges() != null ||
                    MainDbService.MainDataSet.Labels.GetChanges() != null
                    )
                    );
        }

        /// <summary>
        /// ��������� ������ �� �������� ������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropSectionCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (e.Row.Equals(erSectionNationalName) || e.Row.Equals(erSectionName))
            {
                if (treeParameters.Selection.Count > 0 && treeParameters.Selection[0].Tag is FlexibleFormsDS.SectionsRow)
                {
                var treeNode = treeParameters.Selection[0];
                if (e.Row.Equals(erSectionNationalName))
                    treeNode[trlcTreeListParametersColumn] = e.Value;

                //��������� ������ ����
                var rowSection = (FlexibleFormsDS.SectionsRow)treeNode.Tag;
                //���� ��������� ��� �� ������
                if (rowSection.IsDefaultNameNull() || rowSection.DefaultName.Equals(String.Empty))
                    rowSection.DefaultName = DefaultSectionName;

                //���� ������������ ��� �� ������, �� ����������� ��������� ���
                if (rowSection.IsNationalNameNull() || rowSection.NationalName.Equals(String.Empty))
                {
                    rowSection.NationalName = rowSection.DefaultName;
                    treeNode[trlcTreeListParametersColumn] = rowSection.NationalName;
                }

                rowSection.FullPath = GetFullPathForNode(treeParameters.Selection[0]);
                propSection.SelectedObject = rowSection;
            }
            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);
        }
        }

        /// <summary>
        /// �����������, �������������� ��� ��������� ������, ���������� ������ ���� � ���� � ������
        /// </summary>
        private string delimiterPath { get; set; }

        /// <summary>
        /// ���� � ��������� ������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropParameterCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (treeParameters.Selection.Count == 0 ||
                treeParameters.Selection[0].Tag == null ||
                !(treeParameters.Selection[0].Tag is FlexibleFormsDS.ParametersRow))
                return;

            var treeListNode = treeParameters.Selection[0];
            var rowParameter = (FlexibleFormsDS.ParametersRow)treeListNode.Tag;
                //���� ��������� ��� �� ������
            if (e.Row.Equals(erParameterDefaultName) &&
                (rowParameter.IsDefaultNameNull() || rowParameter.DefaultName.Equals(String.Empty)))
                {
                    rowParameter.DefaultName = DefaultParameterName;
                }

            //����� ������������� �����
            if (e.Row.Equals(erParameterNationalName))
            {
                rowParameter.NationalName = e.Value.ToString();
                if (rowParameter.IsNationalNameNull() || rowParameter.NationalName.Equals(String.Empty))
                {
                    rowParameter.NationalName = rowParameter.DefaultName;
                }
                //������ ��������� � ��������� ����������
                parEditingParameter.Text = rowParameter.NationalName;
                //� ��������� ����
                treeListNode[trlcTreeListParametersColumn] = rowParameter.NationalName;
                //��������� ������ ����
                rowParameter.FullPath = GetFullPathForNode(treeListNode);
            }
            //����� ����������� ����� ���������
            if (e.Row.Equals(erParameterScheme))
            {
                parEditingParameter.Scheme = ParameterSchemeHelper.ConvertToParameterScheme(Convert.ToInt32(e.Value));
                //������������� ������ ���������
                if (parEditingParameter.ParametersRow != null)
                {
                    parEditingParameter.ParametersRow.intLabelSize = parEditingParameter.LabelSize;
                }
            }
            //����� ���� ������������ �������� � ���������
            if (e.Row.Equals(erParameterEditors))
            {
                parEditingParameter.Editor = ParameterControlTypeHelper.ConvertToParameterControlType(Convert.ToInt64(e.Value));
            }
            int val;
            //����� ������
            if (e.Row.Equals(erParameterWidth))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int width = val;
                    if (width < parEditingParameter.MinimumSize.Width) width = parEditingParameter.MinimumSize.Width;
                    if (width > Parameter.MaxParameterSize.Width) width = Parameter.MaxParameterSize.Width;
                    if (parEditingParameter.ParametersRow != null) parEditingParameter.ParametersRow.intWidth = parEditingParameter.Width = width;
                }
            }
            //����� ������
            if (e.Row.Equals(erParameterHeight))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int height = val;
                    if (height < parEditingParameter.MinimumSize.Height) height = parEditingParameter.MinimumSize.Height;
                    if (height > Parameter.MaxParameterSize.Height) height = Parameter.MaxParameterSize.Height;
                    if (parEditingParameter.ParametersRow != null) parEditingParameter.ParametersRow.intHeight = parEditingParameter.Height = height;
                }
            }
            //����� ������ ���������� ����
            if (e.Row.Equals(erParameterLabelSize))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int labelsize = val;
                    if (labelsize < 1) labelsize = 1;
                    if (parEditingParameter.ParametersRow != null)
                    {
                        if (parEditingParameter.Scheme.IsHorizontalOrientation())
                        {
                            if (labelsize > parEditingParameter.Width)
                                labelsize = parEditingParameter.Width;
                        }
                        else
                        {
                            if (labelsize > parEditingParameter.Height)
                                labelsize = parEditingParameter.Height;
                        }
                    }

                    if (parEditingParameter.ParametersRow != null) parEditingParameter.ParametersRow.intLabelSize = parEditingParameter.LabelSize = labelsize;
                }
            }

            propParameter.SelectedObject = null;
            propParameter.SelectedObject = parEditingParameter.Tag;

            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="formTypesRow"></param>
        public void LoadTemplateNodes(TreeListNode parentNode, FlexibleFormsDS.FormTypesRow formTypesRow)
        {
            //���� � ���� ��� ��������� �������� ����, �� �������� �� �� �����������
            if (parentNode.FirstNode != null && !parentNode.FirstNode.Tag.Equals(StrEmptyKey))
                return;

            //������������� ����� ���� ��� �����, ���� ������
            //�������� �� ����� ��������� ��������� ����
            Cursor = Cursors.WaitCursor;

            //���� ������������ ��� �����

            #region �������� �����������

            //������������ ���������� ��������
            MainDbService.LoadTemplates(formTypesRow.idfsFormType);
            //��������� ������� � ������
            treeTemplates.AppendTemplatesNodes(parentNode, MainDbService.GetTemplatesByFormType(formTypesRow.idfsFormType));

            #endregion

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// ������������ ����� ������ ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeTemplatesBeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            if (e.Node.Tag is FlexibleFormsDS.FormTypesRow)
            {
            var parentNode = e.Node;
            var formTypesRow = (FlexibleFormsDS.FormTypesRow)e.Node.Tag;
            LoadTemplateNodes(parentNode, formTypesRow);
        }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        private void RefreshRules(long? idfsFormTemplate)
        {
            treeRules.Nodes.Clear();
            if (idfsFormTemplate.HasValue)
            {
                MainDbService.LoadRulesForTemplate(idfsFormTemplate.Value);
                AppendRuleNodes(treeRules, MainDbService.GetRules(idfsFormTemplate.Value));
            }
        }

        /// <summary>
        /// ��������� ������� � ������ ������
        /// </summary>
        /// <param name="parentTree"></param>
        /// <param name="rows"></param>
        private void AppendRuleNodes(TreeList parentTree, IEnumerable<FlexibleFormsDS.RulesRow> rows)
        {
            foreach (var row in rows)
            {
                if (row.IsRowAlive())
                {
                var node = parentTree.AppendNode(row, null, row);
                node.ImageIndex = node.SelectImageIndex = 3;
                node[trlcTreeListParametersColumn] = row.NationalName;
            }
        }
        }

        /// <summary>
        /// ��������� ���������� ������������� ������������� ������� � ��������� �������
        /// </summary>
        private /*EditorRow*/void RefreshDeterminantsInProp()
        {
            var templatesRow = GetActiveTemplate();
            if (templatesRow != null)
            RefreshDeterminantsInProp((FlexibleFormsDS.TemplateDeterminantValuesRow[])MainDbService.MainDataSet.TemplateDeterminantValues.Select(DataHelper.Filter("idfsFormTemplate", templatesRow.idfsFormTemplate)));
        }

        /// <summary>
        /// ��������� ���������� ������������� ������������� ������� � ��������� ������� 
        /// </summary>
        /// <param name="rows"></param>
        private /*EditorRow*/ void RefreshDeterminantsInProp(IEnumerable<FlexibleFormsDS.TemplateDeterminantValuesRow> rows)
        {
            //������� ������ ���, ����� �� ������� ������������� �����
            propTemplateDeterminant.Rows.Clear();

            //���� ������������� ����� �����������, � ������ -- �������������� ����������
            //������ ������ ������� �������� ��� ����� ��������
            foreach (var row in rows)
            {
                if (row.IsRowAlive())
                {
                //��������� ��������� ���: catDeterminantXXX_YYY, ��� XXX - ������, YYY-��� ������������
                //�������������� ���������� ������� (editrow) �� �����
                //������� �������� � Tag ������� ��� ������

                //������� ��� �������� ���������, ��������������� ��� ���� ������
                var categoryRow = GetDeterminantCategory(row);
                //���� ��� ���������, �� �������� ������ � ������� � ���������
                if (categoryRow != null)
                {
                    var editorRow = new EditorRow();
                    editorRow.Properties.Value = row.DeterminantNationalName;
                    editorRow.Properties.ReadOnly = false;
                    editorRow.Tag = row;
                    categoryRow.ChildRows.Add(editorRow);
                }
            }
            }

            /*return editorRow*/
        }

        /// <summary>
        /// ���������� ����� ��������� ������ �� ����� � ���������� �. ���� ������� ���, �� ������ � ����������.
        /// </summary>
        /// <param name="rowCategory"></param>
        /// <returns></returns>
        private CategoryRow GetDeterminantCategory(FlexibleFormsDS.TemplateDeterminantValuesRow rowCategory)
        {
            CategoryRow result = null;

            string categoryName = String.Format("catDeterminant{0}_{1}", rowCategory.idfsFormTemplate, rowCategory.DeterminantTypeDefaultName);

            foreach (BaseRow row in propTemplateDeterminant.Rows)
            {
                if (row.Name.Equals(categoryName))
                {
                    result = (CategoryRow)row;
                    break;
                }
            }
            //���� ����� �� �������, �� ������
            if (result == null)
            {
                result = new CategoryRow { Name = categoryName };
                result.Properties.Caption = rowCategory.DeterminantTypeNationalName;
                propTemplateDeterminant.Rows.Add(result);
            }
            return result;
        }

        /// <summary>
        /// ����������� ������ ������� ������������� ������� ��� �������� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropTemplateDeterminantsResize(object sender, EventArgs e)
        {
            propTemplateDeterminant.RowHeaderWidth = 0;
            propTemplateDeterminant.RecordWidth = propTemplateDeterminant.ClientSize.Width - 26 - propTemplateDeterminant.RowHeaderWidth;
        }

        /// <summary>
        /// ����� �������� � ������� ������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropTemplateCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (treeTemplates.Selection.Count == 0)
                return;

            var treeNode = treeTemplates.Selection[0];
            var templatesRow = treeNode.Tag as FlexibleFormsDS.TemplatesRow;
            if (templatesRow == null)
                return;

            //����� ������������� �����
            if (e.Row.Equals(erTemplateNationalName))
            {
                if (String.IsNullOrEmpty(e.Value.ToString()) && (!templatesRow.IsDefaultNameNull()))
                {
                    templatesRow.NationalName = templatesRow.DefaultName;
                }
            }

            if (templatesRow.IsDefaultNameNull()) templatesRow.DefaultName = DefaultTemplateName;
            if (templatesRow.IsNationalNameNull()) templatesRow.NationalName = nationalTemplateName;

            treeNode[trlcTemplateTreeListColumn] = templatesRow.NationalName.Length > 0
                                                       ? templatesRow.NationalName
                                                       : templatesRow.DefaultName;

            propTemplate.SelectedObject = null;
            propTemplate.SelectedObject = templatesRow;
        }

        /// <summary>
        /// ���������� ������ ��� ���������� ����������� ������
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetItemsString(int count)
        {
            if (count == 1)
                return EidssMessages.Get("item");

            return count > 1 ? EidssMessages.Get("items") : String.Empty;
        }

        /// <summary>
        /// ������ ������ ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoSearchParameters(object sender, EventArgs e)
        {
            if (tbSearchParameterCriteria.Text.Length == 0 && tbSearchSectionCriteria.Text.Length == 0)
                return;

            Cursor = Cursors.WaitCursor;

            var parameterCriteria = tbSearchParameterCriteria.Text;
            var sectionCriteria = tbSearchSectionCriteria.Text;

            MainDbService.LoadParametersSearch(parameterCriteria, sectionCriteria);
            lbSearchParameterResults.DataSource = MainDbService.MainDataSet.ParametersSearch;

            int count = MainDbService.MainDataSet.ParametersSearch.Rows.Count;
            lSearchParameterResults.Text = count > 0 ? String.Format(EidssMessages.Get("ResultsFound"), count) : EidssMessages.Get("NoResults");

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// ����� ������-������ �������� � ������ ��������� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLbSearchParameterResultsDoubleClick(object sender, EventArgs e)
        {
            if (lbSearchParameterResults.SelectedItem == null)
                return;

            string path = ((DataRowView)lbSearchParameterResults.SelectedItem).Row["FullPathIdfs"].ToString();
            string[] pathElements = path.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (pathElements.Length < 2)
                return;

            //������ ��������� ������ ��� Form Type (idfsFormTypeID)
            //����� ����� ���� �����-�� ���������� ������ idfsSectionID (>=0)
            //��������� ������ ��� idfsParameterID

            TreeListNode nodeFormType = null;

            //������� ��� �����
            foreach (TreeListNode node in treeParameters.Nodes)
            {
                var row = (FlexibleFormsDS.FormTypesRow)node.Tag;
                if (row.idfsFormType.Equals(Convert.ToInt64(pathElements[0])))
                {
                    nodeFormType = node;
                    nodeFormType.Expanded = true;
                    break;
                }
            }
            //���� �� ������� ������-�� ����� ��� �����, �������
            if (nodeFormType == null)
                return;

            //���� ���� �����-�� ������, �� ������� ��
            var nodeParentSection = nodeFormType; //�� ������ ������, �� �� ��������
            for (int i = 1; i < (pathElements.Length - 1); i++)
            {
                foreach (TreeListNode node in nodeParentSection.Nodes)
                {
                    if (node.Tag is FlexibleFormsDS.SectionsRow)
                    {
                    var row = (FlexibleFormsDS.SectionsRow)node.Tag;
                    if (row.idfsSection.Equals(Convert.ToInt64(pathElements[i])))
                    {
                        nodeParentSection = node;
                        nodeParentSection.Expanded = true;
                        break;
                    }
                }
            }
            }

            //���������� ��� ��������� � ����� �� ����
            //nodeParentSection ������ ����� ��� ������������ ������ ��� ���� �����
            foreach (TreeListNode node in nodeParentSection.Nodes)
            {
                if (node.Tag is FlexibleFormsDS.ParametersRow)
                {
                var row = (FlexibleFormsDS.ParametersRow)node.Tag;
                if (row.idfsParameter.Equals(Convert.ToInt64(pathElements[pathElements.Length - 1])))
                {
                    //�������� ������ -- ������� �� ����
                    treeParameters.Selection.Clear();
                    treeParameters.Selection.Add(node);
                    OnTreeParametersClick(treeParameters, null);
                    treeParameters.MakeNodeVisible(node);
                    break;
                }
            }
            }

        }

        /// <summary>
        /// ����������� ������ �������� ��������� ��� ���������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLbSearchParameterResultsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSearchParameterResults.SelectedItem != null)
            {
            var row = ((DataRowView)lbSearchParameterResults.SelectedItem).Row;
            lbSearchParameterResults.ToolTipTitle = row["NationalName"].ToString();
            lbSearchParameterResults.ToolTip = row["NationalLongName"].ToString();
        }
        }

        /// <summary>
        /// ������ �������������� � ������ ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersMouseDown(object sender, MouseEventArgs e)
        {
            dragStartHitInfo = treeParameters.CalcHitInfo(new Point(e.X, e.Y));
        }

        private TreeListHitInfo dragStartHitInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && dragStartHitInfo != null && dragStartHitInfo.Node != null)
            {
                var dragSize = SystemInformation.DragSize;
                var dragRect = new Rectangle(new Point(dragStartHitInfo.MousePoint.X - dragSize.Width / 2, dragStartHitInfo.MousePoint.Y - dragSize.Height / 2), dragSize);
                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    var nodeDrag = treeParameters.Selection[0];
                    treeParameters.DoDragDrop(nodeDrag, DragDropEffects.Move);//All);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersDragOver(object sender, DragEventArgs e)
        {
            var hi = treeParameters.CalcHitInfo(treeParameters.PointToClient(new Point(e.X, e.Y)));
            if (hi.HitInfoType == HitInfoType.Empty || hi.Node != null)
                e.Effect = DragDropEffects.Move;// &DragDropEffects.Scroll;

            else
                e.Effect = DragDropEffects.None;
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersDragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeListNode)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            //���, ������� ���������������
            var nodeSource = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
            var hi = treeParameters.CalcHitInfo(treeParameters.PointToClient(new Point(e.X, e.Y)));
            if (hi.HitInfoType == HitInfoType.Empty || hi.Node == null)
                return;

            //���, ������� �������� ����� ��������������
            var nodeTarget = hi.Node;
            if (nodeSource == null || nodeSource.Equals(nodeTarget))
                return;

            //����-�������� �� ����� ���� �������� �� ��������� � ����-���������
            if (HelperFunctions.CheckForChildNode(nodeSource, nodeTarget))
                return;

            //��������� ����� ���� ������ ������ ��� ��� �����
            if (!(nodeTarget.Tag is FlexibleFormsDS.FormTypesRow)
                && !(nodeTarget.Tag is FlexibleFormsDS.SectionsRow))
                return;
            //��������������� ���� ������ ������������� � �������, � �������� ������ ��� �����
            var rowSource = (DataRow)nodeSource.Tag;
            var rowTarget = (DataRow)nodeTarget.Tag;
            if (!rowSource["idfsFormType"].Equals(rowTarget["idfsFormType"]))
                return;

            //���-������� ����������� ��������
            nodeTarget.Expanded = true;
            //� ����������� �� ��������� ������ ����� ����
            if (nodeSource.Tag is FlexibleFormsDS.ParametersRow)
            {
                var row = (FlexibleFormsDS.ParametersRow)nodeSource.Tag;
                //���� �������� ������������ � �����-������ �������, �� ������ ��� ����������
                if (MainDbService.IsParameterUsed(row.idfsParameter))
                    return;

                //����������� ������ ����� ��������
                if (nodeTarget.Tag is FlexibleFormsDS.FormTypesRow)
                {
                    //��� ��� �����
                    row.SetidfsSectionNull();
                }
                else
                {
                    //��� ������
                    row.idfsSection = ((FlexibleFormsDS.SectionsRow)nodeTarget.Tag).idfsSection;
                }
                //
                nodeSource.ParentNode.Nodes.Remove(nodeSource);
                treeParameters.AppendParameterNodes(nodeTarget, new[] { row }, false);
            }
            if (nodeSource.Tag is FlexibleFormsDS.SectionsRow)
            {
                var row = (FlexibleFormsDS.SectionsRow)nodeSource.Tag;
                //���� ������ ������������ � �����-������ �������, �� ������ � ����������
                if (MainDbService.IsSectionUsed(row.idfsSection))
                    return;

                //����������� ������ ����� ��������
                if (nodeTarget.Tag is FlexibleFormsDS.FormTypesRow)
                {
                    //��� ��� �����
                    row.idfsFormType = ((FlexibleFormsDS.FormTypesRow)nodeTarget.Tag).idfsFormType;
                    row.SetidfsParentSectionNull();
                }
                else
                {
                    var rowSection = (FlexibleFormsDS.SectionsRow)nodeTarget.Tag;
                    //��� ������
                    row.idfsParentSection = rowSection.idfsSection; //TODO ������������� ����� ������
                    //���� ������������ ������ ���������, �� ��� � ��� � ������� ���� ������ ����� ����������
                    if (rowSection.blnGrid) SetSectionType(nodeSource, rowSection.blnGrid);
                }
                nodeSource.ParentNode.Nodes.Remove(nodeSource);
                MainDbService.AppendSectionNodes(treeParameters, nodeTarget, new[] { row }, true, true);
            }
            //�������� ������������ ��� ���, ���� ��� ���������� ������ ��������
            nodeTarget.Expanded = true;
            //������� ������� ���������� �����
            RefreshOrder(nodeSource);
            //������� ���������� ����������
            RefreshParametersLibrary(true);
        }

        /// <summary>
        /// ���������� ������ � ���� � �������� ������� ������� ����, ��������� �� ��� ������
        /// </summary>
        /// <param name="nodeSection"></param>
        /// <param name="isGrid"></param>
        private static void SetSectionType(TreeListNode nodeSection, bool isGrid)
        {
            var rowSection = (FlexibleFormsDS.SectionsRow)nodeSection.Tag;
            rowSection.blnGrid = isGrid;
            //�������� ���� ���� ������ ���������� ������������
            SetImageForSectionType(isGrid, nodeSection);
            //�������� � ��������
            nodeSection.Expanded = true;
            //������� �������-������ ���� ������� ���������
            foreach (TreeListNode nodeChild in nodeSection.Nodes)
            {
                if (nodeChild.Tag is FlexibleFormsDS.SectionsRow)
                    SetSectionType(nodeChild, isGrid);
            }
        }

        /// <summary>
        /// �������� ��������� ����� � ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnParametersUpOrder(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0)
                return;
            TreeListNode nodeSelected = treeParameters.Selection[0];
            if (nodeSelected.Tag is FlexibleFormsDS.FormTypesRow)
                return;
            if (nodeSelected == nodeSelected.ParentNode.FirstNode)
                return;
            treeParameters.SetNodeIndex(nodeSelected, nodeSelected.ParentNode.Nodes.IndexOf(nodeSelected.PrevNode));
            RefreshOrder(nodeSelected);
        }

        /// <summary>
        /// � �������� ������������� ���� ����������� ���������� �������
        /// </summary>
        /// <param name="node"></param>
        private static void RefreshOrder(TreeListNode node)
        {
            if (node.ParentNode == null) return;
            //� ������� ��������� ���� ��������� ��������� intOrder
            for (var i = 0; i < node.ParentNode.Nodes.Count; i++)
            {
                if (node.ParentNode.Nodes[i].Tag is FlexibleFormsDS.SectionsRow)
                    ((FlexibleFormsDS.SectionsRow)node.ParentNode.Nodes[i].Tag).intOrder = i;

                if (node.ParentNode.Nodes[i].Tag is FlexibleFormsDS.ParametersRow)
                    ((FlexibleFormsDS.ParametersRow)node.ParentNode.Nodes[i].Tag).intOrder = i;
                }
            }

        /// <summary>
        /// �������� ��������� ���� � ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnParametersDownOrder(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0)
                return;
            TreeListNode nodeSelected = treeParameters.Selection[0];
            if (nodeSelected.Tag is FlexibleFormsDS.FormTypesRow)
                return;
            if (nodeSelected == nodeSelected.ParentNode.LastNode)
                return;
            treeParameters.SetNodeIndex(nodeSelected, nodeSelected.ParentNode.Nodes.IndexOf(nodeSelected.NextNode));
            RefreshOrder(nodeSelected);
        }

        const string DefaultTemplateName = "New Template";
        const string DefaultRuleName = "New Rule";
        const string DefaultSectionName = "New Section";
        const string DefaultParameterName = "New Parameter";
        const string DefaultLabelName = "New Label";
        string nationalTemplateName { get; set; }
        string nationalRuleName { get; set; }
        string nationalSectionName { get; set; }
        string nationalParameterName { get; set; }
        string nationalLabelName { get; set; }

        /// <summary>
        /// ���������� ������ �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddTemplate(object sender, ItemClickEventArgs e)
        {
            //����� ������ �� ���� ���� �����
            if (treeTemplates.Selection.Count == 0)
                return;
            if (treeTemplates.Selection[0].Tag == null)
                return;
            var rowFormType = treeTemplates.Selection[0].Tag as FlexibleFormsDS.FormTypesRow;
            if (rowFormType == null)
                return;
            treeTemplates.Selection[0].Expanded = true;

            #region ������ ����� ������
            var rowTemplate = MainDbService.MainDataSet.Templates.NewTemplatesRow();
            rowTemplate.idfsFormType = rowFormType.idfsFormType;
            //���� ��� ������ ������ � ���� ���� �����, �� ������� ��� UNI
            rowTemplate.blnUNI = (MainDbService.GetTemplatesByFormType(rowFormType.idfsFormType).Length == 0);

            //����������, ������� ��������, � ������������� �� ���������, ��� ���������� � �������
            int count = MainDbService.MainDataSet.Templates.Select(String.Format("[DefaultName] like '{0}%'", DefaultTemplateName)).Length;

            rowTemplate.DefaultName = String.Format("{0}{1}", DefaultTemplateName, count + 1);
            rowTemplate.NationalName = String.Format("{0}{1}", nationalTemplateName, count + 1);

            rowTemplate.NationalLongName = nationalTemplateName;
            rowTemplate.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            MainDbService.MainDataSet.Templates.AddTemplatesRow(rowTemplate);
            #endregion

            //��������� ���� �� ��������� ������ � ������ ��������
            var parentNode = treeTemplates.Selection[0];
            treeTemplates.AppendTemplatesNodes(parentNode, new[] { rowTemplate });
            //��������� language layout
            miTemplateLanguage.EditValue = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            //����� �� ��������� ���� � ����� ��������
            treeTemplates.FocusedNode = parentNode.LastNode;
            OnTreeTemplatesClick(sender, null);
        }

        /// <summary>
        /// �������� ������������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnDeleteTemplate(object sender, ItemClickEventArgs e)
        {
            //����� ������ �� ���� ���� �����
            if (treeTemplates.Selection.Count == 0)
                return;
            if (treeTemplates.Selection[0].Tag == null)
                return;
            var templatesRow = treeTemplates.Selection[0].Tag as FlexibleFormsDS.TemplatesRow;
            if (templatesRow == null)
                return;

            #region ��������

            //��������, ��� �� ���������� �� �������
            if (!MainDbService.CanDeleteTemplate(templatesRow))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("TemplateRemove_Has_Observation"));
                return;
            }

            //UNI-������ ������� ������
            if (templatesRow.blnUNI)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("UNI_template_cantbe_deleted"));
                return;

                #region ������� ������-������

                /*
                //�������� ��� ������� ����� ���� �����
                if (!MainDbService.GetTemplateInFFTypeWithUNICredentionals(templatesRow))
                {
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("TemplateRemove_Must_Have_Another_Template"));
                    return;
                }
                */
                #endregion
            }

            #endregion

            var result = WinUtils.ConfirmMessage(EidssMessages.Get("TemplateDeleteQuestion"), String.Empty);
            if (!result)
                return;

            ffRender.ClearTemplate();

            //���� ������ ������� ��� �����������, �� ������ ��� �������
            if ((RowCopiedTemplate != null) && (RowCopiedTemplate.idfsFormTemplate.Equals(templatesRow.idfsFormTemplate))) RowCopiedTemplate = null;

            //������� ��� ������������
            MainDbService.DeleteDeterminantsFromTemplate(templatesRow.idfsFormTemplate);
            //������� ��������� ������
            MainDbService.DeleteSectionTemplate(templatesRow.idfsFormTemplate, String.Empty);
            //������� ��������� ���������
            MainDbService.DeleteParameterTemplate(templatesRow.idfsFormTemplate, String.Empty);
            //������� ��������� �������
            MainDbService.DeleteRulesFromTemplate(templatesRow.idfsFormTemplate);
            //������� ��������� ������
            MainDbService.DeleteLabelTemplate(templatesRow.idfsFormTemplate, String.Empty);
            //������� ��������� �����
            MainDbService.DeleteLineTemplate(templatesRow.idfsFormTemplate, String.Empty);

            //������� ������
            templatesRow.Delete();
            //������� ����
            var nodeParent = treeTemplates.Selection[0].ParentNode;
            nodeParent.Nodes.Remove(treeTemplates.Selection[0]);
            OnTreeTemplatesClick(this, null);
        }

        /// <summary>
        /// �������� ���������� ������� �� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTemplateRemoveObject(object sender, ItemClickEventArgs e)
        {
            var rowTemplate = GetActiveTemplate();
            if (rowTemplate == null)
                return;

            //� ������ ������ ��������� � �������, ���� � ���� ��� ���� observation
            if (MainDbService.HasObservationsForTemplate(rowTemplate.idfsFormTemplate))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("DeleteFromTemplate_Template_Has_Observations"));
                return;
            }

            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() == 0)
                return;

            if (!WinUtils.ConfirmMessage(EidssMessages.Get("TemplateObjectDeleteQuestion"), String.Empty))
                return;

            var parameterHost = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0);

            #region �������� ���������� � ������

            //� ������ ������ ��������� � ������� ��������� � ������, ���� � ���� ��� ���� observation
            //if ((parameterHost is Section) || (parameterHost is SectionTable) || (parameterHost is Parameter))
            if ((parameterHost is Section) || (parameterHost is Parameter))
            {
                //������ ����� �� ������� �������� �������
                var parameter = parameterHost as Parameter;
                var section = parameterHost as Section;
                var sectionTable = parameterHost as SectionTable;
                if ((parameter != null) && (parameter.IdfsParameter.HasValue) && ffRender.ParameterList.ContainsKey(parameter.IdfsParameter.Value) && (parameter.ParameterTemplateRow != null))
                {
                    string ruleNames;
                    if (MainDbService.IsParameterUseInAction(parameter.ParameterTemplateRow, out ruleNames))
                    {
                        ErrorForm.ShowMessageDirect(
                            String.Format(EidssMessages.Get("DeleteFromTemplate_ParameterUseInRuleAction"),
                                          ruleNames));
                        return;
                    }
                    ffRender.ParameterList.Remove(parameter.IdfsParameter.Value);
                }

                if ((sectionTable != null) && (sectionTable.IdfsSection.HasValue))
                {
                    //�� ������ �� �������� � ��������� ������?
                    if (sectionTable.SelectedBand != null)
                    {
                        var sectionTemplateRow = sectionTable.SelectedBand.Tag as FlexibleFormsDS.SectionTemplateRow;
                        var parameterTemplateRow = sectionTable.SelectedBand.Tag as FlexibleFormsDS.ParameterTemplateRow;
                        if ((sectionTemplateRow != null) && (ffRender.SectionTableList.ContainsKey(sectionTemplateRow.idfsSection)))
                        {
                            if (MainDbService.CheckSectionDependItems(sectionTemplateRow.idfsSection, rowTemplate.idfsFormTemplate))
                            {
                                ErrorForm.ShowMessageDirect(EidssMessages.Get("DeleteFromTemplate_Section_Has_Children"));
                                return;
                            }
                            ffRender.SectionTableList.Remove(sectionTemplateRow.idfsSection);
                        }
                        if ((parameterTemplateRow != null) && (ffRender.ParameterList.ContainsKey(parameterTemplateRow.idfsParameter)))
                        {
                            string ruleNames;
                            if (MainDbService.IsParameterUseInAction(parameterTemplateRow, out ruleNames))
                            {
                                ErrorForm.ShowMessageDirect(
                                    String.Format(EidssMessages.Get("DeleteFromTemplate_ParameterUseInRuleAction"),
                                                  ruleNames));
                                return;
                            }
                            ffRender.ParameterList.Remove(parameterTemplateRow.idfsParameter);
                        }
                    }
                    else
                    {
                        //���� ���, �� ��������� ���� ��������� ������
                        //��� �������� �. ����� ��������� �� �������� �� � �� ��������� ���������� ��� ������ �.
                        if ((MainDbService.GetSectionTemplateChildrenRows(sectionTable.IdfsSection.Value, rowTemplate.idfsFormTemplate).Length > 0)
                            || (MainDbService.GetParameterTemplateChildrenRows(sectionTable.IdfsSection.Value, rowTemplate.idfsFormTemplate).Length > 0))
                        {
                            ErrorForm.ShowMessageDirect(EidssMessages.Get("DeleteFromTemplate_Section_Has_Children"));
                            return;
                        }

                        if (ffRender.SectionTableList.ContainsKey(sectionTable.IdfsSection.Value)) ffRender.SectionTableList.Remove(sectionTable.IdfsSection.Value);
                    }
                }
                else if ((section != null) && (section.IdfsSection.HasValue))
                {
                    //��� �������� �. ����� ��������� �� �������� �� � �� ��������� ���������� ��� ������ �.
                    if (MainDbService.CheckSectionDependItems(section.IdfsSection.Value, rowTemplate.idfsFormTemplate))
                    {
                        ErrorForm.ShowMessageDirect(EidssMessages.Get("DeleteFromTemplate_Section_Has_Children"));
                        return;
                    }
                    if (ffRender.SectionList.ContainsKey(section.IdfsSection.Value)) ffRender.SectionList.Remove(section.IdfsSection.Value);
                }
            }

            #endregion

            #region �������� ������������ ���������

            if (parameterHost is Line)
            {
                var linesRow = (FlexibleFormsDS.LinesRow)parameterHost.Tag;
                if (ffRender.LineList.ContainsKey(linesRow.idfDecorElement)) ffRender.LineList.Remove(linesRow.idfDecorElement);
            }

            if (parameterHost is Label)
            {
                var labelsRow = (FlexibleFormsDS.LabelsRow)parameterHost.Tag;
                if (ffRender.LabelList.ContainsKey(labelsRow.idfDecorElement)) ffRender.LabelList.Remove(labelsRow.idfDecorElement);
            }

            #endregion

            //������ � ����� ��������� ������ ���
            parameterHost.Delete();
            ffRender.TemplateDesignerHost.GetSelectedParameterHosts().RemoveAt(0);
            ffRender.TemplateDesignerHost.ClearActive();
            //���������� ��������� �� ����������
            SelectedObjectInParametersLibrary = null;
            //������� �� ������ ����������, ����� ���������������� � �����������
            OnTreeParametersClick(sender, null);
        }

        /// <summary>
        /// ������ ������ "Freeze"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFreezeItemClick(object sender, ItemClickEventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() == 0)
                return;
            var sectTable = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as SectionTable;
            if (sectTable != null && sectTable.SelectedBand != null)
            {
            sectTable.SelectedBand.RootBand.Fixed = btnFreezeSection.Down ? FixedStyle.Left : FixedStyle.None;
            if (sectTable.SelectedBand.Tag is FlexibleFormsDS.SectionTemplateRow)
            {
                var row = (FlexibleFormsDS.SectionTemplateRow)sectTable.SelectedBand.Tag;
                //����� ����� ������ ��� ������
                row.blnFreeze = btnFreezeSection.Down;
            }
            else if (sectTable.SelectedBand.Tag is FlexibleFormsDS.ParameterTemplateRow)
            {
                var row = (FlexibleFormsDS.ParameterTemplateRow)sectTable.SelectedBand.Tag;
                //����� ����� ������ ��� ������
                row.blnFreeze = btnFreezeSection.Down;
            }
        }
        }

        /// <summary>
        /// ���������� ������ �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddRuleItemClick(object sender, ItemClickEventArgs e)
        {
            //����� ������ �� ���� ������� �����
            if (treeTemplates.Selection.Count == 0)
                return;
            if (treeTemplates.Selection[0].Tag == null)
                return;
            var rowTemplate = treeTemplates.Selection[0].Tag as FlexibleFormsDS.TemplatesRow;
            if (rowTemplate == null)
                return;
            //������� ������ ��������� � ������������ ������
            if (rowTemplate.idfsFormTemplate < 0)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("Rule_Cant_Add_Unsaved_Template"));
                return;
            }

            #region ������ ����� ������

            var rowRules = MainDbService.MainDataSet.Rules.NewRulesRow();
            rowRules.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            rowRules.idfsFormTemplate = rowTemplate.idfsFormTemplate;

            //����������, ������� ������, � ������������� �� ���������, ��� ���������� � �������
            int count = MainDbService.MainDataSet.Rules.Select(String.Format("[DefaultName] like '{0}%'", DefaultRuleName)).Length;

            rowRules.DefaultName = String.Format("{0}{1}", DefaultRuleName, count + 1);
            rowRules.NationalName = String.Format("{0}{1}", nationalRuleName, count + 1);

            rowRules.idfsCheckPoint = ((long)FFRuleCheckPointType.OnValueChanged).ToString();
            rowRules.idfsCheckPointDescr = String.Empty;
            rowRules.blnNot = false;
            rowRules.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            rowRules.idfsRuleFunction = -1L;//(long) FFRuleFunctions.Empty;

            MainDbService.MainDataSet.Rules.AddRulesRow(rowRules);

            #endregion

            //��������� ���� �� ��������� ������ � ������ ������
            AppendRuleNodes(treeRules, new[] { rowRules });
            //����� �� ��������� ���� � ����� ��������
            treeRules.FocusedNode = treeRules.Nodes.LastNode;
            //�������� ������ � �����������
            erTemplateRuleConstants.Visible = false;
            //
            OnTreeRulesClick(sender, null);
        }

        /// <summary>
        /// ����� �����-���� ������� ��� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnRuleFunctionClick(object sender, ItemClickEventArgs e)
        {
            //������ ���� ������� �������� �������
            var rowRules = GetActiveRule();
            if (rowRules == null || e.Item.Tag == null)
                return;
            string tag = e.Item.Tag.ToString();
            //"�������" �� ���� ����, ����� ��������� ����������� ��� �����������
            OnTreeRulesClick(sender, null);
            //��� "id �������;��������1;��������2"
            string[] tagsplited = tag.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            //���������� ������� �������
            rowRules.idfsRuleFunction = Convert.ToInt64(tagsplited[0]);
            //��������� ������ ���������� � ������� ��� ������� ����� ����������, �� ������ ��������� � ������ �� ������
            for (int i = MainDbService.MainDataSet.RuleParameterForFunction.Count - 1; i >= 0; i--)
            {
                var row = MainDbService.MainDataSet.RuleParameterForFunction[i];
                if (row.IsRowAlive() &&
                    row.idfsRule.Equals(rowRules.idfsRule))
                    row.Delete();
            }

            //����� ������� �������� ��������, ������� ������������� ����� ������� � ��� ����������
            grdRuleConstants.DataSource = null;
            grdvRuleConstants.EditingValue = null;
            for (int i = MainDbService.MainDataSet.RuleConstant.Count - 1; i >= 0; i--)
            {
                var row = MainDbService.MainDataSet.RuleConstant[i];
                if (row.IsRowAlive() &&
                    row.idfsRule.Equals(rowRules.idfsRule))
                    row.Delete();
            }

            //��� ������� ��������� (�� 1 ��� 2) ���������� ������ � ��������� ��� ���������� � �������
            for (int i = 1; i < tagsplited.Length; i++)
            {
                #region ������ ����� ������

                var rowRuleParameterForFunctionRow = MainDbService.MainDataSet.RuleParameterForFunction.NewRuleParameterForFunctionRow();
                rowRuleParameterForFunctionRow.idfsFormTemplate = rowRules.idfsFormTemplate;
                rowRuleParameterForFunctionRow.idfsRule = rowRules.idfsRule;
                rowRuleParameterForFunctionRow.intOrder = i - 1;
                rowRuleParameterForFunctionRow.idfsParameter = Convert.ToInt64(tagsplited[i]);
                //������� ��� ���������
                var rowParameter =
                    (FlexibleFormsDS.ParameterTemplateRow[])
                    MainDbService.MainDataSet.ParameterTemplate.Select(
                        DataHelper.Filter("idfsParameter", rowRuleParameterForFunctionRow.idfsParameter, "idfsFormTemplate", rowRuleParameterForFunctionRow.idfsFormTemplate));
                if (rowParameter.Length == 1)
                    rowRuleParameterForFunctionRow.idfsParameterType = rowParameter[0].idfsParameterType;
                MainDbService.MainDataSet.RuleParameterForFunction.AddRuleParameterForFunctionRow(rowRuleParameterForFunctionRow);

                #endregion
            }
            rowRules.FunctionText = MainDbService.GetRuleFunctionText(rowRules);
            rowRules.intNumberOfParameters = tagsplited.Length;
            //��������� ��������� ������ ��� ����� ����������������� ��������
            SetRuleConstantVisible(rowRules);
            rowRules.ConstantsText = MainDbService.GetRuleConstantsText(rowRules);
            //�������� �������� ���������� �������
            propTemplateRule.SelectedObject = null;
            propTemplateRule.SelectedObject = rowRules;
            //�� ������ ������ ��������� �� ������� ������
            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateRule;
        }

        /// <summary>
        /// ���������� ��������� ������ ��� ����� �������� ��� ������� � �������
        /// </summary>
        /// <param name="rowRules"></param>
        private void SetRuleConstantVisible(FlexibleFormsDS.RulesRow rowRules)
        {
            //���� �� ������ ������� ��� �������, �� �������
            if (rowRules.IsidfsRuleFunctionNull())
                return;
            //���� ��� ������� ��������������� �������� ���������� ��������, �� ������� ������ ��� �� �����. ����� ��� �� ������ ���� �����.
            var rfRows = (FlexibleFormsDS.RuleFunctionsRow[])MainDbService.MainDataSet.RuleFunctions.Select(DataHelper.Filter("idfsRuleFunction", rowRules.idfsRuleFunction));
            erTemplateRuleConstants.Visible = rfRows.Length == 1 && rfRows[0].idfsRuleFunction.Equals((long)FFRuleFunctions.Value);

            //���� ������ ����� �������� ������, �� ��������� ����� �������� ������������, ������ �� ���� ���������
            if (!erTemplateRuleConstants.Visible)
            {
                //������������ ����� ��������� ������� ������� ��������, ������� ����� ���� ������� �����
                MainDbService.DeleteRuleConstants(rowRules.idfsRule);
                return;
            }
            var rows = (FlexibleFormsDS.RuleParameterForFunctionRow[])MainDbService.MainDataSet.RuleParameterForFunction.Select(DataHelper.Filter("idfsRule", rowRules.idfsRule));
            //������� ��������
            var rowParameter = (FlexibleFormsDS.ParametersRow[])MainDbService.MainDataSet.Parameters.Select(DataHelper.Filter("idfsParameter", rows[0].idfsParameter));
            //����� ���� ������ ���� ��������, ����� ������������ �-��� Arg is Value
            if (rows.Length == 1)
            {
                switch (rows[0].idfsParameterType)
                {
                    case (long)FFParameterTypes.Boolean/*"parBoolean"*/:
                        colRuleConstants.ColumnEdit = constantsBoolean;
                        break;
                    case (long)FFParameterTypes.Date/*"parDate"*/:
                        colRuleConstants.ColumnEdit = constantsDateTime;
                        break;
                    case (long)FFParameterTypes.DateTime/*"parDateTime"*/:
                        colRuleConstants.ColumnEdit = constantsDateTime;
                        break;
                    case (long)FFParameterTypes.String/*"parString"*/:
                        colRuleConstants.ColumnEdit = constantsText;
                        break;
                    case (long)FFParameterTypes.Numeric /*"parNumeric"*/:
                    case (long)FFParameterTypes.NumericNatural /*"parNumeric"*/:
                    case (long)FFParameterTypes.NumericPositive /*"parNumeric"*/:
                    case (long)FFParameterTypes.NumericInteger /*"parNumeric"*/:
                        colRuleConstants.ColumnEdit = constantsInt;
                        break;
                    default:
                        //��� ��������� ���� ������, � ������ -- �������
                        colRuleConstants.ColumnEdit = constantsFixedPreset;
                        if (rowParameter.Length == 1)
                            constantsFixedPreset.DataSource = MainDbService.LoadParameterSelectList(rowParameter[0].idfsParameter, rowParameter[0].idfsParameterType, rowParameter[0].intHACode);
                        break;
                }
            }
        }

        /// <summary>
        /// ���� �� ������ ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeRulesClick(object sender, EventArgs e)
        {
            propTemplateRule.SelectedObject = null;
            FlexibleFormsDS.RulesRow rowRules = GetActiveRule();
            if (rowRules == null) return;
            //������� ��������� ����-������ ������� ��� ������
            MainDbService.LoadRuleFunctions(-1); //-1 ������, ��� ��� ������� ��� ����
            //��������� ������ �� ���������� �������, ������� � ���� �������
            MainDbService.LoadRuleParameterForFunction(rowRules.idfsRule);
            //��������� �������� �� �������
            MainDbService.LoadRuleParameterForAction(rowRules.idfsRule);
            //��������� ��������� (��� ����������������� ��������) �� �������
            MainDbService.LoadRuleConstants(rowRules.idfsRule);
            //��������������� ����� �������
            rowRules.FunctionText = MainDbService.GetRuleFunctionText(rowRules);
            //������������� ���������� ����� ��� ��������
            rowRules.ActionText = String.Format("{0}{1}", MainDbService.MainDataSet.RuleParameterForAction.Select(DataHelper.Filter("idfsRule", rowRules.idfsRule)).Length, EidssMessages.Get("actions"));
            //��������� ��������� ������ ����� ����������������� ��������
            SetRuleConstantVisible(rowRules);
            rowRules.ConstantsText = MainDbService.GetRuleConstantsText(rowRules);
            //�������� �������� ���������� �������
            propTemplateRule.SelectedObject = rowRules;
            //������������� �� ������� ������
            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateRule;
        }

        /// <summary>
        /// ��������� ������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropRuleInTemplateCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (treeRules.Selection.Count == 0) return;
            if (e.Row.Equals(erTemplateRuleNationalName))
            {
                treeRules.Selection[0][trlcTreeListParametersColumn] = e.Value;
            }
            else if (e.Row.Equals(erTemplateRuleNot))
            {
                var rowRules = (FlexibleFormsDS.RulesRow)treeRules.Selection[0].Tag;
                rowRules.FunctionText = MainDbService.GetRuleFunctionText(rowRules);
                //�������� �������� ���������� �������
                //propTemplateRule.SelectedObject = null;
                //propTemplateRule.SelectedObject = rowRules;
            }
        }

        /// <summary>
        /// ���������� ����� ��������� � �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnRuleAddConstantItemClick(object sender, ItemClickEventArgs e)
        {
            FlexibleFormsDS.RulesRow rowRules = GetActiveRule();
            if (rowRules == null ||
                rowRules.IsidfsRuleFunctionNull())
                return;

            #region ������ ����� ������

            FlexibleFormsDS.RuleConstantRow rowConstant = MainDbService.MainDataSet.RuleConstant.NewRuleConstantRow();
            rowConstant.idfsRule = rowRules.idfsRule;
            MainDbService.MainDataSet.RuleConstant.AddRuleConstantRow(rowConstant);

            #endregion

            RefreshRuleConstants();
        }

        /// <summary>
        /// �������� ��������� �� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnRuleDeleteConstantItemClick(object sender, ItemClickEventArgs e)
        {
            int[] handles = grdvRuleConstants.GetSelectedRows();
            foreach (int handle in handles)
            {
                var row = (DataRow)grdvRuleConstants.GetRow(handle);
                if (row != null)
                {
                    row.Delete();
                    RefreshRuleConstants();
                }

            }
        }

        /// <summary>
        /// ������ ���� � ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersClick(object sender, EventArgs e)
        {
            if (treeParameters.Selection.Count == 0)
                return;

            SetButtonEnabled(btnAddParameter, true);
            SetButtonEnabled(btnAddSection, true);

            var treeListNode = treeParameters.Selection[0];
            if (treeListNode.Tag == null) return;
            Cursor = Cursors.WaitCursor;
            if (treeListNode.Tag is FlexibleFormsDS.FormTypesRow)
            {
                //������������� ��� ����� ������, ������ ������ ������������� �� ��� �������
                tbcRedParamProperties.SelectedTabPage = tbpRedFormType;
            }
            else if (treeListNode.Tag is FlexibleFormsDS.SectionsRow)
            {
                propSection.Enabled = !ReadOnly;
                btnDeleteParameter.Enabled = false;
                SetButtonEnabled(btnDeleteSection, false);

                #region ����� ����������� ������

                tbcRedParamProperties.SelectedTabPage = tbRedSection;
                var rowSection = (FlexibleFormsDS.SectionsRow)treeListNode.Tag;
                //������� ������ ����
                rowSection.FullPath = GetFullPathForNode(treeListNode);
                //������� ����������� �������� ����� ������ ����� ���������� ������ ���� ��� ��������� ������
                erSectionFixedRowset.Visible = rowSection.blnFixedRowSet;
                //���� � ������ ���� ������������ ������ � ��� ���������, �� ������ ������������� ��� ������ (� ��������� ������ �����)
                FlexibleFormsDS.SectionsRow rowParentSection = null;
                if (!rowSection.IsidfsParentSectionNull())
                    rowParentSection = MainDbService.GetSectionRow(rowSection.idfsParentSection);
                //���� ������ ������������ � �����-���� �������, �� ��������� �� ������, ������� ��� ���� ������ �������������
                erSectionGrid.Properties.ReadOnly =
                    erSectionFixedRowset.Properties.ReadOnly =
                    MainDbService.IsSectionUsed(rowSection.idfsSection) ||
                    ((rowParentSection != null) && (rowParentSection.blnGrid));
                //
                propSection.SelectedObject = rowSection;

                #endregion
            }
            else if (treeListNode.Tag is FlexibleFormsDS.ParametersRow)
            {
                propParameter.Enabled = !ReadOnly;

                btnAddParameter.Enabled = btnAddSection.Enabled = btnDeleteSection.Enabled = false;
                SetButtonEnabled(btnDeleteParameter, false);

                #region ����� ����������� ���������

                //��� ���� ���������, ������� ������ � ������
                var rowParameter = (FlexibleFormsDS.ParametersRow)treeParameters.Selection[0].Tag;
                //��������� ���������� ������ � ������� ��� ����������� � �������
                propParameter.SelectedObject = rowParameter;
                //������� ������ ����
                rowParameter.FullPath = GetFullPathForNode(treeListNode);
                //���� �������� ������������ � �����-���� �������, �� ��������� �� ������, ������� ��� ���� ������ �������������
                erParameterScheme.Properties.ReadOnly
                    = erParameterType.Properties.ReadOnly
                      = erParameterEditors.Properties.ReadOnly
                        = erParameterWidth.Properties.ReadOnly
                          = erParameterHeight.Properties.ReadOnly
                            = erParameterLabelSize.Properties.ReadOnly
                              = erParameterHACode.Properties.ReadOnly //= false;
                                = MainDbService.IsParameterUsed(rowParameter.idfsParameter);
                //��������� ��������� ���������� ������ � ������� ��� ����������� � �������
                propParameter.SelectedObject = rowParameter;
                //��������� ���������� ������������� �������������� ���������
                if (parEditingParameter == null)
                {
                    //���� �� �� ������, �� �������� ��� � �������� �� �����-����
                    parEditingParameter = new Parameter
                                              {
                                                  NowLoading = true,
                                                  MainDbService = MainDbService,
                                                  Location = new Point(10, 10),
                                                  Name = "parEditingParameter"
                                              };
                    //��� ������ ����������� ���������
                    parEditingParameter.SizeChanged += OnParEditingParameterSizeChanged;
                    parEditingParameter.m_Splitter.SplitterMoved += OnParEditingParameterSizeChanged;

                    pnlParameterDesignHost.Add(parEditingParameter);
                    parEditingParameter.NowLoading = false;
                }
                parEditingParameter.Tag = rowParameter;
                parEditingParameter.Scheme = ParameterSchemeHelper.ConvertToParameterScheme(rowParameter.intScheme);
                parEditingParameter.Editor = ParameterControlTypeHelper.ConvertToParameterControlType(rowParameter.idfsEditor);
                parEditingParameter.Text = rowParameter.NationalName;
                //� ����������� �� ����� ��������� �������� �������� ������
                parEditingParameter.LabelSize = rowParameter.intLabelSize;
                //������ ����������� ��� �������� ������ ���������� ���������
                parEditingParameter.Size = new Size(rowParameter.intWidth, rowParameter.intHeight);

                //��������� ����������� ������������
                //���������� �������� ����� ����������, ������������ �� ������� �������� �����
                //���� ����� ������, ��� ��������, �� �� ����� ��������� ��� ��������������

                if ((pnlParameterDesignHost.Width > parEditingParameter.Width) &&
                    (pnlParameterDesignHost.Height > parEditingParameter.Height))
                {
                    parEditingParameter.Location =
                        new Point((pnlParameterDesignHost.Width - parEditingParameter.Width) / 2,
                                  (pnlParameterDesignHost.Height - parEditingParameter.Height) / 2);
                }

                #region ��������� ��������� ��������

                var dv =
                    LookupCache.Get(LookupTables.ParameterTypeReferenceEditor);
                var rows = dv.Table.Select(DataHelper.Filter("idfsParameterType", rowParameter.idfsParameterType));
                if (rows.Length == 1)
                {
                    SetFilterControlTypeEditors(rowParameter.idfsParameterType);
                }

                #endregion

                #region ���������� ������ ��������, ��� ������������ ���� ��������

                MainDbService.LoadTemplates(rowParameter.idfsFormType);
                var idfsParameter = rowParameter.idfsParameter;
                // = MainDbService.MainDataSet.ParameterTemplate.Select(DataHelper.Filter("idfsParameter", rowParameter.idfsParameter)).to;
                MainDbService.LoadTemplatesByParameter(idfsParameter);

                //������� � �������� ����������� �� �������
                foreach (var templateRow in MainDbService.MainDataSet.TemplatesByParameter)
                {
                    //��� ��� ��������, ��� ��� �� �����������, ��������. ��, ������� ���������, �������� ����������� �� �����
                    MainDbService.LoadParameterTemplate(templateRow.idfsFormTemplate);
                }

                //������� �� �������, ������� ��� ��� �� �������
                for (var i = MainDbService.MainDataSet.TemplatesByParameter.Count - 1; i >= 0; i--)
                {
                    var idfsFormTemplate = MainDbService.MainDataSet.TemplatesByParameter[i].idfsFormTemplate;
                    if (!MainDbService.MainDataSet.Templates.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplate)).Any()
                        ||
                        (!MainDbService.MainDataSet.ParameterTemplate.Select(DataHelper.Filter("idfsParameter",
                                                                                               idfsParameter,
                                                                                               "idfsFormTemplate",
                                                                                               idfsFormTemplate)).Any()))
                    {
                        MainDbService.MainDataSet.TemplatesByParameter.Rows.RemoveAt(i);
                    }
                }

                //��������� �� �������, ������� ���������� �� �������
                var paramsTemplate = MainDbService.MainDataSet.ParameterTemplate.Select(DataHelper.Filter("idfsParameter", idfsParameter));
                foreach (var parameterTemplate in paramsTemplate)
                {
                    var idfsFormTemplate = ((FlexibleFormsDS.ParameterTemplateRow)parameterTemplate).idfsFormTemplate;
                    if (MainDbService.MainDataSet.TemplatesByParameter.Count(t => t.idfsFormTemplate == idfsFormTemplate) == 0)
                    {
                        var template = MainDbService.GetTemplateRow(idfsFormTemplate);
                        if (template == null) continue;

                        var row = (FlexibleFormsDS.TemplatesByParameterRow)MainDbService.MainDataSet.TemplatesByParameter.NewRow();
                        row.DefaultName = template.DefaultName;
                        row.NationalName = template.NationalName;
                        MainDbService.MainDataSet.TemplatesByParameter.AddTemplatesByParameterRow(row);
                    }
                }


                var count = MainDbService.MainDataSet.TemplatesByParameter.Rows.Count;
                grpTemplatesByParameter.Text = count > 0
                                                   ? String.Format(EidssMessages.Get("TemplatesWhichUseThisParameter"),
                                                                   count, GetItemsString(count))
                                                   : EidssMessages.Get("ThereAreNoTemplatesWhichUseThisParameter");

                lbTemplatesByParameter.DataSource = MainDbService.MainDataSet.TemplatesByParameter;

                #endregion

                //������������ �� ������� ����������
                tbcRedParamProperties.SelectedTabPage = tbRedParameter;

                #endregion
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// ����� ���� 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersLibraryClick(object sender, EventArgs e)
        {
            if (treeParametersLibrary.Selection.Count == 0)
                return;
            //���� ������ ����.����, �� ������� ���������
            if (treeParametersLibrary.Selection[0].Tag.Equals(StrPointer))
            {
                SelectedObjectInParametersLibrary = null;
            }
            else if ((treeParametersLibrary.Selection[0].Tag is FlexibleFormsDS.SectionsRow) || (treeParametersLibrary.Selection[0].Tag is FlexibleFormsDS.ParametersRow))
            {
                SelectedObjectInParametersLibrary = treeParametersLibrary.Selection[0].Tag;
            }
        }

        /// <summary>
        /// ��������� ���� � ������ ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeTemplatesClick(object sender, EventArgs e)
        {
            propTemplate.SelectedObject = null;

            propTemplate.Enabled = !ReadOnly;
            SetButtonEnabled(btnAddTemplate, true);
            SetButtonEnabled(btnDeleteTemplate, false);

            var templatesRow = GetActiveTemplate();
            if (templatesRow == null)
            {
                btnDeleteTemplate.Enabled = false;
                return;
            }

            btnAddTemplate.Enabled = false;

            Cursor = Cursors.WaitCursor;
            //�������� �������� �������
            propTemplate.SelectedObject = templatesRow;

            #region ��������� �������������

            MainDbService.LoadTemplateDeterminants(templatesRow.idfsFormTemplate);
            var rows = (FlexibleFormsDS.TemplateDeterminantValuesRow[])MainDbService.MainDataSet.TemplateDeterminantValues.Select(DataHelper.Filter("idfsFormTemplate", templatesRow.idfsFormTemplate));

            //��������� ������� �������������
            //���� ���� � ���������, �������� ���� � ������������� ��������
            RefreshDeterminantsInProp(rows);

            //��������� ������� UNI, ���� �� ���������, ������ ��� ������� ��� ����� ������
            //erTemplateUNI.Enabled = !templatesRow.blnUNI;

            #endregion

            #region �������� ���������� ����������, ������� �������� ��� ������� �������

            RefreshParametersLibrary(false);

            #endregion

            #region ��������������� ������ �� ������� ����� ��������

            ffRender.RestoreTemplate(templatesRow.idfsFormTemplate, templatesRow.idfsFormType, LanguageTemplate);

            #endregion

            #region ���������� �������� ������ ��� ����� �������

            RefreshRules(templatesRow.idfsFormTemplate);

            #endregion

            //���������, ����� ������ ���������� �� �������� ���������� ������� �����
            //������ ���� ��������� ���� ����������, �� ����� ������ ��������
            if (!miTemplateLanguage.EditValue.Equals(bv.common.Core.Localizer.lngEn))
            {
                //[0] - select, [1] - undo
                var hasLanguageTemplate = HasLanguageTemplate(templatesRow.idfsFormTemplate, miTemplateLanguage.EditValue.ToString());

                leTemplateLanguage.Buttons[1].Visible = hasLanguageTemplate;
            }
            else
            {
                leTemplateLanguage.Buttons[1].Visible = false;
            }

            //���������� ��������� �� ����������
            SelectedObjectInParametersLibrary = null;
            //������������� �� ������� ��������, ��� �������� �������, ������� ������������
            tbcTemplateComponentsProperties.SelectedTabPage = tbpTemplateProperties;

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// ��������� ���������� ���������� ����������
        /// </summary>
        /// <param name="mustRefresh">����� �� ������������� �������� ����������</param>
        private void RefreshParametersLibrary(bool mustRefresh)
        {
            if (treeTemplates.Selection.Count == 0) return;
            if (treeTemplates.Selection[0].ParentNode == null) return;
            //���� ����� ������������� ���������� ����������, �� ������ ������ �� �� �� (����� ������� ��������),
            //� ����� �������� ������ (�����, �������� �������������, �� �����������������)
            if (mustRefresh)
            {
                treeParametersLibrary.Nodes.Clear();
            }
            //������ � ������ ���������� ���������� ���� ����� ���� ����� � ������������� ���
            //���� ���� ���� ��� ����� ������, �� �� ������� ���
            var rowFormType = ((FlexibleFormsDS.FormTypesRow)treeTemplates.Selection[0].ParentNode.Tag);
            var needRefresh = mustRefresh || ((treeParametersLibrary.Nodes.Count == 0) ||
                                              (!treeParametersLibrary.Nodes[0].Tag.Equals(rowFormType)));

            if (!needRefresh) return;

            //������ ��� �� ��������-�������� ���������, ������� ������ � ��, ��� ��� ���
            if (treeParametersLibrary.Nodes.Count > 1)
                treeParametersLibrary.Nodes.RemoveAt(1);
            else
            {
                //������ ����������� ���� -- ������
                var node = treeParametersLibrary.AppendNode(null, null, StrPointer);
                node[trlcTreeListParametersColumn] = StrPointer;
                node.ImageIndex = node.SelectImageIndex = 4;
            }
            //�� ������ ������ ��������� ���������� ������ � ���������� �� ����� ���� �����
            //������������ ���������� ������
            if (!mustRefresh)
            {
                MainDbService.LoadSections(rowFormType.idfsFormType, null, null);
                //������������ ���������� ����������
                //����� ������ ��, ��� �� ��������� � �����-���� ������, � ������ � ���� �����
                MainDbService.LoadParameters(rowFormType.idfsFormType, null);
            }
            //������� ��������� ������ �� ����� ���� �����
            rowFormType.HasNestedSections = MainDbService.HasNestedSections(rowFormType.idfsFormType);
            rowFormType.HasParameters = MainDbService.HasNestedParameters(rowFormType.idfsFormType);
            //��������� ���� � ����� �����
            AppendFormTypeNode(treeParametersLibrary, rowFormType, true);
        }

        /// <summary>
        /// ����������, ������� �� �������� ����������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        private bool HasLanguageTemplate(long idfsFormTemplate, string langid)
        {
            var filterstring = DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid), "blnIsRealStruct", 1);
            var sectionTemplateRows = (FlexibleFormsDS.SectionTemplateRow[])MainDbService.MainDataSet.SectionTemplate.Select(filterstring);
            var parameterTemplateRows = (FlexibleFormsDS.ParameterTemplateRow[])MainDbService.MainDataSet.ParameterTemplate.Select(filterstring);
            var labelsRows = MainDbService.GetLabelTemplateRows(idfsFormTemplate, langid);
            var linesRows = MainDbService.GetLineTemplateRows(idfsFormTemplate, langid);

            return ((sectionTemplateRows.Length > 0) || (parameterTemplateRows.Length > 0) || (labelsRows.Length > 0) || (linesRows.Length > 0));
        }

        /// <summary>
        /// ���������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddParameterItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0) return;
            var nodeParent = treeParameters.Selection[0];
            object tag = nodeParent.Tag;
            //����� ����������� � ������ ��� ���� �����
            var rowParentSection = tag as FlexibleFormsDS.SectionsRow;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            if ((rowParentSection == null) && (rowFormType == null)) return;
            //�������� ������������ ���
            nodeParent.Expanded = true;

            #region ������ ����� ������

            var rowParameter = MainDbService.MainDataSet.Parameters.NewParametersRow();
            if (rowParentSection != null)
            {
                rowParameter.idfsSection = rowParentSection.idfsSection;
                rowParameter.idfsFormType = rowParentSection.idfsFormType;
            }
            else
            {
                rowParameter.idfsFormType = rowFormType.idfsFormType;
            }
            rowParameter.intScheme = 0;
            rowParameter.idfsParameterType = 10071045; //parString
            rowParameter.ParameterTypeName = "String"; //��� ����� ����� ������� � ���������
            rowParameter.idfsEditor = 10067008; //editText
            rowParameter.intOrder = 0;
            rowParameter.intHACode = 0; //????
            rowParameter.intLabelSize = 75;
            rowParameter.intLeft = 0;
            rowParameter.intTop = 0;
            rowParameter.intWidth = 150;
            rowParameter.intHeight = 100;
            rowParameter.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            rowParameter.IsRealParameter = 1;
            
            //����������, ������� ����������, � ������������� �� ��������� ��� ���������� � �������
            var count = MainDbService.MainDataSet.Parameters.Select(String.Format("[DefaultName] like '{0}%'", DefaultParameterName)).Length;

            rowParameter.DefaultName = rowParameter.DefaultLongName = String.Format("{0}{1}", DefaultParameterName, count + 1);
            rowParameter.NationalName = rowParameter.NationalLongName = String.Format("{0}{1}", nationalParameterName, count + 1);

            //������� ������
            MainDbService.MainDataSet.Parameters.AddParametersRow(rowParameter);
            //���� ����������� � ������, �� ���� �������� ���� ������, ��� ��� ������ �������� ���������

            //������� ���� ��� ����� �������
            treeParameters.AppendParameterNodes(nodeParent, new[] { rowParameter }, false);

            //������������� ������� ����������
            RefreshOrder(nodeParent);

            #endregion

            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);

            //�� ������ ������ ��� ��� �������� ������������ ���, ���� �� ����� � ��� �� ���� �������� �����
            nodeParent.Expanded = true;
            //����� �� ���� � ����� ��������
            treeParameters.FocusedNode = nodeParent.LastNode;
            OnTreeParametersClick(sender, null);
        }



        /// <summary>
        /// �������� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnDeleteParameterItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0) return;
            var nodeParent = treeParameters.Selection[0];
            var rowParameter = nodeParent.Tag as FlexibleFormsDS.ParametersRow;
            if (rowParameter == null) return;

            #region ��������

            //��������, �� ������� �� ���� ������ �����������
            if (ffRender.CriticalObjects.Parameters.ContainsKey(rowParameter.idfsParameter))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("msgCantDeleteCriticalObject"));
                return;
            }

            //�������� �������� �������� ��� ���� �� Form Type, ��� � ��������� ��������
            MainDbService.LoadTemplates(rowParameter.idfsFormType);
            foreach (var row in MainDbService.MainDataSet.Templates)
            {
                if (!row.IsRowAlive()) continue;
                MainDbService.LoadParameterTemplate(row.idfsFormTemplate);
                MainDbService.LoadSectionTemplate(row.idfsFormTemplate);
                MainDbService.LoadRulesForTemplate(row.idfsFormTemplate);
                var rules = (FlexibleFormsDS.RulesRow[])MainDbService.MainDataSet.Rules.Select(DataHelper.Filter("idfsFormTemplate", row.idfsFormTemplate));
                foreach (FlexibleFormsDS.RulesRow rulesRow in rules)
                {
                    if (!rulesRow.IsRowAlive()) continue;
                    MainDbService.LoadRuleParameterForFunction(rulesRow.idfsRule);
                    MainDbService.LoadRuleParameterForAction(rulesRow.idfsRule);
                }
            }
            if (MainDbService.MainDataSet.ParameterTemplate.Select(DataHelper.Filter("idfsParameter", rowParameter.idfsParameter)).Length > 0)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("ParameterRemove_Has_ffParameterForTemplate_Rows"));
                return;
            }
            if (MainDbService.MainDataSet.RuleParameterForFunction.Select(DataHelper.Filter("idfsParameter", rowParameter.idfsParameter)).Length > 0)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("ParameterRemove_Has_Rules_Rows"));
                return;
            }
            if (MainDbService.MainDataSet.RuleParameterForAction.Select(DataHelper.Filter("idfsParameter", rowParameter.idfsParameter)).Length > 0)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("ParameterRemove_Has_Rules_Rows"));
                return;
            }
            //��������, ����� �� � ������� �� ������
            if (MainDbService.IsParameterUsed(rowParameter.idfsParameter))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("ParameterRemove_Has_tlbAggrMatrixVersion_Rows"));
                return;
            }

            #endregion

            if (!WinUtils.ConfirmMessage(EidssMessages.Get("ParameterDeleteQuestion"), String.Empty)) return;

            //���� �������� ������� ��� �����������, �� ������ ��� �������
            if ((RowCopiedParameter != null) && (RowCopiedParameter.idfsParameter.Equals(rowParameter.idfsParameter))) RowCopiedParameter = null;

            //���������� ���� �������� � ������� � ������� ���
            //for (var i = MainDbService.MainDataSet.Parameters.Count - 1; i >= 0; i-- )
            //{
            //    var parameter = MainDbService.MainDataSet.Parameters[i];
            //    if (!parameter.IsRowAlive()) continue;
            //    if (!rowParameter.IsRowAlive()) continue;
            //    if (parameter.idfsParameter == rowParameter.idfsParameter)
            //    {
            //        MainDbService.MainDataSet.Parameters.RemoveParametersRow(parameter);
            //    }
            //}

            rowParameter.Delete();

            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);

            var pn = nodeParent.ParentNode;
            pn.Nodes.Remove(nodeParent);
            //������� ���� ������������ ����
            pn.Selected = true;
            OnTreeParametersClick(sender, null);
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddSectionItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0) return;
            var nodeParent = treeParameters.Selection[0];
            //���� ������� ����������� �������� ����������, �� �� ��� ��������� �����
            if (nodeParent.Level >= 30)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("GainedMaxNestLevel", "Can't add Section. The maximum nest level is reached."));
                return;
            }

            var tag = nodeParent.Tag;
            //����� ����������� � ������ ��� ���� �����
            var rowParentSection = tag as FlexibleFormsDS.SectionsRow;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            if ((rowParentSection == null) && (rowFormType == null)) return;
            //�������� ������������ ���
            nodeParent.Expanded = true;

            #region ������ ����� ������

            var rowSection = MainDbService.MainDataSet.Sections.NewSectionsRow();
            if (rowParentSection != null)
            {
                rowSection.idfsParentSection = rowParentSection.idfsSection;
                rowSection.idfsFormType = rowParentSection.idfsFormType;
                rowSection.blnGrid = rowParentSection.blnGrid;
                rowSection.blnFixedRowSet = rowParentSection.blnFixedRowSet;
            }
            else
            {
                rowSection.idfsFormType = rowFormType.idfsFormType;
                rowSection.blnGrid = rowSection.blnFixedRowSet = false;
            }

            //����������, ������� ������, � ������������� �� ��������� ��� ���������� � �������
            var count = MainDbService.MainDataSet.Sections.Select(String.Format("[DefaultName] like '{0}%'", DefaultSectionName)).Length;
            rowSection.DefaultName = String.Format("{0}{1}", DefaultSectionName, count + 1);
            rowSection.NationalName = String.Format("{0}{1}", nationalSectionName, count + 1);
            rowSection.intOrder = 0;
            rowSection.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            //��� ����� � ��������� � ���������� ������ (������ ��� �������� ��������)
            if (rowSection.IsidfsParentSectionNull() || rowSection.idfsParentSection == 0)
            {
                var ft = MainDbService.MainDataSet.FormTypes.FirstOrDefault(c => c.idfsFormType == rowSection.idfsFormType);
                if ((ft != null) && !ft.IsidfsMatrixTypeNull()) rowSection.idfsMatrixType = ft.idfsMatrixType;
            }
            
            //������� ������
            MainDbService.MainDataSet.Sections.AddSectionsRow(rowSection);
            //���� ����������� � ������, �� ���� �������� ���� ������, ��� ��� ������ �������� ������ ������
            //if (rowParentSection != null) rowParentSection.HasNestedSections = true;

            //������� ���� ��� ����� �������
            var nodeNew = MainDbService.AppendSectionNodes(treeParameters, nodeParent, new[] { rowSection }, true, true);
            //�������� ���� ����
            nodeNew.Expanded = true;

            //������������� ������� ����������
            RefreshOrder(nodeParent);

            #endregion

            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);

            //�� ������ ������ ��� ��� �������� ������������ ���, ���� �� ����� � ��� �� ���� �������� �����
            nodeParent.Expanded = true;
            //����� �� ��������� ���� � ����� ��������
            treeParameters.FocusedNode = nodeParent.LastNode;
            OnTreeParametersClick(sender, null);
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnDeleteSectionItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0) return;
            var nodeParent = treeParameters.Selection[0];
            var rowSection = nodeParent.Tag as FlexibleFormsDS.SectionsRow;
            if (rowSection == null) return;

            #region ��������

            //��������, �� ������� �� ���� ������ �����������
            if (ffRender.CriticalObjects.Sections.ContainsKey(rowSection.idfsSection))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("msgCantDeleteCriticalObject"));
                return;
            }

            //�������� �������� �������� ��� ���� �� Form Type, ��� � ��������� ��������
            MainDbService.LoadTemplates(rowSection.idfsFormType);
            foreach (FlexibleFormsDS.TemplatesRow row in MainDbService.MainDataSet.Templates)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                MainDbService.LoadParameterTemplate(row.idfsFormTemplate);
                MainDbService.LoadSectionTemplate(row.idfsFormTemplate);
            }

            //���� �� ������� ������ ���, �� ��������, ��� �� ���������� �� �������
            if (MainDbService.HasNestedSections(rowSection))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("SectionRemove_Has_ffSection_ParentSection_Rows"));
                return;
            }
            if (MainDbService.HasNestedParameters(rowSection))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("SectionRemove_Has_ffParameter_Rows"));
                return;
            }
            if (MainDbService.MainDataSet.SectionTemplate.Select(DataHelper.Filter("idfsSection", rowSection.idfsSection)).Length > 0)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("SectionRemove_Has_ffSectionForTemplate_Rows"));
                return;
            }

            //TODO ��������� �������� ��� ����� � ������� (����� ffDecorElement)

            #endregion

            bool result = WinUtils.ConfirmMessage(EidssMessages.Get("SectionDeleteQuestion"), String.Empty);
            if (!result) return;

            //���� ������ �������� ��� �����������, �� ������ ��� �������
            if ((RowCopiedSection != null) && (RowCopiedSection.idfsSection.Equals(rowSection.idfsSection))) RowCopiedSection = null;

            rowSection.Delete();

            //������������� ������� ���������� ����������
            RefreshParametersLibrary(true);

            TreeListNode pn = nodeParent.ParentNode;
            pn.Nodes.Remove(nodeParent);
            //������� ���� ������������ ����
            pn.Selected = true;
            OnTreeParametersClick(sender, null);
        }

        /// <summary>
        /// �������� ������ � ������ ���������� ��� �����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnCopySectionOrParameterItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeParameters.Selection.Count == 0) return;
            object tag = treeParameters.Selection[0].Tag;
            //���������� ����� ������ ������ � ���������
            var rowSection = tag as FlexibleFormsDS.SectionsRow;
            var rowParameter = tag as FlexibleFormsDS.ParametersRow;
            if ((rowSection == null) && (rowParameter == null)) return;
            if (rowSection != null)
            {
                RowCopiedSection = rowSection;
                RowCopiedParameter = null;
            }
            else
            {
                RowCopiedSection = null;
                RowCopiedParameter = rowParameter;
            }
        }

        /// <summary>
        /// ������� ������ ��� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnPasteSectionOrParameterItemClick(object sender, ItemClickEventArgs e)
        {
            //������������ ����� ��� ������� ����� ���� ������ ��� ����� ��� ������
            if (treeParameters.Selection.Count == 0) return;
            var nodeParent = treeParameters.Selection[0];
            object tag = nodeParent.Tag;
            var rowSection = tag as FlexibleFormsDS.SectionsRow;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            if ((rowSection == null) && (rowFormType == null)) return;
            //�������� ������������ ���
            nodeParent.Expanded = true;

            //����� ������ ���� ������� ������ �����������
            if ((RowCopiedSection == null) && (RowCopiedParameter == null)) return;

            if (RowCopiedParameter != null)
            {
                //���� ���������� ��������, �� ������ ������ � ���� �����
                var arr = new FlexibleFormsDS.ParametersRow[1];
                arr[0] = RowCopiedParameter;
                CopyParameters(nodeParent, arr);
            }
            else
            {
                //��������, ����� ������ ���� ��������� �������������, ����� ����� �� �������� �����������, � ��� ��������� ��������
                var listNodes = new List<TreeListNode>();
                nodeParent.CollectExpandedNodes(listNodes);

                //���� ���������� ������, �� ����� ������� ��� � �������� ������ � ��������� � ���� �� �����������
                //�������� ��������� ������ ��� ���� �����, ����� ����������� ��� ������
                nodeParent.ExpandAll();

                var arr = new FlexibleFormsDS.SectionsRow[1];
                arr[0] = RowCopiedSection;
                CopySections(nodeParent, arr);

                //������ ��� �� ������, ��� �� ���� ��������� �������������
                nodeParent.CollapseNodes(listNodes);
            }
        }

        /// <summary>
        /// ������ ����� �������
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="template"></param>
        /// <param name="needTransferUNI"></param>
        private FlexibleFormsDS.TemplatesRow CopyTemplate(TreeListNode targetNode, FlexibleFormsDS.TemplatesRow template, bool needTransferUNI)
        {
            var formTypesRow = targetNode.Tag as FlexibleFormsDS.FormTypesRow;
            if (formTypesRow == null) return null;

            var rulesRows = MainDbService.GetRules(template.idfsFormTemplate);

            foreach (var row in rulesRows)
            {
                if (MainDbService.CheckRuleForNew(row))
                {
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("Cant_Use_Unsaved_Rule"));
                    return null;
                }
            }
            var rowTemplate = CopyHelper.CopyTemplate(MainDbService, template, formTypesRow.idfsFormType, needTransferUNI);

            //�������� ����� ���� �������������� ������� � ������
            treeTemplates.AppendTemplatesNodes(targetNode, new[] { rowTemplate });

            return rowTemplate;
        }

        /// <summary>
        /// ������ ����� ������
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="targetNode"></param>
        private void CopySections(TreeListNode targetNode, FlexibleFormsDS.SectionsRow[] sections)
        {
            //��� ������� ��������� ������ ��� ����� � ����������� � �������� ����
            object tag = targetNode.Tag;
            var rowParentSection = tag as FlexibleFormsDS.SectionsRow;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            if ((rowParentSection == null) && (rowFormType == null)) return;

            for (var i = 0; i < sections.Length; i++)
            {
                var row = sections[i];

                #region ������ ����� ������

                var rowSection = MainDbService.MainDataSet.Sections.NewSectionsRow();
                if (rowParentSection != null)
                {
                    rowSection.idfsParentSection = rowParentSection.idfsSection;
                    rowSection.idfsFormType = rowParentSection.idfsFormType;
                }
                else
                {
                    rowSection.idfsFormType = rowFormType.idfsFormType;
                }
                rowSection.DefaultName = row.DefaultName;
                rowSection.NationalName = row.NationalName;
                //rowSection.HasParameters = row.HasParameters;
                //rowSection.HasNestedSections = row.HasNestedSections;
                rowSection.blnGrid = row.blnGrid;
                if ((rowParentSection != null) && (rowParentSection.blnGrid))
                    rowSection.blnGrid = rowParentSection.blnGrid;
                rowSection.blnFixedRowSet = row.blnFixedRowSet;
                rowSection.intOrder = 0;
                //������� ������
                MainDbService.MainDataSet.Sections.AddSectionsRow(rowSection);

                //������� ����� ��� ���� ����� ������
                var nodeNewSection = MainDbService.AppendSectionNodes(treeParameters, targetNode, new[] { rowSection }, true, true);

                //����������� �������� ������
                //��������� ��� ������, ������������� ��� �������-��������
                CopySections(nodeNewSection, MainDbService.GetSectionChildrenRows(row.idfsSection));

                //����������� ��������� ����������
                //��������� ��� ���������, ����������� � ������-�������
                CopyParameters(nodeNewSection, MainDbService.GetParameterChildrenRows(row.idfsSection));

                #endregion
            }

            //������������� ������� ����������
            RefreshOrder(targetNode);
        }

        /// <summary>
        /// ������ ����� ����������
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="targetNode">����, ���� ���� ��������� ������������� ���������</param>
        private void CopyParameters(TreeListNode targetNode, FlexibleFormsDS.ParametersRow[] parameters)
        {
            //��� ������� ��������� ������ ��� ����� � ����������� � �������� ����
            object tag = targetNode.Tag;
            var rowSection = tag as FlexibleFormsDS.SectionsRow;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            if ((rowSection == null) && (rowFormType == null)) return;

            var parametersNew = new FlexibleFormsDS.ParametersRow[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var row = parameters[i];
                if (!row.IsRowAlive()) continue;

                #region ������ ����� ������

                var rowParameter = MainDbService.MainDataSet.Parameters.NewParametersRow();
                if (rowSection != null)
                {
                    rowParameter.idfsSection = rowSection.idfsSection;
                    rowParameter.idfsFormType = rowSection.idfsFormType;
                }
                else
                {
                    rowParameter.idfsFormType = rowFormType.idfsFormType;
                }
                rowParameter.intScheme = row.intScheme;
                rowParameter.idfsParameterType = row.idfsParameterType;
                rowParameter.ParameterTypeName = row.ParameterTypeName;
                rowParameter.idfsEditor = row.idfsEditor;
                rowParameter.intOrder = row.intOrder;
                rowParameter.intHACode = row.intHACode;
                rowParameter.intLabelSize = row.intLabelSize;
                rowParameter.intLeft = row.intLeft;
                rowParameter.intTop = row.intTop;
                rowParameter.intWidth = row.intWidth;
                rowParameter.intHeight = row.intHeight;
                rowParameter.DefaultName = row.DefaultName;
                rowParameter.NationalName = row.NationalName;
                rowParameter.NationalLongName = row.NationalLongName;
                rowParameter.DefaultLongName = row.DefaultLongName;
                rowParameter.langid = row.langid;
                rowParameter.strNote = row.strNote;
                //������� ������
                MainDbService.MainDataSet.Parameters.AddParametersRow(rowParameter);

                parametersNew[i] = rowParameter;

                #endregion
            }

            //������� ��� ���� ��� ����� ������� (��� ����� ����)
            treeParameters.AppendParameterNodes(targetNode, parametersNew, false);

            //������������� ������� ����������
            RefreshOrder(targetNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnPasteTemplateItemClick(object sender, ItemClickEventArgs e)
        {
            //������������ ����� ��� ������� ����� ���� ������ ��� ����� ��� ������
            if (treeTemplates.Selection.Count == 0) return;
            var nodeParent = treeTemplates.Selection[0];
            object tag = nodeParent.Tag;
            var formTypesRow = tag as FlexibleFormsDS.FormTypesRow;
            if (formTypesRow == null) return;
            //�������� ������������ ���
            nodeParent.Expanded = true;

            //����� ������ ���� ������� ������ �����������
            if (RowCopiedTemplate == null) return;

            //��������
            CopyTemplate(nodeParent, RowCopiedTemplate, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnCopyTemplateItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeTemplates.Selection.Count == 0) return;
            object tag = treeTemplates.Selection[0].Tag;
            var rowTemplate = tag as FlexibleFormsDS.TemplatesRow;
            if (rowTemplate != null)
            {
                RowCopiedTemplate = rowTemplate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPopupTemplates(object sender, EventArgs e)
        {
            if (treeTemplates.Selection.Count == 0)
            {
                btnCopyTemplate.Enabled = btnPasteTemplate.Enabled = btnMakeSuccessor.Enabled = false;
                return;
            }

            //������ ����������� ����������, ����� ������ ����� �� ���� �����. ������ ������� �������� ��������.
            //������ ����������� ����������, ���� ��� ����� ��������� ���������� �� ���� ����� ���������� (��-�� ��������� ����������)
            //������ "������� ���������" �������� ������ �� �������
            object tag = treeTemplates.Selection[0].Tag;
            var rowFormType = tag as FlexibleFormsDS.FormTypesRow;
            var rowTemplates = tag as FlexibleFormsDS.TemplatesRow;
            bool isCopiedTemplateNotNull = RowCopiedTemplate != null;
            bool isFormType = rowFormType != null;
            bool isTemplate = rowTemplates != null;
            bool isFormTypeEqual = isCopiedTemplateNotNull && isFormType && (rowFormType.idfsFormType.Equals(RowCopiedTemplate.idfsFormType));
            btnCopyTemplate.Enabled = !isFormType;
            btnPasteTemplate.Enabled = isCopiedTemplateNotNull && isFormTypeEqual;
            btnMakeSuccessor.Enabled = isTemplate;
        }

        /// <summary>
        /// ����� ����� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiTemplateLanguageEditValueChanged(object sender, EventArgs e)
        {
            if (!(miTemplateLanguage.EditValue is string)) return;
            LanguageTemplate = miTemplateLanguage.EditValue.ToString();
            OnTreeTemplatesClick(sender, e);
        }

        /// <summary>
        /// ����, � ������� ����������� ������
        /// </summary>
        private string LanguageTemplate { get; set; }

        

        /// <summary>
        /// ��������� ������� ��������� � �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropParameterInTemplateCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1) return;
            var selectedParameter = (Parameter)ffRender.TemplateDesignerHost.GetSelectedParameterHost(0);

            //����� ����������� ����� ���������
            if (e.Row.Equals(erTemplateParameterScheme))
            {
                selectedParameter.Scheme = ParameterSchemeHelper.ConvertToParameterScheme(Convert.ToInt32(e.Value));
            }
            int val;
            //����� ������
            if (e.Row.Equals(erTemplateParameterWidth))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int width = val;
                    if (width < Parameter.DefaultParameterSize.Width) width = Parameter.DefaultParameterSize.Width;
                    if (width > Parameter.MaxParameterSize.Width) width = Parameter.MaxParameterSize.Width;
                    selectedParameter.Width = width;
                }
            }
            //����� ������
            if (e.Row.Equals(erTemplateParameterHeight))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int height = val;
                    if (height < Parameter.DefaultParameterSize.Height) height = Parameter.DefaultParameterSize.Height;
                    if (height > Parameter.MaxParameterSize.Height) height = Parameter.MaxParameterSize.Height;
                    selectedParameter.Height = height;
                }
            }
            //����� ������ ���������
            if (e.Row.Equals(erTemplateParameterLabelSize))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int labelsize = val;
                    if (labelsize < Parameter.DefaultLabelSize) labelsize = Parameter.DefaultLabelSize;
                    selectedParameter.LabelSize = labelsize;
                }
            }
            //����� ������ ����� ���������
            if (e.Row.Equals(erTemplateParameterEditMode))
            {
                if (Convert.ToInt64(e.Value).Equals((long)FFEditModes.Required))
                {
                    SetControlMandatoryState(selectedParameter.ControlParameter);
                }
                else
                {
                    //TODO ������� ������� � ���������� �����   
                }
            }
            selectedParameter.ParameterMoveAndResize(selectedParameter, e);
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnDeleteRuleItemClick(object sender, ItemClickEventArgs e)
        {
            var rulesRow = GetActiveRule();
            if (rulesRow == null) return;

            bool result = WinUtils.ConfirmMessage(EidssMessages.Get("RuleDeleteQuestion"), String.Empty);
            if (!result) return;

            MainDbService.DeleteRule(rulesRow);
            //������ ���� �������
            TreeListNode node = treeRules.Selection[0];
            treeRules.Nodes.Remove(node);
            OnTreeRulesClick(this, null);
        }

        /// <summary>
        /// ���������, ����� �� ��������� ����� ��� ����� � ������
        /// </summary>
        /// <param name="rowTemplate"></param>
        /// <param name="parentSection">���������� ������, ���� ������������ ������� ����� ��������� � ��</param>
        /// <returns></returns>
        private bool CanAddDecorateElementToTemplate(FlexibleFormsDS.TemplatesRow rowTemplate, out FlexibleFormsDS.SectionTemplateRow parentSection)
        {
            parentSection = null;
            if (rowTemplate == null) return false;

            //���������, ������ �� ������ �����-������ ������ �� ������� ����� ��������
            //����� ��������� ����� ������ � ���� ������� ��� � ������
            //��� ���� ������ �� ������ ���� ����������
            var list = ffRender.TemplateDesignerHost.GetSelectedParameterHosts();
            if (list.Count > 1) return false;
            if ((list.Count == 1) && (list[0].Tag is FlexibleFormsDS.SectionTemplateRow))
            {
                parentSection = (FlexibleFormsDS.SectionTemplateRow)list[0].Tag;
                var sectionsRow = MainDbService.GetSectionRow(parentSection.idfsSection);
                //����������, �� �������� �� ��� ������ ��� � �������� ������ ��� ������������. ������������ ������ � ��������� ������ ��������� � ������.
                if (MainDbService.CheckSectionForNew(sectionsRow)) return false;
                //���� ������ ���������, �� ���� �������
                if (parentSection.blnGrid) return false;
            }
            //� ������ ������ ��������� � �������, ���� � ���� ��� ���� observation
            if (MainDbService.HasObservationsForTemplate(rowTemplate.idfsFormTemplate))
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("AddInTemplate_Template_Has_Observations"));
                return false;
            }
            return true;
        }

        /// <summary>
        /// ���������� ����� � ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddLineToTemplateItemClick(object sender, ItemClickEventArgs e)
        {
            FlexibleFormsDS.TemplatesRow rowTemplate = GetActiveTemplate();
            if (rowTemplate == null) return;

            FlexibleFormsDS.SectionTemplateRow parentSection;
            if (!CanAddDecorateElementToTemplate(rowTemplate, out parentSection)) return;

            #region ��������� ������ � ������� ������

            var rowLines = MainDbService.MainDataSet.Lines.NewLinesRow();
            rowLines.idfsDecorElementType = (long)FFDecorElementTypes.Line;
            //������ ������ ��� ������������� � ������ ������ �����

            rowLines.langid = Utils.IsEmpty(miTemplateLanguage.EditValue) ? bv.model.Model.Core.ModelUserContext.CurrentLanguage : miTemplateLanguage.EditValue.ToString();
            rowLines.idfsFormTemplate = rowTemplate.idfsFormTemplate;
            if (parentSection != null) rowLines.idfsSection = parentSection.idfsSection;
            rowLines.SetblnOrientationNull();
            rowLines.intLeft = rowLines.intTop = 5;
            rowLines.intWidth = 100;
            rowLines.intHeight = 15;
            rowLines.intColor = Color.Black.ToArgb();

            MainDbService.MainDataSet.Lines.AddLinesRow(rowLines);

            #endregion

            ffRender.CreateLineInTemplateHost(rowLines);
        }

        /// <summary>
        /// ��������� �������� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropTemplateLineCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1) return;
            var line = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Line;
            if (line == null) return;
            var linesRow = (FlexibleFormsDS.LinesRow)line.Tag;

            //����� X
            if (e.Row.Equals(erLineLeft))
            {
                if (e.Value != null)
                {
                    int i;
                    if (Int32.TryParse(e.Value.ToString(), out i))
                    {
                        if (i < 0) i = 0;
                        line.Left = i;
                    }
                }
            }
            //����� Y
            if (e.Row.Equals(erLineTop))
            {
                if (e.Value != null)
                {
                    int i;
                    if (Int32.TryParse(e.Value.ToString(), out i))
                    {
                        if (i < 0) i = 0;
                        line.Top = i;
                    }
                }
            }
            //����� ������
            if (e.Row.Equals(erLineWidth))
            {
                if (e.Value != null)
                {
                    int i;
                    if (Int32.TryParse(e.Value.ToString(), out i))
                    {
                        if (i < Line.DefaultLineSize.Width) i = Line.DefaultLineSize.Width;
                        if (i > Line.MaxLineSize.Width) i = Line.MaxLineSize.Width;
                        line.Width = i;
                    }
                }
            }
            //����� ������
            if (e.Row.Equals(erLineHeight))
            {
                if (e.Value != null)
                {
                    int i;
                    if (Int32.TryParse(e.Value.ToString(), out i))
                    {
                        if (i < Line.DefaultLineSize.Height) i = Line.DefaultLineSize.Height;
                        if (i > Line.MaxLineSize.Height) i = Line.MaxLineSize.Height;
                        line.Height = i;
                    }
                }
            }
            //����� �����
            if (e.Row.Equals(erLineColor))
            {
                if (e.Value is Color)
                {
                    var clr = (Color)e.Value;
                    linesRow.intColor = clr.ToArgb();
                    line.BackColor = clr;
                }
            }
            //����� ���������� �����
            if (e.Row.Equals(erLineOrientation))
            {
                if (e.Value != null)
                {
                    int i;
                    if (Int32.TryParse(e.Value.ToString(), out i))
                    {
                        if (i == -1) linesRow.SetblnOrientationNull();
                        if (i == 1) linesRow.blnOrientation = true;
                        if (i == 0) linesRow.blnOrientation = false;
                        line.ResizeLine();
                    }
                }
            }

            propTemplateLine.SelectedObject = null;
            propTemplateLine.SelectedObject = linesRow;
        }

        /// <summary>
        /// ���������� ������ ������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddDeterminantItemClick(object sender, ItemClickEventArgs e)
        {
            var templatesRow = GetActiveTemplate();
            if (templatesRow == null) return;
            //�������� �� ����������� ���������� ������������
            //if (DataHelper.HasObservationsForTemplate(MainDbService, templatesRow.idfsFormTemplate))
            //{
            //    ErrorForm.ShowMessageDirect(EidssMessages.Get("Determinant_Save_Cant_Change_Because_Has_Observations", null, null));
            //    return;
            //}

            //������� ��������� �������� �������������
            MainDbService.LoadDeterminants();
            if (MainDbService.MainDataSet.Determinants.Count == 0) return;
            //
            var ffDeterminants = new FFDeterminants(MainDbService, templatesRow);
            BaseFormManager.ShowModal(ffDeterminants, FindForm());
            if ((ffDeterminants.SelectedDeterminantsRow == null)) return;
            //���� ����� ����������� ��� ��� ��������, �� �������� ��� �� ���������
            if (MainDbService.HasTemplateDeterminant(templatesRow.idfsFormTemplate, ffDeterminants.SelectedDeterminantsRow.idfsBaseReference)) return;
            //���� ����� �������� ���� UNI+������ �� ���� �������, �� ���� �� ������� UNI ������ ����� � ���������� ����
            //������ ����� ������� (���� ������ UNI)
            if (templatesRow.blnUNI && ffDeterminants.SelectedDeterminantsRow.idfsReferenceType.Equals((long)FFDeterminantTypes.Country))
            {
                var templates =
                    MainDbService.GetTemplateWithSameDeterminant(ffDeterminants.SelectedDeterminantsRow.idfsBaseReference, templatesRow.idfsFormType, true);
                foreach (var templateDeterminantValuesRow in templates)
                {
                    var template = MainDbService.GetTemplateRow(templateDeterminantValuesRow.idfsFormTemplate);
                    if (template == null) return;
                    //���� ������������ �� ����� ������� ������� UNI � ���������� �������, ����� ������� ������ �� ���� �������, ������ �� ���� ������ �� ������ �����������-������
                    if (!WinUtils.ConfirmMessage(
                            String.Format(EidssMessages.Get("Determinants_Question_Remove_UNI"),
                                          template.NationalName), String.Empty)) return;
                    template.blnUNI = false;
                }
            }
            //���� ���� ������ �� UNI � � ������ ���� ����� ��� �� ������ ������� UNI ��� ������������� ��� � �������������-������� �������, �� ������ ���� ������ UNI
            bool wasSetAsUNI = false;
            if (!templatesRow.blnUNI)
            {
                if (!MainDbService.GetTemplateInFFTypeWithUNICredentionals(templatesRow))
                {
                    templatesRow.blnUNI = true;
                    wasSetAsUNI = true;
                    //�����������
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("TemplateUNI_This_Template_Set_UNI"));
                }
            }
            string errStr;
            //���������, ����� �� ���� ������ ��������
            if (!MainDbService.CheckValidDeterminantCombinations(templatesRow, ffDeterminants.SelectedDeterminantsRow.idfsBaseReference, true, HelperFunctions.GetDeterminantType(ffDeterminants.SelectedDeterminantsRow.idfsReferenceType), out errStr))
            {
                ErrorForm.ShowMessageDirect(errStr);
                return;
            }

            MainDbService.CreateTemplateDeterminantValuesRow(ffDeterminants.SelectedDeterminantsRow, templatesRow.idfsFormTemplate, templatesRow.idfsFormType);
            if (wasSetAsUNI) OnTreeTemplatesClick(sender, e); else RefreshDeterminantsInProp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnDeleteDeterminantItemClick(object sender, ItemClickEventArgs e)
        {
            var editorRow = propTemplateDeterminant.FocusedRow as EditorRow;
            if (editorRow == null)
                return;

            var templateDeterminantValuesRow = (FlexibleFormsDS.TemplateDeterminantValuesRow)editorRow.Tag;
            if (templateDeterminantValuesRow == null ||
                !templateDeterminantValuesRow.IsRowAlive())
                return;

            var templatesRow = GetActiveTemplate();
            if (templatesRow == null ||
                !templatesRow.IsRowAlive())
                return;

            //�������� �� ����������� �������� ������������
            //if (DataHelper.HasObservationsForTemplate(MainDbService, templateDeterminantValuesRow.idfsFormTemplate))
            //{
            //    ErrorForm.ShowMessageDirect(EidssMessages.Get("Determinant_Save_Cant_Change_Because_Has_Observations", null, null));
            //    return;
            //}
            //���� ��������� UNI-������, �� ����� ���������, ����� � ���� �� ���� ����� ���� ������ UNI ������� ���� ��� ������������-�����, ���� � ������� �������
            if (templatesRow.blnUNI)
            {
                //�������� ��� ������� ����� ���� �����
                if (!MainDbService.GetTemplateInFFTypeWithUNICredentionals(templatesRow, false))
                {
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("Determinant_Remove_Must_Have_Another_Template"));
                    return;
                }
            }
            //���������, ����� �� ���� ������ ��������
            string errStr;
            if (!MainDbService.CheckValidDeterminantCombinations(templatesRow, templateDeterminantValuesRow.DeterminantValue, false, HelperFunctions.GetDeterminantType(templateDeterminantValuesRow.DeterminantType), out errStr))
            {
                ErrorForm.ShowMessageDirect(errStr);
                return;
            }

            templateDeterminantValuesRow.Delete();
            RefreshDeterminantsInProp();
        }

        /// <summary>
        /// ������� ��������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnMakeSuccessorItemClick(object sender, ItemClickEventArgs e)
        {
            //�������� ��������������� �����, ����� ������� ��������� ������ �������, ��� ���������� ��� �������
            //��� ���� ���������� ��������� ������� �������� ���������� � ���������� ������� �������������, �� ����
            //������������ ��������� � �������-��������� � ������������ � �������-�����.

            if (treeParameters.Selection.Count == 0)
                return;

            object tag = treeTemplates.Selection[0].Tag;
            var rowTemplateSource = tag as FlexibleFormsDS.TemplatesRow;
            if (rowTemplateSource == null)
                return;

            //��������� ��������� ����� � ��� �� ��� �����, ��� ��������� ������-��������
            var nodeParent = treeTemplates.Selection[0].ParentNode;
            tag = nodeParent.Tag;
            var formTypesRow = tag as FlexibleFormsDS.FormTypesRow;
            if (formTypesRow == null)
                return;

            //�������� ������������ ���
            nodeParent.Expanded = true;
            var rowTemplateSuccessor = CopyTemplate(nodeParent, rowTemplateSource, true);
            if (rowTemplateSuccessor == null)
                return;

            //� �������-������ ������� ������� UNI
            rowTemplateSource.blnUNI = false;

            //�������� ������������ �� ������� �������
            CopyHelper.CopyDeterminants(MainDbService, rowTemplateSource.idfsFormTemplate, rowTemplateSuccessor.idfsFormTemplate);

            //������ �� ������� ������� ��� ������������
            MainDbService.DeleteDeterminantsFromTemplate(rowTemplateSource.idfsFormTemplate);

            //
            OnTreeTemplatesClick(sender, e);
        }

        /// <summary>
        /// ���������� ������ �� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddLabelToTemplateItemClick(object sender, ItemClickEventArgs e)
        {
            var rowTemplate = GetActiveTemplate();
            if (rowTemplate == null)
                return;

            FlexibleFormsDS.SectionTemplateRow parentSection;
            if (!CanAddDecorateElementToTemplate(rowTemplate, out parentSection))
                return;

            #region ��������� ������ � ������� ������

            var rowLabels = MainDbService.MainDataSet.Labels.NewLabelsRow();
            rowLabels.idfsDecorElementType = (long)FFDecorElementTypes.Label;
            rowLabels.langid = Utils.IsEmpty(miTemplateLanguage.EditValue) ? bv.model.Model.Core.ModelUserContext.CurrentLanguage : miTemplateLanguage.EditValue.ToString();
            rowLabels.idfsFormTemplate = rowTemplate.idfsFormTemplate;
            if (parentSection != null) rowLabels.idfsSection = parentSection.idfsSection;
            rowLabels.intLeft = rowLabels.intTop = 5;
            rowLabels.intWidth = 60;
            rowLabels.intHeight = 20;
            rowLabels.intColor = Color.Black.ToArgb();
            rowLabels.intFontSize = 10;
            rowLabels.intFontStyle = (int)FontStyle.Regular;
            rowLabels.DefaultText = DefaultLabelName;
            rowLabels.NationalText = nationalLabelName;

            MainDbService.MainDataSet.Labels.AddLabelsRow(rowLabels);

            #endregion

            ffRender.CreateLabelInTemplateHost(rowLabels);
        }

        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropTemplateLabelCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1)
                return;

            var label = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Label;
            if (label == null)
                return;

            var labelsRow = (FlexibleFormsDS.LabelsRow)label.Tag;
            if (labelsRow == null)
                return;

            if (e.Row.Equals(erLabelDefaultText))
                labelsRow.DefaultText = e.Value == null ? String.Empty : e.Value.ToString();
            if (e.Row.Equals(erLabelNationalText))
                labelsRow.NationalText = e.Value == null ? String.Empty : e.Value.ToString();

            if (labelsRow.IsDefaultTextNull())
                labelsRow.DefaultText = DefaultLabelName;
            if (labelsRow.IsNationalTextNull())
                labelsRow.NationalText = nationalLabelName;
            if (labelsRow.NationalText.Length == 0 && !labelsRow.IsDefaultTextNull())
                labelsRow.NationalText = labelsRow.DefaultText;

            label.mainLabel.Text = labelsRow.NationalText.Length > 0 ? labelsRow.NationalText : labelsRow.DefaultText;

            int fontSize;
            if (e.Row.Equals(erLabelFontSize) &&
                HelperFunctions.GetInt32(e.Value, out fontSize))
                {
                    if (fontSize < Label.MinFontSize) fontSize = Label.MinFontSize;
                    if (fontSize > Label.MaxFontSize) fontSize = Label.MaxFontSize;
                    labelsRow.intFontSize = fontSize;
                    label.mainLabel.Font = new Font(label.mainLabel.Font.FontFamily, labelsRow.intFontSize,
                                                    HelperFunctions.ConvertToFontStyle(labelsRow.intFontStyle));
                }

                int fontStyle;
            if (e.Row.Equals(erLabelFontStyle) &&
                HelperFunctions.GetInt32(e.Value, out fontStyle))
                {
                    labelsRow.intFontStyle = fontStyle;
                    label.mainLabel.Font = new Font(label.mainLabel.Font.FontFamily, label.mainLabel.Font.Size,
                                                    HelperFunctions.ConvertToFontStyle(fontStyle));
                }

            if (e.Row.Equals(erLabelColor) && e.Value is Color)
                {
                    var clr = (Color)e.Value;
                    labelsRow.intColor = clr.ToArgb();
                    label.mainLabel.ForeColor = clr;
                }
            //����� ������
            int val;
            if (e.Row.Equals(erLabelWidth) &&
                HelperFunctions.GetInt32(e.Value, out val))
                {
                    int width = val;
                    if (width < label.MinimumSize.Width) width = label.MinimumSize.Width;
                    if (width > Label.MaxLabelSize.Width) width = Label.MaxLabelSize.Width;
                    labelsRow.intWidth = label.Width = width;
                }
            //����� ������
            if (e.Row.Equals(erLabelHeight) &&
                HelperFunctions.GetInt32(e.Value, out val))
                {
                    int height = val;
                    if (height < label.MinimumSize.Height) height = label.MinimumSize.Height;
                    if (height > Label.MaxLabelSize.Height) height = Label.MaxLabelSize.Height;
                    labelsRow.intHeight = label.Height = height;
                }

            propTemplateLabel.SelectedObject = null;
            propTemplateLabel.SelectedObject = labelsRow;
        }

        /// <summary>
        /// ��������� ������� ������ � �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropSectionInTemplateCellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (ffRender.TemplateDesignerHost.SelectedParameterHostsCount() != 1)
                return;
            var selectedSection = ffRender.TemplateDesignerHost.GetSelectedParameterHost(0) as Section;
            if (selectedSection == null)
                return;

            int val;
            //����� ������
            if (e.Row.Equals(erTemplateSectionWidth))
            {
                if (HelperFunctions.GetInt32(e.Value, out val))
                {
                    int width = val;
                    if (width < Section.DefaultSectionSize.Width) width = Section.DefaultSectionSize.Width;
                    if (width > Section.MaxSectionSize.Width) width = Section.MaxSectionSize.Width;
                    selectedSection.Width = width;
                }

            }
            //����� ������
            if (e.Row.Equals(erTemplateSectionHeight) &&
                HelperFunctions.GetInt32(e.Value, out val))
                {
                    int height = val;
                    if (height < Section.DefaultSectionSize.Height) height = Section.DefaultSectionSize.Height;
                    if (height > Section.MaxSectionSize.Height) height = Section.MaxSectionSize.Height;
                    selectedSection.Height = height;
                }

            //����� ������ ���������
            if (e.Row.Equals(erTemplateSectionCaptionHeight) &&
                HelperFunctions.GetInt32(e.Value, out val))
                {
                    int captionHeight = val;
                    if (captionHeight < FFRender.DefaultCaptionHeight) captionHeight = FFRender.DefaultCaptionHeight;
                    selectedSection.CaptionHeight = captionHeight;
                }

            selectedSection.SectionMoveAndResize(selectedSection, e);
        }

        /// <summary>
        /// ������������� ��������� �� ������� ����� ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnFitItemClick(object sender, ItemClickEventArgs e)
        {
            //��������� ��� ������� ����� ������ ���� "� ������" ������� ���� �������� ��� ������� ������� ������
            var rowTemplate = GetActiveTemplate();
            if (rowTemplate == null)
                return;
            var list = ffRender.TemplateDesignerHost.GetSelectedParameterHosts();
            if (list.Count > 1)
                return;
            //ID ����������, � ������� ����� ���������� �������������
            long? containerID = null;
            if (list.Count == 1)
            {
                if (list[0].Tag is FlexibleFormsDS.SectionTemplateRow)
                {
                    var sectionTemplateRow =
                        (FlexibleFormsDS.SectionTemplateRow)list[0].Tag;
                    if (sectionTemplateRow.blnGrid)
                        return;
                    containerID = sectionTemplateRow.idfsSection;
                }
                else return;
            }
            MainDbService.FitParameterHostInContainer(rowTemplate.idfsFormTemplate, containerID);
            //����������� ������
            OnTreeTemplatesClick(this, EventArgs.Empty);
        }

        /// <summary>
        /// ������������ ���� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnFitTemplateItemClick(object sender, ItemClickEventArgs e)
        {
            var rowTemplate = GetActiveTemplate();
            if (rowTemplate == null)
                return;
            MainDbService.FitParameterHostInContainer(rowTemplate.idfsFormTemplate);
            //����������� ������
            OnTreeTemplatesClick(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersKeyUp(object sender, KeyEventArgs e)
        {
            treeParameters.ProcessKeyUpForTree(e);
            OnTreeParametersClick(sender, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeTemplatesKeyUp(object sender, KeyEventArgs e)
        {
            treeTemplates.ProcessKeyUpForTree(e);
            OnTreeTemplatesClick(sender, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersLibraryKeyUp(object sender, KeyEventArgs e)
        {
            treeParametersLibrary.ProcessKeyUpForTree(e);
        }

        private void FlexibleFormEditor_Load(object sender, EventArgs e)
        {

        }


        private bool IsMinimized()
        {
            var frm = FindForm();

            return frm != null && frm.WindowState == FormWindowState.Minimized;
        }
        private int m_StoredSplitPosition = 0;
        private void nbccTemplates_SizeChanged(object sender, EventArgs e)
        {
            if (Localizer.IsRtl && nbccTemplates.Parent is NavBarGroupControlContainerWrapper)
            {
                nbccTemplates.Parent.Left = 0;
                nbccTemplates.Parent.Width = nbccTemplates.Width;
                nbccTemplates.Left = 0;
            }
            if (nbcTemplatesEditor.OptionsNavPane.NavPaneState == NavPaneState.Expanded)
            {
                if (IsMinimized())
                {
                    m_StoredSplitPosition = nbcTemplatesEditor.Width;
                }
                else
                {
                    if (splitterControl1.MinSize < nbcTemplatesEditor.Width)
                    {
                        splitterControl1.MinSize = 255;
                        splitterControl1.SplitPosition = m_StoredSplitPosition;
                    }

                }
            }

        }

        private void nbcTemplatesEditor_NavPaneStateChanged(object sender, EventArgs e)
        {
            splitterControl4.Visible = nbcTemplatesEditor.OptionsNavPane.NavPaneState == NavPaneState.Expanded;

        }

        private void nbcTemplatesEditor_NavPaneMinimizedGroupFormShowing(object sender, NavPaneMinimizedGroupFormShowingEventArgs e)
        {
            if (Localizer.IsRtl)
                e.NavPaneForm.Left = e.NavPaneForm.Left - e.NavBar.Width - e.NavPaneForm.Width;
        }
    }
}
