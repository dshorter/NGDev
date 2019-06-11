﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using bv.common.Core;
using bv.common.db;
using bv.common.db.Core;
using bv.model.Model.Core;
using DevExpress.XtraPivotGrid;
using EIDSS;
using eidss.avr.db.Common;
using eidss.avr.db.DBService.DataSource;
using EIDSS.Enums;
using eidss.model.Avr.Pivot;
using eidss.model.AVR.SourceData;
using PivotGridField = DevExpress.Web.ASPxPivotGrid.PivotGridField;

namespace eidss.avr.mweb.Utils
{
    public class WebPivotGridFieldClone
    {
        public WebPivotGridFieldClone()
        {
            Caption = "";
            IsEmpty = true;
        }

        public WebPivotGridFieldClone(WebPivotGridField webField, LayoutDetailDataSet ds)
        {
            Caption = webField.Caption;
            Init(webField, ds);
        }

        private void Init(WebPivotGridField webField, LayoutDetailDataSet ds)
        {
            if (webField == null || ds == null)
            {
                IsEmpty = true;
                return;
            }
            BasicCountFunctionId = webField.BasicCountFunctionId;
            AggregateFunctionId = webField.AggregateFunctionId;
            Precision = webField.Precision;
            UnitLayoutSearchFieldId = webField.UnitLayoutSearchFieldId;
            DateLayoutSearchFieldId = webField.DateLayoutSearchFieldId;
            if (webField.PrivateGroupInterval.HasValue)
            {
                PrivateGroupInterval = (long) GroupIntervalHelper.GetDBGroupInterval(webField.PrivateGroupInterval.Value);
            }
            else
            {
                PrivateGroupInterval = null;
            }
            DiapasonStartDate = webField.DiapasonStartDate;
            DiapasonEndDate = webField.DiapasonEndDate;
            AddMissedValues = webField.AddMissedValues;
            LayoutDetailDataSet.LayoutSearchFieldRow row = GetLayoutSearchFieldRowByField(ds, webField);
            CaptionEn = string.IsNullOrEmpty(row.strNewFieldENCaption) ? row.strOriginalFieldENCaption : row.strNewFieldENCaption;
            OriginalCaptionEn = row.strOriginalFieldENCaption;
            Caption = string.IsNullOrEmpty(row.strNewFieldCaption) ? row.strOriginalFieldCaption : row.strNewFieldCaption;
            OriginalCaption = row.strOriginalFieldCaption;
            IsEmpty = false;
            IsDataArea = webField.Area == PivotArea.DataArea;
        }

        private LayoutDetailDataSet.LayoutSearchFieldRow GetLayoutSearchFieldRowByField(LayoutDetailDataSet ds, WebPivotGridField webField)
        {
            return AvrPivotGridHelper.GetLayoutSearchFieldRowByField(webField, ds);
        }

        public void CancelChanges(WebPivotGridField webField, LayoutDetailDataSet ds)
        {
            Init(webField, ds);
        }

        public void SaveChanges(WebPivotGridField webField, LayoutDetailDataSet ds)
        {
            if (!Equals(BasicCountFunctionId, webField.BasicCountFunctionId))
            {
                webField.BasicCountFunctionId = BasicCountFunctionId;
                webField.HasChanges = true;
            }
            if (!Equals(AggregateFunctionId, webField.AggregateFunctionId))
            {
                webField.AggregateFunctionId = AggregateFunctionId;
                webField.HasChanges = true;
            }
            if (!Equals(Precision, webField.Precision))
            {
                webField.Precision = Precision;
                webField.HasChanges = true;
            }
            if (!Equals(UnitLayoutSearchFieldId, webField.UnitLayoutSearchFieldId))
            {
                webField.UnitLayoutSearchFieldId = UnitLayoutSearchFieldId;
                if (UnitLayoutSearchFieldId != -1)
                {
                    var unitRow = ds.LayoutSearchField.Rows.Find(UnitLayoutSearchFieldId) as LayoutDetailDataSet.LayoutSearchFieldRow;

                    webField.UnitSearchFieldAlias = unitRow == null
                        ? string.Empty
                        : unitRow.strSearchFieldAlias;
                }
                webField.HasChanges = true;
            }
            if (!Equals(DateLayoutSearchFieldId, webField.DateLayoutSearchFieldId))
            {
                webField.DateLayoutSearchFieldId = DateLayoutSearchFieldId;
                webField.HasChanges = true;
            }
            PivotGroupInterval? interval = null;
            if (PrivateGroupInterval.HasValue)
            {
                interval = GroupIntervalHelper.GetGroupInterval(PrivateGroupInterval.Value);
            }
            if (!Equals(interval, webField.PrivateGroupInterval))
            {
                webField.PrivateGroupInterval = interval;
                webField.HasChanges = true;
            }

            if (!Equals(DiapasonStartDate, webField.DiapasonStartDate))
            {
                webField.DiapasonStartDate = DiapasonStartDate;
                webField.HasChanges = true;
            }
            if (!Equals(DiapasonEndDate, webField.DiapasonEndDate))
            {
                webField.DiapasonEndDate = DiapasonEndDate;
                webField.HasChanges = true;
            }
            if (!Equals(AddMissedValues, webField.AddMissedValues))
            {
                webField.AddMissedValues = AddMissedValues;
                webField.UpdateImageIndex();
                webField.HasChanges = true;
            }
            LayoutDetailDataSet.LayoutSearchFieldRow row = GetLayoutSearchFieldRowByField(ds, webField);
            webField.Caption = (ModelUserContext.CurrentLanguage == Localizer.lngEn) ? CaptionEn : Caption;
            if (
                !Equals(string.IsNullOrEmpty(row.strNewFieldENCaption) ? row.strOriginalFieldENCaption : row.strNewFieldENCaption, CaptionEn))
            {
                row.strNewFieldENCaption = CaptionEn;
                webField.HasChanges = true;
            }
            if (ModelUserContext.CurrentLanguage != Localizer.lngEn)
            {
                if (!Equals(string.IsNullOrEmpty(row.strNewFieldCaption) ? row.strOriginalFieldCaption : row.strNewFieldCaption, Caption))
                {
                    row.strNewFieldCaption = Caption;
                    webField.HasChanges = true;
                }
            }
        }

