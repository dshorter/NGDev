﻿using System;
using eidss.model.Schema;
using eidss.winclient.Core;
using eidss.winclient.Schema;

namespace eidss.winclient.Lab
{
    public partial class LaboratorySectionCreateAliquot : BaseDetailPanel_LaboratorySectionItem
    {
        public LaboratorySectionCreateAliquot()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DefineBinding()
        {
            var item = BusinessObject as LaboratorySectionItem;
            if (item == null) return;

            LookupBinder.BindSpinEdit(seNumber, item, "intNewSample");
            LookupBinder.BindDate(dtAccessionDate, item, "datAccession");
        }


    }
}
