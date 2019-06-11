using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using EIDSS.FlexibleForms.Components;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.DesignerHosting;
using bv.model.Model.Core;
using eidss.model.Core;
using Label=EIDSS.FlexibleForms.Components.Label;
using eidss.model.Resources;

namespace EIDSS.FlexibleForms.Helpers
{
    /// <summary>
    /// ����������� ����������� ������������� ����
    /// </summary>
    public class FFRender
    {
        /// <summary>
        /// ������ �� ������� ��� Flexible Forms
        /// </summary>
        public DbService MainDbService { get; private set; }

        /// <summary>
        /// ���������-����, ���� �������������� ����� �������
        /// </summary>
        internal DesignerHost TemplateDesignerHost { get; private set; }

        /// <summary>
        /// ������ ��� ����������� ������������ ����������
        /// </summary>
        internal GroupControl DynamicParametersGroupControl { get; set; }

        /// <summary>
        /// ������ � ���������� ������ ������������ ����������
        /// </summary>
        internal void ShowDynamicParametersGroupControl(long idfsFormTemplate)
        {
            if (DynamicParametersGroupControl == null)
            {
                DynamicParametersGroupControl = new GroupControl
                                                    {Text = EidssMessages.Get("DynamicParametersGroupControlCaption")};
            }

            int left;
            int labelSize;
            int width;
            DataHelper.GetSizesForFit(MainDbService, idfsFormTemplate, null, out left, out labelSize, out width);

            if (!TemplateDesignerHost.Controls.Contains(DynamicParametersGroupControl)) TemplateDesignerHost.Controls.Add(DynamicParametersGroupControl);
            
            DynamicParametersGroupControl.Location = TemplateDesignerHost.GetMostRightTopControl(new Point(5, 5), DynamicParametersGroupControl);
            DynamicParametersGroupControl.Left = left;
            DynamicParametersGroupControl.Width = width;
            //������ ����������� ������, ������� ����� ����� ��������� �����������.
            DynamicParametersGroupControl.Height = 20;
            SetVisibleDynamicParametersGroupControl(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visible"></param>
        internal void SetVisibleDynamicParametersGroupControl(bool visible)
        {
            if (DynamicParametersGroupControl != null) DynamicParametersGroupControl.Visible = visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="mainTemplateDesignerHost"></param>
        public FFRender(DbService mainDbService, DesignerHost mainTemplateDesignerHost)
        {
            MainDbService = mainDbService;
            TemplateDesignerHost = mainTemplateDesignerHost;
            
            ParameterList = new Dictionary<long, Parameter>();
            SectionTableList = new Dictionary<long, SectionTable>();
            SectionList = new Dictionary<long, Section>();
            LineList = new Dictionary<long, Line>();
            LabelList = new Dictionary<long, Label>();
            CriticalObjects = new CriticalObjectsHelper();

            LanguageTemplate = bv.common.Core.Localizer.lngEn;

            SectionTableCaptionsIsVisible = true;
        }

        /// <summary>
        /// ����, � ������� ����������� ������
        /// </summary>
        private string LanguageTemplate { get; set; }

        /// <summary>
        /// ������ ����������� � ������� ���������� ��� �������� ������� � ���. �������� ���� [idfsParameter, ��������� ������ Parameter]
        /// </summary>
        internal Dictionary<long, Parameter> ParameterList{ get; set; }

        /// <summary>
        /// ������ ����������� � ������� ��������� ������ ��� �������� ������� � ���. �������� ���� [idfsSection, ��������� ������ SectionTable]
        /// </summary>
        internal Dictionary<long, SectionTable> SectionTableList{ get; set; }

        /// <summary>
        /// ������ ����������� � ������� ������ ��� �������� ������� � ���. �������� ���� [idfsSection, ��������� ������ Section]
        /// </summary>
        internal Dictionary<long, Section> SectionList{ get; set; }

        /// <summary>
        /// ������ ����������� � ������� ����� ��� �������� ������� � ���. �������� ���� [idfDecorElement, ��������� ������ Line]
        /// </summary>
        internal Dictionary<long, Line> LineList { get; set; }

        /// <summary>
        /// ������ ����������� � ������� ������� ��� �������� ������� � ���. �������� ���� [idfDecorElement, ��������� ������ Label]
        /// </summary>
        internal Dictionary<long, Label> LabelList { get; set; }

        /// <summary>
        /// ���������� ������� ��������� �� ����� ����������
        /// </summary>
        /// <param name="idfsParameter"></param>
        /// <returns></returns>
        internal Parameter GetParameter(long idfsParameter)
        {
            Parameter parameter = null;
            if (ParameterList.ContainsKey(idfsParameter))
                parameter = ParameterList[idfsParameter];
            return parameter;
        }

        private bool m_ReadOnly;

        /// <summary>
        /// 
        /// </summary>
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set
            {
                m_ReadOnly = value;
                TemplateDesignerHost.ReadOnly = m_ReadOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        internal SectionTable GetSectionTable(long idfsSection)
        {
            SectionTable result = null;
            if (SectionTableList != null)
                if (SectionTableList.ContainsKey(idfsSection))
                    result = SectionTableList[idfsSection];
            return result;
        }

        /// <summary>
        /// ���������� ��������� ������, ���� ������� ������� � �������
        /// </summary>
        /// <param name="index">���������� ����� ��������� ������</param>
        /// <returns>���������� ��������� ������, ���� ������� ������� � �������, ��� null, ���� � ���</returns>
        public SectionTable GetSectionTableByIndex(int index)
        {
            SectionTable result = null;
            if (SectionTableList != null)
                if ((SectionTableList.Count > 0) && (index <= SectionTableList.Count))
                    foreach (var sectionTable in SectionTableList.Values)
                    {
                        result = sectionTable;
                        break;
                    }

            return result;
        }

        /// <summary>
        /// ��������� �������� �� ����-�������
        /// </summary>
        /// <param name="rowParameter"></param>
        /// <param name="mouseLocation"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="isSummaryTemplate"></param>
        /// <returns></returns>
        internal Parameter CreateParameterInTemplateHost(FlexibleFormsDS.ParametersRow rowParameter, Point mouseLocation, long idfsFormTemplate, bool isSummaryTemplate)
        {
            var rowParameterTemplate = MainDbService.CreateParameterTemplateRow(rowParameter, idfsFormTemplate);
            rowParameterTemplate.intLeft = mouseLocation.X;
            rowParameterTemplate.intTop = mouseLocation.Y;

            return CreateParameterInTemplateHost(rowParameterTemplate, mouseLocation, isSummaryTemplate);
        }

        /// <summary>
        /// ��������� �������� �� ����-�������
        /// </summary>
        /// <param name="rowParameter"></param>
        /// <param name="parameterLocation"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="parentControl">���� ��������� ��������� ��������</param>
        /// <param name="dynamicParameter">������������ �� ��� ��������</param>
        /// <returns></returns>
        internal Parameter CreateParameterInTemplateHost(FlexibleFormsDS.ParametersRow rowParameter, Point parameterLocation, long idfsFormTemplate, Control parentControl, bool dynamicParameter)
        {
            var rowParameterTemplate = MainDbService.CreateParameterTemplateRow(rowParameter, idfsFormTemplate);
            rowParameterTemplate.intLeft = parameterLocation.X;
            rowParameterTemplate.intTop = parameterLocation.Y;
            rowParameterTemplate.blnDynamicParameter = dynamicParameter;

            if (dynamicParameter)
            {
                int left;
                int labelSize;
                int width;
                DataHelper.GetSizesForFit(MainDbService, idfsFormTemplate, null, out left, out labelSize, out width);

                if (labelSize > 0) rowParameterTemplate.intLabelSize = labelSize;
                rowParameterTemplate.intWidth = DynamicParametersGroupControl.Width - 10;
            }

            return CreateParameterInTemplateHost(rowParameterTemplate, parameterLocation, false);
        }

        /// <summary>
        /// ��������� �������� �� ������ ������������ ����������
        /// </summary>
        /// <param name="rowParameter"></param>
        /// <param name="idfsFormTemplate"></param>
        internal void CreateParameterInParameterDynamicParametersGroupControl(FlexibleFormsDS.ParametersRow rowParameter, long idfsFormTemplate)
        {
            //��������� ���� ��������� ��������
            //��������� ����� ������� ���������� ��� ������������ ���������
            var location = DynamicParametersGroupControl.GetMostRightTopControl(new Point(5, 25), null);
            //���� �������� ���� � ���� �������� ���� ������, �� �������� �
            var parameterWidth = location.X + rowParameter.intWidth;
            var parameterHeight = location.Y + rowParameter.intHeight;
            if (parameterWidth > DynamicParametersGroupControl.Width) rowParameter.intWidth = DynamicParametersGroupControl.Width - 10;
            if (parameterHeight > DynamicParametersGroupControl.Height) DynamicParametersGroupControl.Height = parameterHeight + 10;
            //
            CreateParameterInTemplateHost(rowParameter, location, idfsFormTemplate, DynamicParametersGroupControl, true);
        }

        /// <summary>
        /// ���������� ������� �������
        /// </summary>
        internal void ClearTemplate()
        {
            NowClearingTemplate = true;

            var lines = new List<Line>(LineList.Values);
            for (var i = lines.Count - 1; i >= 0; i--) { lines[i].Delete(false); }

            var labels = new List<Label>(LabelList.Values);
            for (var i = labels.Count - 1; i >= 0; i--) { labels[i].Delete(false); }

            var parameters = new List<Parameter>(ParameterList.Values);
            for (var i = parameters.Count - 1; i >= 0; i--) { parameters[i].Delete(false); }

            var sectionTables = new List<SectionTable>(SectionTableList.Values);
            for (var i = sectionTables.Count - 1; i >= 0; i--) { sectionTables[i].Delete(false); }

            var sections = new List<Section>(SectionList.Values);
            for (var i = sections.Count - 1; i >= 0; i--) { sections[i].Delete(false); }
            
            //������� ������� �������� �������
            LineList.Clear();
            LabelList.Clear();
            ParameterList.Clear();
            SectionTableList.Clear();
            SectionList.Clear();

            foreach (Control c in TemplateDesignerHost.Controls) 
            {
                var st = c as SectionTable;
                if (st != null) 
                    st.Delete(false);
            }

            NowClearingTemplate = false;
        }

        /// <summary>
        /// ��������������� ������ �� ��������-�����
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsFormType"></param>
        /// <param name="langid"></param>
        /// <param name="idfVersion"></param>
        /// <param name="presetStubs"></param>
        /// <param name="idfsMatrixType"></param>
        /// <param name="isSummary"></param>
        internal void RestoreTemplate(long idfsFormTemplate, long idfsFormType, string langid, List<long> presetStubs, long? idfVersion, long? idfsMatrixType, bool isSummary)
        {
            NowLoadingTemplate = true;
            //������� ������� ������� �������
            ClearTemplate();

            TemplateDesignerHost.SuspendLayout();
            Application.DoEvents();

            if (langid == null) langid = ModelUserContext.CurrentLanguage;
            TemplateDesignerHost.AutoScrollPosition = new Point(0, 0);
            TemplateDesignerHost.DisplayRectangleLeftStart = TemplateDesignerHost.DisplayRectangle.Left;
            TemplateDesignerHost.DisplayRectangleTopStart = TemplateDesignerHost.DisplayRectangle.Top;
            
            //���� �� ������� ���� �����, � �������� ��������� ������, �� ��������� ����-������ �� �������, �� ���������� ��
            //(�������� ��� ������)
            MainDbService.LoadSections(idfsFormType, null, null);
            MainDbService.LoadParameters(idfsFormType, null);
            
            //����������� ������ �� ����� ������� (���� ��� �� ��� ��� �� ���������)
            MainDbService.LoadSectionTemplate(idfsFormTemplate, langid);
            MainDbService.LoadParameterTemplate(idfsFormTemplate, langid);

            var stubLength = 0;
            if ((presetStubs != null) && idfVersion.HasValue && idfsMatrixType.HasValue)
            {
                //���������� ����������� �������
                stubLength = presetStubs.IncludeStub(MainDbService, idfsFormTemplate, idfVersion.Value, idfsMatrixType.Value);
            }

            //��������� �����
            //MainDbService.LoadLines(idfsFormTemplate, langid);
            //��������� ������
            MainDbService.LoadLabels(idfsFormTemplate, langid);

            #region ��������� ��, ��� ������� � ���������

            MainDbService.LoadRulesForTemplate(idfsFormTemplate);
            MainDbService.LoadRuleFunctions(-1); //��������, ��� ����� ��������� ��� ������� (��� ���������� ������� ��� ������)
            //��� ������� ������� �������� ��� ��������������
            foreach (var row in MainDbService.MainDataSet.Rules)
            {
                if (!row.IsRowAlive()) continue;
                if (!row.idfsFormTemplate.Equals(idfsFormTemplate)) continue;
                //��������� ������ �� ���������� �������, ������� � ���� �������
                MainDbService.LoadRuleParameterForFunction(row.idfsRule);
                //��������� �������� �� �������
                MainDbService.LoadRuleParameterForAction(row.idfsRule);
                //��������� ��������� (��� ����������������� ��������) �� �������
                MainDbService.LoadRuleConstants(row.idfsRule);
            }

            #endregion

            //���� ������ �������� ���� �������, � �� ������ �������, �� ������� ���
            var finded = false;
            var pts = MainDbService.GetParameterTemplateRows(null, idfsFormTemplate, langid);
            foreach (var parameterTemplate in pts)
            {
                if (parameterTemplate.intOrder >= 0)
                {
                    finded = true;
                    break;
                }
            }

            if (finded)
            {
                //������� ��������� ��� ������, �������� �� �����������
                RestoreSections(idfsFormTemplate, langid, isSummary);
                //��������� ��� ��������� �� �� ���������
                RestoreParameters(idfsFormTemplate, langid, isSummary);
                //��������� ��� �����
                RestoreLines(idfsFormTemplate, langid);
                //��������� ������
                RestoreLabels(idfsFormTemplate, langid);

                //��������� ������� ��������� ������ � ���������� �������
                foreach (var sectionTable in SectionTableList.Values)
                {
                    sectionTable.RefreshBandPositions(sectionTable.GridViewMain.Bands);
                }

                //���� ��� ����������������� �������
                if (presetStubs != null)
                {
                    //������������� ��������
                    presetStubs.PostProcessingStub(SectionTableList, stubLength);
                }

                //���������� ������� �������� �� ����������
                var listTemplate = TemplateHelper.GetReportView(MainDbService, idfsFormTemplate);
                var tabIndex = 0;
                foreach (var dataRow in listTemplate)
                {
                    var parameterTemplateRow = dataRow as FlexibleFormsDS.ParameterTemplateRow;
                    if (parameterTemplateRow == null) continue;
                    if (!ParameterList.ContainsKey(parameterTemplateRow.idfsParameter)) continue;
                    ParameterList[parameterTemplateRow.idfsParameter].TabIndex = tabIndex;
                    tabIndex++;
                }
            }
            TemplateDesignerHost.ResumeLayout(true);
            
            NowLoadingTemplate = false;
        }

        /// <summary>
        /// ��������������� ������ �� ��������-�����
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsFormType"></param>
        /// <param name="langid"></param>
        internal void RestoreTemplate(long idfsFormTemplate, long idfsFormType, string langid)
        {
            RestoreTemplate(idfsFormTemplate, idfsFormType, langid, null, null, null, false);
        }

        /// <summary>
        /// ������������� ������ �� ������� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <param name="isSummary"></param>
        internal void RestoreSections(long idfsFormTemplate, string langid, bool isSummary)
        {
            var sectionTemplateRows = MainDbService.GetSectionTemplateRows(null, idfsFormTemplate, langid);
            //���� �� ������� �������� �������� �������, �� ����� ��������������� ���������� � ������� �����
            if (sectionTemplateRows.Length == 0)
                sectionTemplateRows = MainDbService.GetSectionTemplateRows(null, idfsFormTemplate);
            if (sectionTemplateRows.Length == 0) return;

            ////�������� ��� ��������� ������ �������� ������ (� ������� ��� ���������))
            //foreach (var sectionTemplateRow in sectionTemplateRows)
            //{
            //    if ((sectionTemplateRow.blnGrid) && (sectionTemplateRow.IsidfsParentSectionNull()))
            //    {
            //        CreateSectionInTemplateHost(sectionTemplateRow, new Point(sectionTemplateRow.intLeft, sectionTemplateRow.intTop), isSummary);
            //    }
            //}

            foreach (var sectionTemplateRow in sectionTemplateRows)
            {
                //������� �������� ������ �������� ������
                if (sectionTemplateRow.IsidfsParentSectionNull())
                    CreateSectionInTemplateHost(sectionTemplateRow,
                                                new Point(sectionTemplateRow.intLeft, sectionTemplateRow.intTop),
                                                isSummary);
            }

            foreach (var sectionTemplateRow in sectionTemplateRows)
            {
                //������� �������� ������ �������� ������
                if (!sectionTemplateRow.IsidfsParentSectionNull())
                    CreateSectionInTemplateHost(sectionTemplateRow,
                                                new Point(sectionTemplateRow.intLeft, sectionTemplateRow.intTop),
                                                isSummary);
            }
        }

        /// <summary>
        /// ���������� ��������� �� ��� ������ � ������
        /// </summary>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        internal bool SectionAlreadyAdded(long idfsSection)
        {
            return IsSectionTable(idfsSection) || SectionList.ContainsKey(idfsSection);
        }

        /// <summary>
        /// ���������� ��������� �� ��� ������ � ������
        /// </summary>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        internal bool IsSectionTable(long idfsSection)
        {
            return SectionTableList.ContainsKey(idfsSection);
        }

        /// <summary>
        /// ���������� �������� �� ��� �������� � ������
        /// </summary>
        /// <param name="idfsParameter"></param>
        /// <returns></returns>
        internal bool ParameterAlreadyAdded(long idfsParameter)
        {
            return ParameterList.ContainsKey(idfsParameter);
        }



        /// <summary>
        /// �������, ����� ������ �������
        /// </summary>
        public event ParameterHost.SelectHandler SectionSelect;

        /// <summary>
        /// �������, ����� �������� ������
        /// </summary>
        public event ParameterHost.SelectHandler ParameterSelect;

        /// <summary>
        /// �������, ����� ����� ������
        /// </summary>
        public event ParameterHost.SelectHandler LabelSelect;

        /// <summary>
        /// �������, ����� ����� �������
        /// </summary>
        public event ParameterHost.SelectHandler LineSelect;

        /// <summary>
        /// �������, ����� ��������� �� ������� �������� ���� ���������
        /// </summary>
        public event ParameterHost.SelectHandler ParameterHostUnSelect;

        /// <summary>
        /// �������, ����� ������ �������� �������
        /// </summary>
        public event EventHandler SectionResize;

        /// <summary>
        /// �������, ����� �������� �������� �������
        /// </summary>
        public event EventHandler ParameterResize;

        /// <summary>
        /// �������, ����� ����� �������� �������
        /// </summary>
        public event EventHandler LabelResize;

        /// <summary>
        /// �������, ����� ����� �������� �������
        /// </summary>
        public event EventHandler LineResize;

        /// <summary>
        /// �������, ����� ������������ ���� ���� �� ������� ����� ��������
        /// </summary>
        public event MouseEventHandler TemplateMouseDown;

        public delegate void EditValueChangingDelegate(object sender, ChangingEventArgs e);

        public delegate void SelectedBandChangedDelegate(GridBand selectedBand);

        /// <summary>
        /// �������, ����� ���������� ������� ���� � �����
        /// </summary>
        public event SelectedBandChangedDelegate OnSelectedBandChanged;

        /// <summary>
        /// ������� �����������, ����� �������� �������� � ������������ �������� ������ �� ������� ����������
        /// </summary>
        public event EditValueChangingDelegate EditValueChanging;

        public delegate void EditValueChangedDelegate(object sender, EventArgs e);

        /// <summary>
        /// ������� ��� �������� ������ ����������� mandatory ����������
        /// </summary>
        /// <param name="c"></param>
        public delegate void SetControlMandatoryStateDelegate(BaseControl c);

        /// <summary>
        /// ����������� ��� ����������� ����������� ����� mandatory ���������
        /// </summary>
        public event SetControlMandatoryStateDelegate ParameterIsMandatory;

        /// <summary>
        /// ������� �����������, �����  �������� � ������ ����������
        /// </summary>
        public event EditValueChangedDelegate EditValueChanged;

        /// <summary>
        /// ���������� �������� ������������ �������� � ����� �� ������� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnEditValueChanged(object sender, EventArgs e)
        {
            if (EditValueChanged != null) EditValueChanged(sender, e);
        }

        /// <summary>
        /// ��������� ��������� �������� (��� ������� ���������� � �������� � �������)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnEditValueChanging(object sender, ChangingEventArgs e)
        {
            var baseEdit = sender as BaseEdit;
            if (baseEdit == null) return;
            var parameter = baseEdit.Tag as Parameter;
            if ((parameter != null) && (parameter.IdfsParameterType != null) && (parameter.IdfsParameter != null))
            {
                if (MainDbService.IsParameterNumeric(parameter.IdfsParameter.Value))
                {
                    int value;
                    if ((e.NewValue != null) && Int32.TryParse(e.NewValue.ToString(), out value))
                    {
                        if (value > 99000000) e.Cancel = true;
                    }
                }
            }
            if ((EditValueChanging != null) && !e.Cancel) EditValueChanging(sender, e);
        }

        /// <summary>
        /// ���������� ������������ ������ ���� ������ �
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="parentSectionTemplateId"></param>
        /// <param name="mouseLocation"></param>
        /// <returns></returns>
        private Section FindParentSection(long idfsFormTemplate, long parentSectionTemplateId, Point mouseLocation)
        {
            Section result = null;

            //������ �� parentSectionTemplateId ������
            //������� ����� ������ �� ���������� ����������, ��������� ��� ������ ���� ��� ������
            var sectionsRow = MainDbService.GetSectionRow(parentSectionTemplateId);
            if (sectionsRow != null)
            {
                //������������� �� ���������� �����-��������, ����� ������, ���� �� � ��� ��� ��� ������
                if (SectionList.ContainsKey(sectionsRow.idfsSection))
                {
                    result = SectionList[sectionsRow.idfsSection];
                }
                else if (SectionTableList.ContainsKey(sectionsRow.idfsSection))
                {
                    result = SectionTableList[sectionsRow.idfsSection];
                }
                else
                {
                    //���� �������� ������ �� �������, �� ������� � �������
                    //��������� ����� � ����� ���, ��� ��������� � ������, �� �� �������� �� Design Host
                    var sectionTemplateRow = MainDbService.GetSectionTemplateRow(parentSectionTemplateId, idfsFormTemplate);
                    result = sectionTemplateRow != null ? CreateSectionInTemplateHost(sectionTemplateRow, mouseLocation) : CreateSectionInTemplateHost(sectionsRow, mouseLocation, idfsFormTemplate);
                }
            }

            return result;
        }

        /// <summary>
        /// ������ ������, � ����� ��� � ������������ ������, � ��������� �� �� ������-����� �������
        /// </summary>
        /// <param name="rowSection"></param>
        ///<param name="mouseLocation"></param>
        /// <param name="idfsFormTemplate"></param>
        internal Section CreateSectionInTemplateHost(FlexibleFormsDS.SectionsRow rowSection, Point mouseLocation, long idfsFormTemplate)
        {
            //��������� ������ � ������� ������
            var rowSectionTemplate = MainDbService.CreateSectionTemplateRow(rowSection, idfsFormTemplate);
            rowSectionTemplate.intLeft = mouseLocation.X;
            rowSectionTemplate.intTop = mouseLocation.Y;
            return CreateSectionInTemplateHost(rowSectionTemplate, mouseLocation);
        }

        public const int DefaultCaptionHeight = 23;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowSectionTemplate"></param>
        /// <param name="mouseLocation"></param>
        /// <returns></returns>
        internal Section CreateSectionInTemplateHost(FlexibleFormsDS.SectionTemplateRow rowSectionTemplate, Point mouseLocation)
        {
            return CreateSectionInTemplateHost(rowSectionTemplate, mouseLocation, false);
        }

        /// <summary>
        /// ������ ������, � ����� ��� � ������������ ������, � ��������� �� �� ������-����� �������
        /// </summary>
        /// <param name="rowSectionTemplate"></param>
        ///<param name="mouseLocation"></param>
        ///<param name="isSummaryTemplate"></param>
        internal Section CreateSectionInTemplateHost(FlexibleFormsDS.SectionTemplateRow rowSectionTemplate, Point mouseLocation, bool isSummaryTemplate)
        {
            //��������, ���� ��� ������ ��� ��������� �� ������� ����, �� �������
            if (SectionAlreadyAdded(rowSectionTemplate.idfsSection)) return null;
            var section = rowSectionTemplate.blnGrid ? new SectionTable() : new Section();
            section.NowLoading = true;
            section.MainDbService = MainDbService;
            section.ParentRender = this;
            section.Location = new Point(rowSectionTemplate.intLeft, rowSectionTemplate.intTop); //mouseLocation;
            section.Size = new Size(rowSectionTemplate.intWidth, rowSectionTemplate.intHeight);
            section.CaptionHeight = rowSectionTemplate.IsintCaptionHeightNull()
                                        ? DefaultCaptionHeight
                                        : rowSectionTemplate.intCaptionHeight;
            section.ReadOnly = ReadOnly;
            section.OnSelect += OnSectionOnSelect;
            section.Resize += OnSectionOnResize;
            section.Splitter.SplitterMoved += OnSectionOnResize;
            
            //������� ��������, ������� �� ��� ������ � ������ ������
            //� ������ ����� ���� ������ ���� ��������-������ ��� �� ���� �����
            //(���������� �������)
            Section parentSection = null;
            if (!(rowSectionTemplate.IsidfsParentSectionNull()))
            {
                //����������� ������������ ������
                //������� ���������� ������, ��� ������ ������ ��������
                //���� ����� �, �� ������� ���� �������
                //���� �� ����� �, �� �������� �
                parentSection = FindParentSection(rowSectionTemplate.idfsFormTemplate, rowSectionTemplate.idfsParentSection, mouseLocation);
            }
            //������ ������

            var sectionTable = section as SectionTable;
            //������, ��� ��������� ������ ��������� � ���� ������
            section.Tag = rowSectionTemplate;
            //������ �������� ��� ��������� ������
            if (sectionTable != null)
            {
                //������� �������� ��������� �� �������� ������
                sectionTable.MainDbService = MainDbService;
                //���������, ����� �� � ��� ������� �������� ����� ������ (�� ��������� �����)
                if (rowSectionTemplate.blnFixedRowSet)
                    sectionTable.GridViewMain.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
                sectionTable.GridViewMain.MouseDown += OnSectionTableGridViewMainMouseDown;
                //����� �� ������������� �������
                sectionTable.IsFixedStubSection = CriticalObjects.Sections.ContainsKey(rowSectionTemplate.idfsSection);
            }
            //����� �������� ������
            section.Name = String.Format("section{0}", rowSectionTemplate.idfsSection);

            section.Caption = rowSectionTemplate.NationalName;
            section.Visible = true;
            //�� ������ ���� ������ �������, ����� ����� ���� ���-�� � �� ��������
            section.TopControl().MouseDown += OnTemplateOnMouseDown;
            section.OnUnSelect += OnParameterHostOnUnSelect;
            //
            section.ParentSection = parentSection;

            //�������� �� ���� � ������������ ������, ���� ����� �� ����-�������
            if (parentSection != null)
            {
                //�� ����� ���� ��������, ����� � ��������� ������ ����������� �������
                //��������, ����� � ������� ������ ����������� ���������, ��������
                if (!((parentSection is SectionTable) && (!(section is SectionTable))))
                    parentSection.Add(section);
            }
            else
            {
                //��� ������ -- ��������
                TemplateDesignerHost.Add(section);
            }

            if (sectionTable != null)
            {
                sectionTable.Init(); //
                sectionTable.RootSection = sectionTable;
                //������� � ������ �������� �������
                if (!SectionAlreadyAdded(rowSectionTemplate.idfsSection))
                    SectionTableList.Add(rowSectionTemplate.idfsSection, sectionTable);
                //������������� �� ������� ��������� �������� � ������ �������
                sectionTable.EditValueChanging += OnEditValueChanging;
            }
            else
            {
                //������� � ������ �������� �������
                if (!SectionAlreadyAdded(rowSectionTemplate.idfsSection))
                    SectionList.Add(rowSectionTemplate.idfsSection, section);
            }

            //�������� ���������� �������
            section.SetBestSize();

            //�������� �� ������
            rowSectionTemplate.intLeft = section.Left;
            rowSectionTemplate.intTop = section.Top;
            rowSectionTemplate.intWidth = section.Width;
            rowSectionTemplate.intHeight = section.Height;

            //���� ��� ��������� ������, �� �������� ���������� � ��������
            //(���������� ��������)
            if (sectionTable != null)
            {
                //��������� ����������� ���������� ��������� ������ � ���������� ������� ������ (� ��� ������ �.�. ����� ���������� ����������� ���������� ������ ������)
                RestoreSectionTableChildren(sectionTable, rowSectionTemplate.idfsFormTemplate, isSummaryTemplate);
                //����������� ������������� ��������� ������
                sectionTable.FinishInit();
            }

            section.LabelControlCaption.Visible = SectionTableCaptionsIsVisible;

            section.NowLoading = false;

            return section;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSectionTableGridViewMainMouseDown(object sender, MouseEventArgs e)
        {
            var gv = sender as AdvBandedGridView;
            if (gv == null) return;
            BaseHitInfo hit = gv.CalcHitInfo(e.Location);
            var bhit = hit as BandedGridHitInfo;
            if (bhit == null) return;
            var selectedBand = (bhit.Band != null) && (bhit.InBandPanel) ? bhit.Band : null;
            if (OnSelectedBandChanged != null) OnSelectedBandChanged(selectedBand);
        }

        private bool m_SectionTableCaptionsIsVisible;

        /// <summary>
        /// ���������� �� ��������� � ��������� ������ ������������ �������
        /// </summary>
        internal bool SectionTableCaptionsIsVisible
        {
            get { return m_SectionTableCaptionsIsVisible; }
            set{
                m_SectionTableCaptionsIsVisible = value;
                if (SectionTableList != null)
                {
                    foreach (var sectionTable in SectionTableList)
                    {
                        sectionTable.Value.LabelControlCaption.Visible = m_SectionTableCaptionsIsVisible;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnParameterHostOnUnSelect(ParameterHost parameterHost)
        {
            if (ParameterHostUnSelect != null) ParameterHostUnSelect(parameterHost);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTemplateOnMouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateMouseDown != null) TemplateMouseDown(sender, e);
        }

        /// <summary>
        /// ���������� ������� ������ �������� ��������� ������ � ���������� � ��������� ������
        /// (���������� ����� ������ ������� �����������)
        /// </summary>
        /// <param name="sectionTable"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="isSummaryTemplate"></param>
        private void RestoreSectionTableChildren(SectionTable sectionTable, long idfsFormTemplate, bool isSummaryTemplate)
        {
            //�������� �� ������, ������� ��������������� ��������� � ������ ������
            //����� ���� ���������, � ����� �������� �. � �. �� �������
            var sectionTemplateRows =
                (FlexibleFormsDS.SectionTemplateRow[])
                MainDbService.MainDataSet.SectionTemplate.Select(
                    DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid",
                                      String.Format("'{0}'", LanguageTemplate), "idfsParentSection",
                                      sectionTable.IdfsSection), "[intOrder] ASC");
            var parameterTemplateRows =
                (FlexibleFormsDS.ParameterTemplateRow[])
                MainDbService.MainDataSet.ParameterTemplate.Select(
                    DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid",
                                      String.Format("'{0}'", LanguageTemplate), "idfsSection", sectionTable.IdfsSection),
                    "[intOrder] ASC");
            var columns = new SortedDictionary<int, DataRow>();
            foreach (var sectionTemplateRow in sectionTemplateRows)
            {
                if (columns.ContainsKey(sectionTemplateRow.intOrder)) continue;
                columns.Add(sectionTemplateRow.intOrder, sectionTemplateRow);
            }
            foreach (var parameterTemplateRow in parameterTemplateRows)
            {
                if (columns.ContainsKey(parameterTemplateRow.intOrder)) continue;
                columns.Add(parameterTemplateRow.intOrder, parameterTemplateRow);
            }

            //������ ��� ������� ������� ������. 
            foreach (var dataRow in columns.Values)
            {
                if (dataRow is FlexibleFormsDS.SectionTemplateRow)
                {
                    //��� ��������� ������ ���������� ��������� ���������� �� ������� ������ (������ CreateSectionInTemplateHost)
                    CreateSectionInTemplateHost((FlexibleFormsDS.SectionTemplateRow)dataRow, new Point(0, 0));
                }
                if (dataRow is FlexibleFormsDS.ParameterTemplateRow)
                {
                    CreateParameterInTemplateHost((FlexibleFormsDS.ParameterTemplateRow)dataRow, new Point(0, 0), isSummaryTemplate);
                }
            }
        }

        /// <summary>
        /// ��������� �������� �� ����-�������
        /// </summary>
        /// <param name="rowParameterTemplate"></param>
        /// <param name="mouseLocation"></param>
        /// <param name="isSummaryTemplate"></param>
        /// <returns></returns>
        internal Parameter CreateParameterInTemplateHost(FlexibleFormsDS.ParameterTemplateRow rowParameterTemplate, Point mouseLocation, bool isSummaryTemplate)
        {
            //��������, ���� ��� ������ ��� ��������� �� ������� ����, �� �������
            if (ParameterAlreadyAdded(rowParameterTemplate.idfsParameter)) return null;

            //���� � ������ ������ ���������� Summary-������, �� ���� ��������� ������ �������� ���������, �� �������� � �������
            if (isSummaryTemplate && !MainDbService.IsParameterNumeric(rowParameterTemplate.idfsParameter) && (rowParameterTemplate.intOrder >= 0)) return null;
            
            var parameter = new Parameter
                                      {
                                          NowLoading = true,
                                          ParentRender = this,
                                          MainDbService = MainDbService,
                                          Tag = rowParameterTemplate,
                                          Scheme = ParameterSchemeHelper.ConvertToParameterScheme(rowParameterTemplate.intScheme),
                                          Editor = ParameterControlTypeHelper.ConvertToParameterControlType(rowParameterTemplate.idfsEditor),
                                          Text = rowParameterTemplate.NationalName,
                                          ParameterList = ParameterList,
                                          Visible = true,
                                          ReadOnly = ReadOnly,
                                          Location = mouseLocation,
                                          Size = new Size(rowParameterTemplate.intWidth, rowParameterTemplate.intHeight),
                                          LabelSize = rowParameterTemplate.intLabelSize
                                      };
            //parameter.IsDesignMode = false;
            //
            parameter.OnSelect += OnParameterOnSelect;
            parameter.SizeChanged += OnParameterOnResizeChanged;
            parameter.m_Splitter.SplitterMoved += OnParameterOnResizeChanged;
            parameter.EditValueChanged += OnEditValueChanged;
            parameter.EditValueChanging += OnEditValueChanging;
            
            parameter.OnUnSelect += OnParameterHostOnUnSelect;
            //�������� ����� ���������� ���� � ����� ������, ���� ����� �� ����-��������, ���� � ���� ��� ������,
            //� �� ���������� ����� � ���� �����
            Section parentSection = null;
            if (!rowParameterTemplate.IsidfsSectionNull() && !rowParameterTemplate.blnDynamicParameter)
            {
                //������� ���������� ������, ��� ������ ������ ��������
                //���� ����� �, �� ������� ���� �������
                //���� �� ����� �, �� �������� �
                parentSection = FindParentSection(rowParameterTemplate.idfsFormTemplate, rowParameterTemplate.idfsSection, mouseLocation);
            }
            //�������� �� ���� � ������������ ������, ���� ����� �� ����-�������
            if (!rowParameterTemplate.blnDynamicParameter)
            {
                if (parentSection != null)
                {
                    if (parentSection is SectionTable)
                    {
                        parameter.RootSection = ((SectionTable)parentSection).RootSection;
                    }
                    parentSection.Add(parameter);
                    //parameter.Backcolor = parentSection.BackColor;
                }
                else
                {
                    //�������� �� ����� ������������� � ����� ������
                    //���� � ���� ��� ����� ������, �� �� ������ ���� �� ����-��������
                    TemplateDesignerHost.Add(parameter);
                    //parameter.Backcolor = TemplateDesignerHost.BackColor;
                }
            }
            else
            {
                //������ �������� �� ��������� ������������ �������                
                var parametersRow = MainDbService.GetParameterRow(rowParameterTemplate.idfsParameter);
                if (parametersRow != null) parameter.Text = parametersRow.NationalLongName;
                DynamicParametersGroupControl.Controls.Add(parameter);
                parameter.IsDesignMode = TemplateDesignerHost.IsDesignMode;
            }

            //������� � ������ �������� �������
            //�������� ����� ���� �������� � ������, ���� ��� ������ ���������, � ��� ����������� ����������� ���������� ��������
            if (!ParameterAlreadyAdded(rowParameterTemplate.idfsParameter))
                ParameterList.Add(rowParameterTemplate.idfsParameter, parameter);
            //�������� ���������� �������
            parameter.SetBestSize();

            //�������� �� ������
            rowParameterTemplate.intLeft = parameter.Left;
            rowParameterTemplate.intTop = parameter.Top;
            rowParameterTemplate.intWidth = parameter.Width;
            rowParameterTemplate.intHeight = parameter.Height;
            rowParameterTemplate.intLabelSize = parameter.LabelSize;

            //���� �������� �� ����� ������������, �� ���������� ��������� ���
            if (parameter.IsMandatoryField())
            {
                if (ParameterIsMandatory != null) 
                    ParameterIsMandatory(parameter.ControlParameter);
            }

            parameter.NowLoading = false;

            return parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnParameterOnResizeChanged(object sender, EventArgs e)
        {
            if (ParameterResize != null) ParameterResize(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnParameterOnSelect(ParameterHost parameterHost)
        {
            if (ParameterSelect != null) ParameterSelect(parameterHost);
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSectionOnResize(object sender, EventArgs e)
        {
            if (SectionResize != null) SectionResize(sender, e);
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="parameterHost"></param>
        private void OnSectionOnSelect(ParameterHost parameterHost)
        {
            if (SectionSelect != null) SectionSelect(parameterHost);
        }

        /// <summary>
        /// ������������� ��������� �� ������� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <param name="isSummary"></param>
        internal void RestoreParameters(long idfsFormTemplate, string langid, bool isSummary)
        {
            var parameterTemplateRows = MainDbService.GetParameterTemplateRows(null, idfsFormTemplate, langid);
            //���� �� ������� �������� �������� �������, �� ����� ��������������� ���������� � ������� �����
            if (parameterTemplateRows.Length == 0)
                parameterTemplateRows = MainDbService.GetParameterTemplateRows(null, idfsFormTemplate);
            if (parameterTemplateRows.Length == 0) return;
            foreach (var parameterTemplateRow in parameterTemplateRows)
            {
                //������������ ��������� ���������� ��������
                if (parameterTemplateRow.blnDynamicParameter) continue;
                CreateParameterInTemplateHost(parameterTemplateRow, new Point(parameterTemplateRow.intLeft, parameterTemplateRow.intTop), isSummary);
            }
        }

        /// <summary>
        /// ������������� ����� �� ������� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        internal void RestoreLines(long? idfsFormTemplate, string langid)
        {
            //� ������� �� ���������� � ������, ����� ������ ��������� � �����
            var linesRows = MainDbService.MainDataSet.Lines.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid)));
            if (linesRows.Length == 0) return;
            foreach (FlexibleFormsDS.LinesRow row in linesRows)
            {
                CreateLineInTemplateHost(row);
            }
        }

        /// <summary>
        /// ������ ����� �� ������� ����� �������
        /// </summary>
        /// <param name="linesRow"></param>
        internal Line CreateLineInTemplateHost(FlexibleFormsDS.LinesRow linesRow)
        {
            //������ �����
            var line = new Line
                           {
                               NowLoading = true,
                               MainDbService = MainDbService,
                               ParentRender = this,
                               Tag = linesRow,
                               Location = new Point(linesRow.intLeft, linesRow.intTop),
                               Size = new Size(linesRow.intWidth, linesRow.intHeight),
                               BackColor = Color.FromArgb(linesRow.intColor)
                           };
            //����� �������� �����
            line.OnSelect += OnLineOnSelect;
            line.SizeChanged += OnLineOnSizeChanged;
            line.OnUnSelect += OnParameterHostOnUnSelect;
            line.Visible = true;

            Section parentSection = null;
            if (!(linesRow.IsidfsSectionNull()))
            {
                //����������� ������������ ������
                //���� ����� �, �� ������� ���� �������
                //���� �� ����� �, �� �������� �
                parentSection = FindParentSection(linesRow.idfsFormTemplate, linesRow.idfsSection, line.Location);
            }

            //�������� �� ���� � ������������ ������, ���� ����� �� ����-�������
            Control parent;
            if (parentSection != null)
            {
                parentSection.Add(line);
                parent = parentSection;
            }
            else
            {
                TemplateDesignerHost.Add(line);
                parent = TemplateDesignerHost;
            }

            //��� ������� ����� ������������, ����� ������������ ������� ����� ��� ��������
            parent.SizeChanged += OnLineParentSizeChanged;

            //������� � ��������� �������� �������
            LineList.Add(linesRow.idfDecorElement, line);

            //�������� ���������� �������
            line.SetBestSize();
            line.LookAndFeel.Style = LookAndFeelStyle.Flat;
            line.LookAndFeel.UseDefaultLookAndFeel = false;

            //���� ����� -- ��������� ��� ������ ������������� ����������
            line.ResizeLine();

            line.NowLoading = false;
            return line;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLineParentSizeChanged(object sender, EventArgs e)
        {
            //��� ��������� �������� �������� ����� ����� ���������� ��� �����, ������� ��������� � ����� ��������, � ����� ��������
            foreach (long idfDecorElement in LineList.Keys)
            {
                var line = LineList[idfDecorElement];
                if (line.Parent == null) continue;
                //���� ����� ����������� �� ��������, �� ����� ������������� �� �������
                //��������� ����� ���� ���� ��������-���� ��� ������
                if (line.Parent == TemplateDesignerHost)
                {
                    line.ResizeLine(TemplateDesignerHost);
                }
                else
                {
                    //TODO �.�. ������� ��������������� ����� ��� ��������� ��������� ��������-������
                    if ((line.Parent.Parent != null) && (line.Parent.Parent.Parent == sender))
                    {
                        line.ResizeLine();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLineOnSizeChanged(object sender, EventArgs e)
        {
            if (LineResize != null) LineResize(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnLineOnSelect(ParameterHost parameterHost)
        {
            if (LineSelect != null) LineSelect(parameterHost);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterHost"></param>
        void OnLabelOnSelect(ParameterHost parameterHost)
        {
            if (LabelSelect != null) LabelSelect(parameterHost);
        }

        /// <summary>
        /// ������������� ������ �� ������� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        internal void RestoreLabels(long idfsFormTemplate, string langid)
        {
            //� ������� �� ���������� � ������, ������ ������ ��������� � �����
            var lablesRows = MainDbService.GetLabelTemplateRows(idfsFormTemplate, langid);
            if (lablesRows.Length == 0) return;
            foreach (var row in lablesRows)
            {
                CreateLabelInTemplateHost(row);
            }
        }

        /// <summary>
        /// ������ ����� �� ������� ����� �������
        /// </summary>
        /// <param name="labelsRow"></param>
        internal Label CreateLabelInTemplateHost(FlexibleFormsDS.LabelsRow labelsRow)
        {
            //������ �����
            var label = new Label
                            {
                                NowLoading = true,
                                MainDbService = MainDbService,
                                ParentRender = this,
                                Tag = labelsRow,
                                MinimumSize = new Size(20, 10),
                                Location = new Point(labelsRow.intLeft, labelsRow.intTop),
                                Size = new Size(labelsRow.intWidth, labelsRow.intHeight),
                                mainLabel = {Text = labelsRow.NationalText}
                            };
            //����� ��������
            label.mainLabel.Font = new Font(label.mainLabel.Font.FontFamily, labelsRow.intFontSize,
                                            HelperFunctions.ConvertToFontStyle(labelsRow.intFontStyle));
            label.mainLabel.ForeColor = Color.FromArgb(labelsRow.intColor);
            label.OnSelect += OnLabelOnSelect;
            label.SizeChanged += OnLabelSizeChanged;
            label.OnUnSelect += OnParameterHostOnUnSelect;
            label.Visible = true;

            Section parentSection = null;
            if (!(labelsRow.IsidfsSectionNull()))
            {
                //����������� ������������ ������
                //���� ����� �, �� ������� ���� �������
                //���� �� ����� �, �� �������� �
                parentSection = FindParentSection(labelsRow.idfsFormTemplate, labelsRow.idfsSection, label.Location);
            }

            //�������� �� ���� � ������������ ������, ���� ����� �� ����-�������
            if (parentSection != null)
            {
                parentSection.Add(label);
            }
            else
            {
                TemplateDesignerHost.Add(label);
            }

            //�������� ���������� �������
            label.SetBestSize();
            label.NowLoading = false;

            //������� � ��������� �������� �������
            if (!LabelList.ContainsKey(labelsRow.idfDecorElement))
                LabelList.Add(labelsRow.idfDecorElement, label);

            return label;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLabelSizeChanged(object sender, EventArgs e)
        {
            if (LabelResize != null) LabelResize(sender, e);
        }

        public static long idfObservationFake { get { return -200; } }

        /// <summary>
        /// ������������ ������� �������� � ���������, ������� ��������� � ��������� observation
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="formType"></param>
        /// <param name="idfsFormTemplateMerged">ID �������, � ������� ����� ��������� ���������� ������</param>
        internal bool MergeTemplates(long idfsFormTemplateMerged, List<long> observations, FFType formType)
        {
            var isEmptySummary = observations.Count == 0;

            var idfsFormType = (long)formType;

            //���� �� ������� ���� �����, � �������� ��������� ������, �� ��������� ����-������ �� �������, �� ���������� ��
            //(�������� ��� ������)
            MainDbService.LoadSections(idfsFormType, null, null);
            MainDbService.LoadParameters(idfsFormType, null);
           
            //������� observation ����� ��������������� ���� ������, ������ ���� ��������� �� ���
            var observationsRows = new List<FlexibleFormsDS.ObservationsRow>();
            if (!isEmptySummary)
            {
                observationsRows.AddRange(MainDbService.LoadObservations(observations));
            }
            else
            {
                var row = MainDbService.MainDataSet.Observations.NewObservationsRow();
                long? idfsFormTemplateActual;
                MainDbService.LoadActualTemplate(EidssSiteContext.Instance.CountryID, null, idfsFormType, out idfsFormTemplateActual);
                if (!idfsFormTemplateActual.HasValue) return false;
                row.idfsFormTemplate = idfsFormTemplateActual.Value;
                row.idfObservation = idfObservationFake;
                observationsRows.Add(row);
            }
            
            //���� �� �� ������ �� ������������
            if (observationsRows.Count == 0) return false;

            //������� ��������� ��������� �������
            DataHelper.DeleteRows(MainDbService.MainDataSet.ParameterTemplate, idfsFormTemplateMerged, ModelUserContext.CurrentLanguage);
            DataHelper.DeleteRows(MainDbService.MainDataSet.SectionTemplate, idfsFormTemplateMerged, ModelUserContext.CurrentLanguage);

            #region �������� ������ �� ���� �������

            //����� ������� ����, ������ ��� ������� ��������� ������� ������ ��������� ������ � �������
            foreach (var observationsRow in observationsRows)
            {
                if (observationsRow.IsidfsFormTemplateNull()) continue;
                //��������� ������ �� ������� ������� (���� ��� �� ��� ��� �� ���������)
                MainDbService.LoadSectionTemplate(observationsRow.idfsFormTemplate);
                MainDbService.LoadParameterTemplate(observationsRow.idfsFormTemplate);
            }

            #endregion

            //������� ������ �����, ������ ��� ������� ������� �������� �������� �������� � � ���� �������

            #region ������� ��������� �� ����� �������

            //������� ������ ����������� �������
            ClearTemplate();

            //�������� ������ � �������-������
            foreach (var observationsRow in observationsRows)
            {
                if (observationsRow.IsidfsFormTemplateNull()) continue;

                //����� ������ � �������
                CopyHelper.CopySectionTemplate(MainDbService, observationsRow.idfsFormTemplate, idfsFormTemplateMerged, ModelUserContext.CurrentLanguage);

                //����� ���������� � �������
                CopyHelper.CopyParameterTemplate(MainDbService, observationsRow.idfsFormTemplate, idfsFormTemplateMerged, ModelUserContext.CurrentLanguage);
            }

            #endregion

            #region ������� ��������� �� ��������

            var listStubInfo = MainDbService.GetListSectionsAndVersionsForStub(observationsRows, formType);
            //�������� ���������� � ������ ������� ��������
            //��������� ������ �� �������� � ����� ��������� �� � ������
            var stubLength = MainDbService.IncludeStub(idfsFormTemplateMerged, listStubInfo);

            #endregion
          
            //���� ������ �������� ���� �������, � �� ������ �������, �� ������� ���
            var finded = false;
            var pts = MainDbService.GetParameterTemplateRows(null, idfsFormTemplateMerged, ModelUserContext.CurrentLanguage);
            foreach (var parameterTemplate in pts)
            {
                if (parameterTemplate.intOrder >= 0)
                {
                    finded = true;
                    break;
                }
            }

            if (finded)
            {
                //������� ��������� ��� ������, �������� �� �����������
                RestoreSections(idfsFormTemplateMerged, ModelUserContext.CurrentLanguage, true);
                //��������� ��� ��������� �� �� ���������
                RestoreParameters(idfsFormTemplateMerged, ModelUserContext.CurrentLanguage, true);

                //������������� ��������
                listStubInfo.PostProcessingStub(SectionTableList, stubLength);
            }
            return finded;
        }

        /// <summary>
        /// ���������� �� ������ ��������/������������� �������
        /// </summary>
        public bool NowLoadingTemplate { get; set; }

        /// <summary>
        /// ���������� �� ������ ������� �������
        /// </summary>
        public bool NowClearingTemplate { get; set; }

        public CriticalObjectsHelper CriticalObjects { get; set; }
    }
}
