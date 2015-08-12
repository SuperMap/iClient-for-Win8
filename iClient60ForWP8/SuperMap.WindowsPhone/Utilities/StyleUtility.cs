using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Resources;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>
    /// 	<para>${WP_core_StyleUtility_Title}</para>
    /// 	<para>${WP_core_StyleUtility_Description}</para>
    /// </summary>
    public static class StyleUtility
    {
        /// <summary>${WP_core_StyleUtility_method_LoadObjectFromXamlFile_D}</summary>
        /// <returns>${WP_core_StyleUtility_method_LoadObjectFromXamlFile_return}</returns>
        /// <param name="fileUri">${WP_core_StyleUtility_method_LoadObjectFromXamlFile_param_fileUri}</param>
        public static object LoadObjectFromXamlFile(string fileUri)
        {
            string str = XamlFileToString(fileUri);
            if (!string.IsNullOrEmpty(str))
            {
                return XamlReader.Load(fileUri);
            }
            return null;
        }

        /// <summary>${WP_core_StyleUtility_method_XamlFileToString_D}</summary>
        /// <returns>${WP_core_StyleUtility_method_XamlFileToString_return}</returns>
        /// <param name="fileUri">${WP_core_StyleUtility_method_XamlFileToString_param_fileUri}</param>
        public static string XamlFileToString(string fileUri)
        {
            Uri file = new Uri(fileUri, UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(file);

            if ((streamInfo != null) && (streamInfo.Stream != null))
            {
                using (StreamReader reader = new StreamReader(streamInfo.Stream))
                {
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }
    }
}
