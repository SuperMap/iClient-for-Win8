using System;
using System.ComponentModel;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace SuperMap.WinRT.Utilities
{
    /// <summary>
    /// 	<para>${utility_MultiplicationConverter_Title}</para>
    /// 	<para>${utility_MultiplicationConverter_Description}</para>
    /// </summary>
    public sealed class MultiplicationConverter : IValueConverter
    {
        /// <summary>
        /// ${utility_MultiplicationConverter_method_Convert_D}
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(double))
            {
                double num = (double)value;
                double num2 = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
                return (num * num2);
            }
            if (targetType == typeof(float))
            {
                float num3 = (float)value;
                float num4 = System.Convert.ToSingle(parameter, CultureInfo.InvariantCulture);
                return (num3 * num4);
            }
            if (targetType != typeof(int))
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, SuperMap.WinRT.Resources.ExceptionStrings.InvalidMultiplicationConverter, targetType));
            }
            double num5 = (int)value;
            double num6 = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return (int)Math.Round((double)(num5 * num6));
        }

        /// <summary>
        /// ${utility_MultiplicationConverter_method_ConvertBack_D}
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
