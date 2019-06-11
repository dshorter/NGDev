﻿#pragma warning disable 0472,0414
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;
using BLToolkit.Aspects;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using bv.common.Configuration;
using bv.common.Enums;
using bv.common.Core;
using bv.model.BLToolkit;
using bv.model.Model;
using bv.model.Helpers;
using bv.model.Model.Extenders;
using bv.model.Model.Core;
using bv.model.Model.Handlers;
using bv.model.Model.Validators;
using eidss.model.Core;
using eidss.model.Enums;
		

namespace eidss.model.Schema
{
        
        
    [XmlType(AnonymousType = true)]
    public abstract partial class HumanCaseListWithDeletedItem : 
        EditableObject<HumanCaseListWithDeletedItem>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfCase), NonUpdatable, PrimaryKey]
        public abstract Int64 idfCase { get; set; }
                
        [LocalizedDisplayName(_str_idfsDiagnosis)]
        [MapField(_str_idfsDiagnosis)]
        public abstract Int64? idfsDiagnosis { get; set; }
        protected Int64? idfsDiagnosis_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsDiagnosis).OriginalValue; } }
        protected Int64? idfsDiagnosis_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsDiagnosis).PreviousValue; } }
                
        [LocalizedDisplayName(_str_DiagnosisName)]
        [MapField(_str_DiagnosisName)]
        public abstract String DiagnosisName { get; set; }
        protected String DiagnosisName_Original { get { return ((EditableValue<String>)((dynamic)this)._diagnosisName).OriginalValue; } }
        protected String DiagnosisName_Previous { get { return ((EditableValue<String>)((dynamic)this)._diagnosisName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsCaseStatus)]
        [MapField(_str_idfsCaseStatus)]
        public abstract Int64? idfsCaseStatus { get; set; }
        protected Int64? idfsCaseStatus_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsCaseStatus).OriginalValue; } }
        protected Int64? idfsCaseStatus_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsCaseStatus).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsCaseProgressStatus)]
        [MapField(_str_idfsCaseProgressStatus)]
        public abstract Int64? idfsCaseProgressStatus { get; set; }
        protected Int64? idfsCaseProgressStatus_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsCaseProgressStatus).OriginalValue; } }
        protected Int64? idfsCaseProgressStatus_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsCaseProgressStatus).PreviousValue; } }
                
        [LocalizedDisplayName(_str_CaseStatusName)]
        [MapField(_str_CaseStatusName)]
        public abstract String CaseStatusName { get; set; }
        protected String CaseStatusName_Original { get { return ((EditableValue<String>)((dynamic)this)._caseStatusName).OriginalValue; } }
        protected String CaseStatusName_Previous { get { return ((EditableValue<String>)((dynamic)this)._caseStatusName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_CaseProgressStatusName)]
        [MapField(_str_CaseProgressStatusName)]
        public abstract String CaseProgressStatusName { get; set; }
        protected String CaseProgressStatusName_Original { get { return ((EditableValue<String>)((dynamic)this)._caseProgressStatusName).OriginalValue; } }
        protected String CaseProgressStatusName_Previous { get { return ((EditableValue<String>)((dynamic)this)._caseProgressStatusName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datEnteredDate)]
        [MapField(_str_datEnteredDate)]
        public abstract DateTime? datEnteredDate { get; set; }
        protected DateTime? datEnteredDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datEnteredDate).OriginalValue; } }
        protected DateTime? datEnteredDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datEnteredDate).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strCaseID)]
        [MapField(_str_strCaseID)]
        public abstract String strCaseID { get; set; }
        protected String strCaseID_Original { get { return ((EditableValue<String>)((dynamic)this)._strCaseID).OriginalValue; } }
        protected String strCaseID_Previous { get { return ((EditableValue<String>)((dynamic)this)._strCaseID).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsSite)]
        [MapField(_str_idfsSite)]
        public abstract Int64 idfsSite { get; set; }
        protected Int64 idfsSite_Original { get { return ((EditableValue<Int64>)((dynamic)this)._idfsSite).OriginalValue; } }
        protected Int64 idfsSite_Previous { get { return ((EditableValue<Int64>)((dynamic)this)._idfsSite).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datCompletionPaperFormDate)]
        [MapField(_str_datCompletionPaperFormDate)]
        public abstract DateTime? datCompletionPaperFormDate { get; set; }
        protected DateTime? datCompletionPaperFormDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datCompletionPaperFormDate).OriginalValue; } }
        protected DateTime? datCompletionPaperFormDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datCompletionPaperFormDate).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strLocalIdentifier)]
        [MapField(_str_strLocalIdentifier)]
        public abstract String strLocalIdentifier { get; set; }
        protected String strLocalIdentifier_Original { get { return ((EditableValue<String>)((dynamic)this)._strLocalIdentifier).OriginalValue; } }
        protected String strLocalIdentifier_Previous { get { return ((EditableValue<String>)((dynamic)this)._strLocalIdentifier).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfPersonEnteredBy)]
        [MapField(_str_idfPersonEnteredBy)]
        public abstract Int64? idfPersonEnteredBy { get; set; }
        protected Int64? idfPersonEnteredBy_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfPersonEnteredBy).OriginalValue; } }
        protected Int64? idfPersonEnteredBy_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfPersonEnteredBy).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfHuman)]
        [MapField(_str_idfHuman)]
        public abstract Int64 idfHuman { get; set; }
        protected Int64 idfHuman_Original { get { return ((EditableValue<Int64>)((dynamic)this)._idfHuman).OriginalValue; } }
        protected Int64 idfHuman_Previous { get { return ((EditableValue<Int64>)((dynamic)this)._idfHuman).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strLastName)]
        [MapField(_str_strLastName)]
        public abstract String strLastName { get; set; }
        protected String strLastName_Original { get { return ((EditableValue<String>)((dynamic)this)._strLastName).OriginalValue; } }
        protected String strLastName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strLastName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strFirstName)]
        [MapField(_str_strFirstName)]
        public abstract String strFirstName { get; set; }
        protected String strFirstName_Original { get { return ((EditableValue<String>)((dynamic)this)._strFirstName).OriginalValue; } }
        protected String strFirstName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strFirstName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strSecondName)]
        [MapField(_str_strSecondName)]
        public abstract String strSecondName { get; set; }
        protected String strSecondName_Original { get { return ((EditableValue<String>)((dynamic)this)._strSecondName).OriginalValue; } }
        protected String strSecondName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strSecondName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_PatientName)]
        [MapField(_str_PatientName)]
        public abstract String PatientName { get; set; }
        protected String PatientName_Original { get { return ((EditableValue<String>)((dynamic)this)._patientName).OriginalValue; } }
        protected String PatientName_Previous { get { return ((EditableValue<String>)((dynamic)this)._patientName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datDateofBirth)]
        [MapField(_str_datDateofBirth)]
        public abstract DateTime? datDateofBirth { get; set; }
        protected DateTime? datDateofBirth_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datDateofBirth).OriginalValue; } }
        protected DateTime? datDateofBirth_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datDateofBirth).PreviousValue; } }
                
        [LocalizedDisplayName(_str_intPatientAge)]
        [MapField(_str_intPatientAge)]
        public abstract Int32? intPatientAge { get; set; }
        protected Int32? intPatientAge_Original { get { return ((EditableValue<Int32?>)((dynamic)this)._intPatientAge).OriginalValue; } }
        protected Int32? intPatientAge_Previous { get { return ((EditableValue<Int32?>)((dynamic)this)._intPatientAge).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsHumanAgeType)]
        [MapField(_str_idfsHumanAgeType)]
        public abstract Int64? idfsHumanAgeType { get; set; }
        protected Int64? idfsHumanAgeType_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsHumanAgeType).OriginalValue; } }
        protected Int64? idfsHumanAgeType_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsHumanAgeType).PreviousValue; } }
                
        [LocalizedDisplayName(_str_Age)]
        [MapField(_str_Age)]
        public abstract String Age { get; set; }
        protected String Age_Original { get { return ((EditableValue<String>)((dynamic)this)._age).OriginalValue; } }
        protected String Age_Previous { get { return ((EditableValue<String>)((dynamic)this)._age).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfAddress)]
        [MapField(_str_idfAddress)]
        public abstract Int64? idfAddress { get; set; }
        protected Int64? idfAddress_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfAddress).OriginalValue; } }
        protected Int64? idfAddress_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfAddress).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfGeoLocation)]
        [MapField(_str_idfGeoLocation)]
        public abstract Int64? idfGeoLocation { get; set; }
        protected Int64? idfGeoLocation_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfGeoLocation).OriginalValue; } }
        protected Int64? idfGeoLocation_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfGeoLocation).PreviousValue; } }
                
        [LocalizedDisplayName(_str_GeoLocationName)]
        [MapField(_str_GeoLocationName)]
        public abstract String GeoLocationName { get; set; }
        protected String GeoLocationName_Original { get { return ((EditableValue<String>)((dynamic)this)._geoLocationName).OriginalValue; } }
        protected String GeoLocationName_Previous { get { return ((EditableValue<String>)((dynamic)this)._geoLocationName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfEpiObservation)]
        [MapField(_str_idfEpiObservation)]
        public abstract Int64? idfEpiObservation { get; set; }
        protected Int64? idfEpiObservation_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfEpiObservation).OriginalValue; } }
        protected Int64? idfEpiObservation_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfEpiObservation).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfCSObservation)]
        [MapField(_str_idfCSObservation)]
        public abstract Int64? idfCSObservation { get; set; }
        protected Int64? idfCSObservation_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfCSObservation).OriginalValue; } }
        protected Int64? idfCSObservation_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfCSObservation).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsTentativeDiagnosis)]
        [MapField(_str_idfsTentativeDiagnosis)]
        public abstract Int64? idfsTentativeDiagnosis { get; set; }
        protected Int64? idfsTentativeDiagnosis_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsTentativeDiagnosis).OriginalValue; } }
        protected Int64? idfsTentativeDiagnosis_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsTentativeDiagnosis).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsInitialCaseStatus)]
        [MapField(_str_idfsInitialCaseStatus)]
        public abstract Int64? idfsInitialCaseStatus { get; set; }
        protected Int64? idfsInitialCaseStatus_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsInitialCaseStatus).OriginalValue; } }
        protected Int64? idfsInitialCaseStatus_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsInitialCaseStatus).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsSettlement)]
        [MapField(_str_idfsSettlement)]
        public abstract Int64? idfsSettlement { get; set; }
        protected Int64? idfsSettlement_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsSettlement).OriginalValue; } }
        protected Int64? idfsSettlement_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsSettlement).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsRegion)]
        [MapField(_str_idfsRegion)]
        public abstract Int64? idfsRegion { get; set; }
        protected Int64? idfsRegion_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsRegion).OriginalValue; } }
        protected Int64? idfsRegion_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsRegion).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsRayon)]
        [MapField(_str_idfsRayon)]
        public abstract Int64? idfsRayon { get; set; }
        protected Int64? idfsRayon_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsRayon).OriginalValue; } }
        protected Int64? idfsRayon_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsRayon).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsCountry)]
        [MapField(_str_idfsCountry)]
        public abstract Int64? idfsCountry { get; set; }
        protected Int64? idfsCountry_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsCountry).OriginalValue; } }
        protected Int64? idfsCountry_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsCountry).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsLocationOfExposureRayon)]
        [MapField(_str_idfsLocationOfExposureRayon)]
        public abstract Int64? idfsLocationOfExposureRayon { get; set; }
        protected Int64? idfsLocationOfExposureRayon_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsLocationOfExposureRayon).OriginalValue; } }
        protected Int64? idfsLocationOfExposureRayon_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsLocationOfExposureRayon).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsLocationOfExposureRegion)]
        [MapField(_str_idfsLocationOfExposureRegion)]
        public abstract Int64? idfsLocationOfExposureRegion { get; set; }
        protected Int64? idfsLocationOfExposureRegion_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsLocationOfExposureRegion).OriginalValue; } }
        protected Int64? idfsLocationOfExposureRegion_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsLocationOfExposureRegion).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datFinalCaseClassificationDate)]
        [MapField(_str_datFinalCaseClassificationDate)]
        public abstract DateTime? datFinalCaseClassificationDate { get; set; }
        protected DateTime? datFinalCaseClassificationDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datFinalCaseClassificationDate).OriginalValue; } }
        protected DateTime? datFinalCaseClassificationDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datFinalCaseClassificationDate).PreviousValue; } }
                
        [LocalizedDisplayName(_str_intRowStatus)]
        [MapField(_str_intRowStatus)]
        public abstract Int32 intRowStatus { get; set; }
        protected Int32 intRowStatus_Original { get { return ((EditableValue<Int32>)((dynamic)this)._intRowStatus).OriginalValue; } }
        protected Int32 intRowStatus_Previous { get { return ((EditableValue<Int32>)((dynamic)this)._intRowStatus).PreviousValue; } }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<HumanCaseListWithDeletedItem, object> _get_func;
            internal Action<HumanCaseListWithDeletedItem, string> _set_func;
            internal Action<HumanCaseListWithDeletedItem, HumanCaseListWithDeletedItem, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfCase = "idfCase";
        internal const string _str_idfsDiagnosis = "idfsDiagnosis";
        internal const string _str_DiagnosisName = "DiagnosisName";
        internal const string _str_idfsCaseStatus = "idfsCaseStatus";
        internal const string _str_idfsCaseProgressStatus = "idfsCaseProgressStatus";
        internal const string _str_CaseStatusName = "CaseStatusName";
        internal const string _str_CaseProgressStatusName = "CaseProgressStatusName";
        internal const string _str_datEnteredDate = "datEnteredDate";
        internal const string _str_strCaseID = "strCaseID";
        internal const string _str_idfsSite = "idfsSite";
        internal const string _str_datCompletionPaperFormDate = "datCompletionPaperFormDate";
        internal const string _str_strLocalIdentifier = "strLocalIdentifier";
        internal const string _str_idfPersonEnteredBy = "idfPersonEnteredBy";
        internal const string _str_idfHuman = "idfHuman";
        internal const string _str_strLastName = "strLastName";
        internal const string _str_strFirstName = "strFirstName";
        internal const string _str_strSecondName = "strSecondName";
        internal const string _str_PatientName = "PatientName";
        internal const string _str_datDateofBirth = "datDateofBirth";
        internal const string _str_intPatientAge = "intPatientAge";
        internal const string _str_idfsHumanAgeType = "idfsHumanAgeType";
        internal const string _str_Age = "Age";
        internal const string _str_idfAddress = "idfAddress";
        internal const string _str_idfGeoLocation = "idfGeoLocation";
        internal const string _str_GeoLocationName = "GeoLocationName";
        internal const string _str_idfEpiObservation = "idfEpiObservation";
        internal const string _str_idfCSObservation = "idfCSObservation";
        internal const string _str_idfsTentativeDiagnosis = "idfsTentativeDiagnosis";
        internal const string _str_idfsInitialCaseStatus = "idfsInitialCaseStatus";
        internal const string _str_idfsSettlement = "idfsSettlement";
        internal const string _str_idfsRegion = "idfsRegion";
        internal const string _str_idfsRayon = "idfsRayon";
        internal const string _str_idfsCountry = "idfsCountry";
        internal const string _str_idfsLocationOfExposureRayon = "idfsLocationOfExposureRayon";
        internal const string _str_idfsLocationOfExposureRegion = "idfsLocationOfExposureRegion";
        internal const string _str_datFinalCaseClassificationDate = "datFinalCaseClassificationDate";
        internal const string _str_intRowStatus = "intRowStatus";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfCase, _formname = _str_idfCase, _type = "Int64",
              _get_func = o => o.idfCase,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfCase != newval) o.idfCase = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfCase != c.idfCase || o.IsRIRPropChanged(_str_idfCase, c)) 
                  m.Add(_str_idfCase, o.ObjectIdent + _str_idfCase, o.ObjectIdent2 + _str_idfCase, o.ObjectIdent3 + _str_idfCase, "Int64", 
                    o.idfCase == null ? "" : o.idfCase.ToString(),                  
                  o.IsReadOnly(_str_idfCase), o.IsInvisible(_str_idfCase), o.IsRequired(_str_idfCase)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsDiagnosis, _formname = _str_idfsDiagnosis, _type = "Int64?",
              _get_func = o => o.idfsDiagnosis,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsDiagnosis != newval) o.idfsDiagnosis = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsDiagnosis != c.idfsDiagnosis || o.IsRIRPropChanged(_str_idfsDiagnosis, c)) 
                  m.Add(_str_idfsDiagnosis, o.ObjectIdent + _str_idfsDiagnosis, o.ObjectIdent2 + _str_idfsDiagnosis, o.ObjectIdent3 + _str_idfsDiagnosis, "Int64?", 
                    o.idfsDiagnosis == null ? "" : o.idfsDiagnosis.ToString(),                  
                  o.IsReadOnly(_str_idfsDiagnosis), o.IsInvisible(_str_idfsDiagnosis), o.IsRequired(_str_idfsDiagnosis)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_DiagnosisName, _formname = _str_DiagnosisName, _type = "String",
              _get_func = o => o.DiagnosisName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.DiagnosisName != newval) o.DiagnosisName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.DiagnosisName != c.DiagnosisName || o.IsRIRPropChanged(_str_DiagnosisName, c)) 
                  m.Add(_str_DiagnosisName, o.ObjectIdent + _str_DiagnosisName, o.ObjectIdent2 + _str_DiagnosisName, o.ObjectIdent3 + _str_DiagnosisName, "String", 
                    o.DiagnosisName == null ? "" : o.DiagnosisName.ToString(),                  
                  o.IsReadOnly(_str_DiagnosisName), o.IsInvisible(_str_DiagnosisName), o.IsRequired(_str_DiagnosisName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsCaseStatus, _formname = _str_idfsCaseStatus, _type = "Int64?",
              _get_func = o => o.idfsCaseStatus,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsCaseStatus != newval) o.idfsCaseStatus = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsCaseStatus != c.idfsCaseStatus || o.IsRIRPropChanged(_str_idfsCaseStatus, c)) 
                  m.Add(_str_idfsCaseStatus, o.ObjectIdent + _str_idfsCaseStatus, o.ObjectIdent2 + _str_idfsCaseStatus, o.ObjectIdent3 + _str_idfsCaseStatus, "Int64?", 
                    o.idfsCaseStatus == null ? "" : o.idfsCaseStatus.ToString(),                  
                  o.IsReadOnly(_str_idfsCaseStatus), o.IsInvisible(_str_idfsCaseStatus), o.IsRequired(_str_idfsCaseStatus)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsCaseProgressStatus, _formname = _str_idfsCaseProgressStatus, _type = "Int64?",
              _get_func = o => o.idfsCaseProgressStatus,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsCaseProgressStatus != newval) o.idfsCaseProgressStatus = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsCaseProgressStatus != c.idfsCaseProgressStatus || o.IsRIRPropChanged(_str_idfsCaseProgressStatus, c)) 
                  m.Add(_str_idfsCaseProgressStatus, o.ObjectIdent + _str_idfsCaseProgressStatus, o.ObjectIdent2 + _str_idfsCaseProgressStatus, o.ObjectIdent3 + _str_idfsCaseProgressStatus, "Int64?", 
                    o.idfsCaseProgressStatus == null ? "" : o.idfsCaseProgressStatus.ToString(),                  
                  o.IsReadOnly(_str_idfsCaseProgressStatus), o.IsInvisible(_str_idfsCaseProgressStatus), o.IsRequired(_str_idfsCaseProgressStatus)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_CaseStatusName, _formname = _str_CaseStatusName, _type = "String",
              _get_func = o => o.CaseStatusName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.CaseStatusName != newval) o.CaseStatusName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.CaseStatusName != c.CaseStatusName || o.IsRIRPropChanged(_str_CaseStatusName, c)) 
                  m.Add(_str_CaseStatusName, o.ObjectIdent + _str_CaseStatusName, o.ObjectIdent2 + _str_CaseStatusName, o.ObjectIdent3 + _str_CaseStatusName, "String", 
                    o.CaseStatusName == null ? "" : o.CaseStatusName.ToString(),                  
                  o.IsReadOnly(_str_CaseStatusName), o.IsInvisible(_str_CaseStatusName), o.IsRequired(_str_CaseStatusName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_CaseProgressStatusName, _formname = _str_CaseProgressStatusName, _type = "String",
              _get_func = o => o.CaseProgressStatusName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.CaseProgressStatusName != newval) o.CaseProgressStatusName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.CaseProgressStatusName != c.CaseProgressStatusName || o.IsRIRPropChanged(_str_CaseProgressStatusName, c)) 
                  m.Add(_str_CaseProgressStatusName, o.ObjectIdent + _str_CaseProgressStatusName, o.ObjectIdent2 + _str_CaseProgressStatusName, o.ObjectIdent3 + _str_CaseProgressStatusName, "String", 
                    o.CaseProgressStatusName == null ? "" : o.CaseProgressStatusName.ToString(),                  
                  o.IsReadOnly(_str_CaseProgressStatusName), o.IsInvisible(_str_CaseProgressStatusName), o.IsRequired(_str_CaseProgressStatusName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_datEnteredDate, _formname = _str_datEnteredDate, _type = "DateTime?",
              _get_func = o => o.datEnteredDate,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datEnteredDate != newval) o.datEnteredDate = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datEnteredDate != c.datEnteredDate || o.IsRIRPropChanged(_str_datEnteredDate, c)) 
                  m.Add(_str_datEnteredDate, o.ObjectIdent + _str_datEnteredDate, o.ObjectIdent2 + _str_datEnteredDate, o.ObjectIdent3 + _str_datEnteredDate, "DateTime?", 
                    o.datEnteredDate == null ? "" : o.datEnteredDate.ToString(),                  
                  o.IsReadOnly(_str_datEnteredDate), o.IsInvisible(_str_datEnteredDate), o.IsRequired(_str_datEnteredDate)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strCaseID, _formname = _str_strCaseID, _type = "String",
              _get_func = o => o.strCaseID,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strCaseID != newval) o.strCaseID = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strCaseID != c.strCaseID || o.IsRIRPropChanged(_str_strCaseID, c)) 
                  m.Add(_str_strCaseID, o.ObjectIdent + _str_strCaseID, o.ObjectIdent2 + _str_strCaseID, o.ObjectIdent3 + _str_strCaseID, "String", 
                    o.strCaseID == null ? "" : o.strCaseID.ToString(),                  
                  o.IsReadOnly(_str_strCaseID), o.IsInvisible(_str_strCaseID), o.IsRequired(_str_strCaseID)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsSite, _formname = _str_idfsSite, _type = "Int64",
              _get_func = o => o.idfsSite,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfsSite != newval) o.idfsSite = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsSite != c.idfsSite || o.IsRIRPropChanged(_str_idfsSite, c)) 
                  m.Add(_str_idfsSite, o.ObjectIdent + _str_idfsSite, o.ObjectIdent2 + _str_idfsSite, o.ObjectIdent3 + _str_idfsSite, "Int64", 
                    o.idfsSite == null ? "" : o.idfsSite.ToString(),                  
                  o.IsReadOnly(_str_idfsSite), o.IsInvisible(_str_idfsSite), o.IsRequired(_str_idfsSite)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_datCompletionPaperFormDate, _formname = _str_datCompletionPaperFormDate, _type = "DateTime?",
              _get_func = o => o.datCompletionPaperFormDate,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datCompletionPaperFormDate != newval) o.datCompletionPaperFormDate = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datCompletionPaperFormDate != c.datCompletionPaperFormDate || o.IsRIRPropChanged(_str_datCompletionPaperFormDate, c)) 
                  m.Add(_str_datCompletionPaperFormDate, o.ObjectIdent + _str_datCompletionPaperFormDate, o.ObjectIdent2 + _str_datCompletionPaperFormDate, o.ObjectIdent3 + _str_datCompletionPaperFormDate, "DateTime?", 
                    o.datCompletionPaperFormDate == null ? "" : o.datCompletionPaperFormDate.ToString(),                  
                  o.IsReadOnly(_str_datCompletionPaperFormDate), o.IsInvisible(_str_datCompletionPaperFormDate), o.IsRequired(_str_datCompletionPaperFormDate)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strLocalIdentifier, _formname = _str_strLocalIdentifier, _type = "String",
              _get_func = o => o.strLocalIdentifier,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strLocalIdentifier != newval) o.strLocalIdentifier = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strLocalIdentifier != c.strLocalIdentifier || o.IsRIRPropChanged(_str_strLocalIdentifier, c)) 
                  m.Add(_str_strLocalIdentifier, o.ObjectIdent + _str_strLocalIdentifier, o.ObjectIdent2 + _str_strLocalIdentifier, o.ObjectIdent3 + _str_strLocalIdentifier, "String", 
                    o.strLocalIdentifier == null ? "" : o.strLocalIdentifier.ToString(),                  
                  o.IsReadOnly(_str_strLocalIdentifier), o.IsInvisible(_str_strLocalIdentifier), o.IsRequired(_str_strLocalIdentifier)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfPersonEnteredBy, _formname = _str_idfPersonEnteredBy, _type = "Int64?",
              _get_func = o => o.idfPersonEnteredBy,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfPersonEnteredBy != newval) o.idfPersonEnteredBy = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfPersonEnteredBy != c.idfPersonEnteredBy || o.IsRIRPropChanged(_str_idfPersonEnteredBy, c)) 
                  m.Add(_str_idfPersonEnteredBy, o.ObjectIdent + _str_idfPersonEnteredBy, o.ObjectIdent2 + _str_idfPersonEnteredBy, o.ObjectIdent3 + _str_idfPersonEnteredBy, "Int64?", 
                    o.idfPersonEnteredBy == null ? "" : o.idfPersonEnteredBy.ToString(),                  
                  o.IsReadOnly(_str_idfPersonEnteredBy), o.IsInvisible(_str_idfPersonEnteredBy), o.IsRequired(_str_idfPersonEnteredBy)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfHuman, _formname = _str_idfHuman, _type = "Int64",
              _get_func = o => o.idfHuman,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfHuman != newval) o.idfHuman = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfHuman != c.idfHuman || o.IsRIRPropChanged(_str_idfHuman, c)) 
                  m.Add(_str_idfHuman, o.ObjectIdent + _str_idfHuman, o.ObjectIdent2 + _str_idfHuman, o.ObjectIdent3 + _str_idfHuman, "Int64", 
                    o.idfHuman == null ? "" : o.idfHuman.ToString(),                  
                  o.IsReadOnly(_str_idfHuman), o.IsInvisible(_str_idfHuman), o.IsRequired(_str_idfHuman)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strLastName, _formname = _str_strLastName, _type = "String",
              _get_func = o => o.strLastName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strLastName != newval) o.strLastName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strLastName != c.strLastName || o.IsRIRPropChanged(_str_strLastName, c)) 
                  m.Add(_str_strLastName, o.ObjectIdent + _str_strLastName, o.ObjectIdent2 + _str_strLastName, o.ObjectIdent3 + _str_strLastName, "String", 
                    o.strLastName == null ? "" : o.strLastName.ToString(),                  
                  o.IsReadOnly(_str_strLastName), o.IsInvisible(_str_strLastName), o.IsRequired(_str_strLastName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strFirstName, _formname = _str_strFirstName, _type = "String",
              _get_func = o => o.strFirstName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strFirstName != newval) o.strFirstName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strFirstName != c.strFirstName || o.IsRIRPropChanged(_str_strFirstName, c)) 
                  m.Add(_str_strFirstName, o.ObjectIdent + _str_strFirstName, o.ObjectIdent2 + _str_strFirstName, o.ObjectIdent3 + _str_strFirstName, "String", 
                    o.strFirstName == null ? "" : o.strFirstName.ToString(),                  
                  o.IsReadOnly(_str_strFirstName), o.IsInvisible(_str_strFirstName), o.IsRequired(_str_strFirstName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strSecondName, _formname = _str_strSecondName, _type = "String",
              _get_func = o => o.strSecondName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strSecondName != newval) o.strSecondName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strSecondName != c.strSecondName || o.IsRIRPropChanged(_str_strSecondName, c)) 
                  m.Add(_str_strSecondName, o.ObjectIdent + _str_strSecondName, o.ObjectIdent2 + _str_strSecondName, o.ObjectIdent3 + _str_strSecondName, "String", 
                    o.strSecondName == null ? "" : o.strSecondName.ToString(),                  
                  o.IsReadOnly(_str_strSecondName), o.IsInvisible(_str_strSecondName), o.IsRequired(_str_strSecondName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_PatientName, _formname = _str_PatientName, _type = "String",
              _get_func = o => o.PatientName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.PatientName != newval) o.PatientName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.PatientName != c.PatientName || o.IsRIRPropChanged(_str_PatientName, c)) 
                  m.Add(_str_PatientName, o.ObjectIdent + _str_PatientName, o.ObjectIdent2 + _str_PatientName, o.ObjectIdent3 + _str_PatientName, "String", 
                    o.PatientName == null ? "" : o.PatientName.ToString(),                  
                  o.IsReadOnly(_str_PatientName), o.IsInvisible(_str_PatientName), o.IsRequired(_str_PatientName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_datDateofBirth, _formname = _str_datDateofBirth, _type = "DateTime?",
              _get_func = o => o.datDateofBirth,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datDateofBirth != newval) o.datDateofBirth = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datDateofBirth != c.datDateofBirth || o.IsRIRPropChanged(_str_datDateofBirth, c)) 
                  m.Add(_str_datDateofBirth, o.ObjectIdent + _str_datDateofBirth, o.ObjectIdent2 + _str_datDateofBirth, o.ObjectIdent3 + _str_datDateofBirth, "DateTime?", 
                    o.datDateofBirth == null ? "" : o.datDateofBirth.ToString(),                  
                  o.IsReadOnly(_str_datDateofBirth), o.IsInvisible(_str_datDateofBirth), o.IsRequired(_str_datDateofBirth)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_intPatientAge, _formname = _str_intPatientAge, _type = "Int32?",
              _get_func = o => o.intPatientAge,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt32Nullable(val); if (o.intPatientAge != newval) o.intPatientAge = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.intPatientAge != c.intPatientAge || o.IsRIRPropChanged(_str_intPatientAge, c)) 
                  m.Add(_str_intPatientAge, o.ObjectIdent + _str_intPatientAge, o.ObjectIdent2 + _str_intPatientAge, o.ObjectIdent3 + _str_intPatientAge, "Int32?", 
                    o.intPatientAge == null ? "" : o.intPatientAge.ToString(),                  
                  o.IsReadOnly(_str_intPatientAge), o.IsInvisible(_str_intPatientAge), o.IsRequired(_str_intPatientAge)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsHumanAgeType, _formname = _str_idfsHumanAgeType, _type = "Int64?",
              _get_func = o => o.idfsHumanAgeType,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsHumanAgeType != newval) o.idfsHumanAgeType = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsHumanAgeType != c.idfsHumanAgeType || o.IsRIRPropChanged(_str_idfsHumanAgeType, c)) 
                  m.Add(_str_idfsHumanAgeType, o.ObjectIdent + _str_idfsHumanAgeType, o.ObjectIdent2 + _str_idfsHumanAgeType, o.ObjectIdent3 + _str_idfsHumanAgeType, "Int64?", 
                    o.idfsHumanAgeType == null ? "" : o.idfsHumanAgeType.ToString(),                  
                  o.IsReadOnly(_str_idfsHumanAgeType), o.IsInvisible(_str_idfsHumanAgeType), o.IsRequired(_str_idfsHumanAgeType)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_Age, _formname = _str_Age, _type = "String",
              _get_func = o => o.Age,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.Age != newval) o.Age = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.Age != c.Age || o.IsRIRPropChanged(_str_Age, c)) 
                  m.Add(_str_Age, o.ObjectIdent + _str_Age, o.ObjectIdent2 + _str_Age, o.ObjectIdent3 + _str_Age, "String", 
                    o.Age == null ? "" : o.Age.ToString(),                  
                  o.IsReadOnly(_str_Age), o.IsInvisible(_str_Age), o.IsRequired(_str_Age)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfAddress, _formname = _str_idfAddress, _type = "Int64?",
              _get_func = o => o.idfAddress,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfAddress != newval) o.idfAddress = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfAddress != c.idfAddress || o.IsRIRPropChanged(_str_idfAddress, c)) 
                  m.Add(_str_idfAddress, o.ObjectIdent + _str_idfAddress, o.ObjectIdent2 + _str_idfAddress, o.ObjectIdent3 + _str_idfAddress, "Int64?", 
                    o.idfAddress == null ? "" : o.idfAddress.ToString(),                  
                  o.IsReadOnly(_str_idfAddress), o.IsInvisible(_str_idfAddress), o.IsRequired(_str_idfAddress)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfGeoLocation, _formname = _str_idfGeoLocation, _type = "Int64?",
              _get_func = o => o.idfGeoLocation,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfGeoLocation != newval) o.idfGeoLocation = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfGeoLocation != c.idfGeoLocation || o.IsRIRPropChanged(_str_idfGeoLocation, c)) 
                  m.Add(_str_idfGeoLocation, o.ObjectIdent + _str_idfGeoLocation, o.ObjectIdent2 + _str_idfGeoLocation, o.ObjectIdent3 + _str_idfGeoLocation, "Int64?", 
                    o.idfGeoLocation == null ? "" : o.idfGeoLocation.ToString(),                  
                  o.IsReadOnly(_str_idfGeoLocation), o.IsInvisible(_str_idfGeoLocation), o.IsRequired(_str_idfGeoLocation)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_GeoLocationName, _formname = _str_GeoLocationName, _type = "String",
              _get_func = o => o.GeoLocationName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.GeoLocationName != newval) o.GeoLocationName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.GeoLocationName != c.GeoLocationName || o.IsRIRPropChanged(_str_GeoLocationName, c)) 
                  m.Add(_str_GeoLocationName, o.ObjectIdent + _str_GeoLocationName, o.ObjectIdent2 + _str_GeoLocationName, o.ObjectIdent3 + _str_GeoLocationName, "String", 
                    o.GeoLocationName == null ? "" : o.GeoLocationName.ToString(),                  
                  o.IsReadOnly(_str_GeoLocationName), o.IsInvisible(_str_GeoLocationName), o.IsRequired(_str_GeoLocationName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfEpiObservation, _formname = _str_idfEpiObservation, _type = "Int64?",
              _get_func = o => o.idfEpiObservation,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfEpiObservation != newval) o.idfEpiObservation = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfEpiObservation != c.idfEpiObservation || o.IsRIRPropChanged(_str_idfEpiObservation, c)) 
                  m.Add(_str_idfEpiObservation, o.ObjectIdent + _str_idfEpiObservation, o.ObjectIdent2 + _str_idfEpiObservation, o.ObjectIdent3 + _str_idfEpiObservation, "Int64?", 
                    o.idfEpiObservation == null ? "" : o.idfEpiObservation.ToString(),                  
                  o.IsReadOnly(_str_idfEpiObservation), o.IsInvisible(_str_idfEpiObservation), o.IsRequired(_str_idfEpiObservation)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfCSObservation, _formname = _str_idfCSObservation, _type = "Int64?",
              _get_func = o => o.idfCSObservation,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfCSObservation != newval) o.idfCSObservation = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfCSObservation != c.idfCSObservation || o.IsRIRPropChanged(_str_idfCSObservation, c)) 
                  m.Add(_str_idfCSObservation, o.ObjectIdent + _str_idfCSObservation, o.ObjectIdent2 + _str_idfCSObservation, o.ObjectIdent3 + _str_idfCSObservation, "Int64?", 
                    o.idfCSObservation == null ? "" : o.idfCSObservation.ToString(),                  
                  o.IsReadOnly(_str_idfCSObservation), o.IsInvisible(_str_idfCSObservation), o.IsRequired(_str_idfCSObservation)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsTentativeDiagnosis, _formname = _str_idfsTentativeDiagnosis, _type = "Int64?",
              _get_func = o => o.idfsTentativeDiagnosis,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsTentativeDiagnosis != newval) o.idfsTentativeDiagnosis = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsTentativeDiagnosis != c.idfsTentativeDiagnosis || o.IsRIRPropChanged(_str_idfsTentativeDiagnosis, c)) 
                  m.Add(_str_idfsTentativeDiagnosis, o.ObjectIdent + _str_idfsTentativeDiagnosis, o.ObjectIdent2 + _str_idfsTentativeDiagnosis, o.ObjectIdent3 + _str_idfsTentativeDiagnosis, "Int64?", 
                    o.idfsTentativeDiagnosis == null ? "" : o.idfsTentativeDiagnosis.ToString(),                  
                  o.IsReadOnly(_str_idfsTentativeDiagnosis), o.IsInvisible(_str_idfsTentativeDiagnosis), o.IsRequired(_str_idfsTentativeDiagnosis)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsInitialCaseStatus, _formname = _str_idfsInitialCaseStatus, _type = "Int64?",
              _get_func = o => o.idfsInitialCaseStatus,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsInitialCaseStatus != newval) o.idfsInitialCaseStatus = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsInitialCaseStatus != c.idfsInitialCaseStatus || o.IsRIRPropChanged(_str_idfsInitialCaseStatus, c)) 
                  m.Add(_str_idfsInitialCaseStatus, o.ObjectIdent + _str_idfsInitialCaseStatus, o.ObjectIdent2 + _str_idfsInitialCaseStatus, o.ObjectIdent3 + _str_idfsInitialCaseStatus, "Int64?", 
                    o.idfsInitialCaseStatus == null ? "" : o.idfsInitialCaseStatus.ToString(),                  
                  o.IsReadOnly(_str_idfsInitialCaseStatus), o.IsInvisible(_str_idfsInitialCaseStatus), o.IsRequired(_str_idfsInitialCaseStatus)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsSettlement, _formname = _str_idfsSettlement, _type = "Int64?",
              _get_func = o => o.idfsSettlement,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsSettlement != newval) o.idfsSettlement = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsSettlement != c.idfsSettlement || o.IsRIRPropChanged(_str_idfsSettlement, c)) 
                  m.Add(_str_idfsSettlement, o.ObjectIdent + _str_idfsSettlement, o.ObjectIdent2 + _str_idfsSettlement, o.ObjectIdent3 + _str_idfsSettlement, "Int64?", 
                    o.idfsSettlement == null ? "" : o.idfsSettlement.ToString(),                  
                  o.IsReadOnly(_str_idfsSettlement), o.IsInvisible(_str_idfsSettlement), o.IsRequired(_str_idfsSettlement)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsRegion, _formname = _str_idfsRegion, _type = "Int64?",
              _get_func = o => o.idfsRegion,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsRegion != newval) o.idfsRegion = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsRegion != c.idfsRegion || o.IsRIRPropChanged(_str_idfsRegion, c)) 
                  m.Add(_str_idfsRegion, o.ObjectIdent + _str_idfsRegion, o.ObjectIdent2 + _str_idfsRegion, o.ObjectIdent3 + _str_idfsRegion, "Int64?", 
                    o.idfsRegion == null ? "" : o.idfsRegion.ToString(),                  
                  o.IsReadOnly(_str_idfsRegion), o.IsInvisible(_str_idfsRegion), o.IsRequired(_str_idfsRegion)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsRayon, _formname = _str_idfsRayon, _type = "Int64?",
              _get_func = o => o.idfsRayon,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsRayon != newval) o.idfsRayon = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsRayon != c.idfsRayon || o.IsRIRPropChanged(_str_idfsRayon, c)) 
                  m.Add(_str_idfsRayon, o.ObjectIdent + _str_idfsRayon, o.ObjectIdent2 + _str_idfsRayon, o.ObjectIdent3 + _str_idfsRayon, "Int64?", 
                    o.idfsRayon == null ? "" : o.idfsRayon.ToString(),                  
                  o.IsReadOnly(_str_idfsRayon), o.IsInvisible(_str_idfsRayon), o.IsRequired(_str_idfsRayon)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsCountry, _formname = _str_idfsCountry, _type = "Int64?",
              _get_func = o => o.idfsCountry,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsCountry != newval) o.idfsCountry = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsCountry != c.idfsCountry || o.IsRIRPropChanged(_str_idfsCountry, c)) 
                  m.Add(_str_idfsCountry, o.ObjectIdent + _str_idfsCountry, o.ObjectIdent2 + _str_idfsCountry, o.ObjectIdent3 + _str_idfsCountry, "Int64?", 
                    o.idfsCountry == null ? "" : o.idfsCountry.ToString(),                  
                  o.IsReadOnly(_str_idfsCountry), o.IsInvisible(_str_idfsCountry), o.IsRequired(_str_idfsCountry)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsLocationOfExposureRayon, _formname = _str_idfsLocationOfExposureRayon, _type = "Int64?",
              _get_func = o => o.idfsLocationOfExposureRayon,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsLocationOfExposureRayon != newval) o.idfsLocationOfExposureRayon = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsLocationOfExposureRayon != c.idfsLocationOfExposureRayon || o.IsRIRPropChanged(_str_idfsLocationOfExposureRayon, c)) 
                  m.Add(_str_idfsLocationOfExposureRayon, o.ObjectIdent + _str_idfsLocationOfExposureRayon, o.ObjectIdent2 + _str_idfsLocationOfExposureRayon, o.ObjectIdent3 + _str_idfsLocationOfExposureRayon, "Int64?", 
                    o.idfsLocationOfExposureRayon == null ? "" : o.idfsLocationOfExposureRayon.ToString(),                  
                  o.IsReadOnly(_str_idfsLocationOfExposureRayon), o.IsInvisible(_str_idfsLocationOfExposureRayon), o.IsRequired(_str_idfsLocationOfExposureRayon)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsLocationOfExposureRegion, _formname = _str_idfsLocationOfExposureRegion, _type = "Int64?",
              _get_func = o => o.idfsLocationOfExposureRegion,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsLocationOfExposureRegion != newval) o.idfsLocationOfExposureRegion = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsLocationOfExposureRegion != c.idfsLocationOfExposureRegion || o.IsRIRPropChanged(_str_idfsLocationOfExposureRegion, c)) 
                  m.Add(_str_idfsLocationOfExposureRegion, o.ObjectIdent + _str_idfsLocationOfExposureRegion, o.ObjectIdent2 + _str_idfsLocationOfExposureRegion, o.ObjectIdent3 + _str_idfsLocationOfExposureRegion, "Int64?", 
                    o.idfsLocationOfExposureRegion == null ? "" : o.idfsLocationOfExposureRegion.ToString(),                  
                  o.IsReadOnly(_str_idfsLocationOfExposureRegion), o.IsInvisible(_str_idfsLocationOfExposureRegion), o.IsRequired(_str_idfsLocationOfExposureRegion)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_datFinalCaseClassificationDate, _formname = _str_datFinalCaseClassificationDate, _type = "DateTime?",
              _get_func = o => o.datFinalCaseClassificationDate,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datFinalCaseClassificationDate != newval) o.datFinalCaseClassificationDate = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datFinalCaseClassificationDate != c.datFinalCaseClassificationDate || o.IsRIRPropChanged(_str_datFinalCaseClassificationDate, c)) 
                  m.Add(_str_datFinalCaseClassificationDate, o.ObjectIdent + _str_datFinalCaseClassificationDate, o.ObjectIdent2 + _str_datFinalCaseClassificationDate, o.ObjectIdent3 + _str_datFinalCaseClassificationDate, "DateTime?", 
                    o.datFinalCaseClassificationDate == null ? "" : o.datFinalCaseClassificationDate.ToString(),                  
                  o.IsReadOnly(_str_datFinalCaseClassificationDate), o.IsInvisible(_str_datFinalCaseClassificationDate), o.IsRequired(_str_datFinalCaseClassificationDate)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_intRowStatus, _formname = _str_intRowStatus, _type = "Int32",
              _get_func = o => o.intRowStatus,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt32(val); if (o.intRowStatus != newval) o.intRowStatus = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.intRowStatus != c.intRowStatus || o.IsRIRPropChanged(_str_intRowStatus, c)) 
                  m.Add(_str_intRowStatus, o.ObjectIdent + _str_intRowStatus, o.ObjectIdent2 + _str_intRowStatus, o.ObjectIdent3 + _str_intRowStatus, "Int32", 
                    o.intRowStatus == null ? "" : o.intRowStatus.ToString(),                  
                  o.IsReadOnly(_str_intRowStatus), o.IsInvisible(_str_intRowStatus), o.IsRequired(_str_intRowStatus)); 
                  }
              }, 
        
            new field_info()
        };
        #endregion
        
        private string _getType(string name)
        {
            var i = _field_infos.FirstOrDefault(n => n._name == name);
            return i == null ? "" : i._type;
        }
        private object _getValue(string name)
        {
            var i = _field_infos.FirstOrDefault(n => n._name == name);
            return i == null ? null : i._get_func(this);
        }
        private void _setValue(string name, string val)
        {
            var i = _field_infos.FirstOrDefault(n => n._name == name);
            if (i != null) i._set_func(this, val);
        }
        internal CompareModel _compare(IObject o, CompareModel ret)
        {
            if (ret == null) ret = new CompareModel();
            if (o == null) return ret;
            HumanCaseListWithDeletedItem obj = (HumanCaseListWithDeletedItem)o;
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                Accessor.Instance(null)._LoadLookups(manager, obj);
                foreach (var i in _field_infos)
                    if (i != null && i._compare_func != null) i._compare_func(this, obj, ret, manager);
            }
            return ret;
        }
        #endregion
    
        private BvSelectList _getList(string name)
        {
        
            return null;
        }
    
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "HumanCaseListWithDeletedItem";

        #region Parent and Clone supporting
        [XmlIgnore]
        public IObject Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; /*OnPropertyChanged(_str_Parent);*/ }
        }
        private IObject m_Parent;
        internal void _setParent()
        {
        
        }
        partial void Cloned();
        partial void ClonedWithSetup();
        private bool bIsClone;
        public override object Clone()
        {
            var ret = base.Clone() as HumanCaseListWithDeletedItem;
            ret.bIsClone = true;
            ret.Cloned();
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret._setParent();
            if (this.IsDirty && !ret.IsDirty)
                ret.SetChange();
            else if (!this.IsDirty && ret.IsDirty)
                ret.RejectChanges();
            return ret;
        }
        public IObject CloneWithSetup(DbManagerProxy manager, bool bRestricted = false)
        {
            var ret = base.Clone() as HumanCaseListWithDeletedItem;
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        
            Accessor.Instance(null)._SetupLoad(manager, ret, true);
            ret.ClonedWithSetup();
            ret.DeepAcceptChanges();
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public HumanCaseListWithDeletedItem CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as HumanCaseListWithDeletedItem;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfCase; } }
        public string KeyName { get { return "idfCase"; } }
        public object KeyLookup { get { return idfCase; } }
        public string ToStringProp { get { return ToString(); } }
        private bool m_IsNew;
        public bool IsNew { get { return m_IsNew; } }
        [XmlIgnore]
        [LocalizedDisplayName("HasChanges")]
        public bool HasChanges 
        { 
            get 
            { 
                return IsDirty
        
                ;
            }
        }
        public new void RejectChanges()
        {
        
            base.RejectChanges();
        
        }
        public void DeepRejectChanges()
        {
            RejectChanges();
        
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        
        }
        private bool m_bForceDirty;
        public override void AcceptChanges()
        {
            m_bForceDirty = false;
            base.AcceptChanges();
        }
        [XmlIgnore]
        [LocalizedDisplayName("IsDirty")]
        public override bool IsDirty
        {
            get { return m_bForceDirty || base.IsDirty; }
        }
        public void SetChange()
        { 
            m_bForceDirty = true;
        }
        public void DeepSetChange()
        { 
            SetChange();
        
        }
        public bool MarkToDelete() { return _Delete(false); }
        public string ObjectName { get { return m_ObjectName; } }
        public string ObjectIdent { get { return ObjectName + "_" + Key.ToString() + "_"; } }
        
        public string ObjectIdent2 { get { return ObjectIdent; } }
          
        public string ObjectIdent3 { get { return ObjectIdent; } }
          
        public IObjectAccessor GetAccessor() { return _getAccessor(); }
        public IObjectPermissions GetPermissions() { return _permissions; }
        private IObjectEnvironment _environment;
        public IObjectEnvironment Environment { get { return _environment; } set { _environment = value; } }
        public bool IsValidObject { get { return _isValid; } }
        public bool ReadOnly { get { return _readOnly || !_isValid; } set { _readOnly = value; } }
        public bool IsReadOnly(string name) { return _isReadOnly(name); }
        public bool IsInvisible(string name) { return _isInvisible(name); }
        public bool IsRequired(string name) { return _isRequired(m_isRequired, name); }
        public bool IsHiddenPersonalData(string name) { return _isHiddenPersonalData(name); }
        public string GetType(string name) { return _getType(name); }
        public object GetValue(string name) { return _getValue(name); }
        public void SetValue(string name, string val) { _setValue(name, val); }
        public CompareModel Compare(IObject o) { return _compare(o, null); }
        public BvSelectList GetList(string name) { return _getList(name); }
        public event ValidationEvent Validation;
        public event ValidationEvent ValidationEnd;
        public event AfterPostEvent AfterPost;
      
        public Dictionary<string, string> GetFieldTags(string name)
        {
          return null;
        }
      #endregion

        private bool IsRIRPropChanged(string fld, HumanCaseListWithDeletedItem c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, HumanCaseListWithDeletedItem c)
        {
            var thisLookup = GetValue(fld + "Lookup") as IList;
            var thatLookup = c.GetValue(fld + "Lookup") as IList;
            bool bChangeLookupContent = thisLookup.Count != thatLookup.Count;
            if (!bChangeLookupContent)
            {
                for (int i = 0; i < thisLookup.Count; i++)
                {
                    if (((thisLookup[i] as IObject).Key as IComparable).CompareTo((thatLookup[i] as IObject).Key) != 0 &&
                        (thisLookup[i] as IObject).ToStringProp != null && ((thisLookup[i] as IObject).ToStringProp as IComparable).CompareTo((thatLookup[i] as IObject).ToStringProp) != 0)
                    {
                        bChangeLookupContent = true;
                        break;
                    }
                }
            }
            return bChangeLookupContent;
        }
        

      

        public HumanCaseListWithDeletedItem()
        {
            
        }

        partial void Changed(string fieldName);
        partial void Created(DbManagerProxy manager);
        partial void Loaded(DbManagerProxy manager);
        partial void Deleted();
        partial void ParsedFormCollection(NameValueCollection form);
        partial void ParsingFormCollection(NameValueCollection form);

        

        private bool m_IsForcedToDelete;
        [LocalizedDisplayName("IsForcedToDelete")]
        public bool IsForcedToDelete { get { return m_IsForcedToDelete; } }

        private bool m_IsMarkedToDelete;
        [LocalizedDisplayName("IsMarkedToDelete")]
        public bool IsMarkedToDelete { get { return m_IsMarkedToDelete; } }

        public void _SetupMainHandler()
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HumanCaseListWithDeletedItem_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(HumanCaseListWithDeletedItem_PropertyChanged);
        }
        private void HumanCaseListWithDeletedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as HumanCaseListWithDeletedItem).Changed(e.PropertyName);
            
        }
        
        public bool ForceToDelete() { return _Delete(true); }
        internal bool _Delete(bool isForceDelete)
        {
            if (!_ValidateOnDelete()) return false;
            _DeletingExtenders();
            m_IsMarkedToDelete = true;
            m_IsForcedToDelete = m_IsForcedToDelete ? m_IsForcedToDelete : isForceDelete;
            OnPropertyChanged("IsMarkedToDelete");
            _DeletedExtenders();
            Deleted();
            return true;
        }
        private bool _ValidateOnDelete(bool bReport = true)
        {
            
            return true;                
              
        }
        private void _DeletingExtenders()
        {
            HumanCaseListWithDeletedItem obj = this;
            
        }
        private void _DeletedExtenders()
        {
            HumanCaseListWithDeletedItem obj = this;
            
        }
        
        public bool OnValidation(ValidationModelException ex)
        {
            if (Validation != null)
            {
                var args = new ValidationEventArgs(ex.MessageId, ex.FieldName, ex.PropertyName, ex.Pars, ex.ValidatorType, ex.Obj, ex.ValidationType);
                Validation(this, args);
                return args.Continue;
            }
            return false;
        }
        public bool OnValidationEnd(ValidationModelException ex)
        {
            if (ValidationEnd != null)
            {
                var args = new ValidationEventArgs(ex.MessageId, ex.FieldName, ex.PropertyName, ex.Pars, ex.ValidatorType, ex.Obj, ex.ValidationType);
                ValidationEnd(this, args);
                return args.Continue;
            }
            return false;
        }

        public void OnAfterPost()
        {
            if (AfterPost != null)
            {
                AfterPost(this, EventArgs.Empty);
            }
        }

        public FormSize FormSize
        {
            get { return FormSize.Undefined; }
        }
    
        private bool _isInvisible(string name)
        {
            
            return false;
                
        }

    
        private bool _isReadOnly(string name)
        {
            
            return ReadOnly;
                
        }

        private bool m_isValid = true;
        internal bool _isValid
        {
            get { return m_isValid; }
            set
            {
                m_isValid = value;
        
            }
        }

        private bool m_readOnly;
        private bool _readOnly
        {
            get { return m_readOnly; }
            set
            {
                m_readOnly = value;
        
            }
        }


        internal Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<HumanCaseListWithDeletedItem, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<HumanCaseListWithDeletedItem, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~HumanCaseListWithDeletedItem()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (!bIsDisposed) 
            {
                bIsDisposed = true;
                m_Parent = null;
                m_permissions = null;
                
                this.ClearModelObjEventInvocations();
                
                
                if (bNeedLookupRemove)
                {
                
                }
                Disposed();
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    
        #region ILookupUsage Members
        public void ReloadLookupItem(DbManagerProxy manager, string lookup_object)
        {
            
        }
        #endregion
    
        public void ParseFormCollection(NameValueCollection form, bool bParseLookups = true, bool bParseRelations = true)
        {
            ParsingFormCollection(form);
            if (bParseLookups)
            {
                _field_infos.Where(i => i._type == "Lookup").ToList().ForEach(a => { if (form[ObjectIdent + a._formname] != null) a._set_func(this, form[ObjectIdent + a._formname]);} );
            }
            
            _field_infos.Where(i => i._type != "Lookup" && i._type != "Child" && i._type != "Relation" && i._type != null)
                .ToList().ForEach(a => { if (form.AllKeys.Contains(ObjectIdent + a._formname)) a._set_func(this, form[ObjectIdent + a._formname]);} );
      
            if (bParseRelations)
            {
        
            }
            ParsedFormCollection(form);
        }
    

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<HumanCaseListWithDeletedItem>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectCreator
            , IObjectCreator<HumanCaseListWithDeletedItem>
            
            , IObjectSelectList
            , IObjectSelectList<HumanCaseListWithDeletedItem>
                    
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfCase"; } }
            #endregion
        
            public delegate void on_action(HumanCaseListWithDeletedItem obj);
            private static Accessor g_Instance = CreateInstance<Accessor>();
            private CacheScope m_CS;
            public static Accessor Instance(CacheScope cs) 
            { 
                if (cs == null)
                    return g_Instance;
                lock(cs)
                {
                    object acc = cs.Get(typeof (Accessor));
                    if (acc != null)
                    {
                        return acc as Accessor;
                    }
                    Accessor ret = CreateInstance<Accessor>();
                    ret.m_CS = cs;
                    cs.Add(typeof(Accessor), ret);
                    return ret;
                }
            }
            
            public virtual List<HumanCaseListWithDeletedItem> SelectListT(DbManagerProxy manager
                , FilterParams filters = null
                , KeyValuePair<string, ListSortDirection>[] sorts = null
                , bool serverSort = false
                )
            {
                return _SelectList(manager
                , null, null
                , filters
                , sorts
                , serverSort
                );
            }
            public virtual List<IObject> SelectList(DbManagerProxy manager
                , FilterParams filters = null
                , KeyValuePair<string, ListSortDirection>[] sorts = null
                , bool serverSort = false
                )
            {
                return _SelectList(manager
                , null, null
                , filters
                , sorts
                , serverSort
                ).Cast<IObject>().ToList();
            }
            
            protected virtual List<HumanCaseListWithDeletedItem> _SelectList(DbManagerProxy manager
                , on_action loading, on_action loaded
                , FilterParams filters
                , KeyValuePair<string, ListSortDirection>[] sorts
                , bool serverSort = false
                )
            {
                if (filters == null) filters = new FilterParams();
                
                var sql = new StringBuilder();
                string maxtop = BaseSettings.SelectTopMaxCount.ToString();
                sql.Append(@"select ");
                
                sql.Append(@"top ");
                sql.Append(maxtop);
                sql.Append(@" dbo.fn_HumanCase_SelectListWithDeleted.* from dbo.fn_HumanCase_SelectListWithDeleted(@LangID
                    ) ");
                
                sql.Append(" where 0 = 0");
                
                if (filters.Contains("idfCase"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfCase"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfCase") ? " or " : " and ");
                        
                        if (filters.Operation("idfCase", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfCase,0) {0} @idfCase_{1} = @idfCase_{1})", filters.Operation("idfCase", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfCase,0) {0} @idfCase_{1}", filters.Operation("idfCase", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsDiagnosis"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsDiagnosis"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsDiagnosis") ? " or " : " and ");
                        
                        if (filters.Operation("idfsDiagnosis", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsDiagnosis,0) {0} @idfsDiagnosis_{1} = @idfsDiagnosis_{1})", filters.Operation("idfsDiagnosis", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsDiagnosis,0) {0} @idfsDiagnosis_{1}", filters.Operation("idfsDiagnosis", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("DiagnosisName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("DiagnosisName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("DiagnosisName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.DiagnosisName {0} @DiagnosisName_{1}", filters.Operation("DiagnosisName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsCaseStatus"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsCaseStatus"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsCaseStatus") ? " or " : " and ");
                        
                        if (filters.Operation("idfsCaseStatus", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsCaseStatus,0) {0} @idfsCaseStatus_{1} = @idfsCaseStatus_{1})", filters.Operation("idfsCaseStatus", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsCaseStatus,0) {0} @idfsCaseStatus_{1}", filters.Operation("idfsCaseStatus", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsCaseProgressStatus"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsCaseProgressStatus"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsCaseProgressStatus") ? " or " : " and ");
                        
                        if (filters.Operation("idfsCaseProgressStatus", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsCaseProgressStatus,0) {0} @idfsCaseProgressStatus_{1} = @idfsCaseProgressStatus_{1})", filters.Operation("idfsCaseProgressStatus", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsCaseProgressStatus,0) {0} @idfsCaseProgressStatus_{1}", filters.Operation("idfsCaseProgressStatus", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("CaseStatusName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("CaseStatusName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("CaseStatusName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.CaseStatusName {0} @CaseStatusName_{1}", filters.Operation("CaseStatusName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("CaseProgressStatusName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("CaseProgressStatusName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("CaseProgressStatusName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.CaseProgressStatusName {0} @CaseProgressStatusName_{1}", filters.Operation("CaseProgressStatusName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("datEnteredDate"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("datEnteredDate"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("datEnteredDate") ? " or " : " and ");
                        
                        sql.AppendFormat("CONVERT(NVARCHAR(8), fn_HumanCase_SelectListWithDeleted.datEnteredDate, 112) {0} CONVERT(NVARCHAR(8), @datEnteredDate_{1}, 112)", filters.Operation("datEnteredDate", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("strCaseID"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("strCaseID"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("strCaseID") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.strCaseID {0} @strCaseID_{1}", filters.Operation("strCaseID", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsSite"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsSite"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsSite") ? " or " : " and ");
                        
                        if (filters.Operation("idfsSite", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsSite,0) {0} @idfsSite_{1} = @idfsSite_{1})", filters.Operation("idfsSite", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsSite,0) {0} @idfsSite_{1}", filters.Operation("idfsSite", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("datCompletionPaperFormDate"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("datCompletionPaperFormDate"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("datCompletionPaperFormDate") ? " or " : " and ");
                        
                        sql.AppendFormat("CONVERT(NVARCHAR(8), fn_HumanCase_SelectListWithDeleted.datCompletionPaperFormDate, 112) {0} CONVERT(NVARCHAR(8), @datCompletionPaperFormDate_{1}, 112)", filters.Operation("datCompletionPaperFormDate", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("strLocalIdentifier"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("strLocalIdentifier"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("strLocalIdentifier") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.strLocalIdentifier {0} @strLocalIdentifier_{1}", filters.Operation("strLocalIdentifier", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfPersonEnteredBy"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfPersonEnteredBy"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfPersonEnteredBy") ? " or " : " and ");
                        
                        if (filters.Operation("idfPersonEnteredBy", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfPersonEnteredBy,0) {0} @idfPersonEnteredBy_{1} = @idfPersonEnteredBy_{1})", filters.Operation("idfPersonEnteredBy", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfPersonEnteredBy,0) {0} @idfPersonEnteredBy_{1}", filters.Operation("idfPersonEnteredBy", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfHuman"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfHuman"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfHuman") ? " or " : " and ");
                        
                        if (filters.Operation("idfHuman", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfHuman,0) {0} @idfHuman_{1} = @idfHuman_{1})", filters.Operation("idfHuman", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfHuman,0) {0} @idfHuman_{1}", filters.Operation("idfHuman", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("strLastName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("strLastName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("strLastName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.strLastName {0} @strLastName_{1}", filters.Operation("strLastName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("strFirstName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("strFirstName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("strFirstName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.strFirstName {0} @strFirstName_{1}", filters.Operation("strFirstName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("strSecondName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("strSecondName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("strSecondName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.strSecondName {0} @strSecondName_{1}", filters.Operation("strSecondName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("PatientName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("PatientName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("PatientName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.PatientName {0} @PatientName_{1}", filters.Operation("PatientName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("datDateofBirth"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("datDateofBirth"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("datDateofBirth") ? " or " : " and ");
                        
                        sql.AppendFormat("CONVERT(NVARCHAR(8), fn_HumanCase_SelectListWithDeleted.datDateofBirth, 112) {0} CONVERT(NVARCHAR(8), @datDateofBirth_{1}, 112)", filters.Operation("datDateofBirth", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("intPatientAge"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("intPatientAge"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("intPatientAge") ? " or " : " and ");
                        
                        if (filters.Operation("intPatientAge", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.intPatientAge,0) {0} @intPatientAge_{1} = @intPatientAge_{1})", filters.Operation("intPatientAge", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.intPatientAge,0) {0} @intPatientAge_{1}", filters.Operation("intPatientAge", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsHumanAgeType"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsHumanAgeType"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsHumanAgeType") ? " or " : " and ");
                        
                        if (filters.Operation("idfsHumanAgeType", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsHumanAgeType,0) {0} @idfsHumanAgeType_{1} = @idfsHumanAgeType_{1})", filters.Operation("idfsHumanAgeType", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsHumanAgeType,0) {0} @idfsHumanAgeType_{1}", filters.Operation("idfsHumanAgeType", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("Age"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("Age"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("Age") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.Age {0} @Age_{1}", filters.Operation("Age", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfAddress"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfAddress"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfAddress") ? " or " : " and ");
                        
                        if (filters.Operation("idfAddress", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfAddress,0) {0} @idfAddress_{1} = @idfAddress_{1})", filters.Operation("idfAddress", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfAddress,0) {0} @idfAddress_{1}", filters.Operation("idfAddress", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfGeoLocation"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfGeoLocation"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfGeoLocation") ? " or " : " and ");
                        
                        if (filters.Operation("idfGeoLocation", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfGeoLocation,0) {0} @idfGeoLocation_{1} = @idfGeoLocation_{1})", filters.Operation("idfGeoLocation", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfGeoLocation,0) {0} @idfGeoLocation_{1}", filters.Operation("idfGeoLocation", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("GeoLocationName"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("GeoLocationName"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("GeoLocationName") ? " or " : " and ");
                        
                        sql.AppendFormat("fn_HumanCase_SelectListWithDeleted.GeoLocationName {0} @GeoLocationName_{1}", filters.Operation("GeoLocationName", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfEpiObservation"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfEpiObservation"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfEpiObservation") ? " or " : " and ");
                        
                        if (filters.Operation("idfEpiObservation", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfEpiObservation,0) {0} @idfEpiObservation_{1} = @idfEpiObservation_{1})", filters.Operation("idfEpiObservation", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfEpiObservation,0) {0} @idfEpiObservation_{1}", filters.Operation("idfEpiObservation", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfCSObservation"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfCSObservation"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfCSObservation") ? " or " : " and ");
                        
                        if (filters.Operation("idfCSObservation", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfCSObservation,0) {0} @idfCSObservation_{1} = @idfCSObservation_{1})", filters.Operation("idfCSObservation", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfCSObservation,0) {0} @idfCSObservation_{1}", filters.Operation("idfCSObservation", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsTentativeDiagnosis"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsTentativeDiagnosis"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsTentativeDiagnosis") ? " or " : " and ");
                        
                        if (filters.Operation("idfsTentativeDiagnosis", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsTentativeDiagnosis,0) {0} @idfsTentativeDiagnosis_{1} = @idfsTentativeDiagnosis_{1})", filters.Operation("idfsTentativeDiagnosis", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsTentativeDiagnosis,0) {0} @idfsTentativeDiagnosis_{1}", filters.Operation("idfsTentativeDiagnosis", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsInitialCaseStatus"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsInitialCaseStatus"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsInitialCaseStatus") ? " or " : " and ");
                        
                        if (filters.Operation("idfsInitialCaseStatus", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsInitialCaseStatus,0) {0} @idfsInitialCaseStatus_{1} = @idfsInitialCaseStatus_{1})", filters.Operation("idfsInitialCaseStatus", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsInitialCaseStatus,0) {0} @idfsInitialCaseStatus_{1}", filters.Operation("idfsInitialCaseStatus", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsSettlement"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsSettlement"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsSettlement") ? " or " : " and ");
                        
                        if (filters.Operation("idfsSettlement", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsSettlement,0) {0} @idfsSettlement_{1} = @idfsSettlement_{1})", filters.Operation("idfsSettlement", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsSettlement,0) {0} @idfsSettlement_{1}", filters.Operation("idfsSettlement", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsRegion"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsRegion"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsRegion") ? " or " : " and ");
                        
                        if (filters.Operation("idfsRegion", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsRegion,0) {0} @idfsRegion_{1} = @idfsRegion_{1})", filters.Operation("idfsRegion", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsRegion,0) {0} @idfsRegion_{1}", filters.Operation("idfsRegion", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsRayon"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsRayon"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsRayon") ? " or " : " and ");
                        
                        if (filters.Operation("idfsRayon", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsRayon,0) {0} @idfsRayon_{1} = @idfsRayon_{1})", filters.Operation("idfsRayon", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsRayon,0) {0} @idfsRayon_{1}", filters.Operation("idfsRayon", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsCountry"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsCountry"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsCountry") ? " or " : " and ");
                        
                        if (filters.Operation("idfsCountry", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsCountry,0) {0} @idfsCountry_{1} = @idfsCountry_{1})", filters.Operation("idfsCountry", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsCountry,0) {0} @idfsCountry_{1}", filters.Operation("idfsCountry", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsLocationOfExposureRayon"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsLocationOfExposureRayon"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsLocationOfExposureRayon") ? " or " : " and ");
                        
                        if (filters.Operation("idfsLocationOfExposureRayon", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsLocationOfExposureRayon,0) {0} @idfsLocationOfExposureRayon_{1} = @idfsLocationOfExposureRayon_{1})", filters.Operation("idfsLocationOfExposureRayon", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsLocationOfExposureRayon,0) {0} @idfsLocationOfExposureRayon_{1}", filters.Operation("idfsLocationOfExposureRayon", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("idfsLocationOfExposureRegion"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("idfsLocationOfExposureRegion"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("idfsLocationOfExposureRegion") ? " or " : " and ");
                        
                        if (filters.Operation("idfsLocationOfExposureRegion", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.idfsLocationOfExposureRegion,0) {0} @idfsLocationOfExposureRegion_{1} = @idfsLocationOfExposureRegion_{1})", filters.Operation("idfsLocationOfExposureRegion", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.idfsLocationOfExposureRegion,0) {0} @idfsLocationOfExposureRegion_{1}", filters.Operation("idfsLocationOfExposureRegion", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("datFinalCaseClassificationDate"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("datFinalCaseClassificationDate"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("datFinalCaseClassificationDate") ? " or " : " and ");
                        
                        sql.AppendFormat("CONVERT(NVARCHAR(8), fn_HumanCase_SelectListWithDeleted.datFinalCaseClassificationDate, 112) {0} CONVERT(NVARCHAR(8), @datFinalCaseClassificationDate_{1}, 112)", filters.Operation("datFinalCaseClassificationDate", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (filters.Contains("intRowStatus"))
                {
                    sql.AppendFormat(" and (");
                    for (int i = 0; i < filters.Count("intRowStatus"); i++)
                    {
                        if (i > 0) 
                          sql.AppendFormat(filters.IsOr("intRowStatus") ? " or " : " and ");
                        
                        if (filters.Operation("intRowStatus", i) == "&")
                          sql.AppendFormat("(isnull(fn_HumanCase_SelectListWithDeleted.intRowStatus,0) {0} @intRowStatus_{1} = @intRowStatus_{1})", filters.Operation("intRowStatus", i), i);
                        else
                          sql.AppendFormat("isnull(fn_HumanCase_SelectListWithDeleted.intRowStatus,0) {0} @intRowStatus_{1}", filters.Operation("intRowStatus", i), i);
                            
                    }
                    sql.AppendFormat(")");
                }
                  
                if (serverSort && sorts != null && sorts.Length > 0)
                {
                    sql.Append(" order by");
                    bool bFirst = true;
                        foreach(var sort in sorts)
                        {
                            sql.Append((bFirst ? " " : ", ") + sort.Key + " " + (sort.Value == ListSortDirection.Ascending ? "ASC" : "DESC"));
                            bFirst = false;
                        }
                }
                  

                bool bTransactionStarted = false;
                try
                {
                    if (!manager.IsTransactionStarted)
                    {
                        bTransactionStarted = true;
                        manager.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                    }
                    
                    sql.Append(" option (OPTIMIZE FOR UNKNOWN)");
                    manager
                        .SetCommand(sql.ToString()
                            , manager.Parameter("@LangID", ModelUserContext.CurrentLanguage)
                            
                        );
                    
                    if (filters.Contains("idfCase"))
                        for (int i = 0; i < filters.Count("idfCase"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfCase_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfCase", i), filters.Value("idfCase", i))));
                      
                    if (filters.Contains("idfsDiagnosis"))
                        for (int i = 0; i < filters.Count("idfsDiagnosis"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsDiagnosis_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsDiagnosis", i), filters.Value("idfsDiagnosis", i))));
                      
                    if (filters.Contains("DiagnosisName"))
                        for (int i = 0; i < filters.Count("DiagnosisName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@DiagnosisName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("DiagnosisName", i), filters.Value("DiagnosisName", i))));
                      
                    if (filters.Contains("idfsCaseStatus"))
                        for (int i = 0; i < filters.Count("idfsCaseStatus"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsCaseStatus_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsCaseStatus", i), filters.Value("idfsCaseStatus", i))));
                      
                    if (filters.Contains("idfsCaseProgressStatus"))
                        for (int i = 0; i < filters.Count("idfsCaseProgressStatus"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsCaseProgressStatus_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsCaseProgressStatus", i), filters.Value("idfsCaseProgressStatus", i))));
                      
                    if (filters.Contains("CaseStatusName"))
                        for (int i = 0; i < filters.Count("CaseStatusName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@CaseStatusName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("CaseStatusName", i), filters.Value("CaseStatusName", i))));
                      
                    if (filters.Contains("CaseProgressStatusName"))
                        for (int i = 0; i < filters.Count("CaseProgressStatusName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@CaseProgressStatusName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("CaseProgressStatusName", i), filters.Value("CaseProgressStatusName", i))));
                      
                    if (filters.Contains("datEnteredDate"))
                        for (int i = 0; i < filters.Count("datEnteredDate"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@datEnteredDate_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("datEnteredDate", i), filters.Value("datEnteredDate", i))));
                      
                    if (filters.Contains("strCaseID"))
                        for (int i = 0; i < filters.Count("strCaseID"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@strCaseID_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("strCaseID", i), filters.Value("strCaseID", i))));
                      
                    if (filters.Contains("idfsSite"))
                        for (int i = 0; i < filters.Count("idfsSite"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsSite_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsSite", i), filters.Value("idfsSite", i))));
                      
                    if (filters.Contains("datCompletionPaperFormDate"))
                        for (int i = 0; i < filters.Count("datCompletionPaperFormDate"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@datCompletionPaperFormDate_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("datCompletionPaperFormDate", i), filters.Value("datCompletionPaperFormDate", i))));
                      
                    if (filters.Contains("strLocalIdentifier"))
                        for (int i = 0; i < filters.Count("strLocalIdentifier"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@strLocalIdentifier_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("strLocalIdentifier", i), filters.Value("strLocalIdentifier", i))));
                      
                    if (filters.Contains("idfPersonEnteredBy"))
                        for (int i = 0; i < filters.Count("idfPersonEnteredBy"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfPersonEnteredBy_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfPersonEnteredBy", i), filters.Value("idfPersonEnteredBy", i))));
                      
                    if (filters.Contains("idfHuman"))
                        for (int i = 0; i < filters.Count("idfHuman"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfHuman_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfHuman", i), filters.Value("idfHuman", i))));
                      
                    if (filters.Contains("strLastName"))
                        for (int i = 0; i < filters.Count("strLastName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@strLastName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("strLastName", i), filters.Value("strLastName", i))));
                      
                    if (filters.Contains("strFirstName"))
                        for (int i = 0; i < filters.Count("strFirstName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@strFirstName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("strFirstName", i), filters.Value("strFirstName", i))));
                      
                    if (filters.Contains("strSecondName"))
                        for (int i = 0; i < filters.Count("strSecondName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@strSecondName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("strSecondName", i), filters.Value("strSecondName", i))));
                      
                    if (filters.Contains("PatientName"))
                        for (int i = 0; i < filters.Count("PatientName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@PatientName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("PatientName", i), filters.Value("PatientName", i))));
                      
                    if (filters.Contains("datDateofBirth"))
                        for (int i = 0; i < filters.Count("datDateofBirth"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@datDateofBirth_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("datDateofBirth", i), filters.Value("datDateofBirth", i))));
                      
                    if (filters.Contains("intPatientAge"))
                        for (int i = 0; i < filters.Count("intPatientAge"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@intPatientAge_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("intPatientAge", i), filters.Value("intPatientAge", i))));
                      
                    if (filters.Contains("idfsHumanAgeType"))
                        for (int i = 0; i < filters.Count("idfsHumanAgeType"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsHumanAgeType_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsHumanAgeType", i), filters.Value("idfsHumanAgeType", i))));
                      
                    if (filters.Contains("Age"))
                        for (int i = 0; i < filters.Count("Age"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@Age_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("Age", i), filters.Value("Age", i))));
                      
                    if (filters.Contains("idfAddress"))
                        for (int i = 0; i < filters.Count("idfAddress"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfAddress_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfAddress", i), filters.Value("idfAddress", i))));
                      
                    if (filters.Contains("idfGeoLocation"))
                        for (int i = 0; i < filters.Count("idfGeoLocation"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfGeoLocation_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfGeoLocation", i), filters.Value("idfGeoLocation", i))));
                      
                    if (filters.Contains("GeoLocationName"))
                        for (int i = 0; i < filters.Count("GeoLocationName"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@GeoLocationName_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("GeoLocationName", i), filters.Value("GeoLocationName", i))));
                      
                    if (filters.Contains("idfEpiObservation"))
                        for (int i = 0; i < filters.Count("idfEpiObservation"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfEpiObservation_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfEpiObservation", i), filters.Value("idfEpiObservation", i))));
                      
                    if (filters.Contains("idfCSObservation"))
                        for (int i = 0; i < filters.Count("idfCSObservation"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfCSObservation_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfCSObservation", i), filters.Value("idfCSObservation", i))));
                      
                    if (filters.Contains("idfsTentativeDiagnosis"))
                        for (int i = 0; i < filters.Count("idfsTentativeDiagnosis"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsTentativeDiagnosis_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsTentativeDiagnosis", i), filters.Value("idfsTentativeDiagnosis", i))));
                      
                    if (filters.Contains("idfsInitialCaseStatus"))
                        for (int i = 0; i < filters.Count("idfsInitialCaseStatus"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsInitialCaseStatus_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsInitialCaseStatus", i), filters.Value("idfsInitialCaseStatus", i))));
                      
                    if (filters.Contains("idfsSettlement"))
                        for (int i = 0; i < filters.Count("idfsSettlement"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsSettlement_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsSettlement", i), filters.Value("idfsSettlement", i))));
                      
                    if (filters.Contains("idfsRegion"))
                        for (int i = 0; i < filters.Count("idfsRegion"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsRegion_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsRegion", i), filters.Value("idfsRegion", i))));
                      
                    if (filters.Contains("idfsRayon"))
                        for (int i = 0; i < filters.Count("idfsRayon"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsRayon_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsRayon", i), filters.Value("idfsRayon", i))));
                      
                    if (filters.Contains("idfsCountry"))
                        for (int i = 0; i < filters.Count("idfsCountry"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsCountry_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsCountry", i), filters.Value("idfsCountry", i))));
                      
                    if (filters.Contains("idfsLocationOfExposureRayon"))
                        for (int i = 0; i < filters.Count("idfsLocationOfExposureRayon"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsLocationOfExposureRayon_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsLocationOfExposureRayon", i), filters.Value("idfsLocationOfExposureRayon", i))));
                      
                    if (filters.Contains("idfsLocationOfExposureRegion"))
                        for (int i = 0; i < filters.Count("idfsLocationOfExposureRegion"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@idfsLocationOfExposureRegion_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("idfsLocationOfExposureRegion", i), filters.Value("idfsLocationOfExposureRegion", i))));
                      
                    if (filters.Contains("datFinalCaseClassificationDate"))
                        for (int i = 0; i < filters.Count("datFinalCaseClassificationDate"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@datFinalCaseClassificationDate_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("datFinalCaseClassificationDate", i), filters.Value("datFinalCaseClassificationDate", i))));
                      
                    if (filters.Contains("intRowStatus"))
                        for (int i = 0; i < filters.Count("intRowStatus"); i++)
                            manager.SelectCommand.Parameters.Add(manager.InputParameter("@intRowStatus_" + i, ParsingHelper.CorrectLikeValue(filters.Operation("intRowStatus", i), filters.Value("intRowStatus", i))));
                      
                    List<HumanCaseListWithDeletedItem> objs = manager.ExecuteList<HumanCaseListWithDeletedItem>();
                    if (bTransactionStarted)
                    {
                        manager.CommitTransaction();
                        
                        // restore default isolation level for pool connection
                        manager.BeginTransaction();
                        manager.TestConnection();
                        manager.CommitTransaction();
                    }
                    ListSelected(manager, objs);
                    return objs;
                }
                catch(DataException e)
                {
                    if (bTransactionStarted)
                    {
                        manager.RollbackTransaction();
                        
                        // restore default isolation level for pool connection
                        manager.BeginTransaction();
                        manager.TestConnection();
                        manager.RollbackTransaction();
                    }
                    throw DbModelException.Create(null, e);
                }
            }
            partial void ListSelected(DbManagerProxy manager, List<HumanCaseListWithDeletedItem> objs);
            
            public virtual long? SelectCount(DbManagerProxy manager)
            {
                
                return null;
                    
            }
        
        
        
            internal void _SetupLoad(DbManagerProxy manager, HumanCaseListWithDeletedItem obj, bool bCloning = false)
            {
                if (obj == null) return;
                
                // loading extenters - begin
                // loading extenters - end
                if (!bCloning)
                {
                
                }
                _LoadLookups(manager, obj);
                obj._setParent();
                
                // loaded extenters - begin
                // loaded extenters - end
                
                _SetupHandlers(obj);
                _SetupChildHandlers(obj, null);
                
                _SetPermitions(obj._permissions, obj);
                _SetupRequired(obj);
                _SetupPersonalDataRestrictions(obj);
                obj._SetupMainHandler();
                obj.AcceptChanges();
            }
            
            internal void _SetPermitions(IObjectPermissions permissions, HumanCaseListWithDeletedItem obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                    }
                }
            }

    

            private HumanCaseListWithDeletedItem _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                HumanCaseListWithDeletedItem obj = null;
                try
                {
                    obj = HumanCaseListWithDeletedItem.CreateInstance();
                    obj.m_CS = m_CS;
                    obj.m_IsNew = true;
                    obj.Parent = Parent;
                    
                    if (creating != null)
                        creating(obj);
                
                    // creating extenters - begin
                    // creating extenters - end
                
                    _LoadLookups(manager, obj);
                    _SetupHandlers(obj);
                    _SetupChildHandlers(obj, null);
                    
                    obj._SetupMainHandler();
                    obj._setParent();
                
                    // created extenters - begin
                    // created extenters - end
        
                    if (created != null)
                        created(obj);
                    obj.Created(manager);
                    _SetPermitions(obj._permissions, obj);
                    _SetupRequired(obj);
                    _SetupPersonalDataRestrictions(obj);
                    return obj;
                }
                catch(DataException e)
                {
                    throw DbModelException.Create(obj, e);
                }
            }

            
            public HumanCaseListWithDeletedItem CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public HumanCaseListWithDeletedItem CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public HumanCaseListWithDeletedItem CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            private void _SetupChildHandlers(HumanCaseListWithDeletedItem obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(HumanCaseListWithDeletedItem obj)
            {
                
            }
    

            internal void _LoadLookups(DbManagerProxy manager, HumanCaseListWithDeletedItem obj)
            {
                
            }
    
            public bool Post(DbManagerProxy manager, IObject obj, bool bChildObject = false) 
            {
                throw new NotImplementedException();
            }
            
      
            protected ValidationModelException ChainsValidate(HumanCaseListWithDeletedItem obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(HumanCaseListWithDeletedItem obj, bool bRethrowException)
            {
                ValidationModelException ex = ChainsValidate(obj);
                if (ex != null)
                {
                    if (bRethrowException)
                        throw ex;
                    if (!obj.OnValidation(ex))
                    {
                        obj.OnValidationEnd(ex);
                        return false;
                    }
                }
                return true;
            }
        
            public bool Validate(DbManagerProxy manager, IObject obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                return Validate(manager, obj as HumanCaseListWithDeletedItem, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, HumanCaseListWithDeletedItem obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                return true;
            }
           
    
            private void _SetupRequired(HumanCaseListWithDeletedItem obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(HumanCaseListWithDeletedItem obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as HumanCaseListWithDeletedItem) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as HumanCaseListWithDeletedItem) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "HumanCaseListWithDeletedItemDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
        #region Meta
        public static class Meta
        {
            public static string spSelect = "fn_HumanCase_SelectListWithDeleted";
            public static string spCount = "";
            public static string spPost = "";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>> RequiredByField = new Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>>();
            public static Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>> RequiredByProperty = new Dictionary<string, Func<HumanCaseListWithDeletedItem, bool>>();
            public static List<SearchPanelMetaItem> SearchPanelMeta = new List<SearchPanelMetaItem>();
            public static List<GridMetaItem> GridMeta = new List<GridMetaItem>();
            public static List<ActionMetaItem> Actions = new List<ActionMetaItem>();
            
            
    private static Dictionary<string, List<Func<bool>>> m_isHiddenPersonalData = new Dictionary<string, List<Func<bool>>>();
    internal static bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData.ContainsKey(name))
          return m_isHiddenPersonalData[name].Aggregate(false, (s,c) => s | c());
      return false;
    }

    private static void AddHiddenPersonalData(string name, Func<bool> func)
    {
      if (!m_isHiddenPersonalData.ContainsKey(name))
          m_isHiddenPersonalData.Add(name, new List<Func<bool>>());
      m_isHiddenPersonalData[name].Add(func);
    }
  
            
            static Meta()
            {
                
                Sizes.Add(_str_DiagnosisName, 2000);
                Sizes.Add(_str_CaseStatusName, 2000);
                Sizes.Add(_str_CaseProgressStatusName, 2000);
                Sizes.Add(_str_strCaseID, 200);
                Sizes.Add(_str_strLocalIdentifier, 200);
                Sizes.Add(_str_strLastName, 200);
                Sizes.Add(_str_strFirstName, 200);
                Sizes.Add(_str_strSecondName, 200);
                Sizes.Add(_str_PatientName, 400);
                Sizes.Add(_str_Age, 2013);
                Sizes.Add(_str_GeoLocationName, 2000);
                Actions.Add(new ActionMetaItem(
                    "SelectAll",
                    ActionTypes.SelectAll,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strSelectAll_Id",
                        "selectall",
                        /*from BvMessages*/"tooltipSelectAll_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipSelectAll_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Top,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Select",
                    ActionTypes.Select,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strSelect_Id",
                        "select",
                        /*from BvMessages*/"tooltipSelect_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipSelect_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Top,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Create",
                    ActionTypes.Create,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, ObjectAccessor.CreatorInterface<HumanCase>().CreateNew(manager, c, pars == null ? null : (int?)pars[0])),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strCreate_Id",
                        "add",
                        /*from BvMessages*/"tooltipCreate_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipCreate_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Top,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Edit",
                    ActionTypes.Edit,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, ObjectAccessor.SelectDetailInterface<HumanCase>().SelectDetail(manager, pars[0])),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strEdit_Id",
                        "edit",
                        /*from BvMessages*/"tooltipEdit_Id",
                        /*from BvMessages*/"strView_Id",
                        "View1",
                        /*from BvMessages*/"tooltipEdit_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Top,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Delete",
                    ActionTypes.Delete,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(ObjectAccessor.DeleteInterface<HumanCase>().DeleteObject(manager, c == null ? pars[0] : c.Key), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strDelete_Id",
                        "Delete_Remove",
                        /*from BvMessages*/"tooltipDelete_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipDelete_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Top,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Refresh",
                    ActionTypes.Refresh,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strRefresh_Id",
                        "iconRefresh_Id",
                        /*from BvMessages*/"tooltipRefresh_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipRefresh_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Close",
                    ActionTypes.Close,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strClose_Id",
                        "Close",
                        /*from BvMessages*/"tooltipClose_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipClose_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
                Actions.Add(new ActionMetaItem(
                    "Report",
                    ActionTypes.Report,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strReport_Id",
                        "Report",
                        /*from BvMessages*/"tooltipReport_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipReport_Id",
                        ActionsAlignment.Left,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      null,
                      null,
                      null,
                      false
                      ));
                    
        
                _SetupPersonalDataRestrictions();
            }
            
            
    private static void _SetupPersonalDataRestrictions()
    {
    

    }
  
        }
        #endregion
    

        #endregion
        }
    
}
	