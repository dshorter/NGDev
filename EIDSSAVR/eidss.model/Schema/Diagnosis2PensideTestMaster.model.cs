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
    public abstract partial class Diagnosis2PensideTestMaster : 
        EditableObject<Diagnosis2PensideTestMaster>
        , IObject
        , IDisposable
        , ILookupUsage
        {
        
        [MapField(_str_idfsDiagnosis), NonUpdatable, PrimaryKey]
        public abstract Int64 idfsDiagnosis { get; set; }
                
        #region Set/Get values
        #region filed_info definifion
        protected class field_info
        {
            internal string _name;
            internal string _formname;
            internal string _type;
            internal Func<Diagnosis2PensideTestMaster, object> _get_func;
            internal Action<Diagnosis2PensideTestMaster, string> _set_func;
            internal Action<Diagnosis2PensideTestMaster, Diagnosis2PensideTestMaster, CompareModel, DbManagerProxy> _compare_func;
        }
        internal const string _str_Parent = "Parent";
        internal const string _str_IsNew = "IsNew";
        
        internal const string _str_idfsDiagnosis = "idfsDiagnosis";
        internal const string _str_Diagnosis = "Diagnosis";
        internal const string _str_Diagnosis2PensideTest = "Diagnosis2PensideTest";
        private static readonly field_info[] _field_infos =
        {
                  
            new field_info {
              _name = _str_idfsDiagnosis, _formname = _str_idfsDiagnosis, _type = "Int64",
              _get_func = o => o.idfsDiagnosis,
              _set_func = (o, val) => { var newval = ParsingHelper.ParseInt64(val); 
                if (o.idfsDiagnosis != newval) 
                  o.Diagnosis = o.DiagnosisLookup.FirstOrDefault(c => c.idfsDiagnosis == newval);
                if (o.idfsDiagnosis != newval) o.idfsDiagnosis = newval; },
              _compare_func = (o, c, m, g) => {
                if (o.idfsDiagnosis != c.idfsDiagnosis || o.IsRIRPropChanged(_str_idfsDiagnosis, c)) 
                  m.Add(_str_idfsDiagnosis, o.ObjectIdent + _str_idfsDiagnosis, o.ObjectIdent2 + _str_idfsDiagnosis, o.ObjectIdent3 + _str_idfsDiagnosis, "Int64", 
                    o.idfsDiagnosis == null ? "" : o.idfsDiagnosis.ToString(),                  
                  o.IsReadOnly(_str_idfsDiagnosis), o.IsInvisible(_str_idfsDiagnosis), o.IsRequired(_str_idfsDiagnosis)); 
                  }
              }, 
        
            new field_info {
              _name = _str_Diagnosis, _formname = _str_Diagnosis, _type = "Lookup",
              _get_func = o => { if (o.Diagnosis == null) return null; return o.Diagnosis.idfsDiagnosis; },
              _set_func = (o, val) => { o.Diagnosis = o.DiagnosisLookup.Where(c => c.idfsDiagnosis.ToString() == val).SingleOrDefault(); },
              _compare_func = (o, c, m, g) => {
                bool bChangeLookupContent = o.IsLookupContentChanged(g, _str_Diagnosis, c);
                if (o.idfsDiagnosis != c.idfsDiagnosis || o.IsRIRPropChanged(_str_Diagnosis, c) || bChangeLookupContent) {
                  m.Add(_str_Diagnosis, o.ObjectIdent + _str_Diagnosis, o.ObjectIdent2 + _str_Diagnosis, o.ObjectIdent3 + _str_Diagnosis, "Lookup", o.idfsDiagnosis == null ? "" : o.idfsDiagnosis.ToString(), o.IsReadOnly(_str_Diagnosis), o.IsInvisible(_str_Diagnosis), o.IsRequired(_str_Diagnosis),
                  bChangeLookupContent ? o.DiagnosisLookup.Select(i => new CompareModel.ComboBoxItem() { id = i.KeyLookup, name = i.ToStringProp }).ToList() : null);
                  }
                }
              }, 
            new field_info {
              _name = _str_Diagnosis + "Lookup", _formname = _str_Diagnosis + "Lookup", _type = "LookupContent",
              _get_func = o => o.DiagnosisLookup,
              _set_func = (o, val) => { },
              _compare_func = (o, c, m, g) => { }, 
              }, 
        
            new field_info {
              _name = _str_Diagnosis2PensideTest, _formname = _str_Diagnosis2PensideTest, _type = "Child",
              _get_func = o => null,
              _set_func = (o, val) => {
                  o.Diagnosis2PensideTest.ForEach(c => c.SetValue("blnChecked", "false")); 
                  if (!string.IsNullOrEmpty(val)) 
                      val.Split(',').ToList().ForEach(i => o.Diagnosis2PensideTest.First(c => (long)c.Key == Int64.Parse(i)).SetValue("blnChecked", "true"));
                  },
              _compare_func = (o, c, m, g) => {
                if (o.Diagnosis2PensideTest.Count != c.Diagnosis2PensideTest.Count || o.IsReadOnly(_str_Diagnosis2PensideTest) != c.IsReadOnly(_str_Diagnosis2PensideTest) || o.IsInvisible(_str_Diagnosis2PensideTest) != c.IsInvisible(_str_Diagnosis2PensideTest) || o.IsRequired(_str_Diagnosis2PensideTest) != c._isRequired(o.m_isRequired, _str_Diagnosis2PensideTest)) {
                  m.Add(_str_Diagnosis2PensideTest, o.ObjectIdent + _str_Diagnosis2PensideTest, o.ObjectIdent2 + _str_Diagnosis2PensideTest, o.ObjectIdent3 + _str_Diagnosis2PensideTest, "Child", o.idfsDiagnosis == null ? "" : o.idfsDiagnosis.ToString(), o.IsReadOnly(_str_Diagnosis2PensideTest), o.IsInvisible(_str_Diagnosis2PensideTest), o.IsRequired(_str_Diagnosis2PensideTest)); 
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
            Diagnosis2PensideTestMaster obj = (Diagnosis2PensideTestMaster)o;
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                Accessor.Instance(null)._LoadLookups(manager, obj);
                foreach (var i in _field_infos)
                    if (i != null && i._compare_func != null) i._compare_func(this, obj, ret, manager);
            }
            return ret;
        }
        #endregion
    
        [LocalizedDisplayName(_str_Diagnosis2PensideTest)]
        [Relation(typeof(Diagnosis2PensideTest), eidss.model.Schema.Diagnosis2PensideTest._str_idfsDiagnosis, _str_idfsDiagnosis)]
        public EditableList<Diagnosis2PensideTest> Diagnosis2PensideTest
        {
            get 
            {   
                return _Diagnosis2PensideTest; 
            }
            set 
            {
                _Diagnosis2PensideTest = value;
            }
        }
        protected EditableList<Diagnosis2PensideTest> _Diagnosis2PensideTest = new EditableList<Diagnosis2PensideTest>();
                    
        [LocalizedDisplayName(_str_Diagnosis)]
        [Relation(typeof(DiagnosisLookup), eidss.model.Schema.DiagnosisLookup._str_idfsDiagnosis, _str_idfsDiagnosis)]
        public DiagnosisLookup Diagnosis
        {
            get { return _Diagnosis == null ? null : ((long)_Diagnosis.Key == 0 ? null : _Diagnosis); }
            set 
            { 
                var oldVal = _Diagnosis;
                _Diagnosis = value == null ? null : ((long) value.Key == 0 ? null : value);
                if (_Diagnosis != oldVal)
                {
                    if (idfsDiagnosis != (_Diagnosis == null
                            ? new Int64()
                            : (Int64)_Diagnosis.idfsDiagnosis))
                        idfsDiagnosis = _Diagnosis == null 
                            ? new Int64()
                            : (Int64)_Diagnosis.idfsDiagnosis; 
                    OnPropertyChanged(_str_Diagnosis); 
                }
            }
        }
        private DiagnosisLookup _Diagnosis;

        
        public List<DiagnosisLookup> DiagnosisLookup
        {
            get { return _DiagnosisLookup; }
        }
        private List<DiagnosisLookup> _DiagnosisLookup = new List<DiagnosisLookup>();
            
        private BvSelectList _getList(string name)
        {
        
            switch(name)
            {
            
                case _str_Diagnosis:
                    return new BvSelectList(DiagnosisLookup, eidss.model.Schema.DiagnosisLookup._str_idfsDiagnosis, null, Diagnosis, _str_idfsDiagnosis);
            
                case _str_Diagnosis2PensideTest:
                    return new BvSelectList(Diagnosis2PensideTest, "", "", null, "");
            
            }
        
            return null;
        }
    
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "Diagnosis2PensideTestMaster";

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
        Diagnosis2PensideTest.ForEach(c => { c.Parent = this; });
                
        }
        partial void Cloned();
        partial void ClonedWithSetup();
        private bool bIsClone;
        public override object Clone()
        {
            var ret = base.Clone() as Diagnosis2PensideTestMaster;
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
            var ret = base.Clone() as Diagnosis2PensideTestMaster;
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        
            if (_Diagnosis2PensideTest != null && _Diagnosis2PensideTest.Count > 0)
            {
              ret.Diagnosis2PensideTest.Clear();
              _Diagnosis2PensideTest.ForEach(c => ret.Diagnosis2PensideTest.Add(c.CloneWithSetup(manager, bRestricted)));
            }
                
            Accessor.Instance(null)._SetupLoad(manager, ret, true);
            ret.ClonedWithSetup();
            ret.DeepAcceptChanges();
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public Diagnosis2PensideTestMaster CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as Diagnosis2PensideTestMaster;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return idfsDiagnosis; } }
        public string KeyName { get { return "idfsDiagnosis"; } }
        public object KeyLookup { get { return idfsDiagnosis; } }
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
        
                    || Diagnosis2PensideTest.IsDirty
                    || Diagnosis2PensideTest.Count(c => c.IsMarkedToDelete || c.HasChanges) > 0
                
                ;
            }
        }
        public new void RejectChanges()
        {
        
            var _prev_idfsDiagnosis_Diagnosis = idfsDiagnosis;
            base.RejectChanges();
        
            if (_prev_idfsDiagnosis_Diagnosis != idfsDiagnosis)
            {
                _Diagnosis = _DiagnosisLookup.FirstOrDefault(c => c.idfsDiagnosis == idfsDiagnosis);
            }
        }
        public void DeepRejectChanges()
        {
            RejectChanges();
        Diagnosis2PensideTest.DeepRejectChanges();
                
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        Diagnosis2PensideTest.DeepAcceptChanges();
                
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
        Diagnosis2PensideTest.ForEach(c => c.SetChange());
                
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

        private bool IsRIRPropChanged(string fld, Diagnosis2PensideTestMaster c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, Diagnosis2PensideTestMaster c)
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
        

      

        public Diagnosis2PensideTestMaster()
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
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Diagnosis2PensideTestMaster_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Diagnosis2PensideTestMaster_PropertyChanged);
        }
        private void Diagnosis2PensideTestMaster_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as Diagnosis2PensideTestMaster).Changed(e.PropertyName);
            
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
            Diagnosis2PensideTestMaster obj = this;
            
        }
        private void _DeletedExtenders()
        {
            Diagnosis2PensideTestMaster obj = this;
            
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

    
        private static string[] readonly_names1 = "Diagnosis".Split(new char[] { ',' });
        
        private bool _isReadOnly(string name)
        {
            
            if (readonly_names1.Where(c => c == name).Count() > 0)
                return  new Func<Diagnosis2PensideTestMaster, bool>(c => false)(this);
            
            return ReadOnly;
                
        }

        private bool m_isValid = true;
        internal bool _isValid
        {
            get { return m_isValid; }
            set
            {
                m_isValid = value;
        
                foreach(var o in _Diagnosis2PensideTest)
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
        
                foreach(var o in _Diagnosis2PensideTest)
                    o.ReadOnly |= value;
                
            }
        }


        internal Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>> m_isRequired;
        private bool _isRequired(Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>> isRequiredDict, string name)
        {
            if (isRequiredDict != null && isRequiredDict.ContainsKey(name))
                return isRequiredDict[name](this);
            return false;
        }
        
        public void AddRequired(string name, Func<Diagnosis2PensideTestMaster, bool> func)
        {
            if (m_isRequired == null) 
                m_isRequired = new Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>>();
            if (!m_isRequired.ContainsKey(name))
                m_isRequired.Add(name, func);
        }
    
    internal Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>> m_isHiddenPersonalData;
    private bool _isHiddenPersonalData(string name)
    {
      if (m_isHiddenPersonalData != null && m_isHiddenPersonalData.ContainsKey(name))
        return m_isHiddenPersonalData[name](this);
      return false;
    }

    public void AddHiddenPersonalData(string name, Func<Diagnosis2PensideTestMaster, bool> func)
    {
      if (m_isHiddenPersonalData == null)
        m_isHiddenPersonalData = new Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>>();
      if (!m_isHiddenPersonalData.ContainsKey(name))
        m_isHiddenPersonalData.Add(name, func);
    }
  
        #region IDisposable Members
        partial void Disposed();
        private bool bIsDisposed;
        protected bool bNeedLookupRemove;
        ~Diagnosis2PensideTestMaster()
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
                    Diagnosis2PensideTest.ForEach(c => c.Dispose());
                }
                Diagnosis2PensideTest.ClearModelListEventInvocations();
                
                
                if (bNeedLookupRemove)
                {
                
                LookupManager.RemoveObject("DiagnosisLookup", this);
                
                }
                Disposed();
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    
        #region ILookupUsage Members
        public void ReloadLookupItem(DbManagerProxy manager, string lookup_object)
        {
            
            if (lookup_object == "DiagnosisLookup")
                _getAccessor().LoadLookup_Diagnosis(manager, this);
            
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
        
            if (_Diagnosis2PensideTest != null) _Diagnosis2PensideTest.ForEach(c => c.ParseFormCollection(form, bParseLookups, bParseRelations));
                
            }
            ParsedFormCollection(form);
        }
    

        #region Accessor
        public abstract partial class Accessor
        : DataAccessor<Diagnosis2PensideTestMaster>
            , IObjectAccessor
            , IObjectMeta
            , IObjectValidator
            
            , IObjectPermissions
            
            , IObjectCreator
            , IObjectCreator<Diagnosis2PensideTestMaster>
            
            , IObjectSelectDetail
            , IObjectSelectDetail<Diagnosis2PensideTestMaster>
            , IObjectPost
            , IObjectDelete
                    
        {
            #region IObjectAccessor
            public string KeyName { get { return "idfsDiagnosis"; } }
            #endregion
        
            public delegate void on_action(Diagnosis2PensideTestMaster obj);
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
            private Diagnosis2PensideTest.Accessor Diagnosis2PensideTestAccessor { get { return eidss.model.Schema.Diagnosis2PensideTest.Accessor.Instance(m_CS); } }
            private DiagnosisLookup.Accessor DiagnosisAccessor { get { return eidss.model.Schema.DiagnosisLookup.Accessor.Instance(m_CS); } }
            

            public virtual IObject SelectDetail(DbManagerProxy manager, object ident, int? HACode = null)
            {
                if (ident == null)
                {
                    return CreateNew(manager, null, HACode);
                }
                else
                {
                    return _SelectByKey(manager
                        , null, null
                        );
                }
            }
            public virtual Diagnosis2PensideTestMaster SelectDetailT(DbManagerProxy manager, object ident, int? HACode = null)
            {
                if (ident == null)
                {
                    return CreateNewT(manager, null, HACode);
                }
                else
                {
                    return _SelectByKey(manager
                        , null, null
                        );
                }
            }

            
            public virtual Diagnosis2PensideTestMaster SelectByKey(DbManagerProxy manager
                )
            {
                return _SelectByKey(manager
                    , null, null
                    );
            }
            

            private Diagnosis2PensideTestMaster _SelectByKey(DbManagerProxy manager
                , on_action loading, on_action loaded
                )
            {
                return _SelectByKeyInternal(manager
                    , loading, loaded
                    )
                    
                    ;
            }
      
            
            protected virtual Diagnosis2PensideTestMaster _SelectByKeyInternal(DbManagerProxy manager
                , on_action loading, on_action loaded
                )
            {
            
                MapResultSet[] sets = new MapResultSet[1];
                List<Diagnosis2PensideTestMaster> objs = new List<Diagnosis2PensideTestMaster>();
                sets[0] = new MapResultSet(typeof(Diagnosis2PensideTestMaster), objs);
                Diagnosis2PensideTestMaster obj = null;
                try
                {
                    manager
                        .SetSpCommand("spDiagnosisMaster_SelectDetail"
                            )
                        .ExecuteResultSet(sets);

                    if (objs.Count == 0)
                        return null;

                    obj = objs[0];
                    obj.m_CS = m_CS;
                    
                  
                    if (loading != null)
                        loading(obj);
                    _SetupLoad(manager, obj);
                    

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
    
            private void _SetupAddChildHandlerDiagnosis2PensideTest(Diagnosis2PensideTestMaster obj)
            {
                obj.Diagnosis2PensideTest.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
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
            
            internal void _LoadDiagnosis2PensideTest(Diagnosis2PensideTestMaster obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    _LoadDiagnosis2PensideTest(manager, obj);
                }
            }
            internal void _LoadDiagnosis2PensideTest(DbManagerProxy manager, Diagnosis2PensideTestMaster obj)
            {
              
                obj.Diagnosis2PensideTest.Clear();
                obj.Diagnosis2PensideTest.AddRange(Diagnosis2PensideTestAccessor.SelectDetailList(manager
                    
                    , obj.idfsDiagnosis
                    ));
                obj.Diagnosis2PensideTest.ForEach(c => c.m_ObjectName = _str_Diagnosis2PensideTest);
                obj.Diagnosis2PensideTest.AcceptChanges();
                    
              }
            
        
        
            internal void _SetupLoad(DbManagerProxy manager, Diagnosis2PensideTestMaster obj, bool bCloning = false)
            {
                if (obj == null) return;
                
                // loading extenters - begin
                // loading extenters - end
                if (!bCloning)
                {
                
                _LoadDiagnosis2PensideTest(manager, obj);
                }
                _LoadLookups(manager, obj);
                obj._setParent();
                
                // loaded extenters - begin
                // loaded extenters - end
                
                _SetupHandlers(obj);
                _SetupChildHandlers(obj, null);
                
                _SetupAddChildHandlerDiagnosis2PensideTest(obj);
                
                _SetPermitions(obj._permissions, obj);
                _SetupRequired(obj);
                _SetupPersonalDataRestrictions(obj);
                obj._SetupMainHandler();
                obj.AcceptChanges();
            }
            
            internal void _SetPermitions(IObjectPermissions permissions, Diagnosis2PensideTestMaster obj)
            {
                if (obj != null)
                {
                    obj._permissions = permissions;
                    if (obj._permissions != null)
                    {
                    
                        obj.Diagnosis2PensideTest.ForEach(c => Diagnosis2PensideTestAccessor._SetPermitions(obj._permissions, c));
                    
                    }
                }
            }

    

            private Diagnosis2PensideTestMaster _CreateNew(DbManagerProxy manager, IObject Parent, int? HACode, on_action creating, on_action created, bool isFake = false)
            {
                Diagnosis2PensideTestMaster obj = null;
                try
                {
                    obj = Diagnosis2PensideTestMaster.CreateInstance();
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
                    
                    _SetupAddChildHandlerDiagnosis2PensideTest(obj);
                    
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

            
            public Diagnosis2PensideTestMaster CreateNewT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public IObject CreateNew(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null);
            }
            public Diagnosis2PensideTestMaster CreateFakeT(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            public IObject CreateFake(DbManagerProxy manager, IObject Parent, int? HACode = null)
            {
                return _CreateNew(manager, Parent, HACode, null, null, true);
            }
            
            public Diagnosis2PensideTestMaster CreateWithParamsT(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            public IObject CreateWithParams(DbManagerProxy manager, IObject Parent, List<object> pars)
            {
                return _CreateNew(manager, Parent, null, null, null);
            }
            
            public ActResult DeletePensideTest(DbManagerProxy manager, Diagnosis2PensideTestMaster obj, List<object> pars)
            {
                
                return DeletePensideTest(manager, obj
                    );
            }
            public ActResult DeletePensideTest(DbManagerProxy manager, Diagnosis2PensideTestMaster obj
                )
            {
                
                if (obj != null && !obj.GetPermissions().CanExecute("DeletePensideTest"))
                    throw new PermissionException("Reference", "DeletePensideTest");
                
                return true;
                
            }
            
      
            private void _SetupChildHandlers(Diagnosis2PensideTestMaster obj, object newobj)
            {
                
                foreach(var o in obj.Diagnosis2PensideTest.Where(c => true))
                {
                    if (newobj == null || newobj == o)
                    {                    
                            
                        var item = o;
                        o._RevokeMainHandler();
                        o.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                        {
                            try
                            {
                                if (e.PropertyName == "idfsPensideTestName")
                                {
                                
                (new RequiredValidator( "idfsPensideTestName", "idfsPensideTestName","",
                ValidationEventType.Error
              )).Validate(c => true, item, item.idfsPensideTestName);
            
                                }
                            }
                            catch(ValidationModelException ex)
                            {
                                if (!obj.OnValidation(ex))
                                {
                                    item.LockNotifyChanges();
                                    item.CancelMemberLastChanges("idfsPensideTestName");
                                    obj.OnValidationEnd(ex);
                                    item.UnlockNotifyChanges();
                                }
                            }
                        };
                        o._SetupMainHandler();
                        
                    }
                }
                            
            }
            
            private void _SetupHandlers(Diagnosis2PensideTestMaster obj)
            {
                
            }
    
            public void LoadLookup_Diagnosis(DbManagerProxy manager, Diagnosis2PensideTestMaster obj)
            {
                
                obj.DiagnosisLookup.Clear();
                
                obj.DiagnosisLookup.Add(DiagnosisAccessor.CreateNewT(manager, null));
                
                obj.DiagnosisLookup.AddRange(DiagnosisAccessor.SelectLookupList(manager
                    
                    )
                    .Where(c => ((c.intHACode & (int)HACode.All) != 0) || c.idfsDiagnosis == obj.idfsDiagnosis)
                        
                    .Where(c => (c.idfsUsingType == (long)DiagnosisUsingTypeEnum.StandardCase) || c.idfsDiagnosis == obj.idfsDiagnosis)
                        
                    .Where(c => (c.intRowStatus == 0) || (c.idfsDiagnosis == obj.idfsDiagnosis))
                    
                    .ToList());
                
                if (obj.idfsDiagnosis != 0)
                {
                    obj.Diagnosis = obj.DiagnosisLookup
                        .SingleOrDefault(c => c.idfsDiagnosis == obj.idfsDiagnosis);
                    
                }
              
                LookupManager.AddObject("DiagnosisLookup", obj, DiagnosisAccessor.GetType(), "_SelectListInternal");
                obj.bNeedLookupRemove = true;
              
            }
            

            internal void _LoadLookups(DbManagerProxy manager, Diagnosis2PensideTestMaster obj)
            {
                
                LoadLookup_Diagnosis(manager, obj);
                
            }
    
            [SprocName("spDiagnosis2PensideTestMaster_Delete")]
            protected abstract void _postDelete(DbManagerProxy manager
                );
            protected void _postDeletePredicate(DbManagerProxy manager
                )
            {
                
                _postDelete(manager);
                
            }
        
            public bool DeleteObject(DbManagerProxy manager, object ident)
            {
                IObject obj = SelectDetail(manager, ident);
                if (!obj.MarkToDelete()) return false;
                return Post(manager, obj);
            }
        
            [SprocName("spDummy_Post")]
            protected abstract void _post(DbManagerProxy manager, 
                [Direction.InputOutput()] Diagnosis2PensideTestMaster obj);
            protected void _postPredicate(DbManagerProxy manager, 
                [Direction.InputOutput()] Diagnosis2PensideTestMaster obj)
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
                        Diagnosis2PensideTestMaster bo = obj as Diagnosis2PensideTestMaster;
                        
                        if (!bo.IsNew && bo.IsMarkedToDelete) // delete
                        {
                            if (!CanDelete)
                                throw new PermissionException("Reference", "Delete");
                        }
                        else if (bo.IsNew && !bo.IsMarkedToDelete) // insert
                        {
                            if (!CanInsert)
                                throw new PermissionException("Reference", "Insert");
                        }
                        else if (!bo.IsMarkedToDelete && bo.HasChanges) // update
                        {
                            if (!CanUpdate)
                                throw new PermissionException("Reference", "Update");
                        }
                        
                        long mainObject = bo.idfsDiagnosis;
                        
                        if (!bo.IsNew && bo.IsMarkedToDelete) // delete
                        {
                        }
                        else if (bo.IsNew && !bo.IsMarkedToDelete) // insert
                        {
                            
                            if (new Func<Diagnosis2PensideTestMaster, bool>(c => c.HasChanges)(bo))
                              
                                manager.SetEventParams(false, new object[] { EventType.MatrixChanged, mainObject, "Diagnosis2PensideTestMasterDetail" });
                            
                        }
                        else if (!bo.IsMarkedToDelete) // update
                        {
                            
                            if (new Func<Diagnosis2PensideTestMaster, bool>(c => c.HasChanges)(bo))
                              
                                manager.SetEventParams(false, new object[] { EventType.MatrixChanged, mainObject, "Diagnosis2PensideTestMasterDetail" });
                            
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
                            
                            eidss.model.Enums.EIDSSAuditObject objectType = eidss.model.Enums.EIDSSAuditObject.daoReference;
                            eidss.model.Enums.AuditTable auditTable = eidss.model.Enums.AuditTable.trtBaseReference;
                            manager.AuditParams = new object[] { auditEventType, objectType, auditTable, mainObject };
                            
                            bTransactionStarted = true;
                            manager.BeginTransaction();
                        }
                        bSuccess = _PostNonTransaction(manager, obj as Diagnosis2PensideTestMaster, bChildObject);
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
            private bool _PostNonTransaction(DbManagerProxy manager, Diagnosis2PensideTestMaster obj, bool bChildObject) 
            { 
                bool bHasChanges = obj.HasChanges;
                if (!obj.IsNew && obj.IsMarkedToDelete) // delete
                {
            
                    if (obj.Diagnosis2PensideTest != null)
                    {
                        foreach (var i in obj.Diagnosis2PensideTest)
                        {
                            i.MarkToDelete();
                            if (!Diagnosis2PensideTestAccessor.Post(manager, i, true))
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
            
                    if (obj.IsNew)
                    {
                        if (obj.Diagnosis2PensideTest != null) // forced load potential lazy subobject for new object
                        {
                            foreach (var i in obj.Diagnosis2PensideTest)
                                if (!Diagnosis2PensideTestAccessor.Post(manager, i, true))
                                    return false;
                            obj.Diagnosis2PensideTest.Where(c => c.IsMarkedToDelete).ToList().ForEach(c => obj.Diagnosis2PensideTest.Remove(c));
                            obj.Diagnosis2PensideTest.AcceptChanges();
                        }
                    }
                    else
                    {
                        if (obj._Diagnosis2PensideTest != null) // do not load lazy subobject for existing object
                        {
                            foreach (var i in obj._Diagnosis2PensideTest)
                                if (!Diagnosis2PensideTestAccessor.Post(manager, i, true))
                                    return false;
                            obj._Diagnosis2PensideTest.Where(c => c.IsMarkedToDelete).ToList().ForEach(c => obj._Diagnosis2PensideTest.Remove(c));
                            obj._Diagnosis2PensideTest.AcceptChanges();
                        }
                    }
                                    
                    // posted extenters - begin
                    // posted extenters - end
            
                }
                //obj.AcceptChanges();
                
                return true;
            }
            
            public bool ValidateCanDelete(Diagnosis2PensideTestMaster obj)
            {
                using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
                {
                    return ValidateCanDelete(manager, obj);
                }
            }
            public bool ValidateCanDelete(DbManagerProxy manager, Diagnosis2PensideTestMaster obj)
            {
            
                return true;
            }
        
      
            protected ValidationModelException ChainsValidate(Diagnosis2PensideTestMaster obj)
            {
                
                return null;
            }
            protected bool ChainsValidate(Diagnosis2PensideTestMaster obj, bool bRethrowException)
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
                return Validate(manager, obj as Diagnosis2PensideTestMaster, bPostValidation, bChangeValidation, bDeepValidation, bRethrowException);
            }
            public bool Validate(DbManagerProxy manager, Diagnosis2PensideTestMaster obj, bool bPostValidation, bool bChangeValidation, bool bDeepValidation, bool bRethrowException = false)
            {
                if (!ChainsValidate(obj, bRethrowException))
                    return false;
                    
                
                try
                {
                    if (bPostValidation)
                    {
                
              new DuplicateListValidator("").Validate(obj, 
              o => o.Diagnosis2PensideTest.Where(c => !c.IsMarkedToDelete)
                .Aggregate(new List<Tuple<Diagnosis2PensideTest, int>>(), (list, test) => 
                { 
                    var item = list.Find(c => true
                      
                        && c.Item1.idfsPensideTestName == test.idfsPensideTestName
                        && c.Item1.idfsDiagnosis == test.idfsDiagnosis
                      );
                      if (item == null)
                      list.Add(new Tuple<Diagnosis2PensideTest, int>(test, 1));
                    else
                    {
                        list.Remove(item);
                        list.Add(new Tuple<Diagnosis2PensideTest, int>(test, item.Item2 + 1));
                    }
                    return list;
                })
                .Where(c => c.Item2 > 1).Select(c => c.Item1).FirstOrDefault(),
                    (o, i) => new Tuple<
                      string, object
                      
                      >(
                      "idfsPensideTestName", 
                        
                        i.PensideTestTypeLookup.First(c => c.Key.Equals(i.idfsPensideTestName)).ToStringProp
                          
                      )
                );
            
          
                  
                    }

                    if (bChangeValidation)
                    {
                
                        foreach(var item in obj.Diagnosis2PensideTest.Where(c => true))
                        {
                        
                (new RequiredValidator( "idfsPensideTestName", "idfsPensideTestName","",
                ValidationEventType.Error
              )).Validate(c => true, item, item.idfsPensideTestName);
            
                        }
                
                    }
                    
                    if (bDeepValidation)
                    {
                
                        if (obj.Diagnosis2PensideTest != null)
                            foreach (var i in obj.Diagnosis2PensideTest.Where(c => !c.IsMarkedToDelete && c.HasChanges))
                                Diagnosis2PensideTestAccessor.Validate(manager, i, bPostValidation, bChangeValidation, bDeepValidation, true);
                                        
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
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !CanUpdate; } }
            #endregion
            
            private void _SetupRequired(Diagnosis2PensideTestMaster obj)
            {
            
            }
    
    private void _SetupPersonalDataRestrictions(Diagnosis2PensideTestMaster obj)
    {
    
    
    }
  
            #region IObjectMeta
            public int? MaxSize(string name) { return Meta.Sizes.ContainsKey(name) ? (int?)Meta.Sizes[name] : null; }
            public bool RequiredByField(string name, IObject obj) { return Meta.RequiredByField.ContainsKey(name) ? Meta.RequiredByField[name](obj as Diagnosis2PensideTestMaster) : false; }
            public bool RequiredByProperty(string name, IObject obj) { return Meta.RequiredByProperty.ContainsKey(name) ? Meta.RequiredByProperty[name](obj as Diagnosis2PensideTestMaster) : false; }
            public List<SearchPanelMetaItem> SearchPanelMeta { get { return Meta.SearchPanelMeta; } }
            public List<GridMetaItem> GridMeta { get { return Meta.GridMeta; } }
            public List<ActionMetaItem> Actions { get { return Meta.Actions; } }
            public string DetailPanel { get { return "Diagnosis2PensideTestMasterDetail"; } }
            public string HelpIdWin { get { return "Diagnosis-Penside_tests_matrix"; } }
            public string HelpIdWeb { get { return ""; } }
            public string HelpIdHh { get { return ""; } }
            public string SqlSortOrder { get { return Meta.sqlSortOrder; } }
            #endregion
    
        }

        
            #region IObjectPermissions
        internal class Permissions : IObjectPermissions
        {
            private Diagnosis2PensideTestMaster m_obj;
            internal Permissions(Diagnosis2PensideTestMaster obj)
            {
                m_obj = obj;
            }
            
            public bool CanSelect { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Select"); } }
            public bool CanUpdate { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Update"); } }
            public bool CanDelete { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Delete"); } }
            public bool CanInsert { get { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo("Reference.Insert"); } }
            
            public bool CanExecute(string permission) { return ModelUserContext.Instance == null ? true : ModelUserContext.Instance.CanDo(permission.Contains(".") ? permission : permission + ".Execute"); }
            public bool IsReadOnlyForEdit { get { return !(CanUpdate || (CanInsert && m_obj.IsNew)); } }
        }
            #endregion
            
        #region Meta
        public static class Meta
        {
            public static string spSelect = "spDiagnosisMaster_SelectDetail";
            public static string spCount = "";
            public static string spPost = "spDummy_Post";
            public static string spInsert = "";
            public static string spUpdate = "";
            public static string spDelete = "spDiagnosis2PensideTestMaster_Delete";
            public static string spCanDelete = "";
            public static string sqlSortOrder = "";
            public static Dictionary<string, int> Sizes = new Dictionary<string, int>();
            public static Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>> RequiredByField = new Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>>();
            public static Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>> RequiredByProperty = new Dictionary<string, Func<Diagnosis2PensideTestMaster, bool>>();
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
                    "DeletePensideTest",
                    ActionTypes.Action,
                    true,
                    "",
                    "",
                    
                    (manager, c, pars) => Accessor.Instance(null).DeletePensideTest(manager, (Diagnosis2PensideTestMaster)c, pars),
                        
                    null,
                    
                    new ActionMetaItem.VisualItem(
                        /*from BvMessages*/"strDelete_Id",
                        "Delete_Remove",
                        /*from BvMessages*/"tooltipDelete_Id",
                        /*from BvMessages*/"",
                        "",
                        /*from BvMessages*/"",
                        ActionsAlignment.Right,
                        ActionsPanelType.Main,
                        ActionsAppType.All
                        ),
                      true,
                    null,
                    null,
                    null,
                    null,
                    ActionMetaItem.DefaultDeleteGroupItemVisiblePredicate,
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<Diagnosis2PensideTestMaster>().Post(manager, (Diagnosis2PensideTestMaster)c), c),
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
                    (manager, c, pars) => new ActResult(ObjectAccessor.PostInterface<Diagnosis2PensideTestMaster>().Post(manager, (Diagnosis2PensideTestMaster)c), c),
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
	