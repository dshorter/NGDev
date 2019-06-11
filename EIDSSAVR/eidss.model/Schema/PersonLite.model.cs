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
    public abstract partial class PersonLite : 
        EditableObject<PersonLite>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfPerson), NonUpdatable, PrimaryKey]
        public abstract Int64 idfPerson { get; set; }
                
        [LocalizedDisplayName(_str_idfsStaffPosition)]
        [MapField(_str_idfsStaffPosition)]
        public abstract Int64? idfsStaffPosition { get; set; }
        protected Int64? idfsStaffPosition_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsStaffPosition).OriginalValue; } }
        protected Int64? idfsStaffPosition_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsStaffPosition).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfInstitution)]
        [MapField(_str_idfInstitution)]
        public abstract Int64? idfInstitution { get; set; }
        protected Int64? idfInstitution_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfInstitution).OriginalValue; } }
        protected Int64? idfInstitution_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfInstitution).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfDepartment)]
        [MapField(_str_idfDepartment)]
        public abstract Int64? idfDepartment { get; set; }
        protected Int64? idfDepartment_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfDepartment).OriginalValue; } }
        protected Int64? idfDepartment_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfDepartment).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strFamilyName)]
        [MapField(_str_strFamilyName)]
        public abstract String strFamilyName { get; set; }
        protected String strFamilyName_Original { get { return ((EditableValue<String>)((dynamic)this)._strFamilyName).OriginalValue; } }
        protected String strFamilyName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strFamilyName).PreviousValue; } }
                
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
                
        [LocalizedDisplayName(_str_strContactPhone)]
        [MapField(_str_strContactPhone)]
        public abstract String strContactPhone { get; set; }
        protected String strContactPhone_Original { get { return ((EditableValue<String>)((dynamic)this)._strContactPhone).OriginalValue; } }
        protected String strContactPhone_Previous { get { return ((EditableValue<String>)((dynamic)this)._strContactPhone).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strBarcode)]
        [MapField(_str_strBarcode)]
        public abstract String strBarcode { get; set; }
        protected String strBarcode_Original { get { return ((EditableValue<String>)((dynamic)this)._strBarcode).OriginalValue; } }
        protected String strBarcode_Previous { get { return ((EditableValue<String>)((dynamic)this)._strBarcode).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsSite)]
        [MapField(_str_idfsSite)]
        public abstract Int64 idfsSite { get; set; }
        protected Int64 idfsSite_Original { get { return ((EditableValue<Int64>)((dynamic)this)._idfsSite).OriginalValue; } }
        protected Int64 idfsSite_Previous { get { return ((EditableValue<Int64>)((dynamic)this)._idfsSite).PreviousValue; } }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<PersonLite, object> _get_func;
            internal Action<PersonLite, string> _set_func;
            internal Action<PersonLite, PersonLite, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfPerson = "idfPerson";
        internal const string _str_idfsStaffPosition = "idfsStaffPosition";
        internal const string _str_idfInstitution = "idfInstitution";
        internal const string _str_idfDepartment = "idfDepartment";
        internal const string _str_strFamilyName = "strFamilyName";
        internal const string _str_strFirstName = "strFirstName";
        internal const string _str_strSecondName = "strSecondName";
        internal const string _str_strContactPhone = "strContactPhone";
        internal const string _str_strBarcode = "strBarcode";
        internal const string _str_idfsSite = "idfsSite";
        internal const string _str_idfsOASite = "idfsOASite";
        internal const string _str_strInstitutionName = "strInstitutionName";
        internal const string _str_strPositionName = "strPositionName";
        internal const string _str_HideDeleteAction = "HideDeleteAction";
        internal const string _str_Institution = "Institution";
        internal const string _str_Department = "Department";
        internal const string _str_StaffPosition = "StaffPosition";
        internal const string _str_Site = "Site";
        internal const string _str_LoginInfoList = "LoginInfoList";
        internal const string _str_GroupInfoList = "GroupInfoList";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfPerson, _formname = _str_idfPerson, _type = "Int64",
              _get_func = o => o.idfPerson,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfPerson != newval) o.idfPerson = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfPerson != c.idfPerson || o.IsRIRPropChanged(_str_idfPerson, c)) 
                  m.Add(_str_idfPerson, o.ObjectIdent + _str_idfPerson, o.ObjectIdent2 + _str_idfPerson, o.ObjectIdent3 + _str_idfPerson, "Int64", 
                    o.idfPerson == null ? "" : o.idfPerson.ToString(),                  
                  o.IsReadOnly(_str_idfPerson), o.IsInvisible(_str_idfPerson), o.IsRequired(_str_idfPerson)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsStaffPosition, _formname = _str_idfsStaffPosition, _type = "Int64?",
              _get_func = o => o.idfsStaffPosition,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfsStaffPosition != newval) 
                  o.StaffPosition = o.StaffPositionLookup.FirstOrDefault(c => c.idfsBaseReference == newval);
                if (o.idfsStaffPosition != newval) o.idfsStaffPosition = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsStaffPosition != c.idfsStaffPosition || o.IsRIRPropChanged(_str_idfsStaffPosition, c)) 
                  m.Add(_str_idfsStaffPosition, o.ObjectIdent + _str_idfsStaffPosition, o.ObjectIdent2 + _str_idfsStaffPosition, o.ObjectIdent3 + _str_idfsStaffPosition, "Int64?", 
                    o.idfsStaffPosition == null ? "" : o.idfsStaffPosition.ToString(),                  
                  o.IsReadOnly(_str_idfsStaffPosition), o.IsInvisible(_str_idfsStaffPosition), o.IsRequired(_str_idfsStaffPosition)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfInstitution, _formname = _str_idfInstitution, _type = "Int64?",
              _get_func = o => o.idfInstitution,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfInstitution != newval) 
                  o.Institution = o.InstitutionLookup.FirstOrDefault(c => c.idfInstitution == newval);
                if (o.idfInstitution != newval) o.idfInstitution = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfInstitution != c.idfInstitution || o.IsRIRPropChanged(_str_idfInstitution, c)) 
                  m.Add(_str_idfInstitution, o.ObjectIdent + _str_idfInstitution, o.ObjectIdent2 + _str_idfInstitution, o.ObjectIdent3 + _str_idfInstitution, "Int64?", 
                    o.idfInstitution == null ? "" : o.idfInstitution.ToString(),                  
                  o.IsReadOnly(_str_idfInstitution), o.IsInvisible(_str_idfInstitution), o.IsRequired(_str_idfInstitution)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfDepartment, _formname = _str_idfDepartment, _type = "Int64?",
              _get_func = o => o.idfDepartment,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); 
                if (o.idfDepartment != newval) 
                  o.Department = o.DepartmentLookup.FirstOrDefault(c => c.idfInstitution == newval);
                if (o.idfDepartment != newval) o.idfDepartment = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfDepartment != c.idfDepartment || o.IsRIRPropChanged(_str_idfDepartment, c)) 
                  m.Add(_str_idfDepartment, o.ObjectIdent + _str_idfDepartment, o.ObjectIdent2 + _str_idfDepartment, o.ObjectIdent3 + _str_idfDepartment, "Int64?", 
                    o.idfDepartment == null ? "" : o.idfDepartment.ToString(),                  
                  o.IsReadOnly(_str_idfDepartment), o.IsInvisible(_str_idfDepartment), o.IsRequired(_str_idfDepartment)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strFamilyName, _formname = _str_strFamilyName, _type = "String",
              _get_func = o => o.strFamilyName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strFamilyName != newval) o.strFamilyName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strFamilyName != c.strFamilyName || o.IsRIRPropChanged(_str_strFamilyName, c)) 
                  m.Add(_str_strFamilyName, o.ObjectIdent + _str_strFamilyName, o.ObjectIdent2 + _str_strFamilyName, o.ObjectIdent3 + _str_strFamilyName, "String", 
                    o.strFamilyName == null ? "" : o.strFamilyName.ToString(),                  
                  o.IsReadOnly(_str_strFamilyName), o.IsInvisible(_str_strFamilyName), o.IsRequired(_str_strFamilyName)); 
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
              _name = _str_strContactPhone, _formname = _str_strContactPhone, _type = "String",
              _get_func = o => o.strContactPhone,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strContactPhone != newval) o.strContactPhone = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strContactPhone != c.strContactPhone || o.IsRIRPropChanged(_str_strContactPhone, c)) 
                  m.Add(_str_strContactPhone, o.ObjectIdent + _str_strContactPhone, o.ObjectIdent2 + _str_strContactPhone, o.ObjectIdent3 + _str_strContactPhone, "String", 
                    o.strContactPhone == null ? "" : o.strContactPhone.ToString(),                  
                  o.IsReadOnly(_str_strContactPhone), o.IsInvisible(_str_strContactPhone), o.IsRequired(_str_strContactPhone)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strBarcode, _formname = _str_strBarcode, _type = "String",
              _get_func = o => o.strBarcode,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strBarcode != newval) o.strBarcode = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strBarcode != c.strBarcode || o.IsRIRPropChanged(_str_strBarcode, c)) 
                  m.Add(_str_strBarcode, o.ObjectIdent + _str_strBarcode, o.ObjectIdent2 + _str_strBarcode, o.ObjectIdent3 + _str_strBarcode, "String", 
                    o.strBarcode == null ? "" : o.strBarcode.ToString(),                  
                  o.IsReadOnly(_str_strBarcode), o.IsInvisible(_str_strBarcode), o.IsRequired(_str_strBarcode)); 
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
              _name = _str_idfsOASite, _formname = _str_idfsOASite, _type = "long?",
              _get_func = o => o.idfsOASite,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsOASite != newval) o.idfsOASite = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.idfsOASite != c.idfsOASite || o.IsRIRPropChanged(_str_idfsOASite, c)) {
                  m.Add(_str_idfsOASite, o.ObjectIdent + _str_idfsOASite, o.ObjectIdent2 + _str_idfsOASite, o.ObjectIdent3 + _str_idfsOASite,  "long?", 
                    o.idfsOASite == null ? "" : o.idfsOASite.ToString(),                  
                    o.IsReadOnly(_str_idfsOASite), o.IsInvisible(_str_idfsOASite), o.IsRequired(_str_idfsOASite));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_HideDeleteAction, _formname = _str_HideDeleteAction, _type = "bool",
              _get_func = o => o.HideDeleteAction,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBoolean(val); if (o.HideDeleteAction != newval) o.HideDeleteAction = newval; },
              _compare_func = (o, c, m, g) => {
              
                if (o.HideDeleteAction != c.HideDeleteAction || o.IsRIRPropChanged(_str_HideDeleteAction, c)) {
                  m.Add(_str_HideDeleteAction, o.ObjectIdent + _str_HideDeleteAction, o.ObjectIdent2 + _str_HideDeleteAction, o.ObjectIdent3 + _str_HideDeleteAction,  "bool", 
                    o.HideDeleteAction == null ? "" : o.HideDeleteAction.ToString(),                  
                    o.IsReadOnly(_str_HideDeleteAction), o.IsInvisible(_str_HideDeleteAction), o.IsRequired(_str_HideDeleteAction));
                  }
                 }
              }, 
        
            new field_info {
              _name = _str_strInstitutionName, _formname = _str_strInstitutionName, _type = "string",
              _get_func = o => o.strInstitutionName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strInstitutionName != c.strInstitutionName || o.IsRIRPropChanged(_str_strInstitutionName, c)) {
                  m.Add(_str_strInstitutionName, o.ObjectIdent + _str_strInstitutionName, o.ObjectIdent2 + _str_strInstitutionName, o.ObjectIdent3 + _str_strInstitutionName, "string", o.strInstitutionName == null ? "" : o.strInstitutionName.ToString(), o.IsReadOnly(_str_strInstitutionName), o.IsInvisible(_str_strInstitutionName), o.IsRequired(_str_strInstitutionName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_strPositionName, _formname = _str_strPositionName, _type = "string",
              _get_func = o => o.strPositionName,
              _set_func = (o, val) => {  },
              _compare_func = (o, c, m, g) => { 
              
                if (o.strPositionName != c.strPositionName || o.IsRIRPropChanged(_str_strPositionName, c)) {
                  m.Add(_str_strPositionName, o.ObjectIdent + _str_strPositionName, o.ObjectIdent2 + _str_strPositionName, o.ObjectIdent3 + _str_strPositionName, "string", o.strPositionName == null ? "" : o.strPositionName.ToString(), o.IsReadOnly(_str_strPositionName), o.IsInvisible(_str_strPositionName), o.IsRequired(_str_strPositionName));
                  }
                
                }
              }, 
        
            new field_info {
              _name = _str_Institution, _formname = _str_Institution, _type = "Lookup",
              _get_func = o => { if (o.Institution == null) return null; return o.Institution.idfInstitution; },
              _set_func = (o, val) => { o.Institution = o.InstitutionLookup.Where(c => c.idfInstitution.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_Institution, c);
                if (o.idfInstitution != c.idfInstitution || o.IsRIRPropChanged(_str_Institution, c) || bChangeLookupContent) {
                  m.Add(_str_Institution, o.ObjectIdent + _str_Institution, o.ObjectIdent2 + _str_Institution, o.ObjectIdent3 + _str_Institution, "Lookup", o.idfInstitution == null ? "" : o.idfInstitution.ToString(), o.IsReadOnly(_str_Institution), o.IsInvisible(_str_Institution), o.IsRequired(_str_Institution),
                  bChangeLookupContent ? o.InstitutionLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_Institution + "Lookup", _formname = _str_Institution + "Lookup", _type = "LookupContent",
              _get_func = o => o.InstitutionLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_Department, _formname = _str_Department, _type = "Lookup",
              _get_func = o => { if (o.Department == null) return null; return o.Department.idfInstitution; },
              _set_func = (o, val) => { o.Department = o.DepartmentLookup.Where(c => c.idfInstitution.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_Department, c);
                if (o.idfDepartment != c.idfDepartment || o.IsRIRPropChanged(_str_Department, c) || bChangeLookupContent) {
                  m.Add(_str_Department, o.ObjectIdent + _str_Department, o.ObjectIdent2 + _str_Department, o.ObjectIdent3 + _str_Department, "Lookup", o.idfDepartment == null ? "" : o.idfDepartment.ToString(), o.IsReadOnly(_str_Department), o.IsInvisible(_str_Department), o.IsRequired(_str_Department),
                  bChangeLookupContent ? o.DepartmentLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_Department + "Lookup", _formname = _str_Department + "Lookup", _type = "LookupContent",
              _get_func = o => o.DepartmentLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_StaffPosition, _formname = _str_StaffPosition, _type = "Lookup",
              _get_func = o => { if (o.StaffPosition == null) return null; return o.StaffPosition.idfsBaseReference; },
              _set_func = (o, val) => { o.StaffPosition = o.StaffPositionLookup.Where(c => c.idfsBaseReference.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_StaffPosition, c);
                if (o.idfsStaffPosition != c.idfsStaffPosition || o.IsRIRPropChanged(_str_StaffPosition, c) || bChangeLookupContent) {
                  m.Add(_str_StaffPosition, o.ObjectIdent + _str_StaffPosition, o.ObjectIdent2 + _str_StaffPosition, o.ObjectIdent3 + _str_StaffPosition, "Lookup", o.idfsStaffPosition == null ? "" : o.idfsStaffPosition.ToString(), o.IsReadOnly(_str_StaffPosition), o.IsInvisible(_str_StaffPosition), o.IsRequired(_str_StaffPosition),
                  bChangeLookupContent ? o.StaffPositionLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_StaffPosition + "Lookup", _formname = _str_StaffPosition + "Lookup", _type = "LookupContent",
              _get_func = o => o.StaffPositionLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_Site, _formname = _str_Site, _type = "Lookup",
              _get_func = o => { if (o.Site == null) return null; return o.Site.idfsSite; },
              _set_func = (o, val) => { o.Site = o.SiteLookup.Where(c => c.idfsSite.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_Site, c);
                if (o.idfsOASite != c.idfsOASite || o.IsRIRPropChanged(_str_Site, c) || bChangeLookupContent) {
                  m.Add(_str_Site, o.ObjectIdent + _str_Site, o.ObjectIdent2 + _str_Site, o.ObjectIdent3 + _str_Site, "Lookup", o.idfsOASite == null ? "" : o.idfsOASite.ToString(), o.IsReadOnly(_str_Site), o.IsInvisible(_str_Site), o.IsRequired(_str_Site),
                  bChangeLookupContent ? o.SiteLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_Site + "Lookup", _formname = _str_Site + "Lookup", _type = "LookupContent",
              _get_func = o => o.SiteLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_LoginInfoList, _formname = _str_LoginInfoList, _type = "Child",
              _get_func = o => null,
              _set_func = (o, val) => {
                  o.LoginInfoList.ForEach(c => c.SetValue("blnChecked", "false")); 
                  if (!string.IsNullOrEmpty(val)) 
                      val.Split(',').ToList().ForEach(i => o.LoginInfoList.First(c => (long)c.Key == Int64.Parse(i)).SetValue("blnChecked", "true"));
                  },
              _compare_func = (o, c, m, g) => {
                if (o.LoginInfoList.Count != c.LoginInfoList.Count || o.IsReadOnly(_str_LoginInfoList) != c.IsReadOnly(_str_LoginInfoList) || o.IsInvisible(_str_LoginInfoList) != c.IsInvisible(_str_LoginInfoList) || o.IsRequired(_str_LoginInfoList) != c._isRequired(o.m_isRequired, _str_LoginInfoList)) {
                  m.Add(_str_LoginInfoList, o.ObjectIdent + _str_LoginInfoList, o.ObjectIdent2 + _str_LoginInfoList, o.ObjectIdent3 + _str_LoginInfoList, "Child", o.idfPerson == null ? "" : o.idfPerson.ToString(), o.IsReadOnly(_str_LoginInfoList), o.IsInvisible(_str_LoginInfoList), o.IsRequired(_str_LoginInfoList)); 
                  }
                }
              }, 
        
            new field_info {
              _name = _str_GroupInfoList, _formname = _str_GroupInfoList, _type = "Child",
              _get_func = o => null,
              _set_func = (o, val) => {
                  o.GroupInfoList.ForEach(c => c.SetValue("blnChecked", "false")); 
                  if (!string.IsNullOrEmpty(val)) 
                      val.Split(',').ToList().ForEach(i => o.GroupInfoList.First(c => (long)c.Key == Int64.Parse(i)).SetValue("blnChecked", "true"));
                  },
              _compare_func = (o, c, m, g) => {
                if (o.GroupInfoList.Count != c.GroupInfoList.Count || o.IsReadOnly(_str_GroupInfoList) != c.IsReadOnly(_str_GroupInfoList) || o.IsInvisible(_str_GroupInfoList) != c.IsInvisible(_str_GroupInfoList) || o.IsRequired(_str_GroupInfoList) != c._isRequired(o.m_isRequired, _str_GroupInfoList)) {
                  m.Add(_str_GroupInfoList, o.ObjectIdent + _str_GroupInfoList, o.ObjectIdent2 + _str_GroupInfoList, o.ObjectIdent3 + _str_GroupInfoList, "Child", o.idfPerson == null ? "" : o.idfPerson.ToString(), o.IsReadOnly(_str_GroupInfoList), o.IsInvisible(_str_GroupInfoList), o.IsRequired(_str_GroupInfoList)); 
                  }
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
            PersonLite obj = (PersonLite)o;
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                Accessor.Instance(null)._LoadLookups(manager, obj);
                foreach (var i in _field_infos)
                    if (i != null && i._compare_func != null) i._compare_func(this, obj, ret, manager);
            }
            return ret;
        }
        #endregion
    
        [LocalizedDisplayName(_str_LoginInfoList)]
        [Relation(typeof(LoginInfo), eidss.model.Schema.LoginInfo._str_idfPerson, _str_idfPerson)]
        public EditableList<LoginInfo> LoginInfoList
        {
            get 
            {   
                return _LoginInfoList; 
            }
            set 
            {
                _LoginInfoList = value;
            }
        }
        protected EditableList<LoginInfo> _LoginInfoList = new EditableList<LoginInfo>();
                    
        [LocalizedDisplayName(_str_GroupInfoList)]
        [Relation(typeof(PersonLiteGroupInfo), eidss.model.Schema.PersonLiteGroupInfo._str_idfEmployee, _str_idfPerson)]
        public EditableList<PersonLiteGroupInfo> GroupInfoList
        {
            get 
            {   
                return _GroupInfoList; 
            }
            set 
            {
                _GroupInfoList = value;
            }
        }
        protected EditableList<PersonLiteGroupInfo> _GroupInfoList = new EditableList<PersonLiteGroupInfo>();
                    
        [LocalizedDisplayName(_str_Institution)]
        [Relation(typeof(OrganizationLookup), eidss.model.Schema.OrganizationLookup._str_idfInstitution, _str_idfInstitution)]
        public OrganizationLookup Institution
        {
            get { return _Institution == null ? null : ((long)_Institution.Key == 0 ? null : _Institution); }
            set 
            { 
                var oldVal = _Institution;
                _Institution = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_Institution != oldVal)
                {
                    if (idfInstitution != (_Institution == null
                            ? new Int64?()
                            : (Int64?)_Institution.idfInstitution))
                        idfInstitution = _Institution == null 
                            ? new Int64?()
                            : (Int64?)_Institution.idfInstitution; 
                    OnPropertyChanged(_str_Institution); 
                }
            }
        }
        private OrganizationLookup _Institution;

        
        public List<OrganizationLookup> InstitutionLookup
        {
            get { return _InstitutionLookup; }
        }
        private List<OrganizationLookup> _InstitutionLookup = new List<OrganizationLookup>();
            
        [LocalizedDisplayName(_str_Department)]
        [Relation(typeof(DepartmentLookup), eidss.model.Schema.DepartmentLookup._str_idfInstitution, _str_idfDepartment)]
        public DepartmentLookup Department
        {
            get { return _Department == null ? null : ((long)_Department.Key == 0 ? null : _Department); }
            set 
            { 
                var oldVal = _Department;
                _Department = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_Department != oldVal)
                {
                    if (idfDepartment != (_Department == null
                            ? new Int64?()
                            : (Int64?)_Department.idfInstitution))
                        idfDepartment = _Department == null 
                            ? new Int64?()
                            : (Int64?)_Department.idfInstitution; 
                    OnPropertyChanged(_str_Department); 
                }
            }
        }
        private DepartmentLookup _Department;

        
        public List<DepartmentLookup> DepartmentLookup
        {
            get { return _DepartmentLookup; }
        }
        private List<DepartmentLookup> _DepartmentLookup = new List<DepartmentLookup>();
            
        [LocalizedDisplayName(_str_StaffPosition)]
        [Relation(typeof(BaseReference), eidss.model.Schema.BaseReference._str_idfsBaseReference, _str_idfsStaffPosition)]
        public BaseReference StaffPosition
        {
            get { return _StaffPosition == null ? null : ((long)_StaffPosition.Key == 0 ? null : _StaffPosition); }
            set 
            { 
                var oldVal = _StaffPosition;
                _StaffPosition = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_StaffPosition != oldVal)
                {
                    if (idfsStaffPosition != (_StaffPosition == null
                            ? new Int64?()
                            : (Int64?)_StaffPosition.idfsBaseReference))
                        idfsStaffPosition = _StaffPosition == null 
                            ? new Int64?()
                            : (Int64?)_StaffPosition.idfsBaseReference; 
                    OnPropertyChanged(_str_StaffPosition); 
                }
            }
        }
        private BaseReference _StaffPosition;

        
        public BaseReferenceList StaffPositionLookup
        {
            get { return _StaffPositionLookup; }
        }
        private BaseReferenceList _StaffPositionLookup = new BaseReferenceList("rftPosition");
            
        [LocalizedDisplayName(_str_Site)]
        [Relation(typeof(SiteLookup), eidss.model.Schema.SiteLookup._str_idfsSite, _str_idfsOASite)]
        public SiteLookup Site
        {
            get { return _Site == null ? null : ((long)_Site.Key == 0 ? null : _Site); }
            set 
            { 
                var oldVal = _Site;
                _Site = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_Site != oldVal)
                {
                    if (idfsOASite != (_Site == null
                            ? new long?()
                            : _Site.idfsSite))
                        idfsOASite = _Site == null 
                            ? new long?()
                            : _Site.idfsSite; 
                    OnPropertyChanged(_str_Site); 
                }
            }
        }
        private SiteLookup _Site;

        
        public List<SiteLookup> SiteLookup
        {
            get { return _SiteLookup; }
        }
        private List<SiteLookup> _SiteLookup = new List<SiteLookup>();
            
        private BvSelectList _getList(string name)
        {
        
            switch(name)
            {
            
                case _str_Institution:
                    return new BvSelectList(InstitutionLookup, eidss.model.Schema.OrganizationLookup._str_idfInstitution, null, Institution, _str_idfInstitution);
            
                case _str_Department:
                    return new BvSelectList(DepartmentLookup, eidss.model.Schema.DepartmentLookup._str_idfInstitution, null, Department, _str_idfDepartment);
            
                case _str_StaffPosition:
                    return new BvSelectList(StaffPositionLookup, eidss.model.Schema.BaseReference._str_idfsBaseReference, null, StaffPosition, _str_idfsStaffPosition);
            
                case _str_Site:
                    return new BvSelectList(SiteLookup, eidss.model.Schema.SiteLookup._str_idfsSite, null, Site, _str_idfsOASite);
            
                case _str_LoginInfoList:
                    return new BvSelectList(LoginInfoList, "", "", null, "");
            
                case _str_GroupInfoList:
                    return new BvSelectList(GroupInfoList, "", "", null, "");
            
            }
        
            return null;
        }
    
          [XmlIgnore]
          [LocalizedDisplayName(_str_strInstitutionName)]
        public string strInstitutionName
        {
            get { return new Func<PersonLite, string>(c => c.idfInstitution == null ? (string)null : c.Institution.name)(this); }
            
        }
        
          [XmlIgnore]
          [LocalizedDisplayName(_str_strPositionName)]
        public string strPositionName
        {
            get { return new Func<PersonLite, string>(c => c.idfsStaffPosition == null ? (string)null : c.StaffPosition.name)(this); }
            
        }
        
          [LocalizedDisplayName(_str_idfsOASite)]
        public long? idfsOASite
        {
            get { return m_idfsOASite; }
            set { if (m_idfsOASite != value) { m_idfsOASite = value; OnPropertyChanged(_str_idfsOASite); } }
        }
        private long? m_idfsOASite;
        
          [LocalizedDisplayName(_str_HideDeleteAction)]
        public bool HideDeleteAction
        {
            get { return m_HideDeleteAction; }
            set { if (m_HideDeleteAction != value) { m_HideDeleteAction = value; OnPropertyChanged(_str_HideDeleteAction); } }
        }
        private bool m_HideDeleteAction;
        
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "PersonLite";

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
        LoginInfoList.ForEach(c => { c.Parent = this; });
                GroupInfoList.ForEach(c => { c.Parent = this; });
                
        }
        partial void Cloned();
        partial void ClonedWithSetup();
        private bool bIsClone;
        public override object Clone()
        {
            var ret = base.Clone() as PersonLite;
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
            var ret = base.Clone() as PersonLite;
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        
            if (_LoginInfoList != null && _LoginInfoList.Count > 0)
            {
              ret.LoginInfoList.Clear();
              _LoginInfoList.ForEach(c => ret.LoginInfoList.Add(c.CloneWithSetup(manager, bRestricted)));
            }
                
            if (_GroupInfoList != null && _GroupInfoList.Count > 0)
            {
              ret.GroupInfoList.Clear();
              _GroupInfoList.ForEach(c => ret.GroupInfoList.Add(c.CloneWithSetup(manager, bRestricted)));
            }
                
            Accessor.Instance(null)._SetupLoad(manager, ret, true);
            ret.ClonedWithSetup();
            ret.DeepAcceptChanges();
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public PersonLite CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as PersonLite;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfPerson; } }
        public string KeyName { get { return "idfPerson"; } }
        public object KeyLookup { get { return idfPerson; } }
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
        
                    || LoginInfoList.IsDirty
                    || LoginInfoList.Count(c => c.IsMarkedToDelete || c.HasChanges) > 0
                
                    || GroupInfoList.IsDirty
                    || GroupInfoList.Count(c => c.IsMarkedToDelete || c.HasChanges) > 0
                
                ;
            }
        }
        public new void RejectChanges()
        {
        
            var _prev_idfInstitution_Institution = idfInstitution;
            var _prev_idfDepartment_Department = idfDepartment;
            var _prev_idfsStaffPosition_StaffPosition = idfsStaffPosition;
            var _prev_idfsOASite_Site = idfsOASite;
            base.RejectChanges();
        
            if (_prev_idfInstitution_Institution != idfInstitution)
            {
                _Institution = _InstitutionLookup.FirstOrDefault(c => c.idfInstitution == idfInstitution);
            }
            if (_prev_idfDepartment_Department != idfDepartment)
            {
                _Department = _DepartmentLookup.FirstOrDefault(c => c.idfInstitution == idfDepartment);
            }
            if (_prev_idfsStaffPosition_StaffPosition != idfsStaffPosition)
            {
                _StaffPosition = _StaffPositionLookup.FirstOrDefault(c => c.idfsBaseReference == idfsStaffPosition);
            }
            if (_prev_idfsOASite_Site != idfsOASite)
            {
                _Site = _SiteLookup.FirstOrDefault(c => c.idfsSite == idfsOASite);
            }
        }
        public void DeepRejectChanges()
        {
            RejectChanges();
        LoginInfoList.DeepRejectChanges();
                GroupInfoList.DeepRejectChanges();
                
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        LoginInfoList.DeepAcceptChanges();
                GroupInfoList.DeepAcceptChanges();
                
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
        LoginInfoList.ForEach(c => c.SetChange());
                GroupInfoList.ForEach(c => c.SetChange());
                
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

        private bool IsRIRPropChanged(string fld, PersonLite c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, PersonLite c)
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
        

      

        public PersonLite()
        {
            
            m_permissions = new Permissions(this);
            
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PersonLite_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(PersonLite_PropertyChanged);
        }
        private void PersonLite_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as PersonLite).Changed(e.PropertyName);
            
            if (e.PropertyName == _str_Institution)
                OnPropertyChanged(_str_strInstitutionName);
                  
            if (e.PropertyName == _str_StaffPosition)
                OnPropertyChanged(_str_strPositionName);
                  
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
            PersonLite obj = this;
            
        }
        private void _DeletedExtenders()
        {
            PersonLite obj = this;
            
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

    
        private static string[] readonly_names1 = "strInstitutionName,idfsSite".Split(new char[] { ',' });
        
        private bool _isReadOnly(string name)
        {
            
            if (readonly_names1.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<PersonLite, bool>(c => true)(this);
            
            return ReadOnly;
                
        }

        private bool m_isValid = true;
        internal bool _isValid
        {
            get { return m_isValid; }
            set
            {
                m_isValid = value;
        
                foreach(var o in _LoginInfoList)
                    o._isValid &= value;
                
                foreach(var o in _GroupInfoList)
                    o._isValid &= value;
                
            }
        }

        private bool m_readOnly;
        private bool _readOnly
        {
            get { return m_readOnly; }
            set
            {
                m_readOnly = value;
        
                foreach(var o in _LoginInfoList)
                    o.ReadOnly |= value;
                
                foreach(var o in _GroupInfoList)
                    o.ReadOnly |= value;
                
            }
        }


        internal Dictionary<string, Func<PersonLite, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<PersonLite, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<PersonLite, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<PersonLite, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<PersonLite, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<PersonLite, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<PersonLite, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~PersonLite()
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
                
                if (!bIsClone)
                {
                    LoginInfoList.ForEach(c => c.Dispose());
                }
                LoginInfoList.ClearModelListEventInvocations();
                
                if (!bIsClone)
                {
                    GroupInfoList.ForEach(c => c.Dispose());
                }
                GroupInfoList.ClearModelListEventInvocations();
                
                
                if (bNeedLookupRemove)
                {
                
                LookupManager.RemoveObject("OrganizationLookup", this);
                
                LookupManager.RemoveObject("DepartmentLookup", this);
                
                LookupManager.RemoveObject("rftPosition", this);
                
                LookupManager.RemoveObject("SiteLookup", this);
                
                }
                Disposed();
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    
        #region ILookupUsage Members
        public void ReloadLookupItem(DbManagerProxy manager, string lookup_object)
        {
            
            if (lookup_object == "OrganizationLookup")
                _getAccessor().LoadLookup_Institution(manager, this);
            
            if (lookup_object == "DepartmentLookup")
                _getAccessor().LoadLookup_Department(manager, this);
            
            if (lookup_object == "rftPosition")
                _getAccessor().LoadLookup_StaffPosition(manager, this);
            
            if (lookup_object == "SiteLookup")
                _getAccessor().LoadLookup_Site(manager, this);
            
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
        
            if (_LoginInfoList != null) _LoginInfoList.ForEach(c => c.ParseFormCollection(form, bParseLookups, bParseRelations));
                
            if (_GroupInfoList != null) _GroupInfoList.ForEach(c => c.ParseFormCollection(form, bParseLookups, bParseRelations));
                
            }
            ParsedFormCollection(form);
        }
    

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<PersonLite>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectPermissions
            
            , IObjectCreator
            , IObjectCreator<PersonLite>
            
            , IObjectSelectDetail
            , IObjectSelectDetail<PersonLite>
            , IObjectPost
            , IObjectDelete
                    
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfPerson"; } }
            #endregion
        
            public delegate void on_action(PersonLite obj);
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
            private LoginInfo.Accessor LoginInfoListAccessor { get { return eidss.model.Schema.LoginInfo.Accessor.Instance(m_CS); } }
            private PersonLiteGroupInfo.Accessor GroupInfoListAccessor { get { return eidss.model.Schema.PersonLiteGroupInfo.Accessor.Instance(m_CS); } }
            private OrganizationLookup.Accessor InstitutionAccessor { get { return eidss.model.Schema.OrganizationLookup.Accessor.Instance(m_CS); } }
            private DepartmentLookup.Accessor DepartmentAccessor { get { return eidss.model.Schema.DepartmentLookup.Accessor.Instance(m_CS); } }
            private BaseReference.Accessor StaffPositionAccessor { get { return eidss.model.Schema.BaseReference.Accessor.Instance(m_CS); } }
            private SiteLookup.Accessor SiteAccessor { get { return eidss.model.Schema.SiteLookup.Accessor.Instance(m_CS); } }
            

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
            public virtual PersonLite SelectDetailT(DbManagerProxy manager, object ident, int? HACode = null)
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

            
            public virtual PersonLite SelectByKey(DbManagerProxy manager
                , Int64? idfPerson
                )
            {
                return _SelectByKey(manager
                    , idfPerson
                    , null, null
                    );
            }
            

            private PersonLite _SelectByKey(DbManagerProxy manager
                , Int64? idfPerson
                , on_action loading, on_action loaded
                )
            {
                return _SelectByKeyInternal(manager
                    , idfPerson
                    , loading, loaded
                    )
                    
                    ;
            }
      
            
            protected virtual PersonLite _SelectByKeyInternal(DbManagerProxy manager
                , Int64? idfPerson
                , on_action loading, on_action loaded
                )
            {
            
                MapResultSet[] sets = new MapResultSet[2];
                List<PersonLite> objs = new List<PersonLite>();
                sets[0] = new MapResultSet(typeof(PersonLite), objs);
                
                List<PersonLiteGroupInfo> objs_PersonLiteGroupInfo = new List<PersonLiteGroupInfo>();
                sets[1] = new MapResultSet(typeof(PersonLiteGroupInfo), objs_PersonLiteGroupInfo);
                PersonLite obj = null;
                try
                {
                    manager
                        .SetSpCommand("spPerson_SelectDetail"
                            , manager.Parameter("@idfPerson", idfPerson)
                            , manager.Parameter("@LangID", ModelUserContext.CurrentLanguage)
                            
                            )
                        .ExecuteResultSet(sets);

                    if (objs.Count == 0)
                        return null;

                    obj = objs[0];
                    obj.m_CS = m_CS;
                    
                  
                    if (loading != null)
                        loading(obj);
                    _SetupLoad(manager, obj);
                    
                    obj.GroupInfoList.ForEach(c => GroupInfoListAccessor._SetupLoad(manager, c));
                            

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
    
            private void _SetupAddChildHandlerLoginInfoList(PersonLite obj)
            {
                obj.LoginInfoList.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach(var o in e.NewItems)
                        {
                            _SetupChildHandlers(obj, o);
                        }
                    }
                };
            }
            
            private void _SetupAddChildHandlerGroupInfoList(PersonLite obj)
            {
                obj.GroupInfoList.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach(var o in e.NewItems)
                        {
                            _SetupChildHandlers(obj, o);
                        }
                    }
                };
            }
            
            internal void _LoadLoginInfoList(PersonLite obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadLoginInfoList(manager, obj);
                }
            }
            internal void _LoadLoginInfoList(DbManagerProxy manager, PersonLite obj)
            {
              
                obj.LoginInfoList.Clear();
                obj.LoginInfoList.AddRange(LoginInfoListAccessor.SelectDetailList(manager
                    
                    , obj.idfPerson
                    ));
                obj.LoginInfoList.ForEach(c => c.m_ObjectName = _str_LoginInfoList);
                obj.LoginInfoList.AcceptChanges();
                    
              }
            
        
        
            internal void _SetupLoad(DbManagerProxy manager, PersonLite obj, bool bCloning = false)
            {
                if (obj == null) return;
                
                // loading extenters - begin
                // loading extenters - end
                if (!bCloning)
                {
                
                _LoadLoginInfoList(manager, obj);
                }
                _LoadLookups(manager, obj);
                obj._setParent();
                
                // loaded extenters - begin
              obj.Site = obj.SiteLookup.Where(s => s.idfsSite == EidssSiteContext.Instance.SiteID).FirstOrDefault();
            
                // loaded extenters - end
                
                _SetupHandlers(obj);
                _SetupChildHandlers(obj, null);
                
                _SetupAddChildHandlerLoginInfoList(obj);
                
                _SetupAddChildHandlerGroupInfoList(obj);
                
                _SetPermitions(obj._permissions, obj);
                _SetupRequired(obj);
                _SetupPersonalDataRestrictions(obj);
                obj._SetupMainHandler();
                obj.AcceptChanges();
            }
            
            internal void _SetPermitions(IObjectPermissions permissions, PersonLite obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                        obj.LoginInfoList.ForEach(c => LoginInfoListAccessor._SetPermitions(obj._permissions, c));
                    
                        obj.GroupInfoList.ForEach(c => GroupInfoListAccessor._SetPermitions(obj._permissions, c));
                    
                    }
                }
            }

    

            private PersonLite _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                PersonLite obj = null;
                try
                {
                    obj = PersonLite.CreateInstance();
                    obj.m_CS = m_CS;
                    obj.m_IsNew = true;
                    obj.Parent = Parent;
                    
                    if (creating != null)
                        creating(obj);
                
                    // creating extenters - begin
                obj.idfPerson = (new GetNewIDExtender<PersonLite>()).GetScalar(manager, obj, isFake);
                    // creating extenters - end
                
                    _LoadLookups(manager, obj);
                    _SetupHandlers(obj);
                    _SetupChildHandlers(obj, null);
                    
                    _SetupAddChildHandlerLoginInfoList(obj);
                    
                    _SetupAddChildHandlerGroupInfoList(obj);
                    
                    obj._SetupMainHandler();
                    obj._setParent();
                
                    // created extenters - begin
                obj.Site = obj.SiteLookup.Where(s => s.idfsSite == EidssSiteContext.Instance.SiteID).FirstOrDefault();;
                obj.idfsSite = EidssSiteContext.Instance.SiteID;
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

            
            public PersonLite CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public PersonLite CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public PersonLite CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            public ActResult Delete(DbManagerProxy manager, PersonLite obj, List<object> pars)
            {
                
                return Delete(manager, obj
                    );
            }
            public ActResult Delete(DbManagerProxy manager, PersonLite obj
                )
            {
                
                if (obj != null && !obj.GetPermissions().CanExecute("Delete"))
                    throw new PermissionException("Person", "Delete");
                
              return new ActResult(obj.MarkToDelete() && ObjectAccessor.PostInterface<Person>().Post(manager, obj), obj);
            
            }
            
      
            private void _SetupChildHandlers(PersonLite obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(PersonLite obj)
            {
                
            }
    
            public void LoadLookup_Institution(DbManagerProxy manager, PersonLite obj)
            {
                
                obj.InstitutionLookup.Clear();
                
                obj.InstitutionLookup.Add(InstitutionAccessor.CreateNewT(manager, null));
                
                obj.InstitutionLookup.AddRange(InstitutionAccessor.SelectLookupList(manager
                    
                    , null
                    , null
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfInstitution == obj.idfInstitution))
                    
                    .ToList());
                
                if (obj.idfInstitution != null && obj.idfInstitution != 0)
                {
                    obj.Institution = obj.InstitutionLookup
                        .SingleOrDefault(c => c.idfInstitution == obj.idfInstitution);
                    
                }
              
                LookupManager.AddObject("OrganizationLookup", obj, InstitutionAccessor.GetType(), "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_Department(DbManagerProxy manager, PersonLite obj)
            {
                
                obj.DepartmentLookup.Clear();
                
                obj.DepartmentLookup.Add(DepartmentAccessor.CreateNewT(manager, null));
                
                obj.DepartmentLookup.AddRange(DepartmentAccessor.SelectLookupList(manager
                    
                    , new Func<PersonLite, long>(c => c.idfInstitution ?? 0)(obj)
                            
                    , null
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfInstitution == obj.idfDepartment))
                    
                    .ToList());
                
                if (obj.idfDepartment != null && obj.idfDepartment != 0)
                {
                    obj.Department = obj.DepartmentLookup
                        .SingleOrDefault(c => c.idfInstitution == obj.idfDepartment);
                    
                }
              
                LookupManager.AddObject("DepartmentLookup", obj, DepartmentAccessor.GetType(), "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_StaffPosition(DbManagerProxy manager, PersonLite obj)
            {
                
                obj.StaffPositionLookup.Clear();
                
                obj.StaffPositionLookup.Add(StaffPositionAccessor.CreateNewT(manager, null));
                
                obj.StaffPositionLookup.AddRange(StaffPositionAccessor.rftPosition_SelectList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsBaseReference == obj.idfsStaffPosition))
                    
                    .ToList());
                
                if (obj.idfsStaffPosition != null && obj.idfsStaffPosition != 0)
                {
                    obj.StaffPosition = obj.StaffPositionLookup
                        .SingleOrDefault(c => c.idfsBaseReference == obj.idfsStaffPosition);
                    
                }
              
                LookupManager.AddObject("rftPosition", obj, StaffPositionAccessor.GetType(), "rftPosition_SelectList", "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            
            public void LoadLookup_Site(DbManagerProxy manager, PersonLite obj)
            {
                
                obj.SiteLookup.Clear();
                
                obj.SiteLookup.Add(SiteAccessor.CreateNewT(manager, null));
                
                obj.SiteLookup.AddRange(SiteAccessor.SelectLookupList(manager
                    
                    )
                    .Where(c => (c.intRowStatus == 0) || (c.idfsSite == obj.idfsOASite))
                    
                    .ToList());
                
                if (obj.idfsOASite != null && obj.idfsOASite != 0)
                {
                    obj.Site = obj.SiteLookup
                        .SingleOrDefault(c => c.idfsSite == obj.idfsOASite);
                    
                }
              
                LookupManager.AddObject("SiteLookup", obj, SiteAccessor.GetType(), "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            

            internal void _LoadLookups(DbManagerProxy manager, PersonLite obj)
            {
                
                LoadLookup_Institution(manager, obj);
                
                LoadLookup_Department(manager, obj);
                
                LoadLookup_StaffPosition(manager, obj);
                
                LoadLookup_Site(manager, obj);
                
            }
    
            [SprocName("spPerson_Delete")]
            protected abstract void _postDelete(DbManagerProxy manager
                , Int64? idfPerson
                );
            protected void _postDeletePredicate(DbManagerProxy manager
                , Int64? idfPerson
                )
            {
                
                _postDelete(manager, idfPerson);
                
            }
        
            public bool DeleteObject(DbManagerProxy manager, object ident)
            {
                IObject obj = SelectDetail(manager, ident);
                if (!obj.MarkToDelete()) return false;
                return Post(manager, obj);
            }
        
            [SprocName("spPerson_Post")]
            protected abstract void _post(DbManagerProxy manager, int Action, 
                [Direction.InputOutput()] PersonLite obj);
            protected void _postPredicate(DbManagerProxy manager, int Action, 
                [Direction.InputOutput()] PersonLite obj)
            {
                
                _post(manager, Action, obj);
                
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
                        PersonLite bo = obj as PersonLite;
                        
                        if (!bo.IsNew && bo.IsMarkedToDelete) // delete
                        {
                            if (!CanDelete)
                                throw new PermissionException("Person", "Delete");
                        }
                        else if (bo.IsNew && !bo.IsMarkedToDelete) // insert
                        {
                            if (!CanInsert)
                                throw new PermissionException("Person", "Insert");
                        }
                        else if (!bo.IsMarkedToDelete && bo.HasChanges) // update
                        {
                            if (!CanUpdate)
                                throw new PermissionException("Person", "Update");
                        }
                        
                        long mainObject = bo.idfPerson;
                        
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
                            
                            eidss.model.Enums.AuditEventType auditEventType = eidss.model.Enums.AuditEventType.daeFreeDataUpdate;
                            
                              eidss.model.Enums.EventType eventType = eidss.model.Enums.EventType.RaiseReferenceCacheChange;
                              
                              manager.SetEventParams(false, "Person", new object[] { eventType, null, "Person" });
                              
                            if (!bo.IsNew && bo.IsMarkedToDelete) // delete
                            {
                                
                                auditEventType = eidss.model.Enums.AuditEventType.daeDelete;
                                
                            }
                            else if (bo.IsNew && !bo.IsMarkedToDelete) // insert
                            {
                                
                                auditEventType = eidss.model.Enums.AuditEventType.daeCreate;
                                
                            }
                            else if (!bo.IsMarkedToDelete) // update
                            {
                                
                                auditEventType = eidss.model.Enums.AuditEventType.daeEdit;
                                
                            }
                            
                            eidss.model.Enums.EIDSSAuditObject objectType = eidss.model.Enums.EIDSSAuditObject.daoEmployee;
                            eidss.model.Enums.AuditTable auditTable = eidss.model.Enums.AuditTable.tlbPerson;
                            manager.AuditParams = new object[] { auditEventType, objectType, auditTable, mainObject };
                            
                            bTransactionStarted = true;
                            manager.BeginTransaction();
                        }
                        bSuccess = _PostNonTransaction(manager, obj as PersonLite, bChildObject);
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
            private bool _PostNonTransaction(DbManagerProxy manager, PersonLite obj, bool bChildObject) 
            { 
                bool bHasChanges = obj.HasChanges;
                if (!obj.IsNew && obj.IsMarkedToDelete) // delete
                {
            
                    if (!ValidateCanDelete(manager, obj)) return false;
                                        
                    _postPredicate(manager, 8, obj);
                                            
                    if (obj.LoginInfoList != null)
                    {
                        foreach (var i in obj.LoginInfoList)
                        {
                            i.MarkToDelete();
                            if (!LoginInfoListAccessor.Post(manager, i, true))
                                return false;
                        }
                    }
                                            
                    if (obj.GroupInfoList != null)
                    {
                        foreach (var i in obj.GroupInfoList)
                        {
                            i.MarkToDelete();
                            if (!GroupInfoListAccessor.Post(manager, i, true))
                                return false;
                        }
                    }
                                            
                }
                else if (!obj.IsMarkedToDelete) // insert/update
                {
                    if (!bChildObject)
                        if (!Validate(manager, obj, true, true, true)) 
                            return false;
                    
            
                    // posting extenters - begin
                    // posting extenters - end
            
                    if (obj.IsNew && !obj.IsMarkedToDelete && obj.HasChanges)
                        _postPredicate(manager, 4, obj);
                    else if (!obj.IsNew && !obj.IsMarkedToDelete && obj.HasChanges)
                        _postPredicate(manager, 16, obj);
                                    
                    if (obj.IsNew)
                    {
                        if (obj.LoginInfoList != null) // forced load potential lazy subobject for new object
                        {
                            foreach (var i in obj.LoginInfoList)
                                if (!LoginInfoListAccessor.Post(manager, i, true))
                                    return false;
                            obj.LoginInfoList.Where(c => c.IsMarkedToDelete).ToList().ForEach(c => obj.LoginInfoList.Remove(c));
                            obj.LoginInfoList.AcceptChanges();
                        }
                    }
                    else
                    {
                        if (obj._LoginInfoList != null) // do not load lazy subobject for existing object
                        {
                            foreach (var i in obj._LoginInfoList)
                                if (!LoginInfoListAccessor.Post(manager, i, true))
                                    return false;
                            obj._LoginInfoList.Where(c => c.IsMarkedToDelete).ToList().ForEach(c => obj._LoginInfoList.Remove(c));
                            obj._LoginInfoList.AcceptChanges();
                        }
                    }
                                    
                    if (obj.IsNew)
                    {
                        if (obj.GroupInfoList != null) // forced load potential lazy subobject for new object
                        {
                            foreach (var i in obj.GroupInfoList)
                                if (!GroupInfoListAccessor.Post(manager, i, true))
                                    return false;
                            obj.GroupInfoList.Where(c => c.IsMarkedToDelete).ToList().ForEach(c => obj.GroupInfoList.Remove(c));
                            obj.GroupInfoList.AcceptChanges();
                        }
                    }
                    else
                    {
                        if (obj._GroupInfoList != null) // do not load lazy subobject for existing object
                        {
                            foreach (var i in obj._GroupInfoList)
                                if (!GroupInfoListAccessor.Post(manager, i, true))
                                    return false;
                            obj._GroupInfoList.Where(c => c.IsMarkedToDelete).ToList().ForEach(c => obj._GroupInfoList.Remove(c));
                            obj._GroupInfoList.AcceptChanges();
                        }
                    }
                                    
                    // posted extenters - begin
                    // posted extenters - end
            
                }
                //obj.AcceptChanges();
                
                return true;
            }
            
            public bool ValidateCanDelete(PersonLite obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    return ValidateCanDelete(manager, obj);
                }
            }
            public bool ValidateCanDelete(DbManagerProxy manager, PersonLite obj)
            {
            
                return true;
            }
        
      
            protected ValidationModelException ChainsValidate(PersonLite obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(PersonLite obj, bool bRethrowException)
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
                return Validate(manager, obj as PersonLite, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, PersonLite obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                try
                {
                    if (bPostValidation)
                    {
                
                (new RequiredValidator( "strFirstName", "strFirstName","",
                ValidationEventType.Error
              )).Validate(c => true, obj, obj.strFirstName);
            
                (new RequiredValidator( "strFamilyName", "strFamilyName","",
                ValidationEventType.Error
              )).Validate(c => true, obj, obj.strFamilyName);
            
                (new RequiredValidator( "Institution", "Institution","Organization",
                ValidationEventType.Error
              )).Validate(c => true, obj, obj.Institution);
            
                  
                    }

                    if (bChangeValidation)
                    {
                
                    }
                    
                    if (bDeepValidation)
                    {
                
                        if (obj.LoginInfoList != null)
                            foreach (var i in obj.LoginInfoList.Where(c => !c.IsMarkedToDelete && c.HasChanges))
                                LoginInfoListAccessor.Validate(manager, i, bPostValidation, bChangeValidation, bDeepValidation, true);
                                        
                        if (obj.GroupInfoList != null)
                            foreach (var i in obj.GroupInfoList.Where(c => !c.IsMarkedToDelete && c.HasChanges))
                                GroupInfoListAccessor.Validate(manager, i, bPostValidation, bChangeValidation, bDeepValidation, true);
                                        
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
           
    
            #region IObjectPermissions
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !CanUpdate; } }
            #endregion
            
            private void _SetupRequired(PersonLite obj)
            {
            
                obj
                    .AddRequired("strFirstName", c => true);
                    
                obj
                    .AddRequired("strFamilyName", c => true);
                    
                obj
                    .AddRequired("Institution", c => true);
                    
            }
    
    private void _SetupPersonalDataRestrictions(PersonLite obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as PersonLite) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as PersonLite) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "PersonLiteDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
            #region IObjectPermissions
        internal class Permissions : IObjectPermissions
        {
            private PersonLite m_obj;
            internal Permissions(PersonLite obj)
            {
                m_obj = obj;
            }
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Person.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !(CanUpdate || (CanInsert && m_obj.IsNew)); } }
        }
            #endregion
            
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spPerson_SelectDetail";
            public static string spCount = "";
            public static string spPost = "spPerson_Post";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "spPerson_Delete";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<PersonLite, bool>> RequiredByField = new Dictionary<string, Func<PersonLite, bool>>();
            public static Dictionary<string, Func<PersonLite, bool>> RequiredByProperty = new Dictionary<string, Func<PersonLite, bool>>();
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
                
                Sizes.Add(_str_strFamilyName, 200);
                Sizes.Add(_str_strFirstName, 200);
                Sizes.Add(_str_strSecondName, 200);
                Sizes.Add(_str_strContactPhone, 200);
                Sizes.Add(_str_strBarcode, 200);
                if (!RequiredByField.ContainsKey("strFirstName")) RequiredByField.Add("strFirstName", c => true);
                if (!RequiredByProperty.ContainsKey("strFirstName")) RequiredByProperty.Add("strFirstName", c => true);
                
                if (!RequiredByField.ContainsKey("strFamilyName")) RequiredByField.Add("strFamilyName", c => true);
                if (!RequiredByProperty.ContainsKey("strFamilyName")) RequiredByProperty.Add("strFamilyName", c => true);
                
                if (!RequiredByField.ContainsKey("Institution")) RequiredByField.Add("Institution", c => true);
                if (!RequiredByProperty.ContainsKey("Institution")) RequiredByProperty.Add("Institution", c => true);
                
                Actions.Add(new ActionMetaItem(
                    "Delete",
                    ActionTypes.Action,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => Accessor.Instance(null).Delete(manager, (PersonLite)c, pars),
                        
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
                    null,
                    null,
                    (c,a,p,r) => !((c as Person).IsNew || (c as Person).HideDeleteAction) && EidssUserContext.User.HasPermission(PermissionHelper.DeletePermission(EIDSSPermissionObject.Person)),
                    true,
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<PersonLite>().Post(manager, (PersonLite)c), c),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<PersonLite>().Post(manager, (PersonLite)c), c),
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
                    
        
                _SetupPersonalDataRestrictions();
            }
            
            
    private static void _SetupPersonalDataRestrictions()
    {
    

    }
  
        }
        #endregion
    

        #endregion
        }
    
        
    [XmlType(AnonymousType = true)]
    public abstract partial class PersonLiteGroupInfo : 
        EditableObject<PersonLiteGroupInfo>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfEmployeeGroup), NonUpdatable, PrimaryKey]
        public abstract Int64 idfEmployeeGroup { get; set; }
                
        [LocalizedDisplayName(_str_idfsEmployeeGroupName)]
        [MapField(_str_idfsEmployeeGroupName)]
        public abstract Int64? idfsEmployeeGroupName { get; set; }
        protected Int64? idfsEmployeeGroupName_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsEmployeeGroupName).OriginalValue; } }
        protected Int64? idfsEmployeeGroupName_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsEmployeeGroupName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfEmployee)]
        [MapField(_str_idfEmployee)]
        public abstract Int64 idfEmployee { get; set; }
        protected Int64 idfEmployee_Original { get { return ((EditableValue<Int64>)((dynamic)this)._idfEmployee).OriginalValue; } }
        protected Int64 idfEmployee_Previous { get { return ((EditableValue<Int64>)((dynamic)this)._idfEmployee).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strName)]
        [MapField(_str_strName)]
        public abstract String strName { get; set; }
        protected String strName_Original { get { return ((EditableValue<String>)((dynamic)this)._strName).OriginalValue; } }
        protected String strName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strDescription)]
        [MapField(_str_strDescription)]
        public abstract String strDescription { get; set; }
        protected String strDescription_Original { get { return ((EditableValue<String>)((dynamic)this)._strDescription).OriginalValue; } }
        protected String strDescription_Previous { get { return ((EditableValue<String>)((dynamic)this)._strDescription).PreviousValue; } }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<PersonLiteGroupInfo, object> _get_func;
            internal Action<PersonLiteGroupInfo, string> _set_func;
            internal Action<PersonLiteGroupInfo, PersonLiteGroupInfo, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfEmployeeGroup = "idfEmployeeGroup";
        internal const string _str_idfsEmployeeGroupName = "idfsEmployeeGroupName";
        internal const string _str_idfEmployee = "idfEmployee";
        internal const string _str_strName = "strName";
        internal const string _str_strDescription = "strDescription";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfEmployeeGroup, _formname = _str_idfEmployeeGroup, _type = "Int64",
              _get_func = o => o.idfEmployeeGroup,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfEmployeeGroup != newval) o.idfEmployeeGroup = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfEmployeeGroup != c.idfEmployeeGroup || o.IsRIRPropChanged(_str_idfEmployeeGroup, c)) 
                  m.Add(_str_idfEmployeeGroup, o.ObjectIdent + _str_idfEmployeeGroup, o.ObjectIdent2 + _str_idfEmployeeGroup, o.ObjectIdent3 + _str_idfEmployeeGroup, "Int64", 
                    o.idfEmployeeGroup == null ? "" : o.idfEmployeeGroup.ToString(),                  
                  o.IsReadOnly(_str_idfEmployeeGroup), o.IsInvisible(_str_idfEmployeeGroup), o.IsRequired(_str_idfEmployeeGroup)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsEmployeeGroupName, _formname = _str_idfsEmployeeGroupName, _type = "Int64?",
              _get_func = o => o.idfsEmployeeGroupName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsEmployeeGroupName != newval) o.idfsEmployeeGroupName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsEmployeeGroupName != c.idfsEmployeeGroupName || o.IsRIRPropChanged(_str_idfsEmployeeGroupName, c)) 
                  m.Add(_str_idfsEmployeeGroupName, o.ObjectIdent + _str_idfsEmployeeGroupName, o.ObjectIdent2 + _str_idfsEmployeeGroupName, o.ObjectIdent3 + _str_idfsEmployeeGroupName, "Int64?", 
                    o.idfsEmployeeGroupName == null ? "" : o.idfsEmployeeGroupName.ToString(),                  
                  o.IsReadOnly(_str_idfsEmployeeGroupName), o.IsInvisible(_str_idfsEmployeeGroupName), o.IsRequired(_str_idfsEmployeeGroupName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfEmployee, _formname = _str_idfEmployee, _type = "Int64",
              _get_func = o => o.idfEmployee,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfEmployee != newval) o.idfEmployee = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfEmployee != c.idfEmployee || o.IsRIRPropChanged(_str_idfEmployee, c)) 
                  m.Add(_str_idfEmployee, o.ObjectIdent + _str_idfEmployee, o.ObjectIdent2 + _str_idfEmployee, o.ObjectIdent3 + _str_idfEmployee, "Int64", 
                    o.idfEmployee == null ? "" : o.idfEmployee.ToString(),                  
                  o.IsReadOnly(_str_idfEmployee), o.IsInvisible(_str_idfEmployee), o.IsRequired(_str_idfEmployee)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strName, _formname = _str_strName, _type = "String",
              _get_func = o => o.strName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strName != newval) o.strName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strName != c.strName || o.IsRIRPropChanged(_str_strName, c)) 
                  m.Add(_str_strName, o.ObjectIdent + _str_strName, o.ObjectIdent2 + _str_strName, o.ObjectIdent3 + _str_strName, "String", 
                    o.strName == null ? "" : o.strName.ToString(),                  
                  o.IsReadOnly(_str_strName), o.IsInvisible(_str_strName), o.IsRequired(_str_strName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strDescription, _formname = _str_strDescription, _type = "String",
              _get_func = o => o.strDescription,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strDescription != newval) o.strDescription = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strDescription != c.strDescription || o.IsRIRPropChanged(_str_strDescription, c)) 
                  m.Add(_str_strDescription, o.ObjectIdent + _str_strDescription, o.ObjectIdent2 + _str_strDescription, o.ObjectIdent3 + _str_strDescription, "String", 
                    o.strDescription == null ? "" : o.strDescription.ToString(),                  
                  o.IsReadOnly(_str_strDescription), o.IsInvisible(_str_strDescription), o.IsRequired(_str_strDescription)); 
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
            PersonLiteGroupInfo obj = (PersonLiteGroupInfo)o;
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
        internal string m_ObjectName = "PersonLiteGroupInfo";

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
            var ret = base.Clone() as PersonLiteGroupInfo;
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
            var ret = base.Clone() as PersonLiteGroupInfo;
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
        public PersonLiteGroupInfo CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as PersonLiteGroupInfo;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfEmployeeGroup; } }
        public string KeyName { get { return "idfEmployeeGroup"; } }
        public object KeyLookup { get { return idfEmployeeGroup; } }
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

        private bool IsRIRPropChanged(string fld, PersonLiteGroupInfo c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, PersonLiteGroupInfo c)
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
        

      

        public PersonLiteGroupInfo()
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PersonLiteGroupInfo_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(PersonLiteGroupInfo_PropertyChanged);
        }
        private void PersonLiteGroupInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as PersonLiteGroupInfo).Changed(e.PropertyName);
            
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
            PersonLiteGroupInfo obj = this;
            
        }
        private void _DeletedExtenders()
        {
            PersonLiteGroupInfo obj = this;
            
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


        internal Dictionary<string, Func<PersonLiteGroupInfo, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<PersonLiteGroupInfo, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<PersonLiteGroupInfo, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<PersonLiteGroupInfo, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<PersonLiteGroupInfo, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<PersonLiteGroupInfo, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<PersonLiteGroupInfo, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~PersonLiteGroupInfo()
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
    
        #region Class for web grid
        public class PersonLiteGroupInfoGridModel : IGridModelItem
        {
            public string ErrorMessage { get; set; }
            public long ItemKey { get; set; }
        
            public Int64 idfEmployeeGroup { get; set; }
        
            public String strName { get; set; }
        
            public String strDescription { get; set; }
        
        }
        public partial class PersonLiteGroupInfoGridModelList : List<PersonLiteGroupInfoGridModel>, IGridModelList, IGridModelListLoad
        {
            public long ListKey { get; protected set; }
            public List<string> Columns { get; protected set; }
            public List<string> Hiddens { get; protected set; }
            public Dictionary<string, string> Labels { get; protected set; }
            public Dictionary<string, ActionMetaItem> Actions { get; protected set; }
            public List<string> Keys { get; protected set; }
            public bool IsHiddenPersonalData(string name) { return Meta._isHiddenPersonalData(name); }
            public IObjectMeta ObjectMeta { get { return Accessor.Instance(null); } }
            public PersonLiteGroupInfoGridModelList()
            {
            }
            public void Load(long key, IEnumerable items, string errMes)
            {
                LoadGridModelList(key, items as IEnumerable<PersonLiteGroupInfo>, errMes);
            }
            public PersonLiteGroupInfoGridModelList(long key, IEnumerable items, string errMes)
            {
                LoadGridModelList(key, items as IEnumerable<PersonLiteGroupInfo>, errMes);
            }
            public PersonLiteGroupInfoGridModelList(long key, IEnumerable<PersonLiteGroupInfo> items)
            {
                LoadGridModelList(key, items, null);
            }
            public PersonLiteGroupInfoGridModelList(long key)
            {
                LoadGridModelList(key, new List<PersonLiteGroupInfo>(), null);
            }
            partial void filter(List<PersonLiteGroupInfo> items);
            private void LoadGridModelList(long key, IEnumerable<PersonLiteGroupInfo> items, string errMes)
            {
                ListKey = key;
                
                Columns = new List<string> {_str_strName,_str_strDescription};
                    
                Hiddens = new List<string> {_str_idfEmployeeGroup};
                Keys = new List<string> {_str_idfEmployeeGroup};
                Labels = new Dictionary<string, string> {{_str_strName, _str_strName},{_str_strDescription, _str_strDescription}};
                Actions = new Dictionary<string, ActionMetaItem> {};
                PersonLiteGroupInfo.Meta.Actions.ForEach(a => Actions.Add("@" + a.Name, a));
                var list = new List<PersonLiteGroupInfo>(items);
                filter(list);
                AddRange(list.Where(c => !c.IsMarkedToDelete).Select(c => new PersonLiteGroupInfoGridModel()
                {
                    ItemKey=c.idfEmployeeGroup,strName=c.strName,strDescription=c.strDescription
                }));
                if (Count > 0)
                {
                    this.Last().ErrorMessage = errMes;
                }
            }
        }
        #endregion
        

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<PersonLiteGroupInfo>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectCreator
            , IObjectCreator<PersonLiteGroupInfo>
            
            , IObjectSelectDetail
            , IObjectSelectDetail<PersonLiteGroupInfo>
            , IObjectPost
            , IObjectDelete
                    
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfEmployeeGroup"; } }
            #endregion
        
            public delegate void on_action(PersonLiteGroupInfo obj);
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
            public virtual PersonLiteGroupInfo SelectDetailT(DbManagerProxy manager, object ident, int? HACode = null)
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

            
            public virtual PersonLiteGroupInfo SelectByKey(DbManagerProxy manager
                , Int64? idfPerson
                )
            {
                return _SelectByKey(manager
                    , idfPerson
                    , null, null
                    );
            }
            

            private PersonLiteGroupInfo _SelectByKey(DbManagerProxy manager
                , Int64? idfPerson
                , on_action loading, on_action loaded
                )
            {
                return _SelectByKeyInternal(manager
                    , idfPerson
                    , loading, loaded
                    )
                    
                    ;
            }
      
            
            protected virtual PersonLiteGroupInfo _SelectByKeyInternal(DbManagerProxy manager
                , Int64? idfPerson
                , on_action loading, on_action loaded
                )
            {
            
                throw new NotImplementedException();
                
            }
    
        
        
            internal void _SetupLoad(DbManagerProxy manager, PersonLiteGroupInfo obj, bool bCloning = false)
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
            
            internal void _SetPermitions(IObjectPermissions permissions, PersonLiteGroupInfo obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                    }
                }
            }

    

            private PersonLiteGroupInfo _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                PersonLiteGroupInfo obj = null;
                try
                {
                    obj = PersonLiteGroupInfo.CreateInstance();
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

            
            public PersonLiteGroupInfo CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public PersonLiteGroupInfo CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public PersonLiteGroupInfo CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            public ActResult AddGroupInfo(DbManagerProxy manager, PersonLiteGroupInfo obj, List<object> pars)
            {
                
                return AddGroupInfo(manager, obj
                    );
            }
            public ActResult AddGroupInfo(DbManagerProxy manager, PersonLiteGroupInfo obj
                )
            {
                
                return true;
                
            }
            
      
            public ActResult DeleteGroupInfo(DbManagerProxy manager, PersonLiteGroupInfo obj, List<object> pars)
            {
                
                return DeleteGroupInfo(manager, obj
                    );
            }
            public ActResult DeleteGroupInfo(DbManagerProxy manager, PersonLiteGroupInfo obj
                )
            {
                
                return true;
                
            }
            
      
            private void _SetupChildHandlers(PersonLiteGroupInfo obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(PersonLiteGroupInfo obj)
            {
                
            }
    

            internal void _LoadLookups(DbManagerProxy manager, PersonLiteGroupInfo obj)
            {
                
            }
    
            public bool DeleteObject(DbManagerProxy manager, object ident)
            {
                IObject obj = SelectDetail(manager, ident);
                if (!obj.MarkToDelete()) return false;
                return Post(manager, obj);
            }
        
            [SprocName("dbo.spPersonGroupInfo_Post")]
            protected abstract void _post(DbManagerProxy manager, int Action, 
                [Direction.InputOutput()] PersonLiteGroupInfo obj);
            protected void _postPredicate(DbManagerProxy manager, int Action, 
                [Direction.InputOutput()] PersonLiteGroupInfo obj)
            {
                
                _post(manager, Action, obj);
                
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
                        PersonLiteGroupInfo bo = obj as PersonLiteGroupInfo;
                        
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
                        bSuccess = _PostNonTransaction(manager, obj as PersonLiteGroupInfo, bChildObject);
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
            private bool _PostNonTransaction(DbManagerProxy manager, PersonLiteGroupInfo obj, bool bChildObject) 
            { 
                bool bHasChanges = obj.HasChanges;
                if (!obj.IsNew && obj.IsMarkedToDelete) // delete
                {
            
                    if (!ValidateCanDelete(manager, obj)) return false;
                            
                    _postPredicate(manager, 8, obj);
                                
                }
                else if (!obj.IsMarkedToDelete) // insert/update
                {
                    if (!bChildObject)
                        if (!Validate(manager, obj, true, true, true)) 
                            return false;
                    
            
                    // posting extenters - begin
                    // posting extenters - end
            
                    if (obj.IsNew && !obj.IsMarkedToDelete && obj.HasChanges)
                        _postPredicate(manager, 4, obj);
                    else if (!obj.IsNew && !obj.IsMarkedToDelete && obj.HasChanges)
                        _postPredicate(manager, 16, obj);
                        
                    // posted extenters - begin
                    // posted extenters - end
            
                }
                //obj.AcceptChanges();
                
                return true;
            }
            
            public bool ValidateCanDelete(PersonLiteGroupInfo obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    return ValidateCanDelete(manager, obj);
                }
            }
            public bool ValidateCanDelete(DbManagerProxy manager, PersonLiteGroupInfo obj)
            {
            
                return true;
            }
        
      
            protected ValidationModelException ChainsValidate(PersonLiteGroupInfo obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(PersonLiteGroupInfo obj, bool bRethrowException)
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
                return Validate(manager, obj as PersonLiteGroupInfo, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, PersonLiteGroupInfo obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                return true;
            }
           
    
            private void _SetupRequired(PersonLiteGroupInfo obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(PersonLiteGroupInfo obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as PersonLiteGroupInfo) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as PersonLiteGroupInfo) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "PersonLiteGroupInfoDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spPerson_SelectDetail";
            public static string spCount = "";
            public static string spPost = "dbo.spPersonGroupInfo_Post";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<PersonLiteGroupInfo, bool>> RequiredByField = new Dictionary<string, Func<PersonLiteGroupInfo, bool>>();
            public static Dictionary<string, Func<PersonLiteGroupInfo, bool>> RequiredByProperty = new Dictionary<string, Func<PersonLiteGroupInfo, bool>>();
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
                
                Sizes.Add(_str_strName, 2000);
                Sizes.Add(_str_strDescription, 200);
                GridMeta.Add(new GridMetaItem(
                    _str_idfEmployeeGroup,
                    _str_idfEmployeeGroup, null, false, false, true, true, true, null
                    ));
                GridMeta.Add(new GridMetaItem(
                    _str_strName,
                    _str_strName, null, false, true, true, true, true, null
                    ));
                GridMeta.Add(new GridMetaItem(
                    _str_strDescription,
                    _str_strDescription, null, false, true, true, true, true, null
                    ));
                Actions.Add(new ActionMetaItem(
                    "AddGroupInfo",
                    ActionTypes.Action,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => Accessor.Instance(null).AddGroupInfo(manager, (PersonLiteGroupInfo)c, pars),
                        
                    null,
                    
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strAdd_Id",
                        "add",
                        /*from BvMessages*/"tooltipAdd_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        ActionsAlignment.Right,
                        ActionsPanelType.Group,
                        ActionsAppType.All
                        ),
                      false,
                    null,
                    null,
                    null,
                    null,
                    (c, a, b, p) => EidssUserContext.User.HasPermission(PermissionHelper.UpdatePermission(EIDSSPermissionObject.UserGroup)),
                    false,
                    false,
                    null,
                    false,
                    new ActionMetaItem[] {
                      
                      }
                    
                    ));
                  
                Actions.Add(new ActionMetaItem(
                    "DeleteGroupInfo",
                    ActionTypes.Action,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => Accessor.Instance(null).DeleteGroupInfo(manager, (PersonLiteGroupInfo)c, pars),
                        
                    null,
                    
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strDelete_Id",
                        "delete",
                        /*from BvMessages*/"tooltipDelete_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        ActionsAlignment.Right,
                        ActionsPanelType.Group,
                        ActionsAppType.All
                        ),
                      false,
                    null,
                    null,
                    null,
                    null,
                    (c, a, b, p) => EidssUserContext.User.HasPermission(PermissionHelper.UpdatePermission(EIDSSPermissionObject.UserGroup)),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<PersonLiteGroupInfo>().Post(manager, (PersonLiteGroupInfo)c), c),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<PersonLiteGroupInfo>().Post(manager, (PersonLiteGroupInfo)c), c),
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
	