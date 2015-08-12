using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeUnique_Title}</para>
    /// 	<para>${REST_ThemeUnique_Description}</para>
    /// 	<para>
    /// 		<img src="ThemeUnique_iServer6.bmp"/>
    /// 	</para>
    /// </summary>
    public class ThemeUnique : Theme
    {
        /// <summary>${REST_ThemeUnique_constructor_D}</summary>
        public ThemeUnique()
        { 
		    ColorGradientType = ColorGradientType.YELLOWRED;
		}

        /// <summary>${REST_ThemeUnique_attribute_Items_D}</summary>
        public IList<ThemeUniqueItem> Items
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeUnique_attribute_UniqueExpression_D}</summary>
        public string UniqueExpression
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeUnique_attribute_ColorGradientType_D}</summary>
        public ColorGradientType ColorGradientType
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeUnique_attribute_DefaultStyle_D}</summary>
        public ServerStyle DefaultStyle
        {
            get;
            set;
        }

        internal static string ToJson(ThemeUnique themeUnique)
        {
            string json = GetThemeInfoJson(themeUnique);
            return json;
        }

        private static string GetThemeInfoJson(ThemeUnique themeUnique)
        {
            if (themeUnique == null)
            {
                return null;
            }

            string themeInfoJson = "{";
            List<string> themeInfoJsonList = new List<string>();

            if (themeUnique.Items != null && themeUnique.Items.Count > 0)
            {
                themeInfoJsonList.Add(string.Format("\"items\":[{0}]", GetThemeItemsJson(themeUnique.Items)));
            }
            else
            {
                themeInfoJsonList.Add("\"items\":[]");
            }

            if (!string.IsNullOrEmpty(themeUnique.UniqueExpression))
            {
                themeInfoJsonList.Add(string.Format("\"uniqueExpression\":\"{0}\"", themeUnique.UniqueExpression));
            }
            else
            {
                themeInfoJsonList.Add("\"uniqueExpression\":\"\"");
            }

            themeInfoJsonList.Add(string.Format("\"colorGradientType\":\"{0}\"", themeUnique.ColorGradientType.ToString()));

            if (themeUnique.DefaultStyle != null)
            {
                themeInfoJsonList.Add(string.Format("\"defaultStyle\":{0}", ServerStyle.ToJson(themeUnique.DefaultStyle)));
            }
            else
            {
                themeInfoJsonList.Add(string.Format("\"defaultStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }
            //添加专题图请求体必须的但不开放给用户的字段
            themeInfoJsonList.Add("\"type\":\"UNIQUE\"");
            if (themeUnique.MemoryData != null)
            {
                themeInfoJsonList.Add("\"memoryData\":" + themeUnique.ToJson(themeUnique.MemoryData));
            }
            else
            {
                themeInfoJsonList.Add("\"memoryData\":null");
            }
            themeInfoJson += string.Join(",", themeInfoJsonList.ToArray());
            themeInfoJson += "}";
            return themeInfoJson;
        }

        private static string GetThemeItemsJson(IList<ThemeUniqueItem> items)
        {
            List<string> themeItemsJsonList = new List<string>();

            foreach (var item in items)
            {
                themeItemsJsonList.Add(ThemeUniqueItem.ToJson(item));
            }

            return string.Join(",", themeItemsJsonList.ToArray());
        }

        internal static ThemeUnique FromJson(JsonObject json)
        {
            if (json == null) { return null; }
            ThemeUnique themeUnique = new ThemeUnique();

            if (json["defaultStyle"].ValueType != JsonValueType.Null)
            {
                themeUnique.DefaultStyle = ServerStyle.FromJson(json["defaultStyle"].GetObjectEx());
            }

            if (json["colorGradientType"].ValueType!= JsonValueType.Null)
            {
                themeUnique.ColorGradientType = (ColorGradientType)Enum.Parse(typeof(ColorGradientType), json["colorGradientType"].GetStringEx(), true);
            }
            else
            {
                //这里不处理为空时的情况
            }
            List<ThemeUniqueItem> items = new List<ThemeUniqueItem>();
            if (json["items"].ValueType != JsonValueType.Null && json["items"].GetArray().Count > 0)
            {
                for (int i = 0; i < json["items"].GetArray().Count; i++)
                {
                    items.Add(ThemeUniqueItem.FromJson((JsonValue)json["items"].GetArray()[i]));
                }
            }
            themeUnique.Items = items;
            themeUnique.UniqueExpression = json["uniqueExpression"].GetStringEx();
            return themeUnique;
        }
    }
}
