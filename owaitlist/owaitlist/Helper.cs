using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace owaitlist
{
    public static class Helper
    {
        public static String ConvertTimeSpan(TimeSpan span)
        {
            string text = "";
            if (span.Hours > 0)
                text += span.Hours + " hours ";
            if (span.Minutes > 0)
                text += span.Minutes + " minutes ";
            if (span.TotalMinutes < 1)
                text = "(no wait)";
            return text;
        }
    }
}