using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using bv.common;
using bv.common.Core;
using bv.common.Enums;
using bv.common.db;
using bv.common.win;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.BandedGrid;
using EIDSS.FlexibleForms.Components;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.Helpers;
using bv.model.Model.Core;
using bv.winclient.BasePanel;
using bv.winclient.Core;
using DevExpress.XtraGrid.Views.Base;
using bv.winclient.Layout;
using eidss.model.Core;
using System.Linq;

[assembly: CLSCompliantAttribute(true)]

namespace EIDSS.FlexibleForms
{
    [Description("GUI for displaing Flexible Forms and user input")]
    [CLSCompliantAttribute(true)]
    public partial class FFPresenter : BaseDetailPanel
    {
        /// <summary>
        /// �����������
        /// </summary>
        public FFPresenter()
        {
            InitializeComponent();
            if (WinUtils.IsComponentInDesignMode(this)) return;
            DbService = new DbServiceUserData();
            baseDataSet = MainDbService.MainDataSet;
            ffRender = new FFRender((DbService)DbService, DesignerHostMain);
            ffRender.EditValueChanging += OnFlexibleFormEditorEditValueChanging;
            ffRender.EditValueChanged += OnFlexibleFormEditorEditValueChanged;
            ffRender.ParameterIsMandatory += SetControlMandatoryState;
            activityParametersBuffer = new ActivityParametersBuffer();
            ReadOnlyMode = false;
            DelayedLoadingNeeded = false;
            DynamicParameterEnabled = false;
            observationsDictionary = new Dictionary<long, long>();
            observationsGroupEditing = new List<long>();
        }

