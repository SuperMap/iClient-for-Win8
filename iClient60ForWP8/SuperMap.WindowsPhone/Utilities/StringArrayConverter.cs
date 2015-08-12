using System;
using System.ComponentModel;
using System.Globalization;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// 	<para>${WP_utility_StringArrayConverter_Title}</para>
    /// 	<para>${WP_utility_StringArrayConverter_Description}</para>
    /// </summary>
    public sealed class StringArrayConverter : TypeConverter
    {
        /// <summary>${WP_utility_StringArrayConverter_method_CanConvertFrom_D}</summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        /// <summary>${WP_utility_StringArrayConverter_method_ConvertFrom_D}</summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                string[] values = str.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 0)
                    return null;
                return values;
            }
            //throw new NotSupportedException("无法转换为String Array");
            throw new NotSupportedException(SuperMap.WindowsPhone.Resources.ExceptionStrings.InvalidStringArray);
        }
    }
}
