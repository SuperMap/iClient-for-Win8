using System;
using System.ComponentModel;
using System.Globalization;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// 	<para>${WP_utility_DoubleArrayConverter_Title}</para>
    /// 	<para>${WP_utility_DoubleArrayConverter_Description}</para>
    /// </summary>
    public sealed class DoubleArrayConverter : TypeConverter
    {
        /// <summary>${WP_utility_DoubleArrayConverter_method_CanConvertFrom_D}</summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        /// <summary>${WP_utility_DoubleArrayConverter_method_ConvertFrom_D}</summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                string[] strArray = str.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length == 0)
                {
                    return null;
                }
                double[] doubleArray = new double[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    doubleArray[i] = double.Parse(strArray[i], CultureInfo.InvariantCulture);
                }
                return doubleArray;
            }
            throw new NotSupportedException(SuperMap.WindowsPhone.Resources.ExceptionStrings.InvalidToDoubleArray);
        }
    }
}