        public string CaptionEn { get; set; }
        public string OriginalCaptionEn { get; set; }
        public string Caption { get; set; }
        public string OriginalCaption { get; set; }

        public long AggregateFunctionId { get; set; }
        public long BasicCountFunctionId { get; set; }
        public int? Precision { get; set; }
        public long? UnitLayoutSearchFieldId { get; set; }
        public long? DateLayoutSearchFieldId { get; set; }
        public long? PrivateGroupInterval { get; set; }
        public DateTime? DiapasonStartDate { get; set; }
        public DateTime? DiapasonEndDate { get; set; }
        public bool AddMissedValues { get; set; }
        public bool IsEmpty { get; private set; }
        public bool IsDataArea { get; private set; }
    }

    public class WebPivotGridField : PivotGridField, IAvrPivotGridField
    {
        private bool m_IsHiddenFilterField;
        private readonly Dictionary<CustomSummaryType, int> m_PrecisionDictionary = new Dictionary<CustomSummaryType, int>();

        private PivotGroupInterval m_DefaultGroupInterval;
        private PivotGroupInterval? m_PrivateGroupInterval;
        private List<IAvrPivotGridField> m_RelatedFields;
        private List<IAvrPivotGridField> m_LinkedFields;
        private List<AvrRelatedFieldAttributes> m_RelatedFieldAttributes;
        private long m_FieldId = -1;
        private string m_OriginalFieldName;


        public WebPivotGridField()
        {
        }

        public WebPivotGridField(string fieldName, PivotArea area)
            : base(fieldName, area)
        {
        }

        public WebPivotGridField(PivotGridField pivotGridField)
            : base(pivotGridField.FieldName, pivotGridField.Area)
        {
        }

        #region Web field editing interface

        public bool HasChanges { get; set; }
        public WebFieldAction Action { get; set; }
        private WebPivotGridFieldClone m_ClonedField;

        public WebPivotGridFieldClone GetClonedField(LayoutDetailDataSet ds, bool createNew)
        {
            if (m_ClonedField == null || createNew)
            {
                m_ClonedField = new WebPivotGridFieldClone(this, ds);
            }
            return m_ClonedField;
        }

        /// <summary>
        ///     flushes changes made in cloned field and set flag HasChanges
        /// </summary>
        public void FlashChanges(LayoutDetailDataSet ds)
        {
            if (m_ClonedField != null)
            {
                m_ClonedField.SaveChanges(this, ds);
            }
        }

