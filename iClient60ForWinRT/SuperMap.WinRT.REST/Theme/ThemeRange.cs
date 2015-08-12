using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeRange_Title}</para>
    /// 	<para>${REST_ThemeRange_Description}</para>
    /// 	<para>
    /// 		<img src="ThemeRange_iServer6.bmp"/>
    /// 	</para>
    /// </summary>
    public class ThemeRange : Theme
    {
        /// <summary>${REST_ThemeRange_constructor_D}</summary>
        public ThemeRange()
        {
            ColorGradientType = ColorGradientType.YELLOWRED;
        }

        /// <summary>${REST_ThemeRange_attribute_RangeExpression_D}</summary>
        public string RangeExpression
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeRange_attribute_RangeMode_D}</summary>
        public RangeMode RangeMode
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

        /// <summary>${REST_ThemeRange_attribute_RangeParameter_D}</summary>
        public double RangeParameter
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeRange_attribute_Items_D}</summary>
        public IList<ThemeRangeItem> Items
        {
            get;
            set;
        }

        internal static string ToJson(ThemeRange themeRange)
        {
            string json = "{";
            List<string> list = new List<string>();

            if (themeRange.Items != null && themeRange.Items.Count >= 1)
            {
                List<string> tempTUI = new List<string>();
                foreach (var item in themeRange.Items)
                {
                    tempTUI.Add(ThemeRangeItem.ToJson(item));
                }

                list.Add(string.Format("\"items\":[{0}]", String.Join(",", tempTUI.ToArray())));
            }
            else
            {
                list.Add("\"items\":[]");
            }

            list.Add(string.Format("\"rangeParameter\":\"{0}\"", themeRange.RangeParameter.ToString()));

            list.Add(string.Format("\"rangeMode\":\"{0}\"", themeRange.RangeMode));
            list.Add(string.Format("\"colorGradientType\":\"{0}\"", themeRange.ColorGradientType.ToString()));
            if (!string.IsNullOrEmpty(themeRange.RangeExpression))
            {
                list.Add(string.Format("\"rangeExpression\":\"{0}\"", themeRange.RangeExpression));
            }
            else
            {
                list.Add("\"rangeExpression\":\"\"");
            }

            if (themeRange.MemoryData != null)
            {
                list.Add("\"memoryData\":" + themeRange.ToJson(themeRange.MemoryData));
            }
            else
            {
                list.Add("\"memoryData\":null");
            }
            list.Add("\"type\":\"RANGE\"");

            json += string.Join(",", list.ToArray());
            json += "}";

            return json;
        }

        internal static ThemeRange FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeRange themeRange = new ThemeRange();

            if (json["items"].ValueType != JsonValueType.Null && json["items"].GetObjectEx().Count > 0)
            {
                List<ThemeRangeItem> itemsList = new List<ThemeRangeItem>();
                foreach (JsonObject item in json["items"].GetArray())
                {
                    itemsList.Add(ThemeRangeItem.FromJson(item));
                }

                themeRange.Items = itemsList;
            }

            if (json["colorGradientType"].ValueType != JsonValueType.Null)
            {
                themeRange.ColorGradientType = (ColorGradientType)Enum.Parse(typeof(ColorGradientType), json["colorGradientType"].GetStringEx(), true);
            }
            else
            {
                //这里不处理为空时的情况
            }

            themeRange.RangeExpression = json["rangeExpression"].GetStringEx();
            if (json["rangeMode"].ValueType!= JsonValueType.Null)
            {
                themeRange.RangeMode = (RangeMode)Enum.Parse(typeof(RangeMode), json["rangeMode"].GetStringEx(), true);
            }
            else
            {
                //不处理Null的情况
            }
            themeRange.RangeParameter = json["rangeParameter"].GetNumberEx();
            return themeRange;
        }
    }
}