        public override bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = value;
                if (!ReadOnly)
                {
                    var list = ffRender.ParameterList.Where(p => p.Value.IsMandatoryField()).ToList();
                    foreach (var keyValuePair in list)
                    {
                        LayoutCorrector.SetStyleController(keyValuePair.Value.ControlParameter, LayoutCorrector.MandatoryStyleController);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void BeforeDispose()
        {
            if (ffRender != null)
            {
                ffRender.ClearTemplate();
            }

            base.BeforeDispose();
        }

        private bool m_DynamicParameterEnabled;

        /// <summary>
        /// �������� �� ����������� ��������� ������������ ���������
        /// </summary>
        [Browsable(true)]
        public bool DynamicParameterEnabled
        {
            get { return m_DynamicParameterEnabled; }
            set
            {
                m_DynamicParameterEnabled = value;
                RefreshVisibleDynamicParameters();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshVisibleDynamicParameters()
        {
            barDockControlTop.Visible = barDynamicParameters.Visible = DynamicParameterEnabled;
        }

        /// <summary>
        /// ���������� �������� ������������ �������� � ����� �� ������� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFlexibleFormEditorEditValueChanged(object sender, EventArgs e)
        {
            if (nowLoading || (!TemplateID.HasValue) || Closing) return;
            //�������� ���� �������������� ��������
            OnParameterEditValueChanged(sender, e);
            //
            var baseEdit = sender as BaseEdit;
            if (baseEdit == null) return;
            var parameter = baseEdit.Tag as Parameter;
            if (parameter == null) return;

            var needCheckRules = (baseEdit.IsModified || (baseEdit.OldEditValue != baseEdit.EditValue));
            if (needCheckRules)
            {
                //���� �������� ��������, �� ����� ������ ������ ������������ ��-�� ������������ �����
                if ((parameter.IdfsParameterType != null) && (parameter.IdfsParameter != null))
                {
                    if (MainDbService.IsParameterNumeric(parameter.IdfsParameter.Value))
                    {
                        var intvalue = 0;
                        if (!Utils.IsEmpty(baseEdit.EditValue)) Int32.TryParse(baseEdit.EditValue.ToString(), out intvalue);
                        needCheckRules = !((baseEdit.OldEditValue == null) && (intvalue == 0));
                    }
                }
            }

            if (needCheckRules)
            {
                //����� ������� (��� �������), � ������� ��������� ����� ��������, � �������� ��
                RulesCheck(parameter.IdfsParameter, FFRuleCheckPointType.OnValueChanged);
            }
        }

        /// <summary>
        /// ��������� ���������� ���������� �������
        /// </summary>
        /// <param name="rowRule"></param>
        private bool RuleExecute(FlexibleFormsDS.RulesRow rowRule)
        {
            if (!ObservationID.HasValue) return false;

            //TODO ����������� � ���������� ����������

            //�������� ���������-��������� ����� �������
            var parameterForFunctionRows =
                    (FlexibleFormsDS.RuleParameterForFunctionRow[])
                    MainDbService.MainDataSet.RuleParameterForFunction.Select(DataHelper.Filter("idfsRule", rowRule.idfsRule));
            if (parameterForFunctionRows.Length == 0) return false;
            //�������� ������ ��������
            var parameter1 = parameterForFunctionRows[0];
            var param1Value = MainDbService.GetActivityParametersRow(ObservationID.Value, parameter1.idfsParameter, null);
            //����������� �������� ��������, ����� ��������� ������� ������� Is Empty, �� ��� �� ���������� �������� ������
            //������ ��� ���������� �������� ��������� ������, �� �� ����� � ��������� � �������
            if (Utils.IsEmpty(param1Value))
            {
                param1Value = MainDbService.MainDataSet.ActivityParameters.NewActivityParametersRow();
                param1Value.varValue = null;
            }
            //���� ������ ������� ������������� ��� ���������, �� ������� � ������
            FlexibleFormsDS.RuleParameterForFunctionRow parameter2 = null;
            FlexibleFormsDS.ActivityParametersRow param2Value = null;
            if (rowRule.intNumberOfParameters == 2)
            {
                parameter2 = parameterForFunctionRows[1];
                param2Value = MainDbService.GetActivityParametersRow(ObservationID.Value, parameter2.idfsParameter, null);
            }
            //���� ���� �� ���� �� ���������� �� �����, �� ������� �� ����������� (�� ���� ������������)
            //������� � ������ ��������� ����������� ������ ��� Is Empty & Is Constant
            if (Utils.IsEmpty(param1Value.varValue) 
                && (!rowRule.idfsRuleFunction.Equals((long)FFRuleFunctions.Empty))
                && (!rowRule.idfsRuleFunction.Equals((long)FFRuleFunctions.Value))
                ) return false;
            if (parameter2 != null)
            {
                if (Utils.IsEmpty(param2Value)) return false;
                if (Utils.IsEmpty(param2Value.varValue)) return false;
            }

            //�������� ���������-���� ��� ����� �������
            var parameterForActionRows = MainDbService.GetRuleParameterForAction(rowRule.idfsRule);
            //���� ��� �� ����� ����, �� ������� �� �����������
            if (parameterForActionRows.Length == 0) return false;

            //����� ����, ��� ������� ��� ������������ ������� �����������
            var ruleExecuted = false; //���������� �� ���������
            //��������� �������� � ����������� �� ���� �������
            switch (rowRule.idfsRuleFunction)
            {
                case (long)FFRuleFunctions.CurrentDate:
                    if (param1Value.varValue != null)
                    {
                        var dt = ((DateTime)param1Value.varValue).Date;
                        if (dt <= DateTime.Now.Date) ruleExecuted = true;
                    }
                    break;
                case (long)FFRuleFunctions.Empty:
                    if (Utils.IsEmpty(param1Value.varValue)) ruleExecuted = true;
                    break;
                case (long)FFRuleFunctions.Value:
                    //�������� �������� ��������
                    var ruleConstantRows = (FlexibleFormsDS.RuleConstantRow[])MainDbService.MainDataSet.RuleConstant.Select(DataHelper.Filter("idfsRule", rowRule.idfsRule));
                    //���� ���� ���������, � �������� ���� ����������, �� ������� � ������ �� ���
                    foreach (var row in ruleConstantRows)
                    {
                        if (Utils.IsEmpty(row.varConstant) || row.varConstant.ToString() == "-1")
                        {
                            if (Utils.IsEmpty(param1Value) || Utils.IsEmpty(param1Value.varValue))
                            {
                                ruleExecuted = true;
                                break;
                            }
                        }
                        else
                        {
                            //������� ��� ������
                            if (row.varConstant.ToString().Equals(param1Value.varValue.ToString()))
                            {
                                ruleExecuted = true;
                                break;
                            }
                        }
                    }
                    break;
                case (long)FFRuleFunctions.Greater:
                    double d1;
                    double d2;
                    DateTime dt1;
                    DateTime dt2;
                    if ((param2Value != null) && (param1Value.varValue != null))
                    {
                        var p1 = param1Value.varValue.ToString();
                        var p2 = param2Value.varValue.ToString();

                        if (Double.TryParse(p1, out d1) &&
                            Double.TryParse(p2, out d2))
                        {
                            if (d1 > d2) ruleExecuted = true;
                        }
                        else if (DateTime.TryParse(p1, out dt1) &&
                            DateTime.TryParse(p2, out dt2))
                        {
                            if (dt1 > dt2) ruleExecuted = true;
                        }
                    }

                    break;
                case (long)FFRuleFunctions.GreaterEqual:
                    if ((param2Value != null) && (param1Value.varValue != null))
                    {
                        var p1 = param1Value.varValue.ToString();
                        var p2 = param2Value.varValue.ToString();

                        if (Double.TryParse(p1, out d1) &&
                            Double.TryParse(p2, out d2))
                        {
                            if (d1 >= d2) ruleExecuted = true;
                        }
                        else if (DateTime.TryParse(p1, out dt1) &&
                            DateTime.TryParse(p2, out dt2))
                        {
                            if (dt1 >= dt2) ruleExecuted = true;
                        }
                    }

                    break;

            }
            //����������� ������� ���������, ���� ��� ����
            if (rowRule.blnNot) ruleExecuted = !ruleExecuted;
            var result = ruleExecuted;
            //���� ������� �� �����������
            if (!ruleExecuted)
            {
                foreach (var rowAction in parameterForActionRows)
                {
                    switch (rowAction.idfsRuleAction)
                    {
                        case (long)FFRuleActions.Disabled:
                            RuleActionDisabled(rowAction.idfsParameter, false); //������������ ��������!
                            break;
                    }
                }
            }
            else
            {
                //���� ��� ������� "������� � ������� ��� ����������", �� ������� ��������� � ������, ��� ������� �� ����������� 
                //���� ��� ������� "������� � ������������ ��� ����������", �� ������� ������ ������������. ���� ��� ������ "���", �� ������� �� �����������, ���� "��", �� �����������
                //� ��������� ������� ������ ���������� ���������
                if (rowRule.idfsCheckPoint.Equals(((long)FFRuleCheckPointType.OnSaveWithError).ToString()) || (rowRule.idfsCheckPoint.Equals(((long)FFRuleCheckPointType.OnSaveWithNotify).ToString())))
                {
                    var message = (!rowRule.IsMessageNationalTextNull()) ? rowRule.MessageNationalText.Length > 0 ? rowRule.MessageNationalText : "<No Message>" : "<No Message>";

                    if (rowRule.idfsCheckPoint.Equals(((long)FFRuleCheckPointType.OnSaveWithError).ToString()))
                    {
                        ErrorForm.ShowWarningDirect(message);
                    }
                    else if (rowRule.idfsCheckPoint.Equals(((long)FFRuleCheckPointType.OnSaveWithNotify).ToString()))
                    {
                        //������������ ��� ����� ���������� �������� � ���������� ��� ���
                        result = !WinUtils.ConfirmMessage(message, String.Empty);
                    }
                    //���� ������� ����������� ��� ���������� (�� ���� �������� �� ������), �� ����������� �� ����� �� ����������, ����������� � �������
                    if (result)
                    {
                        //��� ������� ���������� (������� �� � �������)
                        var parameter = GetParameter(parameter1.idfsParameter);
                        if (parameter != null)
                        {
                            FocusOnField(parameter);
                        }
                    }
                }
                else
                {
                    if (!rowRule.IsMessageNationalTextNull() && (rowRule.MessageNationalText.Length > 0)) ErrorForm.ShowMessageDirect(rowRule.MessageNationalText);
                }
                //������ �� ���� ����������, ��������� � ������ �����, � �������� �������� ��� ������� �� ���
                foreach (FlexibleFormsDS.RuleParameterForActionRow rowAction in parameterForActionRows)
                {
                    switch (rowAction.idfsRuleAction)
                    {
                        case (long)FFRuleActions.Clear:
                            RuleActionClear(rowAction.idfsParameter);
                            break;
                        case (long)FFRuleActions.Disabled:
                            RuleActionDisabled(rowAction.idfsParameter, true);
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// �������� ��� ������ (Disabled/ Enabled)
        /// </summary>
        /// <param name="idfsParameter"></param>
        /// <param name="disabled">true, ���� ����� ������������� �������, false, ���� ��������������</param>
        private void RuleActionDisabled(long idfsParameter, bool disabled)
        {
            //���� ���� �����������, �� �������������� ��� ���� ��������
            if (disabled) RuleActionClear(idfsParameter);
            //������� ������� ��� ����� ���������
            //���� ��������� ��� � ����������, �� �������
            //if (!flexibleFormEditor.ParameterList.ContainsKey(idfsParameter)) return;
            //Parameter parameter = flexibleFormEditor.ParameterList[idfsParameter];
            if (!ffRender.ParameterList.ContainsKey(idfsParameter)) return;
            var parameter = ffRender.ParameterList[idfsParameter];
            parameter.ControlParameter.Enabled = !disabled;
        }

        /// <summary>
        /// �������� ��� ������ (Clear)
        /// </summary>
        /// <param name="idfsParameter"></param>
        private void RuleActionClear(long idfsParameter)
        {
            //������� ������� ��� ����� ���������
            //���� ��������� ��� � ����������, �� �������
            //Parameter parameter = flexibleFormEditor.GetParameter(idfsParameter);
            Parameter parameter = ffRender.GetParameter(idfsParameter);
            if (parameter == null) return;
            parameter.ControlParameter.EditValue = null;
        }

        /// <summary>
        /// ��������� ��������� �������� � ����� �� ��������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFlexibleFormEditorEditValueChanging(object sender, ChangingEventArgs e)
        {
            if ((EditValueChanging != null) && (!nowLoading) && (!Closing)) EditValueChanging(sender, e);
        }

        public delegate void EditValueChangingDelegate(object sender, ChangingEventArgs e);

        /// <summary>
        /// ������� �����������, ����� �������� �������� � ������ � ����� �� ��������� ������
        /// </summary>
        public event EditValueChangingDelegate EditValueChanging;

        private FFRender ffRender { get; set; }

        /// <summary>
        /// ���������� Flexible Form c �������
        /// </summary>
        /// <param name="idfsFormTemplate">ID �������</param>
        /// <param name="idfObservation">ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        /// <param name="bufferingUserData"></param>
        public void ShowFlexibleForm(long idfsFormTemplate, long idfObservation, FFType aformType, bool bufferingUserData)
        {
            ShowFlexibleForm(idfsFormTemplate, idfObservation, aformType, bufferingUserData, false);
        }

        /// <summary>
        /// ���������� Flexible Form c �������
        /// </summary>
        /// <param name="idfsFormTemplate">ID �������</param>
        /// <param name="idfObservation">ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        /// <param name="bufferingUserData">false -- ������ ��� ��������� ���������������� ������ �� �� (�� ��������� ����������� ��������)</param>
        /// <param name="runImmediatly">true -- �� ������������ ���������� �������� (�� ��������� ������������)</param>
        public void ShowFlexibleForm(long idfsFormTemplate, long idfObservation, FFType aformType, bool bufferingUserData, bool runImmediatly)
        {
            var observations = new List<long> { idfObservation };
            ShowFlexibleForm(idfsFormTemplate, observations, aformType/*, null*/, null, runImmediatly, bufferingUserData, false, false);
        }

        /// <summary>
        /// ���������� Flexible Form c �������
        /// </summary>
        /// <param name="idfsFormTemplate">ID �������</param>
        /// <param name="idfObservation">ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        public void ShowFlexibleForm(long idfsFormTemplate, long idfObservation, FFType aformType)
        {
            ShowFlexibleForm(idfsFormTemplate, idfObservation, aformType, false);
        }

        /// <summary>
        /// ��������� �� ��������� � ������ ������ ��� ������
        /// </summary>
        public bool ReadOnlyMode { get; private set; }

        //��������� �������� ���������� ��� ���������� �������� ��������
        private long m_IdfsFormTemplateStored;
        private List<long> m_ObservationsStored;
        private FFType m_FormTypeStored;
        private List<long> m_PresetStubsStored;
        private long? m_IdfVersionStored;

        /// <summary>
        /// ������� ���������� ��������
        /// </summary>
        public bool DelayedLoadingNeeded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private long? idfsFormTemplatePrevious { get; set; }

        /// <summary>
        /// ���������� Flexible Form c �������
        /// </summary>
        /// <param name="idfsFormTemplate">ID �������</param>
        /// <param name="observations">��������� ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        /// <param name="idfVersion">������ �������� (���� �� �������, �� ������������ ��, ��� ����������� ��� ������� observation)</param>
        /// <param name="runImmediatly">��������� ����� �������, �� ������� �������� �� �����������</param>
        /// <param name="bufferingUserData">True (�� ���������) - ��������������� ���������������� ������, False - ������ ��� ���������� � �� �� ����������������� �������</param>
        /// <param name="isSummary"></param>
        /// <param name="isGroupEditing">�������� �� ����� � ������ ���������� ��������������</param>
        private void ShowFlexibleForm(long idfsFormTemplate, List<long> observations, FFType aformType, long? idfVersion, bool runImmediatly, bool bufferingUserData, bool isSummary, bool isGroupEditing)
        {
            if (observations.Count == 0) return;

            nowLoading = true;
            try
            {
                IsGroupEditingRegime = isGroupEditing;
                TemplateID = MainDbService.TemplateID = idfsFormTemplate;
                if ((observations.Count == 1) && !isGroupEditing)
                {
                    ObservationID = observations[0];
                }
                else if (isGroupEditing)
                {
                    ObservationID = MainDbService.IdfObservationGroup;
                }
                else
                {
                    //������������ � ������� ���.����� � �������
                    ObservationID = MainDbService.IdfObservationSummary;
                }

                FormTypeRow = MainDbService.GetFormType((long)aformType);
                if (FormTypeRow == null) return;
                VersionMatrixStub = idfVersion;
                ffRender.ReadOnly = ReadOnly;
                btnAddParameter.Enabled = !ReadOnly;
                btnRemoveAllDynamicParameter.Enabled = !ReadOnly;
                DynamicParameterEnabled = DynamicParameterEnabled && !ReadOnly;

                //���������� �� ������ ������� ������ �����, ����� �������� � ��. ��������
                List<long> presetStubs = null;
                long? idfsSection;
                long? idfsMatrixType;
                MainDbService.GetSectionForAggregateCase(idfsFormTemplate, (long)aformType, out idfsSection, out idfsMatrixType);
                if (idfsSection.HasValue)
                {
                    presetStubs = new List<long>(1) { idfsSection.Value };
                    //����� ��� �������������
                    if (!CriticalObjects.Sections.Keys.Contains(idfsSection.Value))
                        CriticalObjects.Sections.Add(idfsSection.Value,
                                                              new CriticalObject(idfsSection.Value, true));
                }

                //��������� ���������� ��� ���������� ��������
                DelayedLoadingNeeded = (m_IdfsFormTemplateStored != idfsFormTemplate
                                        || m_ObservationsStored != observations
                                        || m_FormTypeStored != aformType
                                        || m_PresetStubsStored != presetStubs
                                        || m_IdfVersionStored != idfVersion);

                m_IdfsFormTemplateStored = idfsFormTemplate;
                m_ObservationsStored = observations;
                m_FormTypeStored = aformType;
                m_PresetStubsStored = presetStubs;
                m_IdfVersionStored = idfVersion;

                //���� ������ �������� ������������ ����, �� ����� ����������� ��� �������� ��� ������������
                if (TemplateHasMandatoryFields(TemplateID.Value)) runImmediatly = true;

                //���� � ������ ������ ��������� �� �����, �� �� ����� ����������� ��������
                if (!runImmediatly && !Visible) return;

                //���� ����������� ������ � ���������, �� ��������� ��������� ������ ��������
                if (presetStubs != null)
                {
                    //���� ���� �� ������ ������ ��������, �� ����� ����� ��������
                    if (VersionMatrixStub == null)
                    {
                        var row = MainDbService.LoadMatrixStubInfo(ObservationID.Value);
                        if ((row != null) && (!row.IsidfVersionNull()))
                            VersionMatrixStub = row.idfVersion;
                    }
                    //���� ������ �� ������� ����������, �� ������ ������ ��������� ������
                    if (VersionMatrixStub == null) return;
                }

                //��������������� ������
                ffRender.RestoreTemplate(idfsFormTemplate
                                        , FormTypeRow.idfsFormType
                                        , ModelUserContext.CurrentLanguage
                                        , presetStubs
                                        , VersionMatrixStub.HasValue ? VersionMatrixStub.Value : VersionMatrixStub
                                        , idfsMatrixType
                                        , isSummary);

                //������� �� ��� ��������� ������� ���������, ����� ����� ���������� ������ � �������
                SetEventForParameters();
                //�������� ���������������� ������
                foreach (var observation in observations)
                {
                    MainDbService.LoadUserData(observation, idfsFormTemplate, bufferingUserData);
                }

                #region Ressurection ��� ����������

                //���� ���������� ������, ��� ������� � ������� ������ ��� ����������, �� ���� ��� ��������� ������� � ���������� �� � ������ ����� �� ���������
                //��������� ������ � ������� ������, ���������� ��� ��� ���������, ������ �� ��������� ��������
                //��������� ����� ��������� ������ �������, �� ������ �� ��� �� �������
                var parametersTable = MainDbService.LoadParameterDeletedFromTemplate(
                    ObservationID.Value
                    , idfsFormTemplate
                    , idfsFormTemplatePrevious);

                if (parametersTable.Count > 0)
                {
                    //������� ������, ��� ����� ���������� ������������ ���������
                    ffRender.ShowDynamicParametersGroupControl(idfsFormTemplate);
                }
                else
                {
                    ffRender.SetVisibleDynamicParametersGroupControl(false);
                }

                idfsFormTemplatePrevious = idfsFormTemplate;

                //������� ��������� � ��������� �� � ������
                foreach (var row in parametersTable)
                {
                    //������� ��������-�������
                    var parametersRow = MainDbService.GetParameterRow(row.idfsParameter);
                    if (parametersRow == null) continue;
                    //������ ���� �������� �� ����������� ������
                    ffRender.CreateParameterInParameterDynamicParametersGroupControl(parametersRow, TemplateID.Value);
                }

                #endregion

                ReadOnlyMode = false;

                List<FlexibleFormsDS.PredefinedStubRow> predefinedStubRows = null;
                //���� ���� ����������������� �������, �� �������� ���������������� ������ ��� �������
                if (presetStubs != null) MainDbService.IncludeStubDataInUserData(TemplateID, ObservationID, VersionMatrixStub, null, out predefinedStubRows);

                //��������� �������� �� ����������
                if (!isGroupEditing)
                {
                    SetValuesToParameters(predefinedStubRows);
                }
                else
                {
                    SetValuesToParametersGroupEditing();
                }

                //�������� ���� ���������������� ������, ��� ��� �� ��������, ���� ������������� ������ ����������
                MainDbService.MainDataSet.ActivityParameters.AcceptChanges();

                //�������� �������� ������, ������� ������ ����������� ��� �������� �������
                //������� � �� �������������� ��� ��������� ���� � RestoreTemplate
                //(��������� ��� ���� ����������)
                RulesCheck(null, FFRuleCheckPointType.OnLoad);

                //�������� ����������� ������� ���������� TabIndex'�
                HelperFunctions.SetTabIndex(ffRender.ParameterList, ffRender.SectionList);
                DelayedLoadingNeeded = false;
                nowLoading = false;
                m_BindingDefined = true;
            }
            catch (SqlException e)
            {
                if (!SqlExceptionHandler.Handle(e))
                {
                    ErrorForm.ShowError(StandardError.FillDatasetError, e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool LoadData(ref object id)
        {
            if (observationsDictionary != null) observationsDictionary.Clear();
            return true;
        }

        /// <summary>
        /// ���������� Flexible Form c �������
        /// </summary>
        /// <param name="idfsFormTemplate">ID �������</param>
        /// <param name="observations">��������� ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        /// <param name="idfVersion">������ �������� (���� �� �������, �� ������������ ��, ��� ����������� ��� ������� observation)</param>
        /// <param name="runImmediatly">��������� ����� �������, �� ������� �������� �� �����������</param>
        /// <param name="isSummary"></param>
        public void ShowFlexibleForm(long idfsFormTemplate, List<long> observations, FFType aformType, long? idfVersion, bool runImmediatly, bool isSummary)
        {
            ShowFlexibleForm(idfsFormTemplate, observations, aformType, idfVersion, runImmediatly, true, isSummary, false);
        }

        /// <summary>
        /// ��������� �������� ������ ���������� ����
        /// </summary>
        /// <param name="idfsParameter">���� ����� ��������� ������� ��� ����������� ���������</param>
        /// <param name="ffRuleCheckPointType"></param>
        private bool RulesCheck(long? idfsParameter, FFRuleCheckPointType ffRuleCheckPointType)
        {
            var result = true;
            if (!TemplateID.HasValue) return true;
            foreach (var row in MainDbService.MainDataSet.Rules)
            {
                if (!row.idfsFormTemplate.Equals(TemplateID.Value) || (!Convert.ToInt64(row.idfsCheckPoint).Equals((long)ffRuleCheckPointType))) continue;
                //���� ����� ��������� ������ �� �������, ��� ��������� ��������� ��������
                if (idfsParameter != null)
                {
                    var parameterForFunctionRows =
                        (FlexibleFormsDS.RuleParameterForFunctionRow[])
                        MainDbService.MainDataSet.RuleParameterForFunction.Select(DataHelper.Filter("idfsRule", row.idfsRule, "idfsParameter", idfsParameter));
                    if (parameterForFunctionRows.Length == 0) continue;
                }
                //���� ������� ��������� ������� ��������� � �������, �� ��������� ���������� �������
                var res = RuleExecute(row);
                //���� ��� ���������� �����-�� ������� ����������� (���������), �� ��� ����� ���������� ����������
                //��� ���������� ����������� ������� ������ ������� � �������� ������
                if (res && (ffRuleCheckPointType.Equals(FFRuleCheckPointType.OnSaveWithError) || (ffRuleCheckPointType.Equals(FFRuleCheckPointType.OnSaveWithNotify))))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// �������� �� ������������ ������ UNI-��������
        /// </summary>
        public bool? IsUNITemplate { get; private set; }

        ///// <summary>
        ///// ������� ���������������� ������, ������� �������� ��������
        ///// </summary>
        //private void DeleteStubData()
        //{
        //    //����� ������������ ��� ��������������� ������� ������� ����� ���������� ���� ������ ��������
        //    //��� ����������, ����� ��� ����� � ��� �� observation �������� ������ �������� � ����� ������ ������
        //    FlexibleFormsDS.ActivityParametersRow[] userData = (FlexibleFormsDS.ActivityParametersRow[])MainDbService.MainDataSet.ActivityParameters.Select(DataHelper.Filter("idfObservation", ObservationID));
        //    foreach (FlexibleFormsDS.ActivityParametersRow row in userData)
        //    {
        //        //���� ������ ���������������� ������ ��������� � ��������� � ������������� ���������� �������, �� ��� ������ ��������
        //        if (!flexibleFormEditor.ParameterList.ContainsKey(row.idfsParameter)) continue;
        //        if (flexibleFormEditor.ParameterList[row.idfsParameter].ParameterTemplateRow.intOrder < 0)
        //        {
        //            row.Delete();
        //        }
        //    }
        //}

        /// <summary>
        /// �������� �������� � ����������� �������� ����������
        /// </summary>
        /// <param name="predefinedStubRows">���������� ��������, ���� ������� ����, ��� null, ���� ��� ���</param>
        private void SetValuesToParameters(List<FlexibleFormsDS.PredefinedStubRow> predefinedStubRows)
        {
            if (MainDbService.MainDataSet.ActivityParameters.Rows.Count == 0) return;
            var userData = (FlexibleFormsDS.ActivityParametersRow[])MainDbService.MainDataSet.ActivityParameters.Select(DataHelper.Filter("idfObservation", ObservationID));
            foreach (var row in userData)
            {
                if (!row.IsRowAlive()) continue;
                //���� �� ������� ������ � ������� ������, �� ������ �����-�� �������� ��� ��������� � ��������, � ���������� ��� ������
                if (row.IsintNumRowNull()) continue;
                //���� ������ �������� �� ��������, �� ����� ����������
                if (!row.IsintRowStateNull() && (row.intRowState == 1)) continue;
                //���� ������� �����, �� �� ������� �� ������, ������� ��� � ��������
                if (predefinedStubRows != null)
                {
                    var idRow = row.idfRow;
                    if (predefinedStubRows.Count(r => r.idfRow == idRow) == 0) continue;
                }
                //������� ����� ����� ��������
                if (!ffRender.ParameterList.ContainsKey(row.idfsParameter)) continue;
                var parameter = ffRender.ParameterList[row.idfsParameter];
                //�������� ���� ��������
                //���� �������� ��������� � ������� ������ ��� ������ ���������� � ���� �������, �� ������ �������� ��� ��������
                SetValueToEditor(parameter, row);
            }
        }

        /// <summary>
        /// ���������� �������� � ��������� � ������ ���������� ��������������
        /// </summary>
        private void SetValuesToParametersGroupEditing()
        {
            foreach (var pl in ffRender.ParameterList)
            {
                var idParameter = pl.Key;
                var parameter = pl.Value;

                var userData = MainDbService.MainDataSet.ActivityParameters.Where(ap =>
                    (ap.idfObservation != MainDbService.IdfObservationGroup)
                    &&
                    (ap.idfsParameter == idParameter)
                    ).ToList();
                var enabled = true;
                //��� �������������� �������� �� ���� ����������� ������ ���� �����������
                //���������� �������� �������� ��������� ���� (��������� ����������� ����������)
                if (userData.Count > 0) enabled = userData.All(ap => ap.varValue.Equals(userData[0].varValue));
                if (enabled)
                {
                    if (userData.Count > 0)
                    {
                        SetValueToEditor(parameter, userData[0]);
                    }
                }
                if (!ReadOnly) parameter.ReadOnly = !enabled;
            }
        }

        /// <summary>
        /// ����� ��� ����������� ���������������� ������
        /// </summary>
        private ActivityParametersBuffer activityParametersBuffer { get; set; }

        /// <summary>
        /// ���������� �������� �������� ���������
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="rowData"></param>
        private void SetValueToEditor(Parameter parameter, FlexibleFormsDS.ActivityParametersRow rowData)
        {
            if (!parameter.IdfsParameter.HasValue) return;

            //�������� ���� ��������
            //���� �������� ��������, �� �������� ��� �������� � ������������ ��������
            var value = rowData.varValue;
            //���� �������� ��������, �� ����� �������� ��� � ������������ ��������
            if (MainDbService.IsParameterNumeric(parameter.IdfsParameter.Value))
            {
                decimal decValue;
                if (Decimal.TryParse(value.ToString(), out decValue)) value = decValue;
            }

            //���� �������� ��������� � ������� ������ ��� ������ ���������� � ���� �������, �� ������ �������� ��� ��������
            if (parameter.RootSection == null)
            {
                parameter.SetValue(value);
                //parameter.ControlParameter.EditValue = value;
            }
            else
            {
                parameter.RootSection.SetTableValue(parameter, rowData.intNumRow, value);
            }
        }

        /// <summary>
        /// ������� ������� ��� ����������
        /// </summary>
        private void SetEventForParameters()
        {
            //TODO ��������� � ��������� ������, ����� ������������ � ������ � ���������
            foreach (var parameter in ffRender.ParameterList.Values)
            {
                parameter.ControlParameter.EditValueChanged += OnParameterEditValueChanged;
            }

            foreach (var sectionTable in ffRender.SectionTableList.Values)
            {
                //����� �� ���� �������������� ������, ���� ���� ��������� ��������� ��������� ������
                //sectionTable.RootSection.GridViewMain.CellValueChanged -= OnSectionTableCellValueChanged;
                //sectionTable.RootSection.GridViewMain.CellValueChanged += OnSectionTableCellValueChanged;
                sectionTable.RootSection.CellValueChanged -= OnSectionTableCellValueChanged;
                sectionTable.RootSection.CellValueChanged += OnSectionTableCellValueChanged;
                sectionTable.RootSection.RemoveRow -= OnSectionTemplateRemoveRow;
                sectionTable.RootSection.RemoveRow += OnSectionTemplateRemoveRow;
            }
        }

        /// <summary>
        /// �������� ������ � ��������� ������
        /// </summary>
        /// <param name="idfsSectionTemplate"></param>
        /// <param name="numRow"></param>
        void OnSectionTemplateRemoveRow(long idfsSectionTemplate, int numRow)
        {
            if (nowLoading || Closing || !ObservationID.HasValue || !TemplateID.HasValue) return;

            var rootSectionTable = ffRender.SectionTableList[idfsSectionTemplate];
            //�� ��������� ��� ������ � �������������� ����������
            if (rootSectionTable.IsFixedStubSection) return;

            var idfRow = GetIdfRowTableParameter(ObservationID.Value, numRow, rootSectionTable);
            var pairkey = new Dictionary<string, long>
                              {
                                  {"idfObservation", ObservationID.Value},
                                  {"idfRow", idfRow}
                              };
            var rows = DataHelper.FindDataRows(pairkey, MainDbService.MainDataSet.ActivityParameters);
            foreach (FlexibleFormsDS.ActivityParametersRow dataRow in rows)
            {
                dataRow.intNumRow = dataRow.intNumRow == 0 ? -1000 : -dataRow.intNumRow;
                dataRow.varValue = DBNull.Value;
            }

            //������������� ������ ����� � ���� ����������� ����� ���������
            for (var i = 0; i < MainDbService.MainDataSet.ActivityParameters.Count; i++)
            {
                var row = MainDbService.MainDataSet.ActivityParameters[i];
                if (!row.IsRowAlive()) continue;
                if (row.intNumRow > numRow) row.intNumRow--;
            }
        }

        /// <summary>
        /// ���������� ��������� ������, ���� ������� ������� � �������
        /// </summary>
        /// <param name="idfsSection">ID ��������� ������</param>
        /// <returns>���������� ��������� ������, ���� ������� ������� � �������, ��� null, ���� � ���</returns>
        public SectionTable GetSectionTable(long idfsSection)
        {
            return ffRender.GetSectionTable(idfsSection);
        }

        /// <summary>
        /// ���������� ��������, ���� ������� ������� � �������
        /// </summary>
        /// <param name="idfsParameter">ID ���������</param>
        /// <returns>���������� ��������, ���� ������� ������� � �������, ��� null, ���� ��� ���</returns>
        public Parameter GetParameter(long idfsParameter)
        {
            return ffRender.GetParameter(idfsParameter);
        }

        /// <summary>
        /// ���������� ��������� ������, ���� ������� ������� � �������
        /// </summary>
        /// <param name="index">���������� ����� ��������� ������</param>
        /// <returns>���������� ��������� ������, ���� ������� ������� � �������, ��� null, ���� � ���</returns>
        public SectionTable GetSectionTableByIndex(int index)
        {
            return ffRender.GetSectionTableByIndex(index);
        }

        /// <summary>
        /// ����� �������� �������� � ������ ������� ��������� ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSectionTableCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (nowLoading || Closing || !ObservationID.HasValue || !TemplateID.HasValue) return;

            var column = e.Column as BandedGridColumn;
            if (column == null) return;
            var parameterTemplateRow = ((GridBand)column.Tag).Tag as FlexibleFormsDS.ParameterTemplateRow;
            if (parameterTemplateRow == null) return;
            var rootSectionTable = (SectionTable)((AdvBandedGridView)sender).Tag;
            //��-�� �������� ������ ���������� �����
            var d = e.RowHandle == -2147483647;
            var numRow = rootSectionTable.GridViewMain.IsNewItemRow(e.RowHandle)
                             ? rootSectionTable.GridViewMain.RowCount - (d ? 0 : 1)
                             : e.RowHandle;

            var idfRow = GetIdfRowTableParameter(ObservationID.Value, numRow, rootSectionTable);
            MainDbService.ChangeValue(ObservationID.Value, TemplateID.Value, parameterTemplateRow.idfsParameter, idfRow,
                                      numRow, e.Value, null);
        }

        /// <summary>
        /// ����������, ��� � ������ ������ ���������� �������� ���������� ��� �����-���� ��������� ��������
        /// </summary>
        private bool nowLoading { get; set; }
        public override bool Loading
        {
            get
            {
                return base.Loading || nowLoading;
            }
        }

        /// <summary>
        /// ��������� �������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnParameterEditValueChanged(object sender, EventArgs e)
        {
            if (nowLoading || Closing || !ObservationID.HasValue || !TemplateID.HasValue) return;
            var be = sender as BaseEdit;
            if (be == null) return;
            var parameter = (Parameter)be.Tag;
            if (!parameter.IdfsParameter.HasValue) return;
            var idfRow = GetIdfRowSimpleParameter(ObservationID.Value, parameter.IdfsParameter.Value);
            var varValue = be.EditValue;
            if (be.EditValue != null)
            {
                long longval;
                if (Int64.TryParse(be.EditValue.ToString(), out longval) && (longval == -1)) varValue = null;
            }
            MainDbService.ChangeValue(ObservationID.Value, TemplateID.Value, parameter.IdfsParameter.Value, idfRow, 0, varValue, null);
            if ((observationsGroupEditing.Count > 0) && IsGroupEditingRegime)
            {
                //������ ��� �������� �� ��� ����������
                foreach (var obs in observationsGroupEditing)
                {
                    var idObs = obs;
                    //������� ���������� ��������� idfRow, ������� ��� ����� ����� ��������
                    //���� ��� ��� (��� ����� ������), �� ���� ������ ��� ���������
                    var ap = MainDbService.MainDataSet.ActivityParameters.FirstOrDefault(a => (a.idfObservation == idObs) && (a.idfsParameter == parameter.IdfsParameter.Value));
                    var idr = ap != null ? ap.idfRow : idfRow;
                    MainDbService.ChangeValue(obs, TemplateID.Value, parameter.IdfsParameter.Value, idr, 0, varValue, null);
                }
            }
        }

        /// <summary>
        /// �������� ��� ������ idfRow ��� ���������, ������������ �� � �������
        /// </summary>
        /// <param name="observationid"></param>
        /// <param name="idfsParameter"></param>
        /// <returns></returns>
        private long GetIdfRowSimpleParameter(long observationid, long idfsParameter)
        {
            //������� �������� ������. ���� ������ �� �������, �� ������������ null
            var userData = (FlexibleFormsDS.ActivityParametersRow[])MainDbService.MainDataSet.ActivityParameters.Select(DataHelper.Filter("idfObservation", observationid, "idfsParameter", idfsParameter), "[idfRow] ASC");
            return userData.Length == 1 ? userData[0].idfRow : BaseDbService.NewIntID();
        }

        /// <summary>
        /// �������� ��� ������ idfRow ��� ���������, ������������ � �������
        /// </summary>
        /// <param name="observationid"></param>
        /// <param name="numRow"></param>
        /// <param name="sectionTable">��������� ������, � ������� ��������� ���������</param>
        /// <returns></returns>
        private long GetIdfRowTableParameter(long observationid, int numRow, SectionTable sectionTable)
        {
            long result = -1;

            //����� ���� ���������� (����� ��������), ������� ��������� � ������ ��������� ������, ������� ����� ���� �� ����, ������� ���������� �� ������ ������
            //������, ������� ����� ������, ��������� � ���� �������
            foreach (BandedGridColumn column in sectionTable.GridViewMain.Columns)
            {
                var parameterTemplateRow = ((GridBand)column.Tag).Tag as FlexibleFormsDS.ParameterTemplateRow;
                if (parameterTemplateRow == null) continue;
                var activityParametersRows = (FlexibleFormsDS.ActivityParametersRow[])MainDbService.MainDataSet.ActivityParameters.Select(DataHelper.Filter("idfObservation", observationid, "idfsParameter", parameterTemplateRow.idfsParameter, "intNumRow", numRow));
                if (activityParametersRows.Length > 0)
                {
                    result = activityParametersRows[0].idfRow;
                    break;
                }
            }
            //���� �� ������� �����, �� �������� �����
            if (result == -1) result = BaseDbService.NewIntID();
            return result;
        }

        /// <summary>
        /// ������ �� ������� ��� Flexible Forms
        /// </summary>
        public DbServiceUserData MainDbService { get { return (DbServiceUserData)DbService; } }

        /// <summary>
        /// ���������� Flexible Form c ������� �� ����������� ������
        /// </summary>
        /// <param name="idfVersionStub">������ �������� (���� �� �������, �� ������������ ��, ��� ����������� ��� ������� observation)</param>
        /// <param name="idfObservation">ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        /// <param name="idfsFormTemplate">������, ������� ��������� ������������ ��� ������ ������� �.�.</param>
        /// <param name="runImmediatly">��������� ������ ����������, �� ������� �������� �� �����������</param>
        /// <param name="isSummary">��� ������ ������� Summary. ��� ������ � ��������� summary ����������� ShowSummary</param>
        public void ShowAggregateCase(long idfObservation, long? idfsFormTemplate, long? idfVersionStub, FFType aformType, bool runImmediatly, bool isSummary)
        {
            var observations = new List<long>(1) { idfObservation };

            #region ����������� �������

            //����������: ���������� -> �� observation -> �� �������������

            TemplateID = idfsFormTemplate;
            IsUNITemplate = false;

            if (!TemplateID.HasValue)
            {
                //�������� idfsFormTemplate �� ������� Observations
                var observationsRow = MainDbService.LoadObservations(idfObservation);
                if ((observationsRow != null) && (!observationsRow.IsidfsFormTemplateNull()))
                    TemplateID = observationsRow.idfsFormTemplate;

                if (!TemplateID.HasValue)
                {
                    SetActualTemplate(EidssSiteContext.Instance.CountryID, null, aformType);
                }
            }

            #endregion

            //���� ����� �� ������� ��������� ������, �� �������
            if ((TemplateID.HasValue) && (TemplateID > 0))
            {
                ShowFlexibleForm(TemplateID.Value, observations, aformType, idfVersionStub, runImmediatly, isSummary);
            }
        }

        /// <summary>
        /// ��������� ������������ �������� ���������� ����� � ����� ����������
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="aformType"> </param>
        public void ShowFlexibleFormGroupEditing(List<long> observations, FFType aformType)
        {
            if (observations.Count == 0) return;
            var observationsRows = MainDbService.LoadObservations(observations);
            if (observationsRows.Length == 0) return;
            if (observationsRows[0].IsidfsFormTemplateNull()) return;
            var idTemplate = observationsRows[0].idfsFormTemplate;
            //�� ���� ���������� ����������� ������ ��������� �������
            if (observationsRows.Any(o => o.IsidfsFormTemplateNull() || !o.idfsFormTemplate.Equals(idTemplate))) return;
            observationsGroupEditing.AddRange(observations);
            ShowFlexibleForm(idTemplate, observations, aformType/*, null*/, null, true, false, false, true);
        }

        /// <summary>
        /// ��������� ����������, ������� ������������ ��� ���������� �������������� (���������� �����)
        /// </summary>
        private List<long> observationsGroupEditing { get; set; }

        /// <summary>
        /// ��������� �� ��������� � ������ ���������� ��������������
        /// </summary>
        public bool IsGroupEditingRegime { get; private set; }

        /// <summary>
        /// ���������� Flexible Form c �������, �������� ��������� ������ ������������������ ����������, �� ������� ������
        /// </summary>
        /// <param name="observations">��������� ID ��������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        public bool ShowSummary(List<long> observations, FFType aformType)
        {
            nowLoading = ffRender.NowLoadingTemplate = true;
            IsGroupEditingRegime = false;
            var isEmptySummary = observations.Count == 0;

            if (isEmptySummary)
            {
                SetActualTemplate(EidssSiteContext.Instance.CountryID, null, aformType);
                if (TemplateID == null) return false;
            }
            else
            {
                TemplateID = MainDbService.IdfsFormTemplateSummary;
                //�������� ��� ��������, ������ ��� � ������� ��������� ��������� Observation � � ������� ����� ���� ���� ������ �������� � �������
                VersionMatrixStub = null;
            }

            ObservationID = MainDbService.IdfObservationSummary;

            //��������� ���������
            var isOk = ffRender.MergeTemplates(MainDbService.IdfsFormTemplateSummary, observations, aformType);
            List<SummaryRow> summaryRowList = null;
            if (!isEmptySummary && isOk)
            {
                var observationsRows = MainDbService.LoadObservations(observations);

                //��������� ������
                //�������� ���������������� ������
                MainDbService.LoadUserData(observations);


                //���� ���������� ����� ������ � �������� � � ��������� ObservationSummary, � ����� ���������� ���
                //����������� ����� ������ �� ���������, � ������� ��� Numeric. ��������� ������������ (������, 11.01.2010).
                //����������� ������ ���� �������, ������� �� �����������
                //� ���������� ������������� ������ �� �����������

                //������ summary
                summaryRowList = MainDbService.CalculateSummary(observations);

                var listStubInfo = MainDbService.GetListSectionsAndVersionsForStub(observationsRows, aformType);
                foreach (var stubInfo in listStubInfo)
                {
                    //������ ���� ����.������ ������ ���������������
                    var sectionTable = GetSectionTable(stubInfo.IdfsSectionTable);
                    if (sectionTable == null) continue;
                    sectionTable.GridViewMain.OptionsBehavior.Editable = false;
                }
            }
            //���������� ����� ����������
            ReadOnlyMode = true;

            //���� �� ������� ��������� �������, �� � ������������� �� �����
            if (isOk)
            {
                List<FlexibleFormsDS.PredefinedStubRow> predefinedStubRows;
                //������� ������ �������� � ���������������� ������
                MainDbService.IncludeStubDataInUserData(TemplateID, ObservationID, null, summaryRowList, out predefinedStubRows);

                //��������� �������� �� ����������
                SetValuesToParameters(predefinedStubRows);
            }

            nowLoading = ffRender.NowLoadingTemplate = false;

            return isOk;
        }

        /// <summary>
        /// ���������� �������� ������
        /// </summary>
        /// <param name="countryDeterminant">�����������-������</param>
        /// <param name="secondDeterminant">����������� ������� ��� ����</param>
        /// <param name="aformType"></param>
        private void SetActualTemplate(long countryDeterminant, long? secondDeterminant, FFType aformType)
        {
            MainDbService.LoadActualTemplate(countryDeterminant, secondDeterminant, (long)aformType);
            if (MainDbService.MainDataSet.ActualTemplate.Rows.Count != 1) return;
            var row = (FlexibleFormsDS.ActualTemplateRow)MainDbService.MainDataSet.ActualTemplate.Rows[0];
            TemplateID = row.idfsFormTemplate;
            IsUNITemplate = row.IsUNITemplate;
            //���� ������ �� ������, �� �� ����� -1
            if (TemplateID == -1) TemplateID = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secondDeterminant"></param>
        /// <param name="aformType"></param>
        /// <returns></returns>
        public long? GetActualTemplate(long? secondDeterminant, FFType aformType)
        {
            MainDbService.LoadActualTemplate(EidssSiteContext.Instance.CountryID, secondDeterminant, (long)aformType);
            if (MainDbService.MainDataSet.ActualTemplate.Rows.Count != 1) return null;
            var row = (FlexibleFormsDS.ActualTemplateRow)MainDbService.MainDataSet.ActualTemplate.Rows[0];
            return row.idfsFormTemplate;
        }

        /// <summary>
        /// ����������� ���������������� ������
        /// </summary>
        public void Copy()
        {
            if ((TemplateID == null) || (ObservationID == null)) return;

            //���������� ���������� ������ ������
            activityParametersBuffer.IdfsFormTemplate = TemplateID;
            activityParametersBuffer.IdfsObservation = ObservationID;
            activityParametersBuffer.UserDataBuffer = (FlexibleFormsDS.ActivityParametersRow[])MainDbService.MainDataSet.ActivityParameters.Select(DataHelper.Filter("idfObservation", ObservationID));
        }

        /// <summary>
        /// ����� �� ����������� ������� ������
        /// </summary>
        public bool CanPaste()
        {
            //���� ����� ����, �� ������ ��������� ������
            bool result = !activityParametersBuffer.IsEmpty();
            if (result)
            {
                //���� ���� � ��� �� ������, �� ������ ��������� ����� 
                result = (activityParametersBuffer.IdfsFormTemplate.Equals(TemplateID));
                //� ���� ���� � ��� �� Observation, �� ������ (��� ������� ������)
                if (result)
                {
                    result = !activityParametersBuffer.IdfsObservation.Equals(ObservationID);
                }
            }

            return result;
        }

        /// <summary>
        /// ������� ���������������� ������
        /// </summary>
        public void Paste()
        {
            if (!CanPaste()) return;

            if (ObservationID == null) return;
            if (activityParametersBuffer.UserDataBuffer.Length == 0) return;

            //�������������� ������� ��� ������, ������� ����� ���� � observation, � ������� ����������� ������
            ClearUserData(ObservationID.Value);

            //��� ���� ����� ���������������� ������, ���������� � �����, �������� �� �����
            CopyHelper.CopyActivityParameters(MainDbService, ObservationID.Value, activityParametersBuffer.UserDataBuffer);

            //��������� ����������� ������ � ���������
            SetValuesToParameters(null);
        }

        /// <summary>
        /// ������� ������ �����
        /// </summary>
        public void Clear()
        {
            if ((MainDbService.MainDataSet.ActivityParameters.Count == 0) || !ObservationID.HasValue) return;
            //���� ����� ����������� �������� ������� � ������ ����� � observation, �� �� ����� ��������� ��������
            if (!activityParametersBuffer.IsEmpty() && (ObservationID.Equals(activityParametersBuffer.IdfsObservation)))
            {
                activityParametersBuffer.Clear();
            }
            //������� ������ � �����
            for (var i = MainDbService.MainDataSet.ActivityParameters.Count - 1; i >= 0; i--)
            {
                DeleteValueRow(MainDbService.MainDataSet.ActivityParameters[i], ObservationID.Value);
            }
        }

        /// <summary>
        /// ������� �������
        /// </summary>
        public void ClearTemplate()
        {
            ffRender.ClearTemplate();
        }

        /// <summary>
        /// �������� ������ ������ ������������ ����������
        /// </summary>
        private void ClearUserDataOfDinamicParameters()
        {
            if (ObservationID == null) return;

            var parametersForDelete = new List<long>();
            foreach (var par in ffRender.ParameterList.Values)
            {
                var pt = par.GetParameterTemplateRow();
                if (pt == null || !pt.blnDynamicParameter) continue;
                parametersForDelete.Add(pt.idfsParameter);
            }

            //������� ���������������� ������ � �������
            for (var i = MainDbService.MainDataSet.ActivityParameters.Count - 1; i >= 0; i--)
            {
                var activityParametersRow = MainDbService.MainDataSet.ActivityParameters[i];
                if (!activityParametersRow.IsRowAlive() ||
                    !parametersForDelete.Contains(activityParametersRow.idfsParameter)) continue;

                //������� ���������������� ������
                DeleteValueRow(activityParametersRow, ObservationID.Value);
            }

            foreach (var p in parametersForDelete)
            {
                ffRender.ParameterList[p].Delete();
            }

            //�������� ������ � ������������� �����������
            ffRender.SetVisibleDynamicParametersGroupControl(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="idfsObservation"></param>
        private void DeleteValueRow(FlexibleFormsDS.ActivityParametersRow row, long? idfsObservation)
        {
            if (!row.IsRowAlive()) return;
            if ((idfsObservation != null) && (!row.idfObservation.Equals(idfsObservation))) return;
            //������� �������
            if (ffRender.ParameterList.ContainsKey(row.idfsParameter))
            {
                var parameter = ffRender.ParameterList[row.idfsParameter];
                if (parameter != null) parameter.ControlParameter.EditValue = null;
            }
            row.SetvarValueNull();
            row.intRowState = 1; //������� ����� �������
        }

        /// <summary>
        /// ������� ������ ����� (��������� ��������� ��� ������)
        /// </summary>
        public void ClearUserData()
        {
            ClearUserData(null);
        }

        /// <summary>
        /// ������� ������ ����� (�� ���������� observation)
        /// </summary>
        public void ClearUserData(long? idfsObservation)
        {
            if (MainDbService.MainDataSet.ActivityParameters.Count == 0) return;
            for (var i = MainDbService.MainDataSet.ActivityParameters.Count - 1; i >= 0; i--)
            {
                DeleteValueRow(MainDbService.MainDataSet.ActivityParameters[i], idfsObservation);
            }
        }


        /// <summary>
        /// ����������  Flexible Form c ������� �� ������� ������������ (������ -- ������)
        /// </summary>
        /// <param name="diagnosisID">ID ��������</param>
        /// <param name="idfObservation">ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        public void ShowFlexibleFormByDeterminant(long diagnosisID, long idfObservation, FFType aformType)
        {
            ShowFlexibleFormByDeterminant(diagnosisID, idfObservation, aformType, true);
        }

        /// <summary>
        /// ����������  Flexible Form c ������� �� ������� ������������ (������ -- ������)
        /// </summary>
        /// <param name="diagnosisID">ID ��������</param>
        /// <param name="idfObservation">ID �������� (observation)</param>
        /// <param name="aformType">ID ���� �����</param>
        /// <param name="bufferingUserData"></param>
        public void ShowFlexibleFormByDeterminant(long diagnosisID, long idfObservation, FFType aformType, bool bufferingUserData)
        {
            TemplateID = null;
            IsUNITemplate = null;

            SetActualTemplate(EidssSiteContext.Instance.CountryID, diagnosisID, aformType);

            if (TemplateID.HasValue && (TemplateID > 0))
            {
                ShowFlexibleForm(TemplateID.Value, idfObservation, aformType, bufferingUserData);
            }
            else
            {
                ffRender.ClearTemplate();
            }
        }

        private long? m_ObservationID;

        /// <summary>
        /// idfsFormTemplate, ������� ���������� ������ ���������
        /// </summary>
        public long? TemplateID { get; private set; }

        /// <summary>
        /// ��� �����, � ������� �������� ������ ���������
        /// </summary>
        public FlexibleFormsDS.FormTypesRow FormTypeRow { get; private set; }

        /// <summary>
        /// idfVersion �������, ������� ������������ � ������ ������. ����������� ������ ��� Aggregate Case.
        /// </summary>
        public long? VersionMatrixStub { get; private set; }

        /// <summary>
        /// idfObservation, ������� ���������� ������ ���������
        /// </summary>
        public long? ObservationID
        {
            get { return m_ObservationID; }
            private set
            {
                m_ObservationID = value;
                if (m_ObservationID.HasValue && TemplateID.HasValue)
                {
                    if (!observationsDictionary.ContainsKey(m_ObservationID.Value))
                    {
                        observationsDictionary.Add(m_ObservationID.Value, TemplateID.Value);
                    }
                    else if (!observationsDictionary[m_ObservationID.Value].Equals(TemplateID.Value))
                    {
                        observationsDictionary[m_ObservationID.Value] = TemplateID.Value;
                    }

                }
            }
        }

        /// <summary>
        /// ��������� ��� observarionID-idfsFormTemplate, ������� ����������� �� ����� ������ ������� FFPresenter
        /// </summary>
        private Dictionary<long, long> observationsDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool HasChanges()
        {
            //��������, ���������� �� ���-������
            var result = ReadOnlyMode;
            if (!result)
            {
                var dataTable = (FlexibleFormsDS.ActivityParametersDataTable)MainDbService.MainDataSet.ActivityParameters.GetChanges();
                if (dataTable != null)
                {
                    foreach (var row in dataTable)
                    {
                        if (!row.IsRowAlive() || (row.intRowState == 1))
                        {
                            result = true;
                            break;
                        }
                        //��������, �������� �� ��������, � �������� ��������� ������ ������, ������ ��������. ���� ��, �� ��� ��������� �� ���������, ������ ��� ��� ���������.
                        var parameterTemplateRow = MainDbService.GetParameterTemplateRow(row.idfsParameter, row.idfsFormTemplate);
                        //��������� ������, � ������� ��������� ��������
                        if (parameterTemplateRow == null) continue;
                        //����� �� ��������� ������� ���������� ��������� (��������� �� ����� ���������� ������ ��������� ������, ����� �� ����������� ������ �� ��������)
                        var needCheckOrder = false;
                        if (!parameterTemplateRow.IsidfsSectionNull())
                        {
                            //���������, �������� �� ������������ ������ ���������
                            var sectionsRow = MainDbService.GetSectionRow(parameterTemplateRow.idfsSection);
                            needCheckOrder = sectionsRow.blnGrid;
                        }
                        if (!needCheckOrder || (parameterTemplateRow.intOrder >= 0))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <returns></returns>
        public override bool ValidateData()
        {
            //������� �������� ������
            //(��������� ��� ���� ����������)
            if (!RulesCheck(null, FFRuleCheckPointType.OnSaveWithNotify) ||
                !RulesCheck(null, FFRuleCheckPointType.OnSaveWithError)) return false;

            if (TemplateID.HasValue && !ReadOnly)
            {
                #region �������� ������������ �����

                FlexibleFormsDS.ParameterTemplateRow parameterTemplateRow;
                if (!MainDbService.CheckMandatoryParameters(observationsDictionary, out parameterTemplateRow) && (parameterTemplateRow != null))
                {
                    //���������� ��������, �� ������� ����� ������ (�� ����� ���� � � ������ �����, ������� ������ �� ���������������, ����� �� ������)
                    if (ffRender.ParameterList.ContainsKey(parameterTemplateRow.idfsParameter))
                    {
                        var parameter = ffRender.ParameterList[parameterTemplateRow.idfsParameter];
                        ShowErrorAtValidateMandatoryFields(parameter, parameter.Text);
                        return false;
                    }
                }

                /*
                foreach (KeyValuePair<long, Parameter> pair in ffRender.ParameterList)
                {
                    Parameter parameter = pair.Value;
                    FlexibleFormsDS.ParameterTemplateRow parameterTemplateRow = parameter.ParameterTemplateRow;
                    if (parameterTemplateRow == null) continue;
                    //������� ������������ ���� � �������� �� ��� ������ ��� ����������� �� �������
                    //�� ��������� �� ���������, ������� �� ������ � ������� ������
                    if (parameterTemplateRow.idfsFormTemplate.Equals(TemplateID.Value) && parameter.IsMandatoryFieldEmpty())
                    {
                        ShowErrorAtValidateMandatoryFields(parameter, parameter.Text);
                        return false;
                    }
                }
                */

                #endregion
            }

            return base.ValidateData();
        }

        /// <summary>
        /// ������, �������� � �������� ������
        /// </summary>
        public FlexibleFormsDS.ActivityParametersDataTable ActivityParameters
        {
            get { return MainDbService.MainDataSet.ActivityParameters; }
        }

        /// <summary>
        /// ���������, �������� � �������� ������
        /// </summary>
        public FlexibleFormsDS.ParameterTemplateDataTable ParameterTemplate
        {
            get { return MainDbService.MainDataSet.ParameterTemplate; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVisibleChanged(object sender, EventArgs e)
        {
            //���� ��������� ���� ����� � ���� ��������� ��������� ��� ���������� ��������, �� �������� ��������
            if (!Visible || !DelayedLoadingNeeded) return;
            ShowFlexibleForm(m_IdfsFormTemplateStored, m_ObservationsStored, m_FormTypeStored/*, m_PresetStubsStored*/, m_IdfVersionStored, false, false);
        }

        /// <summary>
        /// ���������� ������������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddParameterItemClick(object sender, ItemClickEventArgs e)
        {
            if (!TemplateID.HasValue || (FormTypeRow == null)) return;
            if (!FormTypeRow.IsRowAlive()) return;

            //����� ���������
            var form = new FFTreeParameterForTemplate(MainDbService, FormTypeRow, ffRender.ParameterList);
            var result = BaseFormManager.ShowModal(form, FindForm());

            if (result)
            {
                if (form.SelectedParameters.Count == 0) return;
                //������� ������ ������������ ����������, ���� ��� ��� �� �����
                ffRender.ShowDynamicParametersGroupControl(TemplateID.Value);
                //
                foreach (var idfsParameter in form.SelectedParameters)
                {
                    var parametersRow = MainDbService.GetParameterRow(idfsParameter);
                    //������� ��������� �������� �� ��
                    ffRender.CreateParameterInParameterDynamicParametersGroupControl(parametersRow, TemplateID.Value);
                }
            }
        }
        
        private void OnBtnRemoveAllDynamicParameterItemClick(object sender, ItemClickEventArgs e)
        {
            ClearUserDataOfDinamicParameters();
        }
        
        private bool TemplateHasMandatoryFields(long idfsFormTemplate)
        {
            bool result = true;

            string commandText = String.Format("Select [Result] from dbo.fnFFTemplateHasMandatoryFields({0})", idfsFormTemplate);
            var cmd = DbService.CreateCommand(commandText);
            ErrorMessage errMsg = null;
            var o = BaseDbService.ExecScalar(cmd, DbService.Connection, ref errMsg);
            //
            if (o is bool)
            {
                result = Convert.ToBoolean(o);
            }

            return result;
        }

        /// <summary>
        /// ���������� �� ��������� � ��������� ������ ������������ �������
        /// </summary>
        public bool SectionTableCaptionsIsVisible
        {
            get { return ffRender == null || ffRender.SectionTableCaptionsIsVisible; }
            set { if (ffRender != null) ffRender.SectionTableCaptionsIsVisible = value; }
        }

        public CriticalObjectsHelper CriticalObjects { get { return ffRender != null ? ffRender.CriticalObjects : null; } }
    }
}
