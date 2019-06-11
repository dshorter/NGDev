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
    public abstract partial class ObjectAccess : 
        EditableObject<ObjectAccess>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfObjectAccess), NonUpdatable, PrimaryKey]
        public abstract Int64 idfObjectAccess { get; set; }
                
        [LocalizedDisplayName(_str_FunctionName)]
        [MapField(_str_FunctionName)]
        public abstract String FunctionName { get; set; }
        protected String FunctionName_Original { get { return ((EditableValue<String>)((dynamic)this)._functionName).OriginalValue; } }
        protected String FunctionName_Previous { get { return ((EditableValue<String>)((dynamic)this)._functionName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsObjectOperation)]
        [MapField(_str_idfsObjectOperation)]
        public abstract Int64? idfsObjectOperation { get; set; }
        protected Int64? idfsObjectOperation_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsObjectOperation).OriginalValue; } }
        protected Int64? idfsObjectOperation_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsObjectOperation).PreviousValue; } }
                
        [LocalizedDisplayName(_str_ObjectOperationName)]
        [MapField(_str_ObjectOperationName)]
        public abstract String ObjectOperationName { get; set; }
        protected String ObjectOperationName_Original { get { return ((EditableValue<String>)((dynamic)this)._objectOperationName).OriginalValue; } }
        protected String ObjectOperationName_Previous { get { return ((EditableValue<String>)((dynamic)this)._objectOperationName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsObjectType)]
        [MapField(_str_idfsObjectType)]
        public abstract Int64? idfsObjectType { get; set; }
        protected Int64? idfsObjectType_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsObjectType).OriginalValue; } }
        protected Int64? idfsObjectType_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsObjectType).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsObjectID)]
        [MapField(_str_idfsObjectID)]
        public abstract Int64? idfsObjectID { get; set; }
        protected Int64? idfsObjectID_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsObjectID).OriginalValue; } }
        protected Int64? idfsObjectID_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfsObjectID).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfsSite)]
        [MapField(_str_idfsSite)]
        public abstract Int64 idfsSite { get; set; }
        protected Int64 idfsSite_Original { get { return ((EditableValue<Int64>)((dynamic)this)._idfsSite).OriginalValue; } }
        protected Int64 idfsSite_Previous { get { return ((EditableValue<Int64>)((dynamic)this)._idfsSite).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strSiteID)]
        [MapField(_str_strSiteID)]
        public abstract String strSiteID { get; set; }
        protected String strSiteID_Original { get { return ((EditableValue<String>)((dynamic)this)._strSiteID).OriginalValue; } }
        protected String strSiteID_Previous { get { return ((EditableValue<String>)((dynamic)this)._strSiteID).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strSiteName)]
        [MapField(_str_strSiteName)]
        public abstract String strSiteName { get; set; }
        protected String strSiteName_Original { get { return ((EditableValue<String>)((dynamic)this)._strSiteName).OriginalValue; } }
        protected String strSiteName_Previous { get { return ((EditableValue<String>)((dynamic)this)._strSiteName).PreviousValue; } }
                
        [LocalizedDisplayName(_str_idfEmployee)]
        [MapField(_str_idfEmployee)]
        public abstract Int64? idfEmployee { get; set; }
        protected Int64? idfEmployee_Original { get { return ((EditableValue<Int64?>)((dynamic)this)._idfEmployee).OriginalValue; } }
        protected Int64? idfEmployee_Previous { get { return ((EditableValue<Int64?>)((dynamic)this)._idfEmployee).PreviousValue; } }
                
        [LocalizedDisplayName(_str_isAllow)]
        [MapField(_str_isAllow)]
        public abstract Boolean? isAllow { get; set; }
        protected Boolean? isAllow_Original { get { return ((EditableValue<Boolean?>)((dynamic)this)._isAllow).OriginalValue; } }
        protected Boolean? isAllow_Previous { get { return ((EditableValue<Boolean?>)((dynamic)this)._isAllow).PreviousValue; } }
                
        [LocalizedDisplayName(_str_isDeny)]
        [MapField(_str_isDeny)]
        public abstract Boolean? isDeny { get; set; }
        protected Boolean? isDeny_Original { get { return ((EditableValue<Boolean?>)((dynamic)this)._isDeny).OriginalValue; } }
        protected Boolean? isDeny_Previous { get { return ((EditableValue<Boolean?>)((dynamic)this)._isDeny).PreviousValue; } }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<ObjectAccess, object> _get_func;
            internal Action<ObjectAccess, string> _set_func;
            internal Action<ObjectAccess, ObjectAccess, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfObjectAccess = "idfObjectAccess";
        internal const string _str_FunctionName = "FunctionName";
        internal const string _str_idfsObjectOperation = "idfsObjectOperation";
        internal const string _str_ObjectOperationName = "ObjectOperationName";
        internal const string _str_idfsObjectType = "idfsObjectType";
        internal const string _str_idfsObjectID = "idfsObjectID";
        internal const string _str_idfsSite = "idfsSite";
        internal const string _str_strSiteID = "strSiteID";
        internal const string _str_strSiteName = "strSiteName";
        internal const string _str_idfEmployee = "idfEmployee";
        internal const string _str_isAllow = "isAllow";
        internal const string _str_isDeny = "isDeny";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfObjectAccess, _formname = _str_idfObjectAccess, _type = "Int64",
              _get_func = o => o.idfObjectAccess,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfObjectAccess != newval) o.idfObjectAccess = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfObjectAccess != c.idfObjectAccess || o.IsRIRPropChanged(_str_idfObjectAccess, c)) 
                  m.Add(_str_idfObjectAccess, o.ObjectIdent + _str_idfObjectAccess, o.ObjectIdent2 + _str_idfObjectAccess, o.ObjectIdent3 + _str_idfObjectAccess, "Int64", 
                    o.idfObjectAccess == null ? "" : o.idfObjectAccess.ToString(),                  
                  o.IsReadOnly(_str_idfObjectAccess), o.IsInvisible(_str_idfObjectAccess), o.IsRequired(_str_idfObjectAccess)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_FunctionName, _formname = _str_FunctionName, _type = "String",
              _get_func = o => o.FunctionName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.FunctionName != newval) o.FunctionName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.FunctionName != c.FunctionName || o.IsRIRPropChanged(_str_FunctionName, c)) 
                  m.Add(_str_FunctionName, o.ObjectIdent + _str_FunctionName, o.ObjectIdent2 + _str_FunctionName, o.ObjectIdent3 + _str_FunctionName, "String", 
                    o.FunctionName == null ? "" : o.FunctionName.ToString(),                  
                  o.IsReadOnly(_str_FunctionName), o.IsInvisible(_str_FunctionName), o.IsRequired(_str_FunctionName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsObjectOperation, _formname = _str_idfsObjectOperation, _type = "Int64?",
              _get_func = o => o.idfsObjectOperation,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsObjectOperation != newval) o.idfsObjectOperation = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsObjectOperation != c.idfsObjectOperation || o.IsRIRPropChanged(_str_idfsObjectOperation, c)) 
                  m.Add(_str_idfsObjectOperation, o.ObjectIdent + _str_idfsObjectOperation, o.ObjectIdent2 + _str_idfsObjectOperation, o.ObjectIdent3 + _str_idfsObjectOperation, "Int64?", 
                    o.idfsObjectOperation == null ? "" : o.idfsObjectOperation.ToString(),                  
                  o.IsReadOnly(_str_idfsObjectOperation), o.IsInvisible(_str_idfsObjectOperation), o.IsRequired(_str_idfsObjectOperation)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_ObjectOperationName, _formname = _str_ObjectOperationName, _type = "String",
              _get_func = o => o.ObjectOperationName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.ObjectOperationName != newval) o.ObjectOperationName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.ObjectOperationName != c.ObjectOperationName || o.IsRIRPropChanged(_str_ObjectOperationName, c)) 
                  m.Add(_str_ObjectOperationName, o.ObjectIdent + _str_ObjectOperationName, o.ObjectIdent2 + _str_ObjectOperationName, o.ObjectIdent3 + _str_ObjectOperationName, "String", 
                    o.ObjectOperationName == null ? "" : o.ObjectOperationName.ToString(),                  
                  o.IsReadOnly(_str_ObjectOperationName), o.IsInvisible(_str_ObjectOperationName), o.IsRequired(_str_ObjectOperationName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsObjectType, _formname = _str_idfsObjectType, _type = "Int64?",
              _get_func = o => o.idfsObjectType,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsObjectType != newval) o.idfsObjectType = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsObjectType != c.idfsObjectType || o.IsRIRPropChanged(_str_idfsObjectType, c)) 
                  m.Add(_str_idfsObjectType, o.ObjectIdent + _str_idfsObjectType, o.ObjectIdent2 + _str_idfsObjectType, o.ObjectIdent3 + _str_idfsObjectType, "Int64?", 
                    o.idfsObjectType == null ? "" : o.idfsObjectType.ToString(),                  
                  o.IsReadOnly(_str_idfsObjectType), o.IsInvisible(_str_idfsObjectType), o.IsRequired(_str_idfsObjectType)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsObjectID, _formname = _str_idfsObjectID, _type = "Int64?",
              _get_func = o => o.idfsObjectID,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfsObjectID != newval) o.idfsObjectID = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsObjectID != c.idfsObjectID || o.IsRIRPropChanged(_str_idfsObjectID, c)) 
                  m.Add(_str_idfsObjectID, o.ObjectIdent + _str_idfsObjectID, o.ObjectIdent2 + _str_idfsObjectID, o.ObjectIdent3 + _str_idfsObjectID, "Int64?", 
                    o.idfsObjectID == null ? "" : o.idfsObjectID.ToString(),                  
                  o.IsReadOnly(_str_idfsObjectID), o.IsInvisible(_str_idfsObjectID), o.IsRequired(_str_idfsObjectID)); 
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
              _name = _str_strSiteID, _formname = _str_strSiteID, _type = "String",
              _get_func = o => o.strSiteID,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strSiteID != newval) o.strSiteID = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strSiteID != c.strSiteID || o.IsRIRPropChanged(_str_strSiteID, c)) 
                  m.Add(_str_strSiteID, o.ObjectIdent + _str_strSiteID, o.ObjectIdent2 + _str_strSiteID, o.ObjectIdent3 + _str_strSiteID, "String", 
                    o.strSiteID == null ? "" : o.strSiteID.ToString(),                  
                  o.IsReadOnly(_str_strSiteID), o.IsInvisible(_str_strSiteID), o.IsRequired(_str_strSiteID)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strSiteName, _formname = _str_strSiteName, _type = "String",
              _get_func = o => o.strSiteName,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strSiteName != newval) o.strSiteName = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strSiteName != c.strSiteName || o.IsRIRPropChanged(_str_strSiteName, c)) 
                  m.Add(_str_strSiteName, o.ObjectIdent + _str_strSiteName, o.ObjectIdent2 + _str_strSiteName, o.ObjectIdent3 + _str_strSiteName, "String", 
                    o.strSiteName == null ? "" : o.strSiteName.ToString(),                  
                  o.IsReadOnly(_str_strSiteName), o.IsInvisible(_str_strSiteName), o.IsRequired(_str_strSiteName)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfEmployee, _formname = _str_idfEmployee, _type = "Int64?",
              _get_func = o => o.idfEmployee,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64Nullable(val); if (o.idfEmployee != newval) o.idfEmployee = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfEmployee != c.idfEmployee || o.IsRIRPropChanged(_str_idfEmployee, c)) 
                  m.Add(_str_idfEmployee, o.ObjectIdent + _str_idfEmployee, o.ObjectIdent2 + _str_idfEmployee, o.ObjectIdent3 + _str_idfEmployee, "Int64?", 
                    o.idfEmployee == null ? "" : o.idfEmployee.ToString(),                  
                  o.IsReadOnly(_str_idfEmployee), o.IsInvisible(_str_idfEmployee), o.IsRequired(_str_idfEmployee)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_isAllow, _formname = _str_isAllow, _type = "Boolean?",
              _get_func = o => o.isAllow,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBooleanNullable(val); if (o.isAllow != newval) o.isAllow = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.isAllow != c.isAllow || o.IsRIRPropChanged(_str_isAllow, c)) 
                  m.Add(_str_isAllow, o.ObjectIdent + _str_isAllow, o.ObjectIdent2 + _str_isAllow, o.ObjectIdent3 + _str_isAllow, "Boolean?", 
                    o.isAllow == null ? "" : o.isAllow.ToString(),                  
                  o.IsReadOnly(_str_isAllow), o.IsInvisible(_str_isAllow), o.IsRequired(_str_isAllow)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_isDeny, _formname = _str_isDeny, _type = "Boolean?",
              _get_func = o => o.isDeny,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBooleanNullable(val); if (o.isDeny != newval) o.isDeny = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.isDeny != c.isDeny || o.IsRIRPropChanged(_str_isDeny, c)) 
                  m.Add(_str_isDeny, o.ObjectIdent + _str_isDeny, o.ObjectIdent2 + _str_isDeny, o.ObjectIdent3 + _str_isDeny, "Boolean?", 
                    o.isDeny == null ? "" : o.isDeny.ToString(),                  
                  o.IsReadOnly(_str_isDeny), o.IsInvisible(_str_isDeny), o.IsRequired(_str_isDeny)); 
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
            ObjectAccess obj = (ObjectAccess)o;
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
        internal string m_ObjectName = "ObjectAccess";

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
            var ret = base.Clone() as ObjectAccess;
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
            var ret = base.Clone() as ObjectAccess;
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
        public ObjectAccess CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as ObjectAccess;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfObjectAccess; } }
        public string KeyName { get { return "idfObjectAccess"; } }
        public object KeyLookup { get { return idfObjectAccess; } }
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

        private bool IsRIRPropChanged(string fld, ObjectAccess c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, ObjectAccess c)
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
        

      

        public ObjectAccess()
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ObjectAccess_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(ObjectAccess_PropertyChanged);
        }
        private void ObjectAccess_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as ObjectAccess).Changed(e.PropertyName);
            
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
            ObjectAccess obj = this;
            
        }
        private void _DeletedExtenders()
        {
            ObjectAccess obj = this;
            
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

    
        private static string[] readonly_names1 = "FunctionName".Split(new char[] { ',' });
        
        private static string[] readonly_names2 = "ObjectOperationName".Split(new char[] { ',' });
        
        private static string[] readonly_names3 = "isDeny".Split(new char[] { ',' });
        
        private static string[] readonly_names4 = "isAllow".Split(new char[] { ',' });
        
        private bool _isReadOnly(string name)
        {
            
            if (readonly_names1.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<ObjectAccess, bool>(c => true)(this);
            
            if (readonly_names2.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<ObjectAccess, bool>(c => true)(this);
            
            if (readonly_names3.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<ObjectAccess, bool>(c => false)(this);
            
            if (readonly_names4.Where(c => c == name).Count() > 0)
                return ReadOnly || new Func<ObjectAccess, bool>(c => false)(this);
            
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


        internal Dictionary<string, Func<ObjectAccess, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<ObjectAccess, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<ObjectAccess, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<ObjectAccess, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<ObjectAccess, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<ObjectAccess, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<ObjectAccess, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~ObjectAccess()
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
        public class ObjectAccessGridModel : IGridModelItem
        {
            public string ErrorMessage { get; set; }
            public long ItemKey { get; set; }
        
            public Int64 idfObjectAccess { get; set; }
        
            public String FunctionName { get; set; }
        
            public String ObjectOperationName { get; set; }
        
            public Boolean? isDeny { get; set; }
        
            public Boolean? isAllow { get; set; }
        
        }
        public partial class ObjectAccessGridModelList : List<ObjectAccessGridModel>, IGridModelList, IGridModelListLoad
        {
            public long ListKey { get; protected set; }
            public List<string> Columns { get; protected set; }
            public List<string> Hiddens { get; protected set; }
            public Dictionary<string, string> Labels { get; protected set; }
            public Dictionary<string, ActionMetaItem> Actions { get; protected set; }
            public List<string> Keys { get; protected set; }
            public bool IsHiddenPersonalData(string name) { return Meta._isHiddenPersonalData(name); }
            public IObjectMeta ObjectMeta { get { return Accessor.Instance(null); } }
            public ObjectAccessGridModelList()
            {
            }
            public void Load(long key, IEnumerable items, string errMes)
            {
                LoadGridModelList(key, items as IEnumerable<ObjectAccess>, errMes);
            }
            public ObjectAccessGridModelList(long key, IEnumerable items, string errMes)
            {
                LoadGridModelList(key, items as IEnumerable<ObjectAccess>, errMes);
            }
            public ObjectAccessGridModelList(long key, IEnumerable<ObjectAccess> items)
            {
                LoadGridModelList(key, items, null);
            }
            public ObjectAccessGridModelList(long key)
            {
                LoadGridModelList(key, new List<ObjectAccess>(), null);
            }
            partial void filter(List<ObjectAccess> items);
            private void LoadGridModelList(long key, IEnumerable<ObjectAccess> items, string errMes)
            {
                ListKey = key;
                
                Columns = new List<string> {_str_FunctionName,_str_ObjectOperationName,_str_isDeny,_str_isAllow};
                    
                Hiddens = new List<string> {_str_idfObjectAccess};
                Keys = new List<string> {_str_idfObjectAccess};
                Labels = new Dictionary<string, string> {{_str_FunctionName, _str_FunctionName},{_str_ObjectOperationName, _str_ObjectOperationName},{_str_isDeny, _str_isDeny},{_str_isAllow, _str_isAllow}};
                Actions = new Dictionary<string, ActionMetaItem> {};
                ObjectAccess.Meta.Actions.ForEach(a => Actions.Add("@" + a.Name, a));
                var list = new List<ObjectAccess>(items);
                filter(list);
                AddRange(list.Where(c => !c.IsMarkedToDelete).Select(c => new ObjectAccessGridModel()
                {
                    ItemKey=c.idfObjectAccess,FunctionName=c.FunctionName,ObjectOperationName=c.ObjectOperationName,isDeny=c.isDeny,isAllow=c.isAllow
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
        : DataAccessor<ObjectAccess>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectPermissions
            
            , IObjectCreator
            , IObjectCreator<ObjectAccess>
            
            , IObjectSelectDetailList
            , IObjectPost
                
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfObjectAccess"; } }
            #endregion
        
            public delegate void on_action(ObjectAccess obj);
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
            
            public virtual List<IObject> SelectDetailList(DbManagerProxy manager, object ident, int? HACode = null)
            {
                return _SelectList(manager
                    , (Int64?)ident
                        
                    , null
                    , null
                    ).Cast<IObject>().ToList();
            }
            
        
            public virtual List<ObjectAccess> SelectList(DbManagerProxy manager
                , Int64? idfEmployee
                )
            {
                return _SelectList(manager
                    , idfEmployee
                    , delegate(ObjectAccess obj)
                        {
                        }
                    , delegate(ObjectAccess obj)
                        {
                        }
                    );
            }

            

            public List<ObjectAccess> _SelectList(DbManagerProxy manager
                , Int64? idfEmployee
                , on_action loading, on_action loaded
                )
            {
                var ret = _SelectListInternal(manager
                    , idfEmployee
                    , loading
                    , loaded
                    );
                  
                  return ret;
            }
      
            
            public virtual List<ObjectAccess> _SelectListInternal(DbManagerProxy manager
                , Int64? idfEmployee
                , on_action loading, on_action loaded
                )
            {
                ObjectAccess _obj = null;
                try
                {
                    MapResultSet[] sets = new MapResultSet[1];
                    List<ObjectAccess> objs = new List<ObjectAccess>();
                    sets[0] = new MapResultSet(typeof(ObjectAccess), objs);
                    
                    manager
                        .SetSpCommand("spObjectAccess_SelectDetail"
                            , manager.Parameter("@idfEmployee", idfEmployee)
                            , manager.Parameter("@LangID", ModelUserContext.CurrentLanguage)
                            
                            )
                        .ExecuteResultSet(sets);
                    foreach(var obj in objs) 
                    {
                        _obj = obj;
                        obj.m_CS = m_CS;
                        
                        if (loading != null)
                            loading(obj);
                        _SetupLoad(manager, obj);
                        
                        if (loaded != null)
                            loaded(obj);
                    }
                    
                    return objs;
                }
                catch(DataException e)
                {
                    throw DbModelException.Create(_obj, e);
                }
            }
    
        
        
            internal void _SetupLoad(DbManagerProxy manager, ObjectAccess obj, bool bCloning = false)
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
            
            internal void _SetPermitions(IObjectPermissions permissions, ObjectAccess obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                    }
                }
            }

    

            private ObjectAccess _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                ObjectAccess obj = null;
                try
                {
                    obj = ObjectAccess.CreateInstance();
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

            
            public ObjectAccess CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public ObjectAccess CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public ObjectAccess CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            private void _SetupChildHandlers(ObjectAccess obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(ObjectAccess obj)
            {
                
                obj.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                    {
                    
                        if (e.PropertyName == _str_isDeny)
                        {
                    if (obj.isDeny.HasValue && obj.isDeny.Value) obj.isAllow = false;
                        }
                    
                        if (e.PropertyName == _str_isAllow)
                        {
                    if (obj.isAllow.HasValue && obj.isAllow.Value) obj.isDeny = false;
                        }
                    
                    };
                
            }
    

            internal void _LoadLookups(DbManagerProxy manager, ObjectAccess obj)
            {
                
            }
    
            [SprocName("spObjectAccess_Post")]
            protected abstract void _post(DbManagerProxy manager, int Action, 
                [Direction.InputOutput("idfObjectAccess")] ObjectAccess obj);
            protected void _postPredicate(DbManagerProxy manager, int Action, 
                [Direction.InputOutput("idfObjectAccess")] ObjectAccess obj)
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
                        ObjectAccess bo = obj as ObjectAccess;
                        
                        if (!bo.IsNew && bo.IsMarkedToDelete) // delete
                        {
                            if (!CanDelete)
                                throw new PermissionException("SystemFunction", "Delete");
                        }
                        else if (bo.IsNew && !bo.IsMarkedToDelete) // insert
                        {
                            if (!CanInsert)
                                throw new PermissionException("SystemFunction", "Insert");
                        }
                        else if (!bo.IsMarkedToDelete && bo.HasChanges) // update
                        {
                            if (!CanUpdate)
                                throw new PermissionException("SystemFunction", "Update");
                        }
                        
                        long mainObject = bo.idfObjectAccess;
                        
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
                            
                            eidss.model.Enums.EIDSSAuditObject objectType = eidss.model.Enums.EIDSSAuditObject.daoSystemFunction;
                            eidss.model.Enums.AuditTable auditTable = eidss.model.Enums.AuditTable.trtSystemFunction;
                            manager.AuditParams = new object[] { auditEventType, objectType, auditTable, mainObject };
                            
                            bTransactionStarted = true;
                            manager.BeginTransaction();
                        }
                        bSuccess = _PostNonTransaction(manager, obj as ObjectAccess, bChildObject);
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
            private bool _PostNonTransaction(DbManagerProxy manager, ObjectAccess obj, bool bChildObject) 
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
            
            public bool ValidateCanDelete(ObjectAccess obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    return ValidateCanDelete(manager, obj);
                }
            }
            public bool ValidateCanDelete(DbManagerProxy manager, ObjectAccess obj)
            {
            
                return true;
            }
        
      
            protected ValidationModelException ChainsValidate(ObjectAccess obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(ObjectAccess obj, bool bRethrowException)
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
                return Validate(manager, obj as ObjectAccess, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, ObjectAccess obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                return true;
            }
           
    
            #region IObjectPermissions
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !CanUpdate; } }
            #endregion
            
            private void _SetupRequired(ObjectAccess obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(ObjectAccess obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as ObjectAccess) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as ObjectAccess) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "ObjectAccessDetail"; } }
            public string HelpIdWin { get { return "UserGroupForm"; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
            #region IObjectPermissions
        internal class Permissions : IObjectPermissions
        {
            private ObjectAccess m_obj;
            internal Permissions(ObjectAccess obj)
            {
                m_obj = obj;
            }
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("SystemFunction.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !(CanUpdate || (CanInsert && m_obj.IsNew)); } }
        }
            #endregion
            
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spObjectAccess_SelectDetail";
            public static string spCount = "";
            public static string spPost = "spObjectAccess_Post";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<ObjectAccess, bool>> RequiredByField = new Dictionary<string, Func<ObjectAccess, bool>>();
            public static Dictionary<string, Func<ObjectAccess, bool>> RequiredByProperty = new Dictionary<string, Func<ObjectAccess, bool>>();
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
                
                Sizes.Add(_str_FunctionName, 2000);
                Sizes.Add(_str_ObjectOperationName, 2000);
                Sizes.Add(_str_strSiteID, 36);
                Sizes.Add(_str_strSiteName, 200);
                GridMeta.Add(new GridMetaItem(
                    _str_idfObjectAccess,
                    _str_idfObjectAccess, null, false, false, true, true, true, null
                    ));
                GridMeta.Add(new GridMetaItem(
                    _str_FunctionName,
                    _str_FunctionName, null, false, true, true, true, true, null
                    ));
                GridMeta.Add(new GridMetaItem(
                    _str_ObjectOperationName,
                    _str_ObjectOperationName, null, false, true, true, true, true, null
                    ));
                GridMeta.Add(new GridMetaItem(
                    _str_isDeny,
                    _str_isDeny, null, false, true, true, true, true, null
                    ));
                GridMeta.Add(new GridMetaItem(
                    _str_isAllow,
                    _str_isAllow, null, false, true, true, true, true, null
                    ));
                Actions.Add(new ActionMetaItem(
                    "Ok",
                    ActionTypes.Ok,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, c),
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
	