﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eidss.web.common.Utils
{
    public class EidssWebHelper
    {
        public static string UnescapeHtml(string val)
        {
            if (val != null)
                val = val.Replace("&amp;", "&")
                         .Replace("&gt;", ">")
                         .Replace("&lt;", "<")
                         .Replace("&quot;", "\"")
                         .Replace("&apos;", "'");
            return val;
        }
    }
}
