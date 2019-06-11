﻿using System.Data.SqlClient;
using System.Linq;

namespace EIDSS.Reports.Parameterized.Human.AJ.DataSets
{
    public partial class LabTestingResultDataSet
    {
    }
}

namespace EIDSS.Reports.Parameterized.Human.AJ.DataSets.LabTestingResultDataSetTableAdapters

{
    public partial class LabTestingResultAdapter
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