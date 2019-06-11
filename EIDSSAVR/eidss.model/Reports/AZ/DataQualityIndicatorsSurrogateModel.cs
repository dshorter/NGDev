﻿using System;
using System.Collections.Generic;
using eidss.model.Enums;
using eidss.model.Reports.Common;

namespace eidss.model.Reports.AZ
{
    [Serializable]
    public class DataQualityIndicatorsSurrogateModel : BaseDateModel
    {
        public DataQualityIndicatorsSurrogateModel()
        {
            DiagnosisCheckedItems = new string[0];
        }

        public DataQualityIndicatorsSurrogateModel
            (string language, string[] checkedDiagnosis, string diagnosisName,
                int year, int? startMonth, int? endMonth,
                long? regionId, long? rayonId,
                string regionName, string rayonName,
                bool arrangeRayons,
                long? organizationId, List<PersonalDataGroup> forbiddenGroups,
                bool useArchive)
            : base(language, year, startMonth, endMonth, useArchive)
        {
            DiagnosisCheckedItems = checkedDiagnosis ?? new string[0];
            DiagnosisName = diagnosisName;

            RegionId = regionId;
            RayonId = rayonId;
            RegionName = regionName;
            RayonName = rayonName;

            ArrangeRayonsAlphabetically = arrangeRayons;
            OrganizationId = organizationId;
            ForbiddenGroups = forbiddenGroups;
        }

        public long? RegionId { get; set; }

        public long? RayonId { get; set; }

        public string RegionName { get; set; }

        public string RayonName { get; set; }

        public string[] DiagnosisCheckedItems { get; set; }
        public string DiagnosisName { get; set; }

        public bool ArrangeRayonsAlphabetically { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, RegionId='{1}', RayonId='{2}',  Diagnosis={3}",
                base.ToString(), RegionId, RayonId, DiagnosisName);
        }
    }
}