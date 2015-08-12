using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace REST_SampleCode
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class DictionaryConverter : IValueConverter
    {
     
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IDictionary<string, object> dictionary = value as IDictionary<string, object>;
            if ((dictionary != null) && dictionary.ContainsKey(parameter as string))
            {
                return dictionary[parameter as string];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
