﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eidss.openapi.contract
{
    /// <summary>Short description of Vet Case</summary>
    public class VetCaseListItem 
    {
        /// <summary>
        /// A unique number of the record
        /// </summary>
        public long RecordID { get; set; }

        /// <summary>
        /// A number that is assigned to the individual case and automatically generated by EIDSS. The number appears when the case information is saved first time. 
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// Farm Name
        /// </summary>
        public string FarmName { get; set; }

        /// <summary>
        /// Date that the case was assigned to the investigator.
        /// </summary>
        public DateTime? AssignedDate { get; set; }

        /// <summary>
        /// This value is auto populated to reflect the date when the record was first created.
        /// </summary>
        public DateTime? EnteredDate { get; set; }

        /// <summary>
        /// Date the case was first reported by the Officer identified in the Reported By field.
        /// </summary>
        public DateTime? InitialReportDate { get; set; }

        /// <summary>
        /// Date that a case investigation began. 
        /// </summary>
        public DateTime? InvestigationDate { get; set; }

        /// <summary>
        /// Diagnosis<br/>
        /// Reference number: 19000019<br/>
        /// Reference type: Diagnosis
        /// </summary>
        public Diagnosis Diagnosis { get; set; }

        /// <summary>
        /// Final Diagnosis<br/>
        /// Reference number: 19000019<br/>
        /// Reference type: Diagnosis
        /// </summary>
        public Diagnosis FinalDiagnosis { get; set; }

        /// <summary>
        /// Tentative Diagnosis 1<br/>
        /// Reference number: 19000019<br/>
        /// Reference type: Diagnosis
        /// </summary>
        public Diagnosis TentativeDiagnosis1 { get; set; }

        /// <summary>
        /// Tentative Diagnosis 2<br/>
        /// Reference number: 19000019<br/>
        /// Reference type: Diagnosis
        /// </summary>
        public Diagnosis TentativeDiagnosis2 { get; set; }

        /// <summary>
        /// Tentative Diagnosis 3<br/>
        /// Reference number: 19000019<br/>
        /// Reference type: Diagnosis
        /// </summary>
        public Diagnosis TentativeDiagnosis3 { get; set; }

        /// <summary>
        /// User assigns classification based on case definition for disease. <br/>
        /// Reference number: 19000011<br/>
        /// Reference type: Case Classification
        /// </summary>
        public Reference CaseClassification { get; set; }

        /// <summary>
        /// In process and Closed are the user options to indicate that the investigation is continuing (In process) or has concluded (closed).  Once closed, other fields in the record become read only.<br/>
        /// Reference number: 19000111<br/>
        /// Reference type: Case Status
        /// </summary>
        public Reference CaseStatus { get; set; }

        /// <summary>
        /// System assigns report as either Livestock or Avian based on form used for data entry.<br/>
        /// Reference number: 19000012<br/>
        /// Reference type: Case Type
        /// </summary>
        public Reference CaseType { get; set; }

        /// <summary>
        /// Identifies the method by which the report became known.  Cases identified through a surveillance program will be labled as "Active" in AVR while regular case entries will be labled "Passive."<br/>
        /// Reference number: 19000144<br/>
        /// Reference type: Case Report Type
        /// </summary>
        public Reference ReportType { get; set; }

        /// <summary>
        /// Farm Country
        /// </summary>
        public GisReference Country { get; set; }

        /// <summary>
        /// Farm Region
        /// </summary>
        public GisReference Region { get; set; }

        /// <summary>
        /// Farm Rayon
        /// </summary>
        public GisReference Rayon { get; set; }

        /// <summary>
        /// Farm Settlement
        /// </summary>
        public GisReference Settlement { get; set; }
    }
}