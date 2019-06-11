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
    public abstract partial class VetAggregateAction : 
        EditableObject<VetAggregateAction>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfAggrCase), NonUpdatable, PrimaryKey]
        public abstract Int64 idfAggrCase { get; set; }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<VetAggregateAction, object> _get_func;
            internal Action<VetAggregateAction, string> _set_func;
            internal Action<VetAggregateAction, VetAggregateAction, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfAggrCase = "idfAggrCase";
        internal const string _str_Header = "Header";
        internal const string _str_DetailedDiagnostic = "DetailedDiagnostic";
        internal const string _str_DetailedProphylactic = "DetailedProphylactic";
        internal const string _str_DetailedSanitary = "DetailedSanitary";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfAggrCase, _formname = _str_idfAggrCase, _type = "Int64",
              _get_func = o => o.idfAggrCase,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); if (o.idfAggrCase != newval) o.idfAggrCase = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfAggrCase != c.idfAggrCase || o.IsRIRPropChanged(_str_idfAggrCase, c)) 
                  m.Add(_str_idfAggrCase, o.ObjectIdent + _str_idfAggrCase, o.ObjectIdent2 + _str_idfAggrCase, o.ObjectIdent3 + _str_idfAggrCase, "Int64", 
                    o.idfAggrCase == null ? "" : o.idfAggrCase.ToString(),                  
                  o.IsReadOnly(_str_idfAggrCase), o.IsInvisible(_str_idfAggrCase), o.IsRequired(_str_idfAggrCase)); 
                  }
              }, 
        
            new field_info {
              _name = _str_Header, _formname = _str_Header, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.Header != null) o.Header._compare(c.Header, m); }
              }, 
        
            new field_info {
              _name = _str_DetailedDiagnostic, _formname = _str_DetailedDiagnostic, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.DetailedDiagnostic != null) o.DetailedDiagnostic._compare(c.DetailedDiagnostic, m); }
              }, 
        
            new field_info {
              _name = _str_DetailedProphylactic, _formname = _str_DetailedProphylactic, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.DetailedProphylactic != null) o.DetailedProphylactic._compare(c.DetailedProphylactic, m); }
              }, 
        
            new field_info {
              _name = _str_DetailedSanitary, _formname = _str_DetailedSanitary, _type = "Relation",
              _get_func = o => null,
              _set_func = (o, val) => {},
              _compare_func = (o, c, m, g) => {
                if (o.DetailedSanitary != null) o.DetailedSanitary._compare(c.DetailedSanitary, m); }
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
            VetAggregateAction obj = (VetAggregateAction)o;
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                Accessor.Instance(null)._LoadLookups(manager, obj);
                foreach (var i in _field_infos)
                    if (i != null && i._compare_func != null) i._compare_func(this, obj, ret, manager);
            }
            return ret;
        }
        #endregion
    
        [LocalizedDisplayName(_str_Header)]
        [Relation(typeof(AggregateCaseHeader), eidss.model.Schema.AggregateCaseHeader._str_idfAggrCase, _str_idfAggrCase)]
        public AggregateCaseHeader Header
        {
            get 
            {   
                return _Header; 
            }
            set 
            {   
                _Header = value;
                if (_Header != null) 
                { 
                    _Header.m_ObjectName = _str_Header;
                    _Header.Parent = this;
                }
                idfAggrCase = _Header == null 
                        ? new Int64()
                        : _Header.idfAggrCase;
                
            }
        }
        protected AggregateCaseHeader _Header;
                    
        [LocalizedDisplayName(_str_DetailedDiagnostic)]
        [Relation(typeof(VetAggregateActionDiagnosticDetailed), "", _str_idfAggrCase)]
        public VetAggregateActionDiagnosticDetailed DetailedDiagnostic
        {
            get 
            {   
                return _DetailedDiagnostic; 
            }
            set 
            {   
                _DetailedDiagnostic = value; 
                if (_DetailedDiagnostic != null) 
                { 
                    _DetailedDiagnostic.m_ObjectName = _str_DetailedDiagnostic;
                    _DetailedDiagnostic.Parent = this;
                }
            }
        }
        protected VetAggregateActionDiagnosticDetailed _DetailedDiagnostic;
                    
        [LocalizedDisplayName(_str_DetailedProphylactic)]
        [Relation(typeof(VetAggregateActionProphylacticDetailed), "", _str_idfAggrCase)]
        public VetAggregateActionProphylacticDetailed DetailedProphylactic
        {
            get 
            {   
                return _DetailedProphylactic; 
            }
            set 
            {   
                _DetailedProphylactic = value; 
                if (_DetailedProphylactic != null) 
                { 
                    _DetailedProphylactic.m_ObjectName = _str_DetailedProphylactic;
                    _DetailedProphylactic.Parent = this;
                }
            }
        }
        protected VetAggregateActionProphylacticDetailed _DetailedProphylactic;
                    
        [LocalizedDisplayName(_str_DetailedSanitary)]
        [Relation(typeof(VetAggregateActionSanitaryDetailed), "", _str_idfAggrCase)]
        public VetAggregateActionSanitaryDetailed DetailedSanitary
        {
            get 
            {   
                return _DetailedSanitary; 
            }
            set 
            {   
                _DetailedSanitary = value; 
                if (_DetailedSanitary != null) 
                { 
                    _DetailedSanitary.m_ObjectName = _str_DetailedSanitary;
                    _DetailedSanitary.Parent = this;
                }
            }
        }
        protected VetAggregateActionSanitaryDetailed _DetailedSanitary;
                    
        private BvSelectList _getList(string name)
        {
        
            return null;
        }
    
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "VetAggregateAction";

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
        
            if (_Header != null) { _Header.Parent = this; }
                
            if (_DetailedDiagnostic != null) { _DetailedDiagnostic.Parent = this; }
                
            if (_DetailedProphylactic != null) { _DetailedProphylactic.Parent = this; }
                
            if (_DetailedSanitary != null) { _DetailedSanitary.Parent = this; }
                
        }
        partial void Cloned();
        partial void ClonedWithSetup();
        private bool bIsClone;
        public override object Clone()
        {
            var ret = base.Clone() as VetAggregateAction;
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
            var ret = base.Clone() as VetAggregateAction;
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        
            if (_Header != null)
              ret.Header = _Header.CloneWithSetup(manager, bRestricted) as AggregateCaseHeader;
                
            if (_DetailedDiagnostic != null)
              ret.DetailedDiagnostic = _DetailedDiagnostic.CloneWithSetup(manager, bRestricted) as VetAggregateActionDiagnosticDetailed;
                
            if (_DetailedProphylactic != null)
              ret.DetailedProphylactic = _DetailedProphylactic.CloneWithSetup(manager, bRestricted) as VetAggregateActionProphylacticDetailed;
                
            if (_DetailedSanitary != null)
              ret.DetailedSanitary = _DetailedSanitary.CloneWithSetup(manager, bRestricted) as VetAggregateActionSanitaryDetailed;
                
            Accessor.Instance(null)._SetupLoad(manager, ret, true);
            ret.ClonedWithSetup();
            ret.DeepAcceptChanges();
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public VetAggregateAction CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as VetAggregateAction;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfAggrCase; } }
        public string KeyName { get { return "idfAggrCase"; } }
        public object KeyLookup { get { return idfAggrCase; } }
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
        
                    || (_Header != null && _Header.HasChanges)
                
                    || (_DetailedDiagnostic != null && _DetailedDiagnostic.HasChanges)
                
                    || (_DetailedProphylactic != null && _DetailedProphylactic.HasChanges)
                
                    || (_DetailedSanitary != null && _DetailedSanitary.HasChanges)
                
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
        
            if (Header != null) Header.DeepRejectChanges();
                
            if (DetailedDiagnostic != null) DetailedDiagnostic.DeepRejectChanges();
                
            if (DetailedProphylactic != null) DetailedProphylactic.DeepRejectChanges();
                
            if (DetailedSanitary != null) DetailedSanitary.DeepRejectChanges();
                
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        
            if (_Header != null) _Header.DeepAcceptChanges();
                
            if (_DetailedDiagnostic != null) _DetailedDiagnostic.DeepAcceptChanges();
                
            if (_DetailedProphylactic != null) _DetailedProphylactic.DeepAcceptChanges();
                
            if (_DetailedSanitary != null) _DetailedSanitary.DeepAcceptChanges();
                
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
        
            if (_Header != null) _Header.SetChange();
                
            if (_DetailedDiagnostic != null) _DetailedDiagnostic.SetChange();
                
            if (_DetailedProphylactic != null) _DetailedProphylactic.SetChange();
                
            if (_DetailedSanitary != null) _DetailedSanitary.SetChange();
                
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

        private bool IsRIRPropChanged(string fld, VetAggregateAction c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, VetAggregateAction c)
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
        

      

        public VetAggregateAction()
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(VetAggregateAction_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(VetAggregateAction_PropertyChanged);
        }
        private void VetAggregateAction_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as VetAggregateAction).Changed(e.PropertyName);
            
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
            VetAggregateAction obj = this;
            
        }
        private void _DeletedExtenders()
        {
            VetAggregateAction obj = this;
            
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
        
                if (_Header != null)
                    _Header._isValid &= value;
                
                if (_DetailedDiagnostic != null)
                    _DetailedDiagnostic._isValid &= value;
                
                if (_DetailedProphylactic != null)
                    _DetailedProphylactic._isValid &= value;
                
                if (_DetailedSanitary != null)
                    _DetailedSanitary._isValid &= value;
                
            }
        }

        private bool m_readOnly;
        private bool _readOnly
        {
            get { return m_readOnly; }
            set
            {
                m_readOnly = value;
        
                if (_Header != null)
                    _Header.ReadOnly |= value;
                
                if (_DetailedDiagnostic != null)
                    _DetailedDiagnostic.ReadOnly |= value;
                
                if (_DetailedProphylactic != null)
                    _DetailedProphylactic.ReadOnly |= value;
                
                if (_DetailedSanitary != null)
                    _DetailedSanitary.ReadOnly |= value;
                
            }
        }


        internal Dictionary<string, Func<VetAggregateAction, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<VetAggregateAction, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<VetAggregateAction, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<VetAggregateAction, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<VetAggregateAction, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<VetAggregateAction, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<VetAggregateAction, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~VetAggregateAction()
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
                
                if (_Header != null)
                    Header.Dispose();
                
                if (_DetailedDiagnostic != null)
                    DetailedDiagnostic.Dispose();
                
                if (_DetailedProphylactic != null)
                    DetailedProphylactic.Dispose();
                
                if (_DetailedSanitary != null)
                    DetailedSanitary.Dispose();
                
                
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
        
            if (_Header != null) _Header.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            if (_DetailedDiagnostic != null) _DetailedDiagnostic.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            if (_DetailedProphylactic != null) _DetailedProphylactic.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            if (_DetailedSanitary != null) _DetailedSanitary.ParseFormCollection(form, bParseLookups, bParseRelations);
                
            }
            ParsedFormCollection(form);
        }
    

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<VetAggregateAction>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectPermissions
            
            , IObjectCreator
            , IObjectCreator<VetAggregateAction>
            
            , IObjectSelectDetail
            , IObjectSelectDetail<VetAggregateAction>
            , IObjectPost
            , IObjectDelete
                    
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfAggrCase"; } }
            #endregion
        
            public delegate void on_action(VetAggregateAction obj);
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
            private AggregateCaseHeader.Accessor HeaderAccessor { get { return eidss.model.Schema.AggregateCaseHeader.Accessor.Instance(m_CS); } }
            private VetAggregateActionDiagnosticDetailed.Accessor DetailedDiagnosticAccessor { get { return eidss.model.Schema.VetAggregateActionDiagnosticDetailed.Accessor.Instance(m_CS); } }
            private VetAggregateActionProphylacticDetailed.Accessor DetailedProphylacticAccessor { get { return eidss.model.Schema.VetAggregateActionProphylacticDetailed.Accessor.Instance(m_CS); } }
            private VetAggregateActionSanitaryDetailed.Accessor DetailedSanitaryAccessor { get { return eidss.model.Schema.VetAggregateActionSanitaryDetailed.Accessor.Instance(m_CS); } }
            

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
            public virtual VetAggregateAction SelectDetailT(DbManagerProxy manager, object ident, int? HACode = null)
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

            
            public virtual VetAggregateAction SelectByKey(DbManagerProxy manager
                , Int64? idfAggrCase
                )
            {
                return _SelectByKey(manager
                    , idfAggrCase
                    , null, null
                    );
            }
            

            private VetAggregateAction _SelectByKey(DbManagerProxy manager
                , Int64? idfAggrCase
                , on_action loading, on_action loaded
                )
            {
                return _SelectByKeyInternal(manager
                    , idfAggrCase
                    , loading, loaded
                    )
                    
                    ;
            }
      
            
            protected virtual VetAggregateAction _SelectByKeyInternal(DbManagerProxy manager
                , Int64? idfAggrCase
                , on_action loading, on_action loaded
                )
            {
            
                MapResultSet[] sets = new MapResultSet[1];
                List<VetAggregateAction> objs = new List<VetAggregateAction>();
                sets[0] = new MapResultSet(typeof(VetAggregateAction), objs);
                VetAggregateAction obj = null;
                try
                {
                    manager
                        .SetSpCommand("spAggregateCaseDummy_SelectDetail"
                            , manager.Parameter("@idfAggrCase", idfAggrCase)
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
                          obj._isValid = (manager.SetSpCommand("spVetAggregateAction_Validate", obj.Key).ExecuteScalar<int>(ScalarSourceType.ReturnValue) == 0);
                    

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
    
            internal void _LoadHeader(VetAggregateAction obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadHeader(manager, obj);
                }
            }
            internal void _LoadHeader(DbManagerProxy manager, VetAggregateAction obj)
            {
              
                if (obj.Header == null && obj.idfAggrCase != 0)
                {
                    obj.Header = HeaderAccessor.SelectByKey(manager
                        
                        , obj.idfAggrCase
                        , new Func<VetAggregateAction, long>(c => (long)AggregateCaseType.VetAggregateAction)(obj)
                        
                        );
                    if (obj.Header != null)
                    {
                        obj.Header.m_ObjectName = _str_Header;
                    }
                }
                    
              }
            
            internal void _LoadDetailedDiagnostic(VetAggregateAction obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadDetailedDiagnostic(manager, obj);
                }
            }
            internal void _LoadDetailedDiagnostic(DbManagerProxy manager, VetAggregateAction obj)
            {
              
                if (obj.DetailedDiagnostic == null && obj.idfAggrCase != 0)
                {
                    obj.DetailedDiagnostic = DetailedDiagnosticAccessor.SelectByKey(manager
                        
                        , obj.idfAggrCase
                        );
                    if (obj.DetailedDiagnostic != null)
                    {
                        obj.DetailedDiagnostic.m_ObjectName = _str_DetailedDiagnostic;
                    }
                }
                    
              }
            
            internal void _LoadDetailedProphylactic(VetAggregateAction obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadDetailedProphylactic(manager, obj);
                }
            }
            internal void _LoadDetailedProphylactic(DbManagerProxy manager, VetAggregateAction obj)
            {
              
                if (obj.DetailedProphylactic == null && obj.idfAggrCase != 0)
                {
                    obj.DetailedProphylactic = DetailedProphylacticAccessor.SelectByKey(manager
                        
                        , obj.idfAggrCase
                        );
                    if (obj.DetailedProphylactic != null)
                    {
                        obj.DetailedProphylactic.m_ObjectName = _str_DetailedProphylactic;
                    }
                }
                    
              }
            
            internal void _LoadDetailedSanitary(VetAggregateAction obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadDetailedSanitary(manager, obj);
                }
            }
            internal void _LoadDetailedSanitary(DbManagerProxy manager, VetAggregateAction obj)
            {
              
                if (obj.DetailedSanitary == null && obj.idfAggrCase != 0)
                {
                    obj.DetailedSanitary = DetailedSanitaryAccessor.SelectByKey(manager
                        
                        , obj.idfAggrCase
                        );
                    if (obj.DetailedSanitary != null)
                    {
                        obj.DetailedSanitary.m_ObjectName = _str_DetailedSanitary;
                    }
                }
                    
              }
            
        
        
            internal void _SetupLoad(DbManagerProxy manager, VetAggregateAction obj, bool bCloning = false)
            {
                if (obj == null) return;
                
                // loading extenters - begin
                // loading extenters - end
                if (!bCloning)
                {
                
                _LoadHeader(manager, obj);
                _LoadDetailedDiagnostic(manager, obj);
                _LoadDetailedProphylactic(manager, obj);
                _LoadDetailedSanitary(manager, obj);
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
            
            internal void _SetPermitions(IObjectPermissions permissions, VetAggregateAction obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    HeaderAccessor._SetPermitions(obj._permissions, obj.Header);
                    DetailedDiagnosticAccessor._SetPermitions(obj._permissions, obj.DetailedDiagnostic);
                    DetailedProphylacticAccessor._SetPermitions(obj._permissions, obj.DetailedProphylactic);
                    DetailedSanitaryAccessor._SetPermitions(obj._permissions, obj.DetailedSanitary);
                    
                    }
                }
            }

    

            private VetAggregateAction _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                VetAggregateAction obj = null;
                try
                {
                    obj = VetAggregateAction.CreateInstance();
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
                obj.DetailedDiagnostic = new Func<VetAggregateAction, VetAggregateActionDiagnosticDetailed>(c => DetailedDiagnosticAccessor.SelectByKey(manager, c.idfAggrCase))(obj);
                obj.DetailedProphylactic = new Func<VetAggregateAction, VetAggregateActionProphylacticDetailed>(c => DetailedProphylacticAccessor.SelectByKey(manager, c.idfAggrCase))(obj);
                obj.DetailedSanitary = new Func<VetAggregateAction, VetAggregateActionSanitaryDetailed>(c => DetailedSanitaryAccessor.SelectByKey(manager, c.idfAggrCase))(obj);
                obj.Header = new Func<VetAggregateAction, AggregateCaseHeader>(c => HeaderAccessor.CreateWithParamsManyVersions(manager, c, (long)AggregateCaseType.VetAggregateAction, obj.DetailedDiagnostic != null  ? obj.DetailedDiagnostic.idfVersion : new long?(), obj.DetailedProphylactic != null ? obj.DetailedProphylactic.idfVersion : new long?(), obj.DetailedSanitary != null ? obj.DetailedSanitary.idfVersion : new long?()))(obj);
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

            
            public VetAggregateAction CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public VetAggregateAction CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public VetAggregateAction CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            public ActResult ReportContextMenu(DbManagerProxy manager, VetAggregateAction obj, List<object> pars)
            {
                
                return ReportContextMenu(manager, obj
                    );
            }
            public ActResult ReportContextMenu(DbManagerProxy manager, VetAggregateAction obj
                )
            {
                
                if (obj != null && !obj.GetPermissions().CanExecute("ReportContextMenu"))
                    throw new PermissionException("AccessToVetAggregateAction", "ReportContextMenu");
                
                return true;
                
            }
            
      
            public ActResult VetAggregateActionReport(DbManagerProxy manager, VetAggregateAction obj, List<object> pars)
            {
                
                return VetAggregateActionReport(manager, obj
                    );
            }
            public ActResult VetAggregateActionReport(DbManagerProxy manager, VetAggregateAction obj
                )
            {
                
                if (obj != null && !obj.GetPermissions().CanExecute("VetAggregateActionReport"))
                    throw new PermissionException("AccessToVetAggregateAction", "VetAggregateActionReport");
                
                return true;
                
            }
            
      
            private void _SetupChildHandlers(VetAggregateAction obj, object newobj)
            {
                
            }
            
            private void _SetupHandlers(VetAggregateAction obj)
            {
                
            }
    

            internal void _LoadLookups(DbManagerProxy manager, VetAggregateAction obj)
            {
                
            }
    
            public bool DeleteObject(DbManagerProxy manager, object ident)
            {
                IObject obj = SelectDetail(manager, ident);
                if (!obj.MarkToDelete()) return false;
                return Post(manager, obj);
            }
        
            public bool Post(DbManagerProxy manager, IObject obj, bool bChildObject = false) 
            {
                throw new NotImplementedException();
            }
            
      
            protected ValidationModelException ChainsValidate(VetAggregateAction obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(VetAggregateAction obj, bool bRethrowException)
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
                return Validate(manager, obj as VetAggregateAction, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, VetAggregateAction obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                return true;
            }
           
    
            #region IObjectPermissions
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !CanUpdate; } }
            #endregion
            
            private void _SetupRequired(VetAggregateAction obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(VetAggregateAction obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as VetAggregateAction) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as VetAggregateAction) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "VetAggregateActionDetail"; } }
            public string HelpIdWin { get { return ""; } }
            public string HelpIdWeb { get { return "web_vetcaseaggregateactiondetailform"; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
            #region IObjectPermissions
        internal class Permissions : IObjectPermissions
        {
            private VetAggregateAction m_obj;
            internal Permissions(VetAggregateAction obj)
            {
                m_obj = obj;
            }
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("AccessToVetAggregateAction.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !(CanUpdate || (CanInsert && m_obj.IsNew)); } }
        }
            #endregion
            
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spAggregateCaseDummy_SelectDetail";
            public static string spCount = "";
            public static string spPost = "";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<VetAggregateAction, bool>> RequiredByField = new Dictionary<string, Func<VetAggregateAction, bool>>();
            public static Dictionary<string, Func<VetAggregateAction, bool>> RequiredByProperty = new Dictionary<string, Func<VetAggregateAction, bool>>();
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
                
                Actions.Add(new ActionMetaItem(
                    "ReportContextMenu",
                    ActionTypes.Action,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => Accessor.Instance(null).ReportContextMenu(manager, (VetAggregateAction)c, pars),
                        
                    null,
                    
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strReport_Id",
                        "Report",
                        /*from BvMessages*/"strReport_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        ActionsAlignment.Left,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                        ),
                      true,
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
                    "VetAggregateActionReport",
                    ActionTypes.Action,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => Accessor.Instance(null).VetAggregateActionReport(manager, (VetAggregateAction)c, pars),
                        
                    null,
                    
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"titleVetAggrActionReport",
                        "",
                        /*from BvMessages*/"",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        ActionsAlignment.Left,
                        ActionsPanelType.ContextMenu,
                        ActionsAppType.All
                        ),
                      true,
                    null,
                    null,
                    null,
                    null,
                    (o1, o2, p, r) => eidss.model.Reports.BaseMenuReportRegistrator.IsPaperFormAllowed("VetAggregateCaseActions"),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<VetAggregateAction>().Post(manager, (VetAggregateAction)c), c),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<VetAggregateAction>().Post(manager, (VetAggregateAction)c), c),
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
	