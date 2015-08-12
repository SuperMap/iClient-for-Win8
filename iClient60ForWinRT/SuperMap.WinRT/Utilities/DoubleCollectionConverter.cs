using System;
using System.ComponentModel;
using System.Globalization;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Utilities
{
    /// <summary>
    /// 	<para>${utility_DoubleCollectionConverter_Title}</para>
    /// 	<para>${utility_DoubleCollectionConverter_Description}</para>
    /// </summary>
    public sealed class DoubleCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
