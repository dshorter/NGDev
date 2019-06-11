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
    public abstract partial class BaseReference : 
        EditableObject<BaseReference>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfsBaseReference), NonUpdatable, PrimaryKey]
        public abstract Int64 idfsBaseReference { get; set; }
                
        [LocalizedDisplayName(_str_idfsReferenceType)]
        [MapField(_str_idfsReferenceType)]
        public abstract Int64 idfsReferenceType { get; set; }
        protected Int64 idfsReferenceType_Original { get { return ((EditableValue<Int64>)((dynamic)this)._idfsReferenceType).OriginalValue; } }
        protected Int64 idfsReferenceType_Previous { get { return ((EditableValue<Int64>)((dynamic)this)._idfsReferenceType).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strBaseReferenceCode)]
        [MapField(_str_strBaseReferenceCode)]
        public abstract String strBaseReferenceCode { get; set; }
        protected String strBaseReferenceCode_Original { get { return ((EditableValue<String>)((dynamic)this)._strBaseReferenceCode).OriginalValue; } }
        protected String strBaseReferenceCode_Previous { get { return ((EditableValue<String>)((dynamic)this)._strBaseReferenceCode).PreviousValue; } }
                
        [LocalizedDisplayName(_str_name)]
        [MapField(_str_name)]
        public abstract String name { get; set; }
        protected String name_Original { get { return ((EditableValue<String>)((dynamic)this)._name).OriginalValue; } }
        protected String name_Previous { get { return ((EditableValue<String>)((dynamic)this)._name).PreviousValue; } }
                
        [LocalizedDisplayName(_str_strDefault)]
        [MapField(_str_strDefault)]
        public abstract String strDefault { get; set; }
        protected String strDefault_Original { get { return ((EditableValue<String>)((dynamic)this)._strDefault).OriginalValue; } }
        protected String strDefault_Previous { get { return ((EditableValue<String>)((dynamic)this)._strDefault).PreviousValue; } }
                
        [LocalizedDisplayName(_str_intHACode)]
        [MapField(_str_intHACode)]
        public abstract Int32? intHACode { get; set; }
        protected Int32? intHACode_Original { get { return ((EditableValue<Int32?>)((dynamic)this)._intHACode).OriginalValue; } }
        protected Int32? intHACode_Previous { get { return ((EditableValue<Int32?>)((dynamic)this)._intHACode).PreviousValue; } }
                
        [LocalizedDisplayName(_str_intOrder)]
        [MapField(_str_intOrder)]
        public abstract Int32? intOrder { get; set; }
        protected Int32? intOrder_Original { get { return ((EditableValue<Int32?>)((dynamic)this)._intOrder).OriginalValue; } }
        protected Int32? intOrder_Previous { get { return ((EditableValue<Int32?>)((dynamic)this)._intOrder).PreviousValue; } }
                
        [LocalizedDisplayName(_str_intRowStatus)]
        [MapField(_str_intRowStatus)]
        public abstract Int32 intRowStatus { get; set; }
        protected Int32 intRowStatus_Original { get { return ((EditableValue<Int32>)((dynamic)this)._intRowStatus).OriginalValue; } }
        protected Int32 intRowStatus_Previous { get { return ((EditableValue<Int32>)((dynamic)this)._intRowStatus).PreviousValue; } }
                
        [LocalizedDisplayName(_str_blnSystem)]
        [MapField(_str_blnSystem)]
        public abstract Boolean? blnSystem { get; set; }
        protected Boolean? blnSystem_Original { get { return ((EditableValue<Boolean?>)((dynamic)this)._blnSystem).OriginalValue; } }
        protected Boolean? blnSystem_Previous { get { return ((EditableValue<Boolean?>)((dynamic)this)._blnSystem).PreviousValue; } }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<BaseReference, object> _get_func;
            internal Action<BaseReference, string> _set_func;
            internal Action<BaseReference, BaseReference, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfsBaseReference = "idfsBaseReference";
        internal const string _str_idfsReferenceType = "idfsReferenceType";
        internal const string _str_strBaseReferenceCode = "strBaseReferenceCode";
        internal const string _str_name = "name";
        internal const string _str_strDefault = "strDefault";
        internal const string _str_intHACode = "intHACode";
        internal const string _str_intOrder = "intOrder";
        internal const string _str_intRowStatus = "intRowStatus";
        internal const string _str_blnSystem = "blnSystem";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfsBaseReference, _formname = _str_idfsBaseReference, _type = "Int64",
              _get_func = o => o.idfsBaseReference,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfsBaseReference != newval) o.idfsBaseReference = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsBaseReference != c.idfsBaseReference || o.IsRIRPropChanged(_str_idfsBaseReference, c)) 
                  m.Add(_str_idfsBaseReference, o.ObjectIdent + _str_idfsBaseReference, o.ObjectIdent2 + _str_idfsBaseReference, o.ObjectIdent3 + _str_idfsBaseReference, "Int64", 
                    o.idfsBaseReference == null ? "" : o.idfsBaseReference.ToString(),                  
                  o.IsReadOnly(_str_idfsBaseReference), o.IsInvisible(_str_idfsBaseReference), o.IsRequired(_str_idfsBaseReference)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_idfsReferenceType, _formname = _str_idfsReferenceType, _type = "Int64",
              _get_func = o => o.idfsReferenceType,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfsReferenceType != newval) o.idfsReferenceType = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsReferenceType != c.idfsReferenceType || o.IsRIRPropChanged(_str_idfsReferenceType, c)) 
                  m.Add(_str_idfsReferenceType, o.ObjectIdent + _str_idfsReferenceType, o.ObjectIdent2 + _str_idfsReferenceType, o.ObjectIdent3 + _str_idfsReferenceType, "Int64", 
                    o.idfsReferenceType == null ? "" : o.idfsReferenceType.ToString(),                  
                  o.IsReadOnly(_str_idfsReferenceType), o.IsInvisible(_str_idfsReferenceType), o.IsRequired(_str_idfsReferenceType)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strBaseReferenceCode, _formname = _str_strBaseReferenceCode, _type = "String",
              _get_func = o => o.strBaseReferenceCode,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strBaseReferenceCode != newval) o.strBaseReferenceCode = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strBaseReferenceCode != c.strBaseReferenceCode || o.IsRIRPropChanged(_str_strBaseReferenceCode, c)) 
                  m.Add(_str_strBaseReferenceCode, o.ObjectIdent + _str_strBaseReferenceCode, o.ObjectIdent2 + _str_strBaseReferenceCode, o.ObjectIdent3 + _str_strBaseReferenceCode, "String", 
                    o.strBaseReferenceCode == null ? "" : o.strBaseReferenceCode.ToString(),                  
                  o.IsReadOnly(_str_strBaseReferenceCode), o.IsInvisible(_str_strBaseReferenceCode), o.IsRequired(_str_strBaseReferenceCode)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_name, _formname = _str_name, _type = "String",
              _get_func = o => o.name,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.name != newval) o.name = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.name != c.name || o.IsRIRPropChanged(_str_name, c)) 
                  m.Add(_str_name, o.ObjectIdent + _str_name, o.ObjectIdent2 + _str_name, o.ObjectIdent3 + _str_name, "String", 
                    o.name == null ? "" : o.name.ToString(),                  
                  o.IsReadOnly(_str_name), o.IsInvisible(_str_name), o.IsRequired(_str_name)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_strDefault, _formname = _str_strDefault, _type = "String",
              _get_func = o => o.strDefault,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseString(val); if (o.strDefault != newval) o.strDefault = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.strDefault != c.strDefault || o.IsRIRPropChanged(_str_strDefault, c)) 
                  m.Add(_str_strDefault, o.ObjectIdent + _str_strDefault, o.ObjectIdent2 + _str_strDefault, o.ObjectIdent3 + _str_strDefault, "String", 
                    o.strDefault == null ? "" : o.strDefault.ToString(),                  
                  o.IsReadOnly(_str_strDefault), o.IsInvisible(_str_strDefault), o.IsRequired(_str_strDefault)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_intHACode, _formname = _str_intHACode, _type = "Int32?",
              _get_func = o => o.intHACode,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt32Nullable(val); if (o.intHACode != newval) o.intHACode = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.intHACode != c.intHACode || o.IsRIRPropChanged(_str_intHACode, c)) 
                  m.Add(_str_intHACode, o.ObjectIdent + _str_intHACode, o.ObjectIdent2 + _str_intHACode, o.ObjectIdent3 + _str_intHACode, "Int32?", 
                    o.intHACode == null ? "" : o.intHACode.ToString(),                  
                  o.IsReadOnly(_str_intHACode), o.IsInvisible(_str_intHACode), o.IsRequired(_str_intHACode)); 
                  }
              }, 
                  
            new field_info {
              _name = _str_intOrder, _formname = _str_intOrder, _type = "Int32?",
              _get_func = o => o.intOrder,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt32Nullable(val); if (o.intOrder != newval) o.intOrder = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.intOrder != c.intOrder || o.IsRIRPropChanged(_str_intOrder, c)) 
                  m.Add(_str_intOrder, o.ObjectIdent + _str_intOrder, o.ObjectIdent2 + _str_intOrder, o.ObjectIdent3 + _str_intOrder, "Int32?", 
                    o.intOrder == null ? "" : o.intOrder.ToString(),                  
                  o.IsReadOnly(_str_intOrder), o.IsInvisible(_str_intOrder), o.IsRequired(_str_intOrder)); 
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
                  
            new field_info {
              _name = _str_blnSystem, _formname = _str_blnSystem, _type = "Boolean?",
              _get_func = o => o.blnSystem,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseBooleanNullable(val); if (o.blnSystem != newval) o.blnSystem = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.blnSystem != c.blnSystem || o.IsRIRPropChanged(_str_blnSystem, c)) 
                  m.Add(_str_blnSystem, o.ObjectIdent + _str_blnSystem, o.ObjectIdent2 + _str_blnSystem, o.ObjectIdent3 + _str_blnSystem, "Boolean?", 
                    o.blnSystem == null ? "" : o.blnSystem.ToString(),                  
                  o.IsReadOnly(_str_blnSystem), o.IsInvisible(_str_blnSystem), o.IsRequired(_str_blnSystem)); 
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
            BaseReference obj = (BaseReference)o;
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
        internal string m_ObjectName = "BaseReference";

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
            var ret = base.Clone() as BaseReference;
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
            var ret = base.Clone() as BaseReference;
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
        public BaseReference CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as BaseReference;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfsBaseReference; } }
        public string KeyName { get { return "idfsBaseReference"; } }
        public object KeyLookup { get { return idfsBaseReference; } }
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

        private bool IsRIRPropChanged(string fld, BaseReference c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, BaseReference c)
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
        

      
        public override string ToString()
        {
            return new Func<BaseReference, string>(c => c.name)(this);
        }
        

        public BaseReference()
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(BaseReference_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(BaseReference_PropertyChanged);
        }
        private void BaseReference_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as BaseReference).Changed(e.PropertyName);
            
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
            BaseReference obj = this;
            
        }
        private void _DeletedExtenders()
        {
            BaseReference obj = this;
            
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


        internal Dictionary<string, Func<BaseReference, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<BaseReference, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<BaseReference, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<BaseReference, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<BaseReference, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<BaseReference, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<BaseReference, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~BaseReference()
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
        : DataAccessor<BaseReference>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectCreator
            , IObjectCreator<BaseReference>
            
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfsBaseReference"; } }
            #endregion
        
            public delegate void on_action(BaseReference obj);
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
            
            public virtual List<BaseReference> SelectLookupList(DbManagerProxy manager
                )
            {
                return _SelectList(manager
                    , null, null
                    );
            }
            
            public virtual List<BaseReference> SelectList(DbManagerProxy manager
                )
            {
                return _SelectList(manager
                    , delegate(BaseReference obj)
                        {
                        }
                    , delegate(BaseReference obj)
                        {
                        }
                    );
            }

            

            public List<BaseReference> _SelectList(DbManagerProxy manager
                , on_action loading, on_action loaded
                )
            {
                var ret = _SelectListInternal(manager
                    , loading
                    , loaded
                    );
                  
                  return ret;
            }
      
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual List<BaseReference> _SelectListInternal(DbManagerProxy manager
                , on_action loading, on_action loaded
                )
            {
                BaseReference _obj = null;
                try
                {
                    MapResultSet[] sets = new MapResultSet[1];
                    List<BaseReference> objs = new List<BaseReference>();
                    sets[0] = new MapResultSet(typeof(BaseReference), objs);
                    
                    manager
                        .SetSpCommand("spBaseReference_SelectLookup"
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
    [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftEmpty_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 0)
                    
                    .ToBaseReferenceList("rftEmpty");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftTestStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000001)
                    
                    .ToBaseReferenceList("rftTestStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftAnimalAgeList_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000005)
                    
                    .ToBaseReferenceList("rftAnimalAgeList");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftAnimalCondition_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000006)
                    
                    .ToBaseReferenceList("rftAnimalCondition");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftAnimalSex_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000007)
                    
                    .ToBaseReferenceList("rftAnimalSex");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftAvianFarmType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000008)
                    
                    .ToBaseReferenceList("rftAvianFarmType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCaseClassification_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000011)
                    
                    .ToBaseReferenceList("rftCaseClassification");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCaseType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000012)
                    
                    .ToBaseReferenceList("rftCaseType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftContact_Type_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000014)
                    
                    .ToBaseReferenceList("rftContact_Type");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSampleStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000015)
                    
                    .ToBaseReferenceList("rftSampleStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftDataAuditEventType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000016)
                    
                    .ToBaseReferenceList("rftDataAuditEventType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftDataAuditObjectType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000017)
                    
                    .ToBaseReferenceList("rftDataAuditObjectType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftDiagnosis_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000019)
                    
                    .ToBaseReferenceList("rftDiagnosis");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftEmployeeType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000023)
                    
                    .ToBaseReferenceList("rftEmployeeType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftEventType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000025)
                    
                    .ToBaseReferenceList("rftEventType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftFFType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000034)
                    
                    .ToBaseReferenceList("rftFFType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftPatientState_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000035)
                    
                    .ToBaseReferenceList("rftPatientState");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftGeoLocType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000036)
                    
                    .ToBaseReferenceList("rftGeoLocType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftGroundType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000038)
                    
                    .ToBaseReferenceList("rftGroundType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftHACodes_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000040)
                    
                    .ToBaseReferenceList("rftHACodes");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftHospStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000041)
                    
                    .ToBaseReferenceList("rftHospStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftHumanAgeType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000042)
                    
                    .ToBaseReferenceList("rftHumanAgeType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftHumanGender_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000043)
                    
                    .ToBaseReferenceList("rftHumanGender");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftInstitutionName_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000046)
                    
                    .ToBaseReferenceList("rftInstitutionName");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftNationality_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000054)
                    
                    .ToBaseReferenceList("rftNationality");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftNumberingType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000057)
                    
                    .ToBaseReferenceList("rftNumberingType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftOccupationType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000061)
                    
                    .ToBaseReferenceList("rftOccupationType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftOutbreakStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000063)
                    
                    .ToBaseReferenceList("rftOutbreakStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftOutcome_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000064)
                    
                    .ToBaseReferenceList("rftOutcome");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftOwnershipType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000065)
                    
                    .ToBaseReferenceList("rftOwnershipType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftPosition_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000073)
                    
                    .ToBaseReferenceList("rftPosition");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSiteType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000085)
                    
                    .ToBaseReferenceList("rftSiteType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSpeciesList_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000086)
                    
                    .ToBaseReferenceList("rftSpeciesList");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSampleType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000087)
                    
                    .ToBaseReferenceList("rftSampleType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftStatisticAreaType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000089)
                    
                    .ToBaseReferenceList("rftStatisticAreaType");
            }
            
            public virtual BaseReferenceList rftStatisticDataType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000090)
                    
                    .ToBaseReferenceList("rftStatisticDataType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftStatisticPeriodType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000091)
                    
                    .ToBaseReferenceList("rftStatisticPeriodType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftStorageType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000092)
                    
                    .ToBaseReferenceList("rftStorageType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftTestCategory_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000095)
                    
                    .ToBaseReferenceList("rftTestCategory");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftTestResult_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000096)
                    
                    .ToBaseReferenceList("rftTestResult");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftTestName_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000097)
                    
                    .ToBaseReferenceList("rftTestName");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftVaccinationRoute_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000098)
                    
                    .ToBaseReferenceList("rftVaccinationRoute");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftVaccinationType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000099)
                    
                    .ToBaseReferenceList("rftVaccinationType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftYesNoValue_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000100)
                    
                    .ToBaseReferenceList("rftYesNoValue");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftVetCaseLogStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000103)
                    
                    .ToBaseReferenceList("rftVetCaseLogStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftPensideTestType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000104)
                    
                    .ToBaseReferenceList("rftPensideTestType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftPensideTestResult_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000105)
                    
                    .ToBaseReferenceList("rftPensideTestResult");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftRuleInValue_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000106)
                    
                    .ToBaseReferenceList("rftRuleInValue");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftAccessionCondition_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000110)
                    
                    .ToBaseReferenceList("rftAccessionCondition");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCaseProgressStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000111)
                    
                    .ToBaseReferenceList("rftCaseProgressStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSecurityAuditAction_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000112)
                    
                    .ToBaseReferenceList("rftSecurityAuditAction");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSecurityAuditResult_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000113)
                    
                    .ToBaseReferenceList("rftSecurityAuditResult");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSecurityAuditProcessType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000114)
                    
                    .ToBaseReferenceList("rftSecurityAuditProcessType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCampaignStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000115)
                    
                    .ToBaseReferenceList("rftCampaignStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCampaignType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000116)
                    
                    .ToBaseReferenceList("rftCampaignType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftMonitoringSessionStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000117)
                    
                    .ToBaseReferenceList("rftMonitoringSessionStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftMonitoringSessionActionType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000127)
                    
                    .ToBaseReferenceList("rftMonitoringSessionActionType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftMonitoringSessionActionStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000128)
                    
                    .ToBaseReferenceList("rftMonitoringSessionActionStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftVectorSurveillanceSessionStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000133)
                    
                    .ToBaseReferenceList("rftVectorSurveillanceSessionStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftPensideTestCategory_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000134)
                    
                    .ToBaseReferenceList("rftPensideTestCategory");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCollectionMethod_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000135)
                    
                    .ToBaseReferenceList("rftCollectionMethod");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCollectionTimePeriod_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000136)
                    
                    .ToBaseReferenceList("rftCollectionTimePeriod");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBasisOfRecord_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000137)
                    
                    .ToBaseReferenceList("rftBasisOfRecord");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftIdentificationMethod_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000138)
                    
                    .ToBaseReferenceList("rftIdentificationMethod");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSurrounding_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000139)
                    
                    .ToBaseReferenceList("rftSurrounding");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftVectorType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000140)
                    
                    .ToBaseReferenceList("rftVectorType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftVectorSubType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000141)
                    
                    .ToBaseReferenceList("rftVectorSubType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftCaseReportType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000144)
                    
                    .ToBaseReferenceList("rftCaseReportType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftStatisticalAgeGroup_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000145)
                    
                    .ToBaseReferenceList("rftStatisticalAgeGroup");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftChangeDiagnosisReason_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000147)
                    
                    .ToBaseReferenceList("rftChangeDiagnosisReason");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftNonNotifiableDiagnosis_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000149)
                    
                    .ToBaseReferenceList("rftNonNotifiableDiagnosis");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftNotCollectedReason_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000150)
                    
                    .ToBaseReferenceList("rftNotCollectedReason");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBirdStatus_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000151)
                    
                    .ToBaseReferenceList("rftBirdStatus");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftDiagnosisGroup_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000156)
                    
                    .ToBaseReferenceList("rftDiagnosisGroup");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftDestructionMethod_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000157)
                    
                    .ToBaseReferenceList("rftDestructionMethod");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSampleKind_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000158)
                    
                    .ToBaseReferenceList("rftSampleKind");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBssType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000159)
                    
                    .ToBaseReferenceList("rftBssType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBssMethodOfMeasurement_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000160)
                    
                    .ToBaseReferenceList("rftBssMethodOfMeasurement");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBssOutcome_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000161)
                    
                    .ToBaseReferenceList("rftBssOutcome");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBssTestResult_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000162)
                    
                    .ToBaseReferenceList("rftBssTestResult");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftBssAggregateColumns_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000163)
                    
                    .ToBaseReferenceList("rftBssAggregateColumns");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftPersonIDType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000148)
                    
                    .ToBaseReferenceList("rftPersonIDType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftSummaryReportType_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000129)
                    
                    .ToBaseReferenceList("rftSummaryReportType");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftReportDiagnosisGroup_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000130)
                    
                    .ToBaseReferenceList("rftReportDiagnosisGroup");
            }
            [InstanceCache(typeof(BvCacheAspect))]
            public virtual BaseReferenceList rftReportAdditionalText_SelectList(DbManagerProxy manager
                )
            {
                return SelectLookupList(manager
                    )
                    .Where(c => c.idfsReferenceType == 19000132)
                    
                    .ToBaseReferenceList("rftReportAdditionalText");
            }
            
            public static string GetLookupTableName(string method)
            {
                switch(method)
                {
            
                    case "rftEmpty_SelectList" : return "rftEmpty";
            
                    case "rftTestStatus_SelectList" : return "rftTestStatus";
            
                    case "rftAnimalAgeList_SelectList" : return "rftAnimalAgeList";
            
                    case "rftAnimalCondition_SelectList" : return "rftAnimalCondition";
            
                    case "rftAnimalSex_SelectList" : return "rftAnimalSex";
            
                    case "rftAvianFarmType_SelectList" : return "rftAvianFarmType";
            
                    case "rftCaseClassification_SelectList" : return "rftCaseClassification";
            
                    case "rftCaseType_SelectList" : return "rftCaseType";
            
                    case "rftContact_Type_SelectList" : return "rftContact_Type";
            
                    case "rftSampleStatus_SelectList" : return "rftSampleStatus";
            
                    case "rftDataAuditEventType_SelectList" : return "rftDataAuditEventType";
            
                    case "rftDataAuditObjectType_SelectList" : return "rftDataAuditObjectType";
            
                    case "rftDiagnosis_SelectList" : return "rftDiagnosis";
            
                    case "rftEmployeeType_SelectList" : return "rftEmployeeType";
            
                    case "rftEventType_SelectList" : return "rftEventType";
            
                    case "rftFFType_SelectList" : return "rftFFType";
            
                    case "rftPatientState_SelectList" : return "rftPatientState";
            
                    case "rftGeoLocType_SelectList" : return "rftGeoLocType";
            
                    case "rftGroundType_SelectList" : return "rftGroundType";
            
                    case "rftHACodes_SelectList" : return "rftHA_Code_List";
            
                    case "rftHospStatus_SelectList" : return "rftHospStatus";
            
                    case "rftHumanAgeType_SelectList" : return "rftHumanAgeType";
            
                    case "rftHumanGender_SelectList" : return "rftHumanGender";
            
                    case "rftInstitutionName_SelectList" : return "rftInstitutionName";
            
                    case "rftNationality_SelectList" : return "rftNationality";
            
                    case "rftNumberingType_SelectList" : return "rftNumberingType";
            
                    case "rftOccupationType_SelectList" : return "rftOccupationType";
            
                    case "rftOutbreakStatus_SelectList" : return "rftOutbreakStatus";
            
                    case "rftOutcome_SelectList" : return "rftOutcome";
            
                    case "rftOwnershipType_SelectList" : return "rftOwnershipType";
            
                    case "rftPosition_SelectList" : return "rftPosition";
            
                    case "rftSiteType_SelectList" : return "rftSiteType";
            
                    case "rftSpeciesList_SelectList" : return "rftSpeciesList";
            
                    case "rftSampleType_SelectList" : return "rftSampleType";
            
                    case "rftStatisticAreaType_SelectList" : return "rftStatisticAreaType";
            
                    case "rftStatisticDataType_SelectList" : return "rftStatisticDataType";
            
                    case "rftStatisticPeriodType_SelectList" : return "rftStatisticPeriodType";
            
                    case "rftStorageType_SelectList" : return "rftStorageType";
            
                    case "rftTestCategory_SelectList" : return "rftTestCategory";
            
                    case "rftTestResult_SelectList" : return "rftTestResult";
            
                    case "rftTestName_SelectList" : return "rftTestName";
            
                    case "rftVaccinationRoute_SelectList" : return "rftVaccinationRoute";
            
                    case "rftVaccinationType_SelectList" : return "rftVaccinationType";
            
                    case "rftYesNoValue_SelectList" : return "rftYesNoValue";
            
                    case "rftVetCaseLogStatus_SelectList" : return "rftVetCaseLogStatus";
            
                    case "rftPensideTestType_SelectList" : return "rftPensideTestType";
            
                    case "rftPensideTestResult_SelectList" : return "rftPensideTestResult";
            
                    case "rftRuleInValue_SelectList" : return "rftRuleInValue";
            
                    case "rftAccessionCondition_SelectList" : return "rftAccessionCondition";
            
                    case "rftCaseProgressStatus_SelectList" : return "rftCaseProgressStatus";
            
                    case "rftSecurityAuditAction_SelectList" : return "rftSecurityAuditAction";
            
                    case "rftSecurityAuditResult_SelectList" : return "rftSecurityAuditResult";
            
                    case "rftSecurityAuditProcessType_SelectList" : return "rftSecurityAuditProcessType";
            
                    case "rftCampaignStatus_SelectList" : return "rftCampaignStatus";
            
                    case "rftCampaignType_SelectList" : return "rftCampaignType";
            
                    case "rftMonitoringSessionStatus_SelectList" : return "rftMonitoringSessionStatus";
            
                    case "rftMonitoringSessionActionType_SelectList" : return "rftMonitoringSessionActionType";
            
                    case "rftMonitoringSessionActionStatus_SelectList" : return "rftMonitoringSessionActionStatus";
            
                    case "rftVectorSurveillanceSessionStatus_SelectList" : return "rftVectorSurveillanceSessionStatus";
            
                    case "rftPensideTestCategory_SelectList" : return "rftPensideTestCategory";
            
                    case "rftCollectionMethod_SelectList" : return "rftCollectionMethod";
            
                    case "rftCollectionTimePeriod_SelectList" : return "rftCollectionTimePeriod";
            
                    case "rftBasisOfRecord_SelectList" : return "rftBasisOfRecord";
            
                    case "rftIdentificationMethod_SelectList" : return "rftIdentificationMethod";
            
                    case "rftSurrounding_SelectList" : return "rftSurrounding";
            
                    case "rftVectorType_SelectList" : return "rftVectorType";
            
                    case "rftVectorSubType_SelectList" : return "rftVectorSubType";
            
                    case "rftCaseReportType_SelectList" : return "rftCaseReportType";
            
                    case "rftStatisticalAgeGroup_SelectList" : return "rftStatisticalAgeGroup";
            
                    case "rftChangeDiagnosisReason_SelectList" : return "rftChangeDiagnosisReason";
            
                    case "rftNonNotifiableDiagnosis_SelectList" : return "rftNonNotifiableDiagnosis";
            
                    case "rftNotCollectedReason_SelectList" : return "rftNotCollectedReason";
            
                    case "rftBirdStatus_SelectList" : return "rftBirdStatus";
            
                    case "rftDiagnosisGroup_SelectList" : return "rftDiagnosisGroup";
            
                    case "rftDestructionMethod_SelectList" : return "rftDestructionMethod";
            
                    case "rftSampleKind_SelectList" : return "rftSampleKind";
            
                    case "rftBssType_SelectList" : return "rftBssType";
            
                    case "rftBssMethodOfMeasurement_SelectList" : return "rftBssMethodOfMeasurement";
            
                    case "rftBssOutcome_SelectList" : return "rftBssOutcome";
            
                    case "rftBssTestResult_SelectList" : return "rftBssTestResult";
            
                    case "rftBssAggregateColumns_SelectList" : return "rftBssAggregateColumns";
            
                    case "rftPersonIDType_SelectList" : return "rftPersonIDType";
            
                    case "rftSummaryReportType_SelectList" : return "rftSummaryReportType";
            
                    case "rftReportDiagnosisGroup_SelectList" : return "rftReportDiagnosisGroup";
            
                    case "rftReportAdditionalText_SelectList" : return "rftReportAdditionalText";
            
                }
                return "BaseReference";
            }
            
        
        
            internal void _SetupLoad(DbManagerProxy manager, BaseReference obj, bool bCloning = false)
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
            
            internal void _SetPermitions(IObjectPermissions permissions, BaseReference obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                    }
                }
            }

    

            private BaseReference _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                BaseReference obj = null;
                try
                {
                    obj = BaseReference.CreateInstance();
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

            
            public BaseReference CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public BaseReference CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public BaseReference CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            public BaseReference CreateDummyT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                
                if (pars == null) throw new ParamsCountException();
                if (pars.Count != 2) 
                    throw new ParamsCountException();
                if (pars[0] != null && !(pars[0] is long)) 
                    throw new TypeMismatchException("id", typeof(long));
                if (pars[1] != null && !(pars[1] is string)) 
                    throw new TypeMismatchException("name", typeof(string));
                
                return CreateDummy(manager, Parent
                    , (long)pars[0]
                    , (string)pars[1]
                    );
            }
            public IObject CreateDummy(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return CreateDummyT(manager, Parent, pars);
            }
            public BaseReference CreateDummy(DbManagerProxy manager, IObject Parent
                , long id
                , string name
                )
            {
                return _CreateNew(manager, Parent
                
                    , null
                    
                    , obj =>
                {
                obj.idfsBaseReference = new Func<BaseReference, long>(c => id)(obj);
                obj.name = new Func<BaseReference, string>(c => name)(obj);
                }
                    , obj =>
                {
                }
                );
            }
            
            private void _SetupChildHandlers(BaseReference obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(BaseReference obj)
            {
                
            }
    

            internal void _LoadLookups(DbManagerProxy manager, BaseReference obj)
            {
                
            }
    
            public bool Post(DbManagerProxy manager, IObject obj, bool bChildObject = false) 
            {
                throw new NotImplementedException();
            }
            
      
            protected ValidationModelException ChainsValidate(BaseReference obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(BaseReference obj, bool bRethrowException)
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
                return Validate(manager, obj as BaseReference, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, BaseReference obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                return true;
            }
           
    
            private void _SetupRequired(BaseReference obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(BaseReference obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as BaseReference) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as BaseReference) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "BaseReferenceDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spBaseReference_SelectLookup";
            public static string spCount = "";
            public static string spPost = "";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<BaseReference, bool>> RequiredByField = new Dictionary<string, Func<BaseReference, bool>>();
            public static Dictionary<string, Func<BaseReference, bool>> RequiredByProperty = new Dictionary<string, Func<BaseReference, bool>>();
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
                
                Sizes.Add(_str_strBaseReferenceCode, 36);
                Sizes.Add(_str_name, 2000);
                Sizes.Add(_str_strDefault, 2000);
                Actions.Add(new ActionMetaItem(
                    "CreateDummy",
                    ActionTypes.Create,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => new ActResult(true, Accessor.Instance(null).CreateDummy(manager, c, pars)),
                        
                    null,
                    
                    null,
                      false,
                    null,
                    null,
                    null,
                    null,
                    null,
                    false,
                    false,
                    null,
                    false,
                    new ActionMetaItem[] {
                      
                      }
                    
                    ));
                  
                Actions.Add(new ActionMetaItem(
                    "Create",
                    ActionTypes.Create,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(true, Accessor.Instance(null).CreateWithParams(manager, c, pars)),
                    null,
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strCreate_Id",
                        "add",
                        /*from BvMessages*/"tooltipCreate_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"tooltipCreate_Id",
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
                    "Save",
                    ActionTypes.Save,
                    false,
                    String.Empty,
                    String.Empty,
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<BaseReference>().Post(manager, (BaseReference)c), c),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<BaseReference>().Post(manager, (BaseReference)c), c),
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
                    (manager, c, pars) => new ActResult(((BaseReference)c).MarkToDelete() && ObjectAccessor.PostInterface<BaseReference>().Post(manager, (BaseReference)c), c),
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
	