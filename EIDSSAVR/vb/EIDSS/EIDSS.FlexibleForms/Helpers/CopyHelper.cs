using EIDSS.FlexibleForms.DataBase;

namespace EIDSS.FlexibleForms.Helpers
{
    public static class CopyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsObservationNew"></param>
        /// <param name="activityParametersRows"></param>
        public static void CopyActivityParameters(DbServiceUserData mainDbService, long idfsObservationNew, FlexibleFormsDS.ActivityParametersRow[] activityParametersRows)
        {
            foreach (var row in activityParametersRows)
            {
                if (!row.IsRowAlive()) continue;
                var rowResult = mainDbService.ChangeValue(idfsObservationNew, row.idfsFormTemplate, row.idfsParameter, row.idfRow, row.intNumRow, row.varValue, null);
                //�������� ��������
                if (rowResult != null) rowResult.intRowState = 0;
            }
        }

        /// <summary>
        /// ���������� ����������� ������ � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplateTarget"></param>
        /// <param name="idfsLanguage"></param>
        /// <param name="idfsFormTemplateSample"></param>
        public static void CopySectionTemplate(DbService mainDbService, long idfsFormTemplateSample, long idfsFormTemplateTarget, string idfsLanguage)
        {
            var sectionTemplateRows = mainDbService.GetSectionTemplateRows(null, idfsFormTemplateSample);

            foreach (FlexibleFormsDS.SectionTemplateRow row in sectionTemplateRows)
            {
                //���� ����� ������ ��� ������������ � ������� �������, �� �������� � �� ���������
                if (mainDbService.GetSectionTemplateRow(row.idfsSection, idfsFormTemplateTarget, idfsLanguage) != null) continue;
                
                var rowSectionTemplate = mainDbService.MainDataSet.SectionTemplate.NewSectionTemplateRow();
                
                rowSectionTemplate.idfsSection = row.idfsSection;
                if (!row.IsidfsParentSectionNull()) rowSectionTemplate.idfsParentSection = row.idfsParentSection;
                rowSectionTemplate.idfsFormType = row.idfsFormType;
                rowSectionTemplate.idfsFormTemplate = idfsFormTemplateTarget;
                rowSectionTemplate.langid = idfsLanguage.Length > 0 ? idfsLanguage : row.langid;
                rowSectionTemplate.blnFreeze = row.blnFreeze;
                rowSectionTemplate.blnGrid = row.blnGrid;
                rowSectionTemplate.blnFixedRowSet = row.blnFixedRowSet;
                rowSectionTemplate.DefaultName = row.DefaultName;
                rowSectionTemplate.NationalName = row.NationalName;
                rowSectionTemplate.intLeft = row.intLeft;
                rowSectionTemplate.intTop = row.intTop;
                rowSectionTemplate.intWidth = row.intWidth;
                rowSectionTemplate.intHeight = row.intHeight;
                rowSectionTemplate.IsStubLoaded = !row.IsIsStubLoadedNull() && row.IsStubLoaded;
                rowSectionTemplate.intOrder = row.intOrder;
                rowSectionTemplate.intCaptionHeight = !row.IsintCaptionHeightNull() ? row.intCaptionHeight : 23;
                mainDbService.MainDataSet.SectionTemplate.AddSectionTemplateRow(rowSectionTemplate);
            }
        }

