﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:bv="urn:schemas-bv:objectmodel"
	xmlns:util="urn:util">

    <xsl:import href="globals.xslt" />
    <xsl:import href="extenders.xslt" />
    <xsl:import href="validators.xslt" />

    <xsl:template name="other">
        <xsl:param name="tablename" />
        <xsl:param name="hacodable" />
        <xsl:param name="tablekeys" />
        protected CacheScope m_CS;
        protected Accessor _getAccessor() { return Accessor.Instance(m_CS); }
        private IObjectPermissions m_permissions = null;
        internal IObjectPermissions _permissions { get { return m_permissions; } set { m_permissions = value; } }
        internal string m_ObjectName = "<xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />";

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
        <xsl:for-each select="bv:relations/bv:relation[not(@self)][not(@recursive)]">
            <xsl:choose>
                <xsl:when test="@type='child'">
            <xsl:value-of select="@name" />.ForEach(c => { c.Parent = this; });
                </xsl:when>
                <xsl:otherwise>
            if (_<xsl:value-of select="@name" /> != null) { _<xsl:value-of select="@name" />.Parent = this; }
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
        }
        partial void Cloned();
        partial void ClonedWithSetup();
        private bool bIsClone;
        public override object Clone()
        {
            var ret = base.Clone() as <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />;
            ret.bIsClone = true;
            ret.Cloned();
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret._setParent();
            if (this.IsDirty &amp;&amp; !ret.IsDirty)
                ret.SetChange();
            else if (!this.IsDirty &amp;&amp; ret.IsDirty)
                ret.RejectChanges();
            return ret;
        }
        public IObject CloneWithSetup(DbManagerProxy manager, bool bRestricted = false)
        {
            var ret = base.Clone() as <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />;
            ret.m_Parent = this.Parent;
            ret.m_IsMarkedToDelete = this.m_IsMarkedToDelete;
            ret.m_IsForcedToDelete = this.m_IsForcedToDelete;
            ret.m_IsNew = this.IsNew;
            ret.m_ObjectName = this.m_ObjectName;
        <xsl:for-each select="bv:relations/bv:relation[not(@self)][not(@recursive)][not(@skipclone)]">
            <xsl:choose>
                <xsl:when test="@type='child'">
            if (_<xsl:value-of select="@name" /> != null &amp;&amp; _<xsl:value-of select="@name" />.Count > 0)
            {
              ret.<xsl:value-of select="@name" />.Clear();
              _<xsl:value-of select="@name" />.ForEach(c => ret.<xsl:value-of select="@name" />.Add(c.CloneWithSetup(manager, bRestricted)));
            }
                </xsl:when>
                <xsl:otherwise>
            if (_<xsl:value-of select="@name" /> != null)
              ret.<xsl:value-of select="@name" /> = _<xsl:value-of select="@name" />.CloneWithSetup(manager, bRestricted) as <xsl:value-of select="@table" /><xsl:value-of select="$class_suffix" />;
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
            Accessor.Instance(null)._SetupLoad(manager, ret, true);
            ret.ClonedWithSetup();
            ret.DeepAcceptChanges();
            ret._setParent();
            ret._permissions = _permissions;
            return ret;
        }
        public <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" /> CloneWithSetup()
        {
            using (DbManagerProxy manager = DbManagerFactory.Factory.Create(ModelUserContext.Instance))
            {
                return CloneWithSetup(manager) as <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />;
            }
        }
        #endregion

        #region IObject implementation
        public object Key { get { return <xsl:value-of select="msxsl:node-set($tablekeys)/keys/key[1]/@name" />; } }
        public string KeyName { get { return "<xsl:value-of select="msxsl:node-set($tablekeys)/keys/key[1]/@name" />"; } }
        public object KeyLookup { get { return <xsl:choose>
          <xsl:when test="msxsl:node-set($tablekeys)/keys/keyLookup[1]/@name">
            <xsl:value-of select="msxsl:node-set($tablekeys)/keys/keyLookup[1]/@name" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="msxsl:node-set($tablekeys)/keys/key[1]/@name" />
          </xsl:otherwise>
        </xsl:choose>; } }
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
        <xsl:for-each select="bv:relations/bv:relation[not(@self)][not(@recursive)]">
            <xsl:choose>
                <xsl:when test="@type='child'">
                    || <xsl:value-of select="@name" />.IsDirty
                    || <xsl:value-of select="@name" />.Count(c => c.IsMarkedToDelete || c.HasChanges) > 0
                </xsl:when>
                <xsl:otherwise>
                    || (_<xsl:value-of select="@name" /> != null &amp;&amp; _<xsl:value-of select="@name" />.HasChanges)
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
                ;
            }
        }
        public new void RejectChanges()
        {
        <xsl:for-each select="bv:lookups/bv:lookup[not(@hintonly)][not(@existinglookup)]">
            var _prev_<xsl:value-of select="@source" />_<xsl:value-of select="@name" /> = <xsl:value-of select="@source" />;</xsl:for-each>
            base.RejectChanges();
        <xsl:for-each select="bv:lookups/bv:lookup[not(@hintonly)][not(@existinglookup)]">
            if (_prev_<xsl:value-of select="@source" />_<xsl:value-of select="@name" /> != <xsl:value-of select="@source" />)
            {
                _<xsl:value-of select="@name" /> = _<xsl:value-of select="@name" />Lookup.FirstOrDefault(c => c.<xsl:value-of select="@target" /> == <xsl:value-of select="@source" />);
            }</xsl:for-each>
        }
        public void DeepRejectChanges()
        {
            RejectChanges();
        <xsl:for-each select="bv:relations/bv:relation[not(@self)][not(@recursive)]">
            <xsl:choose>
                <xsl:when test="@type='child'">
            <xsl:value-of select="@name" />.DeepRejectChanges();
                </xsl:when>
                <xsl:otherwise>
            if (<xsl:value-of select="@name" /> != null) <xsl:value-of select="@name" />.DeepRejectChanges();
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
        }
        public void DeepAcceptChanges()
        { 
            AcceptChanges();
        <xsl:for-each select="bv:relations/bv:relation[not(@self)][not(@recursive)]">
            <xsl:choose>
                <xsl:when test="@type='child'">
            <xsl:value-of select="@name" />.DeepAcceptChanges();
                </xsl:when>
                <xsl:otherwise>
            if (_<xsl:value-of select="@name" /> != null) _<xsl:value-of select="@name" />.DeepAcceptChanges();
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
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
        <xsl:for-each select="bv:relations/bv:relation[not(@self)][not(@recursive)]">
            <xsl:choose>
                <xsl:when test="@type='child'">
            
            <xsl:value-of select="@name" />.ForEach(c => c.SetChange());
                </xsl:when>
                <xsl:otherwise>
            if (_<xsl:value-of select="@name" /> != null) _<xsl:value-of select="@name" />.SetChange();
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
        }
        public bool MarkToDelete() { return _Delete(false); }
        public string ObjectName { get { return m_ObjectName; } }
        public string ObjectIdent { get { return ObjectName + "<xsl:value-of select="$ident_delimeter" />" + Key.ToString() + "<xsl:value-of select="$ident_delimeter" />"; } }
        <xsl:choose>
          <xsl:when test="bv:keys/bv:key2/@name">
        public string ObjectIdent2 { get { return ObjectName + "<xsl:value-of select="$ident_delimeter" />" + <xsl:value-of select="bv:keys/bv:key2/@name"/>.ToString() + "<xsl:value-of select="$ident_delimeter" />"; } }
          </xsl:when>
          <xsl:otherwise>
        public string ObjectIdent2 { get { return ObjectIdent; } }
          </xsl:otherwise>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="bv:keys/bv:key3/@name">
        public string ObjectIdent3 { get { return ObjectName + "<xsl:value-of select="$ident_delimeter" />" + <xsl:value-of select="bv:keys/bv:key3/@name"/>.ToString() + "<xsl:value-of select="$ident_delimeter" />"; } }
          </xsl:when>
          <xsl:otherwise>
        public string ObjectIdent3 { get { return ObjectIdent; } }
          </xsl:otherwise>
        </xsl:choose>
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
      
        public Dictionary&lt;string, string&gt; GetFieldTags(string name)
        {
          <xsl:choose>
            <xsl:when test="bv:fieldtags/bv:field">
              switch(name)
              {
              <xsl:for-each select="bv:fieldtags/bv:field">
                case "<xsl:value-of select="@name" />":
                  return new Dictionary&lt;string, string&gt; {
                <xsl:for-each select="bv:tag">
                    { "<xsl:value-of select="@name" />", "<xsl:value-of select="@value" />" },
                </xsl:for-each>
                    } ;
              </xsl:for-each>
                default:
                  return null;
              }
            </xsl:when>
            <xsl:otherwise>return null;</xsl:otherwise>
          </xsl:choose>
        }
      #endregion

        private bool IsRIRPropChanged(string fld, <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" /> c)
        {
            return IsReadOnly(fld) != c.IsReadOnly(fld) || IsInvisible(fld) != c.IsInvisible(fld) || IsRequired(fld) != c._isRequired(m_isRequired, fld);
        }
        private bool IsLookupContentChanged(DbManagerProxy manager, string fld, <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" /> c)
        {
            var thisLookup = GetValue(fld + "Lookup") as IList;
            var thatLookup = c.GetValue(fld + "Lookup") as IList;
            bool bChangeLookupContent = thisLookup.Count != thatLookup.Count;
            if (!bChangeLookupContent)
            {
                for (int i = 0; i &lt; thisLookup.Count; i++)
                {
                    if (((thisLookup[i] as IObject).Key as IComparable).CompareTo((thatLookup[i] as IObject).Key) != 0 &amp;&amp;
                        (thisLookup[i] as IObject).ToStringProp != null &amp;&amp; ((thisLookup[i] as IObject).ToStringProp as IComparable).CompareTo((thatLookup[i] as IObject).ToStringProp) != 0)
                    {
                        bChangeLookupContent = true;
                        break;
                    }
                }
            }
            return bChangeLookupContent;
        }
        

      <xsl:if test="@tostring">
        public override string ToString()
        {
            return new Func&lt;<xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />, string&gt;(<xsl:value-of select="@tostring" />)(this);
        }
        </xsl:if>

        public <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />()
        {
            <xsl:if test="bv:properties/@permissionObject or bv:properties/@permissionAction">
            m_permissions = new Permissions(this);
            </xsl:if>
        }

        partial void Changed(string fieldName);
        partial void Created(DbManagerProxy manager);
        partial void Loaded(DbManagerProxy manager);
        partial void Deleted();
        partial void ParsedFormCollection(NameValueCollection form);
        partial void ParsingFormCollection(NameValueCollection form);

        <xsl:if test="$hacodable='true'">
        private int? m_HACode;
        public int? _HACode { get { return m_HACode; } set { m_HACode = value; } }
        </xsl:if>

        private bool m_IsForcedToDelete;
        [LocalizedDisplayName("IsForcedToDelete")]
        public bool IsForcedToDelete { get { return m_IsForcedToDelete; } }

        private bool m_IsMarkedToDelete;
        [LocalizedDisplayName("IsMarkedToDelete")]
        public bool IsMarkedToDelete { get { return m_IsMarkedToDelete; } }

        public void _SetupMainHandler()
        {
            PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(<xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />_PropertyChanged);
        }
        public void _RevokeMainHandler()
        {
            PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(<xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />_PropertyChanged);
        }
        private void <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" />).Changed(e.PropertyName);
            <xsl:for-each select="bv:fields/bv:calculated">
                <xsl:variable name="fieldname" select="@name" />
                <xsl:variable name="keys" select="util:ExpandXml('depends,depends1', string(@depends), string(@depends))" />
                <xsl:for-each select="$keys/keys/key">
                  <xsl:if test="@depends != ''">
            if (e.PropertyName == _str_<xsl:value-of select="@depends" />)
                OnPropertyChanged(_str_<xsl:value-of select="$fieldname" />);
                  </xsl:if>
                </xsl:for-each>
            </xsl:for-each>
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
            <xsl:if test="bv:validators/bv:delete/*">
            <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" /> obj = this;
            try
            {
            <xsl:for-each select="bv:validators/bv:delete/*">
                <xsl:call-template name="generate-validator">
                    <xsl:with-param name="tablename" select="$tablename" />
                </xsl:call-template>
            </xsl:for-each>
            }
            catch(ValidationModelException ex)
            {
                if (bReport &amp;&amp; !OnValidation(ex))
                {
                    OnValidationEnd(ex);
                }
                
                return false;
            }
            </xsl:if>
            <xsl:choose>
              <xsl:when test="bv:storage/bv:candelete">
            return Accessor.Instance(null).ValidateCanDelete(this);
              </xsl:when>
              <xsl:otherwise>
            return true;                
              </xsl:otherwise>
            </xsl:choose>
        }
        private void _DeletingExtenders()
        {
            <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" /> obj = this;
            <xsl:for-each select="bv:extenders/bv:deleting/*">
              <xsl:call-template name="generate-extender">
                <xsl:with-param name="tablename" select="$tablename" />
                <xsl:with-param name="hacodable" select="$hacodable" />
              </xsl:call-template>
            </xsl:for-each>
        }
        private void _DeletedExtenders()
        {
            <xsl:value-of select="$tablename" /><xsl:value-of select="$class_suffix" /> obj = this;
            <xsl:for-each select="bv:extenders/bv:deleted/*">
              <xsl:call-template name="generate-extender">
                <xsl:with-param name="tablename" select="$tablename" />
                <xsl:with-param name="hacodable" select="$hacodable" />
              </xsl:call-template>
            </xsl:for-each>
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
            get { return FormSize.<xsl:choose><xsl:when test="@formsize"><xsl:value-of select="@formsize"/></xsl:when><xsl:otherwise>Undefined</xsl:otherwise></xsl:choose>; }
        }
    </xsl:template>
    
</xsl:stylesheet>
