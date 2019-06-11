using System;
using System.Windows.Forms;
using bv.common.Core;
using bv.common.win;
using DevExpress.XtraTreeList;
using EIDSS.FlexibleForms.Components;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.Helpers;
using System.Collections.Generic;
using bv.winclient.Core;
using eidss.model.Resources;

namespace EIDSS.FlexibleForms
{
    public partial class FFTreeParameterForTemplate : BaseDetailForm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbService"></param>
        /// <param name="idfsformtype"></param>
        /// <param name="parameterList"></param>
        public FFTreeParameterForTemplate(DbService dbService, FlexibleFormsDS.FormTypesRow idfsformtype, Dictionary<long, Parameter> parameterList)
        {
            InitializeComponent();
            DbService = dbService;
            IgnoreAudit = true;
            formType = idfsformtype;
            this.parameterList = parameterList;
            m_StrEmpty = EidssMessages.Get("strEmpty");
        }

        //  for scan mode only
        public FFTreeParameterForTemplate()
        {
            InitializeComponent();
        }

        private const string StrEmptyKey = "emptykey";
        private readonly string m_StrEmpty;
        private Dictionary<long, Parameter> parameterList { get; set; }
        private FlexibleFormsDS.FormTypesRow formType { get; set; }
        
        private DbService MainDbService { get { return (DbService)DbService; } }

        public List<long> SelectedParameters
        {
            get { return GetSelectedParameters(); }
        }
        
        private List<long> GetSelectedParameters()
        {
            var result = new List<long>();
            for (var i = 0; i < treeParametersLibrary.Selection.Count; i++)
            {
                if (treeParametersLibrary.Selection[i].Tag is FlexibleFormsDS.ParametersRow)
                    result.Add(((FlexibleFormsDS.ParametersRow) treeParametersLibrary.Selection[i].Tag).idfsParameter);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool LoadData(ref object id)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshTreeParameters()
        {
            if (formType == null) return;
            if (!formType.IsRowAlive()) return;

            //� ������ �������� ������ �� ���������, ������� �� ����������� ��������� �������
            //����� � ��������� ��������� ������� ����� ����������
            treeParametersLibrary.Nodes.Clear();
            
            //�� ������ ������ ��������� ���������� ������ � ���������� �� ����� ���� �����
            //������������ ���������� ������
            MainDbService.LoadSections(formType.idfsFormType, null, null);
            //������������ ���������� ����������
            //����� ������ ��, ��� �� ��������� � �����-���� ������, � ������ � ���� �����
            MainDbService.LoadParameters(formType.idfsFormType, null);

            //������� ��������� ������ �� ����� ���� �����
            formType.HasNestedSections = MainDbService.HasNestedSections(formType.idfsFormType);
            formType.HasParameters = MainDbService.HasNestedParameters(formType.idfsFormType);

            //��������� ���� � ����� �����
            AppendFormTypeNode();
        }
       

        /// <summary>
        /// ��������������� ����� ���������� ���� ���� ����� � ������
        /// </summary>
        public void AppendFormTypeNode()
        {
            var node = treeParametersLibrary.AppendNode(null, null, formType);
            node[trlcTreeListParametersColumn] = formType.Name;
            node.ImageIndex = node.SelectImageIndex = 0;
            //���� � ���� ����� ���� ������ ��� ���������, �� ������� ��� ��������� ����
            //��������� ���� ����� ���������� � ����� ������ ������ ���������, �� ����� ��������� ������������ ��� ��������
            if (formType.HasNestedSections || formType.HasParameters)
            {
                var nodeEmpty = treeParametersLibrary.AppendNode(null, node, StrEmptyKey);
                nodeEmpty[trlcTreeListParametersColumn] = m_StrEmpty;
                node.Expanded = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFFTreeParameterForTemplateLoad(object sender, EventArgs e)
        {
            RefreshTreeParameters();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeParametersLibraryBeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            var parentTree = (TreeList)sender;
            var parentNode = e.Node;
            var parentObject = e.Node.Tag;
            //���� � ���� ��� ��������� �������� ����, �� �������� �� �� �����������
            if ((parentNode.FirstNode != null) && (!parentNode.FirstNode.Tag.Equals(StrEmptyKey))) return;

            //������������� ����� ���� ��� �����, ���� ������
            //�������� �� ����� ��������� ��������� ����
            Cursor = Cursors.WaitCursor;

            //���� ������������ ��� �����
            if (parentObject is FlexibleFormsDS.FormTypesRow)
            {
                #region �������� �����������

                //�������� ������, ����������� � ����� ���� ����
                var row = (FlexibleFormsDS.FormTypesRow)parentObject;

                //���������, ��� �� ���� ����� �������, � ��� ���
                if (MainDbService.HasNestedSections(row.idfsFormType))
                {
                    //��������� ������ � ������
                    MainDbService.AppendSectionNodes(parentTree, parentNode, MainDbService.GetSectionsByFormType(row.idfsFormType), true, false);
                }
                if (MainDbService.HasNestedParameters(row.idfsFormType))
                {
                    //��������� ��������� � ������ (�������� ������ ��, ������� ��������� � ���� �����, �� �� ����� ������� ������)
                    HelperFunctions.AppendParameterNodes(parentTree, parentNode,MainDbService.GetParametersByFormType(row.idfsFormType), true);
                }

                #endregion
            }

            //���� ������������ ������
            if (parentObject is FlexibleFormsDS.SectionsRow)
            {
                #region �������� �����������

                //�������� ������, ��� ������� ��� ������ �������� ������������
                var row = (FlexibleFormsDS.SectionsRow)parentObject;

                //������������ ���������� ������
                MainDbService.LoadSections(row.idfsFormType, null, row.idfsSection);
                //������������ ���������� ����������
                //����� ������ ��, ��� �� ��������� � �����-���� ������, � ������ � ���� �����
                MainDbService.LoadParameters(row.idfsFormType, row.idfsSection);

                if (MainDbService.HasNestedSections(row))
                {
                    //��������� ������ � ������
                    MainDbService.AppendSectionNodes(parentTree, parentNode, MainDbService.GetSectionChildrenRows(row.idfsSection), true, false);
                }
                if (MainDbService.HasNestedParameters(row))
                {

                    //��������� ������ � ������
                    HelperFunctions.AppendParameterNodes(parentTree, parentNode, (FlexibleFormsDS.ParametersRow[])MainDbService.MainDataSet.Parameters.Select(DataHelper.Filter("idfsSection", row.idfsSection)), true);

                }

                #endregion
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public override bool Post(bv.common.Enums.PostType postType)
        {
            if (!Utils.IsCalledFromUnitTest() && FindForm() == null)
                return true;
            return m_ClosingMode.Equals(ClosingMode.Cancel) || ValidateData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ValidateData()
        {
            var result =  base.ValidateData();

            if (result)
            {
                //�������� ����� ������ ���������
                //��������, �� ������ �� ��� ���� �������� � ����������� ��� ������������ ����������
                result = (SelectedParameters.Count > 0) && (parameterList != null);
                var alreadyContains = false;
                if (result)
                {
                    foreach (var sp in SelectedParameters)
                    {
                       if (parameterList.ContainsKey(sp))
                       {
                           result = false;
                           alreadyContains = true;
                           break;
                       }
                    }
                }
                if (!result)
                {
                    if (alreadyContains)
                    {
                        ErrorForm.ShowMessageDirect(EidssMessages.Get("ParameterAlreadyInTemplateMessage"));
                        treeParametersLibrary.Selection.Clear();
                    }
                }
            }

            return result;
        }
    }
}

