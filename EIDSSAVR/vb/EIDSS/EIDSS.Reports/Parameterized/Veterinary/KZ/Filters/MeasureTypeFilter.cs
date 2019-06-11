﻿using System;
using System.ComponentModel;
using System.Data;
using bv.common.db.Core;
using EIDSS.Reports.BaseControls.Filters;

namespace EIDSS.Reports.Parameterized.Veterinary.KZ.Filters
{
    public partial class MeasureTypeFilter : BaseMultilineFilter
    {
        private readonly ComponentResourceManager m_Resources =
            new ComponentResourceManager(typeof (MeasureTypeFilter));

        private MatrixType m_MatrixType = MatrixType.None;

        public MeasureTypeFilter()
        {
            InitializeComponent();
            SetMandatory();
        }

        [Browsable(true)]
        public MatrixType Matrix
        {
            get { return m_MatrixType; }
            set { m_MatrixType = value; }
        }

        protected override DataView CreateDataSource()
        {
            switch (Matrix)
            {
                case MatrixType.Sanitary:
                    return LookupCache.Get(LookupTables.SanitaryMeasureTypeName);
                case MatrixType.Prophylactic:
                    return LookupCache.Get(LookupTables.ProphylacticMeasureType);
                default:
                    throw new ApplicationException("Matrix type is not set");
            }
        }

        protected override void ApplyResources()
        {
            base.ApplyResources();
            m_Resources.ApplyResources(lblcheckedComboBoxName, "lblcheckedComboBoxName");
            m_Resources.ApplyResources(this, "$this");
        }
    }
}