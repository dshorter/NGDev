using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using bv.common.Core;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.Helpers.ReportHelper.DataItems;
using EIDSS.FlexibleForms.Helpers.ReportHelper.Tree;
using eidss.model.Core;
using eidss.model.Resources;

namespace EIDSS.FlexibleForms.Helpers
{
    public class TemplateHelper
    {
        private DbServiceUserData MainDbService { get; set; }
        public FlexibleFormsDS.TemplatesRow ActualTemplate { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public TemplateHelper()
        {
            MainDbService = new DbServiceUserData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbService"></param>
        internal TemplateHelper(DbServiceUserData dbService)
        {
            MainDbService = dbService;
        }

        /// <summary>
        /// ID ��������� �������
        /// </summary>
        public long? IdfsFormTemplate { get; private set; }

        /// <summary>
        /// �������� �� �������� ������ UNI-��������
        /// </summary>
        public bool? IsUNITemplate { get; private set; }

        /// <summary>
        /// ID ���������� Observation, ������� �������� ������������� ������
        /// </summary>
        public long IdfObservationSummary
        {
            get { return MainDbService.IdfObservationSummary; }
        }

        /// <summary>
        /// ID ���������� Form Template, ������� �������� ������ (merged) ��������� �� ���� Observation, ������� ������ � ������� �����
        /// </summary>
        public long IdfsFormTemplateSummary
        {
            get { return MainDbService.IdfsFormTemplateSummary; }
        }

        /// <summary>
        /// idfVersion �������, ������� ������������ � ������ ������. ����������� ������ ��� Aggregate Case.
        /// </summary>
        public long? VersionMatrixStub { get; private set; }

        /// <summary>
        /// ������, �������� � �������� ������
        /// </summary>
        public FlexibleFormsDS.SectionTemplateDataTable SectionTemplate
        {
            get { return MainDbService.MainDataSet.SectionTemplate; }
        }

        /// <summary>
        /// ���������, �������� � �������� ������
        /// </summary>
        public FlexibleFormsDS.ParameterTemplateDataTable ParameterTemplate
        {
            get { return MainDbService.MainDataSet.ParameterTemplate; }
        }

        /// <summary>
        /// ������, �������� � �������� ������
        /// </summary>
        public FlexibleFormsDS.ActivityParametersDataTable ActivityParameters
        {
            get { return MainDbService.MainDataSet.ActivityParameters; }
        }

        /// <summary>
        /// ������������ ������� �������� � ���������, ������� ��������� � ��������� observation
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="formType"></param>
        /// <param name="idfsFormTemplateMerged">ID �������, � ������� ����� ��������� ���������� ������</param>
        private void MergeTemplates(long idfsFormTemplateMerged, IEnumerable<long> observations, FFType formType)
        {
            var idfsFormType = (long)formType;

            //���� �� ������� ���� �����, � �������� ��������� ������, �� ��������� ����-������ �� �������, �� ���������� ��
            //(�������� ��� ������)
            MainDbService.LoadSections(idfsFormType, null, null);
            MainDbService.LoadParameters(idfsFormType, null);

            //������� observation ����� ��������������� ���� ������, ������ ���� ��������� �� ���
            var observationsRows = MainDbService.LoadObservations(observations);
            if (observationsRows.Length == 0) return;

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
            MainDbService.DeleteSummaryTemplate();

            //�������� ������ � �������-������
            foreach (var observationsRow in observationsRows)
            {
                if (observationsRow.IsidfsFormTemplateNull()) continue;

                //����� ������ � �������
                CopyHelper.CopySectionTemplate(MainDbService, observationsRow.idfsFormTemplate, idfsFormTemplateMerged, bv.model.Model.Core.ModelUserContext.CurrentLanguage);

                //����� ���������� � �������
                CopyHelper.CopyParameterTemplate(MainDbService, observationsRow.idfsFormTemplate, idfsFormTemplateMerged, bv.model.Model.Core.ModelUserContext.CurrentLanguage);
            }

            #endregion

            #region ������� ��������� �� ��������

            var listStubInfo = MainDbService.GetListSectionsAndVersionsForStub(observationsRows, formType);
            //�������� ���������� � ������ ������� ��������
            //��������� ������ �� �������� � ����� ��������� �� � ������
            MainDbService.IncludeStub(idfsFormTemplateMerged, listStubInfo);

            #endregion
        }

        /// <summary>
        /// ���������� ���������� ������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="formType"></param>
        /// <param name="idfObservation"></param>
        private void SetActualTemplate(long idfsFormTemplate, long formType, long idfObservation)
        {
            //����������: ���������� -> �� observation -> �� �������������
            IsUNITemplate = false;

            if (idfsFormTemplate > 0)
            {
                IdfsFormTemplate = idfsFormTemplate;
            }
            else if (idfObservation > 0)
            {
                //�������� idfsFormTemplate �� ������� Observations
                var observationsRow = MainDbService.LoadObservations(idfObservation);
                if ((observationsRow != null) && (!observationsRow.IsidfsFormTemplateNull()))
                    IdfsFormTemplate = observationsRow.idfsFormTemplate;
            }
            else
            {
                MainDbService.LoadActualTemplate(EidssSiteContext.Instance.CountryID, null, formType);
                if (MainDbService.MainDataSet.ActualTemplate.Rows.Count != 1) return;
                var row = (FlexibleFormsDS.ActualTemplateRow)MainDbService.MainDataSet.ActualTemplate.Rows[0];
                IdfsFormTemplate = row.idfsFormTemplate;
                IsUNITemplate = row.IsUNITemplate;
            }

            if (IdfsFormTemplate > 0)
            {
            //������� ���������� �� ����� �������
                MainDbService.LoadTemplates(formType);
                var templatesRows = (FlexibleFormsDS.TemplatesRow[])MainDbService.MainDataSet.Templates.Select(DataHelper.Filter("idfsFormTemplate", IdfsFormTemplate));
            if (templatesRows.Length == 1)
            {
                    ActualTemplate = templatesRows[0];
                    IsUNITemplate = ActualTemplate.blnUNI;
            }
        }
        }

        /// <summary>
        /// ��������� ����������� �������, �������������� �� ���������
        /// </summary>
        public List<DataRow> GetReportView()
        {
            return IdfsFormTemplate.HasValue ? GetReportView(MainDbService, IdfsFormTemplate.Value) : new List<DataRow>();
        }

        /// <summary>
        /// ��������� ����������� �������, �������������� �� ���������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static List<DataRow> GetReportView(DbService mainDbService, long templateID)
        {
            var result = new List<DataRow>();

            string filter = DataHelper.Filter("idfsFormTemplate", templateID);

            //��������� �������� ���� TopAbsolute � ���� ������, ����������, ������� � �����
            CalculateTopAbsolute(mainDbService, mainDbService.MainDataSet.SectionTemplate.Select(filter), "idfsParentSection", result);
            CalculateTopAbsolute(mainDbService, mainDbService.MainDataSet.ParameterTemplate.Select(filter), "idfsSection", result);
            CalculateTopAbsolute(mainDbService, mainDbService.MainDataSet.Labels.Select(filter), "idfsSection", result);
            CalculateTopAbsolute(mainDbService, mainDbService.MainDataSet.Lines.Select(filter), "idfsSection", result);

            //��������� �� ����������� ���� TopAbsolute
            result.Sort(CompareParameterHostDataRows);

            return result;
        }

        /// <summary>
        /// ��������� �������� 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="rows"></param>
        /// <param name="parentSectionColumnName"></param>
        /// <param name="resultList"></param>
        private static void CalculateTopAbsolute(DbService mainDbService, IEnumerable<DataRow> rows, string parentSectionColumnName, List<DataRow> resultList)
        {
            foreach (var row in rows)
            {
                int intTop = Convert.ToInt32(row["intTop"]);
                int intAdditionalTop = 0;
                if (!Utils.IsEmpty(row[parentSectionColumnName]))
                {
                    long idfsSection = Convert.ToInt64(row[parentSectionColumnName]);
                    long idfsformTemplate = Convert.ToInt64(row["idfsFormTemplate"]);
                    intAdditionalTop = GetTopParentSection(mainDbService, idfsSection, idfsformTemplate);
                }
                row[TopAbsoluteColumnName] = intTop + intAdditionalTop;
                resultList.Add(row);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <returns></returns>
        private static int CompareParameterHostDataRows(DataRow row1, DataRow row2)
        {
            return (Convert.ToInt32(row1[TopAbsoluteColumnName]) - Convert.ToInt32(row2[TopAbsoluteColumnName]));
        }

        private const string TopAbsoluteColumnName = "TopAbsolute";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsformTemplate"></param>
        /// <returns></returns>
        private static int GetTopParentSection(DbService mainDbService, long idfsSection, long idfsformTemplate)
        {
            int result = 0;
            var sectionTemplateRow = mainDbService.GetSectionTemplateRow(idfsSection, idfsformTemplate);
            if (sectionTemplateRow != null)
            {
                result = sectionTemplateRow.intTop;
                if (!sectionTemplateRow.IsidfsParentSectionNull())
                {
                    result += GetTopParentSection(mainDbService, sectionTemplateRow.idfsParentSection, sectionTemplateRow.idfsFormTemplate);
                }
            }
            return result;
        }

        /// <summary>
        /// ������������ ������� ������, �������� ��������� ������ ������������������ ����������, �� ������� ������
        /// </summary>
        /// <param name="observations">��������� ID ��������� (observation)</param>
        /// <param name="formType">ID ���� �����</param>
        public void LoadSummary(List<long> observations, FFType formType)
        {
            if (observations == null) return;

            IdfsFormTemplate = IdfsFormTemplateSummary;
            //�������� ��� ��������, ������ ��� � ������� ��������� ��������� Observation � � ������� ����� ���� ���� ������ �������� � �������
            VersionMatrixStub = null;
            var idfsFormType = (long) formType;
            //������� ���������� �� ����� �������
            SetActualTemplate(0, idfsFormType, 0);

            //��������� ���������
            MergeTemplates(MainDbService.IdfsFormTemplateSummary, observations, formType);
            var observationsRows = MainDbService.LoadObservations(observations);
            
            //��������� ������
            //�������� ���������������� ������
            foreach (var observationsRow in observationsRows)
            {
                if (observationsRow.IsidfsFormTemplateNull()) continue;
                MainDbService.LoadUserData(observationsRow.idfObservation, observationsRow.idfsFormTemplate, true);
            }

            //���� ���������� ����� ������ � �������� � � ��������� ObservationSummary, � ����� ���������� ���
            //����������� ����� ������ �� ���������, � ������� ��� Numeric. ��������� ������������ (������, 11.01.2010).
            //����������� ������ ���� �������, ������� �� �����������
            //� ���������� ������������� ������ �� �����������

            //������ summary
            var lst = new List<long>();
            lst.AddRange(observations);
            MainDbService.CalculateSummary(lst);
            List<FlexibleFormsDS.PredefinedStubRow> predefinedStubRows;

            MainDbService.IncludeStubDataInUserData(IdfsFormTemplate, IdfObservationSummary, null, null, out predefinedStubRows);

            //��������� ��������� �������
            MainDbService.LoadSections(idfsFormType, null, null);
            MainDbService.LoadParameters(idfsFormType, null);
            MainDbService.LoadSectionTemplate(IdfsFormTemplate);
            MainDbService.LoadParameterTemplate(IdfsFormTemplate);
        }

        /// <summary>
        /// ��������� �������������� �������
        /// </summary>
        /// <param name="idFormTemplate"></param>
        /// <param name="observation"></param>
        /// <param name="formType"></param>
        public void LoadAggregateTemplate(long idFormTemplate, long observation, FFType formType)
        {
            LoadTemplate(formType, idFormTemplate, new List<long> {observation});
        }

        /// <summary>
        /// ��������� �������������� �������
        /// </summary>
        /// <param name="observations"></param>
        /// <param name="formType"></param>
        /// <param name="secondDeterminant">������� ��� ��� �����</param>
        public void LoadTemplate(List<long> observations, long? secondDeterminant, FFType formType)
        {
            LoadTemplate(formType, EidssSiteContext.Instance.CountryID, secondDeterminant, observations);
        }

        /// <summary>
        /// ��������� �������������� �������
        /// </summary>
        /// <param name="formType">��� �����</param>
        /// <param name="countryDeterminant">�����������-������</param>
        /// <param name="secondDeterminant">����������� ������� ��� ����</param>
        /// <param name="observationList">�������� observation</param>
        private void LoadTemplate(FFType formType, long countryDeterminant, long? secondDeterminant, List<long> observationList /*,IEnumerable<AggregateCaseSection> presetStubs*/)
        {
            IdfsFormTemplate = null;
            if (observationList.Count > 1)
            {
                MainDbService.LoadActualTemplate(countryDeterminant, secondDeterminant, (long) formType);

                //���� ��� �� ����� ������, �� �������� ������� �� ������� ����� (�� ��������, �� UNI)
                if (MainDbService.MainDataSet.ActualTemplate.Rows.Count == 0) return;
                var actualTemplateRow =
                    (FlexibleFormsDS.ActualTemplateRow) MainDbService.MainDataSet.ActualTemplate.Rows[0];
                IdfsFormTemplate = actualTemplateRow.idfsFormTemplate != -1
                                       ? (long?) actualTemplateRow.idfsFormTemplate
                                       : null;
            }
            else
            {
                var observationsRows = MainDbService.LoadObservations(observationList);
                if (observationsRows.Length > 0)
                {
                    if (!observationsRows[0].IsidfsFormTemplateNull())
                        IdfsFormTemplate = observationsRows[0].idfsFormTemplate;
                }
            }

            if ((IdfsFormTemplate == null) || Utils.IsEmpty(IdfsFormTemplate)) return;

            LoadTemplate(formType, IdfsFormTemplate.Value, observationList/*, presetStubs*/);
        }

        /// <summary>
        /// ��������� �������������� �������
        /// </summary>
        /// <param name="formType">��� �����</param>
        /// <param name="idFormTemplate">������, ������� ��������� ���������</param>
        /// <param name="observationList">�������� observation</param>
        internal void LoadTemplate(FFType formType, long idFormTemplate, List<long> observationList)
        {
            if (observationList == null) return;

            IdfsFormTemplate = idFormTemplate;

            //������� ���������� �� ����� �������
            var idfsFormType = (long) formType;
            SetActualTemplate(idFormTemplate, idfsFormType, observationList.Count == 1 ? observationList[0] : 0);

            //��������� ��������� �������
            MainDbService.LoadSections(idfsFormType, null, null);
            MainDbService.LoadParameters(idfsFormType, null);
            MainDbService.LoadSectionTemplate(IdfsFormTemplate);
            MainDbService.LoadParameterTemplate(IdfsFormTemplate);

            //MainDbService.LoadLines(IdfsFormTemplate);
            MainDbService.LoadLabels(IdfsFormTemplate.Value);

            //���������� �� ������ ������� ������ �����, ����� �������� � ��. ��������
            List<long> presetStubs = null;
            long? idfsSection;
            long? idfsMatrixType;
            MainDbService.GetSectionForAggregateCase(IdfsFormTemplate, idfsFormType, out idfsSection, out idfsMatrixType);
            if (idfsSection.HasValue) presetStubs = new List<long>(1) { idfsSection.Value };

            //��� ���� �������� observation �������� ���������������� ������
            foreach (var idfObservation in observationList)
            {
                MainDbService.LoadUserData(idfObservation, IdfsFormTemplate.Value, true);
                //���� ���� ����������������� �������, �� �������� ���������������� ������ ��� �������
                if (presetStubs == null) continue;
                
                //�������� ���������� � ������ ������� ��������
                var matrixVersionRow = MainDbService.LoadMatrixStubInfo(idfObservation);
                //���� �� ������� ����� ������� (��������, ��� ��� �� ��������), �� ���� � ������ Observation
                if (matrixVersionRow == null) continue;
                if (matrixVersionRow.IsidfVersionNull()) continue;
                if ((matrixVersionRow.idfVersion > 0) && idfsMatrixType.HasValue)
                {
                    //���������� ����������� �������
                    presetStubs.IncludeStub(MainDbService, IdfsFormTemplate.Value, matrixVersionRow.idfVersion, idfsMatrixType.Value);
                }

                var predefinedStubRows = MainDbService.GetPredefinedStubRows(matrixVersionRow.idfVersion);
                foreach (var predefinedStubRow in predefinedStubRows)
                {
                    MainDbService.MainDataSet.ActivityParameters.RenumActivityParameters(predefinedStubRow, idfObservation);
                    MainDbService.ChangeValue(idfObservation, IdfsFormTemplate.Value, predefinedStubRow.idfsParameter, predefinedStubRow.idfRow, predefinedStubRow.NumRow, predefinedStubRow.idfsParameterValue, predefinedStubRow.strNameValue);
                }
            }
        }

        /// <summary>
        /// ��������� ��������� ������� ���������� (������ ��� ������ ���� ��������)
        /// </summary>
        /// <param name="idfObservation"></param>
        /// <returns></returns>
        public FlexNode GetFlexNodeFromTemplate(long idfObservation)
        {
            var rootNode = FlexNode.CreateRoot();
            //�������� ������, ��������� � Observation
            var observationsRow = MainDbService.LoadObservations(idfObservation);
            if ((observationsRow != null) && (!observationsRow.IsidfsFormTemplateNull()))
            {
                //�������� � ����� ������� � ������
                FillFlexNodeForSection(rootNode, null, observationsRow.idfObservation, observationsRow.idfsFormTemplate);
                
                //������� ������ � ������������� �����������
                var parametersTable = MainDbService.LoadParameterDeletedFromTemplate(idfObservation, null, null);
                if (parametersTable.Count > 0)
                {
                    //������� ����������-��������� ��� ������������ ����������
                    var dynamicGroup = rootNode.Add(EidssMessages.Get("DynamicParametersGroupControlCaption"));
                    int heightTotal = 0;
                    int widthTotal = 0;
                    foreach (var rowParameter in parametersTable)
                    {
                        Size size;
                        //������� �������� ��� ����� ���������, ��������� ��� ��������
                        dynamicGroup.Add(rowParameter, MainDbService.GetActivityParametersRow(idfObservation, rowParameter.idfsParameter, null), out size);
                        heightTotal += size.Height;
                        if (widthTotal < size.Width) widthTotal = size.Width;
                    }
                    //��������� ��� ���������� ������� � �������
                    if (heightTotal * widthTotal > 0)
                    {
                        //dynamicGroup.DataItem.Width = widthTotal + 30;
                        dynamicGroup.DataItem.Width = widthTotal;
                        dynamicGroup.DataItem.Height = heightTotal + 30;

                        //������������ ������������ ���� � ����������, ����� �� ������� �� ������� �����
                        if (rootNode.Count > 1)
                        {
                            var staticItemWidth = rootNode[0].DataItem.Width;
                            if (dynamicGroup.DataItem.Width > staticItemWidth)
                                dynamicGroup.DataItem.Width = staticItemWidth;
                            //���������� ������������ � ������������ ���������
                            foreach (var rootNodes in rootNode.ChildList)
                            {
                                if (rootNodes.ChildListCount == 0) continue;
                                foreach (var item in rootNodes.ChildList)
                                {
                                    var paramWidth = item.DataItem.Width;
                                    foreach (var child in dynamicGroup.ChildList)
                                    {
                                        if (child.DataItem.Width > paramWidth) child.DataItem.Width = paramWidth;
                                    }
                                    break;
                                }
                                break;
                            }
                        }
                    }
                }
                //��� ������ ��������� ������ �������� ������, �������� � ������, �������� ������� � �������
                AddDataTable(rootNode, idfObservation);
            }
            return rootNode;
        }

        /// <summary>
        /// ���������� ������ � Summary � ����������� ����� (������ ��� ������ ���� ��������)
        /// </summary>
        /// <returns></returns>
        public FlexNode GetFlexNodeFromTemplateSummary()
        {
            var rootNode = FlexNode.CreateRoot();
            FillFlexNodeForSection(rootNode, null, IdfObservationSummary, IdfsFormTemplateSummary); //�������� � ����� ������� � ������
            //��� ������ ��������� ������ �������� ������, �������� � ������, �������� ������� � �������
            AddDataTable(rootNode, IdfObservationSummary);

            return rootNode;
        }

        /// <summary>
        /// �������� ���������� ��� ����������� ���������, ������� ��� ������������� �� ������� ����������
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="parameters"></param>
        private void CollectParametersUnderSection(FlexNode parentNode, List<FlexibleFormsDS.ParameterTemplateRow> parameters)
        {
            var parameterTemplateRow = parentNode.DataItem.LinkedObject as FlexibleFormsDS.ParameterTemplateRow;
            if (parameterTemplateRow != null)
            {
                parameters.Add(parameterTemplateRow);
            }
            //���������� �� ���� ��������
            foreach (var childNode in parentNode.ChildList)
            {
                CollectParametersUnderSection(childNode, parameters);
            }
        }

        /// <summary>
        /// �������� ������� ��� ������� � TemplateHelper
        /// </summary>
        /// <param name="idfObservation"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DataTable GetDataTableForTemplate(long idfObservation, List<FlexibleFormsDS.ParameterTemplateRow> parameters)
        {
            //������ ������ ���� ��������, ������ ��������
            //��������� �������������� ������� �������� ��� ��������� ��� ���������� �������.
            //�������� ������� �������� ��� ��������� idfRow ��� ������� Observation
            //������ ������ ����������� DBNull.Value
            var resultTable = new DataTable();
            var rows = MainDbService.GetActivityParametersRows(idfObservation);
            if ((parameters.Count > 0) && (rows.Length > 0))
            {
                foreach (var parameterRow in parameters)
                {
                    if (!parameterRow.IsRowAlive()) continue;
                    var column = new DataColumn(parameterRow.idfsParameter.ToString(), typeof(string))
                                     {
                                         Caption = parameterRow.NationalName,
                                         AllowDBNull = true,
                                         DefaultValue = DBNull.Value
                                     };
                    resultTable.Columns.Add(column);
                }

                foreach (var activityParametersRow in rows)
                {
                    if (!activityParametersRow.IsRowAlive()) continue;
                    //���� �� ������� ������ � ������� ������, �� ������ �����-�� �������� ��� ��������� � ��������, � ���������� ��� ������
                    if (activityParametersRow.IsintNumRowNull()) continue;
                    //��������� ������� �����, ����� ������� ��� ���������� ����������� ������
                    int cnt = activityParametersRow.intNumRow + 1;
                    if (cnt > resultTable.Rows.Count)
                    {
                        for (int i = resultTable.Rows.Count; i < cnt; i++)
                        {
                            resultTable.Rows.Add(resultTable.NewRow());
                        }
                    }

                    var row = resultTable.Rows[activityParametersRow.intNumRow];

                    foreach (var parameterRow in parameters)
                    {
                        if (!parameterRow.IsRowAlive()) continue;
                        var rowValue = MainDbService.GetActivityParametersRow(idfObservation, parameterRow.idfsParameter, activityParametersRow.idfRow);
                        if (!Utils.IsEmpty(rowValue))
                        {
                            //��������� ���� � ���������� ������
                            if (rowValue.varValue is DateTime)
                            {
                                var destFormat = (CultureInfo.CurrentCulture).DateTimeFormat.ShortDatePattern;
                                rowValue.strNameValue = ((DateTime)rowValue.varValue).ToString(destFormat, CultureInfo.InvariantCulture);
                            }
                            row[parameterRow.idfsParameter.ToString()] = rowValue.strNameValue.Length > 0 ? rowValue.strNameValue : rowValue.varValue;
                        }
                    }
                }
            }


            return resultTable;
        }

        /// <summary>
        /// ������ ���������� ������ �������, ������ �� �����, �������� � ���� ��������
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        private int CalculateWidthTotal(FlexNode parentNode)
        {
            var result = 0;
            if (parentNode.ChildListCount > 0)
            {
                result += parentNode.ChildList.Sum(node => CalculateWidthTotal(node));
                parentNode.DataItem.Width = result;
            }
            else
            {
                result = parentNode.DataItem.Width;
            }
            
            return result;
        }

        /// <summary>
        /// �������� ���������� �� ���� ����� ������, ���������� ��������� ������ �������� ������, ���������� � ��� ������ �� ����������� ����������
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="idfObservation"></param>
        private void AddDataTable(FlexNode parentNode, long idfObservation)
        {
            //���� ���� ������� ��������� ������ � ��� �������� ������ (�.�. ��� ��� ��� ������ ��������� ������), ��
            //������ ��� ����� ����� ���������������� ������
            var flexTableItem = parentNode.DataItem as FlexTableItem;
            if ((flexTableItem != null) && (!(parentNode.ParentNode.DataItem is FlexTableItem)))
            {
                //�������� ���������� ��� ����������� ���������, ������� ��� ������������� �� ������� ����������
                var parameters = new List<FlexibleFormsDS.ParameterTemplateRow>();
                CollectParametersUnderSection(parentNode, parameters);
                flexTableItem.BodyData = GetDataTableForTemplate(idfObservation, parameters);
                //��������� ���������� ������ ���� ������ ��� ������������ ����� �������� � �� ��������
                flexTableItem.Width = CalculateWidthTotal(parentNode);
            }
            //���������� �� ���� ��������
            foreach (var childNode in parentNode.ChildList)
            {
                AddDataTable(childNode, idfObservation);
            }
        }

        /// <summary>
        /// ������ ���� � ���, ��� ����� � ������ � ���������� �� ���� ������� ������ ��. ���� ������ �� �������, �� �������� ��, ��� ����� � ����� �������.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="idfsParentSection"></param>
        /// <param name="idfObservation"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        private void  FillFlexNodeForSection(FlexNode parentNode
                                            , long? idfsParentSection
                                            , long idfObservation
                                            , long idfsFormTemplate
                                            )
        {
            //���������� �������, ������� ������ � ������������ ������
            var sectionTemplateRows = MainDbService.GetSectionTemplateChildrenRows(idfsParentSection, idfsFormTemplate);
            var parameterTemplateRows = MainDbService.GetParameterTemplateChildrenRows(idfsParentSection, idfsFormTemplate);
            var labelsRows = MainDbService.GetLabelsTemplateChildrenRows(idfsParentSection, idfsFormTemplate);
            var linesRows = MainDbService.GetLinesTemplateChildrenRows(idfsParentSection, idfsFormTemplate);
            
            //���������� ������ � ���� ������
            foreach (var sectionTemplateRow in sectionTemplateRows)
            {
                var sectionNode = parentNode.Add(sectionTemplateRow);
                sectionNode.ProcessAsTable = sectionTemplateRow.blnGrid;
                FillFlexNodeForSection(sectionNode, sectionTemplateRow.idfsSection, idfObservation, idfsFormTemplate);
            }

            #region ���������� ���������� � ���� ������
            
            foreach (var parameterTemplateRow in parameterTemplateRows)
            {
                //������������ ���������, �� �������� � �������
                if (parameterTemplateRow.IsidfsSectionNull() || (!parameterTemplateRow.IsidfsSectionNull() && !MainDbService.IsSectionTable(parameterTemplateRow.idfsSection)))
                {
                    //������� �������� ��� ����� ���������, ��������� ��� ��������
                    parentNode.Add(parameterTemplateRow, MainDbService.GetActivityParametersRow(idfObservation, parameterTemplateRow.idfsParameter, null));
                }
                else
                {
                    bool isSummaryTemplate = idfsFormTemplate.Equals(MainDbService.IdfsFormTemplateSummary);
                    if (isSummaryTemplate && !MainDbService.IsParameterNumeric(parameterTemplateRow.idfsParameter) && (parameterTemplateRow.intOrder > 0)) continue;
            
                    //� ��������� ������� ������������ ������� ��������� � ����� ��� ��������
                    parentNode.Add(parameterTemplateRow, true);
                }
            }

            #endregion

            //���������� ������ � ����������
            parentNode.Sort();

            //���������� ������� � ���� ������
            foreach (var labelsRow in labelsRows)
            {
                parentNode.Add(labelsRow);
            }
            //���������� ����� � ���� ������
            foreach (var linesRow in linesRows)
            {
                parentNode.Add(linesRow);
            }
        }


    }
}
