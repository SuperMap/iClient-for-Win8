using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// 	<para>${WP_utility_DoubleCollectionConverter_Title}</para>
    /// 	<para>${WP_utility_DoubleCollectionConverter_Description}</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]

    public class DoubleCollectionConverter : IValueConverter
    {
        /// <summary>${WP_utility_DoubleCollectionConverter_method_Convert_D}</summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (targetType != typeof(DoubleCollection))
            {
                throw new NotSupportedException();
            }
            DoubleCollection doubles = (DoubleCollection)value;
            DoubleCollection doubles2 = new DoubleCollection();
            foreach (double num in doubles)
            {
                doubles2.Add(num);
            }
            return doubles2;
        }
        /// <summary>${WP_utility_DoubleCollectionConverter_method_ConvertBack_D}</summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
