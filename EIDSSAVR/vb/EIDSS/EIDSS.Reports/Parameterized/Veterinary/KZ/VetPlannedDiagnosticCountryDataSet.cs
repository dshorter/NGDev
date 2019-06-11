﻿using System.Data.SqlClient;
using System.Linq;
using EIDSS.Reports.BaseControls.BaseDataSetTableAdapters;

namespace EIDSS.Reports.Parameterized.Veterinary.KZ {
    
    
    public partial class VetPlannedDiagnosticCountryDataSet {
        

    }
}

namespace EIDSS.Reports.Parameterized.Veterinary.KZ.VetPlannedDiagnosticCountryDataSetTableAdapters
{


    public partial class spRepVetPlannedDiagnosticTests_CountryTableAdapter 
    {
        private SqlTransaction m_Transaction;

        internal SqlTransaction Transaction
        {
            get { return m_Transaction; }
            set
            {
                m_Transaction = value;
                BaseAdapter.SetTransaction(Adapter, CommandCollection, value);
            }
        }

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
