using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeParameters_Title}</para>
    /// 	<para>${REST_ThemeParameters_Description}</para>
    /// </summary>
    public class ThemeParameters
    {
        /// <summary>${REST_ThemeParameters_constructor_D}</summary>
        public ThemeParameters()
        {
            JoinItems = new List<JoinItem>();
        }

        /// <summary>${REST_ThemeParameters_attribute_Theme_D}</summary>
        public List<Theme> Themes
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeParameters_attribute_DataSourceName_D}</summary>
        public string DataSourceName
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeParameters_attribute_DatasetName_D}</summary>
        public string DatasetName
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeParameters_attribute_JoinItems_D}</summary>
        public List<JoinItem> JoinItems
        {
            get;
            private set;
        }

        internal static string ToJson(ThemeParameters param)
        {
            string json = "{";
            List<string> themeList = new List<string>();
            json += "\"layers\":[";
            if (param.Themes != null && param.Themes.Count > 0)
            {
                foreach (var item in param.Themes)
                {
                    themeList.Add(GetThemeLayer(item, param));
                }
            }

            json += string.Join(",", themeList.ToArray());
            json += "]}";
            return json;
        }


        private static string GetThemeLayer(Theme theme, ThemeParameters param)
        {
            string themeLayerJson = "{";

            List<string> themeLayerList = new List<string>();
            List<string> joinItemsList = new List<string>();

            if (theme is ThemeUnique)
            {
                themeLayerList.Add(string.Format("\"theme\":{0}", ThemeUnique.ToJson((ThemeUnique)theme)));
            }
            else if (theme is ThemeRange)
            {
                themeLayerList.Add(string.Format("\"theme\":{0}", ThemeRange.ToJson((ThemeRange)theme)));
            }
            else if (theme is ThemeDotDensity)
            {
                themeLayerList.Add(string.Format("\"theme\":{0}", ThemeDotDensity.ToJson((ThemeDotDensity)theme)));
            }
            else if (theme is ThemeGraduatedSymbol)
            {
                themeLayerList.Add(string.Format("\"theme\":{0}", ThemeGraduatedSymbol.ToJson((ThemeGraduatedSymbol)theme)));
            }
            else if (theme is ThemeGraph)
            {
                themeLayerList.Add(string.Format("\"theme\":{0}", ThemeGraph.ToJson((ThemeGraph)theme)));
            }
            else if (theme is ThemeLabel)
            {
                themeLayerList.Add(string.Format("\"theme\":{0}", ThemeLabel.ToJson((ThemeLabel)theme)));
            }
            //else if (theme is ThemeGridRange)
            //{
            //themeLayerList.Add(string.Format("\"theme\":{0}", ThemeGridRange.ToJson((ThemeGridRange)theme)));
            //}
            //else if (theme is ThemeGridUnique)
            //{
            //themeLayerList.Add(string.Format("\"theme\":{0}", ThemeGridUnique.ToJson((ThemeGridUnique)theme)));
            //}
            else
            {
                themeLayerList.Add("\"theme\":null");
            }

            themeLayerList.Add("\"type\":\"UGC\"");
            themeLayerList.Add("\"ugcLayerType\":\"THEME\"");
            //themeLayerList.Add(string.Format("\"name\":\"{0}\"", param.Name));

            themeLayerList.Add(string.Format("\"datasetInfo\":{0}",
                GetDatasetInfo(param.DatasetName, param.DataSourceName)));

            foreach (var item in param.JoinItems)
            {
                joinItemsList.Add(JoinItem.ToJson(item));
            }

            string joinItemsStr = string.Join(",", joinItemsList);
            themeLayerList.Add(string.Format("\"joinItems\":[{0}]", joinItemsStr));
            themeLayerJson += string.Join(",", themeLayerList.ToArray());
            themeLayerJson += "}";
            return themeLayerJson;
        }

        private static string GetDatasetInfo(string datasetName, string dataSourceName)
        {
            string datasetInfoJson = "{";
            datasetInfoJson += string.Format("\"name\":\"{0}\",\"dataSourceName\":\"{1}\"", datasetName, dataSourceName);
            datasetInfoJson += "}";
            return datasetInfoJson;
        }
    }
}
