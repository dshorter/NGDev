﻿using System;

namespace bv.winclient.Core.TranslationTool
{
    [Flags]
    public enum DesignElement
    {
        None = 0,
        Caption = 1,
        Moving = 2,
        Sizing = 4,
        Visibility = 8,
        All = Caption | Moving | Sizing | Visibility
    }
}
