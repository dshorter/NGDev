using System;

namespace eidss.avr.db.AvrEventArgs.AvrEventArgs
{
    public delegate void PivotFieldVisibleChangedEventHandler(LayoutFieldVisibleChanged e);

    public class LayoutFieldVisibleChanged : EventArgs
    {
        private readonly bool m_Visible;

        public LayoutFieldVisibleChanged(bool visible)
        {
            m_Visible = visible;
        }

        public bool Visible
        {
            get { return m_Visible; }
        }
    }
}