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
using System.ComponentModel;
using System.Globalization;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// ${WP_utility_IntArrayConverter_Title}<br/>
    /// ${WP_utility_IntArrayConverter_Description}
    /// </summary>
    public class IntArrayConverter:TypeConverter
    {
        /// <summary>${WP_utility_IntArrayConverter_method_CanConvertFrom_D}</summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        /// <summary>${WP_utility_IntArrayConverter_method_ConvertFrom_D}</summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                string[] strArray = str.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return strArray;
            }
            throw new NotSupportedException(SuperMap.WindowsPhone.Resources.ExceptionStrings.InvalidToDoubleArray);
        }
    }
}
