using System;
using System.Windows.Forms;
using bv.common;
using bv.common.Core;
using bv.common.db;
using bv.common.win;
using bv.winclient.BasePanel;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using EIDSS.FlexibleForms.DataBase;
using EIDSS.FlexibleForms.Helpers;
using bv.winclient.Core;
using eidss.model.Resources;


namespace EIDSS.FlexibleForms
{
    public partial class FFParameterTypesEditor : BaseDetailList
    {
        public FFParameterTypesEditor()
        {
            InitializeComponent();
            IgnoreAudit = true;
            DbService = new DBServiceParameterTypeEditor();
            PermissionObject = eidss.model.Enums.EIDSSPermissionObject.FlexibleForm;
            if (Localizer.IsRtl)
            {
                panelControl1.Dock = DockStyle.Right;
                splitterControl1.Dock = DockStyle.Right;
            }
        }

        /// <summary>
        /// ����������� ������ ���������
        /// </summary>
        /// <param name="parentControl"></param>
        public static void Register(Control parentControl)
        {
            var category = MenuActionManager.Instance.FindAction("MenuConfiguration", MenuActionManager.Instance.System, 960);
            new MenuAction(ShowMe, MenuActionManager.Instance, category, "MenuParameterTypesEditor", 1000, false, (int)eidss.model.Enums.MenuIconsSmall.ParameterTypesEditor)
                                {
                                    SelectPermission = PermissionHelper.SelectPermission(eidss.model.Enums.EIDSSPermissionObject.FlexibleForm),
                                    BeginGroup = true
                                };
        }

