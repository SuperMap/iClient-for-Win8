using System;
using System.Windows;

namespace SuperMap.WindowsPhone.Core
{
    internal class ResourceData
    {
        private static ResourceDictionary dictionary;

        public static ResourceDictionary Dictionary
        {
            get
            {
                if (dictionary == null)
                {
                    dictionary = new ResourceDictionary();
                    dictionary.MergedDictionaries.Add(LoadDictionary("/SuperMap.WindowsPhone;component/Core/Styles/Templates.xaml"));
                    return dictionary;
                }
                return dictionary;
            }
        }

        private static ResourceDictionary LoadDictionary(string key)
        {
            return new ResourceDictionary { Source = new Uri(key, UriKind.Relative) };
        }
    }
}
