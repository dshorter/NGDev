using hmis2eidss.service.HMIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace hmis2eidss.service.CaseData
{
    class TestCases
    {
        //private CaseContract cc = new CaseContract();
        //private MessageContract mc = new MessageContract();
        //private DiagnosisContract dc = new DiagnosisContract();
        List<CaseContract> cases = new List<CaseContract>();
        CasesResultContract caseData = new CasesResultContract();
        public TestCases()
        {
        }

        private DataTable LoadData()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook workbook = xlApp.Workbooks.Open(@"C:\HMISTest\HMISTestData.xlsx");
            DataTable dt = new DataTable();

            try
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in workbook.Worksheets)
                {
                    int rows = sheet.UsedRange.Rows.Count;
                    int cols = sheet.UsedRange.Columns.Count;
                    int noofrow = 1;

                    //Header/column names
                    for (int c = 1; c <= cols; c++)
                    {
                        string colname = sheet.Cells[1, c].Text;
                        dt.Columns.Add(colname);
                        noofrow = 2;
                    }
                    //Rows
                    for (int r = noofrow; r <= rows; r++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int c = 1; c <= cols; c++)
                        {
                            dr[c - 1] = sheet.Cells[r, c].Text;
                        }
                        dt.Rows.Add(dr);
                    }
                }

                workbook.Close();
                xlApp.Quit();

                return dt;
            }
            catch(Exception ex)
            {
                workbook.Saved = true;
                workbook.Close();
                xlApp.Quit();
                throw;
            }
        }

        public CasesResultContract GetData()
        {
            DataTable dt = LoadData();
            
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CaseContract cc = new CaseContract();
                    MessageContract mc = new MessageContract();
                    DiagnosisContract dc = new DiagnosisContract();
                    cc.Messages = new List<MessageContract>();
                    //Add a person contract object to the message contract
                    mc.PatientPerson = new PersonContract();
                    mc.Diagnoses = new List<DiagnosisContract>();
                    mc.NotifierPerson = new PersonContract();
                    mc.CurrentProvider = new OrganizationContract();

                    //Case contract
                    if(dr["CaseId"].ToString() == "")
                    {
                        if (dr["CreateCaseID"].ToString() != "")
                        {
                            switch (dr["CreateCaseID"].ToString())
                            {
                                case "Yes":
                                case "yes":
                                case "Y":
                                case "y":
                                    cc.ID = Guid.NewGuid();
                                    break;
                                case "No":
                                case "no":
                                case "N":
                                case "n":
                                    cc.ID = Guid.Empty;
                                    break;
                                default:
                                    cc.ID = Guid.NewGuid();
                                    break;
                            }

                        }
                        else
                        {
                            cc.ID = Guid.NewGuid();
                        }
                    }
                    else
                    {
                        cc.ID = Guid.Parse(dr["CaseId"].ToString());
                    }
                    

                    //For the Message contract
                    mc.ID = Guid.NewGuid();

                    //For the person contract
                    mc.PatientPerson.PersonalID = dr["PersonalID"].ToString();
                    mc.PatientPerson.FirstName = dr["FirstName"].ToString();
                    mc.PatientPerson.LastName = dr["LastName"].ToString();
                    mc.PatientPerson.Gender = dr["Gender"].ToString();
                    mc.PatientPerson.Tel = dr["Tel"].ToString();
                    if(dr["Age"].ToString() != "")
                        mc.PatientPerson.Age = Convert.ToInt32(dr["Age"].ToString());
                    if(dr["RegionID"].ToString() != "")
                        mc.PatientPerson.FactualRegionID = Guid.Parse(dr["RegionID"].ToString());
                    mc.PatientPerson.RegionName = dr["RegionName"].ToString();
                    if (dr["RayonID"].ToString() != "")
                        mc.PatientPerson.FactualMunicipalityID = Guid.Parse(dr["RayonID"].ToString());
                    mc.PatientPerson.FactualMunicipalityName = dr["RayonName"].ToString();
                    if (dr["SettlementID"].ToString() != "")
                        mc.PatientPerson.FactualSettlementID = Guid.Parse(dr["SettlementID"].ToString());
                    mc.PatientPerson.FactualSettlementName = dr["SettlementName"].ToString();

                    //Determine patient location
                    if(dr["Location"].ToString() != "")
                    {
                        int loc = Convert.ToInt32(dr["Location"].ToString());
                        if (loc <= 1)
                        {
                            switch (loc)
                            {
                                case 0:
                                    mc.PatientStatus = PatientStatusEnums.NotInMedicalFacility;
                                    break;
                                case 1:
                                    mc.PatientStatus = PatientStatusEnums.Treating;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    //Diagnosis Contract
                    dc.Id = Guid.NewGuid();
                    dc.IClassCode = dr["ICD10Code"].ToString();
                    dc.IClassName = dr["ICD10Name"].ToString();

                    //Organization contract
                    if(dr["OrganizationID"].ToString() != "")
                        mc.CurrentProvider.Id = Guid.Parse(dr["OrganizationID"].ToString());

                    //Notification sent by

                    mc.NotifierPerson.LastName = dr["NotifierLastName"].ToString();
                    mc.NotifierPerson.FirstName = dr["NotifierFirstName"].ToString();

                    //DateTime conversions
                    if(dr["BirthDate"].ToString() != "")
                    {
                        DateTime dob = DateTime.Parse(dr["BirthDate"].ToString());
                        mc.PatientPerson.BirthDate = dob;
                    }

                    if (dr["DateofDischarge"].ToString() != "")
                    {
                        DateTime discharge = DateTime.Parse(dr["DateofDischarge"].ToString());
                        mc.DateClosed = discharge;
                    }

                    if (dr["NotificationDate"].ToString() != "")
                    {
                        DateTime notificationDate = DateTime.Parse(dr["NotificationDate"].ToString());
                        mc.DateCreated = notificationDate;
                    }                    

                    if (dr["DiagnosisDate"].ToString() != "")
                    {
                        DateTime diagnosisDate = DateTime.Parse(dr["DiagnosisDate"].ToString());
                        dc.DateAdded = diagnosisDate;
                    }
                    
                    mc.Diagnoses.Add(dc);
                    cc.Messages.Add(mc);
                    cases.Add(cc);
                }
                caseData.Result = cases;

                return caseData;
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                throw;
            }
        }
    }
}