        /// <summary>
        /// ���������� ����������� ������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplateTarget"></param>
        /// <param name="idfsLanguage"></param>
        /// <param name="idfsFormTemplateSample"></param>
        public static void CopyLabelTemplate(DbService mainDbService, long idfsFormTemplateSample, long idfsFormTemplateTarget, string idfsLanguage)
        {
            var labelsRows = mainDbService.GetLabelTemplateRows(idfsFormTemplateSample);

            foreach (var row in labelsRows)
            {
                //���� ��� ������������ � ������� �������, �� �������� �� ���������
                if (mainDbService.GetLabelTemplateRow(row.idfDecorElement, idfsFormTemplateTarget, idfsLanguage) != null) continue;

                var labelsRow = mainDbService.MainDataSet.Labels.NewLabelsRow();
                labelsRow.idfsDecorElementType = row.idfsDecorElementType;
                labelsRow.idfsBaseReference = row.idfsBaseReference;
                labelsRow.langid = idfsLanguage;
                labelsRow.idfsFormTemplate = idfsFormTemplateTarget;
                labelsRow.intLeft = row.intLeft;
                labelsRow.intTop = row.intTop;
                labelsRow.intWidth = row.intWidth;
                labelsRow.intHeight = row.intHeight;
                if (!row.IsColorNull()) labelsRow.Color = row.Color;
                if (!row.IsidfsSectionNull()) labelsRow.idfsSection = row.idfsSection;
                labelsRow.intColor = row.intColor;
                labelsRow.intFontStyle = row.intFontStyle;
                labelsRow.intFontSize = row.intFontSize;
                labelsRow.DefaultText = row.DefaultText;
                labelsRow.NationalText = row.NationalText;
                mainDbService.MainDataSet.Labels.AddLabelsRow(labelsRow);
            }
        }

        /// <summary>
        /// ���������� ����������� ����� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplateTarget"></param>
        /// <param name="idfsLanguage"></param>
        /// <param name="idfsFormTemplateSample"></param>
        public static void CopyLineTemplate(DbService mainDbService, long idfsFormTemplateSample, long idfsFormTemplateTarget, string idfsLanguage)
        {
            var linesRows = mainDbService.GetLineTemplateRows(idfsFormTemplateSample);

            foreach (var row in linesRows)
            {
                //���� ����� ������ ��� ������������ � ������� �������, �� �������� � �� ���������
                if (mainDbService.GetLineTemplateRow(row.idfDecorElement, idfsFormTemplateTarget, idfsLanguage) != null) continue;

                FlexibleFormsDS.LinesRow linesRow = mainDbService.MainDataSet.Lines.NewLinesRow();
                linesRow.langid = idfsLanguage;
                linesRow.idfsDecorElementType = row.idfsDecorElementType;
                linesRow.idfsFormTemplate = idfsFormTemplateTarget;
                if (!row.IsidfsSectionNull()) linesRow.idfsSection = row.idfsSection;
                linesRow.intLeft = row.intLeft;
                linesRow.intTop = row.intTop;
                linesRow.intWidth = row.intWidth;
                linesRow.intHeight = row.intHeight;
                if (!row.IsColorNull()) linesRow.Color = row.Color;
                linesRow.intColor = row.intColor;
                if (!row.IsblnOrientationNull())
                {
                    linesRow.blnOrientation = row.blnOrientation;
                }
                else
                {
                    linesRow.SetblnOrientationNull();
                }
                if (!row.IsintOrientationNull())
                {
                    linesRow.intOrientation = row.intOrientation;
                }
                else
                {
                    linesRow.SetintOrientationNull();
                }
                mainDbService.MainDataSet.Lines.AddLinesRow(linesRow);
            }
        }

