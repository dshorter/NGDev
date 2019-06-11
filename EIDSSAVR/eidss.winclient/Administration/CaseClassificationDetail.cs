﻿using System;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using bv.common.Core;
using bv.model.BLToolkit;
using bv.model.Model.Core;
using eidss.model.Schema;
using eidss.winclient.Core;
using eidss.winclient.Helpers;
using eidss.winclient.Schema;

namespace eidss.winclient.Administration
{
    public partial class CaseClassificationDetail : BaseGroupPanel_CaseClassification
    {
        public CaseClassificationDetail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dummyObjectWithLookups"></param>
        /// <param name="manager"></param>
        public override void InitGridBehavior(IObject dummyObjectWithLookups, DbManagerProxy manager)
        {
            var matrix = dummyObjectWithLookups as CaseClassification;
            if (matrix == null) return;
            var column = Grid.GridView.Columns.ColumnByName("intHACode");
            if (column != null)
            {
                LookupBinder.BindRepositoryHACodeLookup(column.SetPopupContainerEdit(), null, 226);
                column.SortMode = ColumnSortMode.DisplayText;
            }
            column = Grid.GridView.Columns.ColumnByName("CaseClassificationNameTranslated");
            if (column != null && ModelUserContext.CurrentLanguage == Localizer.lngEn)
                column.Visible = false;
            Grid.GridView.ValidateRow += OnGridViewValidateRow;
            TopPanelVisible = false;

        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGridViewValidateRow(object sender, ValidateRowEventArgs e)
        {
            /*
            //var bo = BusinessObject as DiagnosisAgeGroupMaster;
            //if (bo == null) return;
            var row = Grid.GridView.GetFocusedRow() as DiagnosisAgeGroup;
            if (row == null) return;
            using (var manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                var acc = DiagnosisAgeGroup.Accessor.Instance(null);
                var valid = acc.Validate(manager, row, true, true, true);
               
                if (!valid)
                {
                    e.ErrorText = "saadasd";
                    //TODO как более развёрнутое сообщение получить из валидатора
                    e.Valid = false;
                }
            }


            //if (!(row.intLowerBoundary < row.intUpperBoundary)) e.Valid = false;
            //if (row.DiagnosisAgeGroupNameTranslated.Length == 0) e.Valid = false;
            //if (row.DiagnosisAgeGroupName.Length == 0) e.Valid = false;


            //if (column.Name.Equals(ColLowerBoundary))
            //{
            //    intLowerBoundary = Convert.ToInt32(e.Value ?? 0);
            //    intUpperBoundary = row.intUpperBoundary ?? 0;
            //    needCheck = row.intUpperBoundary.HasValue;
            //}
            //else if (column.Name.Equals(ColUpperBoundary))
            //{
            //    intLowerBoundary = row.intLowerBoundary;
            //    intUpperBoundary = Convert.ToInt32(e.Value ?? 0);
            //    needCheck = row.intLowerBoundary > 0;
            //}
            //else return;

            //if (!needCheck) return;

            //if ((intLowerBoundary >= 0) && (intUpperBoundary > 0))
            //{
            //    if (!(intLowerBoundary < intUpperBoundary)) e.Valid = false;
            //}
             */
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void OnGridViewValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        //{
        //    var row = Grid.GridView.GetFocusedRow() as DiagnosisAgeGroup;
        //    if (row == null) return;
        //    var column = Grid.GridView.FocusedColumn;
        //    int intLowerBoundary;
        //    int intUpperBoundary;
        //    bool needCheck;
        //    if (column.Name.Equals(ColLowerBoundary))
        //    {
        //        intLowerBoundary = Convert.ToInt32(e.Value ?? 0);
        //        intUpperBoundary = row.intUpperBoundary ?? 0;
        //        needCheck = row.intUpperBoundary.HasValue;
        //    }
        //    else if (column.Name.Equals(ColUpperBoundary))
        //    {
        //        intLowerBoundary = row.intLowerBoundary;
        //        intUpperBoundary = Convert.ToInt32(e.Value ?? 0);
        //        needCheck = row.intLowerBoundary > 0;
        //    }
        //    else return;

        //    if (!needCheck) return;

        //    if ((intLowerBoundary >= 0) && (intUpperBoundary > 0))
        //    {
        //        if (!(intLowerBoundary < intUpperBoundary)) e.Valid = false;
        //    }
        //}

        
        private long? m_IdfsVectorType;

        /// <summary>
        /// 
        /// </summary>
        public long? idfsVectorType
        {
            get { return m_IdfsVectorType; }
            set
            {
                m_IdfsVectorType = value;
                Refresh(DataSource,
                    m_IdfsVectorType.HasValue
                    ? String.Format("idfsVectorType={0}", m_IdfsVectorType)
                    : "idfsVectorType=-1");
            }
        }

        
        //private void InitAgeGroupRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        //{
        //    var obj = Grid.GridView.GetRow(e.RowHandle);
        //    ((DiagnosisAgeGroup)obj).AgeType = ((DiagnosisAgeGroup)obj).AgeTypeLookup.SingleOrDefault(c => c.idfsBaseReference == 10042003);/*Years*/
        //}
        

    }
}