        /// <summary>
        /// ������ ���������
        /// </summary>
        public static void ShowMe()
        {
            BaseFormManager.ShowNormal(new FFParameterTypesEditor(), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool HasChanges()
        {
            return (MainDbService.MainDataSet.ParameterReferenceType.GetChanges() != null ||
                    MainDbService.MainDataSet.ParameterFixedPresetValue.GetChanges() != null
                    );
        }

        /// <summary>
        /// ������ �� ������� �������
        /// </summary>
        private DBServiceParameterTypeEditor MainDbService { get { return (DBServiceParameterTypeEditor)DbService; } }

        /// <summary>
        /// 
        /// </summary>
        protected override void DefineBinding()
        {
            //��������� �������� ��������� ����� ����������. ������������ ���� �� ������.
            MainDbService.LoadParameterReferenceTypes();
            RefreshParameterTypes();
            //��������� �������� Reference Type
            leReferenceType.Properties.DataSource = MainDbService.LoadReferenceTypes();
            if (MainDbService.MainDataSet.ReferenceTypes.Count > 0)
            {
                //leReferenceType.EditValue = MainDbService.MainDataSet.ReferenceTypes[0].idfsReferenceType;
            }
            //
            icbParameterTypeSystem.SelectedValueChanged += OnIcbParameterTypeSystemSelectedValueChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnIcbParameterTypeSystemSelectedValueChanged(object sender, EventArgs e)
        {
            var rowParameterType = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;
            if (rowParameterType == null ||
                leReferenceType.EditValue == null)
                return;
            //
            var imageComboBoxEdit = sender as ImageComboBoxEdit;
            if (imageComboBoxEdit == null ||
                imageComboBoxEdit.EditValue == null)
                return;

            bool isFixedPresed = imageComboBoxEdit.EditValue.ToString().Equals("0");
            if (!isFixedPresed)
            {
                //���� ���������� ������� ���� ��������� �� ����������������� �������� � �����������, �� ����� ������� ���������
                //����������� ��������, � ����� ������� ��� ����������������� ��������
                //������� ������ �������� ����
                if (!CheckParameterTypeChildren(rowParameterType.idfsParameterType))
                {
                    imageComboBoxEdit.SelectedIndex = 0; //���������� �������
                    return;
                }
                //������ �������� ����
                for (var i = MainDbService.MainDataSet.ParameterFixedPresetValue.Count - 1; i >= 0; i--)
                {
                    var row = MainDbService.MainDataSet.ParameterFixedPresetValue[i];
                    if (row.IsRowAlive()) row.Delete();
                }

                //���������� ����� ����������� ���
                rowParameterType.idfsReferenceType = (long)leReferenceType.EditValue;
                RefreshReferenceTypesList(rowParameterType.idfsReferenceType);
            }
            else
            {
                rowParameterType.idfsReferenceType = (long)BaseReferenceType.rftParametersFixedPresetValue;
                RefreshFixedPresetValues(rowParameterType.idfsParameterType);
            }
            SetWindowState(isFixedPresed);
        }

        /// <summary>
        /// ��������� ��� ������� ��� ���� ���������
        /// </summary>
        /// <param name="idfsParameterType"></param>
        /// <returns></returns>
        private bool CheckParameterTypeChildren(long idfsParameterType)
        {
            var result = true;
            var fixedPresetValueRows = DataHelper.GetParameterFixedPresetValues(MainDbService, idfsParameterType);
            if (fixedPresetValueRows != null)
            {
                foreach (var row in fixedPresetValueRows)
                {
                    //�������� �� ����������� ��������
                    if (row.IsRowAlive() && !CheckRemoveFixedPresetValue(row.idfsParameterFixedPresetValue))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshParameterTypes()
        {
            gcParameterTypes.DataSource = MainDbService.MainDataSet.ParameterReferenceType.Select();
        }

        /// <summary>
        /// �������� ��� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddParameterTypeItemClick(object sender, ItemClickEventArgs e)
        {
            var row = MainDbService.MainDataSet.ParameterReferenceType.NewParameterReferenceTypeRow();
            row.idfsReferenceType = (long)BaseReferenceType.rftParametersFixedPresetValue;
            row.DefaultName = row.NationalName = String.Empty;
            row.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
            row.System = "0";
            MainDbService.MainDataSet.ParameterReferenceType.AddParameterReferenceTypeRow(row);
            //
            RefreshParameterTypes();
            //
            RefreshFixedPresetValues(row.idfsParameterType);
            //
            SetWindowState(true);
        }

        /// <summary>
        /// �������� Parameter Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnRemoveParameterTypeItemClick(object sender, ItemClickEventArgs e)
        {
            if (gvParameterTypes.IsEditing)
                return;

            //�������� Parameter Type
            var rowParameterType = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;

            if (rowParameterType != null &&
                CheckRemoveParameterType(rowParameterType.idfsParameterType))
            {
                //������� ��������� ��������
                for (var i = MainDbService.MainDataSet.ParameterFixedPresetValue.Count - 1; i >= 0; i--)
                {
                    var row = MainDbService.MainDataSet.ParameterFixedPresetValue[i];
                    if (row.IsRowAlive() && row.idfsParameterType == rowParameterType.idfsParameterType)
                        row.Delete();
                }

                //������� ������
                rowParameterType.Delete();

                RefreshParameterTypes();
            }
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnAddFixedPresetValueItemClick(object sender, ItemClickEventArgs e)
        {
            var rowParameterType = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;
            if (rowParameterType != null)
            {
                var row = MainDbService.MainDataSet.ParameterFixedPresetValue.NewParameterFixedPresetValueRow();

                row.DefaultName = row.NationalName = String.Empty;
                row.langid = bv.model.Model.Core.ModelUserContext.CurrentLanguage;
                row.idfsParameterType = rowParameterType.idfsParameterType;
                MainDbService.MainDataSet.ParameterFixedPresetValue.AddParameterFixedPresetValueRow(row);

                RefreshFixedPresetValues(rowParameterType.idfsParameterType);
            }
        }

        /// <summary>
        /// �������� ����������� �������� �������
        /// </summary>
        /// <param name="idfsParameterFixedPresetValue"></param>
        /// <returns></returns>
        private bool CheckRemoveFixedPresetValue(long idfsParameterFixedPresetValue)
        {
            bool result = true;

            string commandText = String.Format("Select [ErrorMessage] from dbo.fnFFCheckForRemoveParameterFixedPresetValue({0})", idfsParameterFixedPresetValue);
            var cmd = DbService.CreateCommand(commandText);
            ErrorMessage errMsg = null;
            var errorMessage = BaseDbService.ExecScalar(cmd, DbService.Connection, ref errMsg);
            //���� ������ �� �������, �� ������ �� �����
            if (errorMessage is string)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get(errorMessage.ToString()));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// �������� ����������� �������� ���� ���������
        /// </summary>
        /// <param name="idfsParameterType"></param>
        /// <returns></returns>
        private bool CheckRemoveParameterType(long idfsParameterType)
        {
            //������� �� �� �������� ���� ���������� � ������ ����� ��� ���������
            MainDbService.LoadParameters(null, null);
            bool result = MainDbService.MainDataSet.Parameters.Select(DataHelper.Filter("idfsParameterType", idfsParameterType)).Length == 0;
            if (!result)
            {
                ErrorForm.ShowMessageDirect(EidssMessages.Get("ParameterTypeRemove_Has_ffParameter"));
            }
            return result;
        }

        /// <summary>
        /// �������� ������������������ ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnRemoveFixedPresetValueItemClick(object sender, ItemClickEventArgs e)
        {
            if (gvFixedPresetValues.IsEditing) return;
            //���������� ��������� ������
            var row = gvFixedPresetValues.GetFocusedRow() as FlexibleFormsDS.ParameterFixedPresetValueRow;
            if (row == null) return;

            //�������� �� ����������� ��������
            if (!CheckRemoveFixedPresetValue(row.idfsParameterFixedPresetValue)) return;

            //�������� Parameter Type
            var rowParameterType = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;
            if (rowParameterType != null)
            {
                //������� ������
                row.Delete();

                RefreshFixedPresetValues(rowParameterType.idfsParameterType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsParameterType"></param>
        /// <param name="autoSort"></param>
        private void RefreshFixedPresetValues(long idfsParameterType, bool autoSort = true)
        {
            if (autoSort)
                RefreshOrders();

            gcFixedPresetValues.DataSource = DataHelper.GetParameterFixedPresetValues(MainDbService, idfsParameterType);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshOrders()
        {
            //��������, ����� �������� ������� ���������� ���� ����������
            //��� ��� ������������� � ���������� �������
            for (int i = 0; i < MainDbService.MainDataSet.ParameterFixedPresetValue.Count; i++)
            {
                var row = MainDbService.MainDataSet.ParameterFixedPresetValue[i];
                if (row.IsRowAlive())
                {
                    if (row.IsintOrderNull() || !row.intOrder.Equals(i))
                        row.intOrder = i;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private FlexibleFormsDS.ParameterFixedPresetValueRow GetSelectedRow()
        {
            return gvFixedPresetValues.GetFocusedRow() as FlexibleFormsDS.ParameterFixedPresetValueRow;
        }

        /// <summary>
        /// ����� ������ � ������� ����� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGvParameterTypesFocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //��������� �������� ����������������� �������� ��� ��������� ���
            var row = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;
            if (row == null) return;

            //���� ����������������� �������� (0)
            bool isFixedPresetValues = row.System.Equals("0");
            //��������� ������ ����� reference type
            //pnlReferenceType.Visible = !isFixedPresetValues;
            if (isFixedPresetValues)
            {
                MainDbService.LoadParameterFixedPresetValues(row.idfsParameterType);
                //�������� ��, ������� � ���� ���������
                RefreshFixedPresetValues(row.idfsParameterType);
            }
            else
            {
                //���������� reference type
                leReferenceType.EditValue = row.idfsReferenceType;
                //
                //pnlReferenceType.BringToFront();
                //gcFixedPresetValues.BringToFront();
                //��������� �������� ������ ������ ��� ������
                RefreshReferenceTypesList(row.idfsReferenceType);
            }

            //btnAddFixedPresetValue.Enabled = btnRemoveFixedPresetValue.Enabled = isFixedPresetValues;
            //����������� �������������� ������ ��������� ��������
            //gvFixedPresetValues.OptionsBehavior.Editable = isFixedPresetValues;
            //
            SetWindowState(isFixedPresetValues);
        }

        /// <summary>
        /// ���������� �������� � ������ ���������, � ����������� �� ���� ���������
        /// </summary>
        /// <param name="isFixedPresetValues"></param>
        private void SetWindowState(bool isFixedPresetValues)
        {
            btnAddFixedPresetValue.Enabled = btnRemoveFixedPresetValue.Enabled = btnUp.Enabled = btnDown.Enabled = isFixedPresetValues;
            pnlReferenceType.Visible = !isFixedPresetValues;
            if (pnlReferenceType.Visible)
            {
                //
                pnlReferenceType.BringToFront();
                gcFixedPresetValues.BringToFront();
            }
            //����������� �������������� ������ ��������� ��������
            gvFixedPresetValues.OptionsBehavior.Editable = isFixedPresetValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idfsReferenceType"></param>
        private void RefreshReferenceTypesList(long idfsReferenceType)
        {
            MainDbService.LoadReferenceTypesList(idfsReferenceType.Equals((long)BaseReferenceType.rftParametersFixedPresetValue) ? -1 : idfsReferenceType);
            gcFixedPresetValues.DataSource = DataHelper.GetReferenceTypesList(MainDbService, idfsReferenceType);
        }

        /// <summary>
        /// ����� ����������� Reference Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLeReferenceTypeEditValueChanged(object sender, EventArgs e)
        {
            //���������� ��������� ������
            if (leReferenceType.EditValue != null)
            {
                var row = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;
                if (row != null)
                {
                    //������������� ����� ��������
                    var idfsReferenceTypeNew = (long)leReferenceType.EditValue;
                    if (!row.idfsReferenceType.Equals(idfsReferenceTypeNew))
                    {
                        row.idfsReferenceType = (long)leReferenceType.EditValue;
                        //������� �������� ������������ ��������
                        RefreshReferenceTypesList(row.idfsReferenceType);
                    }
                }
            }
        }

        /// <summary>
        /// ����� �� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnUpItemClick(object sender, ItemClickEventArgs e)
        {
            SetOrder(true);
        }

        /// <summary>
        /// ���� �� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnDownItemClick(object sender, ItemClickEventArgs e)
        {
            SetOrder(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetOrder(bool up)
        {
            int index = gvFixedPresetValues.GetFocusedDataSourceRowIndex();
            if ((up && index == 0) ||
                (!up && index == gvFixedPresetValues.RowCount - 1))
                return;

            var row = GetSelectedRow();
            if (row != null)
            {
                int newIndex = up ? index - 1 : index + 1;
                var rowSibling = ((FlexibleFormsDS.ParameterFixedPresetValueRow[])gvFixedPresetValues.DataSource)[newIndex]; //MainDbService.MainDataSet.ParameterFixedPresetValue[newIndex];
                int oldOrder = row.intOrder;
                row.intOrder = rowSibling.intOrder;
                rowSibling.intOrder = oldOrder;
                var rowP = gvParameterTypes.GetFocusedRow() as FlexibleFormsDS.ParameterReferenceTypeRow;
                if (rowP != null)
                {
                    RefreshFixedPresetValues(rowP.idfsParameterType, false);
                    gvFixedPresetValues.FocusedRowHandle = gvFixedPresetValues.GetRowHandle(row.intOrder);
                }
            }
        }

    }
}

