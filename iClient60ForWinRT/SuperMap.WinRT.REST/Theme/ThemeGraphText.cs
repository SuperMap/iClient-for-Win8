using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeGraphText_Title}</para>
    /// 	<para>${REST_ThemeGraphText_Description}</para>
    /// </summary>
    public class ThemeGraphText
    {
        /// <summary>${REST_ThemeGraphText_constructor_D}</summary>
        public ThemeGraphText()
        {
            //GraphTextDisplayed = false;
        }
        /// <summary>${REST_ThemeGraphText_attribute_graphTextDisplayed_D}</summary>
        public bool GraphTextDisplayed
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphText_attribute_graphTextFormat_D}</summary>
        public ThemeGraphTextFormat GraphTextFormat
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphText_attribute_graphTextStyle_D}</summary>
        public ServerTextStyle GraphTextStyle
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraphText graphText)
        {
            string json = "";
            List<string> list = new List<string>();

            list.Add(string.Format("\"graphTextDisplayed\":{0}", graphText.GraphTextDisplayed.ToString().ToLower()));

            list.Add(string.Format("\"graphTextFormat\":\"{0}\"", graphText.GraphTextFormat));

            if (graphText.GraphTextStyle != null)
            {
                list.Add(string.Format("\"graphTextStyle\":{0}", ServerTextStyle.ToJson(graphText.GraphTextStyle)));
            }
            else
            {
                list.Add(string.Format("\"graphTextStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
            }
            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeGraphText FromJson(JsonObject json)
        {
            if (json == null) return null;

            ThemeGraphText graphText = new ThemeGraphText();
            graphText.GraphTextDisplayed = json["graphTextDisplayed"].GetBooleanEx();
            if (json["graphTextFormat"] != null)
            {
                graphText.GraphTextFormat = (ThemeGraphTextFormat)Enum.Parse(typeof(ThemeGraphTextFormat), json["graphTextFormat"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }
            graphText.GraphTextStyle = ServerTextStyle.FromJson(json["graphTextStyle"].GetObjectEx());
            return graphText;
        }
    }
}
