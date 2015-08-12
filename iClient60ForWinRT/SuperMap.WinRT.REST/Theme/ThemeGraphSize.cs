using System.Collections.Generic;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeGraphSize_Title}</para>
    /// 	<para>${REST_ThemeGraphSize_Description}</para>
    /// </summary>
    public class ThemeGraphSize
    {
        /// <summary>${REST_ThemeGraphSize_constructor_D}</summary>
        public ThemeGraphSize()
        { }
        /// <summary>${REST_ThemeGraphSize_attribute_maxGraphSize_D}</summary>  
        public double MaxGraphSize
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphSize_attribute_minGraphSize_D}</summary>
        public double MinGraphSize
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraphSize graphSize)
        {
            string json = "";

            List<string> list = new List<string>();
            list.Add(string.Format("\"maxGraphSize\":{0}", graphSize.MaxGraphSize.ToString()));
            list.Add(string.Format("\"minGraphSize\":{0}", graphSize.MinGraphSize.ToString()));
            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeGraphSize FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeGraphSize graphSize = new ThemeGraphSize();
            graphSize.MaxGraphSize = json["maxGraphSize"].GetNumberEx();
            graphSize.MinGraphSize = json["minGraphSize"].GetNumberEx();
            return graphSize;
        }
    }
}
