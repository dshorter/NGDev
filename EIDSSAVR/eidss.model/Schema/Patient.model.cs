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
    public abstract partial class Patient : 
        EditableObject<Patient>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfHuman), NonUpdatable, PrimaryKey]
        public abstract Int64 idfHuman { get; set; }
                
        [LocalizedDisplayName(_str_idfRootHuman)]
        [MapField(_str_idfRootHuman)]
        public abstract Int64? idfRootHuman { get; set; }
        protected Int64? idfRootHuman_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfRootHuman).OriginalValue; } }
        protected Int64? idfRootHuman_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfRootHuman).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfCase)]
        [MapField(_str_idfCase)]
        public abstract Int64? idfCase { get; set; }
        protected Int64? idfCase_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfCase).OriginalValue; } }
        protected Int64? idfCase_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfCase).PreviousValue; } }
                
        [LocalizedDisplayName(_str_intPatientAgeFromCase)]
        [MapField(_str_intPatientAgeFromCase)]
        public abstract Int32? intPatientAgeFromCase { get; set; }
        protected Int32? intPatientAgeFromCase_Original { get { return ((EditableValue<Int32?>)((dynamic)this)._intPatientAgeFromCase).OriginalValue; } }
        protected Int32? intPatientAgeFromCase_Previous { get { return ((EditableValue<Int32?>)((dynamic)this)._intPatientAgeFromCase).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsHumanAgeTypeFromCase)]
        [MapField(_str_idfsHumanAgeTypeFromCase)]
        public abstract Int64? idfsHumanAgeTypeFromCase { get; set; }
        protected Int64? idfsHumanAgeTypeFromCase_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsHumanAgeTypeFromCase).OriginalValue; } }
        protected Int64? idfsHumanAgeTypeFromCase_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsHumanAgeTypeFromCase).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsOccupationType)]
        [MapField(_str_idfsOccupationType)]
        public abstract Int64? idfsOccupationType { get; set; }
        protected Int64? idfsOccupationType_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsOccupationType).OriginalValue; } }
        protected Int64? idfsOccupationType_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsOccupationType).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsNationality)]
        [MapField(_str_idfsNationality)]
        public abstract Int64? idfsNationality { get; set; }
        protected Int64? idfsNationality_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsNationality).OriginalValue; } }
        protected Int64? idfsNationality_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsNationality).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsHumanGender)]
        [MapField(_str_idfsHumanGender)]
        public abstract Int64? idfsHumanGender { get; set; }
        protected Int64? idfsHumanGender_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsHumanGender).OriginalValue; } }
        protected Int64? idfsHumanGender_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsHumanGender).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfCurrentResidenceAddress)]
        [MapField(_str_idfCurrentResidenceAddress)]
        public abstract Int64? idfCurrentResidenceAddress { get; set; }
        protected Int64? idfCurrentResidenceAddress_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfCurrentResidenceAddress).OriginalValue; } }
        protected Int64? idfCurrentResidenceAddress_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfCurrentResidenceAddress).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfEmployerAddress)]
        [MapField(_str_idfEmployerAddress)]
        public abstract Int64? idfEmployerAddress { get; set; }
        protected Int64? idfEmployerAddress_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfEmployerAddress).OriginalValue; } }
        protected Int64? idfEmployerAddress_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfEmployerAddress).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfRegistrationAddress)]
        [MapField(_str_idfRegistrationAddress)]
        public abstract Int64? idfRegistrationAddress { get; set; }
        protected Int64? idfRegistrationAddress_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfRegistrationAddress).OriginalValue; } }
        protected Int64? idfRegistrationAddress_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfRegistrationAddress).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datDateofBirth)]
        [MapField(_str_datDateofBirth)]
        public abstract DateTime? datDateofBirth { get; set; }
        protected DateTime? datDateofBirth_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datDateofBirth).OriginalValue; } }
        protected DateTime? datDateofBirth_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datDateofBirth).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datDateOfDeath)]
        [MapField(_str_datDateOfDeath)]
        public abstract DateTime? datDateOfDeath { get; set; }
        protected DateTime? datDateOfDeath_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datDateOfDeath).OriginalValue; } }
        protected DateTime? datDateOfDeath_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datDateOfDeath).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strLastName)]
        [MapField(_str_strLastName)]
        public abstract String strLastName { get; set; }
        protected String strLastName_Original { get { return ((EditableValue<String>)((dynamic)this)._strLastName).OriginalValue; } }
        protected String strLastName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strLastName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strSecondName)]
        [MapField(_str_strSecondName)]
        public abstract String strSecondName { get; set; }
        protected String strSecondName_Original { get { return ((EditableValue<String>)((dynamic)this)._strSecondName).OriginalValue; } }
        protected String strSecondName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strSecondName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strFirstName)]
        [MapField(_str_strFirstName)]
        public abstract String strFirstName { get; set; }
        protected String strFirstName_Original { get { return ((EditableValue<String>)((dynamic)this)._strFirstName).OriginalValue; } }
        protected String strFirstName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strFirstName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strRegistrationPhone)]
        [MapField(_str_strRegistrationPhone)]
        public abstract String strRegistrationPhone { get; set; }
        protected String strRegistrationPhone_Original { get { return ((EditableValue<String>)((dynamic)this)._strRegistrationPhone).OriginalValue; } }
        protected String strRegistrationPhone_Previous { get { return ((EditableValue<String>)((dynamic)this)._strRegistrationPhone).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strEmployerName)]
        [MapField(_str_strEmployerName)]
        public abstract String strEmployerName { get; set; }
        protected String strEmployerName_Original { get { return ((EditableValue<String>)((dynamic)this)._strEmployerName).OriginalValue; } }
        protected String strEmployerName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strEmployerName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strHomePhone)]
        [MapField(_str_strHomePhone)]
        public abstract String strHomePhone { get; set; }
        protected String strHomePhone_Original { get { return ((EditableValue<String>)((dynamic)this)._strHomePhone).OriginalValue; } }
        protected String strHomePhone_Previous { get { return ((EditableValue<String>)((dynamic)this)._strHomePhone).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strWorkPhone)]
        [MapField(_str_strWorkPhone)]
        public abstract String strWorkPhone { get; set; }
        protected String strWorkPhone_Original { get { return ((EditableValue<String>)((dynamic)this)._strWorkPhone).OriginalValue; } }
        protected String strWorkPhone_Previous { get { return ((EditableValue<String>)((dynamic)this)._strWorkPhone).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsPersonIDType)]
        [MapField(_str_idfsPersonIDType)]
        public abstract Int64? idfsPersonIDType { get; set; }
        protected Int64? idfsPersonIDType_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsPersonIDType).OriginalValue; } }
        protected Int64? idfsPersonIDType_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsPersonIDType).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strPersonID)]
        [MapField(_str_strPersonID)]
        public abstract String strPersonID { get; set; }
        protected String strPersonID_Original { get { return ((EditableValue<String>)((dynamic)this)._strPersonID).OriginalValue; } }
        protected String strPersonID_Previous { get { return ((EditableValue<String>)((dynamic)this)._strPersonID).PreviousValue; } }
                
        [LocalizedDisplayName("datPersonEnteredDate")]
        [MapField(_str_datEnteredDate)]
        public abstract DateTime? datEnteredDate { get; set; }
        protected DateTime? datEnteredDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datEnteredDate).OriginalValue; } }
        protected DateTime? datEnteredDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datEnteredDate).PreviousValue; } }
                
        [LocalizedDisplayName("datPersonModificationDate")]
        [MapField(_str_datModificationDate)]
        public abstract DateTime? datModificationDate { get; set; }
        protected DateTime? datModificationDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datModificationDate).OriginalValue; } }
        protected DateTime? datModificationDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datModificationDate).PreviousValue; } }
                
        [LocalizedDisplayName(_str_datModificationForArchiveDate)]
        [MapField(_str_datModificationForArchiveDate)]
        public abstract DateTime? datModificationForArchiveDate { get; set; }
        protected DateTime? datModificationForArchiveDate_Original { get { return ((EditableValue<DateTime?>)((dynamic)this)._datModificationForArchiveDate).OriginalValue; } }
        protected DateTime? datModificationForArchiveDate_Previous { get { return ((EditableValue<DateTime?>)((dynamic)this)._datModificationForArchiveDate).PreviousValue; } }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<Patient, object> _get_func;
            internal Action<Patient, string> _set_func;
            internal Action<Patient, Patient, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfHuman = "idfHuman";
        internal const string _str_idfRootHuman = "idfRootHuman";
        internal const string _str_idfCase = "idfCase";
        internal const string _str_intPatientAgeFromCase = "intPatientAgeFromCase";
        internal const string _str_idfsHumanAgeTypeFromCase = "idfsHumanAgeTypeFromCase";
        internal const string _str_idfsOccupationType = "idfsOccupationType";
        internal const string _str_idfsNationality = "idfsNationality";
        internal const string _str_idfsHumanGender = "idfsHumanGender";
        internal const string _str_idfCurrentResidenceAddress = "idfCurrentResidenceAddress";
        internal const string _str_idfEmployerAddress = "idfEmployerAddress";
        internal const string _str_idfRegistrationAddress = "idfRegistrationAddress";
        internal const string _str_datDateofBirth = "datDateofBirth";
        internal const string _str_datDateOfDeath = "datDateOfDeath";
        internal const string _str_strLastName = "strLastName";
        internal const string _str_strSecondName = "strSecondName";
        internal const string _str_strFirstName = "strFirstName";
        internal const string _str_strRegistrationPhone = "strRegistrationPhone";
        internal const string _str_strEmployerName = "strEmployerName";
        internal const string _str_strHomePhone = "strHomePhone";
        internal const string _str_strWorkPhone = "strWorkPhone";
        internal const string _str_idfsPersonIDType = "idfsPersonIDType";
        internal const string _str_strPersonID = "strPersonID";
        internal const string _str_datEnteredDate = "datEnteredDate";
        internal const string _str_datModificationDate = "datModificationDate";
        internal const string _str_datModificationForArchiveDate = "datModificationForArchiveDate";
        internal const string _str_bSearchMode = "bSearchMode";
        internal const string _str_bPINMode = "bPINMode";
        internal const string _str_bFirstCreated = "bFirstCreated";
        internal const string _str_strPinValid = "strPinValid";
        internal const string _str_idfContactedCasePerson = "idfContactedCasePerson";
        internal const string _str_idfsSearchMethod = "idfsSearchMethod";
        internal const string _str_idfsDocumentType = "idfsDocumentType";
        internal const string _str_strDocumentNumber = "strDocumentNumber";
        internal const string _str_datDocumentDate = "datDocumentDate";
        internal const string _str_HCase = "HCase";
        internal const string _str_strName = "strName";
        internal const string _str_IsReadOnlyParent = "IsReadOnlyParent";
        internal const string _str_strReadOnlyLastName = "strReadOnlyLastName";
        internal const string _str_strReadOnlyFirstName = "strReadOnlyFirstName";
        internal const string _str_strReadOnlySecondName = "strReadOnlySecondName";
        internal const string _str_strReadOnlyDateofBirth = "strReadOnlyDateofBirth";
        internal const string _str_strReadOnlyHumanGender = "strReadOnlyHumanGender";
        internal const string _str_strReadOnlyNationality = "strReadOnlyNationality";
        internal const string _str_strReadOnlyEmployerName = "strReadOnlyEmployerName";
        internal const string _str_IsRoot = "IsRoot";
        internal const string _str_strEnteredDate = "strEnteredDate";
        internal const string _str_strModificationDate = "strModificationDate";
        internal const string _str_IsFirstCreatedAndGGPin = "IsFirstCreatedAndGGPin";
        internal const string _str_IsGGPinChangedAndNotValidated = "IsGGPinChangedAndNotValidated";
        internal const string _str_OccupationType = "OccupationType";
        internal const string _str_Nationality = "Nationality";
        internal const string _str_Gender = "Gender";
        internal const string _str_PersonIDType = "PersonIDType";
        internal const string _str_HumanAgeType = "HumanAgeType";
        internal const string _str_SearchMethod = "SearchMethod";
        internal const string _str_DocumentType = "DocumentType";
        internal const string _str_CurrentResidenceAddress = "CurrentResidenceAddress";
        internal const string _str_EmployerAddress = "EmployerAddress";
        internal const string _str_RegistrationAddress = "RegistrationAddress";
        private static readonly field_info[] _field_infos =
        {
                  
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
              _name = _str_idfRootHuman, _formname = _str_idfRootHuman, _type = "Int64?",
              _get_func = o => o.idfRootHuman,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfRootHuman != newval) o.idfRootHuman = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfRootHuman != c.idfRootHuman || o.IsRIRPropChanged(_str_idfRootHuman, c)) 
                  m.Add(_str_idfRootHuman, o.ObjectIdent + _str_idfRootHuman, o.ObjectIdent2 + _str_idfRootHuman, o.ObjectIdent3 + _str_idfRootHuman, "Int64?", 
                    o.idfRootHuman == null ? "" : o.idfRootHuman.ToString(),                  
                  o.IsReadOnly(_str_idfRootHuman), o.IsInvisible(_str_idfRootHuman), o.IsRequired(_str_idfRootHuman)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfCase, _formname = _str_idfCase, _type = "Int64?",
              _get_func = o => o.idfCase,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfCase != newval) o.idfCase = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfCase != c.idfCase || o.IsRIRPropChanged(_str_idfCase, c)) 
                  m.Add(_str_idfCase, o.ObjectIdent + _str_idfCase, o.ObjectIdent2 + _str_idfCase, o.ObjectIdent3 + _str_idfCase, "Int64?", 
                    o.idfCase == null ? "" : o.idfCase.ToString(),                  
                  o.IsReadOnly(_str_idfCase), o.IsInvisible(_str_idfCase), o.IsRequired(_str_idfCase)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_intPatientAgeFromCase, _formname = _str_intPatientAgeFromCase, _type = "Int32?",
              _get_func = o => o.intPatientAgeFromCase,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt32Nullable(val); if (o.intPatientAgeFromCase != newval) o.intPatientAgeFromCase = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.intPatientAgeFromCase != c.intPatientAgeFromCase || o.IsRIRPropChanged(_str_intPatientAgeFromCase, c)) 
                  m.Add(_str_intPatientAgeFromCase, o.ObjectIdent + _str_intPatientAgeFromCase, o.ObjectIdent2 + _str_intPatientAgeFromCase, o.ObjectIdent3 + _str_intPatientAgeFromCase, "Int32?", 
                    o.intPatientAgeFromCase == null ? "" : o.intPatientAgeFromCase.ToString(),                  
                  o.IsReadOnly(_str_intPatientAgeFromCase), o.IsInvisible(_str_intPatientAgeFromCase), o.IsRequired(_str_intPatientAgeFromCase)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsHumanAgeTypeFromCase, _formname = _str_idfsHumanAgeTypeFromCase, _type = "Int64?",
              _get_func = o => o.idfsHumanAgeTypeFromCase,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfsHumanAgeTypeFromCase != newval) 
                  o.HumanAgeType = o.HumanAgeTypeLookup.FirstOrDefault(c => c.idfsBaseReference == newval);
                if (o.idfsHumanAgeTypeFromCase != newval) o.idfsHumanAgeTypeFromCase = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsHumanAgeTypeFromCase != c.idfsHumanAgeTypeFromCase || o.IsRIRPropChanged(_str_idfsHumanAgeTypeFromCase, c)) 
                  m.Add(_str_idfsHumanAgeTypeFromCase, o.ObjectIdent + _str_idfsHumanAgeTypeFromCase, o.ObjectIdent2 + _str_idfsHumanAgeTypeFromCase, o.ObjectIdent3 + _str_idfsHumanAgeTypeFromCase, "Int64?", 
                    o.idfsHumanAgeTypeFromCase == null ? "" : o.idfsHumanAgeTypeFromCase.ToString(),                  
                  o.IsReadOnly(_str_idfsHumanAgeTypeFromCase), o.IsInvisible(_str_idfsHumanAgeTypeFromCase), o.IsRequired(_str_idfsHumanAgeTypeFromCase)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsOccupationType, _formname = _str_idfsOccupationType, _type = "Int64?",
              _get_func = o => o.idfsOccupationType,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfsOccupationType != newval) 
                  o.OccupationType = o.OccupationTypeLookup.FirstOrDefault(c => c.idfsBaseReference == newval);
                if (o.idfsOccupationType != newval) o.idfsOccupationType = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsOccupationType != c.idfsOccupationType || o.IsRIRPropChanged(_str_idfsOccupationType, c)) 
                  m.Add(_str_idfsOccupationType, o.ObjectIdent + _str_idfsOccupationType, o.ObjectIdent2 + _str_idfsOccupationType, o.ObjectIdent3 + _str_idfsOccupationType, "Int64?", 
                    o.idfsOccupationType == null ? "" : o.idfsOccupationType.ToString(),                  
                  o.IsReadOnly(_str_idfsOccupationType), o.IsInvisible(_str_idfsOccupationType), o.IsRequired(_str_idfsOccupationType)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsNationality, _formname = _str_idfsNationality, _type = "Int64?",
              _get_func = o => o.idfsNationality,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfsNationality != newval) 
                  o.Nationality = o.NationalityLookup.FirstOrDefault(c => c.idfsBaseReference == newval);
                if (o.idfsNationality != newval) o.idfsNationality = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsNationality != c.idfsNationality || o.IsRIRPropChanged(_str_idfsNationality, c)) 
                  m.Add(_str_idfsNationality, o.ObjectIdent + _str_idfsNationality, o.ObjectIdent2 + _str_idfsNationality, o.ObjectIdent3 + _str_idfsNationality, "Int64?", 
                    o.idfsNationality == null ? "" : o.idfsNationality.ToString(),                  
                  o.IsReadOnly(_str_idfsNationality), o.IsInvisible(_str_idfsNationality), o.IsRequired(_str_idfsNationality)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsHumanGender, _formname = _str_idfsHumanGender, _type = "Int64?",
              _get_func = o => o.idfsHumanGender,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfsHumanGender != newval) 
                  o.Gender = o.GenderLookup.FirstOrDefault(c => c.idfsBaseReference == newval);
                if (o.idfsHumanGender != newval) o.idfsHumanGender = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsHumanGender != c.idfsHumanGender || o.IsRIRPropChanged(_str_idfsHumanGender, c)) 
                  m.Add(_str_idfsHumanGender, o.ObjectIdent + _str_idfsHumanGender, o.ObjectIdent2 + _str_idfsHumanGender, o.ObjectIdent3 + _str_idfsHumanGender, "Int64?", 
                    o.idfsHumanGender == null ? "" : o.idfsHumanGender.ToString(),                  
                  o.IsReadOnly(_str_idfsHumanGender), o.IsInvisible(_str_idfsHumanGender), o.IsRequired(_str_idfsHumanGender)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfCurrentResidenceAddress, _formname = _str_idfCurrentResidenceAddress, _type = "Int64?",
              _get_func = o => o.idfCurrentResidenceAddress,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfCurrentResidenceAddress != newval) o.idfCurrentResidenceAddress = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfCurrentResidenceAddress != c.idfCurrentResidenceAddress || o.IsRIRPropChanged(_str_idfCurrentResidenceAddress, c)) 
                  m.Add(_str_idfCurrentResidenceAddress, o.ObjectIdent + _str_idfCurrentResidenceAddress, o.ObjectIdent2 + _str_idfCurrentResidenceAddress, o.ObjectIdent3 + _str_idfCurrentResidenceAddress, "Int64?", 
                    o.idfCurrentResidenceAddress == null ? "" : o.idfCurrentResidenceAddress.ToString(),                  
                  o.IsReadOnly(_str_idfCurrentResidenceAddress), o.IsInvisible(_str_idfCurrentResidenceAddress), o.IsRequired(_str_idfCurrentResidenceAddress)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfEmployerAddress, _formname = _str_idfEmployerAddress, _type = "Int64?",
              _get_func = o => o.idfEmployerAddress,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfEmployerAddress != newval) o.idfEmployerAddress = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfEmployerAddress != c.idfEmployerAddress || o.IsRIRPropChanged(_str_idfEmployerAddress, c)) 
                  m.Add(_str_idfEmployerAddress, o.ObjectIdent + _str_idfEmployerAddress, o.ObjectIdent2 + _str_idfEmployerAddress, o.ObjectIdent3 + _str_idfEmployerAddress, "Int64?", 
                    o.idfEmployerAddress == null ? "" : o.idfEmployerAddress.ToString(),                  
                  o.IsReadOnly(_str_idfEmployerAddress), o.IsInvisible(_str_idfEmployerAddress), o.IsRequired(_str_idfEmployerAddress)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfRegistrationAddress, _formname = _str_idfRegistrationAddress, _type = "Int64?",
              _get_func = o => o.idfRegistrationAddress,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfRegistrationAddress != newval) o.idfRegistrationAddress = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfRegistrationAddress != c.idfRegistrationAddress || o.IsRIRPropChanged(_str_idfRegistrationAddress, c)) 
                  m.Add(_str_idfRegistrationAddress, o.ObjectIdent + _str_idfRegistrationAddress, o.ObjectIdent2 + _str_idfRegistrationAddress, o.ObjectIdent3 + _str_idfRegistrationAddress, "Int64?", 
                    o.idfRegistrationAddress == null ? "" : o.idfRegistrationAddress.ToString(),                  
                  o.IsReadOnly(_str_idfRegistrationAddress), o.IsInvisible(_str_idfRegistrationAddress), o.IsRequired(_str_idfRegistrationAddress)); 
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
              _name = _str_datDateOfDeath, _formname = _str_datDateOfDeath, _type = "DateTime?",
              _get_func = o => o.datDateOfDeath,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datDateOfDeath != newval) o.datDateOfDeath = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datDateOfDeath != c.datDateOfDeath || o.IsRIRPropChanged(_str_datDateOfDeath, c)) 
                  m.Add(_str_datDateOfDeath, o.ObjectIdent + _str_datDateOfDeath, o.ObjectIdent2 + _str_datDateOfDeath, o.ObjectIdent3 + _str_datDateOfDeath, "DateTime?", 
                    o.datDateOfDeath == null ? "" : o.datDateOfDeath.ToString(),                  
                  o.IsReadOnly(_str_datDateOfDeath), o.IsInvisible(_str_datDateOfDeath), o.IsRequired(_str_datDateOfDeath)); 
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
              _name = _str_strRegistrationPhone, _formname = _str_strRegistrationPhone, _type = "String",
              _get_func = o => o.strRegistrationPhone,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strRegistrationPhone != newval) o.strRegistrationPhone = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strRegistrationPhone != c.strRegistrationPhone || o.IsRIRPropChanged(_str_strRegistrationPhone, c)) 
                  m.Add(_str_strRegistrationPhone, o.ObjectIdent + _str_strRegistrationPhone, o.ObjectIdent2 + _str_strRegistrationPhone, o.ObjectIdent3 + _str_strRegistrationPhone, "String", 
                    o.strRegistrationPhone == null ? "" : o.strRegistrationPhone.ToString(),                  
                  o.IsReadOnly(_str_strRegistrationPhone), o.IsInvisible(_str_strRegistrationPhone), o.IsRequired(_str_strRegistrationPhone)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strEmployerName, _formname = _str_strEmployerName, _type = "String",
              _get_func = o => o.strEmployerName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strEmployerName != newval) o.strEmployerName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strEmployerName != c.strEmployerName || o.IsRIRPropChanged(_str_strEmployerName, c)) 
                  m.Add(_str_strEmployerName, o.ObjectIdent + _str_strEmployerName, o.ObjectIdent2 + _str_strEmployerName, o.ObjectIdent3 + _str_strEmployerName, "String", 
                    o.strEmployerName == null ? "" : o.strEmployerName.ToString(),                  
                  o.IsReadOnly(_str_strEmployerName), o.IsInvisible(_str_strEmployerName), o.IsRequired(_str_strEmployerName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strHomePhone, _formname = _str_strHomePhone, _type = "String",
              _get_func = o => o.strHomePhone,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strHomePhone != newval) o.strHomePhone = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strHomePhone != c.strHomePhone || o.IsRIRPropChanged(_str_strHomePhone, c)) 
                  m.Add(_str_strHomePhone, o.ObjectIdent + _str_strHomePhone, o.ObjectIdent2 + _str_strHomePhone, o.ObjectIdent3 + _str_strHomePhone, "String", 
                    o.strHomePhone == null ? "" : o.strHomePhone.ToString(),                  
                  o.IsReadOnly(_str_strHomePhone), o.IsInvisible(_str_strHomePhone), o.IsRequired(_str_strHomePhone)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strWorkPhone, _formname = _str_strWorkPhone, _type = "String",
              _get_func = o => o.strWorkPhone,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strWorkPhone != newval) o.strWorkPhone = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strWorkPhone != c.strWorkPhone || o.IsRIRPropChanged(_str_strWorkPhone, c)) 
                  m.Add(_str_strWorkPhone, o.ObjectIdent + _str_strWorkPhone, o.ObjectIdent2 + _str_strWorkPhone, o.ObjectIdent3 + _str_strWorkPhone, "String", 
                    o.strWorkPhone == null ? "" : o.strWorkPhone.ToString(),                  
                  o.IsReadOnly(_str_strWorkPhone), o.IsInvisible(_str_strWorkPhone), o.IsRequired(_str_strWorkPhone)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsPersonIDType, _formname = _str_idfsPersonIDType, _type = "Int64?",
              _get_func = o => o.idfsPersonIDType,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfsPersonIDType != newval) 
                  o.PersonIDType = o.PersonIDTypeLookup.FirstOrDefault(c => c.idfsBaseReference == newval);
                if (o.idfsPersonIDType != newval) o.idfsPersonIDType = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsPersonIDType != c.idfsPersonIDType || o.IsRIRPropChanged(_str_idfsPersonIDType, c)) 
                  m.Add(_str_idfsPersonIDType, o.ObjectIdent + _str_idfsPersonIDType, o.ObjectIdent2 + _str_idfsPersonIDType, o.ObjectIdent3 + _str_idfsPersonIDType, "Int64?", 
                    o.idfsPersonIDType == null ? "" : o.idfsPersonIDType.ToString(),                  
                  o.IsReadOnly(_str_idfsPersonIDType), o.IsInvisible(_str_idfsPersonIDType), o.IsRequired(_str_idfsPersonIDType)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strPersonID, _formname = _str_strPersonID, _type = "String",
              _get_func = o => o.strPersonID,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strPersonID != newval) o.strPersonID = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strPersonID != c.strPersonID || o.IsRIRPropChanged(_str_strPersonID, c)) 
                  m.Add(_str_strPersonID, o.ObjectIdent + _str_strPersonID, o.ObjectIdent2 + _str_strPersonID, o.ObjectIdent3 + _str_strPersonID, "String", 
                    o.strPersonID == null ? "" : o.strPersonID.ToString(),                  
                  o.IsReadOnly(_str_strPersonID), o.IsInvisible(_str_strPersonID), o.IsRequired(_str_strPersonID)); 
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
              _name = _str_datModificationDate, _formname = _str_datModificationDate, _type = "DateTime?",
              _get_func = o => o.datModificationDate,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datModificationDate != newval) o.datModificationDate = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datModificationDate != c.datModificationDate || o.IsRIRPropChanged(_str_datModificationDate, c)) 
                  m.Add(_str_datModificationDate, o.ObjectIdent + _str_datModificationDate, o.ObjectIdent2 + _str_datModificationDate, o.ObjectIdent3 + _str_datModificationDate, "DateTime?", 
                    o.datModificationDate == null ? "" : o.datModificationDate.ToString(),                  
                  o.IsReadOnly(_str_datModificationDate), o.IsInvisible(_str_datModificationDate), o.IsRequired(_str_datModificationDate)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_datModificationForArchiveDate, _formname = _str_datModificationForArchiveDate, _type = "DateTime?",
              _get_func = o => o.datModificationForArchiveDate,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datModificationForArchiveDate != newval) o.datModificationForArchiveDate = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.datModificationForArchiveDate != c.datModificationForArchiveDate || o.IsRIRPropChanged(_str_datModificationForArchiveDate, c)) 
                  m.Add(_str_datModificationForArchiveDate, o.ObjectIdent + _str_datModificationForArchiveDate, o.ObjectIdent2 + _str_datModificationForArchiveDate, o.ObjectIdent3 + _str_datModificationForArchiveDate, "DateTime?", 
                    o.datModificationForArchiveDate == null ? "" : o.datModificationForArchiveDate.ToString(),                  
                  o.IsReadOnly(_str_datModificationForArchiveDate), o.IsInvisible(_str_datModificationForArchiveDate), o.IsRequired(_str_datModificationForArchiveDate)); 
                  }
              }, 
        
            new field_info {
              _name = _str_bSearchMode, _formname = _str_bSearchMode, _type = "bool",
              _get_func = o => o.bSearchMode,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBoolean(val); if (o.bSearchMode != newval) o.bSearchMode = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.bSearchMode != c.bSearchMode || o.IsRIRPropChanged(_str_bSearchMode, c)) {
                  m.Add(_str_bSearchMode, o.ObjectIdent + _str_bSearchMode, o.ObjectIdent2 + _str_bSearchMode, o.ObjectIdent3 + _str_bSearchMode,  "bool", 
                    o.bSearchMode == null ? "" : o.bSearchMode.ToString(),                  
                    o.IsReadOnly(_str_bSearchMode), o.IsInvisible(_str_bSearchMode), o.IsRequired(_str_bSearchMode));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_bPINMode, _formname = _str_bPINMode, _type = "bool",
              _get_func = o => o.bPINMode,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBoolean(val); if (o.bPINMode != newval) o.bPINMode = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.bPINMode != c.bPINMode || o.IsRIRPropChanged(_str_bPINMode, c)) {
                  m.Add(_str_bPINMode, o.ObjectIdent + _str_bPINMode, o.ObjectIdent2 + _str_bPINMode, o.ObjectIdent3 + _str_bPINMode,  "bool", 
                    o.bPINMode == null ? "" : o.bPINMode.ToString(),                  
                    o.IsReadOnly(_str_bPINMode), o.IsInvisible(_str_bPINMode), o.IsRequired(_str_bPINMode));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_bFirstCreated, _formname = _str_bFirstCreated, _type = "bool",
              _get_func = o => o.bFirstCreated,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBoolean(val); if (o.bFirstCreated != newval) o.bFirstCreated = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.bFirstCreated != c.bFirstCreated || o.IsRIRPropChanged(_str_bFirstCreated, c)) {
                  m.Add(_str_bFirstCreated, o.ObjectIdent + _str_bFirstCreated, o.ObjectIdent2 + _str_bFirstCreated, o.ObjectIdent3 + _str_bFirstCreated,  "bool", 
                    o.bFirstCreated == null ? "" : o.bFirstCreated.ToString(),                  
                    o.IsReadOnly(_str_bFirstCreated), o.IsInvisible(_str_bFirstCreated), o.IsRequired(_str_bFirstCreated));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_strPinValid, _formname = _str_strPinValid, _type = "string",
              _get_func = o => o.strPinValid,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strPinValid != newval) o.strPinValid = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.strPinValid != c.strPinValid || o.IsRIRPropChanged(_str_strPinValid, c)) {
                  m.Add(_str_strPinValid, o.ObjectIdent + _str_strPinValid, o.ObjectIdent2 + _str_strPinValid, o.ObjectIdent3 + _str_strPinValid,  "string", 
                    o.strPinValid == null ? "" : o.strPinValid.ToString(),                  
                    o.IsReadOnly(_str_strPinValid), o.IsInvisible(_str_strPinValid), o.IsRequired(_str_strPinValid));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_idfContactedCasePerson, _formname = _str_idfContactedCasePerson, _type = "long",
              _get_func = o => o.idfContactedCasePerson,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfContactedCasePerson != newval) o.idfContactedCasePerson = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.idfContactedCasePerson != c.idfContactedCasePerson || o.IsRIRPropChanged(_str_idfContactedCasePerson, c)) {
                  m.Add(_str_idfContactedCasePerson, o.ObjectIdent + _str_idfContactedCasePerson, o.ObjectIdent2 + _str_idfContactedCasePerson, o.ObjectIdent3 + _str_idfContactedCasePerson,  "long", 
                    o.idfContactedCasePerson == null ? "" : o.idfContactedCasePerson.ToString(),                  
                    o.IsReadOnly(_str_idfContactedCasePerson), o.IsInvisible(_str_idfContactedCasePerson), o.IsRequired(_str_idfContactedCasePerson));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_idfsSearchMethod, _formname = _str_idfsSearchMethod, _type = "long?",
              _get_func = o => o.idfsSearchMethod,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsSearchMethod != newval) o.idfsSearchMethod = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.idfsSearchMethod != c.idfsSearchMethod || o.IsRIRPropChanged(_str_idfsSearchMethod, c)) {
                  m.Add(_str_idfsSearchMethod, o.ObjectIdent + _str_idfsSearchMethod, o.ObjectIdent2 + _str_idfsSearchMethod, o.ObjectIdent3 + _str_idfsSearchMethod,  "long?", 
                    o.idfsSearchMethod == null ? "" : o.idfsSearchMethod.ToString(),                  
                    o.IsReadOnly(_str_idfsSearchMethod), o.IsInvisible(_str_idfsSearchMethod), o.IsRequired(_str_idfsSearchMethod));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_idfsDocumentType, _formname = _str_idfsDocumentType, _type = "long?",
              _get_func = o => o.idfsDocumentType,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsDocumentType != newval) o.idfsDocumentType = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.idfsDocumentType != c.idfsDocumentType || o.IsRIRPropChanged(_str_idfsDocumentType, c)) {
                  m.Add(_str_idfsDocumentType, o.ObjectIdent + _str_idfsDocumentType, o.ObjectIdent2 + _str_idfsDocumentType, o.ObjectIdent3 + _str_idfsDocumentType,  "long?", 
                    o.idfsDocumentType == null ? "" : o.idfsDocumentType.ToString(),                  
                    o.IsReadOnly(_str_idfsDocumentType), o.IsInvisible(_str_idfsDocumentType), o.IsRequired(_str_idfsDocumentType));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_strDocumentNumber, _formname = _str_strDocumentNumber, _type = "string",
              _get_func = o => o.strDocumentNumber,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strDocumentNumber != newval) o.strDocumentNumber = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.strDocumentNumber != c.strDocumentNumber || o.IsRIRPropChanged(_str_strDocumentNumber, c)) {
                  m.Add(_str_strDocumentNumber, o.ObjectIdent + _str_strDocumentNumber, o.ObjectIdent2 + _str_strDocumentNumber, o.ObjectIdent3 + _str_strDocumentNumber,  "string", 
                    o.strDocumentNumber == null ? "" : o.strDocumentNumber.ToString(),                  
                    o.IsReadOnly(_str_strDocumentNumber), o.IsInvisible(_str_strDocumentNumber), o.IsRequired(_str_strDocumentNumber));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_datDocumentDate, _formname = _str_datDocumentDate, _type = "DateTime?",
              _get_func = o => o.datDocumentDate,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseDateTimeNullable(val); if (o.datDocumentDate != newval) o.datDocumentDate = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.datDocumentDate != c.datDocumentDate || o.IsRIRPropChanged(_str_datDocumentDate, c)) {
                  m.Add(_str_datDocumentDate, o.ObjectIdent + _str_datDocumentDate, o.ObjectIdent2 + _str_datDocumentDate, o.ObjectIdent3 + _str_datDocumentDate,  "DateTime?", 
                    o.datDocumentDate == null ? "" : o.datDocumentDate.ToString(),                  
                    o.IsReadOnly(_str_datDocumentDate), o.IsInvisible(_str_datDocumentDate), o.IsRequired(_str_datDocumentDate));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_HCase, _formname = _str_HCase, _type = "HumanCase",
              _get_func = o => o.HCase,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                }
              }, 
        
            new field_info {
              _name = _str_strName, _formname = _str_strName, _type = "string",
              _get_func = o => o.strName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strName != c.strName || o.IsRIRPropChanged(_str_strName, c)) {
                  m.Add(_str_strName, o.ObjectIdent + _str_strName, o.ObjectIdent2 + _str_strName, o.ObjectIdent3 + _str_strName, "string", o.strName == null ? "" : o.strName.ToString(), o.IsReadOnly(_str_strName), o.IsInvisible(_str_strName), o.IsRequired(_str_strName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_IsReadOnlyParent, _formname = _str_IsReadOnlyParent, _type = "bool",
              _get_func = o => o.IsReadOnlyParent,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.IsReadOnlyParent != c.IsReadOnlyParent || o.IsRIRPropChanged(_str_IsReadOnlyParent, c)) {
                  m.Add(_str_IsReadOnlyParent, o.ObjectIdent + _str_IsReadOnlyParent, o.ObjectIdent2 + _str_IsReadOnlyParent, o.ObjectIdent3 + _str_IsReadOnlyParent, "bool", o.IsReadOnlyParent == null ? "" : o.IsReadOnlyParent.ToString(), o.IsReadOnly(_str_IsReadOnlyParent), o.IsInvisible(_str_IsReadOnlyParent), o.IsRequired(_str_IsReadOnlyParent));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlyLastName, _formname = _str_strReadOnlyLastName, _type = "string",
              _get_func = o => o.strReadOnlyLastName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlyLastName != c.strReadOnlyLastName || o.IsRIRPropChanged(_str_strReadOnlyLastName, c)) {
                  m.Add(_str_strReadOnlyLastName, o.ObjectIdent + _str_strReadOnlyLastName, o.ObjectIdent2 + _str_strReadOnlyLastName, o.ObjectIdent3 + _str_strReadOnlyLastName, "string", o.strReadOnlyLastName == null ? "" : o.strReadOnlyLastName.ToString(), o.IsReadOnly(_str_strReadOnlyLastName), o.IsInvisible(_str_strReadOnlyLastName), o.IsRequired(_str_strReadOnlyLastName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlyFirstName, _formname = _str_strReadOnlyFirstName, _type = "string",
              _get_func = o => o.strReadOnlyFirstName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlyFirstName != c.strReadOnlyFirstName || o.IsRIRPropChanged(_str_strReadOnlyFirstName, c)) {
                  m.Add(_str_strReadOnlyFirstName, o.ObjectIdent + _str_strReadOnlyFirstName, o.ObjectIdent2 + _str_strReadOnlyFirstName, o.ObjectIdent3 + _str_strReadOnlyFirstName, "string", o.strReadOnlyFirstName == null ? "" : o.strReadOnlyFirstName.ToString(), o.IsReadOnly(_str_strReadOnlyFirstName), o.IsInvisible(_str_strReadOnlyFirstName), o.IsRequired(_str_strReadOnlyFirstName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlySecondName, _formname = _str_strReadOnlySecondName, _type = "string",
              _get_func = o => o.strReadOnlySecondName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlySecondName != c.strReadOnlySecondName || o.IsRIRPropChanged(_str_strReadOnlySecondName, c)) {
                  m.Add(_str_strReadOnlySecondName, o.ObjectIdent + _str_strReadOnlySecondName, o.ObjectIdent2 + _str_strReadOnlySecondName, o.ObjectIdent3 + _str_strReadOnlySecondName, "string", o.strReadOnlySecondName == null ? "" : o.strReadOnlySecondName.ToString(), o.IsReadOnly(_str_strReadOnlySecondName), o.IsInvisible(_str_strReadOnlySecondName), o.IsRequired(_str_strReadOnlySecondName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlyDateofBirth, _formname = _str_strReadOnlyDateofBirth, _type = "string",
              _get_func = o => o.strReadOnlyDateofBirth,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlyDateofBirth != c.strReadOnlyDateofBirth || o.IsRIRPropChanged(_str_strReadOnlyDateofBirth, c)) {
                  m.Add(_str_strReadOnlyDateofBirth, o.ObjectIdent + _str_strReadOnlyDateofBirth, o.ObjectIdent2 + _str_strReadOnlyDateofBirth, o.ObjectIdent3 + _str_strReadOnlyDateofBirth, "string", o.strReadOnlyDateofBirth == null ? "" : o.strReadOnlyDateofBirth.ToString(), o.IsReadOnly(_str_strReadOnlyDateofBirth), o.IsInvisible(_str_strReadOnlyDateofBirth), o.IsRequired(_str_strReadOnlyDateofBirth));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlyHumanGender, _formname = _str_strReadOnlyHumanGender, _type = "string",
              _get_func = o => o.strReadOnlyHumanGender,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlyHumanGender != c.strReadOnlyHumanGender || o.IsRIRPropChanged(_str_strReadOnlyHumanGender, c)) {
                  m.Add(_str_strReadOnlyHumanGender, o.ObjectIdent + _str_strReadOnlyHumanGender, o.ObjectIdent2 + _str_strReadOnlyHumanGender, o.ObjectIdent3 + _str_strReadOnlyHumanGender, "string", o.strReadOnlyHumanGender == null ? "" : o.strReadOnlyHumanGender.ToString(), o.IsReadOnly(_str_strReadOnlyHumanGender), o.IsInvisible(_str_strReadOnlyHumanGender), o.IsRequired(_str_strReadOnlyHumanGender));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlyNationality, _formname = _str_strReadOnlyNationality, _type = "string",
              _get_func = o => o.strReadOnlyNationality,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlyNationality != c.strReadOnlyNationality || o.IsRIRPropChanged(_str_strReadOnlyNationality, c)) {
                  m.Add(_str_strReadOnlyNationality, o.ObjectIdent + _str_strReadOnlyNationality, o.ObjectIdent2 + _str_strReadOnlyNationality, o.ObjectIdent3 + _str_strReadOnlyNationality, "string", o.strReadOnlyNationality == null ? "" : o.strReadOnlyNationality.ToString(), o.IsReadOnly(_str_strReadOnlyNationality), o.IsInvisible(_str_strReadOnlyNationality), o.IsRequired(_str_strReadOnlyNationality));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strReadOnlyEmployerName, _formname = _str_strReadOnlyEmployerName, _type = "string",
              _get_func = o => o.strReadOnlyEmployerName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strReadOnlyEmployerName != c.strReadOnlyEmployerName || o.IsRIRPropChanged(_str_strReadOnlyEmployerName, c)) {
                  m.Add(_str_strReadOnlyEmployerName, o.ObjectIdent + _str_strReadOnlyEmployerName, o.ObjectIdent2 + _str_strReadOnlyEmployerName, o.ObjectIdent3 + _str_strReadOnlyEmployerName, "string", o.strReadOnlyEmployerName == null ? "" : o.strReadOnlyEmployerName.ToString(), o.IsReadOnly(_str_strReadOnlyEmployerName), o.IsInvisible(_str_strReadOnlyEmployerName), o.IsRequired(_str_strReadOnlyEmployerName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_IsRoot, _formname = _str_IsRoot, _type = "bool",
              _get_func = o => o.IsRoot,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.IsRoot != c.IsRoot || o.IsRIRPropChanged(_str_IsRoot, c)) {
                  m.Add(_str_IsRoot, o.ObjectIdent + _str_IsRoot, o.ObjectIdent2 + _str_IsRoot, o.ObjectIdent3 + _str_IsRoot, "bool", o.IsRoot == null ? "" : o.IsRoot.ToString(), o.IsReadOnly(_str_IsRoot), o.IsInvisible(_str_IsRoot), o.IsRequired(_str_IsRoot));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strEnteredDate, _formname = _str_strEnteredDate, _type = "string",
              _get_func = o => o.strEnteredDate,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strEnteredDate != c.strEnteredDate || o.IsRIRPropChanged(_str_strEnteredDate, c)) {
                  m.Add(_str_strEnteredDate, o.ObjectIdent + _str_strEnteredDate, o.ObjectIdent2 + _str_strEnteredDate, o.ObjectIdent3 + _str_strEnteredDate, "string", o.strEnteredDate == null ? "" : o.strEnteredDate.ToString(), o.IsReadOnly(_str_strEnteredDate), o.IsInvisible(_str_strEnteredDate), o.IsRequired(_str_strEnteredDate));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strModificationDate, _formname = _str_strModificationDate, _type = "string",
              _get_func = o => o.strModificationDate,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strModificationDate != c.strModificationDate || o.IsRIRPropChanged(_str_strModificationDate, c)) {
                  m.Add(_str_strModificationDate, o.ObjectIdent + _str_strModificationDate, o.ObjectIdent2 + _str_strModificationDate, o.ObjectIdent3 + _str_strModificationDate, "string", o.strModificationDate == null ? "" : o.strModificationDate.ToString(), o.IsReadOnly(_str_strModificationDate), o.IsInvisible(_str_strModificationDate), o.IsRequired(_str_strModificationDate));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_IsFirstCreatedAndGGPin, _formname = _str_IsFirstCreatedAndGGPin, _type = "bool",
              _get_func = o => o.IsFirstCreatedAndGGPin,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.IsFirstCreatedAndGGPin != c.IsFirstCreatedAndGGPin || o.IsRIRPropChanged(_str_IsFirstCreatedAndGGPin, c)) {
                  m.Add(_str_IsFirstCreatedAndGGPin, o.ObjectIdent + _str_IsFirstCreatedAndGGPin, o.ObjectIdent2 + _str_IsFirstCreatedAndGGPin, o.ObjectIdent3 + _str_IsFirstCreatedAndGGPin, "bool", o.IsFirstCreatedAndGGPin == null ? "" : o.IsFirstCreatedAndGGPin.ToString(), o.IsReadOnly(_str_IsFirstCreatedAndGGPin), o.IsInvisible(_str_IsFirstCreatedAndGGPin), o.IsRequired(_str_IsFirstCreatedAndGGPin));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_IsGGPinChangedAndNotValidated, _formname = _str_IsGGPinChangedAndNotValidated, _type = "bool",
              _get_func = o => o.IsGGPinChangedAndNotValidated,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.IsGGPinChangedAndNotValidated != c.IsGGPinChangedAndNotValidated || o.IsRIRPropChanged(_str_IsGGPinChangedAndNotValidated, c)) {
                  m.Add(_str_IsGGPinChangedAndNotValidated, o.ObjectIdent + _str_IsGGPinChangedAndNotValidated, o.ObjectIdent2 + _str_IsGGPinChangedAndNotValidated, o.ObjectIdent3 + _str_IsGGPinChangedAndNotValidated, "bool", o.IsGGPinChangedAndNotValidated == null ? "" : o.IsGGPinChangedAndNotValidated.ToString(), o.IsReadOnly(_str_IsGGPinChangedAndNotValidated), o.IsInvisible(_str_IsGGPinChangedAndNotValidated), o.IsRequired(_str_IsGGPinChangedAndNotValidated));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_OccupationType, _formname = _str_OccupationType, _type = "Lookup",
              _get_func = o => { if (o.OccupationType == null) return null; return o.OccupationType.idfsBaseReference; },
              _set_func = (o, val) => { o.OccupationType = o.OccupationTypeLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_OccupationType, c);
                if (o.idfsOccupationType != c.idfsOccupationType || o.IsRIRPropChanged(_str_OccupationType, c) || bChangeLookupContent) {
                  m.Add(_str_OccupationType, o.ObjectIdent + _str_OccupationType, o.ObjectIdent2 + _str_OccupationType, o.ObjectIdent3 + _str_OccupationType, "Lookup", o.idfsOccupationType == null ? "" : o.idfsOccupationType.ToString(), o.IsReadOnly(_str_OccupationType), o.IsInvisible(_str_OccupationType), o.IsRequired(_str_OccupationType),
                  bChangeLookupContent ? o.OccupationTypeLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_OccupationType + "Lookup", _formname = _str_OccupationType + "Lookup", _type = "LookupContent",
              _get_func = o => o.OccupationTypeLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_Nationality, _formname = _str_Nationality, _type = "Lookup",
              _get_func = o => { if (o.Nationality == null) return null; return o.Nationality.idfsBaseReference; },
              _set_func = (o, val) => { o.Nationality = o.NationalityLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_Nationality, c);
                if (o.idfsNationality != c.idfsNationality || o.IsRIRPropChanged(_str_Nationality, c) || bChangeLookupContent) {
                  m.Add(_str_Nationality, o.ObjectIdent + _str_Nationality, o.ObjectIdent2 + _str_Nationality, o.ObjectIdent3 + _str_Nationality, "Lookup", o.idfsNationality == null ? "" : o.idfsNationality.ToString(), o.IsReadOnly(_str_Nationality), o.IsInvisible(_str_Nationality), o.IsRequired(_str_Nationality),
                  bChangeLookupContent ? o.NationalityLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_Nationality + "Lookup", _formname = _str_Nationality + "Lookup", _type = "LookupContent",
              _get_func = o => o.NationalityLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_Gender, _formname = _str_Gender, _type = "Lookup",
              _get_func = o => { if (o.Gender == null) return null; return o.Gender.idfsBaseReference; },
              _set_func = (o, val) => { o.Gender = o.GenderLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_Gender, c);
                if (o.idfsHumanGender != c.idfsHumanGender || o.IsRIRPropChanged(_str_Gender, c) || bChangeLookupContent) {
                  m.Add(_str_Gender, o.ObjectIdent + _str_Gender, o.ObjectIdent2 + _str_Gender, o.ObjectIdent3 + _str_Gender, "Lookup", o.idfsHumanGender == null ? "" : o.idfsHumanGender.ToString(), o.IsReadOnly(_str_Gender), o.IsInvisible(_str_Gender), o.IsRequired(_str_Gender),
                  bChangeLookupContent ? o.GenderLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_Gender + "Lookup", _formname = _str_Gender + "Lookup", _type = "LookupContent",
              _get_func = o => o.GenderLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_PersonIDType, _formname = _str_PersonIDType, _type = "Lookup",
              _get_func = o => { if (o.PersonIDType == null) return null; return o.PersonIDType.idfsBaseReference; },
              _set_func = (o, val) => { o.PersonIDType = o.PersonIDTypeLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_PersonIDType, c);
                if (o.idfsPersonIDType != c.idfsPersonIDType || o.IsRIRPropChanged(_str_PersonIDType, c) || bChangeLookupContent) {
                  m.Add(_str_PersonIDType, o.ObjectIdent + _str_PersonIDType, o.ObjectIdent2 + _str_PersonIDType, o.ObjectIdent3 + _str_PersonIDType, "Lookup", o.idfsPersonIDType == null ? "" : o.idfsPersonIDType.ToString(), o.IsReadOnly(_str_PersonIDType), o.IsInvisible(_str_PersonIDType), o.IsRequired(_str_PersonIDType),
                  bChangeLookupContent ? o.PersonIDTypeLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_PersonIDType + "Lookup", _formname = _str_PersonIDType + "Lookup", _type = "LookupContent",
              _get_func = o => o.PersonIDTypeLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_HumanAgeType, _formname = _str_HumanAgeType, _type = "Lookup",
              _get_func = o => { if (o.HumanAgeType == null) return null; return o.HumanAgeType.idfsBaseReference; },
              _set_func = (o, val) => { o.HumanAgeType = o.HumanAgeTypeLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_HumanAgeType, c);
                if (o.idfsHumanAgeTypeFromCase != c.idfsHumanAgeTypeFromCase || o.IsRIRPropChanged(_str_HumanAgeType, c) || bChangeLookupContent) {
                  m.Add(_str_HumanAgeType, o.ObjectIdent + _str_HumanAgeType, o.ObjectIdent2 + _str_HumanAgeType, o.ObjectIdent3 + _str_HumanAgeType, "Lookup", o.idfsHumanAgeTypeFromCase == null ? "" : o.idfsHumanAgeTypeFromCase.ToString(), o.IsReadOnly(_str_HumanAgeType), o.IsInvisible(_str_HumanAgeType), o.IsRequired(_str_HumanAgeType),
                  bChangeLookupContent ? o.HumanAgeTypeLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_HumanAgeType + "Lookup", _formname = _str_HumanAgeType + "Lookup", _type = "LookupContent",
              _get_func = o => o.HumanAgeTypeLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_SearchMethod, _formname = _str_SearchMethod, _type = "Lookup",
              _get_func = o => { if (o.SearchMethod == null) return null; return o.SearchMethod.idfsBaseReference; },
              _set_func = (o, val) => { o.SearchMethod = o.SearchMethodLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_SearchMethod, c);
                if (o.idfsSearchMethod != c.idfsSearchMethod || o.IsRIRPropChanged(_str_SearchMethod, c) || bChangeLookupContent) {
                  m.Add(_str_SearchMethod, o.ObjectIdent + _str_SearchMethod, o.ObjectIdent2 + _str_SearchMethod, o.ObjectIdent3 + _str_SearchMethod, "Lookup", o.idfsSearchMethod == null ? "" : o.idfsSearchMethod.ToString(), o.IsReadOnly(_str_SearchMethod), o.IsInvisible(_str_SearchMethod), o.IsRequired(_str_SearchMethod),
                  bChangeLookupContent ? o.SearchMethodLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_SearchMethod + "Lookup", _formname = _str_SearchMethod + "Lookup", _type = "LookupContent",
              _get_func = o => o.SearchMethodLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_DocumentType, _formname = _str_DocumentType, _type = "Lookup",
              _get_func = o => { if (o.DocumentType == null) return null; return o.DocumentType.idfsBaseReference; },
              _set_func = (o, val) => { o.DocumentType = o.DocumentTypeLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_DocumentType, c);
                if (o.idfsDocumentType != c.idfsDocumentType || o.IsRIRPropChanged(_str_DocumentType, c) || bChangeLookupContent) {
                  m.Add(_str_DocumentType, o.ObjectIdent + _str_DocumentType, o.ObjectIdent2 + _str_DocumentType, o.ObjectIdent3 + _str_DocumentType, "Lookup", o.idfsDocumentType == null ? "" : o.idfsDocumentType.ToString(), o.IsReadOnly(_str_DocumentType), o.IsInvisible(_str_DocumentType), o.IsRequired(_str_DocumentType),
                  bChangeLookupContent ? o.DocumentTypeLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_DocumentType + "Lookup", _formname = _str_DocumentType + "Lookup", _type = "LookupContent",
              _get_func = o => o.DocumentTypeLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_CurrentResidenceAddress, _formname = _str_CurrentResidenceAddress, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.CurrentResidenceAddress != null) o.CurrentResidenceAddress._compare(c.CurrentResidenceAddress, m); }
              }, 
        
            new field_info {
              _name = _str_EmployerAddress, _formname = _str_EmployerAddress, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.EmployerAddress != null) o.EmployerAddress._compare(c.EmployerAddress, m); }
              }, 
        
            new field_info {
              _name = _str_RegistrationAddress, _formname = _str_RegistrationAddress, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.RegistrationAddress != null) o.RegistrationAddress._compare(c.RegistrationAddress, m); }
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
            Patient obj = (Patient)o;
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                Accessor.Instance(null)._LoadLookups(manager, obj);
                foreach (var i in _field_infos)
                    if (i != null && i._compare_func != null) i._compare_func(this, obj, ret, manager);
            }
            return ret;
        }
        #endregion
    
        [LocalizedDisplayName(_str_CurrentResidenceAddress)]
        [Relation(typeof(Address), eidss.model.Schema.Address._str_idfGeoLocation, _str_idfCurrentResidenceAddress)]
        public Address CurrentResidenceAddress
        {
            get 
            {   
                return _CurrentResidenceAddress; 
            }
            set 
            {   
                _CurrentResidenceAddress = value;
                if (_CurrentResidenceAddress != null) 
                { 
                    _CurrentResidenceAddress.m_ObjectName = _str_CurrentResidenceAddress;
                    _CurrentResidenceAddress.Parent = this;
                }
                idfCurrentResidenceAddress = _CurrentResidenceAddress == null 
                        ? new Int64?()
                        : _CurrentResidenceAddress.idfGeoLocation;
                
            }
        }
        protected Address _CurrentResidenceAddress;
                    
        [LocalizedDisplayName(_str_EmployerAddress)]
        [Relation(typeof(Address), eidss.model.Schema.Address._str_idfGeoLocation, _str_idfEmployerAddress)]
        public Address EmployerAddress
        {
            get 
            {   
                return _EmployerAddress; 
            }
            set 
            {   
                _EmployerAddress = value;
                if (_EmployerAddress != null) 
                { 
                    _EmployerAddress.m_ObjectName = _str_EmployerAddress;
                    _EmployerAddress.Parent = this;
                }
                idfEmployerAddress = _EmployerAddress == null 
                        ? new Int64?()
                        : _EmployerAddress.idfGeoLocation;
                
            }
        }
        protected Address _EmployerAddress;
                    
        [LocalizedDisplayName(_str_RegistrationAddress)]
        [Relation(typeof(Address), eidss.model.Schema.Address._str_idfGeoLocation, _str_idfRegistrationAddress)]
        public Address RegistrationAddress
        {
            get 
            {   
                return _RegistrationAddress; 
            }
            set 
            {   
                _RegistrationAddress = value;
                if (_RegistrationAddress != null) 
                { 
                    _RegistrationAddress.m_ObjectName = _str_RegistrationAddress;
                    _RegistrationAddress.Parent = this;
                }
                idfRegistrationAddress = _RegistrationAddress == null 
                        ? new Int64?()
                        : _RegistrationAddress.idfGeoLocation;
                
            }
        }
        protected Address _RegistrationAddress;
                    
        [LocalizedDisplayName(_str_OccupationType)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsOccupationType)]
        public BaseReference OccupationType
        {
            get { return _OccupationType == null ? null : ((long)_OccupationType.Key == 0 ? null : _OccupationType); }
            set 
            { 
                var oldVal = _OccupationType;
                _OccupationType = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_OccupationType != oldVal)
                {
                    if (idfsOccupationType != (_OccupationType == null
                            ? new Int64?()
                            : (Int64?)_OccupationType.idfsBaseReference))
                        idfsOccupationType = _OccupationType == null 
                            ? new Int64?()
                            : (Int64?)_OccupationType.idfsBaseReference; 
                    OnPropertyChanged(_str_OccupationType); 
                }
            }
        }
        private BaseReference _OccupationType;

        
        public BaseReferenceList OccupationTypeLookup
        {
            get { return _OccupationTypeLookup; }
        }
        private BaseReferenceList _OccupationTypeLookup = new BaseReferenceList("rftOccupationType");
            
        [LocalizedDisplayName(_str_Nationality)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsNationality)]
        public BaseReference Nationality
        {
            get { return _Nationality == null ? null : ((long)_Nationality.Key == 0 ? null : _Nationality); }
            set 
            { 
                var oldVal = _Nationality;
                _Nationality = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_Nationality != oldVal)
                {
                    if (idfsNationality != (_Nationality == null
                            ? new Int64?()
                            : (Int64?)_Nationality.idfsBaseReference))
                        idfsNationality = _Nationality == null 
                            ? new Int64?()
                            : (Int64?)_Nationality.idfsBaseReference; 
                    OnPropertyChanged(_str_Nationality); 
                }
            }
        }
        private BaseReference _Nationality;

        
        public BaseReferenceList NationalityLookup
        {
            get { return _NationalityLookup; }
        }
        private BaseReferenceList _NationalityLookup = new BaseReferenceList("rftNationality");
            
        [LocalizedDisplayName(_str_Gender)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsHumanGender)]
        public BaseReference Gender
        {
            get { return _Gender == null ? null : ((long)_Gender.Key == 0 ? null : _Gender); }
            set 
            { 
                var oldVal = _Gender;
                _Gender = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_Gender != oldVal)
                {
                    if (idfsHumanGender != (_Gender == null
                            ? new Int64?()
                            : (Int64?)_Gender.idfsBaseReference))
                        idfsHumanGender = _Gender == null 
                            ? new Int64?()
                            : (Int64?)_Gender.idfsBaseReference; 
                    OnPropertyChanged(_str_Gender); 
                }
            }
        }
        private BaseReference _Gender;

        
        public BaseReferenceList GenderLookup
        {
            get { return _GenderLookup; }
        }
        private BaseReferenceList _GenderLookup = new BaseReferenceList("rftHumanGender");
            
        [LocalizedDisplayName(_str_PersonIDType)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsPersonIDType)]
        public BaseReference PersonIDType
        {
            get { return _PersonIDType == null ? null : ((long)_PersonIDType.Key == 0 ? null : _PersonIDType); }
            set 
            { 
                var oldVal = _PersonIDType;
                _PersonIDType = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_PersonIDType != oldVal)
                {
                    if (idfsPersonIDType != (_PersonIDType == null
                            ? new Int64?()
                            : (Int64?)_PersonIDType.idfsBaseReference))
                        idfsPersonIDType = _PersonIDType == null 
                            ? new Int64?()
                            : (Int64?)_PersonIDType.idfsBaseReference; 
                    OnPropertyChanged(_str_PersonIDType); 
                }
            }
        }
        private BaseReference _PersonIDType;

        
        public BaseReferenceList PersonIDTypeLookup
        {
            get { return _PersonIDTypeLookup; }
        }
        private BaseReferenceList _PersonIDTypeLookup = new BaseReferenceList("rftPersonIDType");
            
        [LocalizedDisplayName(_str_HumanAgeType)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsHumanAgeTypeFromCase)]
        public BaseReference HumanAgeType
        {
            get { return _HumanAgeType == null ? null : ((long)_HumanAgeType.Key == 0 ? null : _HumanAgeType); }
            set 
            { 
                var oldVal = _HumanAgeType;
                _HumanAgeType = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_HumanAgeType != oldVal)
                {
                    if (idfsHumanAgeTypeFromCase != (_HumanAgeType == null
                            ? new Int64?()
                            : (Int64?)_HumanAgeType.idfsBaseReference))
                        idfsHumanAgeTypeFromCase = _HumanAgeType == null 
                            ? new Int64?()
                            : (Int64?)_HumanAgeType.idfsBaseReference; 
                    OnPropertyChanged(_str_HumanAgeType); 
                }
            }
        }
        private BaseReference _HumanAgeType;

        
        public BaseReferenceList HumanAgeTypeLookup
        {
            get { return _HumanAgeTypeLookup; }
        }
        private BaseReferenceList _HumanAgeTypeLookup = new BaseReferenceList("rftHumanAgeType");
            
        [LocalizedDisplayName(_str_SearchMethod)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsSearchMethod)]
        public BaseReference SearchMethod
        {
            get { return _SearchMethod; }
            set 
            { 
                var oldVal = _SearchMethod;
                _SearchMethod = value;
                if (_SearchMethod != oldVal)
                {
                    if (idfsSearchMethod != (_SearchMethod == null
                            ? new long?()
                            : _SearchMethod.idfsBaseReference))
                        idfsSearchMethod = _SearchMethod == null 
                            ? new long?()
                            : _SearchMethod.idfsBaseReference; 
                    OnPropertyChanged(_str_SearchMethod); 
                }
            }
        }
        private BaseReference _SearchMethod;

        
        public BaseReferenceList SearchMethodLookup
        {
            get { return _SearchMethodLookup; }
        }
        private BaseReferenceList _SearchMethodLookup = new BaseReferenceList("rftEmpty");
            
        [LocalizedDisplayName(_str_DocumentType)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsDocumentType)]
        public BaseReference DocumentType
        {
            get { return _DocumentType; }
            set 
            { 
                var oldVal = _DocumentType;
                _DocumentType = value;
                if (_DocumentType != oldVal)
                {
                    if (idfsDocumentType != (_DocumentType == null
                            ? new long?()
                            : _DocumentType.idfsBaseReference))
                        idfsDocumentType = _DocumentType == null 
                            ? new long?()
                            : _DocumentType.idfsBaseReference; 
                    OnPropertyChanged(_str_DocumentType); 
                }
            }
        }
        private BaseReference _DocumentType;

        
        public BaseReferenceList DocumentTypeLookup
        {
            get { return _DocumentTypeLookup; }
        }
        private BaseReferenceList _DocumentTypeLookup = new BaseReferenceList("rftEmpty");
            
        private BvSelectList _getList(string name)
        {
        
            switch(name)
            {
            
                case _str_OccupationType:
                    return new BvSelectList(OccupationTypeLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, OccupationType, _str_idfsOccupationType);
            
                case _str_Nationality:
                    return new BvSelectList(NationalityLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, Nationality, _str_idfsNationality);
            
                case _str_Gender:
                    return new BvSelectList(GenderLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, Gender, _str_idfsHumanGender);
            
                case _str_PersonIDType:
                    return new BvSelectList(PersonIDTypeLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, PersonIDType, _str_idfsPersonIDType);
            
                case _str_HumanAgeType:
                    return new BvSelectList(HumanAgeTypeLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, HumanAgeType, _str_idfsHumanAgeTypeFromCase);
            
                case _str_SearchMethod:
                    return new BvSelectList(SearchMethodLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, SearchMethod, _str_idfsSearchMethod);
            
                case _str_DocumentType:
                    return new BvSelectList(DocumentTypeLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, DocumentType, _str_idfsDocumentType);
            
            }
        
            return null;
        }
    
          [XmlIgnore]
          [LocalizedDisplayName(_str_HCase)]
        public HumanCase HCase
        {
            get { return new Func<Patient, HumanCase>(c => c.Parent as HumanCase)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strName)]
        public string strName
        {
            get { return new Func<Patient, string>(c => Customization.Instance.BuildFullName(c.strLastName, c.strFirstName, c.strSecondName))(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_IsReadOnlyParent)]
        public bool IsReadOnlyParent
        {
            get { return new Func<Patient, bool>(c => c.HCase == null ? false : c.HCase.IsClosed || c.HCase.ReadOnly)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlyLastName)]
        public string strReadOnlyLastName
        {
            get { return new Func<Patient, string>(c => c.strLastName)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlyFirstName)]
        public string strReadOnlyFirstName
        {
            get { return new Func<Patient, string>(c => c.strFirstName)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlySecondName)]
        public string strReadOnlySecondName
        {
            get { return new Func<Patient, string>(c => c.strSecondName)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlyDateofBirth)]
        public string strReadOnlyDateofBirth
        {
            get { return new Func<Patient, string>(c => c.datDateofBirth == null ? (string)null : c.datDateofBirth.Value.ToString())(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlyHumanGender)]
        public string strReadOnlyHumanGender
        {
            get { return new Func<Patient, string>(c => c.idfsHumanGender == null ? (string)null : c.Gender.name)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlyNationality)]
        public string strReadOnlyNationality
        {
            get { return new Func<Patient, string>(c => c.idfsNationality == null ? (string)null : c.Nationality.name)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strReadOnlyEmployerName)]
        public string strReadOnlyEmployerName
        {
            get { return new Func<Patient, string>(c => c.strEmployerName)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_IsRoot)]
        public bool IsRoot
        {
            get { return new Func<Patient, bool>(c => !c.idfRootHuman.HasValue)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strEnteredDate)]
        public string strEnteredDate
        {
            get { return new Func<Patient, string>(c => c.datEnteredDate.HasValue ? c.datEnteredDate.Value.ToShortDateString() : "")(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strModificationDate)]
        public string strModificationDate
        {
            get { return new Func<Patient, string>(c => c.datModificationDate.HasValue ? c.datModificationDate.Value.ToShortDateString() : "")(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_IsFirstCreatedAndGGPin)]
        public bool IsFirstCreatedAndGGPin
        {
            get { return new Func<Patient, bool>(c => eidss.model.Core.EidssSiteContext.Instance.IsGeorgiaCustomization && c.bFirstCreated && c.idfsPersonIDType == (long)PersonalIDType.PIN_GG)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_IsGGPinChangedAndNotValidated)]
        public bool IsGGPinChangedAndNotValidated
        {
            get { return new Func<Patient, bool>(c => eidss.model.Core.EidssSiteContext.Instance.IsGeorgiaCustomization && c.idfsPersonIDType == (long)PersonalIDType.PIN_GG && c.strPersonID.CompareTo(c.strPinValid) != 0)(this); }
            
        }
        
          [LocalizedDisplayName(_str_bSearchMode)]
        public bool bSearchMode
        {
            get { return m_bSearchMode; }
            set { if (m_bSearchMode != value) { m_bSearchMode = value; OnPropertyChanged(_str_bSearchMode); } }
        }
        private bool m_bSearchMode;
        
          [LocalizedDisplayName(_str_bPINMode)]
        public bool bPINMode
        {
            get { return m_bPINMode; }
            set { if (m_bPINMode != value) { m_bPINMode = value; OnPropertyChanged(_str_bPINMode); } }
        }
        private bool m_bPINMode;
        
          [LocalizedDisplayName(_str_bFirstCreated)]
        public bool bFirstCreated
        {
            get { return m_bFirstCreated; }
            set { if (m_bFirstCreated != value) { m_bFirstCreated = value; OnPropertyChanged(_str_bFirstCreated); } }
        }
        private bool m_bFirstCreated;
        
          [LocalizedDisplayName(_str_strPinValid)]
        public string strPinValid
        {
            get { return m_strPinValid; }
            set { if (m_strPinValid != value) { m_strPinValid = value; OnPropertyChanged(_str_strPinValid); } }
        }
        private string m_strPinValid;
        
          [LocalizedDisplayName(_str_idfContactedCasePerson)]
        public long idfContactedCasePerson
        {
            get { return m_idfContactedCasePerson; }
            set { if (m_idfContactedCasePerson != value) { m_idfContactedCasePerson = value; OnPropertyChanged(_str_idfContactedCasePerson); } }
        }
        private long m_idfContactedCasePerson;
        
          [LocalizedDisplayName(_str_idfsSearchMethod)]
        public long? idfsSearchMethod
        {
            get { return m_idfsSearchMethod; }
            set { if (m_idfsSearchMethod != value) { m_idfsSearchMethod = value; OnPropertyChanged(_str_idfsSearchMethod); } }
        }
        private long? m_idfsSearchMethod;
        
          [LocalizedDisplayName(_str_idfsDocumentType)]
        public long? idfsDocumentType
        {
            get { return m_idfsDocumentType; }
            set { if (m_idfsDocumentType != value) { m_idfsDocumentType = value; OnPropertyChanged(_str_idfsDocumentType); } }
        }
        private long? m_idfsDocumentType;
        
          [LocalizedDisplayName(_str_strDocumentNumber)]
        public string strDocumentNumber
        {
            get { return m_strDocumentNumber; }
            set { if (m_strDocumentNumber != value) { m_strDocumentNumber = value; OnPropertyChanged(_str_strDocumentNumber); } }
        }
        private string m_strDocumentNumber;
        
          [LocalizedDisplayName(_str_datDocumentDate)]
        public DateTime? datDocumentDate
        {
            get { return m_datDocumentDate; }
            set { if (m_datDocumentDate != value) { m_datDocumentDate = value; OnPropertyChanged(_str_datDocumentDate); } }
        }
        private DateTime? m_datDocumentDate;
        
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "Patient";

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
        
            if (_CurrentResidenceAddress != null) { _CurrentResidenceAddress.Parent = this; }
                
            if (_EmployerAddress != null) { _EmployerAddress.Parent = this; }
                
            if (_RegistrationAddress != null) { _RegistrationAddress.Parent = this; }
                
        }
        partial void Cloned();
        partial void ClonedWithSetup();
        private bool bIsClone;
        public override object Clone()
        {
            var ret = base.Clone() as Patient;
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
            var ret = base.Clone() as Patient;
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        
            if (_CurrentResidenceAddress != null)
              ret.CurrentResidenceAddress = _CurrentResidenceAddress.CloneWithSetup(manager, bRestricted) as Address;
                
            if (_EmployerAddress != null)
              ret.EmployerAddress = _EmployerAddress.CloneWithSetup(manager, bRestricted) as Address;
                
            if (_RegistrationAddress != null)
              ret.RegistrationAddress = _RegistrationAddress.CloneWithSetup(manager, bRestricted) as Address;
                
            Accessor.Instance(null)._SetupLoad(manager, ret, true);
            ret.ClonedWithSetup();
            ret.DeepAcceptChanges();
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public Patient CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as Patient;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfHuman; } }
        public string KeyName { get { return "idfHuman"; } }
        public object KeyLookup { get { return idfHuman; } }
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
        
                    || (_CurrentResidenceAddress != null && _CurrentResidenceAddress.HasChanges)
                
                    || (_EmployerAddress != null && _EmployerAddress.HasChanges)
                
                    || (_RegistrationAddress != null && _RegistrationAddress.HasChanges)
                
                ;
            }
        }
        public new void RejectChanges()
        {
        
            var _prev_idfsOccupationType_OccupationType = idfsOccupationType;
            var _prev_idfsNationality_Nationality = idfsNationality;
            var _prev_idfsHumanGender_Gender = idfsHumanGender;
            var _prev_idfsPersonIDType_PersonIDType = idfsPersonIDType;
            var _prev_idfsHumanAgeTypeFromCase_HumanAgeType = idfsHumanAgeTypeFromCase;
            var _prev_idfsSearchMethod_SearchMethod = idfsSearchMethod;
            var _prev_idfsDocumentType_DocumentType = idfsDocumentType;
            base.RejectChanges();
        
            if (_prev_idfsOccupationType_OccupationType != idfsOccupationType)
            {
                _OccupationType = _OccupationTypeLookup.FirstOrDefault(c => c.idfsBaseReference == idfsOccupationType);
            }
            if (_prev_idfsNationality_Nationality != idfsNationality)
            {
                _Nationality = _NationalityLookup.FirstOrDefault(c => c.idfsBaseReference == idfsNationality);
            }
            if (_prev_idfsHumanGender_Gender != idfsHumanGender)
            {
                _Gender = _GenderLookup.FirstOrDefault(c => c.idfsBaseReference == idfsHumanGender);
            }
            if (_prev_idfsPersonIDType_PersonIDType != idfsPersonIDType)
            {
                _PersonIDType = _PersonIDTypeLookup.FirstOrDefault(c => c.idfsBaseReference == idfsPersonIDType);
            }
            if (_prev_idfsHumanAgeTypeFromCase_HumanAgeType != idfsHumanAgeTypeFromCase)
            {
                _HumanAgeType = _HumanAgeTypeLookup.FirstOrDefault(c => c.idfsBaseReference == idfsHumanAgeTypeFromCase);
            }
            if (_prev_idfsSearchMethod_SearchMethod != idfsSearchMethod)
            {
                _SearchMethod = _SearchMethodLookup.FirstOrDefault(c => c.idfsBaseReference == idfsSearchMethod);
            }
            if (_prev_idfsDocumentType_DocumentType != idfsDocumentType)
            {
                _DocumentType = _DocumentTypeLookup.FirstOrDefault(c => c.idfsBaseReference == idfsDocumentType);
            }
        }
        public void DeepRejectChanges()
        {
            RejectChanges();
        
            if (CurrentResidenceAddress != null) CurrentResidenceAddress.DeepRejectChanges();
                
            if (EmployerAddress != null) EmployerAddress.DeepRejectChanges();
                
            if (RegistrationAddress != null) RegistrationAddress.DeepRejectChanges();
                
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        
            if (_CurrentResidenceAddress != null) _CurrentResidenceAddress.DeepAcceptChanges();
                
            if (_EmployerAddress != null) _EmployerAddress.DeepAcceptChanges();
                
            if (_RegistrationAddress != null) _RegistrationAddress.DeepAcceptChanges();
                
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
        
            if (_CurrentResidenceAddress != null) _CurrentResidenceAddress.SetChange();
                
            if (_EmployerAddress != null) _EmployerAddress.SetChange();
                
            if (_RegistrationAddress != null) _RegistrationAddress.SetChange();
                
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

        private bool IsRIRPropChanged(string fld, Patient c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, Patient c)
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
        

      

        public Patient()
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Patient_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Patient_PropertyChanged);
        }
        private void Patient_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as Patient).Changed(e.PropertyName);
            
            if (e.PropertyName == _str_Parent)
                OnPropertyChanged(_str_HCase);
                  
            if (e.PropertyName == _str_strFirstName)
                OnPropertyChanged(_str_strName);
                  
            if (e.PropertyName == _str_strSecondName)
                OnPropertyChanged(_str_strName);
                  
            if (e.PropertyName == _str_strLastName)
                OnPropertyChanged(_str_strName);
                  
            if (e.PropertyName == _str_HCase)
                OnPropertyChanged(_str_IsReadOnlyParent);
                  
            if (e.PropertyName == _str_strLastName)
                OnPropertyChanged(_str_strReadOnlyLastName);
                  
            if (e.PropertyName == _str_strFirstName)
                OnPropertyChanged(_str_strReadOnlyFirstName);
                  
            if (e.PropertyName == _str_strSecondName)
                OnPropertyChanged(_str_strReadOnlySecondName);
                  
            if (e.PropertyName == _str_datDateofBirth)
                OnPropertyChanged(_str_strReadOnlyDateofBirth);
                  
            if (e.PropertyName == _str_idfsHumanGender)
                OnPropertyChanged(_str_strReadOnlyHumanGender);
                  
            if (e.PropertyName == _str_idfsNationality)
                OnPropertyChanged(_str_strReadOnlyNationality);
                  
            if (e.PropertyName == _str_strEmployerName)
                OnPropertyChanged(_str_strReadOnlyEmployerName);
                  
            if (e.PropertyName == _str_idfRootHuman)
                OnPropertyChanged(_str_IsRoot);
                  
            if (e.PropertyName == _str_datEnteredDate)
                OnPropertyChanged(_str_strEnteredDate);
                  
            if (e.PropertyName == _str_datModificationDate)
                OnPropertyChanged(_str_strModificationDate);
                  
            if (e.PropertyName == _str_idfsPersonIDType)
                OnPropertyChanged(_str_IsFirstCreatedAndGGPin);
                  
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
            
            return Accessor.Instance(null).ValidateCanDelete(this);
              
        }
        private void _DeletingExtenders()
        {
            Patient obj = this;
            
        }
        private void _DeletedExtenders()
        {
            Patient obj = this;
            
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
    
        private static string[] invisible_names1 = "DocumentType,strDocumentNumber".Split(new char[] { ',' });
        
        private static string[] invisible_names2 = "datDocumentDate".Split(new char[] { ',' });
        
        private bool _isInvisible(string name)
        {
            
            if (invisible_names1.Where(c => c == name).Count() > 0)
                return new Func<Patient, bool>(c => c.idfsSearchMethod != 2)(this);
            
            if (invisible_names2.Where(c => c == name).Count() > 0)
                return new Func<Patient, bool>(c => c.idfsSearchMethod != 2 || c.idfsDocumentType != 6)(this);
            
            return false;
                
        }

    
        private static string[] readonly_names1 = "strName".Split(new char[] { ',' });
        
        private static string[] readonly_names2 = "strReadOnlyLastName,strReadOnlyFirstName,strReadOnlySecondName,strReadOnlyEmployerName".Split(new char[] { ',' });
        
        private static string[] readonly_names3 = "strReadOnlyDateofBirth,strReadOnlyHumanGender,strReadOnlyNationality".Split(new char[] { ',' });
        
        private static string[] readonly_names4 = "strPersonID".Split(new char[] { ',' });
        
        private static string[] readonly_names5 = "datEnteredDate,datModificationDate,strEnteredDate,strModificationDate".Split(new char[] { ',' });
        
        private static string[] readonly_names6 = "intPatientAgeFromCase,idfsHumanAgeTypeFromCase,HumanAgeType".Split(new char[] { ',' });

        private static string[] readonly_names7;

        private bool _isReadOnly(string name)
        {
            if(EidssSiteContext.Instance.IsGeorgiaCustomization)
            {
                readonly_names7 = "strLastName,strSecondName,strFirstName,strRegistrationPhone,strEmployerName,strHomePhone,strWorkPhone,idfsOccupationType,OccupationType,idfsNationality,Nationality,idfsHumanGender,Gender,datDateOfDeath".Split(new char[] { ',' });
            }
            else
            {
                readonly_names7 = "strLastName,strSecondName,strFirstName,strRegistrationPhone,strEmployerName,strHomePhone,strWorkPhone,idfsOccupationType,OccupationType,idfsNationality,Nationality,idfsHumanGender,Gender,datDateofBirth,datDateOfDeath".Split(new char[] { ',' });
            }
            
            if (readonly_names1.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => true)(this);
            
            if (readonly_names2.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => true)(this);
            
            if (readonly_names3.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => true)(this);
            
            if (readonly_names4.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => c.bPINMode || c.IsReadOnlyParent || !c.idfsPersonIDType.HasValue || c.idfsPersonIDType.Value == (long)eidss.model.Enums.PersonalIDType.Unknown)(this);
            
            if (readonly_names5.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => true)(this);
            
            if (readonly_names6.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => c.bPINMode || c.IsReadOnlyParent || c.IsFirstCreatedAndGGPin || c.datDateofBirth != null)(this);

            if (readonly_names7.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<Patient, bool>(c => c.bPINMode || c.IsReadOnlyParent || c.IsFirstCreatedAndGGPin)(this);

            return ReadOnly || new Func<Patient, bool>(c => c.bPINMode || c.IsReadOnlyParent)(this);
                
        }

        private bool m_isValid = true;
        internal bool _isValid
        {
            get { return m_isValid; }
            set
            {
                m_isValid = value;
        
                if (_CurrentResidenceAddress != null)
                    _CurrentResidenceAddress._isValid &= value;
                
                if (_EmployerAddress != null)
                    _EmployerAddress._isValid &= value;
                
                if (_RegistrationAddress != null)
                    _RegistrationAddress._isValid &= value;
                
            }
        }

        private bool m_readOnly;
        private bool _readOnly
        {
            get { return m_readOnly; }
            set
            {
                m_readOnly = value;
        
                if (_CurrentResidenceAddress != null)
                    _CurrentResidenceAddress.ReadOnly |= value;
                
                if (_EmployerAddress != null)
                    _EmployerAddress.ReadOnly |= value;
                
                if (_RegistrationAddress != null)
                    _RegistrationAddress.ReadOnly |= value;
                
            }
        }


        internal Dictionary<string, Func<Patient, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<Patient, bool>> isRequiredDict, string name)
        {
            if (EidssSiteContext.Instance.IsGeorgiaCustomization)
            {
                if(name == "datDateofBirth")
                {
                    if(IsFirstCreatedAndGGPin)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                    //    return isRequiredDict[name](this);
                    //return false;
                }
                else
                {
                    if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                        return isRequiredDict[name](this);
                    return false;
                }
            }
            else
            {
                if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                    return isRequiredDict[name](this);
                return false;
            }
        }
        
        public void AddRequired(string name, Func<Patient, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<Patient, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<Patient, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<Patient, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<Patient, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~Patient()
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
                
                if (_CurrentResidenceAddress != null)
                    CurrentResidenceAddress.Dispose();
                
                if (_EmployerAddress != null)
                    EmployerAddress.Dispose();
                
                if (_RegistrationAddress != null)
                    RegistrationAddress.Dispose();
                
                
                if (bNeedLookupRemove)
                {
                
                LookupManager.RemoveObject("rftOccupationType", this);
                
                LookupManager.RemoveObject("rftNationality", this);
                
                LookupManager.RemoveObject("rftHumanGender", this);
                
                LookupManager.RemoveObject("rftPersonIDType", this);
                
                LookupManager.RemoveObject("rftHumanAgeType", this);
                
                LookupManager.RemoveObject("rftEmpty", this);
                
                LookupManager.RemoveObject("rftEmpty", this);
                
                }
                Disposed();
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    
        #region ILookupUsage Members
        public void ReloadLookupItem(DbManagerProxy manager, string lookup_object)
        {
            
            if (lookup_object == "rftOccupationType")
                _getAccessor().LoadLookup_OccupationType(manager, this);
            
            if (lookup_object == "rftNationality")
                _getAccessor().LoadLookup_Nationality(manager, this);
            
            if (lookup_object == "rftHumanGender")
                _getAccessor().LoadLookup_Gender(manager, this);
            
            if (lookup_object == "rftPersonIDType")
                _getAccessor().LoadLookup_PersonIDType(manager, this);
            
            if (lookup_object == "rftHumanAgeType")
                _getAccessor().LoadLookup_HumanAgeType(manager, this);
            
            if (lookup_object == "rftEmpty")
                _getAccessor().LoadLookup_SearchMethod(manager, this);
            
            if (lookup_object == "rftEmpty")
                _getAccessor().LoadLookup_DocumentType(manager, this);
            
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
        
            if (_CurrentResidenceAddress != null) _CurrentResidenceAddress.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            if (_EmployerAddress != null) _EmployerAddress.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            if (_RegistrationAddress != null) _RegistrationAddress.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            }
            ParsedFormCollection(form);
        }
    

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<Patient>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectCreator
            , IObjectCreator<Patient>
            
            , IObjectSelectDetail
            , IObjectSelectDetail<Patient>
            , IObjectPost
            , IObjectDelete
                    
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfHuman"; } }
            #endregion
        
            public delegate void on_action(Patient obj);
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
            private Address.Accessor CurrentResidenceAddressAccessor { get { return eidss.model.Schema.Address.Accessor.Instance(m_CS); } }
            private Address.Accessor EmployerAddressAccessor { get { return eidss.model.Schema.Address.Accessor.Instance(m_CS); } }
            private Address.Accessor RegistrationAddressAccessor { get { return eidss.model.Schema.Address.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor OccupationTypeAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor NationalityAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor GenderAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor PersonIDTypeAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor HumanAgeTypeAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor SearchMethodAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor DocumentTypeAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            

            public virtual IObject SelectDetail(DbManagerProxy manager, object ident, int? HACode = null)
            {
                if (ident == null)
                {
                    return CreateNew(manager, null, HACode);
                }
                else
                {
                    return _SelectByKey(manager
                        , (Int64?)ident
                            
                        , null, null
                        );
                }
            }
            public virtual Patient SelectDetailT(DbManagerProxy manager, object ident, int? HACode = null)
            {
                if (ident == null)
                {
                    return CreateNewT(manager, null, HACode);
                }
                else
                {
                    return _SelectByKey(manager
                        , (Int64?)ident
                            
                        , null, null
                        );
                }
            }

            
            public virtual Patient SelectByKey(DbManagerProxy manager
                , Int64? idfHuman
                )
            {
                return _SelectByKey(manager
                    , idfHuman
                    , null, null
                    );
            }
            

            private Patient _SelectByKey(DbManagerProxy manager
                , Int64? idfHuman
                , on_action loading, on_action loaded
                )
            {
                return _SelectByKeyInternal(manager
                    , idfHuman
                    , loading, loaded
                    )
                    
                    ;
            }
      
            
            protected virtual Patient _SelectByKeyInternal(DbManagerProxy manager
                , Int64? idfHuman
                , on_action loading, on_action loaded
                )
            {
            
                MapResultSet[] sets = new MapResultSet[1];
                List<Patient> objs = new List<Patient>();
                sets[0] = new MapResultSet(typeof(Patient), objs);
                Patient obj = null;
                try
                {
                    manager
                        .SetSpCommand("spPatient_SelectDetail"
                            , manager.Parameter("@idfHuman", idfHuman)
                            )
                        .ExecuteResultSet(sets);

                    if (objs.Count == 0)
                        return null;

                    obj = objs[0];
                    obj.m_CS = m_CS;
                    
                  
                    if (loading != null)
                        loading(obj);
                    _SetupLoad(manager, obj);
                    
                      if (BaseSettings.ValidateObject)
                          obj._isValid = (manager.SetSpCommand("spPatient_Validate", obj.Key).ExecuteScalar<int>(ScalarSourceType.ReturnValue) == 0);
                    

                    //obj._setParent();
                    if (loaded != null)
                    loaded(obj);
                    obj.Loaded(manager);
                    return obj;
                  }
                  catch(DataException e)
                  {
                    throw DbModelException.Create(obj, e);
                  }
                
            }
    
            internal void _LoadCurrentResidenceAddress(Patient obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadCurrentResidenceAddress(manager, obj);
                }
            }
            internal void _LoadCurrentResidenceAddress(DbManagerProxy manager, Patient obj)
            {
              
                if (obj.CurrentResidenceAddress == null && obj.idfCurrentResidenceAddress != null && obj.idfCurrentResidenceAddress != 0)
                {
                    obj.CurrentResidenceAddress = CurrentResidenceAddressAccessor.SelectByKey(manager
                        
                        , obj.idfCurrentResidenceAddress.Value
                        );
                    if (obj.CurrentResidenceAddress != null)
                    {
                        obj.CurrentResidenceAddress.m_ObjectName = _str_CurrentResidenceAddress;
                    }
                }
                    
              }
            
            internal void _LoadEmployerAddress(Patient obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadEmployerAddress(manager, obj);
                }
            }
            internal void _LoadEmployerAddress(DbManagerProxy manager, Patient obj)
            {
              
                if (obj.EmployerAddress == null && obj.idfEmployerAddress != null && obj.idfEmployerAddress != 0)
                {
                    obj.EmployerAddress = EmployerAddressAccessor.SelectByKey(manager
                        
                        , obj.idfEmployerAddress.Value
                        );
                    if (obj.EmployerAddress != null)
                    {
                        obj.EmployerAddress.m_ObjectName = _str_EmployerAddress;
                    }
                }
                    
              }
            
            internal void _LoadRegistrationAddress(Patient obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadRegistrationAddress(manager, obj);
                }
            }
            internal void _LoadRegistrationAddress(DbManagerProxy manager, Patient obj)
            {
              
                if (obj.RegistrationAddress == null && obj.idfRegistrationAddress != null && obj.idfRegistrationAddress != 0)
                {
                    obj.RegistrationAddress = RegistrationAddressAccessor.SelectByKey(manager
                        
                        , obj.idfRegistrationAddress.Value
                        );
                    if (obj.RegistrationAddress != null)
                    {
                        obj.RegistrationAddress.m_ObjectName = _str_RegistrationAddress;
                    }
                }
                    
              }
            
        
        
            internal void _SetupLoad(DbManagerProxy manager, Patient obj, bool bCloning = false)
            {
                if (obj == null) return;
                
                // loading extenters - begin
                // loading extenters - end
                if (!bCloning)
                {
                
                _LoadCurrentResidenceAddress(manager, obj);
                _LoadEmployerAddress(manager, obj);
                _LoadRegistrationAddress(manager, obj);
                }
                _LoadLookups(manager, obj);
                obj._setParent();
                
                // loaded extenters - begin
                obj.CurrentResidenceAddress = new Func<Patient, Address>(c => c.CurrentResidenceAddress == null ? CurrentResidenceAddressAccessor.CreateAsSharedOrNot(manager, c, c.IsRoot) : c.CurrentResidenceAddress)(obj);
                obj.EmployerAddress = new Func<Patient, Address>(c => c.EmployerAddress == null ? EmployerAddressAccessor.CreateAsSharedOrNot(manager, c, c.IsRoot) : c.EmployerAddress)(obj);
                obj.RegistrationAddress = new Func<Patient, Address>(c => c.RegistrationAddress == null ? RegistrationAddressAccessor.CreateAsSharedOrNot(manager, c, c.IsRoot) : c.RegistrationAddress)(obj);
                obj.strPinValid = new Func<Patient, string>(c => c.strPersonID)(obj);
                // loaded extenters - end
                
                _SetupHandlers(obj);
                _SetupChildHandlers(obj, null);
                
                _SetPermitions(obj._permissions, obj);
                _SetupRequired(obj);
                _SetupPersonalDataRestrictions(obj);
                obj._SetupMainHandler();
                obj.AcceptChanges();
            }
            
            internal void _SetPermitions(IObjectPermissions permissions, Patient obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    CurrentResidenceAddressAccessor._SetPermitions(obj._permissions, obj.CurrentResidenceAddress);
                    EmployerAddressAccessor._SetPermitions(obj._permissions, obj.EmployerAddress);
                    RegistrationAddressAccessor._SetPermitions(obj._permissions, obj.RegistrationAddress);
                    
                    }
                }
            }

    

            private Patient _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                Patient obj = null;
                try
                {
                    obj = Patient.CreateInstance();
                    obj.m_CS = m_CS;
                    obj.m_IsNew = true;
                    obj.Parent = Parent;
                    
                    if (creating != null)
                        creating(obj);
                
                    // creating extenters - begin
                obj.idfHuman = (new GetNewIDExtender<Patient>()).GetScalar(manager, obj, isFake);
                obj.idfRootHuman = (new GetNewIDExtender<Patient>()).GetScalar(manager, obj, isFake);
                obj.datEnteredDate = new Func<Patient, DateTime?>(c => DateTime.Now)(obj);
                obj.CurrentResidenceAddress = new Func<Patient, Address>(c => CurrentResidenceAddressAccessor.CreateNewT(manager, c))(obj);
                obj.EmployerAddress = new Func<Patient, Address>(c => EmployerAddressAccessor.CreateNewT(manager, c))(obj);
                obj.EmployerAddress.Country = new Func<Patient, CountryLookup>(c => null)(obj);
                obj.EmployerAddress.blnNeedForeignAddressStr = new Func<Patient, bool>(c => false)(obj);
                obj.RegistrationAddress = new Func<Patient, Address>(c => RegistrationAddressAccessor.CreateNewT(manager, c))(obj);
                obj.RegistrationAddress.Country = new Func<Patient, CountryLookup>(c => null)(obj);
                obj.RegistrationAddress.blnNeedForeignAddressStr = new Func<Patient, bool>(c => true)(obj);
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

            
            public Patient CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public Patient CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public Patient CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            public Patient CreateT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                
                if (pars == null) throw new ParamsCountException();
                if (pars.Count != 1) 
                    throw new ParamsCountException();
                if (pars[0] != null && !(pars[0] is long)) 
                    throw new TypeMismatchException("idfCase", typeof(long));
                
                return Create(manager, Parent
                    , (long)pars[0]
                    );
            }
            public IObject Create(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return CreateT(manager, Parent, pars);
            }
            public Patient Create(DbManagerProxy manager, IObject Parent
                , long idfCase
                )
            {
                return _CreateNew(manager, Parent
                
                    , null
                    
                    , obj =>
                {
                obj.idfCase = new Func<Patient, long>(c => idfCase)(obj);
                }
                    , obj =>
                {
                }
                );
            }
            
            private void _SetupChildHandlers(Patient obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(Patient obj)
            {
                
                obj.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                    {
                    
                        if (e.PropertyName == _str_datDateofBirth)
                        {
                                var ex = ChainsValidate(obj);
                                if (ex != null && !obj.OnValidation(ex))
                                {
                                    obj.LockNotifyChanges();
                                    obj.CancelMemberLastChanges(_str_datDateofBirth);
                                    obj.OnValidationEnd(ex);
                                    obj.UnlockNotifyChanges();
                                }
                        }
                    
                    };
                
                obj.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                    {
                    
                        if (e.PropertyName == _str_intPatientAgeFromCase)
                        {
                            try
                            {
                            
                (new PredicateValidator("mbAgeShallNotExceed", "intPatientAgeFromCase", "intPatientAgeFromCase", "intPatientAgeFromCase",
              new object[] {
              },
                        ValidationEventType.Error
                    )).Validate(obj, i => i.idfsHumanAgeTypeFromCase == null || i.intPatientAgeFromCase == null
                                            || (i.idfsHumanAgeTypeFromCase == (long)HumanAgeTypeEnum.Years && i.intPatientAgeFromCase <= 200)
                                            || (i.idfsHumanAgeTypeFromCase == (long)HumanAgeTypeEnum.Month && i.intPatientAgeFromCase <= 60)
                                            || (i.idfsHumanAgeTypeFromCase == (long)HumanAgeTypeEnum.Days && i.intPatientAgeFromCase <= 31)
                                            
                    );
            
                            }
                            catch(ValidationModelException ex)
                            {
                                if (!obj.OnValidation(ex))
                                {
                                    obj.LockNotifyChanges();
                                    obj.CancelMemberLastChanges(_str_intPatientAgeFromCase);
                                    obj.OnValidationEnd(ex);
                                    obj.UnlockNotifyChanges();
                                }
                                
                            }
                        }
                    
                        if (e.PropertyName == _str_idfCurrentResidenceAddress)
                        {
                    
                    obj.CheckAddress();
                  
                        }
                    
                        if (e.PropertyName == _str_idfsPersonIDType)
                        {
                    
                obj.strPersonID = new Func<Patient, string>(c => (!c.idfsPersonIDType.HasValue || c.idfsPersonIDType.Value == (long)eidss.model.Enums.PersonalIDType.Unknown) ? "" : c.strPersonID)(obj);
                        }
                    
                    };
                
            }
    
            public void LoadLookup_OccupationType(DbManagerProxy manager, Patient obj)
            {
                
                obj.OccupationTypeLookup.Clear();
                
                obj.OccupationTypeLookup.Add(OccupationTypeAccessor.CreateNewT(manager, null));
                
                obj.OccupationTypeLookup.AddRange(OccupationTypeAccessor.rftOccupationType_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsOccupationType))
                    
                    .ToList());
                
                if (obj.idfsOccupationType != null && obj.idfsOccupationType != 0)
                {
                    obj.OccupationType = obj.OccupationTypeLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsOccupationType);
                    
                }
              
                LookupManager.AddObject("rftOccupationType", obj, OccupationTypeAccessor.GetType(), "rftOccupationType_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_Nationality(DbManagerProxy manager, Patient obj)
            {
                
                obj.NationalityLookup.Clear();
                
                obj.NationalityLookup.Add(NationalityAccessor.CreateNewT(manager, null));
                
                obj.NationalityLookup.AddRange(NationalityAccessor.rftNationality_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsNationality))
                    
                    .ToList());
                
                if (obj.idfsNationality != null && obj.idfsNationality != 0)
                {
                    obj.Nationality = obj.NationalityLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsNationality);
                    
                }
              
                LookupManager.AddObject("rftNationality", obj, NationalityAccessor.GetType(), "rftNationality_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_Gender(DbManagerProxy manager, Patient obj)
            {
                
                obj.GenderLookup.Clear();
                
                obj.GenderLookup.Add(GenderAccessor.CreateNewT(manager, null));
                
                obj.GenderLookup.AddRange(GenderAccessor.rftHumanGender_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsHumanGender))
                    
                    .ToList());
                
                if (obj.idfsHumanGender != null && obj.idfsHumanGender != 0)
                {
                    obj.Gender = obj.GenderLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsHumanGender);
                    
                }
              
                LookupManager.AddObject("rftHumanGender", obj, GenderAccessor.GetType(), "rftHumanGender_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_PersonIDType(DbManagerProxy manager, Patient obj)
            {
                
                obj.PersonIDTypeLookup.Clear();
                
                obj.PersonIDTypeLookup.Add(PersonIDTypeAccessor.CreateNewT(manager, null));
                
                obj.PersonIDTypeLookup.AddRange(PersonIDTypeAccessor.rftPersonIDType_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsPersonIDType))
                    
                    .ToList());
                
                if (obj.idfsPersonIDType != null && obj.idfsPersonIDType != 0)
                {
                    obj.PersonIDType = obj.PersonIDTypeLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsPersonIDType);
                    
                }
              
                LookupManager.AddObject("rftPersonIDType", obj, PersonIDTypeAccessor.GetType(), "rftPersonIDType_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_HumanAgeType(DbManagerProxy manager, Patient obj)
            {
                
                obj.HumanAgeTypeLookup.Clear();
                
                obj.HumanAgeTypeLookup.Add(HumanAgeTypeAccessor.CreateNewT(manager, null));
                
                obj.HumanAgeTypeLookup.AddRange(HumanAgeTypeAccessor.rftHumanAgeType_SelectList(manager
                    
                    )
                    .Where(c => c.idfsBaseReference != (long)eidss.model.Enums.HumanAgeTypeEnum.Weeks)
                        
                    .Where(c => (c.intHACode.GetValueOrDefault() & (int)HACode.Human) != 0)
                        
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsHumanAgeTypeFromCase))
                    
                    .ToList());
                
                if (obj.idfsHumanAgeTypeFromCase != null && obj.idfsHumanAgeTypeFromCase != 0)
                {
                    obj.HumanAgeType = obj.HumanAgeTypeLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsHumanAgeTypeFromCase);
                    
                }
              
                LookupManager.AddObject("rftHumanAgeType", obj, HumanAgeTypeAccessor.GetType(), "rftHumanAgeType_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_SearchMethod(DbManagerProxy manager, Patient obj)
            {
                
                obj.SearchMethodLookup.Clear();
                
                obj.SearchMethodLookup.Add(SearchMethodAccessor.CreateDummy(manager, null, 1, eidss.model.Resources.EidssMessages.Instance.GetString("strSearchMethodByAttribute")));
                
                obj.SearchMethodLookup.Add(SearchMethodAccessor.CreateDummy(manager, null, 2, eidss.model.Resources.EidssMessages.Instance.GetString("strSearchMethodByIdentityDocument")));
                
                obj.SearchMethodLookup.Add(SearchMethodAccessor.CreateDummy(manager, null, 3, eidss.model.Resources.EidssMessages.Instance.GetString("strSearchMethodByPIN")));
                
                obj.SearchMethodLookup.AddRange(SearchMethodAccessor.rftEmpty_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsSearchMethod))
                    
                    .ToList());
                
                if (obj.idfsSearchMethod != null && obj.idfsSearchMethod != 0)
                {
                    obj.SearchMethod = obj.SearchMethodLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsSearchMethod);
                    
                }
              
                LookupManager.AddObject("rftEmpty", obj, SearchMethodAccessor.GetType(), "rftEmpty_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_DocumentType(DbManagerProxy manager, Patient obj)
            {
                
                obj.DocumentTypeLookup.Clear();
                
                obj.DocumentTypeLookup.Add(DocumentTypeAccessor.CreateDummy(manager, null, 1, eidss.model.Resources.EidssMessages.Instance.GetString("strDocumentTypeAzPassport")));
                
                obj.DocumentTypeLookup.Add(DocumentTypeAccessor.CreateDummy(manager, null, 2, eidss.model.Resources.EidssMessages.Instance.GetString("strDocumentTypeAzIdCardOver18")));
                
                obj.DocumentTypeLookup.Add(DocumentTypeAccessor.CreateDummy(manager, null, 3, eidss.model.Resources.EidssMessages.Instance.GetString("strDocumentTypeAzIdCardUnder18")));
                
                obj.DocumentTypeLookup.Add(DocumentTypeAccessor.CreateDummy(manager, null, 4, eidss.model.Resources.EidssMessages.Instance.GetString("strDocumentTypeAzTemporaryPermit")));
                
                obj.DocumentTypeLookup.Add(DocumentTypeAccessor.CreateDummy(manager, null, 5, eidss.model.Resources.EidssMessages.Instance.GetString("strDocumentTypeAzPermanentPermit")));
                
                obj.DocumentTypeLookup.Add(DocumentTypeAccessor.CreateDummy(manager, null, 6, eidss.model.Resources.EidssMessages.Instance.GetString("strDocumentTypeAzBirthCertificate")));
                
                obj.DocumentTypeLookup.AddRange(DocumentTypeAccessor.rftEmpty_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsDocumentType))
                    
                    .ToList());
                
                if (obj.idfsDocumentType != null && obj.idfsDocumentType != 0)
                {
                    obj.DocumentType = obj.DocumentTypeLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsDocumentType);
                    
                }
              
                LookupManager.AddObject("rftEmpty", obj, DocumentTypeAccessor.GetType(), "rftEmpty_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            

            internal void _LoadLookups(DbManagerProxy manager, Patient obj)
            {
                
                LoadLookup_OccupationType(manager, obj);
                
                LoadLookup_Nationality(manager, obj);
                
                LoadLookup_Gender(manager, obj);
                
                LoadLookup_PersonIDType(manager, obj);
                
                LoadLookup_HumanAgeType(manager, obj);
                
                LoadLookup_SearchMethod(manager, obj);
                
                LoadLookup_DocumentType(manager, obj);
                
            }
    
            [SprocName("spPatientActual_CanDelete")]
            protected abstract void _canDelete(DbManagerProxy manager, Int64? ID, out Boolean Result
                );
        
            [SprocName("spPatientActual_Delete")]
            protected abstract void _postDelete(DbManagerProxy manager
                , Int64? ID
                );
            protected void _postDeletePredicate(DbManagerProxy manager
                , Int64? ID
                )
            {
                
                _postDelete(manager, ID);
                
            }
        
            public bool DeleteObject(DbManagerProxy manager, object ident)
            {
                IObject obj = SelectDetail(manager, ident);
                if (!obj.MarkToDelete()) return false;
                return Post(manager, obj);
            }
        
            [SprocName("spPatient_Post")]
            protected abstract void _post(DbManagerProxy manager, 
                [Direction.InputOutput("datEnteredDate", "datModificationDate")] Patient obj);
            protected void _postPredicate(DbManagerProxy manager, 
                [Direction.InputOutput("datEnteredDate", "datModificationDate")] Patient obj)
            {
                
                _post(manager, obj);
                
            }
            
            
            public bool Post(DbManagerProxy manager, IObject obj, bool bChildObject = false)
            {
                bool bSuccess = false;
                int iDeadlockAttemptsCount = BaseSettings.DeadlockAttemptsCount;
                for (int iAttemptNumber = 0; iAttemptNumber < iDeadlockAttemptsCount; iAttemptNumber++)
                {
                    bool bTransactionStarted = false;
                    try
                    {
                        Patient bo = obj as Patient;
                        
                        if (!bo.IsNew && bo.IsMarkedToDelete) // delete
                        {
                        }
                        else if (bo.IsNew && !bo.IsMarkedToDelete) // insert
                        {
                            
                        }
                        else if (!bo.IsMarkedToDelete) // update
                        {
                            
                        }

                        if (!manager.IsTransactionStarted)
                        {
                            
                            bTransactionStarted = true;
                            manager.BeginTransaction();
                        }
                        bSuccess = _PostNonTransaction(manager, obj as Patient, bChildObject);
                        if (bTransactionStarted)
                        {
                            if (bSuccess)
                            {
                                obj.DeepAcceptChanges();
                                manager.CommitTransaction();
                                
                            }
                            else
                            {
                                manager.RollbackTransaction();
                            }
                            
                        }
                        if (bSuccess && bo.IsNew && !bo.IsMarkedToDelete) // insert
                        {
                            bo.m_IsNew = false;
                        }
                        if (bSuccess && bTransactionStarted)
                        {
                            bo.OnAfterPost();
                        }
                        
                        break;
                    }
                    catch(Exception e)
                    {
                        if (bTransactionStarted)
                        {
                            manager.RollbackTransaction();
                            
                            if (DbModelException.IsDeadlockException(e))
                            {
                                System.Threading.Thread.Sleep(BaseSettings.DeadlockDelay);
                                continue;
                            }
                        }
                    
                        if (e is DataException)
                        {
                            throw DbModelException.Create(obj, e as DataException);
                        }
                        if (e is System.Data.SqlClient.SqlException)
                        {
                            throw DbModelException.Create(obj, e as System.Data.SqlClient.SqlException);
                        }
                        else 
                            throw;
                    }
                }
                return bSuccess;
            }
            private bool _PostNonTransaction(DbManagerProxy manager, Patient obj, bool bChildObject) 
            { 
                bool bHasChanges = obj.HasChanges;
                if (!obj.IsNew && obj.IsMarkedToDelete) // delete
                {
            
                    if (obj.CurrentResidenceAddress != null)
                    {
                        obj.CurrentResidenceAddress.MarkToDelete();
                        if (!CurrentResidenceAddressAccessor.Post(manager, obj.CurrentResidenceAddress, true))
                            return false;
                    }
                                    
                    if (obj.EmployerAddress != null)
                    {
                        obj.EmployerAddress.MarkToDelete();
                        if (!EmployerAddressAccessor.Post(manager, obj.EmployerAddress, true))
                            return false;
                    }
                                    
                    if (obj.RegistrationAddress != null)
                    {
                        obj.RegistrationAddress.MarkToDelete();
                        if (!RegistrationAddressAccessor.Post(manager, obj.RegistrationAddress, true))
                            return false;
                    }
                                    
                    if (!ValidateCanDelete(manager, obj)) return false;
                                
                    _postDeletePredicate(manager
                        , obj.idfHuman
                        );
                                        
                }
                else if (!obj.IsMarkedToDelete) // insert/update
                {
                    if (!bChildObject)
                        if (!Validate(manager, obj, true, true, true)) 
                            return false;
                    
            
                    // posting extenters - begin
                obj.datModificationForArchiveDate = new Func<Patient, DateTime?>(c => c.HasChanges ? DateTime.Now : c.datModificationForArchiveDate)(obj);
                    obj.FixAddress();
                  
                    // posting extenters - end
            
                    if (obj.IsNew)
                    {
                        if (obj.CurrentResidenceAddress != null) // forced load potential lazy subobject for new object
                            if (!CurrentResidenceAddressAccessor.Post(manager, obj.CurrentResidenceAddress, true))
                                return false;
                    }
                    else
                    {
                        if (obj._CurrentResidenceAddress != null) // do not load lazy subobject for existing object
                            if (!CurrentResidenceAddressAccessor.Post(manager, obj.CurrentResidenceAddress, true))
                                return false;
                    }
                                    
                    if (obj.IsNew)
                    {
                        if (obj.EmployerAddress != null) // forced load potential lazy subobject for new object
                            if (!EmployerAddressAccessor.Post(manager, obj.EmployerAddress, true))
                                return false;
                    }
                    else
                    {
                        if (obj._EmployerAddress != null) // do not load lazy subobject for existing object
                            if (!EmployerAddressAccessor.Post(manager, obj.EmployerAddress, true))
                                return false;
                    }
                                    
                    if (obj.IsNew)
                    {
                        if (obj.RegistrationAddress != null) // forced load potential lazy subobject for new object
                            if (!RegistrationAddressAccessor.Post(manager, obj.RegistrationAddress, true))
                                return false;
                    }
                    else
                    {
                        if (obj._RegistrationAddress != null) // do not load lazy subobject for existing object
                            if (!RegistrationAddressAccessor.Post(manager, obj.RegistrationAddress, true))
                                return false;
                    }
                                    
                    if (!obj.IsMarkedToDelete && bHasChanges)
                        _postPredicate(manager, obj);
                                    
                    // posted extenters - begin
                    // posted extenters - end
            
                }
                //obj.AcceptChanges();
                
                return true;
            }
            
            public bool ValidateCanDelete(Patient obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    return ValidateCanDelete(manager, obj);
                }
            }
            public bool ValidateCanDelete(DbManagerProxy manager, Patient obj)
            {
            
                try
                {
                    if (!obj.IsForcedToDelete)
                    {
                        bool result = false;
                        _canDelete(manager
                            , obj.idfHuman
                            , out result
                            );
                        if (!result) 
                        {
                            throw new ValidationModelException("msgCantDelete", "_on_delete", "_on_delete", null, null, ValidationEventType.Error, obj);
                        }
                     }
                }
                catch(ValidationModelException ex)
                {
                    if (!obj.OnValidation(ex))
                    {
                        obj.OnValidationEnd(ex);
                        return false;
                    }
                    else
                        obj.m_IsForcedToDelete = true;
                }
            
                return true;
            }
        
      
            protected ValidationModelException ChainsValidate(Patient obj)
            {
                
                try
                {
                  
    new ChainsValidatorDateTime<Patient>(obj, "datDateofBirth", c => true, 
      obj.datDateofBirth,
      obj.GetType(),
      false, listdatDateofBirth => {
    listdatDateofBirth.Add(
    new ChainsValidatorDateTime<Patient>(obj, "CurrentDate", c => true, 
      DateTime.Now,
      null,
      false, listCurrentDate => {
    
    }));
  
    }).Process();
  
                }
                catch(ValidationModelException ex)
                {
                    return ex;
                }
                
                return null;
            }
            protected bool ChainsValidate(Patient obj, bool bRethrowException)
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
                return Validate(manager, obj as Patient, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, Patient obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                try
                {
                    if (bPostValidation)
                    {
                
                (new RequiredValidator( "strLastName", "strLastName","",
                ValidationEventType.Error
              )).Validate(c => !c.bSearchMode && !c.bPINMode, obj, obj.strLastName);
            
                (new RequiredValidator( "strPersonID", "strPersonID","",
                ValidationEventType.Error
              )).Validate(c => !c.bSearchMode && !c.bPINMode && c.idfsPersonIDType.HasValue && c.idfsPersonIDType.Value != (long)eidss.model.Enums.PersonalIDType.Unknown, obj, obj.strPersonID);
            
                (new RequiredValidator( "intPatientAgeFromCase", "intPatientAgeFromCase","",
                ValidationEventType.Error
              )).Validate(c => !c.bSearchMode && !c.bPINMode && (c.idfsHumanAgeTypeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_Age))), obj, obj.intPatientAgeFromCase);
            
                (new RequiredValidator( "idfsHumanAgeTypeFromCase", "idfsHumanAgeTypeFromCase","",
                ValidationEventType.Error
              )).Validate(c => !c.bSearchMode && !c.bPINMode && (c.intPatientAgeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_AgeType))), obj, obj.idfsHumanAgeTypeFromCase);
            
                (new RequiredValidator( "SearchMethod", "SearchMethod","",
                ValidationEventType.Error
              )).Validate(c => c.bSearchMode, obj, obj.SearchMethod);
            
                (new RequiredValidator( "DocumentType", "DocumentType","",
                ValidationEventType.Error
              )).Validate(c => c.bSearchMode && c.idfsSearchMethod == 2, obj, obj.DocumentType);
            
                (new RequiredValidator( "strDocumentNumber", "strDocumentNumber","",
                ValidationEventType.Error
              )).Validate(c => c.bSearchMode && c.idfsSearchMethod == 2, obj, obj.strDocumentNumber);
            
                (new RequiredValidator( "datDocumentDate", "datDocumentDate","",
                ValidationEventType.Error
              )).Validate(c => c.bSearchMode && c.idfsSearchMethod == 2 && c.idfsDocumentType == 6, obj, obj.datDocumentDate);
            
                  
                    }

                    if (bChangeValidation)
                    {
                
                (new PredicateValidator("mbAgeShallNotExceed", "intPatientAgeFromCase", "intPatientAgeFromCase", "intPatientAgeFromCase",
              new object[] {
              },
                        ValidationEventType.Error
                    )).Validate(obj, i => i.idfsHumanAgeTypeFromCase == null || i.intPatientAgeFromCase == null
                                            || (i.idfsHumanAgeTypeFromCase == (long)HumanAgeTypeEnum.Years && i.intPatientAgeFromCase <= 200)
                                            || (i.idfsHumanAgeTypeFromCase == (long)HumanAgeTypeEnum.Month && i.intPatientAgeFromCase <= 60)
                                            || (i.idfsHumanAgeTypeFromCase == (long)HumanAgeTypeEnum.Days && i.intPatientAgeFromCase <= 31)
                                            
                    );
            
                    }
                    
                    if (bDeepValidation)
                    {
                
                        if (obj.CurrentResidenceAddress != null)
                            CurrentResidenceAddressAccessor.Validate(manager, obj.CurrentResidenceAddress, bPostValidation, bChangeValidation, bDeepValidation, true);
                                        
                        if (obj.EmployerAddress != null)
                            EmployerAddressAccessor.Validate(manager, obj.EmployerAddress, bPostValidation, bChangeValidation, bDeepValidation, true);
                                        
                        if (obj.RegistrationAddress != null)
                            RegistrationAddressAccessor.Validate(manager, obj.RegistrationAddress, bPostValidation, bChangeValidation, bDeepValidation, true);
                                        
                    }
                }
                catch(ValidationModelException ex)
                {
                    if (bRethrowException)
                        throw;
                    if (!obj.OnValidation(ex))
                    {
                        obj.OnValidationEnd(ex);
                        return false;
                    }
                }
                
                return true;
            }
           
    
            private void _SetupRequired(Patient obj)
            {
            
                obj
                    .AddRequired("strLastName", c => !c.bSearchMode && !c.bPINMode);
                    
                obj
                    .AddRequired("strPersonID", c => !c.bSearchMode && !c.bPINMode && c.idfsPersonIDType.HasValue && c.idfsPersonIDType.Value != (long)eidss.model.Enums.PersonalIDType.Unknown);
                    
                obj
                    .AddRequired("intPatientAgeFromCase", c => !c.bSearchMode && !c.bPINMode && (c.idfsHumanAgeTypeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_Age))));
                    
                obj
                    .AddRequired("idfsHumanAgeTypeFromCase", c => !c.bSearchMode && !c.bPINMode && (c.intPatientAgeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_AgeType))));
                    
                  obj
                    .AddRequired("HumanAgeType", c => !c.bSearchMode && !c.bPINMode && (c.intPatientAgeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_AgeType))));
                
                obj
                    .AddRequired("SearchMethod", c => c.bSearchMode);
                    
                obj
                    .AddRequired("DocumentType", c => c.bSearchMode && c.idfsSearchMethod == 2);
                    
                obj
                    .AddRequired("strDocumentNumber", c => c.bSearchMode && c.idfsSearchMethod == 2);
                    
                obj
                    .AddRequired("datDocumentDate", c => c.bSearchMode && c.idfsSearchMethod == 2 && c.idfsDocumentType == 6);

                if (EidssSiteContext.Instance.IsGeorgiaCustomization)
                {
                    obj
                        .AddRequired("datDateofBirth", c => c.IsFirstCreatedAndGGPin);
                }

            }
    
    private void _SetupPersonalDataRestrictions(Patient obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as Patient) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as Patient) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "PatientDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return "HC_H04"; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spPatient_SelectDetail";
            public static string spCount = "";
            public static string spPost = "spPatient_Post";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "spPatientActual_Delete";
            public static string spCanDelete = "spPatientActual_CanDelete";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<Patient, bool>> RequiredByField = new Dictionary<string, Func<Patient, bool>>();
            public static Dictionary<string, Func<Patient, bool>> RequiredByProperty = new Dictionary<string, Func<Patient, bool>>();
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
                
                Sizes.Add(_str_strLastName, 200);
                Sizes.Add(_str_strSecondName, 200);
                Sizes.Add(_str_strFirstName, 200);
                Sizes.Add(_str_strRegistrationPhone, 200);
                Sizes.Add(_str_strEmployerName, 200);
                Sizes.Add(_str_strHomePhone, 200);
                Sizes.Add(_str_strWorkPhone, 200);
                Sizes.Add(_str_strPersonID, 100);
                if (!RequiredByField.ContainsKey("strLastName")) RequiredByField.Add("strLastName", c => !c.bSearchMode && !c.bPINMode);
                if (!RequiredByProperty.ContainsKey("strLastName")) RequiredByProperty.Add("strLastName", c => !c.bSearchMode && !c.bPINMode);
                
                if (!RequiredByField.ContainsKey("strPersonID")) RequiredByField.Add("strPersonID", c => !c.bSearchMode && !c.bPINMode && c.idfsPersonIDType.HasValue && c.idfsPersonIDType.Value != (long)eidss.model.Enums.PersonalIDType.Unknown);
                if (!RequiredByProperty.ContainsKey("strPersonID")) RequiredByProperty.Add("strPersonID", c => !c.bSearchMode && !c.bPINMode && c.idfsPersonIDType.HasValue && c.idfsPersonIDType.Value != (long)eidss.model.Enums.PersonalIDType.Unknown);
                
                if (!RequiredByField.ContainsKey("intPatientAgeFromCase")) RequiredByField.Add("intPatientAgeFromCase", c => !c.bSearchMode && !c.bPINMode && (c.idfsHumanAgeTypeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_Age))));
                if (!RequiredByProperty.ContainsKey("intPatientAgeFromCase")) RequiredByProperty.Add("intPatientAgeFromCase", c => !c.bSearchMode && !c.bPINMode && (c.idfsHumanAgeTypeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_Age))));
                
                if (!RequiredByField.ContainsKey("idfsHumanAgeTypeFromCase")) RequiredByField.Add("idfsHumanAgeTypeFromCase", c => !c.bSearchMode && !c.bPINMode && (c.intPatientAgeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_AgeType))));
                if (!RequiredByProperty.ContainsKey("idfsHumanAgeTypeFromCase")) RequiredByProperty.Add("idfsHumanAgeTypeFromCase", c => !c.bSearchMode && !c.bPINMode && (c.intPatientAgeFromCase.HasValue || (c.Parent != null && c.Parent is HumanCase && EidssSiteContext.Instance.CustomMandatoryFields.Contains(CustomMandatoryField.HumanCase_Patient_AgeType))));
                
                if (!RequiredByField.ContainsKey("SearchMethod")) RequiredByField.Add("SearchMethod", c => c.bSearchMode);
                if (!RequiredByProperty.ContainsKey("SearchMethod")) RequiredByProperty.Add("SearchMethod", c => c.bSearchMode);
                
                if (!RequiredByField.ContainsKey("DocumentType")) RequiredByField.Add("DocumentType", c => c.bSearchMode && c.idfsSearchMethod == 2);
                if (!RequiredByProperty.ContainsKey("DocumentType")) RequiredByProperty.Add("DocumentType", c => c.bSearchMode && c.idfsSearchMethod == 2);
                
                if (!RequiredByField.ContainsKey("strDocumentNumber")) RequiredByField.Add("strDocumentNumber", c => c.bSearchMode && c.idfsSearchMethod == 2);
                if (!RequiredByProperty.ContainsKey("strDocumentNumber")) RequiredByProperty.Add("strDocumentNumber", c => c.bSearchMode && c.idfsSearchMethod == 2);
                
                if (!RequiredByField.ContainsKey("datDocumentDate")) RequiredByField.Add("datDocumentDate", c => c.bSearchMode && c.idfsSearchMethod == 2 && c.idfsDocumentType == 6);
                if (!RequiredByProperty.ContainsKey("datDocumentDate")) RequiredByProperty.Add("datDocumentDate", c => c.bSearchMode && c.idfsSearchMethod == 2 && c.idfsDocumentType == 6);
                
                Actions.Add(new ActionMetaItem(
                    "Create",
                    ActionTypes.Create,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => new ActResult(true, Accessor.Instance(null).Create(manager, c, pars)),
                        
                    null,
                    
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        ActionsAlignment.Left,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                        ),
                      false,
                    null,
                    null,
                    null,
                    null,
                    (c,a,p,r) => c==null,
                    false,
                    false,
                    null,
                    false,
                    new ActionMetaItem[] {
                      
                      }
                    
                    ));
                  
                Actions.Add(new ActionMetaItem(
                    "Save",
                    ActionTypes.Save,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<Patient>().Post(manager, (Patient)c), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strSave_Id",
                        "Save",
                        /*from BvMessages*/"tooltipSave_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipSave_Id",
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
                    "Ok",
                    ActionTypes.Ok,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<Patient>().Post(manager, (Patient)c), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strOK_Id",
                        "",
                        /*from BvMessages*/"tooltipOK_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipOK_Id",
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
                    "Cancel",
                    ActionTypes.Cancel,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strCancel_Id",
                        "",
                        /*from BvMessages*/"tooltipCancel_Id",
                        /*from BvMessages*/"strOK_Id",
                        "",
                        /*from BvMessages*/"tooltipCancel_Id",
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
                    "Delete",
                    ActionTypes.Delete,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(((Patient)c).MarkToDelete() && ObjectAccessor.PostInterface<Patient>().Post(manager, (Patient)c), c),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strDelete_Id",
                        "Delete_Remove",
                        /*from BvMessages*/"tooltipDelete_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipDelete_Id",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                      ),
                      false,
                      null,
                      null,
                      (o, p, r) => r && !o.IsNew && !o.HasChanges,
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
	