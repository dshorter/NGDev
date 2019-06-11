﻿using System.Data.SqlClient;
using System.Linq;

namespace EIDSS.Reports.Parameterized.Human.AJ.DataSets {
    
    
    public partial class SummaryDataSet {
        partial class spRepHumSummaryRayonDiseaseAgeDataTable
        {
        }
    
        partial class spRepHumSummaryRayonDiseaseAge1DataTable
        {
        }
    }
}

namespace EIDSS.Reports.Parameterized.Human.AJ.DataSets.SummaryDataSetTableAdapters
{


    public partial class spRepHumSummaryRayonDiseaseAgeTableAdapter 
    {
        internal int CommandTimeout
        {
            get { return CommandCollection.Select(c => c.CommandTimeout).FirstOrDefault(); }
            set
            {
                foreach (SqlCommand command in CommandCollection)
                {
                    command.CommandTimeout = value;
                }
            }
        }

        
    }

}
