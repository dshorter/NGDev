using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using bv.common;
using bv.common.Core;
using bv.common.db;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.Components;
using System.Linq;
using bv.common.db.Core;

namespace EIDSS.FlexibleForms.Helpers
{
    /// <summary>
    /// ��������������� ������� ��� ������ � �������
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// ��������� ������� ��������� ��������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static bool CheckSectionDependItems(this DbService mainDbService, long idfsSection, long idfsFormTemplate)
        {
            return
                ((mainDbService.GetSectionTemplateChildrenRows(idfsSection, idfsFormTemplate).Length > 0)
                 ||
                 (mainDbService.GetSectionTemplateChildrenRows(idfsSection, idfsFormTemplate).Length > 0)
                 ||
                 (mainDbService.GetSectionTemplateChildrenRows(idfsSection, idfsFormTemplate).Length > 0)
                 ||
                 (mainDbService.GetSectionTemplateChildrenRows(idfsSection, idfsFormTemplate).Length > 0));
        }

        /// <summary>
        /// ��������� ������������� ������������� ��������� �� ���� ��������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfObservation"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsParameter"></param>
        /// <returns>true-���� ��� ���������, false-���� ���-�� �� ��������� (��� ������)</returns>
        public static bool CheckMandatoryParameter(this DbServiceUserData mainDbService, long idfObservation, long idfsFormTemplate, long idfsParameter)
        {
            var activityParametersRows =
                (FlexibleFormsDS.ActivityParametersRow[])mainDbService.MainDataSet.ActivityParameters.Select(Filter("idfObservation", idfObservation,
                                                                           "idfsFormTemplate", idfsFormTemplate,
                                                                           "idfsParameter", idfsParameter));
            //���� �������� � ����� ��������� ��� ��� �� ��� ���� � ������, �� ��� ������
            bool result = activityParametersRows.Length > 0;
            if (result)
            {
                if (activityParametersRows.Any(c => Utils.IsEmpty(c.varValue)))
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// ��������� ���������� ���������������� ������ � ����� ������ ������������ ����������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="observationsDictionary"></param>
        /// <param name="parameterTemplateRow"></param>
        /// <returns></returns>
        public static bool CheckMandatoryParameters(this DbServiceUserData mainDbService, Dictionary<long, long> observationsDictionary, out FlexibleFormsDS.ParameterTemplateRow parameterTemplateRow)
        {
            parameterTemplateRow = null;
            foreach (var observationPair in observationsDictionary)
            {
                var parameterTemplateRows = GetParameterTemplateRows(mainDbService,
                                                                                                        null, observationPair.Value);
                foreach (var pt in parameterTemplateRows)
                {
                    if (!IsMandatoryParameter(mainDbService, pt)) continue;
                    if (!CheckMandatoryParameter(mainDbService, observationPair.Key, pt.idfsFormTemplate, pt.idfsParameter))
                    {
                        parameterTemplateRow = pt;
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static bool IsMandatoryParameter(this DbService mainDbService, FlexibleFormsDS.ParameterTemplateRow pt)
        {
            return pt.idfsEditMode.Equals(((long)FFEditModes.Required).ToString());
        }

        /// <summary>
        /// �������� �� �������� � ����� ������������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsParameter"></param>
        /// <returns></returns>
        public static bool IsMandatoryParameter(this DbService mainDbService, long idfsFormTemplate, long idfsParameter)
        {
            bool result = false;
            var pt = GetParameterTemplateRow(mainDbService, idfsParameter, idfsFormTemplate);
            if (pt != null) result = IsMandatoryParameter(mainDbService, pt);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.DeterminantsRow[] GetDeterminants(this DbService mainDbService, FFDeterminantTypes type)
        {
            return (FlexibleFormsDS.DeterminantsRow[])mainDbService.MainDataSet.Determinants.Select(Filter("idfsReferenceType", (int)type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsBaseReference"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.DeterminantsRow GetDeterminant(this DbService mainDbService, long idfsBaseReference)
        {
            var dets = (FlexibleFormsDS.DeterminantsRow[])mainDbService.MainDataSet.Determinants.Select(Filter("idfsBaseReference", idfsBaseReference));
            return dets.Length == 1 ? dets[0] : null;
        }

        /// <summary>
        /// ������ ��� �������� ������
        /// </summary>
        /// <param name="primaryKey">��������� ����, ������� ������������ ��� ������ ������</param>
        /// <param name="table">�������, ��� ������� ��������� �������� ������</param>
        /// <returns></returns>
        public static DataRow GetDataRow(Dictionary<string, long> primaryKey, DataTable table)
        {
            DataRow result = null;
            //��������� ���������� ������ � �������, ����� ���������� ��� ������� �����, ��������������� �������
            var findedRows = FindDataRows(primaryKey, table);
            switch (findedRows.Length)
            {
                case 1:
                    result = findedRows[0];
                    break;
                case 0:
                    result = table.NewRow();
                    foreach (var pair in primaryKey)
                    {
                        result[pair.Key] = pair.Value;
                    }
                    table.Rows.Add(result);
                    break;
            }

            return result;
        }

        /// <summary>
        /// �� ���������� ���������� ���������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionsRow GetSectionRow(this DbService mainDbService, long idfsSection)
        {
            var rows = (FlexibleFormsDS.SectionsRow[])mainDbService.MainDataSet.Sections.Select(Filter("idfsSection", idfsSection));
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// �� ���������� ���������� ���������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.RulesRow[] GetRules(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.RulesRow[])mainDbService.MainDataSet.Rules.Select(Filter("idfsFormTemplate", idfsFormTemplate));
        }

        /// <summary>
        /// �� ���������� ���������� ���������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsRule"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.RulesRow GetRule(this DbService mainDbService, long idfsRule)
        {
            var rulesRows = (FlexibleFormsDS.RulesRow[])mainDbService.MainDataSet.Rules.Select(Filter("idfsRule", idfsRule));
            return rulesRows.Length == 1 ? rulesRows[0] : null;
        }

        /// <summary>
        /// ���������� ��������, ��������� � ���������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsParameter"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.RuleParameterForActionRow[] GetRuleParameterForAction(this DbService mainDbService, long idfsFormTemplate, long idfsParameter)
        {
            return (FlexibleFormsDS.RuleParameterForActionRow[])mainDbService.MainDataSet.RuleParameterForAction.Select(Filter("idfsFormTemplate", idfsFormTemplate, "idfsParameter", idfsParameter));
        }

        /// <summary>
        /// ���������� ��������, ��������� � ���������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsRule"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.RuleParameterForActionRow[] GetRuleParameterForAction(this DbService mainDbService, long idfsRule)
        {
            return (FlexibleFormsDS.RuleParameterForActionRow[])mainDbService.MainDataSet.RuleParameterForAction.Select(Filter("idfsRule", idfsRule));
        }

        /// <summary>
        /// �� ���������� ���������� ���������� ���������, ������� �������� ��������� ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParametersRow[] GetParameterChildrenRows(this DbService mainDbService, long idfsSection)
        {
            return (FlexibleFormsDS.ParametersRow[])mainDbService.MainDataSet.Parameters.Select(Filter("idfsSection", idfsSection));
        }

        /// <summary>
        /// �� ���������� ���������� ���������� ������, ������� �������� ��������� ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionsRow[] GetSectionChildrenRows(this DbService mainDbService, long idfsSection)
        {
            return (FlexibleFormsDS.SectionsRow[])mainDbService.MainDataSet.Sections.Select(Filter("idfsParentSection", idfsSection));
        }

        /// <summary>
        /// �� ���������� ���������� ���������� ������, ������� ����������� ���� ����� � ��������� �� ������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionsRow[] GetSectionsByFormType(this DbService mainDbService, long idfsFormType)
        {
            return (FlexibleFormsDS.SectionsRow[])
                   mainDbService.MainDataSet.Sections.Select(
                       Filter("idfsFormType", idfsFormType, "idfsParentSection", null));
        }

        /// <summary>
        /// �� ���������� ���������� ���������� ���������, ������� ����������� ���� ����� � ��������� �� ������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParametersRow[] GetParametersByFormType(this DbService mainDbService, long idfsFormType)
        {
            return (FlexibleFormsDS.ParametersRow[])
                   mainDbService.MainDataSet.Parameters.Select(
                       Filter("idfsFormType", idfsFormType, "idfsSection", null));
        }

        /// <summary>
        /// ���������� �������, ������� ����������� ���� �����
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplatesRow[] GetTemplatesByFormType(this DbService mainDbService, long idfsFormType)
        {
            return (FlexibleFormsDS.TemplatesRow[])
                               mainDbService.MainDataSet.Templates.Select(
                               Filter("idfsFormType", idfsFormType));
        }

        /// <summary>
        /// ��������������� ��������� ��� GetParameterParent
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="sectionList"></param>
        private static void AddSectionInList(this DbService mainDbService, List<long> sectionList)
        {
            //�� ������ ������ ���� ��������� ����������� ������
            if (sectionList == null) return;
            if (sectionList.Count == 0) return;
            var sectionsRow = mainDbService.GetSectionRow(sectionList[sectionList.Count - 1]);
            if (sectionsRow == null) return;
            if (!sectionsRow.IsidfsParentSectionNull())
            {
                sectionList.Add(sectionsRow.idfsParentSection);
                mainDbService.AddSectionInList(sectionList);
            }
        }

        /// <summary>
        /// �������� ��������� ������� ������-��������� ��������� (���� � ������� �� ��������� � ����)
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameters"></param>
        /// <returns></returns>
        public static List<long> GetParameterParent(this DbService mainDbService, long idfsParameters)
        {
            var sectionList = new List<long>();

            var parametersRow = GetParameterRow(mainDbService, idfsParameters);
            if (parametersRow != null)
            {
                if (!parametersRow.IsidfsSectionNull())
                {
                    sectionList.Add(parametersRow.idfsSection);
                    mainDbService.AddSectionInList(sectionList);
                }
            }

            return sectionList;
        }

        /// <summary>
        /// ��������� ������� ������ ����� ��������� ������, ���� �� ������� �� ��� ������ ��������� �����
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="sections"></param>
        /// <returns></returns>
        private static bool HasSectionInSections(this DbService mainDbService, long idfsSection, IEnumerable<FlexibleFormsDS.SectionsRow> sections)
        {
            bool result = false;
            foreach (var row in sections)
            {
                result = row.idfsSection.Equals(idfsSection);
                if (result) break;
                result = HasSectionInSections(mainDbService, idfsSection, GetSectionChildrenRows(mainDbService, row.idfsSection));
                if (result) break;
            }

            return result;
        }

        /// <summary>
        /// ����������, ������� �� � ������ ������ � ������� �������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="sectionsRow"></param>
        /// <returns></returns>
        public static bool HasNestedSections(this DbService mainDbService, FlexibleFormsDS.SectionsRow sectionsRow)
        {
            return (mainDbService.MainDataSet.Sections.Select(Filter("idfsParentSection", sectionsRow.idfsSection))).Length > 0;
        }

        /// <summary>
        /// ����������, ������� �� � ������ ������ � ������� �������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static bool HasNestedSections(this DbService mainDbService, long idfsFormType)
        {
            return (mainDbService.MainDataSet.Sections.Select(Filter("idfsFormType", idfsFormType, "idfsParentSection", null))).Length > 0;
        }

        /// <summary>
        /// ����������, ������� �� � ������ ������ � ������� �������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.FormTypesRow GetFormType(this DbService mainDbService, long idfsFormType)
        {
            if (mainDbService.MainDataSet.FormTypes.Count == 0) mainDbService.LoadFormTypes();
            var formTypesRows = (FlexibleFormsDS.FormTypesRow[])mainDbService.MainDataSet.FormTypes.Select(Filter("idfsFormType", idfsFormType));
            if (formTypesRows.Length == 1) return formTypesRows[0];
            return null;
        }

        /// <summary>
        /// ����������, ������� �� � ������ ������ � ������� �������� ���������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="sectionsRow"></param>
        /// <returns></returns>
        public static bool HasNestedParameters(this DbService mainDbService, FlexibleFormsDS.SectionsRow sectionsRow)
        {
            return (mainDbService.MainDataSet.Parameters.Select(Filter("idfsSection", sectionsRow.idfsSection))).Length > 0;
        }

        /// <summary>
        /// ����������, ������� �� � ������ ������ � ������� �������� ���������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static bool HasNestedParameters(this DbService mainDbService, long idfsFormType)
        {
            return (mainDbService.MainDataSet.Parameters.Select(Filter("idfsFormType", idfsFormType, "idfsSection", null))).Length > 0;
        }

        /// <summary>
        /// ���������, �������� �� ���� ������ �������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSectionParent">������-��������</param>
        /// <param name="idfsSectionChild">������-�������</param>
        /// <returns></returns>
        public static bool IsChildSection(this DbService mainDbService, long idfsSectionParent, long idfsSectionChild)
        {
            bool result = idfsSectionParent.Equals(idfsSectionChild);
            if (!result)
            {
                result = mainDbService.HasSectionInSections(idfsSectionChild, GetSectionChildrenRows(mainDbService, idfsSectionParent));
            }

            return result;
        }

        /// <summary>
        /// �� ���������� ���������� ���������� ��������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameter"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParametersRow GetParameterRow(this DbService mainDbService, long idfsParameter)
        {
            var rows = (FlexibleFormsDS.ParametersRow[])mainDbService.MainDataSet.Parameters.Select(Filter("idfsParameter", idfsParameter));
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// �������� ������ ��� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow[] GetLabelTemplateRows(this DbService mainDbService, long idfsFormTemplate, string langid)
        {
            return (FlexibleFormsDS.LabelsRow[])mainDbService.MainDataSet.Labels.Select(Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid)));
        }

        /// <summary>
        /// �������� ������ ��� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfDecorElement"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow[] GetLabelTemplateRows(this DbService mainDbService, long idfDecorElement, long idfsFormTemplate, string langid)
        {
            return (FlexibleFormsDS.LabelsRow[])mainDbService.MainDataSet.Labels.Select(Filter("idfsFormTemplate", idfsFormTemplate, "idfDecorElement", idfDecorElement, "langid", String.Format("'{0}'", langid)));
        }

        /// <summary>
        /// �������� ������ ��� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow[] GetLabelTemplateRows(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.LabelsRow[])mainDbService.MainDataSet.Labels.Select(Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)));
        }

        /// <summary>
        /// �������� ������ ��� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfDecorElement"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow GetLabelTemplateRow(this DbService mainDbService, long idfDecorElement, long idfsFormTemplate, string langid)
        {
            FlexibleFormsDS.LabelsRow[] labelsRows = GetLabelTemplateRows(mainDbService, idfDecorElement, idfsFormTemplate, langid);
            return labelsRows.Length == 1 ? labelsRows[0] : null;
        }

        /// <summary>
        /// �������� �������� �������������, ������� ����������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplateDeterminantValuesRow[] GetTemplateDeterminants(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.TemplateDeterminantValuesRow[])mainDbService.MainDataSet.TemplateDeterminantValues.Select(Filter("idfsFormTemplate", idfsFormTemplate), "[idfsBaseReference] ASC");
        }

        /// <summary>
        /// ��������� ������� � ������� ������������ � ���������� �����������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsBaseReference"></param>
        /// <returns></returns>
        public static bool HasTemplateDeterminant(this DbService mainDbService, long idfsFormTemplate, long idfsBaseReference)
        {
            return (mainDbService.MainDataSet.TemplateDeterminantValues.Select(Filter("idfsFormTemplate", idfsFormTemplate, "idfsBaseReference", idfsBaseReference), "[idfsBaseReference] ASC").Length > 0)
                || (mainDbService.MainDataSet.TemplateDeterminantValues.Select(Filter("idfsFormTemplate", idfsFormTemplate, "idfsGISBaseReference", idfsBaseReference), "[idfsBaseReference] ASC").Length > 0)
                ;
        }


        /// <summary>
        /// �������� ������ ��� �����
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfDecorElement"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LinesRow GetLineTemplateRow(this DbService mainDbService, long idfDecorElement, long idfsFormTemplate, string langid)
        {
            FlexibleFormsDS.LinesRow[] linesRows = GetLineTemplateRows(mainDbService, idfsFormTemplate, langid);
            FlexibleFormsDS.LinesRow result = null;
            if (linesRows.Length == 1) result = linesRows[0];
            return result;
        }

        /// <summary>
        /// �������� ������ ��� �����
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LinesRow[] GetLineTemplateRows(this DbService mainDbService, long idfsFormTemplate, string langid)
        {
            return (FlexibleFormsDS.LinesRow[])mainDbService.MainDataSet.Lines.Select(Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid)));
        }

        /// <summary>
        /// �������� ������ ��� �����
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LinesRow[] GetLineTemplateRows(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.LinesRow[])mainDbService.MainDataSet.Lines.Select(Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)));
        }


        /// <summary>
        /// ���������� ������ �� ��������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsObservation"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="idfRow"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ActivityParametersRow GetActivityParametersRow(this DbService mainDbService, long idfsObservation, long idfsParameter, long? idfRow)
        {
            FlexibleFormsDS.ActivityParametersRow[] rows;
            if (idfRow.HasValue)
                rows = (FlexibleFormsDS.ActivityParametersRow[])mainDbService.MainDataSet.ActivityParameters.Select(Filter("idfObservation", idfsObservation, "idfsParameter", idfsParameter, "idfRow", idfRow));
            else
                rows = (FlexibleFormsDS.ActivityParametersRow[])mainDbService.MainDataSet.ActivityParameters.Select(Filter("idfObservation", idfsObservation, "idfsParameter", idfsParameter));
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// ���������� ������ �� ��������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfObservation"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ActivityParametersRow[] GetActivityParametersRows(this DbService mainDbService, long idfObservation)
        {
            return (FlexibleFormsDS.ActivityParametersRow[])mainDbService.MainDataSet.ActivityParameters.Select(Filter("idfObservation", idfObservation), "[idfRow] ASC");
        }

        /// <summary>
        /// ���������� ������, �������� � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection">���� null, �� ��������� ��� ������ �� ���������� �������</param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow[] GetSectionTemplateRows(this DbService mainDbService, long? idfsSection, long idfsFormTemplate)
        {
            return GetSectionTemplateRows(mainDbService, idfsSection, idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ���������� ������, �������� � ������, � ������ ��������� layout
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection">���� null, �� ��������� ��� ������ �� ���������� �������</param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow[] GetSectionTemplateRows(this DbService mainDbService, long? idfsSection, long idfsFormTemplate, string langid)
        {
            FlexibleFormsDS.SectionTemplateRow[] rows;
            if (idfsSection.HasValue)
                rows = (FlexibleFormsDS.SectionTemplateRow[])mainDbService.MainDataSet.SectionTemplate.Select(Filter("idfsSection", idfsSection, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid)), "[intOrder] ASC, [intTop] ASC");
            else
                rows = (FlexibleFormsDS.SectionTemplateRow[])mainDbService.MainDataSet.SectionTemplate.Select(Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid)), "[intOrder] ASC, [intTop] ASC");
            return rows;
        }

        /// <summary>
        /// ���������� ������, �������� � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection">���� null, �� ��������� ��� ������ �� ���������� �������</param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow GetSectionTemplateRow(this DbService mainDbService, long idfsSection, long idfsFormTemplate)
        {
            FlexibleFormsDS.SectionTemplateRow[] rows = GetSectionTemplateRows(mainDbService, idfsSection, idfsFormTemplate);
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// ���������� ������, �������� � ������, � ������ ��������� layout
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection">���� null, �� ��������� ��� ������ �� ���������� �������</param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid">���� (en, ru � ������)</param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow GetSectionTemplateRow(this DbService mainDbService, long idfsSection, long idfsFormTemplate, string langid)
        {
            FlexibleFormsDS.SectionTemplateRow[] rows = GetSectionTemplateRows(mainDbService, idfsSection, idfsFormTemplate, langid);
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// ���������� ���������, �������� � ������, � ������ ��������� layout
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid">���� (en, ru � ������)</param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow GetParameterTemplateRow(this DbService mainDbService, long idfsParameter, long idfsFormTemplate, string langid)
        {
            FlexibleFormsDS.ParameterTemplateRow[] rows = GetParameterTemplateRows(mainDbService, idfsParameter, idfsFormTemplate, langid);
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// �������� �������� ����������������� �������� ��� ��������� ���� ���������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameterType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterFixedPresetValueRow[] GetParameterFixedPresetValues(DBServiceParameterTypeEditor mainDbService, long idfsParameterType)
        {
            var rows = (FlexibleFormsDS.ParameterFixedPresetValueRow[])mainDbService.MainDataSet.ParameterFixedPresetValue.Select(Filter("idfsParameterType", idfsParameterType), "[intOrder] ASC");
            return rows.Length > 0 ? rows : null;
        }

        /// <summary>
        /// �������� �������� �������� Reference Type
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsReferenceType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ReferenceTypesListRow[] GetReferenceTypesList(DBServiceParameterTypeEditor mainDbService, long idfsReferenceType)
        {
            var rows = (FlexibleFormsDS.ReferenceTypesListRow[])mainDbService.MainDataSet.ReferenceTypesList.Select(Filter("idfsReferenceType", idfsReferenceType));
            return rows.Length > 0 ? rows : null;
        }

        /// <summary>
        /// ���������� ���������, �������� � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow GetParameterTemplateRow(this DbService mainDbService, long idfsParameter, long idfsFormTemplate)
        {
            FlexibleFormsDS.ParameterTemplateRow[] rows = GetParameterTemplateRows(mainDbService, idfsParameter, idfsFormTemplate);
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// ���������� ���������, �������� � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow[] GetParameterTemplateRows(this DbService mainDbService, long? idfsParameter, long idfsFormTemplate, string langid)
        {
            FlexibleFormsDS.ParameterTemplateRow[] rows;
            var lng = String.Format("'{0}'", langid);
            if (idfsParameter.HasValue)
                rows = (FlexibleFormsDS.ParameterTemplateRow[])mainDbService.MainDataSet.ParameterTemplate.Select(Filter("idfsParameter", idfsParameter, "idfsFormTemplate", idfsFormTemplate, "langid", lng), "[intOrder] ASC, [intTop] ASC");
            else
                rows = (FlexibleFormsDS.ParameterTemplateRow[])mainDbService.MainDataSet.ParameterTemplate.Select(Filter("idfsFormTemplate", idfsFormTemplate, "langid", lng), "[intOrder] ASC, [intTop] ASC");
            return rows;
        }

        /// <summary>
        /// �������� �������� ��������� �������� ��� ���������� ���� ���������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="idfsParameterType"></param>
        /// <param name="intHACode"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterSelectListRow[] GetParameterSelectListRows(this DbService mainDbService, long idfsParameter, long idfsParameterType, long? intHACode, string langid)
        {
            var filter = Filter("idfsParameter", idfsParameter, "idfsParameterType", idfsParameterType, "langid", String.Format("'{0}'", langid));
            if (intHACode.HasValue && !intHACode.Value.Equals(0))
            {
                var haCodeFilter = String.Empty;
                for (var i = 1; i <= 7; i++)

                {
                    if ((intHACode.Value & (1 << i)) != 0)
                        haCodeFilter = AppendHacodeFilter(haCodeFilter,
                                                          string.Format(
                                                              "Convert((intHACode - intHACode%{0})/{0},'System.Int32') % 2 = 1",
                                                              1<<i));
                }

                if (haCodeFilter.Length > 0)
                    filter =
                        String.Format(
                            "[idfsParameter]={0} and [idfsParameterType]={1} and [langid]='{2}' and ({3})"
                            , idfsParameter
                            , idfsParameterType
                            , langid
                            , haCodeFilter);
            }

            return (FlexibleFormsDS.ParameterSelectListRow[])mainDbService.MainDataSet.ParameterSelectList.Select(filter, "[intOrder] ASC, [strValueCaption] ASC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filterPart"></param>
        /// <returns></returns>
        public static string AppendHacodeFilter(string filter, string filterPart)
        {
            if (String.IsNullOrEmpty(filterPart))
                return filter;
            if (String.IsNullOrEmpty(filter))
                return string.Format("({0})", filterPart);
            return string.Format("{0} OR ({1})", filter, filterPart);
        }

        /// <summary>
        /// ���������� ���������, �������� � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow[] GetParameterTemplateRows(this DbService mainDbService, long? idfsParameter, long idfsFormTemplate)
        {
            return GetParameterTemplateRows(mainDbService, idfsParameter, idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ���������� ���������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow[] GetParameterTemplateChildrenRows(this DbService mainDbService, long? idfsSection, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.ParameterTemplateRow[])mainDbService.MainDataSet.ParameterTemplate.Select(Filter("idfsSection", idfsSection, "idfsFormTemplate", idfsFormTemplate), "[intOrder] ASC");
        }

        /// <summary>
        /// ���������� ���������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow[] GetParameterTemplateChildrenRows(this DbService mainDbService, long idfsSection, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.ParameterTemplateRow[])mainDbService.MainDataSet.ParameterTemplate.Select(Filter("idfsSection", idfsSection, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)), "[intTop] ASC");
        }

        /// <summary>
        /// ���������� ���������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow[] GetParameterTemplateChildrenRows(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.ParameterTemplateRow[])mainDbService.MainDataSet.ParameterTemplate.Select(Filter("idfsSection", null, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)), "[intTop] ASC, [intOrder] ASC");
        }

        /// <summary>
        /// ���������� ������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow[] GetLabelsTemplateChildrenRows(this DbService mainDbService, long? idfsSection, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.LabelsRow[])mainDbService.MainDataSet.Labels.Select(Filter("idfsSection", idfsSection, "idfsFormTemplate", idfsFormTemplate));
        }

        /// <summary>
        /// ���������� ������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow[] GetLabelsTemplateChildrenRows(this DbService mainDbService, long idfsSection, long idfsFormTemplate, string langid)
        {
            return (FlexibleFormsDS.LabelsRow[])mainDbService.MainDataSet.Labels.Select(Filter("idfsSection", idfsSection, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid)), "[intTop] ASC");
        }

        /// <summary>
        /// ���������� ������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LabelsRow[] GetLabelsTemplateChildrenRows(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.LabelsRow[])mainDbService.MainDataSet.Labels.Select(Filter("idfsSection", null, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)), "[intTop] ASC");
        }

        /// <summary>
        /// ���������� �����, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.LinesRow[] GetLinesTemplateChildrenRows(this DbService mainDbService, long? idfsSection, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.LinesRow[])mainDbService.MainDataSet.Lines.Select(Filter("idfsSection", idfsSection, "idfsFormTemplate", idfsFormTemplate));
        }

        /// <summary>
        /// ���������� ������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow[] GetSectionTemplateChildrenRows(this DbService mainDbService, long idfsSection, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.SectionTemplateRow[])mainDbService.MainDataSet.SectionTemplate.Select(Filter("idfsParentSection", idfsSection, "idfsFormTemplate", idfsFormTemplate));
        }

        /// <summary>
        /// ���������� ������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow[] GetSectionTemplateChildrenRows(this DbService mainDbService, long? idfsSection, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.SectionTemplateRow[])mainDbService.MainDataSet.SectionTemplate.Select(Filter("idfsParentSection", idfsSection, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)), "[intTop] ASC, [intOrder] ASC");
        }

        /// <summary>
        /// ���������� ������, �������� � ������, ������� ��������� � ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow[] GetSectionTemplateChildrenRows(this DbService mainDbService, long idfsFormTemplate)
        {
            return (FlexibleFormsDS.SectionTemplateRow[])mainDbService.MainDataSet.SectionTemplate.Select(Filter("idfsParentSection", null, "idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", bv.model.Model.Core.ModelUserContext.CurrentLanguage)), "[intTop] ASC, [intOrder] ASC");
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplatesRow GetTemplateRow(this DbService mainDbService, long idfsFormTemplate)
        {
            var rows = (FlexibleFormsDS.TemplatesRow[])mainDbService.MainDataSet.Templates.Select(Filter("idfsFormTemplate", idfsFormTemplate));
            return rows.Length == 1 ? rows[0] : null;
        }

        /// <summary>
        /// ������� �������� ������
        /// </summary>
        /// <param name="primaryKey">��������� ����, ������� ������������ ��� ������ ������</param>
        /// <param name="table">�������, ��� ������� ��������� �������� ������</param>
        /// <returns></returns>
        public static DataRow[] FindDataRows(Dictionary<string, long> primaryKey, DataTable table)
        {
            //���������� ������ �������, ����� ���������� ������� ������� ������
            var sbFilter = new StringBuilder();
            int i = 0;
            foreach (var pair in primaryKey)
            {
                sbFilter.AppendFormat(Filter(pair.Key, pair.Value));
                if (i < primaryKey.Count - 1) sbFilter.Append(" and ");
                i++;
            }
            //��������� ���������� ������ � �������, ����� ���������� ��� ������� �����, ��������������� �������
            return table.Select(sbFilter.ToString());
        }

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Filter(string fieldName, object value)
        {
            return Filter(fieldName, value, true);
        }

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="notNull">true - ���������� �������� �������������� ���������, false - '-1' ��� ������ ����������������� � null</param>
        /// <returns></returns>
        public static string Filter(string fieldName, object value, bool notNull)
        {
            string result;

            if (Utils.IsEmpty(value))
            {
                result = String.Format("[{0}] is null", fieldName);
            }
            else if (value is Int64)
            {
                result = Convert.ToInt64(value) < 0
                             ? (notNull
                                    ? String.Format("[{0}] = {1}", fieldName, value)
                                    : String.Format("[{0}] is null", fieldName))
                             : String.Format("[{0}] = {1}", fieldName, value);
            }
            else
            {
                result = String.Format("[{0}] = {1}", fieldName, value);
            }

            return result;
        }

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        /// <returns></returns>
        public static string Filter(string fieldName1, object value1, string fieldName2, object value2)
        {
            return String.Format("{0} and {1}", Filter(fieldName1, value1), Filter(fieldName2, value2));
        }

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        /// <returns></returns>
        public static string Filter(string fieldName1, object value1, string fieldName2, object value2, string fieldName3, object value3)
        {
            return String.Format("{0} and {1} and {2}", Filter(fieldName1, value1), Filter(fieldName2, value2), Filter(fieldName3, value3));
        }

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        /// <returns></returns>
        public static string Filter(string fieldName1, object value1, string fieldName2, object value2, string fieldName3, object value3, string fieldName4, object value4)
        {
            return String.Format("{0} and {1} and {2} and {3}", Filter(fieldName1, value1), Filter(fieldName2, value2), Filter(fieldName3, value3), Filter(fieldName4, value4));
        }

        /// <summary>
        /// ���������� ������� ����� � ������� �� ����������� ������ ������
        /// </summary>
        /// <param name="table">�������, � ������� ���� ����������� �����</param>
        /// <param name="keys">����� ������ ��� ������</param>
        /// <returns></returns>
        public static bool HasRows(this DataTable table, Dictionary<string, long?> keys)
        {
            bool result = false;
            foreach (DataRow row in table.Rows)
            {
                //�������, ��� ��� ���� ������������� �������
                bool sequence = true;

                //�������, ��� ��� ���������� ���� ���� � �������. ���� ��� � ��� ����� ��� ������, ������ ��� �������� ���������� ���������
                foreach (var key in keys)
                {
                    if (!row[key.Key, DataRowVersion.Original].Equals(key.Value))
                    {
                        sequence = false;
                        break;
                    }
                }

                //���� ���� �� ��� ����� ������ ��������� �������, �� �������
                if (sequence)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// ���������� ������� ����� � ������� �� ����������� ������ ������
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key1"></param>
        /// <param name="value1"></param>
        /// <returns></returns>
        public static bool HasRows(this DataTable table, string key1, long? value1)
        {
            var keys = new Dictionary<string, long?> { { key1, value1 } };
            return HasRows(table, keys);
        }

        /// <summary>
        /// ���������� ������� ����� � ������� �� ����������� ������ ������
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key1"></param>
        /// <param name="value1"></param>
        /// <param name="key2"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool HasRows(this DataTable table, string key1, long? value1, string key2, long? value2)
        {
            var keys = new Dictionary<string, long?> { { key1, value1 }, { key2, value2 } };
            return HasRows(table, keys);
        }

        /// <summary>
        /// ��������� ����������� �������� �� ������ �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfVersion"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.PredefinedStubRow[] GetPredefinedStubRows(this DbServiceUserData mainDbService, long idfVersion)
        {
            return (FlexibleFormsDS.PredefinedStubRow[])mainDbService.MainDataSet.PredefinedStub.Select(Filter("idfVersion", idfVersion));
        }

        /// <summary>
        /// ��������� ����������� �������� �� ID ��������� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.PredefinedStubRow[] GetPredefinedStubRowsBySection(this DbService mainDbService, long idfsSection)
        {
            return (FlexibleFormsDS.PredefinedStubRow[])mainDbService.MainDataSet.PredefinedStub.Select(Filter("idfsSection", idfsSection));
        }

        /// <summary>
        /// �������� ���� ��� ����������� ������ - ������ ��� ������������� � ������ ��������� �������� ��������� ������ (MergeTemplates)
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="observations"></param>
        /// <param name="ffType"> </param>
        /// <returns></returns>
        public static List<StubInfo> GetListSectionsAndVersionsForStub(this DbService mainDbService, IEnumerable<long> observations, FFType ffType)
        {
            var observationsRows = mainDbService.LoadObservations(observations);
            return GetListSectionsAndVersionsForStub(mainDbService, observationsRows, ffType);
        }

        /// <summary>
        /// �������� ���� ��� ����������� ������ - ������ ��� ������������� � ������ ��������� �������� ��������� ������ (MergeTemplates)
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="observationsRows"></param>
        /// <param name="formType"> </param>
        /// <returns></returns>
        public static List<StubInfo> GetListSectionsAndVersionsForStub(this DbService mainDbService, IEnumerable<FlexibleFormsDS.ObservationsRow> observationsRows, FFType formType)
        {
            var list = new List<StubInfo>();
            foreach (var observationsRow in observationsRows)
            {
                if (observationsRow.IsidfsFormTemplateNull()) continue;

                FlexibleFormsDS.AggregateGetMatrixVersionRow matrixVersionRow;
                long? idfsSection;
                long? idfsMatrixType;
                mainDbService.GetSectionForAggregateCase(observationsRow.idfsFormTemplate, (long)formType, out idfsSection, out idfsMatrixType);
                if (!idfsSection.HasValue || !idfsMatrixType.HasValue) continue;

                if (observationsRow.idfObservation == FFRender.idfObservationFake)
                {
                    matrixVersionRow =
                        mainDbService.MainDataSet.AggregateGetMatrixVersion.NewAggregateGetMatrixVersionRow();

                    var cache = LookupCache.Get(LookupTables.AggregateMatrixVersion);

                    long? matrixVersion = null;
                    cache.RowFilter = String.Format("idfsAggrCaseSection={0} and blnIsActive=1", idfsSection.Value);
                    if (cache.Count > 0)
                    {
                        var o = cache[0]["idfVersion"];
                        if (o != null) matrixVersion = Int64.Parse(o.ToString());
                    }

                    if (matrixVersion.HasValue)
                    {
                        matrixVersionRow.idfAggregateCaseSection = idfsSection.Value;
                        matrixVersionRow.idfVersion = matrixVersion.Value;
                    }
                }
                else
                {
                    matrixVersionRow = mainDbService.LoadMatrixStubInfo(observationsRow.idfObservation);
                }

                //���� �� ������� ����� ������� (��������, ��� ��� �� ��������), �� ���� � ������ Observation
                if (matrixVersionRow == null) continue;
                if (matrixVersionRow.IsidfVersionNull()) continue;
                if (matrixVersionRow.idfVersion <= 0) continue;

                var stubInfo = new StubInfo
                                   {
                                       IdfObservation = observationsRow.idfObservation,
                                       IdfsFormTemplate = observationsRow.idfsFormTemplate,
                                       IdfsSectionTable = idfsSection.Value,
                                       IdfVersion = matrixVersionRow.idfVersion,
                                       MatrixType = idfsMatrixType.Value
                                   };
                list.Add(stubInfo);
            }
            return list;
        }

        /// <summary>
        /// ������ ������ ������ ��� ������� �� ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="rowSection"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.SectionTemplateRow CreateSectionTemplateRow(this DbService mainDbService, FlexibleFormsDS.SectionsRow rowSection, long idfsFormTemplate)
        {
            #region ��������� ������ � ������� ������

            var rowSectionTemplate = mainDbService.MainDataSet.SectionTemplate.NewSectionTemplateRow();
            rowSectionTemplate.idfsSection = rowSection.idfsSection;
            if (!rowSection.IsidfsParentSectionNull())
                rowSectionTemplate.idfsParentSection = rowSection.idfsParentSection;
            else
                rowSectionTemplate.SetidfsParentSectionNull();
            rowSectionTemplate.idfsFormType = rowSection.idfsFormType;
            rowSectionTemplate.idfsFormTemplate = idfsFormTemplate;
            rowSectionTemplate.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            rowSectionTemplate.blnFreeze = false;
            rowSectionTemplate.blnGrid = rowSection.blnGrid;
            rowSectionTemplate.blnFixedRowSet = rowSection.blnFixedRowSet;
            rowSectionTemplate.DefaultName = rowSection.DefaultName;
            rowSectionTemplate.NationalName = rowSection.NationalName;
            rowSectionTemplate.intLeft = 0;
            rowSectionTemplate.intTop = 0;
            rowSectionTemplate.intWidth = Section.DefaultSectionSize.Width;
            rowSectionTemplate.intHeight = Section.DefaultSectionSize.Height;
            rowSectionTemplate.intCaptionHeight = FFRender.DefaultCaptionHeight;
            rowSectionTemplate.intOrder = 1000;
            rowSectionTemplate.IsStubLoaded = false;
            rowSectionTemplate.blnIsRealStruct = true;
            mainDbService.MainDataSet.SectionTemplate.AddSectionTemplateRow(rowSectionTemplate);

            #endregion

            return rowSectionTemplate;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetStubLoaded(this DbService mainDbService, long idfsFormTemplate, long idfsSectionTable)
        {
            var sectionTemplateRow = GetSectionTemplateRow(mainDbService, idfsSectionTable, idfsFormTemplate);
            if (sectionTemplateRow != null) sectionTemplateRow.IsStubLoaded = GetPredefinedStubRowsBySection(mainDbService, idfsSectionTable).Length > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="templateDeterminantValuesRow"></param>
        /// <param name="determinantsRow"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsFormType"></param>
        /// <param name="isNewRow"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplateDeterminantValuesRow CreateTemplateDeterminantValuesRow(this DbService mainDbService, FlexibleFormsDS.TemplateDeterminantValuesRow templateDeterminantValuesRow, FlexibleFormsDS.DeterminantsRow determinantsRow, long idfsFormTemplate, long idfsFormType, bool isNewRow)
        {
            templateDeterminantValuesRow.idfsFormTemplate = idfsFormTemplate;
            templateDeterminantValuesRow.DeterminantType = determinantsRow.idfsReferenceType;
            templateDeterminantValuesRow.idfsFormType = idfsFormType;
            templateDeterminantValuesRow.DeterminantDefaultName = determinantsRow.DefaultName;
            templateDeterminantValuesRow.DeterminantNationalName = determinantsRow.NationalName;
            templateDeterminantValuesRow.DeterminantTypeDefaultName = determinantsRow.DefaultTypeName;
            templateDeterminantValuesRow.DeterminantTypeNationalName = determinantsRow.NationalTypeName;
            templateDeterminantValuesRow.DeterminantValue = determinantsRow.idfsBaseReference;
            if (determinantsRow.idfsReferenceType.Equals((long)FFDeterminantTypes.Country))
            {
                templateDeterminantValuesRow.idfsGISBaseReference = determinantsRow.idfsBaseReference;
                templateDeterminantValuesRow.SetidfsBaseReferenceNull();
            }
            else
            {
                templateDeterminantValuesRow.idfsBaseReference = determinantsRow.idfsBaseReference;
                templateDeterminantValuesRow.SetidfsGISBaseReferenceNull();
            }

            if (isNewRow)
                mainDbService.MainDataSet.TemplateDeterminantValues.AddTemplateDeterminantValuesRow(templateDeterminantValuesRow);

            return templateDeterminantValuesRow;
        }

        /// <summary>
        /// ������ ������ �������� ������������ �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="determinantsRow"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplateDeterminantValuesRow CreateTemplateDeterminantValuesRow(this DbService mainDbService, FlexibleFormsDS.DeterminantsRow determinantsRow, long idfsFormTemplate, long idfsFormType)
        {
            var templateDeterminantValuesRow = mainDbService.MainDataSet.TemplateDeterminantValues.NewTemplateDeterminantValuesRow();
            return mainDbService.CreateTemplateDeterminantValuesRow(templateDeterminantValuesRow, determinantsRow, idfsFormTemplate, idfsFormType, true);
        }

        /// <summary>
        /// ������ ������ ��������� ��� ������� �� ���������
        /// </summary>
        /// <param name="rowParameter"></param>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.ParameterTemplateRow CreateParameterTemplateRow(this DbService mainDbService, FlexibleFormsDS.ParametersRow rowParameter, long idfsFormTemplate)
        {
            //�������� ������� ����� ������
            var rowParameterTemplate = GetParameterTemplateRow(mainDbService, rowParameter.idfsParameter, idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);

            if (rowParameterTemplate == null)
            {
                rowParameterTemplate = mainDbService.MainDataSet.ParameterTemplate.NewParameterTemplateRow();

                rowParameterTemplate.idfsParameter = rowParameter.idfsParameter;
                if (!rowParameter.IsidfsSectionNull()) rowParameterTemplate.idfsSection = rowParameter.idfsSection;
                rowParameterTemplate.idfsFormType = rowParameter.idfsFormType;
                rowParameterTemplate.idfsFormTemplate = idfsFormTemplate;
                rowParameterTemplate.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
                rowParameterTemplate.idfsEditor = rowParameter.idfsEditor;
                rowParameterTemplate.idfsParameterType = rowParameter.idfsParameterType;
                rowParameterTemplate.idfsEditMode = ((long)FFEditModes.Ordinary).ToString();
                rowParameterTemplate.intScheme = rowParameter.intScheme;
                rowParameterTemplate.intLabelSize = rowParameter.intLabelSize;
                rowParameterTemplate.DefaultName = rowParameter.DefaultName;
                rowParameterTemplate.NationalName = rowParameter.NationalName;
                rowParameterTemplate.intWidth = rowParameter.intWidth;
                rowParameterTemplate.intHeight = rowParameter.intHeight;
                rowParameterTemplate.intHACode = rowParameter.intHACode;
                rowParameterTemplate.intOrder = rowParameter.intOrder;
                rowParameterTemplate.blnIsRealStruct = true;
                //��� ���������� � ��������� ������� ����� ����� ������ ���������� ������� (intOrder)
                if ((rowParameterTemplate.intOrder >= 0) && (!rowParameter.IsidfsSectionNull()))
                {
                    var parentSection = GetSectionTemplateRow(mainDbService, rowParameter.idfsSection, idfsFormTemplate);
                    if (parentSection != null)
                    {
                        //��������� ������� ��� ��������� ����������
                        var parameters = GetParameterTemplateRows(mainDbService, null, idfsFormTemplate);
                        rowParameterTemplate.intOrder = parameters.Length;
                    }
                }
                rowParameterTemplate.intLeft = 0;
                rowParameterTemplate.intTop = 0;
                rowParameterTemplate.blnFreeze = false;
                mainDbService.MainDataSet.ParameterTemplate.AddParameterTemplateRow(rowParameterTemplate);
            }

            return rowParameterTemplate;
        }

        /// <summary>
        /// ��������� ����� �� ������� ������ ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="templatesRow"></param>
        public static bool CanDeleteTemplate(this DbService mainDbService, FlexibleFormsDS.TemplatesRow templatesRow)
        {
            return Utils.IsEmpty(GetResponse(mainDbService, String.Format("Select [ErrorMessage] from dbo.fnFFCheckForRemoveTemplate({0})", templatesRow.idfsFormTemplate), null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="commandText"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private static object GetResponse(this DbService mainDbService, string commandText, IDbTransaction transaction)
        {
            var cmd = mainDbService.CreateCommand(commandText, transaction);
            ErrorMessage errMsg = null;
            return BaseDbService.ExecScalar(cmd, mainDbService.Connection, ref errMsg, transaction);
        }

        /// <summary>
        /// ��������� ������� �� observation �� ������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        public static bool HasObservationsForTemplate(this DbService mainDbService, long idfsFormTemplate)
        {
            return HasObservationsForTemplate(mainDbService, idfsFormTemplate, null);
        }

        /// <summary>
        /// ��������� ������� �� observation �� ������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="transaction"></param>
        public static bool HasObservationsForTemplate(this DbService mainDbService, long idfsFormTemplate, IDbTransaction transaction)
        {
            return !Utils.IsEmpty(GetResponse(mainDbService, String.Format("Select [Result] from dbo.fnFFCheckForTemplateHasObservations({0})", idfsFormTemplate), transaction));
        }

        /// <summary>
        /// �������� ������ �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="langid">����, ��� �������� ����� ������� ������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteSectionTemplate(this DbService mainDbService, long idfsFormTemplate, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.SectionTemplate, idfsFormTemplate, langid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        private static void DeleteRow(DataRow row, long idfsFormTemplate, string langid)
        {
            //���� ����� ����, �� ������� ������ ��� ����� �����. ���� �� ����� -- �� ���.
            if (langid.Length > 0)
            {
                if (row["idfsFormTemplate"].Equals(idfsFormTemplate) && row["langid"].Equals(langid))
                {
                    row.Delete();
                }
            }
            else
            {
                if (row["idfsFormTemplate"].Equals(idfsFormTemplate))
                {
                    row.Delete();
                }
            }
        }

        /// <summary>
        /// �������� ���������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="langid">����, ��� �������� ����� ������� ���������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteParameterTemplate(this DbService mainDbService, long idfsFormTemplate, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.ParameterTemplate, idfsFormTemplate, langid);
        }

        /// <summary>
        /// �������� ���������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="idfsParameter"></param>
        /// <param name="langid">����, ��� �������� ����� ������� ���������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteParameterTemplate(this DbService mainDbService, long idfsFormTemplate, long idfsParameter, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.ParameterTemplate, GetFilterWithParameter(idfsFormTemplate, idfsParameter, langid));
        }

        /// <summary>
        /// �������� ���������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="idfsSection"></param>
        /// <param name="langid">����, ��� �������� ����� ������� ���������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteSectionTemplate(this DbService mainDbService, long idfsFormTemplate, long idfsSection, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.SectionTemplate, GetFilterWithSection(idfsFormTemplate, idfsSection, langid));
        }

        /// <summary>
        /// �������� ���������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="idfDecorElement"></param>
        /// <param name="langid">����, ��� �������� ����� ������� ���������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteLabelTemplate(this DbService mainDbService, long idfsFormTemplate, long idfDecorElement, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.Labels, GetFilterWithDecorElement(idfsFormTemplate, idfDecorElement, langid));
        }

        /// <summary>
        /// �������� ���������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="idfDecorElement"></param>
        /// <param name="langid">����, ��� �������� ����� ������� ���������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteLineTemplate(this DbService mainDbService, long idfsFormTemplate, long idfDecorElement, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.Lines, GetFilterWithDecorElement(idfsFormTemplate, idfDecorElement, langid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsParameter"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        private static string GetFilterWithParameter(long idfsFormTemplate, long idfsParameter, string langid)
        {
            return langid.Length == 0
                                      ? Filter("idfsFormTemplate", idfsFormTemplate, "idfsParameter", idfsParameter)
                                      : Filter("idfsFormTemplate", idfsFormTemplate, "idfsParameter", idfsParameter, "langid", langid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsSection"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        private static string GetFilterWithSection(long idfsFormTemplate, long idfsSection, string langid)
        {
            return langid.Length == 0
                                      ? Filter("idfsFormTemplate", idfsFormTemplate, "idfsSection", idfsSection)
                                      : Filter("idfsFormTemplate", idfsFormTemplate, "idfsSection", idfsSection, "langid", langid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfDecorElement"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        private static string GetFilterWithDecorElement(long idfsFormTemplate, long idfDecorElement, string langid)
        {
            return langid.Length == 0
                                      ? Filter("idfsFormTemplate", idfsFormTemplate, "idfDecorElement", idfDecorElement)
                                      : Filter("idfsFormTemplate", idfsFormTemplate, "idfDecorElement", idfDecorElement, "langid", langid);
        }

        /// <summary>
        /// �������� ������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="langid">����, ��� �������� ����� �������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteLabelTemplate(this DbService mainDbService, long idfsFormTemplate, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.Labels, idfsFormTemplate, langid);
        }

        /// <summary>
        /// �������� ����� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        /// <param name="langid">����, ��� �������� ����� �������. ���� �� �����, �� ��������� ���.</param>
        public static void DeleteLineTemplate(this DbService mainDbService, long idfsFormTemplate, string langid)
        {
            DeleteRows(mainDbService.MainDataSet.Lines, idfsFormTemplate, langid);
        }

        /// <summary>
        /// ������� ������ �� �������
        /// </summary>
        /// <param name="table"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        public static void DeleteRows(DataTable table, long idfsFormTemplate, string langid)
        {
            for (int i = table.Rows.Count - 1; i >= 0; i--)
            {
                var row = table.Rows[i];
                if (!row.IsRowAlive()) continue;
                DeleteRow(row, idfsFormTemplate, langid);
            }
        }

        /// <summary>
        /// ������� ������ �� �������
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filterString"></param>
        public static void DeleteRows(DataTable table, string filterString)
        {
            var rows = table.Select(filterString);

            for (int i = rows.Length - 1; i >= 0; i--)
            {
                var row = rows[i];
                if (row.IsRowAlive()) row.Delete();
            }
        }

        /// <summary>
        /// ���������, ����� �� ��������� ������ � �������
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public static bool NeedRefreshDataTable(DataTable dataTable, string filterString)
        {
            var needRefresh = dataTable.Select(filterString).Length == 0;
            //���� ������ ��� �� �������, �� ��������� ������� �� ������� �����, ������� ����� ������������, �� ���� �������
            if (needRefresh)
                needRefresh = dataTable.Select(filterString, String.Empty, DataViewRowState.Deleted).Length == 0;
            return needRefresh;
        }

        /// <summary>
        /// �������� ������������� �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        public static void DeleteDeterminantsFromTemplate(this DbService mainDbService, long idfsFormTemplate)
        {
            for (int i = mainDbService.MainDataSet.TemplateDeterminantValues.Count - 1; i >= 0; i--)
            {
                var determinantValuesRow = mainDbService.MainDataSet.TemplateDeterminantValues[i];
                if (!determinantValuesRow.IsRowAlive()) continue;
                if (determinantValuesRow.idfsFormTemplate.Equals(idfsFormTemplate)) determinantValuesRow.Delete();
            }
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="rulesRow"></param>
        public static void DeleteRule(this DbService mainDbService, FlexibleFormsDS.RulesRow rulesRow)
        {
            DeleteRuleConstants(mainDbService, rulesRow.idfsRule);

            for (int i = 0; i < mainDbService.MainDataSet.RuleParameterForAction.Count; i++)
            {
                var ruleParameterForActionRow = (FlexibleFormsDS.RuleParameterForActionRow)mainDbService.MainDataSet.RuleParameterForAction.Rows[i];
                if (!ruleParameterForActionRow.IsRowAlive()) continue;
                if (ruleParameterForActionRow.idfsRule.Equals(rulesRow.idfsRule)) ruleParameterForActionRow.Delete();
            }

            for (int i = 0; i < mainDbService.MainDataSet.RuleParameterForFunction.Count; i++)
            {
                var ruleParameterForFunctionRow = (FlexibleFormsDS.RuleParameterForFunctionRow)mainDbService.MainDataSet.RuleParameterForFunction.Rows[i];
                if (!ruleParameterForFunctionRow.IsRowAlive()) continue;
                if (ruleParameterForFunctionRow.idfsRule.Equals(rulesRow.idfsRule)) ruleParameterForFunctionRow.Delete();
            }

            rulesRow.Delete();
        }

        /// <summary>
        /// ������� ���������, ��������� � ��������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsRule"></param>
        public static void DeleteRuleConstants(this DbService mainDbService, long idfsRule)
        {
            for (int i = 0; i < mainDbService.MainDataSet.RuleConstant.Count; i++)
            {
                var ruleConstantRow = (FlexibleFormsDS.RuleConstantRow)mainDbService.MainDataSet.RuleConstant.Rows[i];
                if (!ruleConstantRow.IsRowAlive()) continue;
                if (ruleConstantRow.idfsRule.Equals(idfsRule)) ruleConstantRow.Delete();
            }
        }

        /// <summary>
        /// �������� ������ �� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate">������</param>
        public static void DeleteRulesFromTemplate(this DbService mainDbService, long idfsFormTemplate)
        {
            for (int i = 0; i < mainDbService.MainDataSet.Rules.Count; i++)
            {
                var rulesRow = mainDbService.MainDataSet.Rules[i];
                if (!rulesRow.IsRowAlive() || (rulesRow.RowState == DataRowState.Unchanged)) continue;
                if (rulesRow.idfsFormTemplate.Equals(idfsFormTemplate)) DeleteRule(mainDbService, rulesRow);
            }
        }

        /// <summary>
        /// ������������ ��������� ����������� �� ���� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        public static void FitParameterHostInContainer(this DbService mainDbService, long idfsFormTemplate)
        {
            //����������� ������� ������ (������� ���� ��������)
            FitParameterHostInContainer(mainDbService, idfsFormTemplate, null);

            //�������� �� ���� ������� � ������������ � ���
            var sections = GetSectionTemplateRows(mainDbService, null, idfsFormTemplate);
            foreach (var sectionTemplateRow in sections)
            {
                FitParameterHostInContainer(mainDbService, idfsFormTemplate, sectionTemplateRow.idfsSection);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="containerID"></param>
        /// <param name="left"></param>
        /// <param name="labelSize"></param>
        /// <param name="width"></param>
        public static void GetSizesForFit(DbService mainDbService, long idfsFormTemplate, long? containerID, out int left, out int labelSize, out int width)
        {
            //����� ����� ����� ������� �������� ��� ������ � ���������� (������� ������ ��� ������� ����� ��������). ������ ������� Left.
            //����� � ����� �� ��������� � "��������" (� ����������� �������� ����� ������������ �� ���, ���� ��� �� ����� �����, �� ���� ������� ��� ���������� �����)
            left = 3;
            labelSize = 300;
            width = 750;

            //������� ������������� �� ����������� ���������� Top
            FlexibleFormsDS.ParameterTemplateRow[] parameters;
            FlexibleFormsDS.SectionTemplateRow[] sections;

            if (containerID.HasValue)
            {
                parameters = GetParameterTemplateChildrenRows(mainDbService, containerID.Value, idfsFormTemplate); //LanguageTemplate????
                sections = GetSectionTemplateChildrenRows(mainDbService, containerID.Value, idfsFormTemplate);
            }
            else
            {
                parameters = GetParameterTemplateChildrenRows(mainDbService, idfsFormTemplate); //LanguageTemplate????
                sections = GetSectionTemplateChildrenRows(mainDbService, idfsFormTemplate);
            }

            if ((parameters.Length == 0) && (sections.Length == 0)) return;

            if ((parameters.Length > 0) && (sections.Length > 0))
            {
                if (parameters[0].intTop < sections[0].intTop)
                {
                    left = parameters[0].intLeft;
                    width = parameters[0].intWidth;
                }
                else
                {
                    left = sections[0].intLeft;
                    width = sections[0].intWidth;
                }
                labelSize = parameters[0].intLabelSize;
            }
            else if ((parameters.Length > 0) && (sections.Length == 0))
            {
                left = parameters[0].intLeft;
                width = parameters[0].intWidth;
                labelSize = parameters[0].intLabelSize;
            }
            else if ((parameters.Length == 0) && (sections.Length > 0))
            {
                left = sections[0].intLeft;
                width = sections[0].intWidth;
            }
        }

        /// <summary>
        /// ������������ ��������� ����������� � ����������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="containerID"></param>
        public static void FitParameterHostInContainer(this DbService mainDbService, long idfsFormTemplate, long? containerID)
        {
            //����� ����� ����� ������� �������� ��� ������ � ���������� (������� ������ ��� ������� ����� ��������). ������ ������� Left.
            //����� � ����� �� ��������� � "��������" (� ����������� �������� ����� ������������ �� ���, ���� ��� �� ����� �����, �� ���� ������� ��� ���������� �����)
            //int left = -100;
            //int labelSize = -100;
            var left = 3;
            var labelSize = 300;
            //var width = 750;

            //������� ������������� �� ����������� ���������� Top
            FlexibleFormsDS.ParameterTemplateRow[] parameters;
            FlexibleFormsDS.SectionTemplateRow[] sections;
            FlexibleFormsDS.LabelsRow[] labels;
            if (containerID.HasValue)
            {
                parameters = GetParameterTemplateChildrenRows(mainDbService, containerID.Value, idfsFormTemplate); //LanguageTemplate????
                sections = GetSectionTemplateChildrenRows(mainDbService, containerID.Value, idfsFormTemplate);
                labels = GetLabelsTemplateChildrenRows(mainDbService, containerID.Value, idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);
            }
            else
            {
                parameters = GetParameterTemplateChildrenRows(mainDbService, idfsFormTemplate); //LanguageTemplate????
                sections = GetSectionTemplateChildrenRows(mainDbService, idfsFormTemplate);
                labels = GetLabelsTemplateChildrenRows(mainDbService, idfsFormTemplate);
            }

            if ((parameters.Length == 0) && (sections.Length == 0)) return;

            if ((parameters.Length > 0) && (sections.Length > 0))
            {
                left = parameters[0].intTop < sections[0].intTop ? parameters[0].intLeft : sections[0].intLeft;
                labelSize = parameters[0].intLabelSize;
            }
            if ((parameters.Length > 0) && (sections.Length == 0))
            {
                left = parameters[0].intLeft;
                labelSize = parameters[0].intLabelSize;
            }
            if ((parameters.Length == 0) && (sections.Length > 0))
            {
                left = sections[0].intLeft;
            }

            //�������� �� ���� ����������� ���������� � ������������ ��
            if (labelSize > 0)
            {
                foreach (var parameterTemplateRow in parameters)
                {
                    parameterTemplateRow.intLabelSize = labelSize;
                }
            }

            if (left > 0)
            {
                foreach (var parameterTemplateRow in parameters)
                {
                    parameterTemplateRow.intLeft = left;
                }
                foreach (var sectionTemplateRow in sections)
                {
                    sectionTemplateRow.intLeft = left;
                }
                foreach (var labelsRow in labels)
                {
                    labelsRow.intLeft = left;
                }
            }
        }

        /// <summary>
        /// ���������� ��������� �� ��� ������ � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        public static bool IsSectionTable(this DbService mainDbService, long idfsSection)
        {
            var sectionsRow = GetSectionRow(mainDbService, idfsSection);
            return sectionsRow != null ? sectionsRow.blnGrid : false;
        }

        /// <summary>
        /// ����������, �������� �� ��������� ��� ������������ ��� ���������� ���� ����� (������� ������ ���� ��������� ������ ������������� ������ ��� ������� ���� �����)
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="determinantType"></param>
        /// <returns></returns>
        public static bool HasDeterminantType(this DbService mainDbService, FFDeterminantTypes determinantType)
        {
            var detType = (long)determinantType;
            return mainDbService.MainDataSet.DeterminantTypes.Select(FilterOr("idfsReferenceType", detType, "idfsGISReferenceType", detType)).Length > 0;
        }

        /// <summary>
        /// �������� ������������ � ��������� ����� ����� ������������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="determinantType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplateDeterminantValuesRow[] GetTemplateDeterminants(this DbService mainDbService, long idfsFormTemplate, FFDeterminantTypes determinantType)
        {
            return (FlexibleFormsDS.TemplateDeterminantValuesRow[])mainDbService.MainDataSet.TemplateDeterminantValues.Select(Filter("idfsFormTemplate", idfsFormTemplate, "DeterminantType", (long)determinantType));
        }

        /// <summary>
        /// �������� ��� ������������ ����������� ��������, ����������� � ������� ���� �����
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="ffType"></param>
        /// <returns></returns>
        public static FlexibleFormsDS.TemplateDeterminantValuesRow[] GetFFTypeDeterminants(this DbService mainDbService, long ffType)
        {
            return (FlexibleFormsDS.TemplateDeterminantValuesRow[])mainDbService.MainDataSet.TemplateDeterminantValues.Select(Filter("idfsFormType", ffType));
        }

        /// <summary>
        /// ��������� ������ ��� �������
        /// </summary>
        /// <returns></returns>
        public static string FilterOr(string columnName1, object value1, string columnName2, object value2, string columnName3, object value3)
        {
            return String.Format("({0}) Or ({1}) Or ({2})", Filter(columnName1, value1), Filter(columnName2, value2), Filter(columnName3, value3));
        }

        /// <summary>
        /// ��������� ������ ��� �������
        /// </summary>
        /// <returns></returns>
        public static string FilterOr(string columnName1, object value1, string columnName2, object value2)
        {
            return String.Format("({0}) Or ({1})", Filter(columnName1, value1), Filter(columnName2, value2));
        }

        /// <summary>
        /// �������� ��������� �� ����������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="rowRules"></param>
        /// <returns></returns>
        public static string GetRuleConstantsText(this DbService mainDbService, FlexibleFormsDS.RulesRow rowRules)
        {
            var result = new StringBuilder();

            var rfRows = (FlexibleFormsDS.RuleFunctionsRow[])mainDbService.MainDataSet.RuleFunctions.Select(Filter("idfsRuleFunction", rowRules.idfsRuleFunction));
            //��������, � �������� ��������� ������ �������
            var parameterRows = (FlexibleFormsDS.RuleParameterForFunctionRow[])mainDbService.MainDataSet.RuleParameterForFunction.Select(Filter("idfsRule", rowRules.idfsRule));

            var rows = (FlexibleFormsDS.RuleConstantRow[])mainDbService.MainDataSet.RuleConstant.Select(Filter("idfsRule", rowRules.idfsRule));
            foreach (var row in rows)
            {
                if (result.Length > 0) result.Append("; ");

                if (row.IsvarConstantNull() || (row.varConstant is long && (long)(row.varConstant) == -1L))
                {
                    result.Append(eidss.model.Resources.EidssMessages.Get("strEmpty"));
                    continue;
                }
                //���� �������� �� ��������, �� ���� �� ��� ������ �������� �����, � ���������
                if (rfRows.Length != 1) continue;
                //���� �������� ��������, �� ����� �������� ��� � ������������ ��������
                if (mainDbService.IsParameterNumeric(parameterRows[0].idfsParameter))
                {
                    row.varConstant = Convert.ToDecimal(row.varConstant, CultureInfo.InvariantCulture);
                }

                if (rfRows[0].idfsRuleFunction.Equals((long)FFRuleFunctions.Value))
                {
                    //���� ������ �������� �������� ���������� �������, �� ������ ��� ���������
                    if (mainDbService.IsParameterComboBox(parameterRows[0].idfsParameter))
                    {
                        var rowsValues =
                            (FlexibleFormsDS.ParameterSelectListRow[])
                            mainDbService.MainDataSet.ParameterSelectList.Select(Filter("idfsValue", row.varConstant));
                        if (rowsValues.Length > 0)
                            result.Append(rowsValues[0].strValueCaption);
                        else
                            result.Append(row.varConstant);
                    }
                    else
                    {
                        result.Append(row.varConstant);
                    }
                }
                else
                {
                    result.Append(row.varConstant);
                }
            }

            return result.ToString();
        }
    }
}