        /// <summary>
        /// ���������� ����������� ���������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplateSample"></param>
        /// <param name="idfsFormTemplateTarget"></param>
        /// <param name="idfsLanguage"></param>
        public static void CopyParameterTemplate(DbService mainDbService, long idfsFormTemplateSample, long idfsFormTemplateTarget, string idfsLanguage)
        {
            var parameterTemplateRows = mainDbService.GetParameterTemplateRows(null, idfsFormTemplateSample);

            foreach (var row in parameterTemplateRows)
            {
                //���� ����� �������� ��� ������������ � ������� �������, �� �������� ��� �� ���������
                if (mainDbService.GetParameterTemplateRow(row.idfsParameter, idfsFormTemplateTarget, idfsLanguage) != null) continue;
                
                FlexibleFormsDS.ParameterTemplateRow rowParameterTemplate = mainDbService.MainDataSet.ParameterTemplate.NewParameterTemplateRow();

                rowParameterTemplate.idfsParameter = row.idfsParameter;
                if (!row.IsidfsSectionNull())
                    rowParameterTemplate.idfsSection = row.idfsSection;
                rowParameterTemplate.idfsFormType = row.idfsFormType;
                rowParameterTemplate.idfsFormTemplate = idfsFormTemplateTarget;
                rowParameterTemplate.langid = idfsLanguage.Length > 0 ? idfsLanguage : row.langid;
                rowParameterTemplate.idfsEditor = row.idfsEditor;
                rowParameterTemplate.intScheme = row.intScheme;
                rowParameterTemplate.intLabelSize = row.intLabelSize;
                rowParameterTemplate.DefaultName = row.DefaultName;
                rowParameterTemplate.NationalName = row.NationalName;
                rowParameterTemplate.intLeft = row.intLeft;
                rowParameterTemplate.intTop = row.intTop;
                rowParameterTemplate.intWidth = row.intWidth;
                rowParameterTemplate.intHeight = row.intHeight;
                rowParameterTemplate.intHACode = row.intHACode;
                rowParameterTemplate.idfsParameterType = row.idfsParameterType;
                rowParameterTemplate.blnFreeze = row.blnFreeze;
                rowParameterTemplate.idfsEditMode = row.idfsEditMode;
                rowParameterTemplate.intOrder = row.intOrder;
                mainDbService.MainDataSet.ParameterTemplate.AddParameterTemplateRow(rowParameterTemplate);
            }
        }

        /// <summary>
        /// ���������� ����������� ����������� �������� � �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplate"> </param>
        /// <param name="idfsLanguageTo"> </param>
        public static void RestoreLanguageLayout(DbService mainDbService, long idfsFormTemplate, string idfsLanguageTo)
        {
            //�������� �� ����������� ���� � ������������
            //������ � ������������ ���� ������ ����
            string idfsLanguageFrom = bv.common.Core.Localizer.lngEn;
            var parameterTemplateRows = mainDbService.GetParameterTemplateRows(null, idfsFormTemplate, idfsLanguageFrom);

            foreach (var row in parameterTemplateRows)
            {
                //���� ����� �������� ��� ������������ � ������� �������, �� �������� ��� �� ���������
                var targetRow = (mainDbService.GetParameterTemplateRow(row.idfsParameter, idfsFormTemplate, idfsLanguageTo));
                if (targetRow == null) continue;
                targetRow.intScheme = row.intScheme;
                targetRow.intLabelSize = row.intLabelSize;
                targetRow.intLeft = row.intLeft;
                targetRow.intTop = row.intTop;
                targetRow.intWidth = row.intWidth;
                targetRow.intHeight = row.intHeight;
            }

            var sectionTemplateRows = mainDbService.GetSectionTemplateRows(null, idfsFormTemplate, idfsLanguageFrom);

            foreach (var row in sectionTemplateRows)
            {
                var targetRow = mainDbService.GetSectionTemplateRow(row.idfsSection, idfsFormTemplate, idfsLanguageTo);
                if (targetRow == null) continue;
                targetRow.intLeft = row.intLeft;
                targetRow.intTop = row.intTop;
                targetRow.intWidth = row.intWidth;
                targetRow.intHeight = row.intHeight;
            }

            var labelsRows = mainDbService.GetLabelTemplateRows(idfsFormTemplate, idfsLanguageFrom);

            foreach (var row in labelsRows)
            {
                //���� ��� ������������ � ������� �������, �� �������� �� ���������
                var targetRow = (mainDbService.GetLabelTemplateRow(row.idfDecorElement, idfsFormTemplate, idfsLanguageTo));
                if (targetRow == null) continue;
                targetRow.intLeft = row.intLeft;
                targetRow.intTop = row.intTop;
                targetRow.intWidth = row.intWidth;
                targetRow.intHeight = row.intHeight;
                if (!row.IsColorNull()) targetRow.Color = row.Color;
                targetRow.intColor = row.intColor;
                targetRow.intFontStyle = row.intFontStyle;
                targetRow.intFontSize = row.intFontSize;
            }

            //����� � ��� ����� ���
        }

