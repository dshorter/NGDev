﻿using System;
using System.Xml.Serialization;

namespace eidss.model.Avr.Chart
{
    [Serializable]
    public class PieProperties : IParentSeries, IPieProperties
    {
        [XmlIgnore]
        public SeriesProperties ParentSeries { get; set; }

        public void CopyFrom(IPieProperties pie)
        {
            ShowArguments = pie.ShowArguments;
            Format = pie.Format;
        }

        public bool ShowArguments { get; set; }
        public int Format { get; set; }
    }
}
