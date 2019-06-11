using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using bv.common;
using bv.common.Core;
using bv.common.Enums;
using bv.common.db;
using EIDSS.FlexibleForms.DataBase.FlexibleFormsDSTableAdapters;
using EIDSS.FlexibleForms.Helpers;
using bv.common.db.Core;
using eidss.model.Enums;
using bv.model.Model.Core;
using FFRuleFunctions = bv.common.FFRuleFunctions;

namespace EIDSS.FlexibleForms.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class DbService : BaseDbService
    {
        /// <summary>
        /// �������� �������
        /// </summary>
        public FlexibleFormsDS MainDataSet { get; set; }

        private SqlCommand m_CommandDeleteParameter;
        private SqlCommand m_CommandDeleteSection;
        private SqlCommand m_CommandDeleteParameterTemplate;
        private SqlCommand m_CommandDeleteSectionTemplate;
        private SqlCommand m_CommandDeleteTemplates;
        private SqlCommand m_CommandDeleteRules;
        private SqlCommand m_CommandDeleteRuleParameterForFunction;
        private SqlCommand m_CommandDeleteRuleParameterForAction;
        private SqlCommand m_CommandDeleteRuleConstants;
        private SqlCommand m_CommandDeleteLines;
        private SqlCommand m_CommandDeleteTemplateDeterminants;
        private SqlCommand m_CommandDeleteLabels;

        private SqlCommand m_CommandSaveParameters;
        private SqlCommand m_CommandSaveParameterTemplate;
        private SqlCommand m_CommandSaveSections;
        private SqlCommand m_CommandSaveSectionTemplate;
        private SqlCommand m_CommandSaveTemplates;
        private SqlCommand m_CommandSaveRules;
        private SqlCommand m_CommandSaveRuleParameterForFunction;
        private SqlCommand m_CommandSaveRuleConstants;
        private SqlCommand m_CommandSaveRuleParameterForAction;
        private SqlCommand m_CommandSaveLines;
        private SqlCommand m_CommandSaveTemplateDeterminants;
        private SqlCommand m_CommandSaveLabels;

        private FlexibleFormsDS.TemplateDeterminantValuesDataTable TemplateDeterminantsBackup { get; set; }

        #region ��������

        /// <summary>
        /// ������� ��� FormTypes
        /// </summary>
        private FormTypesTableAdapter m_FormTypesTableAdapter;

        /// <summary>
        /// ������� ��� HACodes
        /// </summary>
        private HACodeListTableAdapter m_HACodeListTableAdapter;

        /// <summary>
        /// ������� ��� ����� ���������� ��� ����������
        /// </summary>
        private ParameterEditorsTableAdapter m_ParameterControlTypesEditorsTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ������ ������, ��������� ��� ������� ���������
        /// </summary>
        private ParameterSelectListTableAdapter m_ParameterSelectListTableAdapter;

        /// <summary>
        /// ������� ��� ���������� � ������ ������/��������� �������
        /// </summary>
        private ParametersSearchTableAdapter m_ParametersSearchTableAdapter;

        /// <summary>
        /// ������� ��� Parameters
        /// </summary>
        private ParametersTableAdapter m_ParametersTableAdapter;

        /// <summary>
        /// ������� ��� ���������� � �������
        /// </summary>
        private ParameterTemplateTableAdapter m_ParameterTemplateTableAdapter;

        /// <summary>
        /// ������� ��� Sections
        /// </summary>
        private SectionsTableAdapter m_SectionsTableAdapter;

        /// <summary>
        /// ������� ��� ������ � �������
        /// </summary>
        private SectionTemplateTableAdapter m_SectionTemplateTableAdapter;

        /// <summary>
        /// ������� ��� ������������� ��������
        /// </summary>
        private TemplateDeterminantValuesTableAdapter m_TemplateDeterminantValuesTableAdapter;

        /// <summary>
        /// ������� ��� ������� ��������, ��� ������������ ��������� ��������
        /// </summary>
        private TemplatesByParameterTableAdapter m_TemplatesByParameterTableAdapter;

        /// <summary>
        /// ������� ��� ��������
        /// </summary>
        private TemplatesTableAdapter m_TemplatesTableAdapter;
        
        /// <summary>
        /// ������� ��� ������
        /// </summary>
        private RulesTableAdapter m_RulesTableAdapter;

        /// <summary>
        /// �������� ������� ��� ������
        /// </summary>
        private RuleFunctionsTableAdapter m_RuleFunctionsTableAdapter;

        /// <summary>
        /// ������� ��� ���������� � �������, ������� � �������
        /// </summary>
        private RuleParameterForFunctionTableAdapter m_RuleParameterForFunctionTableAdapter;

        /// <summary>
        /// ������� ��� �������� ������� � �������
        /// </summary>
        private RuleConstantTableAdapter m_RuleConstantTableAdapter;

        /// <summary>
        /// ������� ��� ��������, ��������� � ��������
        /// </summary>
        private RuleParameterForActionTableAdapter m_RuleParameterForActionTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ���������� �� ������������� ������ � ��������
        /// </summary>
        private SectionUsedTableAdapter m_SectionUsedTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ���������� �� ������������� ��������� � ��������
        /// </summary>
        private ParameterUsedTableAdapter m_ParameterUsedTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ���������� � ������ � ��������
        /// </summary>
        private LinesTableAdapter m_LinesTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ������ �������������
        /// </summary>
        private DeterminantsTableAdapter m_DeterminantsTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ������ � ����������������� ���������
        /// </summary>
        private PredefinedStubTableAdapter m_PredefinedStubTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ������� ����������, ������� ���� ������� �� �������, �� �� ������� �������� ������
        /// </summary>
        private ParametersDeletedFromTemplateTableAdapter m_ParametersDeletedFromTemplateTableAdapter;

        /// <summary>
        /// ������� ��� ��������� ������ �������
        /// </summary>
        private AggregateGetMatrixVersionTableAdapter m_AggregateGetMatrixVersionTableAdapter;

        /// <summary>
        /// ������� ��� ��������� �������  observation
        /// </summary>
        private ObservationsTableAdapter m_ObservationsTableAdapter;

        /// <summary>
        /// ������� ��� ��������� �������
        /// </summary>
        private LabelsTableAdapter m_LablesTableAdapter;

        /// <summary>
        /// ���� �������������
        /// </summary>
        private DeterminantTypesTableAdapter m_DeterminantTypesTableAdapter;

        private ActualTemplateTableAdapter m_ActualTemplateTableAdapter;

        #endregion

        protected virtual void Init(SqlConnection connection)
        {
            ObjectName = "FlexibleForms";

            MainDataSet = new FlexibleFormsDS();

            #region ������������� ���������

            m_FormTypesTableAdapter = new FormTypesTableAdapter();
            m_SectionsTableAdapter = new SectionsTableAdapter();
            m_ParametersTableAdapter = new ParametersTableAdapter();
            m_HACodeListTableAdapter = new HACodeListTableAdapter();
            m_TemplatesTableAdapter = new TemplatesTableAdapter();
            m_TemplateDeterminantValuesTableAdapter = new TemplateDeterminantValuesTableAdapter();
            m_ParameterControlTypesEditorsTableAdapter = new ParameterEditorsTableAdapter();
            m_TemplatesByParameterTableAdapter = new TemplatesByParameterTableAdapter();
            m_ParametersSearchTableAdapter = new ParametersSearchTableAdapter();
            m_SectionTemplateTableAdapter = new SectionTemplateTableAdapter();
            m_ParameterTemplateTableAdapter = new ParameterTemplateTableAdapter();
            m_ParameterSelectListTableAdapter = new ParameterSelectListTableAdapter();
            m_RulesTableAdapter = new RulesTableAdapter();
            m_RuleFunctionsTableAdapter = new RuleFunctionsTableAdapter();
            m_RuleParameterForFunctionTableAdapter = new RuleParameterForFunctionTableAdapter();
            m_RuleConstantTableAdapter = new RuleConstantTableAdapter();
            m_RuleParameterForActionTableAdapter = new RuleParameterForActionTableAdapter();
            m_SectionUsedTableAdapter = new SectionUsedTableAdapter();
            m_ParameterUsedTableAdapter = new ParameterUsedTableAdapter();
            m_LinesTableAdapter = new LinesTableAdapter();
            m_DeterminantsTableAdapter = new DeterminantsTableAdapter();
            m_PredefinedStubTableAdapter = new PredefinedStubTableAdapter();
            m_ParametersDeletedFromTemplateTableAdapter = new ParametersDeletedFromTemplateTableAdapter();
            m_AggregateGetMatrixVersionTableAdapter = new AggregateGetMatrixVersionTableAdapter();
            m_ObservationsTableAdapter = new ObservationsTableAdapter();
            m_LablesTableAdapter = new LabelsTableAdapter();
            m_DeterminantTypesTableAdapter = new DeterminantTypesTableAdapter();
            m_ActualTemplateTableAdapter = new ActualTemplateTableAdapter();
           
            m_FormTypesTableAdapter.Connection =
                m_SectionsTableAdapter.Connection =
                m_ParametersTableAdapter.Connection =
                m_HACodeListTableAdapter.Connection =
                m_TemplatesTableAdapter.Connection =
                m_TemplateDeterminantValuesTableAdapter.Connection =
                m_ParameterControlTypesEditorsTableAdapter.Connection =
                m_TemplatesByParameterTableAdapter.Connection =
                m_ParametersSearchTableAdapter.Connection =
                m_SectionTemplateTableAdapter.Connection =
                m_ParameterTemplateTableAdapter.Connection =
                m_ParameterSelectListTableAdapter.Connection =
                m_RulesTableAdapter.Connection =
                m_RuleFunctionsTableAdapter.Connection =
                m_RuleParameterForFunctionTableAdapter.Connection =
                m_RuleConstantTableAdapter.Connection =
                m_RuleParameterForActionTableAdapter.Connection =
                m_SectionUsedTableAdapter.Connection =
                m_ParameterUsedTableAdapter.Connection =
                m_LinesTableAdapter.Connection =
                m_DeterminantsTableAdapter.Connection = 
                m_PredefinedStubTableAdapter.Connection =
                m_ParametersDeletedFromTemplateTableAdapter.Connection =
                m_AggregateGetMatrixVersionTableAdapter.Connection =
                m_ObservationsTableAdapter.Connection =
                m_LablesTableAdapter.Connection =
                m_DeterminantTypesTableAdapter.Connection =
                m_ActualTemplateTableAdapter.Connection =
                connection;//(SqlConnection) Connection;

            //��������� ������� ����� �����������
            //��� ����������� (�������������) ������ ���������� �� ����� ������ ����������
            m_SectionsTableAdapter.ClearBeforeFill =
                m_ParametersTableAdapter.ClearBeforeFill =
                m_TemplatesTableAdapter.ClearBeforeFill =
                m_TemplateDeterminantValuesTableAdapter.ClearBeforeFill =
                m_SectionTemplateTableAdapter.ClearBeforeFill =
                m_ParameterTemplateTableAdapter.ClearBeforeFill =
                m_ParameterSelectListTableAdapter.ClearBeforeFill =
                m_RulesTableAdapter.ClearBeforeFill =
                m_RuleParameterForFunctionTableAdapter.ClearBeforeFill =
                m_RuleConstantTableAdapter.ClearBeforeFill =
                m_RuleParameterForActionTableAdapter.ClearBeforeFill=
                m_LinesTableAdapter.ClearBeforeFill =
                m_LablesTableAdapter.ClearBeforeFill
                = false;

            #endregion

            //��� summary
            IdfObservationSummary = 0;
            IdfsFormTemplateSummary = 0;
            IdfObservationGroup = 1;

            UseDatasetCopyInPost = false;

            TemplateDeterminantsBackup = new FlexibleFormsDS.TemplateDeterminantValuesDataTable();
        }

        /// <summary>
        /// ID ���������� Observation, ������� �������� ������������� ������
        /// </summary>
        internal long IdfObservationSummary { get; private set; }

        /// <summary>
        /// ID ���������� Form Template, ������� �������� ������ (merged) ��������� �� ���� Observation, ������� ������ � ������� �����
        /// </summary>
        internal long IdfsFormTemplateSummary { get; private set; }

        /// <summary>
        /// ID ��������� ���������� ��� ���������� ��������������
        /// </summary>
        internal long IdfObservationGroup { get; private set; }

        /// <summary>
        /// �����������
        /// </summary>
        public DbService()
        {
            Init((SqlConnection) Connection);
        }

        /// <summary>
        /// �����������
        /// </summary>
        public DbService(SqlConnection connection)
        {
            Init(connection);
        }

        /// <summary>
        /// ������� ��� ���������� ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveSections
        {
            get
            {
                if (m_CommandSaveSections == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveSections = new SqlCommand
                                                {
                                                    CommandText = "dbo.spFFSaveSections",
                                                    CommandType = CommandType.StoredProcedure,
                                                    Connection = (SqlConnection) Connection
                                                };

                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsSection", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@idfsParentSection", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsParentSection", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@idfsFormType", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormType", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@DefaultName", SqlDbType.NVarChar, 400, ParameterDirection.Input, 0, 0, "DefaultName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@NationalName", SqlDbType.NVarChar, 600, ParameterDirection.Input, 0, 0, "NationalName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@intOrder", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intOrder", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@blnGrid", SqlDbType.Bit, 1, ParameterDirection.Input, 3, 0, "blnGrid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@blnFixedRowset", SqlDbType.Bit, 1, ParameterDirection.Input, 3, 0, "blnFixedRowset", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSections.Parameters.Add(new SqlParameter("@idfsMatrixType", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsMatrixType", DataRowVersion.Current, false, null, "", "", ""));
                    
                    #endregion
                }
                return m_CommandSaveSections;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ���������� ������� � ������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveRuleParameterForFunction
        {
            get
            {
                if (m_CommandSaveRuleParameterForFunction == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveRuleParameterForFunction = new SqlCommand
                                                                {
                                                                    CommandText = "dbo.spFFSaveRuleParameterForFunction",
                                                                    CommandType = CommandType.StoredProcedure,
                                                                    Connection = (SqlConnection) Connection
                                                                };

                    m_CommandSaveRuleParameterForFunction.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForFunction.Parameters.Add(new SqlParameter("@idfParameterForFunction", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfParameterForFunction", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForFunction.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForFunction.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForFunction.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForFunction.Parameters.Add(new SqlParameter("@intOrder", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intOrder", DataRowVersion.Current, false, null, "", "", ""));
        
                    #endregion
                }
                return m_CommandSaveRuleParameterForFunction;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ������������� � ������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveTemplateDeterminants
        {
            get
            {
                if (m_CommandSaveTemplateDeterminants == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveTemplateDeterminants = new SqlCommand
                                                            {
                                                                CommandText = "dbo.spFFSaveTemplateDeterminantValues",
                                                                CommandType = CommandType.StoredProcedure,
                                                                Connection = (SqlConnection) Connection
                                                            };

                    m_CommandSaveTemplateDeterminants.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplateDeterminants.Parameters.Add(new SqlParameter("@idfDeterminantValue", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfDeterminantValue", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplateDeterminants.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplateDeterminants.Parameters.Add(new SqlParameter("@idfsBaseReference", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsBaseReference", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplateDeterminants.Parameters.Add(new SqlParameter("@idfsGISBaseReference", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsGISBaseReference", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandSaveTemplateDeterminants;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ������������� �������� � ������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveRuleConstants
        {
            get
            {
                if (m_CommandSaveRuleConstants == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveRuleConstants = new SqlCommand
                                                     {
                                                         CommandText = "dbo.spFFSaveRuleConstant",
                                                         CommandType = CommandType.StoredProcedure,
                                                         Connection = (SqlConnection) Connection
                                                     };

                    m_CommandSaveRuleConstants.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleConstants.Parameters.Add(new SqlParameter("@idfRuleConstant", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfRuleConstant", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleConstants.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleConstants.Parameters.Add(new SqlParameter("@varConstant", SqlDbType.Variant, 8016, ParameterDirection.Input, 0, 0, "varConstant", DataRowVersion.Current, false, null, "", "", ""));
        
                    #endregion
                }
                return m_CommandSaveRuleConstants;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ������������� �������� � ������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveLines
        {
            get
            {
                if (m_CommandSaveLines == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveLines = new SqlCommand
                                             {
                                                 CommandText = "dbo.spFFSaveLine",
                                                 CommandType = CommandType.StoredProcedure,
                                                 Connection = (SqlConnection) Connection
                                             };

                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@idfDecorElement", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfDecorElement", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@idfsDecorElementType", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsDecorElementType", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsSection", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@intLeft", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLeft", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@intTop", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intTop", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@intWidth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intWidth", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@intHeight", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intHeight", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@intColor", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intColor", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLines.Parameters.Add(new SqlParameter("@blnOrientation", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "blnOrientation", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandSaveLines;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ������������� �������� � ������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveLabels
        {
            get
            {
                if (m_CommandSaveLabels == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveLabels = new SqlCommand
                                              {
                                                  CommandText = "dbo.spFFSaveLabel",
                                                  CommandType = CommandType.StoredProcedure,
                                                  Connection = (SqlConnection) Connection
                                              };

                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@idfDecorElement", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfDecorElement", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@idfsBaseReference", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsBaseReference", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsSection", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intLeft", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLeft", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intTop", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intTop", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intWidth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intWidth", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intHeight", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intHeight", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intFontStyle", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intFontStyle", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intFontSize", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intFontSize", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@intColor", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intColor", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@DefaultText", SqlDbType.VarChar, 200, ParameterDirection.Input, 0, 0, "DefaultText", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveLabels.Parameters.Add(new SqlParameter("@NationalText", SqlDbType.NVarChar, 400, ParameterDirection.Input, 0, 0, "NationalText", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandSaveLabels;
            }
        }

        /// <summary>
        /// ������� ��� ���������� �������� � ������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveRuleParameterForAction
        {
            get
            {
                if (m_CommandSaveRuleParameterForAction == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveRuleParameterForAction = new SqlCommand
                                                              {
                                                                  CommandText = "dbo.spFFSaveRuleParameterForAction",
                                                                  CommandType = CommandType.StoredProcedure,
                                                                  Connection = (SqlConnection) Connection
                                                              };

                    m_CommandSaveRuleParameterForAction.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForAction.Parameters.Add(new SqlParameter("@idfParameterForAction", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 0, 0, "idfParameterForAction", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForAction.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForAction.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForAction.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 86, ParameterDirection.Input, 0, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRuleParameterForAction.Parameters.Add(new SqlParameter("@idfsRuleAction", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsRuleAction", DataRowVersion.Current, false, null, "", "", ""));
        
                    #endregion
                }
                return m_CommandSaveRuleParameterForAction;
            }
        }


        /// <summary>
        /// ������� ��� ���������� �������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveTemplates
        {
            get
            {
                if (m_CommandSaveTemplates == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveTemplates = new SqlCommand
                                                 {
                                                     CommandText = "dbo.spFFSaveTemplate",
                                                     CommandType = CommandType.StoredProcedure,
                                                     Connection = (SqlConnection) Connection
                                                 };

                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@idfsFormType", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormType", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@DefaultName", SqlDbType.NVarChar, 400, ParameterDirection.Input, 0, 0, "DefaultName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@NationalName", SqlDbType.NVarChar, 600, ParameterDirection.Input, 0, 0, "NationalName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@strNote", SqlDbType.NVarChar, 200, ParameterDirection.Input, 0, 0, "strNote", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveTemplates.Parameters.Add(new SqlParameter("@blnUNI", SqlDbType.Bit, 1, ParameterDirection.Input, 0, 0, "blnUNI", DataRowVersion.Current, false, null, "", "", ""));
        
                    #endregion
                }
                return m_CommandSaveTemplates;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveRules
        {
            get
            {
                if (m_CommandSaveRules == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveRules = new SqlCommand
                                             {
                                                 CommandText = "dbo.spFFSaveRules",
                                                 CommandType = CommandType.StoredProcedure,
                                                 Connection = (SqlConnection) Connection
                                             };

                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@idfsRuleMessage", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsRuleMessage", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@idfsCheckPoint", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsCheckPoint", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@idfsRuleFunction", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsRuleFunction", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@DefaultName", SqlDbType.NVarChar, 200, ParameterDirection.Input, 0, 0, "DefaultName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@NationalName", SqlDbType.NVarChar, 300, ParameterDirection.Input, 0, 0, "NationalName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@MessageText", SqlDbType.NVarChar, 200, ParameterDirection.Input, 0, 0, "MessageText", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@MessageNationalText", SqlDbType.NVarChar, 300, ParameterDirection.Input, 0, 0, "MessageNationalText", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@blnNot", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "blnNot", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveRules.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    
                    #endregion
                }
                return m_CommandSaveRules;
            }
        }

        /// <summary>
        /// ������� ��� �������� �������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandDeleteTemplates
        {
            get
            {
                if (m_CommandDeleteTemplates == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandDeleteTemplates = new SqlCommand
                                                   {
                                                       CommandText = "dbo.spFFRemoveTemplate",
                                                       CommandType = CommandType.StoredProcedure,
                                                       Connection = (SqlConnection) Connection
                                                   };

                    m_CommandDeleteTemplates.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteTemplates.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteTemplates;
            }
        }

        /// <summary>
        /// ������� ��� �������� ���������� ������� � ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandDeleteRuleParameterForFunction
        {
            get
            {
                if (m_CommandDeleteRuleParameterForFunction == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandDeleteRuleParameterForFunction = new SqlCommand
                                                                  {
                                                                      CommandText =
                                                                          "dbo.spFFRemoveRuleParameterForFunction",
                                                                      CommandType = CommandType.StoredProcedure,
                                                                      Connection = (SqlConnection) Connection
                                                                  };
                    m_CommandDeleteRuleParameterForFunction.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteRuleParameterForFunction.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteRuleParameterForFunction.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteRuleParameterForFunction;
            }
        }

        /// <summary>
        /// ������� ��� �������� �������� � ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandDeleteRuleParameterForAction
        {
            get
            {
                if (m_CommandDeleteRuleParameterForAction == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandDeleteRuleParameterForAction = new SqlCommand
                                                                {
                                                                    CommandText = "dbo.spFFRemoveRuleParameterForAction",
                                                                    CommandType = CommandType.StoredProcedure,
                                                                    Connection = (SqlConnection) Connection
                                                                };
                    m_CommandDeleteRuleParameterForAction.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteRuleParameterForAction.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteRuleParameterForAction.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteRuleParameterForAction;
            }
        }

        /// <summary>
        /// ������� ��� �������� �������� � ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandDeleteRuleConstants
        {
            get
            {
                if (m_CommandDeleteRuleConstants == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandDeleteRuleConstants = new SqlCommand
                                                       {
                                                           CommandText = "dbo.spFFRemoveRuleConstant",
                                                           CommandType = CommandType.StoredProcedure,
                                                           Connection = (SqlConnection) Connection
                                                       };
                    m_CommandDeleteRuleConstants.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteRuleConstants.Parameters.Add(new SqlParameter("@idfRuleConstant", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfRuleConstant", DataRowVersion.Current, false, null, "", "", ""));
                    
                    #endregion
                }
                return m_CommandDeleteRuleConstants;
            }
        }

        /// <summary>
        /// ������� ��� �������� ������
        /// </summary>
        private SqlCommand CommandDeleteRules
        {
            get
            {
                if (m_CommandDeleteRules == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteRules = new SqlCommand
                                               {
                                                   CommandText = "dbo.spFFRemoveRule",
                                                   CommandType = CommandType.StoredProcedure,
                                                   Connection = (SqlConnection) Connection
                                               };
                    m_CommandDeleteRules.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteRules.Parameters.Add(new SqlParameter("@idfsRule", SqlDbType.BigInt, 8, ParameterDirection.Input, 0, 0, "idfsRule", DataRowVersion.Current, false, null, "", "", ""));

                    #endregion
                }
                return m_CommandDeleteRules;
            }
        }

        /// <summary>
        /// ������� ��� �������� �������������
        /// </summary>
        private SqlCommand CommandDeleteTemplateDeterminants
        {
            get
            {
                if (m_CommandDeleteTemplateDeterminants == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteTemplateDeterminants = new SqlCommand
                                                              {
                                                                  CommandText =
                                                                      "dbo.spFFRemoveTemplateDeterminantValues",
                                                                  CommandType = CommandType.StoredProcedure,
                                                                  Connection = (SqlConnection) Connection
                                                              };

                    m_CommandDeleteTemplateDeterminants.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteTemplateDeterminants.Parameters.Add(new SqlParameter("@idfDeterminantValue", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfDeterminantValue", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteTemplateDeterminants;
            }
        }

        /// <summary>
        /// ������� ��� �������� ����� �� ��������
        /// </summary>
        private SqlCommand CommandDeleteLines
        {
            get
            {
                if (m_CommandDeleteLines == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteLines = new SqlCommand
                                               {
                                                   CommandText = "dbo.spFFRemoveLine",
                                                   CommandType = CommandType.StoredProcedure,
                                                   Connection = (SqlConnection) Connection
                                               };
                    m_CommandDeleteLines.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteLines.Parameters.Add(new SqlParameter("@idfDecorElement", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfDecorElement", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteLines;
            }
        }

        /// <summary>
        /// ������� ��� �������� ������� �� ��������
        /// </summary>
        private SqlCommand CommandDeleteLabels
        {
            get
            {
                if (m_CommandDeleteLabels == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteLabels = new SqlCommand
                                                {
                                                    CommandText = "dbo.spFFRemoveLabel",
                                                    CommandType = CommandType.StoredProcedure,
                                                    Connection = (SqlConnection) Connection
                                                };
                    m_CommandDeleteLabels.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteLabels.Parameters.Add(new SqlParameter("@idfDecorElement", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfDecorElement", DataRowVersion.Current, false, null, "", "", ""));

                    #endregion
                }
                return m_CommandDeleteLabels;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ������ � ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveSectionTemplate
        {
            get
            {
                if (m_CommandSaveSectionTemplate == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveSectionTemplate = new SqlCommand
                                                       {
                                                           CommandText = "dbo.spFFSaveSectionTemplate",
                                                           CommandType = CommandType.StoredProcedure,
                                                           Connection = (SqlConnection) Connection
                                                       };

                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsSection", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@blnFreeze", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "blnFreeze", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@intLeft", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLeft", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@intTop", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intTop", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@intWidth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intWidth", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@intHeight", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intHeight", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@intCaptionHeight", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intCaptionHeight", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 10, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveSectionTemplate.Parameters.Add(new SqlParameter("@intOrder", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intOrder", DataRowVersion.Current, false, null, "", "", ""));
                    
                    #endregion
                }
                return m_CommandSaveSectionTemplate;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ���������� � ������ (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveParameterTemplate
        {
            get
            {
                if (m_CommandSaveParameterTemplate == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveParameterTemplate = new SqlCommand
                                                         {
                                                             CommandText = "dbo.spFFSaveParameterTemplate",
                                                             CommandType = CommandType.StoredProcedure,
                                                             Connection = (SqlConnection) Connection
                                                         };

                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@idfsEditMode", SqlDbType.VarChar, 8, ParameterDirection.Input, 19, 0, "idfsEditMode", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intLeft", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLeft", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intTop", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intTop", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intWidth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intWidth", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intHeight", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intHeight", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intScheme", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intScheme", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intLabelSize", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLabelSize", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@intOrder", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intOrder", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameterTemplate.Parameters.Add(new SqlParameter("@blnFreeze", SqlDbType.Bit, 1, ParameterDirection.Input, 10, 0, "blnFreeze", DataRowVersion.Current, false, null, "", "", ""));
        
                    #endregion
                }
                return m_CommandSaveParameterTemplate;
            }
        }

        /// <summary>
        /// ������� ��� �������� ���������� � �������
        /// </summary>
        private SqlCommand CommandDeleteParameterTemplate
        {
            get
            {
                if (m_CommandDeleteParameterTemplate == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteParameterTemplate = new SqlCommand
                                                           {
                                                               CommandText = "dbo.spFFRemoveParameterTemplate",
                                                               CommandType = CommandType.StoredProcedure,
                                                               Connection = (SqlConnection) Connection
                                                           };

                    m_CommandDeleteParameterTemplate.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteParameterTemplate.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteParameterTemplate.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormTemplate", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteParameterTemplate.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteParameterTemplate;
            }
        }

        /// <summary>
        /// ������� ��� �������� ����������
        /// </summary>
        private SqlCommand CommandDeleteParameter
        {
            get
            {
                if (m_CommandDeleteParameter == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteParameter = new SqlCommand
                                                   {
                                                       CommandText = "dbo.spFFRemoveParameter",
                                                       CommandType = CommandType.StoredProcedure,
                                                       Connection = (SqlConnection) Connection
                                                   };

                    m_CommandDeleteParameter.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteParameter.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
           
                    #endregion
                }
                return m_CommandDeleteParameter;
            }
        }

        /// <summary>
        /// ������� ��� �������� ������ � �������
        /// </summary>
        private SqlCommand CommandDeleteSectionTemplate
        {
            get
            {
                if (m_CommandDeleteSectionTemplate == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteSectionTemplate = new SqlCommand
                                                         {
                                                             CommandText = "dbo.spFFRemoveSectionTemplate",
                                                             CommandType = CommandType.StoredProcedure,
                                                             Connection = (SqlConnection) Connection
                                                         };

                    m_CommandDeleteSectionTemplate.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4,
                                                                                 ParameterDirection.ReturnValue, 10, 0,
                                                                                 null, DataRowVersion.Current, false,
                                                                                 null, "", "", ""));
                    m_CommandDeleteSectionTemplate.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8,
                                                                                 ParameterDirection.Input, 0, 0,
                                                                                 "idfsSection", DataRowVersion.Current,
                                                                                 false, null, "", "", ""));
                    m_CommandDeleteSectionTemplate.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt,
                                                                                 8, ParameterDirection.Input, 0, 0,
                                                                                 "idfsFormTemplate",
                                                                                 DataRowVersion.Current, false, null, "",
                                                                                 "", ""));
                    m_CommandDeleteSectionTemplate.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50,
                                                                                 ParameterDirection.Input, 0, 0,
                                                                                 "langid", DataRowVersion.Current,
                                                                                 false, null, "", "", ""));

                    #endregion
                }
                return m_CommandDeleteSectionTemplate;
            }
        }

        /// <summary>
        /// ������� ��� �������� ������
        /// </summary>
        private SqlCommand CommandDeleteSection
        {
            get
            {
                if (m_CommandDeleteSection == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandDeleteSection = new SqlCommand
                                                 {
                                                     CommandText = "dbo.spFFRemoveSection",
                                                     CommandType = CommandType.StoredProcedure,
                                                     Connection = (SqlConnection) Connection
                                                 };

                    m_CommandDeleteSection.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandDeleteSection.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsSection", DataRowVersion.Current, false, null, "", "", ""));
            
                    #endregion
                }
                return m_CommandDeleteSection;
            }
        }

        /// <summary>
        /// ������� ��� ���������� ���������� (Insert+Update)
        /// </summary>
        private SqlCommand CommandSaveParameters
        {
            get
            {
                if (m_CommandSaveParameters == null)
                {
                    #region �������� ���������� ��� ��������� ����������

                    m_CommandSaveParameters = new SqlCommand
                                                  {
                                                      CommandText = "dbo.spFFSaveParameters",
                                                      Connection = (SqlConnection) Connection,
                                                      CommandType = CommandType.StoredProcedure
                                                  };

                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@idfsParameter", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsParameter", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsSection", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@idfsFormType", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsFormType", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@idfsParameterCaption", SqlDbType.BigInt, 8, ParameterDirection.InputOutput, 19, 0, "idfsParameterCaption", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intScheme", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intScheme", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@idfsParameterType", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsParameterType", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@idfsEditor", SqlDbType.BigInt, 8, ParameterDirection.Input, 19, 0, "idfsEditor", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intHACode", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intHACode", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intOrder", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intOrder", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@strNote", SqlDbType.NVarChar, 1000, ParameterDirection.Input, 0, 0, "strNote", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@DefaultName", SqlDbType.NVarChar, 400, ParameterDirection.Input, 0, 0, "DefaultName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@NationalName", SqlDbType.NVarChar, 600, ParameterDirection.Input, 0, 0, "NationalName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@DefaultLongName", SqlDbType.NVarChar, 400, ParameterDirection.Input, 0, 0, "DefaultLongName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@NationalLongName", SqlDbType.NVarChar, 600, ParameterDirection.Input, 0, 0, "NationalLongName", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@LangID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 19, 0, "langid", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intLeft", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLeft", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intTop", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intTop", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intWidth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intWidth", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intHeight", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intHeight", DataRowVersion.Current, false, null, "", "", ""));
                    m_CommandSaveParameters.Parameters.Add(new SqlParameter("@intLabelSize", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "intLabelSize", DataRowVersion.Current, false, null, "", "", ""));
        
                    #endregion
                }

                return m_CommandSaveParameters;
            }
        }

        /// <summary>
        /// ��������� ������� ������ ���� (� ������� ��������� �������)
        /// </summary>
        /// <returns></returns>
        public void LoadFormTypes()
        {
            //MainDataSet.Sections.Rows.Clear();
            //MainDataSet.Parameters.Rows.Clear();
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_FormTypesTableAdapter.Connection = (SqlConnection) manager.Connection;
                
            m_FormTypesTableAdapter.Fill(MainDataSet.FormTypes, ModelUserContext.CurrentLanguage, null);
            //}
            
        }


        /// <summary>
        /// ���������� �������� HA �����
        /// </summary>
        /// <returns></returns>
        public DataView LoadHACodeList()
        {
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_HACodeListTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_HACodeListTableAdapter.Fill(MainDataSet.HACodeList, ModelUserContext.CurrentLanguage, null, null); 
            //}
            
            return MainDataSet.HACodeList.DefaultView;
        }

        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        /// <returns></returns>
        public void LoadSections(long? idfsFormType, long? idfsSection, long? idfsParentSection)
        {
            if (
                (MainDataSet.Sections.Select(DataHelper.Filter("idfsFormType", idfsFormType)).Length > 0)
                //||
                //((MainDataSet.Sections.Select(DataHelper.Filter("idfsParentSection", idfsParentSection)).Length > 0)
                //&& (MainDataSet.Sections.Select(DataHelper.Filter("idfsSection", idfsSection)).Length > 0))
                )
                return;
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_SectionsTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_SectionsTableAdapter.Fill(MainDataSet.Sections, ModelUserContext.CurrentLanguage, idfsFormType, idfsSection, idfsParentSection);
            //}
            
        }

        /// <summary>
        /// �������� �������� �� ������ ��� � ������ ������
        /// </summary>
        /// <param name="rowSection"></param>
        /// <returns></returns>
        public bool CheckSectionForNew(FlexibleFormsDS.SectionsRow rowSection)
        {
            bool result = false;
            if (rowSection.idfsSection < 0)
            {
                result = true;
            }
            else
            {
                if (!rowSection.IsidfsParentSectionNull())
                {
                    var rowSectionParent = (FlexibleFormsDS.SectionsRow[])MainDataSet.Sections.Select(DataHelper.Filter("idfsSection", rowSection.idfsParentSection, false));
                    if (rowSectionParent.Length == 1)
                        result = CheckSectionForNew(rowSectionParent[0]);
                }
            }

            return result;
        }

        /// <summary>
        /// �������� �������� �� �������� ��� ��� ������ ������
        /// </summary>
        /// <param name="rowParameter"></param>
        /// <returns></returns>
        public bool CheckParameterForNew(FlexibleFormsDS.ParametersRow rowParameter)
        {
            bool result = false;
            if (rowParameter.idfsParameter < 0)
            {
                result = true;
            }
            else
            {
                if (!rowParameter.IsidfsSectionNull())
                {
                    var rowSectionParent = (FlexibleFormsDS.SectionsRow[])MainDataSet.Sections.Select(DataHelper.Filter("idfsSection", rowParameter.idfsSection, false));
                    if (rowSectionParent.Length == 1)
                        result = CheckSectionForNew(rowSectionParent[0]);
                }
            }

            return result;
        }

        /// <summary>
        /// ��������� �������� �� ������� ����� (�� ����������)
        /// </summary>
        /// <param name="rulesRow"></param>
        /// <returns></returns>
        public bool CheckRuleForNew(FlexibleFormsDS.RulesRow rulesRow)
        {
            return (rulesRow.idfsRule < 0);
        }

        /// <summary>
        /// ��������� ���� ������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        public FlexibleFormsDS.TemplatesRow LoadTemplate(long idfsFormTemplate)
        {
            var rows = MainDataSet.Templates.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplate));
            if (rows.Length > 0) return rows[0] as FlexibleFormsDS.TemplatesRow;
            m_TemplatesTableAdapter.Fill(MainDataSet.Templates, ModelUserContext.CurrentLanguage, idfsFormTemplate, null);
            LoadTemplateDeterminants(idfsFormTemplate);
            var templatesRow = this.GetTemplateRow(idfsFormTemplate);
            if ((templatesRow != null) && templatesRow.IsRowAlive()) templatesRow.blnUNIInitial = templatesRow.blnUNI;
            return templatesRow;
        }

        /// <summary>
        /// ��������� ������� ��������
        /// </summary>
        /// <param name="idfsFormType"></param>
        /// <returns></returns>
        public void LoadTemplates(long? idfsFormType)
        {
            if (MainDataSet.Templates.Select(DataHelper.Filter("idfsFormType", idfsFormType)).Length > 0) return;
            m_TemplatesTableAdapter.Fill(MainDataSet.Templates, ModelUserContext.CurrentLanguage, null, idfsFormType);
            
            foreach(FlexibleFormsDS.TemplatesRow templatesRow in  MainDataSet.Templates.Rows)
            {
                if (!templatesRow.IsRowAlive()) continue;
                LoadTemplateDeterminants(templatesRow.idfsFormTemplate);
                templatesRow.blnUNIInitial = templatesRow.blnUNI;
            }
        }

        /// <summary>
        /// ��������� ������� ����� � �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public void LoadLines(long? idfsFormTemplate)
        {
            LoadLines(idfsFormTemplate, ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ��������� ������� ����� � �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public void LoadLines(long? idfsFormTemplate, string langid)
        {
            var filterString = DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid));
            if (DataHelper.NeedRefreshDataTable(MainDataSet.Lines, filterString))
            {
                m_LinesTableAdapter.Fill(MainDataSet.Lines, langid, idfsFormTemplate);
            }
        }

        /// <summary>
        /// ��������� ������� ������ � �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public void LoadSectionTemplate(long? idfsFormTemplate)
        {
            LoadSectionTemplate(idfsFormTemplate, ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ��������� ������� ������ � �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public void LoadSectionTemplate(long? idfsFormTemplate, string langid)
        {
            //���� ������ �� ����� ������� ��� ���������, �� �������� �� �� ������,
            //��������� ������� �������� � ������������� ������
            var filterstring = DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid));
            //if (MainDataSet.SectionTemplate.Select(filterstring).Length > 0) return;
            if (DataHelper.NeedRefreshDataTable(MainDataSet.SectionTemplate, filterstring))
            {
                m_SectionTemplateTableAdapter.Fill(MainDataSet.SectionTemplate, null, idfsFormTemplate, langid);
            }
        }

        /// <summary>
        /// ���������, ������������ �� ������ � �����-���� �������
        /// </summary>
        /// <returns></returns>
        public bool IsSectionUsed(long idfsSection)
        {
            //������� �������� ����� �������� ������������ �����
            var result = MainDataSet.SectionTemplate.Select(DataHelper.Filter("idfsSection", idfsSection)).Length > 0;
            if (!result)
            {
                m_SectionUsedTableAdapter.Fill(MainDataSet.SectionUsed, idfsSection);
                //�������� � ��
                result = (bool)MainDataSet.SectionUsed.Rows[0][0];
            }
            return result;
        }

        /// <summary>
        /// ���������, ������������ �� �������� � �����-���� �������
        /// </summary>
        /// <returns></returns>
        public bool IsParameterUsed(long idfsParameter)
        {
            //������� �������� ����� �������� ������������ �����
            var result = MainDataSet.ParameterTemplate.Select(DataHelper.Filter("idfsParameter", idfsParameter)).Length > 0;
            
            //�������� ������� ���������, ��� ���� ����� ����������� ��������
            //TODO ������� ���������� �������� � ��������� ��� ������ ��������� (������ ����� ��������� ��������� ��� �������� ����������� �������� ���������)
            var versions = new List<long>(); //����� ������, ����� ���� ���������� ������
            if (!result)
            {
                LoadPredefinedStub((long) AggregateCaseSection.DiagnosticAction, versions, null);
                result = MainDataSet.PredefinedStub.Select(DataHelper.Filter("idfsParameter", idfsParameter)).Length > 0;
            }
            if (!result)
            {
                LoadPredefinedStub((long)AggregateCaseSection.HumanCase, versions, null);
                result = MainDataSet.PredefinedStub.Select(DataHelper.Filter("idfsParameter", idfsParameter)).Length > 0;
            }
            if (!result)
            {
                LoadPredefinedStub((long)AggregateCaseSection.ProphylacticAction, versions, null);
                result = MainDataSet.PredefinedStub.Select(DataHelper.Filter("idfsParameter", idfsParameter)).Length > 0;
            }
            if (!result)
            {
                LoadPredefinedStub((long)AggregateCaseSection.SanitaryAction, versions, null);
                result = MainDataSet.PredefinedStub.Select(DataHelper.Filter("idfsParameter", idfsParameter)).Length > 0;
            }
            if (!result)
            {
                LoadPredefinedStub((long)AggregateCaseSection.VetCase, versions, null);
                result = MainDataSet.PredefinedStub.Select(DataHelper.Filter("idfsParameter", idfsParameter)).Length > 0;
            }

            return result;
        }

        /// <summary>
        /// �������� ������ ������� ��� ��������. ������������ ������� ������, ��������� � ���������� Observation.
        /// </summary>
        /// <param name="idfObservation"></param>
        /// <returns></returns>
        public FlexibleFormsDS.AggregateGetMatrixVersionRow LoadMatrixStubInfo(long idfObservation)
        {
            m_AggregateGetMatrixVersionTableAdapter.Fill(MainDataSet.AggregateGetMatrixVersion, idfObservation);
            return GetAggregateGetMatrixVersionRow();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FlexibleFormsDS.AggregateGetMatrixVersionRow GetAggregateGetMatrixVersionRow()
        {
            return MainDataSet.AggregateGetMatrixVersion.Count == 1
                                                                   ? MainDataSet.AggregateGetMatrixVersion[0]
                                                                   : null;
        }

        /// <summary>
        /// ��������� ������� ������ ��� ���������� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public void LoadRulesForTemplate(long idfsFormTemplate)
        {
            //���� ������ �� ����� ������� ��� ���������, �� �������� �� �� ������,
            //��������� ������� �������� � ������������� ������
            if (MainDataSet.Rules.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplate)).Length > 0) return;
            
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_RulesTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_RulesTableAdapter.Fill(MainDataSet.Rules, ModelUserContext.CurrentLanguage, idfsFormTemplate);
            //}
        }

        /// <summary>
        /// ��������� ������� ���������� � �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public void LoadParameterTemplate(long? idfsFormTemplate)
        {
            LoadParameterTemplate(idfsFormTemplate, ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ��������� ������� ���������� � �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        /// <returns></returns>
        public void LoadParameterTemplate(long? idfsFormTemplate, string langid)
        {
            //���� ������ �� ����� ������� ��� ���������, �� �������� �� �� ������,
            //��������� ������� �������� � ������������� ������
            var filterstring = DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid));
            if (DataHelper.NeedRefreshDataTable(MainDataSet.ParameterTemplate, filterstring))
            {
                m_ParameterTemplateTableAdapter.Fill(MainDataSet.ParameterTemplate, null, idfsFormTemplate, langid);
            }
        }

        /// <summary>
        /// ��������� ������� � �������� ��������, ��� ������������ ��������� ��������
        /// </summary>
        /// <param name="idfsParameterID"></param>
        /// <returns></returns>
        public void LoadTemplatesByParameter(long? idfsParameterID)
        {
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_TemplatesByParameterTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_TemplatesByParameterTableAdapter.Fill(MainDataSet.TemplatesByParameter, ModelUserContext.CurrentLanguage, idfsParameterID);
            //}
            
        }

        /// <summary>
        /// ��������� �������� �������������, ������� ���� � ��
        /// </summary>
        public void LoadDeterminants()
        {
            if (MainDataSet.Determinants.Count == 0)
            {
                //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
                //{
                //    m_DeterminantsTableAdapter.Connection = (SqlConnection)manager.Connection;
                    m_DeterminantsTableAdapter.Fill(MainDataSet.Determinants, ModelUserContext.CurrentLanguage);
                //}
            }
            
            //return (FlexibleFormsDS.DeterminantsRow[])MainDataSet.Determinants.Select("idfsBaseReference is not null");
        }

        /// <summary>
        /// �������� �������� ��������� ������� ��� �������
        /// </summary>
        /// <param name="countParameters"></param>
        public void LoadRuleFunctions(int countParameters)
        {
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_RuleFunctionsTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_RuleFunctionsTableAdapter.Fill(MainDataSet.RuleFunctions, countParameters, ModelUserContext.CurrentLanguage);
            //}
            
        }

        /// <summary>
        /// �������� �������� ��������� ������� ��� �������
        /// </summary>
        /// <param name="idfsRule"></param>
        public void LoadRuleParameterForFunction(long? idfsRule)
        {
            if (MainDataSet.RuleParameterForFunction.Select(DataHelper.Filter("idfsRule", idfsRule)).Length == 0)
            {
                
                //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
                //{
                //    m_RuleParameterForFunctionTableAdapter.Connection = (SqlConnection)manager.Connection;
                    m_RuleParameterForFunctionTableAdapter.Fill(MainDataSet.RuleParameterForFunction, idfsRule);
                //}
            }
        }

        /// <summary>
        /// �������� �������� ������������� �������� ��� �������
        /// </summary>
        /// <param name="idfsRule"></param>
        public void LoadRuleConstants(long? idfsRule)
        {
            if (MainDataSet.RuleConstant.Select(DataHelper.Filter("idfsRule", idfsRule)).Length == 0)
            {
                
                //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
                //{
                //    m_RuleParameterForFunctionTableAdapter.Connection = (SqlConnection)manager.Connection;
                    m_RuleConstantTableAdapter.Fill(MainDataSet.RuleConstant, idfsRule);
                //}
            }
        }

        /// <summary>
        /// �������� �������� �������� ��� �������
        /// </summary>
        /// <param name="idfsRule"></param>
        public void LoadRuleParameterForAction(long? idfsRule)
        {
            if (MainDataSet.RuleParameterForAction.Select(DataHelper.Filter("idfsRule", idfsRule)).Length == 0)
            {
                
                //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
                //{
                //    m_RuleParameterForActionTableAdapter.Connection = (SqlConnection)manager.Connection;
                    m_RuleParameterForActionTableAdapter.Fill(MainDataSet.RuleParameterForAction, idfsRule);
                //}
            }
        }

        /// <summary>
        /// ��������� ������� �������� ����������, ������� ������� �� �������
        /// </summary>
        /// <param name="parameterCriteria"></param>
        /// <param name="sectionCriteria"></param>
        /// <returns></returns>
        public void LoadParametersSearch(string parameterCriteria, string sectionCriteria)
        {
            m_ParametersSearchTableAdapter.Fill(MainDataSet.ParametersSearch, ModelUserContext.CurrentLanguage, parameterCriteria, sectionCriteria);
        }

        /// <summary>
        /// ������ ��������� ��� Editor ����������
        /// </summary>
        /// <returns></returns>
        public void LoadParameterEditors()
        {
            m_ParameterControlTypesEditorsTableAdapter.Fill(MainDataSet.ParameterEditors, ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ��������� ������� ����������, ������� ���� ������� �� �������, �� �� ������� �������� ������
        /// </summary>
        /// <param name="idfObservation"></param>
        /// <param name="idfsFormTemplateCur"> </param>
        /// <param name="idfsFormTemplatePrev"> </param>
        public FlexibleFormsDS.ParametersDeletedFromTemplateDataTable LoadParameterDeletedFromTemplate(long idfObservation, long? idfsFormTemplateCur, long? idfsFormTemplatePrev)
        {
            m_ParametersDeletedFromTemplateTableAdapter.Fill(MainDataSet.ParametersDeletedFromTemplate, idfObservation,
                                                             ModelUserContext.CurrentLanguage);

            if (idfsFormTemplatePrev.HasValue && idfsFormTemplateCur.HasValue)
            {
                var aps = this.GetActivityParametersRows(idfObservation);
                var parameters = this.GetParameterTemplateRows(null, idfsFormTemplateCur.Value).ToList();
                for (var i = parameters.Count - 1; i >= 0; i--)
                {
                    if (parameters[i].blnDynamicParameter) parameters.RemoveAt(i);
                }
                if (parameters.Count > 0)
                {
                    foreach (var ap in aps)
                    {
                        if (!ap.IsRowAlive()) continue;
                        var idfsParameter = ap.idfsParameter;
                        //���� �������� ��� ���� � ������������ (��� ������� � ������ ������ ������ -- ������ �� ��)
                        if (MainDataSet.ParametersDeletedFromTemplate.Count(p => p.idfsParameter == idfsParameter) > 0)
                        {
                            //��������, �� ������������ �� �������� � �������, ������� �������� ��������
                            if (parameters.Count(p => p.idfsParameter == idfsParameter) > 0)
                            {
                                //����� ��� �� ������������ -- �� ������ �����������
                                var par =
                                    MainDataSet.ParametersDeletedFromTemplate.FirstOrDefault(
                                        r => r.idfsParameter == idfsParameter);
                                if (par != null) MainDataSet.ParametersDeletedFromTemplate.Rows.Remove(par);
                            }
                        }
                        else
                        {
                            //���� ������� ������ �� ����������� ���������� � ������� �������, �� ��������� ��������������� ��������
                            if (parameters.Count(p => (p.idfsParameter == idfsParameter)) == 0)
                            {
                                var parametersOldTemplate =
                                    this.GetParameterTemplateRows(null, idfsFormTemplatePrev.Value).ToList();
                                if (parametersOldTemplate.Count == 0) continue;
                                var parameter =
                                    parametersOldTemplate.FirstOrDefault(p => p.idfsParameter == idfsParameter);
                                if (parameter == null) continue;
                                var row =
                                    MainDataSet.ParametersDeletedFromTemplate.NewParametersDeletedFromTemplateRow();
                                row.DefaultName = parameter.DefaultName;
                                row.NationalName = parameter.NationalName;
                                row.idfObservation = idfObservation;
                                row.idfsEditor = parameter.idfsEditor;
                                row.idfsFormTemplate = parameter.idfsFormTemplate;
                                row.idfsParameter = parameter.idfsParameter;
                                row.intHeight = parameter.intHeight;
                                row.intLabelSize = parameter.intLabelSize;
                                row.intLeft = parameter.intLeft;
                                row.intOrder = parameter.intOrder;
                                row.intScheme = parameter.intScheme;
                                row.intTop = parameter.intTop;
                                row.intWidth = parameter.intWidth;
                                row.langid = ModelUserContext.CurrentLanguage;
                                MainDataSet.ParametersDeletedFromTemplate.AddParametersDeletedFromTemplateRow(row);
                            }
                        }
                    }
                }
            }

            return MainDataSet.ParametersDeletedFromTemplate;
        }

        /// <summary>
        /// ��������� ������� ������������� ��������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public void LoadTemplateDeterminants(long? idfsFormTemplate)
        {
            var needRefresh = !(MainDataSet.TemplateDeterminantValues.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplate)).Length > 0);
            //���� ������ ��� �� �������, �� ��������� ������� �� ������� �����, ������� ����� ������������, �� ���� �������
            //����� ��������� ��� �������� �������� ��������� �������
            if (needRefresh)
                needRefresh = !(MainDataSet.TemplateDeterminantValues.Select(DataHelper.Filter("idfsFormTemplate", idfsFormTemplate), String.Empty, DataViewRowState.Deleted).Length > 0);
            if (needRefresh)
            {
                m_TemplateDeterminantValuesTableAdapter.Fill(MainDataSet.TemplateDeterminantValues,
                                                               ModelUserContext.CurrentLanguage,
                                                               idfsFormTemplate);
                m_TemplateDeterminantValuesTableAdapter.Fill(TemplateDeterminantsBackup,
                                                               ModelUserContext.CurrentLanguage,
                                                               idfsFormTemplate);
            }
        }

        /// <summary>
        /// ��������� ������� ����������
        /// </summary>
        /// <param name="idfsFormType"></param>
        /// <param name="idfsSection"></param>
        /// <returns></returns>
        public void LoadParameters(long? idfsFormType, long? idfsSection)
        {
            if (MainDataSet.Parameters.Select(DataHelper.Filter("idfsSection", idfsSection, "idfsFormType", idfsFormType)).Length > 0) return;
            //if (DataHelper.HasRows(MainDataSet.Parameters, "idfsSection", idfsSection, "idfsFormType", idfsFormType)) return;

            
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_ParametersTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_ParametersTableAdapter.Fill(MainDataSet.Parameters, ModelUserContext.CurrentLanguage, idfsSection, idfsFormType);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override DataSet GetDetail(object id)
        {
            return MainDataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        /// <param name="row"></param>
        private long? SaveSection(SqlDataAdapter da, FlexibleFormsDS.SectionsRow row)
        {
            if (!row.IsRowAlive() || (row.RowState == DataRowState.Unchanged)) return null;
            //��������, � ��������� �� ������������ ������ ���� ������? ���� ���, �� ��������� ���������� ������� ���������� ������������ ������
            //TODO ��������� ������������ ������, ������ ��� ����� ���������� ��������� ���� ����� ���������� ������ �� ����� � �������� �������� ���� ����� ������ ���������� ���������
            //TODO ��� �������� � ���������� ������������. ��������, ����� ��� ��� ��������� �� RowState ����� ���� �������� ��� �� ������� ��������� �����, ����������� ������ ���������
            //TODO � �� ������������ ���� �� ������
            if ((!row.IsidfsParentSectionNull()) && (row.idfsParentSection < 0))
            {
                //������� ��� ������ � ��������� � ����������
                var sectionsRows = (FlexibleFormsDS.SectionsRow[])MainDataSet.Sections.Select(DataHelper.Filter("idfsSection", row.idfsParentSection));
                if (sectionsRows.Length == 1)
                {
                    var result = SaveSection(da, sectionsRows[0]);
                    if (result.HasValue) row.idfsParentSection = result.Value;
                }
            }
            
            long? oldSectionID = null;
            //�������� ������ ID
            if (row.idfsSection < 0) oldSectionID = row.idfsSection;
            da.Update(new DataRow[] { row });

            if (oldSectionID != null)
            {
                //������ ��� ������, ������� ������� �� ����� ID, � �������� ��
                for (var i = 0; i < MainDataSet.Sections.Rows.Count; i++)
                {
                    var sectionsRows = (FlexibleFormsDS.SectionsRow) MainDataSet.Sections.Rows[i];
                    if (!sectionsRows.IsRowAlive() || (sectionsRows.RowState == DataRowState.Unchanged)) continue;
                    //����� � ��������� ������� ��� ������ � ������� ��� ID
                    //if (sectionsRows.idfsSection.Equals(oldSectionID.Value))
                    //{
                    //    //�������� ��������� ID �� ���������
                    //    if (SectionTableList.ContainsKey(sectionsRows.idfsSection))
                    //    {
                    //        SectionTable sectionTable = SectionTableList[sectionsRows.idfsSection];
                    //        SectionTableList.Remove(sectionsRows.idfsSection);
                    //        SectionTableList.Add(row.idfsSection, sectionTable);
                    //    }
                    //    if (SectionList.ContainsKey(sectionsRows.idfsSection))
                    //    {
                    //        Section section = SectionList[sectionsRows.idfsSection];
                    //        SectionList.Remove(sectionsRows.idfsSection);
                    //        SectionList.Add(row.idfsSection, section);
                    //    }

                    //    sectionsRows.idfsSection = row.idfsSection;

                    //}
                    //�������� ��������� ������
                    if (sectionsRows.IsidfsParentSectionNull()) continue;
                    if (!sectionsRows.idfsParentSection.Equals(oldSectionID.Value)) continue;
                    sectionsRows.idfsParentSection = row.idfsSection;
                    SaveSection(da, sectionsRows);
                }
                //������ �� ���������� � ���������� ����� ���������
                for (int i = 0; i < MainDataSet.Parameters.Rows.Count; i++)
                {
                    var parametersRow = (FlexibleFormsDS.ParametersRow)MainDataSet.Parameters.Rows[i];
                    if (!parametersRow.IsRowAlive() || (parametersRow.RowState == DataRowState.Unchanged)) continue;
                    if (parametersRow.IsidfsSectionNull()) continue;
                    if (parametersRow.idfsSection.Equals(oldSectionID.Value))
                    {
                        parametersRow.idfsSection = row.idfsSection;
                    }
                }
            }
            return row.idfsSection;
        }

        /// <summary>
        /// ��������� ������ ������������� � ������ ��� ����� �������� ���������
        /// </summary>
        /// <param name="determinants"></param>
        /// <returns></returns>
        private string DeterminantsArrayToString(IEnumerable<FlexibleFormsDS.TemplateDeterminantValuesRow> determinants)
        {
            var sb = new StringBuilder();
            foreach (var row in determinants)
            {
                if (!row.IsRowAlive()) continue;
                //���� �� ������ ID, �� � ���, ����� ��������� ���������� ID  � ������� ����� � ������� �������������
                sb.Append(!row.IsidfsBaseReferenceNull() ? row.idfsBaseReference : row.idfsGISBaseReference);
                sb.Append(row.DeterminantTypeDefaultName); sb.Append(";"); 
            }

            return sb.ToString();
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="postType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override bool PostDetail(DataSet ds, int postType, IDbTransaction transaction)
        {
            #region �������� ������

            for (int i = 0; i < MainDataSet.Rules.Count; i++)
            {
                var rulesRow = MainDataSet.Rules[i];
                if (!rulesRow.IsRowAlive()) continue;
                //� ������� ������ ���� ������ �������
                //rulesRow.idfsRuleFunction is not null field, so we initialize it with -1L when create new rule
                if (rulesRow.IsidfsRuleFunctionNull() || rulesRow.idfsRuleFunction == -1L)
                {
                    m_Error = new EIDSSErrorMessage("Rule_Save_MustSetFunction", String.Empty, null, rulesRow.NationalName);
                    return false;
                }
                //������ ������� ������ ����� ��� ������� ���� ��������
                if (MainDataSet.RuleParameterForAction.Select(DataHelper.Filter("idfsRule", rulesRow.idfsRule)).Length == 0)
                {
                    m_Error = new EIDSSErrorMessage("Rule_Save_MustSetAction", String.Empty, null, rulesRow.NationalName);
                    return false;
                }
                //� ������ Is Value ������ ���� ������ ���� �� ���� ���������
                if ((rulesRow.idfsRuleFunction == (long)FFRuleFunctions.Value) && (MainDataSet.RuleConstant.Select(DataHelper.Filter("idfsRule", rulesRow.idfsRule)).Length == 0))
                {
                    m_Error = new EIDSSErrorMessage("Rule_Save_MustSetConstant", String.Empty, null, rulesRow.NationalName);
                    return false;
                }
                /*
                //� ������� ������ ���� ������ Message Text � Message National Text
                if (rulesRow.MessageText.Length == 0)
                {
                    m_Error = new EIDSSErrorMessage("Rule_MessageText", String.Empty, null);
                    return false;
                }
                if (rulesRow.MessageNationalText.Length == 0)
                {
                    m_Error = new EIDSSErrorMessage("Rule_MessageNationalText", String.Empty, null);
                    return false;
                }
                */
            }

            #endregion

            try
            {
                //��������� ������ � ���������
                //��������� ���� ���� �� �������������, �� �� ���������
                //�� ���������� ������ � ��������� �� ���������� ���������� �� ����� ���� �������� � ������� �� ���� ��� ��� ����� ���������

                #region ���������� ������

                var da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveSections;
                da.DeleteCommand = CommandDeleteSection;

                ApplyTransaction(da, transaction);
                //�������� �� ���� �������, ������� ��������
                var tableSections =
                    (FlexibleFormsDS.SectionsDataTable) MainDataSet.Sections.GetChanges();
                if (tableSections != null)
                {
                    for (var i = 0; i < MainDataSet.Sections.Rows.Count; i++)
                    {
                        var row = (FlexibleFormsDS.SectionsRow) MainDataSet.Sections.Rows[i];
                        SaveSection(da, row);
                    }

                    //�������� ���������
                    tableSections = (FlexibleFormsDS.SectionsDataTable) MainDataSet.Sections.GetChanges(DataRowState.Deleted);
                    if (tableSections != null)
                    {
                        da.Update(tableSections);
                    }
                }
                MainDataSet.Sections.AcceptChanges();

                #endregion

                #region ���������� ����������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveParameters;
                da.DeleteCommand = CommandDeleteParameter;

                ApplyTransaction(da, transaction);
                //��������� ������ �������� ��������� (���� ��� ��������, ������������ �� ������� ��������)
                var parametersRows = (FlexibleFormsDS.ParametersRow[])MainDataSet.Parameters.Select(DataHelper.Filter("IsRealParameter", 1));
                da.Update(parametersRows);
                //da.Update(MainDataSet.Parameters);
                
                MainDataSet.Parameters.AcceptChanges();

                #endregion

                #region ���������� ��������

                //��������, ������� �� ������� UNI ���-����
                //�������� ����� ��� ���������� ������� ������� � ��� ����� ��� ������������� UNI


                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveTemplates;
                da.DeleteCommand = CommandDeleteTemplates;
                ApplyTransaction(da, transaction);
                //�������� �������� �� �������� �������� ��� �������
                var templateDeleted = new List<long>();
                //�������� �� ���� �������, ������� ��������
                for (var i = 0; i < MainDataSet.Templates.Rows.Count; i++)
                {
                    var row = (FlexibleFormsDS.TemplatesRow) MainDataSet.Templates.Rows[i];
                    if (!row.IsRowAlive()) templateDeleted.Add((long)row["idfsFormTemplate", DataRowVersion.Original]);
                    SaveTemplate(da, row);
                    //������� ���
                    LookupCache.NotifyChange("FormTemplate", transaction);
                }


                //�������� ���������
                var tableTemplates =
                    (FlexibleFormsDS.TemplatesDataTable) MainDataSet.Templates.GetChanges(DataRowState.Deleted);
                if (tableTemplates != null) da.Update(tableTemplates);
                MainDataSet.Templates.AcceptChanges();

                foreach (FlexibleFormsDS.TemplatesRow template in MainDataSet.Templates.Rows)
                {
                    if (template.blnUNI != template.blnUNIInitial) AddEvent(EventType.FFUNITemplateChanged, template.idfsFormTemplate);
                }

                #endregion

                #region ���������� �������������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveTemplateDeterminants;
                da.DeleteCommand = CommandDeleteTemplateDeterminants;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.TemplateDeterminantValues);
                MainDataSet.TemplateDeterminantValues.AcceptChanges();

                //�������� ������������ ������������ � ������ �������
                //���������� ������������ ��� �� �����, ���������� �������, �������� �������
                //� backup-������� ���������� ������ �������� ������������� ��� ��������, ������������� � ���� ������
                
                //� ����� �������� ���������� ������������
                var templatesWasChanged = new List<long>();
                foreach (var tRow in TemplateDeterminantsBackup)
                {
                    var wasFound = false;
                    foreach (var t2Row in MainDataSet.TemplateDeterminantValues)
                    {
                        if (!t2Row.IsRowAlive()) continue;
                        if ((t2Row.idfDeterminantValue != tRow.idfDeterminantValue) || (t2Row.idfsFormTemplate != tRow.idfsFormTemplate)) continue;
                        if (t2Row.DeterminantValue != tRow.DeterminantValue) continue;
                        wasFound = true;
                        break;
                    }
                    if (!wasFound && !templatesWasChanged.Contains(tRow.idfsFormTemplate)) templatesWasChanged.Add(tRow.idfsFormTemplate);
                }

                //�������� ����������� ������������ � ������������ ��������
                foreach (var tRow in MainDataSet.TemplateDeterminantValues)
                {
                    var wasFound = false;
                    foreach (var t2Row in TemplateDeterminantsBackup)
                    {
                        if (!t2Row.IsRowAlive()) continue;
                        if ((t2Row.idfDeterminantValue != tRow.idfDeterminantValue) || (t2Row.idfsFormTemplate != tRow.idfsFormTemplate)) continue;
                        if (t2Row.DeterminantValue != tRow.DeterminantValue) continue;
                        wasFound = true;
                        break;
                    }
                    if (!wasFound && !templatesWasChanged.Contains(tRow.idfsFormTemplate)) templatesWasChanged.Add(tRow.idfsFormTemplate);
                }

                foreach (var idfsFormTemplate in templateDeleted)
                {
                    if (!templatesWasChanged.Contains(idfsFormTemplate)) templatesWasChanged.Add(idfsFormTemplate);
                }

                foreach (var idfsFormTemplate in templatesWasChanged)
                {
                    AddEvent(EventType.FFDeterminantChanged, idfsFormTemplate);
                }

                TemplateDeterminantsBackup.Clear();
                foreach (var tRow in MainDataSet.TemplateDeterminantValues)
                {
                    var row = (FlexibleFormsDS.TemplateDeterminantValuesRow)TemplateDeterminantsBackup.NewRow();
                    row.idfDeterminantValue = tRow.idfDeterminantValue;
                    row.idfsFormTemplate = tRow.idfsFormTemplate;
                    row.DeterminantValue = tRow.DeterminantValue;
                    TemplateDeterminantsBackup.AddTemplateDeterminantValuesRow(row);
                }

                #endregion

                //�� ��������� �� ������, ������� ��������� � �����������, ����������� ���� ������� (FFRender.MergeTemplate) ������ ��������
                var nonMergedFilter = String.Format("idfsFormTemplate <> {0}", IdfsFormTemplateSummary);

                #region ���������� ������ ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveSectionTemplate;
                da.DeleteCommand = CommandDeleteSectionTemplate;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.SectionTemplate.Select(String.Empty, String.Empty, DataViewRowState.Deleted));
                da.Update(MainDataSet.SectionTemplate.Select(nonMergedFilter));
                MainDataSet.SectionTemplate.AcceptChanges();

                #endregion

                #region ���������� ���������� ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveParameterTemplate;
                da.DeleteCommand = CommandDeleteParameterTemplate;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.ParameterTemplate.Select(String.Empty, String.Empty, DataViewRowState.Deleted));
                da.Update(MainDataSet.ParameterTemplate.Select(nonMergedFilter));
                MainDataSet.ParameterTemplate.AcceptChanges();

                #endregion

                #region ���������� ������ ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveRules;
                da.DeleteCommand = CommandDeleteRules;

                ApplyTransaction(da, transaction);

                #region ��������� �������

                //�������� �� ���� �������, ������� ��������
                for (int i = 0; i < MainDataSet.Rules.Count; i++)
                {
                    SaveRule(da, MainDataSet.Rules[i]);
                }
                //�������� ���������
                var tableRules =
                    (FlexibleFormsDS.RulesDataTable) MainDataSet.Rules.GetChanges(DataRowState.Deleted);
                if (tableRules != null) da.Update(tableRules);
                MainDataSet.Rules.AcceptChanges();

                #endregion

                #endregion

                #region ���������� ���������� ������� ��� ������ ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveRuleParameterForFunction;
                da.DeleteCommand = CommandDeleteRuleParameterForFunction;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.RuleParameterForFunction);
                MainDataSet.RuleParameterForFunction.AcceptChanges();

                #endregion

                #region ���������� ������������� �������� ��� ������ ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveRuleConstants;
                da.DeleteCommand = CommandDeleteRuleConstants;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.RuleConstant);
                MainDataSet.RuleConstant.AcceptChanges();

                #endregion

                #region ���������� �������� ��� ������ ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveRuleParameterForAction;
                da.DeleteCommand = CommandDeleteRuleParameterForAction;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.RuleParameterForAction);
                MainDataSet.RuleParameterForAction.AcceptChanges();

                #endregion

                #region ���������� ����� ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveLines;
                da.DeleteCommand = CommandDeleteLines;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.Lines.Select(String.Empty, String.Empty, DataViewRowState.Deleted));
                da.Update(MainDataSet.Lines.Select(nonMergedFilter));
                MainDataSet.Lines.AcceptChanges();

                #endregion

                #region ���������� ������� ������ ��������

                da = new SqlDataAdapter();

                da.InsertCommand = da.UpdateCommand = CommandSaveLabels;
                da.DeleteCommand = CommandDeleteLabels;

                ApplyTransaction(da, transaction);
                da.Update(MainDataSet.Labels.Select(String.Empty, String.Empty, DataViewRowState.Deleted));
                da.Update(MainDataSet.Labels.Select(nonMergedFilter));
                MainDataSet.Labels.AcceptChanges();

                #endregion

            }
            catch (SqlException exc)
            {
                m_Error = ErrorHelper.IsEIDSSError(exc.Message)
                              ? new EIDSSErrorMessage(ErrorHelper.GetMessageString(exc.Message))
                              : new ErrorMessage(StandardError.PostError, exc);

                return false;
            }
            catch (Exception exc)
            {
                m_Error = new ErrorMessage(StandardError.PostError, exc);

                return false;
            }
            return true;
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="da"></param>
        /// <param name="rulesRow"></param>
        private void SaveRule(DbDataAdapter da, FlexibleFormsDS.RulesRow rulesRow)
        {
            if (!rulesRow.IsRowAlive() || (rulesRow.RowState == DataRowState.Unchanged)) return;
            
            long? oldRuleID = null;
            //�������� ������ ID
            if (rulesRow.idfsRule < 0) oldRuleID = rulesRow.idfsRule;
            if (rulesRow.IsidfsRuleMessageNull()) rulesRow.idfsRuleMessage = -1;
            da.Update(new DataRow[] { rulesRow });

            if (oldRuleID == null) return;

            #region ���������� ����� � ��������� ��������

            for(int i =0; i < MainDataSet.RuleConstant.Count; i++)
            {
                var ruleConstantRow = (FlexibleFormsDS.RuleConstantRow)MainDataSet.RuleConstant.Rows[i];
                if ((ruleConstantRow.RowState == DataRowState.Deleted) || (ruleConstantRow.RowState == DataRowState.Unchanged)) continue;
                if (ruleConstantRow.idfsRule.Equals(oldRuleID.Value))
                {
                    ruleConstantRow.idfsRule = rulesRow.idfsRule;
                }
            }

            for (int i = 0; i < MainDataSet.RuleParameterForAction.Count; i++)
            {
                var ruleParameterForActionRow = (FlexibleFormsDS.RuleParameterForActionRow)MainDataSet.RuleParameterForAction.Rows[i];
                if ((ruleParameterForActionRow.RowState == DataRowState.Deleted) || (ruleParameterForActionRow.RowState == DataRowState.Unchanged)) continue;
                if (ruleParameterForActionRow.idfsRule.Equals(oldRuleID.Value))
                {
                    ruleParameterForActionRow.idfsRule = rulesRow.idfsRule;
                }
            }

            for (int i = 0; i < MainDataSet.RuleParameterForFunction.Count; i++)
            {
                var ruleParameterForFunctionRow = (FlexibleFormsDS.RuleParameterForFunctionRow)MainDataSet.RuleParameterForFunction.Rows[i];
                if ((ruleParameterForFunctionRow.RowState == DataRowState.Deleted) || (ruleParameterForFunctionRow.RowState == DataRowState.Unchanged)) continue;
                if (ruleParameterForFunctionRow.idfsRule.Equals(oldRuleID.Value))
                {
                    ruleParameterForFunctionRow.idfsRule = rulesRow.idfsRule;
                }
            }

            #endregion
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="da"></param>
        /// <param name="templatesRow"></param>
        private void SaveTemplate(DbDataAdapter da, FlexibleFormsDS.TemplatesRow templatesRow)
        {
            if (!templatesRow.IsRowAlive() || (templatesRow.RowState == DataRowState.Unchanged)) return;

            long? oldTemplateID = null;
            //�������� ������ ID
            if (templatesRow.idfsFormTemplate < 0) oldTemplateID = templatesRow.idfsFormTemplate;
            da.Update(new DataRow[] { templatesRow });

            if (oldTemplateID == null) return;

            #region ���������� ����� � ��������� ��������

            for (int i = 0; i < MainDataSet.TemplateDeterminantValues.Count; i++)
            {
                var templateDeterminantValuesRow = (FlexibleFormsDS.TemplateDeterminantValuesRow)MainDataSet.TemplateDeterminantValues.Rows[i];
                if (templateDeterminantValuesRow.RowState == DataRowState.Deleted) continue;
                if (templateDeterminantValuesRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    templateDeterminantValuesRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.Rules.Count; i++)
            {
                var rulesRow = (FlexibleFormsDS.RulesRow)MainDataSet.Rules.Rows[i];
                if (rulesRow.RowState == DataRowState.Deleted) continue;
                if (rulesRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    rulesRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.RuleParameterForFunction.Count; i++)
            {
                var parameterForFunctionRow = (FlexibleFormsDS.RuleParameterForFunctionRow)MainDataSet.RuleParameterForFunction.Rows[i];
                if (parameterForFunctionRow.RowState == DataRowState.Deleted) continue;
                if (parameterForFunctionRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    parameterForFunctionRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.RuleParameterForAction.Count; i++)
            {
                var parameterForActionRow = (FlexibleFormsDS.RuleParameterForActionRow)MainDataSet.RuleParameterForAction.Rows[i];
                if (parameterForActionRow.RowState == DataRowState.Deleted) continue;
                if (parameterForActionRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    parameterForActionRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.SectionTemplate.Count; i++)
            {
                var sectionTemplateRow = (FlexibleFormsDS.SectionTemplateRow)MainDataSet.SectionTemplate.Rows[i];
                if (sectionTemplateRow.RowState == DataRowState.Deleted) continue;
                if (sectionTemplateRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    sectionTemplateRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.ParameterTemplate.Count; i++)
            {
                var parameterTemplateRow = (FlexibleFormsDS.ParameterTemplateRow)MainDataSet.ParameterTemplate.Rows[i];
                if (parameterTemplateRow.RowState == DataRowState.Deleted) continue;
                if (parameterTemplateRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    parameterTemplateRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.Lines.Count; i++)
            {
                var linesRow = (FlexibleFormsDS.LinesRow)MainDataSet.Lines.Rows[i];
                if (linesRow.RowState == DataRowState.Deleted) continue;
                if (linesRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    linesRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            for (int i = 0; i < MainDataSet.Labels.Count; i++)
            {
                var labelsRow = (FlexibleFormsDS.LabelsRow)MainDataSet.Labels.Rows[i];
                if (labelsRow.RowState == DataRowState.Deleted) continue;
                if (labelsRow.idfsFormTemplate.Equals(oldTemplateID.Value))
                {
                    labelsRow.idfsFormTemplate = templatesRow.idfsFormTemplate;
                }
            }

            #endregion
        }

        /// <summary>
        /// ��������� ������� ParameterSelectList, � ������� ���������� ������ �������� ��� ������ �� ����� ��������
        /// </summary>
        ///<param name="idfsParameter"></param>
        ///<param name="haCode"></param>
        /// <param name="idfsParameterType"></param>
        /// <returns></returns>
        public FlexibleFormsDS.ParameterSelectListRow[] LoadParameterSelectList(long idfsParameter, long idfsParameterType, long? haCode)
        {
            var langid = ModelUserContext.CurrentLanguage;
            var intHACode = haCode.HasValue ? haCode.Value : 0;

            //���� �� ���� ���������� �� ���� ��������� -- ���������
            var filter = DataHelper.Filter(
                "idfsParameter", idfsParameter
                , "idfsParameterType", idfsParameterType
                , "langid", String.Format("'{0}'", langid)
                , "intHACode", intHACode);

            var table = new FlexibleFormsDS.ParameterSelectListDataTable();
            m_ParameterSelectListTableAdapter.Fill(table, langid, idfsParameter, idfsParameterType, intHACode);
            MainDataSet.ParameterSelectList.Merge(table);

            //if (MainDataSet.ParameterSelectList.Select(filter).Length == 0)
            //{
            //    m_ParameterSelectListTableAdapter.Fill(MainDataSet.ParameterSelectList, langid, idfsParameter, idfsParameterType, intHACode);
            //}

            var filterEmpty = DataHelper.Filter("idfsParameter", idfsParameter, "idfsParameterType", idfsParameterType, "langid", String.Format("'{0}'", langid), "idfsValue", -1);
            if (MainDataSet.ParameterSelectList.Select(filterEmpty).Length == 0)
            {
                var row = MainDataSet.ParameterSelectList.NewParameterSelectListRow();
                row.idfsParameter = idfsParameter;
                row.idfsParameterType = idfsParameterType;
                row.idfsReferenceType = 0;
                row.idfsValue = -1; //�������� ��������
                row.intOrder = -1; //����� ������ ���� �������
                row.intRowStatus = 0;
                if (haCode.HasValue) row.intHACode = haCode.Value;
                row.strValueCaption = String.Empty;
                row.langid = ModelUserContext.CurrentLanguage;
                MainDataSet.ParameterSelectList.AddParameterSelectListRow(row);
            }
            var filter2 = DataHelper.Filter(
                "idfsParameter", idfsParameter
                , "idfsParameterType", idfsParameterType
                , "langid", String.Format("'{0}'", langid));
            return (FlexibleFormsDS.ParameterSelectListRow[])MainDataSet.ParameterSelectList.Select(filter2, "[intOrder] ASC, [strValueCaption] ASC");
        }

        /// <summary>
        /// ��������� ����������������� ������� �� ��������� �������
        /// </summary>
        /// <param name="matrixType"></param>
        /// <param name="versions"></param>
        /// <param name="idfsFormTemplate"></param>
        /// <returns></returns>
        public FlexibleFormsDS.PredefinedStubRow[] LoadPredefinedStub(long matrixType, List<long> versions, long? idfsFormTemplate)
        {
            var sb = new StringBuilder();
            foreach (var version in versions)
            {
                sb.AppendFormat("{0};", version);
            }
            m_PredefinedStubTableAdapter.Fill(MainDataSet.PredefinedStub, matrixType, sb.ToString(), ModelUserContext.CurrentLanguage, idfsFormTemplate);
            
            return (FlexibleFormsDS.PredefinedStubRow[])MainDataSet.PredefinedStub.Select();
        }

        /// <summary>
        /// ��������� idfsFormTemplate �� ������� �������������
        /// </summary>
        /// <param name="countryDeterminant">�����������-������</param>
        /// <param name="secondDeterminant">����������� ������� ��� ����</param>
        /// <param name="idfsFormType">idfsFormType (��� �����)</param>
        /// <returns>idfsFormTemplate, ���� ������� ��� �����, null, ���� �� ������� �����</returns>
        public void LoadActualTemplate(long countryDeterminant, long? secondDeterminant, long idfsFormType)
        {
            long? idfsFormTemplateActual;
            m_ActualTemplateTableAdapter.Fill(MainDataSet.ActualTemplate, countryDeterminant, secondDeterminant,
                                            idfsFormType, out idfsFormTemplateActual);
        }

        /// <summary>
        /// ��������� idfsFormTemplate �� ������� �������������
        /// </summary>
        /// <param name="countryDeterminant">�����������-������</param>
        /// <param name="secondDeterminant">����������� ������� ��� ����</param>
        /// <param name="idfsFormType">idfsFormType (��� �����)</param>
        /// <param name="idfsFormTemplateActual"> </param>
        /// <returns>idfsFormTemplate, ���� ������� ��� �����, null, ���� �� ������� �����</returns>
        public void LoadActualTemplate(long countryDeterminant, long? secondDeterminant, long idfsFormType, out long? idfsFormTemplateActual)
        {
            m_ActualTemplateTableAdapter.Fill(MainDataSet.ActualTemplate, countryDeterminant, secondDeterminant,
                                            idfsFormType, out idfsFormTemplateActual);
        }

        /// <summary>
        /// ��������� �������� Observations
        /// </summary>
        /// <returns></returns>
        public FlexibleFormsDS.ObservationsRow[] LoadObservations(IEnumerable<long> observations)
        {
            var observationsList = observations.ParseToStringList();

            MainDataSet.Observations.Clear();

            if (observationsList.Count > 1)
            {
                for (var i = 0; i < observationsList.Count; i++)
                {
                    using (var dt = new FlexibleFormsDS.ObservationsDataTable())
                    {
                        m_ObservationsTableAdapter.Fill(dt, observationsList[i]);
                        MainDataSet.Observations.Merge(dt);
                    }
                }
            }
            else if (observationsList.Count == 1)
            {
                m_ObservationsTableAdapter.Fill(MainDataSet.Observations, observationsList[0]);
            }
           
            return (FlexibleFormsDS.ObservationsRow[])MainDataSet.Observations.Select();
        }

        /// <summary>
        /// ��������� �������� Observations
        /// </summary>
        /// <returns></returns>
        public FlexibleFormsDS.ObservationsRow LoadObservations(long idfObservation)
        {
            var list = new List<long>(1) {idfObservation};
            var observationsRows = LoadObservations(list);
            return observationsRows.Length == 1 ? observationsRows[0] : null;
        }

        /// <summary>
        /// ��������� �������� ������� ��� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        public void LoadLabels(long idfsFormTemplate)
        {
            LoadLabels(idfsFormTemplate, ModelUserContext.CurrentLanguage);
        }

        /// <summary>
        /// ��������� �������� ������� ��� �������
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="langid"></param>
        public void LoadLabels(long idfsFormTemplate, string langid)
        {
            var filterString = DataHelper.Filter("idfsFormTemplate", idfsFormTemplate, "langid", String.Format("'{0}'", langid));
            if (DataHelper.NeedRefreshDataTable(MainDataSet.Labels, filterString))
            {
                m_LablesTableAdapter.Fill(MainDataSet.Labels, langid, idfsFormTemplate);
            }
        }

        /// <summary>
        /// ������� ������ �� �������-�������
        /// </summary>
        public void DeleteSummaryTemplate()
        {
            this.DeleteSectionTemplate(IdfsFormTemplateSummary, String.Empty);
            this.DeleteParameterTemplate(IdfsFormTemplateSummary, String.Empty);
            this.DeleteLabelTemplate(IdfsFormTemplateSummary, String.Empty);
            this.DeleteLineTemplate(IdfsFormTemplateSummary, String.Empty);
        }

        /// <summary>
        /// �������� �������� ����� �������������
        /// </summary>
        /// <param name="idfsFormType"></param>
        public void LoadDeterminantTypes(long idfsFormType)
        {
            //using (DbManagerProxy manager = DbManagerFactory.Factory.Create())
            //{
            //    m_DeterminantTypesTableAdapter.Connection = (SqlConnection)manager.Connection;
                m_DeterminantTypesTableAdapter.Fill(MainDataSet.DeterminantTypes, idfsFormType, ModelUserContext.CurrentLanguage);
            //}
        }

        /// <summary>
        /// �������� ������ ��� ����������� ������. ���� �� ����� ��������, ���� �� ���������, ������ �� ���� �����
        /// </summary>
        /// <param name="idfsFormTemplate"></param>
        /// <param name="idfsFormType"></param>
        /// <param name="idfsSection"></param>
        /// <param name="idfsMatrixType"></param>
        /// <returns></returns>
        public void GetSectionForAggregateCase(long? idfsFormTemplate, long idfsFormType, out long? idfsSection, out long? idfsMatrixType)
        {
            idfsSection = idfsMatrixType = null;
            if (idfsFormTemplate.HasValue) CommandGetSectionForAggregateCase.Parameters["@idfsFormTemplate"].Value = idfsFormTemplate.Value;
            CommandGetSectionForAggregateCase.Parameters["@idfsFormType"].Value = idfsFormType;
            ExecCommand(CommandGetSectionForAggregateCase, Connection, null, true);
            var ids = CommandGetSectionForAggregateCase.Parameters["@idfsSection"].Value;
            var idm = CommandGetSectionForAggregateCase.Parameters["@idfsMatrixType"].Value;
            if (!Utils.IsEmpty(ids)) idfsSection = (long?)ids;
            if (!Utils.IsEmpty(idm)) idfsMatrixType = (long?)idm;
        }

        private SqlCommand m_CommandGetSectionForAggregateCase;
        /// <summary>
        /// �������� ������ ��� ������� � ���� ����� (��. ������)
        /// </summary>
        private SqlCommand CommandGetSectionForAggregateCase
        {
            get
            {
                if (m_CommandGetSectionForAggregateCase == null)
                {
                    #region �������� ���������� ��� ���������

                    m_CommandGetSectionForAggregateCase = new SqlCommand
                    {
                        CommandText = "dbo.spFFGetSectionForAggregateCase",
                        Connection = (SqlConnection)Connection,
                        CommandType = CommandType.StoredProcedure
                    };
                    m_CommandGetSectionForAggregateCase.Parameters.Add(new SqlParameter("@idfsFormTemplate", SqlDbType.BigInt,
                                                                                  8, ParameterDirection.Input, 19, 0,
                                                                                  "idfsFormTemplate",
                                                                                  DataRowVersion.Current, false, null,
                                                                                  "", "", ""));
                    m_CommandGetSectionForAggregateCase.Parameters.Add(new SqlParameter("@idfsFormType", SqlDbType.BigInt,
                                                                                  8, ParameterDirection.Input, 19, 0,
                                                                                  "idfsFormType",
                                                                                  DataRowVersion.Current, false, null,
                                                                                  "", "", ""));
                    m_CommandGetSectionForAggregateCase.Parameters.Add(new SqlParameter("@idfsSection", SqlDbType.BigInt,
                                                                                  8, ParameterDirection.Output, 19, 0,
                                                                                  "idfsSection",
                                                                                  DataRowVersion.Current, false, null,
                                                                                  "", "", ""));
                    m_CommandGetSectionForAggregateCase.Parameters.Add(new SqlParameter("@idfsMatrixType", SqlDbType.BigInt,
                                                                                  8, ParameterDirection.Output, 19, 0,
                                                                                  "idfsMatrixType",
                                                                                  DataRowVersion.Current, false, null,
                                                                                  "", "", ""));
                    #endregion
                }
                 
                return m_CommandGetSectionForAggregateCase;
            }
        }
    }
}