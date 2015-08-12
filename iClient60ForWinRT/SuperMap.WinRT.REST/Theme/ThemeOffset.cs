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
    /// 	<para>${REST_ThemeOffset_Title}</para>
    /// 	<para>${REST_ThemeOffset_Description}</para>
    /// </summary>
    public class ThemeOffset
    {
        /// <summary>${REST_ThemeOffset_constructor_D}</summary>
        public ThemeOffset()
        { }

        /// <summary>${REST_ThemeOffset_attribute_OffsetFixed_D}</summary>
        public bool OffsetFixed
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeOffset_attribute_OffsetX_D}</summary>
        public string OffsetX
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeOffset_attribute_OffsetY_D}</summary>
        public string OffsetY
        {
            get;
            set;
        }

        internal static string ToJson(ThemeOffset offsetTheme)
        {
            string json = "";

            List<string> list = new List<string>();
            list.Add(string.Format("\"offsetFixed\":{0}", offsetTheme.OffsetFixed.ToString().ToLower()));

            if (!string.IsNullOrEmpty(offsetTheme.OffsetX))
            {
                list.Add(string.Format("\"offsetX\":\"{0}\"", offsetTheme.OffsetX));
            }
            else
            {
                list.Add("\"offsetX\":\"\"");
            }

            if (!string.IsNullOrEmpty(offsetTheme.OffsetY))
            {
                list.Add(string.Format("\"offsetY\":\"{0}\"", offsetTheme.OffsetY));
            }
            else
            {
                list.Add("\"offsetY\":\"\"");
            }

            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeOffset FromJson(JsonObject json)
        {
            if (json == null)
                return null;
            ThemeOffset themeOffset = new ThemeOffset();
            themeOffset.OffsetFixed = json["offsetFixed"].GetBooleanEx();
            themeOffset.OffsetX = json["offsetX"].GetStringEx();
            themeOffset.OffsetY = json["offsetY"].GetStringEx();
            return themeOffset;
        }
    }
}