        /// <summary>
        ///     returns cloned field to original state before editing
        /// </summary>
        public void CancelChanges(LayoutDetailDataSet ds)
        {
            if (m_ClonedField != null)
            {
                m_ClonedField.CancelChanges(this, ds);
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

      public long FieldId
        {
            get
            {
                if (m_FieldId <= 0)
                {
                    m_FieldId= AvrPivotGridFieldHelper.GetIdFromFieldName(FieldName);
                }
                return m_FieldId; 
            }
        }

        public string OriginalFieldName
        {
            get
            {
                if (bv.common.Core.Utils.IsEmpty(m_OriginalFieldName))
                {
                    m_OriginalFieldName = AvrPivotGridFieldHelper.GetOriginalNameFromFieldName(FieldName);
                }
                return m_OriginalFieldName;
            }
        }

        public new  string FieldName
        {
            get { return base.FieldName; }
            set
            {
                base.FieldName = value;
                m_FieldId = -1;
                m_OriginalFieldName = null;
            }
        }

        /// <summary>
        ///     Gets or sets the fields's Name
        ///     DON'T USE IT IN YOUR CODE except single setting when new field creating. Use FieldName instead.
        /// </summary>
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        //public override string Name
        //{
        //    get { return base.Name; }
        //    set { base.Name = value; }
        //}
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Type SearchFieldDataType
        {
            get { return m_SearchFieldDataType; }
            set
            {
                m_SearchFieldDataType = value;
                this.UpdateSearchFieldDataType();
                UpdateImageIndex();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDateTimeField
        {
            get { return SearchFieldDataType.IsDate(); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsNumeric
        {
            get { return SearchFieldDataType.IsNumeric(); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsHiddenFilterField
        {
            get { return m_IsHiddenFilterField; }
            set
            {
                m_IsHiddenFilterField = value;
                this.OnSetIsHiddenFilterField(value);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public CustomSummaryType CustomSummaryType { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public PivotGroupInterval? PrivateGroupInterval
        {
            get { return m_PrivateGroupInterval; }
            set
            {
                m_PrivateGroupInterval = value;
                PrivateGroupIntervalName = LookupCache.GetLookupValue(m_PrivateGroupInterval, BaseReferenceType.rftGroupInterval,
                    "name");
                this.UpdateGroupInterval();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string PrivateGroupIntervalName { get; private set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public PivotGroupInterval DefaultGroupInterval
        {
            get { return m_DefaultGroupInterval; }
            set
            {
                m_DefaultGroupInterval = value;
                this.UpdateGroupInterval();
            }
        }

        public int Ordinal { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<CustomSummaryType, int> PrecisionDictionary
        {
            get { return m_PrecisionDictionary; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int? Precision { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool AllowMissedReferenceValues { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string LookupTableName { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long? ReferenceTypeId { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long? GisReferenceTypeId { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string LookupAttributeName { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int? HaCode { get; set; }

        private long m_AggregateFunctionId;
        private long m_BasicCountFunctionId;
        private Type m_SearchFieldDataType;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long AggregateFunctionId
        {
            get { return m_AggregateFunctionId; }
            set
            {
                m_AggregateFunctionId = value;
                AggregateFunctionName =
                    LookupCache.GetLookupValue(m_AggregateFunctionId, LookupTables.AggregateFunction, "AggregateFunctionName");
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string AggregateFunctionName { get; private set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long BasicCountFunctionId
        {
            get { return m_BasicCountFunctionId; }
            set
            {
                m_BasicCountFunctionId = value;
                BasicCountFunctionName =
                    LookupCache.GetLookupValue(m_AggregateFunctionId, LookupTables.AggregateFunction, "AggregateFunctionName");
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string BasicCountFunctionName { get; private set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string UnitSearchFieldAlias { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long? UnitLayoutSearchFieldId { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string DateSearchFieldAlias { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long? DateLayoutSearchFieldId { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool AddMissedValues { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime? DiapasonStartDate { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime? DiapasonEndDate { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<IAvrPivotGridField> AllPivotFields { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, List<IAvrPivotGridField>> FieldNamesDictionary { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<IAvrPivotGridField> RelatedFields
        {
            get { return m_RelatedFields ?? (m_RelatedFields = this.GetRelatedFields(true)); }
            internal set { m_RelatedFields = value; }
        }

        public List<IAvrPivotGridField> LinkedFields
        {
            get { return m_LinkedFields ?? (m_LinkedFields = this.GetRelatedFields(false)); }
        }

        public List<AvrRelatedFieldAttributes> RelatedFieldAttributes
        {
            get { return m_RelatedFieldAttributes ?? (m_RelatedFieldAttributes = this.GetRelatedFieldAttributes()); }
        }

        public string FieldImage { get; set; }

        public void UpdateImageIndex()
        {
            switch (SearchFieldDataType.ToString())
            {
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.Float":
                    FieldImage = AddMissedValues ? "numeric_bordered.png" : "numeric.png";
                    break;

                case "System.DateTime":
                    FieldImage = AddMissedValues ? "date_bordered.png" : "date.png";
                    break;
                case "System.Boolean":
                    FieldImage = AddMissedValues ? "boolean_bordered.png" : "boolean.png";
                    break;
                default:
                    FieldImage = AddMissedValues ? "string_bordered.png" : "string.png";
                    break;
            }
        }

        public override bool CanShowInCustomizationForm
        {
            get
            {
                if (IsHiddenFilterField)
                {
                    return false;
                }
                return base.CanShowInCustomizationForm;
            }
        }

        public override bool CanShowInPrefilter
        {
            get
            {
                return true.Equals(Tag);
                //return IsHiddenFilterField;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public SearchFieldType SearchFieldType { get; set; }
    }
}