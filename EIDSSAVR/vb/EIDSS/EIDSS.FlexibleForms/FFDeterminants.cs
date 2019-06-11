using System;
using System.Windows.Forms;
using bv.common;
using bv.common.Core;
using bv.common.Enums;
using bv.common.win;
using DevExpress.XtraTreeList.Nodes;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.Helpers;
using bv.winclient.Core;
using eidss.model.Resources;

namespace EIDSS.FlexibleForms
{
    public partial class FFDeterminants : BaseDetailForm
    {
        /// <summary>
        /// 
        /// </summary>
        public FFDeterminants()
        {
            InitializeComponent();
            if(Localizer.IsRtl)
                treeDeterminant.Dock = DockStyle.Right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="tr"></param>
        public FFDeterminants(DbService service, FlexibleFormsDS.TemplatesRow tr)
        {
            InitializeComponent();
            templatesRow = tr;
            MainDbService = service;
            UseParentDataset = true;
        }

        private FlexibleFormsDS.TemplatesRow templatesRow { get; set; }

        /// <summary>
        /// ������ �� ������� ��� Flexible Forms
        /// </summary>
        public DbService MainDbService { get; private set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFFDeterminantsLoad(object sender, EventArgs e)
        {
            RefreshDeterminantsTree();
        }

        /// <summary>
        /// ������ ������ �������������
        /// </summary>
        private void RefreshDeterminantsTree()
        {
            //������ ������ ����� ��, � �� �����������
            treeDeterminant.Nodes.Clear();
            if (treeDeterminant.Nodes.Count > 0) return;
            //�������� �������� �������������, ������� ���� � ��
            MainDbService.LoadDeterminants();
            //��������� ��������� ���� ������������� ��� ������� ���� �����
            MainDbService.LoadDeterminantTypes(templatesRow.idfsFormType);

            //������ ������� ����
            //������
            /*
            if (MainDbService.HasDeterminantType(FFDeterminantTypes.Country))
            {
                var countries = MainDbService.GetDeterminants(FFDeterminantTypes.Country);
                if (countries.Length > 0)
                {
                    var nodeCountry = treeDeterminant.AppendNode(null, null, null);
                    nodeCountry[0] = countries[0].NationalTypeName;
                    foreach (var row in countries)
                    {
                        var node = treeDeterminant.AppendNode(null, nodeCountry, row);
                        node[0] = row.NationalName;
                    }
                }
            }
            */
            //��������
            if (MainDbService.HasDeterminantType(FFDeterminantTypes.Diagnosis))
            {
                var diagnoses = MainDbService.GetDeterminants(FFDeterminantTypes.Diagnosis);
                if (diagnoses.Length > 0)
                {
                    var nodeDiagnosis = treeDeterminant.AppendNode(null, null, null);
                    nodeDiagnosis[0] = diagnoses[0].NationalTypeName;
                    foreach (var row in diagnoses)
                    {
                        var node = treeDeterminant.AppendNode(null, nodeDiagnosis, row);
                        node[0] = row.NationalName;
                    }
                }
            }

            //�����
            if (MainDbService.HasDeterminantType(FFDeterminantTypes.Test))
            {
                var tests = MainDbService.GetDeterminants(FFDeterminantTypes.Test);
                if (tests.Length > 0)
                {
                    var nodeTest = treeDeterminant.AppendNode(null, null, null);
                    nodeTest[0] = tests[0].NationalTypeName;
                    foreach (var row in tests)
                    {
                        var node = treeDeterminant.AppendNode(null, nodeTest, row);
                        node[0] = row.NationalName;
                    }
                }
            }

            //���� ��������
            if (MainDbService.HasDeterminantType(FFDeterminantTypes.VectorType))
            {
                var vectorTypes = MainDbService.GetDeterminants(FFDeterminantTypes.VectorType);
                if (vectorTypes.Length > 0)
                {
                    var nodeVectorType = treeDeterminant.AppendNode(null, null, null);
                    nodeVectorType[0] = vectorTypes[0].NationalTypeName;
                    foreach (var row in vectorTypes)
                    {
                        var node = treeDeterminant.AppendNode(null, nodeVectorType, row);
                        node[0] = row.NationalName;
                    }
                }
            }

            //���������� �������� �������� ��� �������� ������ � ��������������
            cbSearchParameterCriteria.Properties.Items.Clear();
            foreach(var row in MainDbService.MainDataSet.Determinants)
            {
                //TODO ���� ��� ����� �� �����, ����� ������������-������ ����� ������� �� ��
                if (row.idfsReferenceType == (long)FFDeterminantTypes.Country) continue;
                cbSearchParameterCriteria.Properties.Items.Add(row.NationalName);
            }
        }

        /// <summary>
        /// ����� ������������ �� ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBSearchParameterStartClick(object sender, EventArgs e)
        {
            if (cbSearchParameterCriteria.Text.Length == 0) return;
            //������ ������������� ����������� �� �����
            //��������� ����� �� ���� ���� ���������� �������������
            lbSearchParameterResults.Items.Clear();
            for (int i = 0; i < treeDeterminant.Nodes.Count; i++)
            {
                FindDeterminant(cbSearchParameterCriteria.Text, treeDeterminant.Nodes[i]);
            }
            //������ ����� �� ������ ����
            if (lbSearchParameterResults.Items.Count > 0)
            {
                OnLbSearchParameterResultsClick(sender, e);
            }
        }

        /// <summary>
        /// ��������� ������ � �������������
        /// </summary>
        public FlexibleFormsDS.DeterminantsRow SelectedDeterminantsRow { get; private set; }

        /// <summary>
        /// ���������� ����������� ����� �������� ����� ����
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="parentNode"></param>
        private void FindDeterminant(string searchString, TreeListNode parentNode)
        {
            foreach(TreeListNode node in parentNode.Nodes)
            {
                string thisNode = node[0].ToString().ToUpperInvariant();
                string ss = searchString.ToUpperInvariant();

                if (thisNode.Contains(ss))
                {
                    lbSearchParameterResults.Items.Add(String.Format("{0} > {1}", parentNode[0], node[0]));
                }
            }
        }

        /// <summary>
        /// ����� ������������ �� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLbSearchParameterResultsClick(object sender, EventArgs e)
        {
            if (lbSearchParameterResults.SelectedItem == null) return;
            //���������� ������ �������� ����, ������� ���� �������� � �� ������� ���� ������
            var pathElements = lbSearchParameterResults.SelectedItem.ToString().Split(new[] { ">" }, StringSplitOptions.RemoveEmptyEntries);
            if (pathElements.Length < 2) return;
            //���������� ������������ ����, � � ��� ��� ��, ��� ������
            var parentNode = treeDeterminant.Nodes.FindNode(pathElements[0]);
            if (parentNode == null) return;
            var node = parentNode.Nodes.FindNode(pathElements[1]);
            treeDeterminant.Selection.Clear();
            treeDeterminant.Selection.Add(node);
            treeDeterminant.MakeNodeVisible(node);
            //����������� ������
            SelectedDeterminantsRow = (FlexibleFormsDS.DeterminantsRow)node.Tag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeDeterminantClick(object sender, EventArgs e)
        {
            if (treeDeterminant.Selection.Count == 0) return;
            if (treeDeterminant.Selection[0].Tag == null) return;
            //����������� ������
            SelectedDeterminantsRow = (FlexibleFormsDS.DeterminantsRow)treeDeterminant.Selection[0].Tag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeDeterminantKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            treeDeterminant.ProcessKeyUpForTree(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool cmdClose_Click()
        {
            if (m_ClosingMode == ClosingMode.Cancel) SelectedDeterminantsRow = null;
            return base.cmdClose_Click();
        }

        /// <summary>
        /// �������� ������������� �� ������-��������
        /// </summary>
        /// <returns></returns>
        private bool CheckDeterminants()
        {
            if (SelectedDeterminantsRow == null) return true;

            //�������� ������ �� ����� �������
            if (templatesRow.blnUNI)
            {
                if (SelectedDeterminantsRow.idfsReferenceType.Equals((long)FFDeterminantTypes.Country) && (MainDbService.GetTemplateDeterminants(templatesRow.idfsFormTemplate, FFDeterminantTypes.Country).Length > 0))
                {
                    ErrorForm.ShowMessageDirect(EidssMessages.Get("Determinants_Already_Have_Country"));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public override bool Post(PostType postType)
        {
            if (!Utils.IsCalledFromUnitTest() && FindForm() == null)
                return true;
            return m_ClosingMode != ClosingMode.Ok || CheckDeterminants();
        }
    }
}

