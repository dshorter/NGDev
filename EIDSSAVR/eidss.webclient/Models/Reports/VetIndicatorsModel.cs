﻿using System;
using System.Collections.Generic;
using bv.common.Core;
using eidss.model.Core;
using eidss.model.Reports.AZ;
using eidss.model.Reports.Common;

namespace eidss.webclient.Models.Reports
{
    [Serializable]
    public class VetIndicatorsModel : BaseModel
    {

        public VetIndicatorsModel()
        {
            StartYear = EndYear = DateTime.Today.Year;
        }
        [LocalizedDisplayName("FromYearLabel")]
        public int StartYear { get; set; }

        [LocalizedDisplayName("ToYearLabel")]
        public int EndYear { get; set; }

        [LocalizedDisplayName("FromMonthLabel")]
        public int? StartMonth { get; set; }

        [LocalizedDisplayName("ToMonthLabel")]
        public int? EndMonth { get; set; }

        [LocalizedDisplayName("OrganizationLabel")]
        public long? OrganizationEnteredById { get; set; }

        public string OrganizationEnteredByName
        {
            get
            {
                return !OrganizationEnteredById.HasValue
                    ? string.Empty
                    : FilterHelper.GetVetOrganizationName(OrganizationEnteredById.Value, Localizer.CurrentCultureLanguageID);
            }
        }

        public List<SelectListItemSurrogate> SelectedCurrentMonthList
        {
            get { return FilterHelper.GetWebMonthList(DateTime.Now.Month, false); }
        }

        public List<SelectListItemSurrogate> OrganizationList
        {
            get { return FilterHelper.GetVetOrganizationList(VetReportType.Indicators, Localizer.CurrentCultureLanguageID); }
        }

        public static explicit operator VetIndicatorsSurrogateModel(VetIndicatorsModel model)
        {
            var result = new VetIndicatorsSurrogateModel(model.Language,
                model.OrganizationEnteredById, model.OrganizationEnteredByName,
                model.StartYear, model.EndYear,
                model.StartMonth, model.EndMonth,
                model.UseArchive)
            {
                ExportFormat = model.ExportFormat,
                IsOpenInNewWindow = model.IsOpenInNewWindow
            };

            return result;
        }
    }
}