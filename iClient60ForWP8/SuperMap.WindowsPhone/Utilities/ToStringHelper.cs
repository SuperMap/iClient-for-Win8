using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace SuperMap.Web.Utilities
{
    public static class ToStringHelper
    {
        public static string ToStringEx(this object obj)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", obj);
        }
    }
}
