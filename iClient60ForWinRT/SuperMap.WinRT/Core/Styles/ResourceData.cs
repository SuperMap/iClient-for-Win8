using System;
using System.Windows;
using Windows.UI.Xaml;

namespace SuperMap.WinRT.Core
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
                    dictionary.MergedDictionaries.Add(LoadDictionary("ms-appx:///SuperMap.WinRT/Core/Styles/Templates.xaml"));
                    return dictionary;
                }
                return dictionary;
            }
        }

        private static ResourceDictionary LoadDictionary(string key)
        {
            return new ResourceDictionary { Source = new Uri(key) };
        }
    }
}
