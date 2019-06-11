﻿using System;
using System.Collections.Generic;
using bv.common.Core;

namespace eidss.avr.db.Common
{
    public class AvrFieldValuePair
    {
        public AvrFieldValuePair(IAvrPivotGridField field, List<IAvrPivotGridField> fieldCopy, bool isRelated)
        {
            Utils.CheckNotNull(field, "field");
            if (fieldCopy == null)
            {
                throw new ArgumentNullException("fieldCopy");
            }

            Field = field;
            FieldCopy = fieldCopy;
            IsRelated = isRelated;
            LinkedFieldValues = new Dictionary<IAvrPivotGridField, object>();
        }

        public IAvrPivotGridField Field { get; private set; }
        public List<IAvrPivotGridField> FieldCopy { get; private set; }
        public bool IsRelated { get; private set; }
        public object Value { get; set; }
        public IDictionary<IAvrPivotGridField, object> LinkedFieldValues { get; set; }

        public override string ToString()
        {
            return string.Format("({0}): '{1}'", Field.Caption, Utils.Str(Value));
        }
    }
}