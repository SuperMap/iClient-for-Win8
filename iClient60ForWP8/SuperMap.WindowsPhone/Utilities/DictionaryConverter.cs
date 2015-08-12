using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// 	<para>${WP_utility_DictionaryConverter_Title}</para>
    /// 	<para>${WP_utility_DictionaryConverter_Description}</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class DictionaryConverter : IValueConverter
    {
        /// <summary>${WP_utility_DictionaryConverter_method_Convert_D}</summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Dictionary<string, object> dictionary = value as Dictionary<string, object>;
            IDictionary<string, object> dictionary = value as IDictionary<string, object>;
            if ((dictionary != null) && dictionary.ContainsKey(parameter as string))
            {
                return dictionary[parameter as string];
            }
            return null;
        }

        /// <summary>${WP_utility_DictionaryConverter_method_ConvertBack_D}</summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