        /// <summary>
        /// ���������� ����������� ������������� �� ������ ������� � ������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="idfsFormTemplateTarget"></param>
        /// <param name="idfsFormTemplateSample"></param>
        public static void CopyDeterminants(DbService mainDbService, long idfsFormTemplateSample, long idfsFormTemplateTarget)
        {
            //������������ ����������� � ��� ������������ �� ������� �������

            var determinantValuesRows = (FlexibleFormsDS.TemplateDeterminantValuesRow[])mainDbService.MainDataSet.TemplateDeterminantValues.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplateSample));

            foreach (var rowDeterminant in determinantValuesRows)
            {
                var row = mainDbService.MainDataSet.TemplateDeterminantValues.NewTemplateDeterminantValuesRow();
                row.idfsFormTemplate = idfsFormTemplateTarget;
                row.idfsFormType = rowDeterminant.idfsFormType;
                row.DeterminantValue = rowDeterminant.DeterminantValue;
                row.DeterminantDefaultName = rowDeterminant.DeterminantDefaultName;
                row.DeterminantNationalName = rowDeterminant.DeterminantNationalName;
                if (!rowDeterminant.IsidfsBaseReferenceNull()) row.idfsBaseReference = rowDeterminant.idfsBaseReference;
                if (!rowDeterminant.IsidfsGISBaseReferenceNull()) row.idfsGISBaseReference = rowDeterminant.idfsGISBaseReference;
                row.DeterminantType = rowDeterminant.DeterminantType;
                row.DeterminantTypeDefaultName = rowDeterminant.DeterminantTypeDefaultName;
                row.DeterminantTypeNationalName = rowDeterminant.DeterminantTypeNationalName;

                mainDbService.MainDataSet.TemplateDeterminantValues.AddTemplateDeterminantValuesRow(row);
            }
        }

        /// <summary>
        /// ����������� �������
        /// </summary>
        /// <param name="mainDbService"></param>
        /// <param name="template"></param>
        /// <param name="idfsFormType"></param>
        /// <param name="needTransferUNI">����� �� ���������� UNI � ������</param>
        public static FlexibleFormsDS.TemplatesRow CopyTemplate(DbService mainDbService, FlexibleFormsDS.TemplatesRow template, long idfsFormType, bool needTransferUNI)
        {
            #region ����� �������

            FlexibleFormsDS.TemplatesRow rowTemplate = mainDbService.MainDataSet.Templates.NewTemplatesRow();
            rowTemplate.idfsFormType = idfsFormType;
            rowTemplate.blnUNI = needTransferUNI && template.blnUNI;
            rowTemplate.DefaultName = template.DefaultName + " (New)";
            rowTemplate.NationalName = template.NationalName + " (New)"; //TODO ������� ������������ �������
            rowTemplate.NationalLongName = template.NationalLongName;
            rowTemplate.langid = template.langid;
            if (!template.IsstrNoteNull()) rowTemplate.strNote = template.strNote;

            mainDbService.MainDataSet.Templates.AddTemplatesRow(rowTemplate);

            #endregion

            //����� ������ � �������
            CopySectionTemplate(mainDbService, template.idfsFormTemplate, rowTemplate.idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);

            //����� ���������� � �������
            CopyParameterTemplate(mainDbService, template.idfsFormTemplate, rowTemplate.idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);

            //����� ������� � �������
            CopyLabelTemplate(mainDbService, template.idfsFormTemplate, rowTemplate.idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);

            //����� ����� � �������
            CopyLineTemplate(mainDbService, template.idfsFormTemplate, rowTemplate.idfsFormTemplate, bv.model.Model.Core.ModelUserContext.CurrentLanguage);

            #region ����� ������ � �������

            var rulesRows = mainDbService.GetRules(template.idfsFormTemplate);
            foreach (var row in rulesRows)
            {
                if (!row.IsRowAlive()) continue;
                var rowRules = mainDbService.MainDataSet.Rules.NewRulesRow();
                rowRules.langid = row.langid;
                rowRules.idfsFormTemplate = rowTemplate.idfsFormTemplate;
                rowRules.DefaultName = row.DefaultName;
                rowRules.NationalName = row.NationalName;
                rowRules.idfsRuleFunction = row.idfsRuleFunction;
                if (!row.IsMessageTextNull()) rowRules.MessageText = row.MessageText;
                if (!row.IsMessageNationalTextNull()) rowRules.MessageNationalText = row.MessageNationalText;
                if (!row.IsidfsCheckPointDescrNull()) rowRules.idfsCheckPointDescr = row.idfsCheckPointDescr;
                rowRules.idfsCheckPoint = row.idfsCheckPoint;
                if (!row.IsidfsRuleMessageNull()) rowRules.idfsRuleMessage = row.idfsRuleMessage;
                rowRules.blnNot = row.blnNot;
                rowRules.intNumberOfParameters = row.intNumberOfParameters;
                if (!row.IsFunctionTextNull()) rowRules.FunctionText = row.FunctionText;

                mainDbService.MainDataSet.Rules.AddRulesRow(rowRules);

                string filterForRules = DataHelper.Filter("idfsRule", row.idfsRule);

                #region ����� ���������� ������� ��� ������ ������ �������

                var ruleParameterForFunctionRows = (FlexibleFormsDS.RuleParameterForFunctionRow[])mainDbService.MainDataSet.RuleParameterForFunction.Select(filterForRules);

                foreach (var rowParam in ruleParameterForFunctionRows)
                {
                    if (!rowParam.IsRowAlive()) continue;
                    var rowRuleParameterForFunctionRow = mainDbService.MainDataSet.RuleParameterForFunction.NewRuleParameterForFunctionRow();
                    rowRuleParameterForFunctionRow.idfsFormTemplate = rowTemplate.idfsFormTemplate;
                    rowRuleParameterForFunctionRow.idfsRule = rowRules.idfsRule;
                    rowRuleParameterForFunctionRow.intOrder = rowParam.intOrder;
                    rowRuleParameterForFunctionRow.idfsParameter = rowParam.idfsParameter;
                    rowRuleParameterForFunctionRow.idfsParameterType = rowParam.idfsParameterType;
                    mainDbService.MainDataSet.RuleParameterForFunction.AddRuleParameterForFunctionRow(rowRuleParameterForFunctionRow);
                }

                #endregion

                #region ����� ������������� �������� ��� ������ ������ �������

                var ruleConstantRows =
                    (FlexibleFormsDS.RuleConstantRow[])
                    mainDbService.MainDataSet.RuleConstant.Select(filterForRules);

                foreach (var rowConstants in ruleConstantRows)
                {
                    if (!rowConstants.IsRowAlive()) continue;
                    var rowConstant = mainDbService.MainDataSet.RuleConstant.NewRuleConstantRow();
                    rowConstant.idfsRule = rowRules.idfsRule;
                    rowConstant.varConstant = rowConstants.varConstant;
                    mainDbService.MainDataSet.RuleConstant.AddRuleConstantRow(rowConstant);
                }

                #endregion

                #region ����� �������� ��� ������ ������ �������

                var ruleParameterForActionRows =
                    (FlexibleFormsDS.RuleParameterForActionRow[])
                    mainDbService.MainDataSet.RuleParameterForAction.Select(filterForRules);

                foreach (var rowParam in ruleParameterForActionRows)
                {
                    if (!rowParam.IsRowAlive()) continue;
                    var rowParameterForAction = mainDbService.MainDataSet.RuleParameterForAction.NewRuleParameterForActionRow();
                    rowParameterForAction.idfsFormTemplate = rowTemplate.idfsFormTemplate;
                    rowParameterForAction.idfsRule = rowRules.idfsRule;
                    rowParameterForAction.idfsParameter = rowParam.idfsParameter;
                    rowParameterForAction.idfsRuleAction = rowParam.idfsRuleAction;
                    mainDbService.MainDataSet.RuleParameterForAction.AddRuleParameterForActionRow(rowParameterForAction);
                }

                #endregion
            }

            #endregion

            return rowTemplate;
        }

    }
}
