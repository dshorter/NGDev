﻿using System;
using bv.common.Core;
using eidss.model.Reports.Common;

namespace eidss.model.Reports.GG
{
    [Serializable]
    public class AnnualInfectiousDiseaseModel : BaseYearModel
    {
        public AnnualInfectiousDiseaseModel()
        {
            RayonFilter = new RayonModel();
        }

        public AnnualInfectiousDiseaseModel(string language, int year, bool useArchive) : base(language, year, useArchive)
        {
            RayonFilter = new RayonModel();
        }

        public RayonModel RayonFilter { get; set; }
        public override int MinYear
        {
            get { return 2005; }
        }

        public override int MaxYear
        {
            get { return 2012; }
        }

        public static explicit operator IntermediateInfectiousDiseaseSurrogateModel(AnnualInfectiousDiseaseModel model)
        {
            Utils.CheckNotNull(model, "model");

            var startDate = new DateTime(model.Year, 1, 1);
            var endDate = startDate.AddYears(1).AddDays(-1);

            AddressModel address = model.RayonFilter == null
                           ? new AddressModel()
                           : new AddressModel(model.RayonFilter.RegionId, model.RayonFilter.RayonId);

            var result = new IntermediateInfectiousDiseaseSurrogateModel(model.Language, startDate, endDate,
                                                                         address.RegionId, address.RayonId, address.RegionName(model.Language), address.RayonName(model.Language),
                                                                         model.OrganizationId, model.ForbiddenGroups,
                                                                         model.UseArchive)
                {
                    ExportFormat = model.ExportFormat,
                    IsOpenInNewWindow = model.IsOpenInNewWindow,
                    UserId = model.UserId,
                    IsArchiveMode = model.IsArchiveMode,
                };

            return result;
        }
    }
}