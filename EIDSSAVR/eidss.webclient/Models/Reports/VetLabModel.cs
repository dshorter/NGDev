﻿using System;
using System.Collections.Generic;
using bv.common.Core;
using eidss.model.Core;
using eidss.model.Reports.AZ;
using eidss.model.Reports.Common;

namespace eidss.webclient.Models.Reports
{
    [Serializable]
    public class VetLabModel : AFPModel
    {
        public VetLabModel()
        {
            Address = new AddressModel();
        }

        public VetLabModel(string language, int year, int period, int periodType, long? regionId, long? rayonId, bool useArchive)
            : base(language, year, period, periodType, useArchive)
        {
            Address = new AddressModel(regionId, rayonId);
        }

        public AddressModel Address { get; set; }

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

        public List<SelectListItemSurrogate> OrganizationList
        {
            get { return FilterHelper.GetVetOrganizationList(VetReportType.Lab, Localizer.CurrentCultureLanguageID); }
        }

        public static explicit operator VetLabSurrogateModel(VetLabModel model)
        {
            AddressModel address = model.Address ?? new AddressModel();
            var result = new VetLabSurrogateModel(model.Language,
                model.Year, model.Period, model.PeriodType,
                model.OrganizationEnteredById, model.OrganizationEnteredByName,
                address.RegionId, address.RayonId,
                address.RegionName(model.Language), address.RayonName(model.Language),
                model.OrganizationId, model.ForbiddenGroups,
                model.UseArchive)
            {
                ExportFormat = model.ExportFormat,
                IsOpenInNewWindow = model.IsOpenInNewWindow
            };

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}, Diagnosis={1}", base.ToString(), Address);
        }
    }
}