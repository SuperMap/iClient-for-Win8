using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeGraphAxes_Title}</para>
    /// 	<para>${REST_ThemeGraphAxes_Description}</para>
    /// </summary>
    public class ThemeGraphAxes
    {
        /// <summary>${REST_ThemeGraphAxes_constructor_D}</summary>
        public ThemeGraphAxes()
        {
        }

        /// <summary>${REST_ThemeGraphAxes_attribute_axesColor_D}</summary>
        public ServerColor AxesColor
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphAxes_attribute_axesDisplayed_D}</summary>
        public bool AxesDisplayed
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphAxes_attribute_axesGridDisplayed_D}</summary>
        public bool AxesGridDisplayed
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphAxes_attribute_axesTextDisplayed_D}</summary>
        public bool AxesTextDisplayed
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphAxes_attribute_axesTextStyle_D}</summary>
        public ServerTextStyle AxesTextStyle
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraphAxes graphAxes)
        {
            string json = "";

            List<string> list = new List<string>();

            if (graphAxes.AxesColor != null)
            {
                list.Add(string.Format("\"axesColor\":{0}", ServerColor.ToJson(graphAxes.AxesColor)));
            }
            else
            {
                list.Add(string.Format("\"axesColor\":{0}", ServerColor.ToJson(new ServerColor())));
            }

            list.Add(string.Format("\"axesDisplayed\":{0}", graphAxes.AxesDisplayed.ToString().ToLower()));

            list.Add(string.Format("\"axesGridDisplayed\":{0}", graphAxes.AxesGridDisplayed.ToString().ToLower()));

            list.Add(string.Format("\"axesTextDisplayed\":{0}", graphAxes.AxesTextDisplayed.ToString().ToLower()));

            if (graphAxes.AxesTextStyle != null)
            {
                list.Add(string.Format("\"axesTextStyle\":{0}", ServerTextStyle.ToJson(graphAxes.AxesTextStyle)));
            }
            else
            {
                list.Add(string.Format("\"axesTextStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
            }

            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeGraphAxes FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeGraphAxes graphAxes = new ThemeGraphAxes();
            graphAxes.AxesColor = ServerColor.FromJson(json["axesColor"].GetObjectEx());
            graphAxes.AxesDisplayed = json["axesDisplayed"].GetBooleanEx();
            graphAxes.AxesGridDisplayed = json["axesGridDisplayed"].GetBooleanEx();
            graphAxes.AxesTextDisplayed = json["axesTextDisplayed"].GetBooleanEx();
            graphAxes.AxesTextStyle = ServerTextStyle.FromJson(json["axesTextStyle"].GetObjectEx());
            return graphAxes;
        }
    }
}
