﻿using System;
using eidss.model.Schema;
using eidss.winclient.Core;
using eidss.winclient.Schema;

namespace eidss.winclient.Lab
{
    public partial class LaboratorySectionTransferOut : BaseDetailPanel_LaboratorySectionItem
    {
        public LaboratorySectionTransferOut()
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

            LookupBinder.BindDate(dtSentDate, item, "datSendDate");
            LookupBinder.BindLookup(leSendToOffice, item, "SendToOfficeOut", item.SendToOfficeOutLookup, false);
        }


    }
}
