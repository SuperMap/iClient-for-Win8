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
    /// 	<para>${REST_ThemeDotDensity_Title}</para>
    /// 	<para>${REST_ThemeDotDensity_Description}</para>
    /// 	<para>
    /// 		<img src="ThemeDotDensityiServer6.bmp"/>
    /// 	</para>
    /// </summary>
    public class ThemeDotDensity : Theme
    {
        /// <summary>${REST_ThemeDotDensity_constructor_D}</summary>
        public ThemeDotDensity()
        {
            Style = new ServerStyle { MarkerSize = 2 };
        }

        /// <summary>${REST_ThemeDotDensity_attribute_DotExpression_D}</summary>
        public string DotExpression
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeDotDensity_attribute_Value_D}</summary>
        public double Value
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeDotDensity_attribute_Style_D}</summary>
        public ServerStyle Style
        {
            get;
            set;
        }

        internal static string ToJson(ThemeDotDensity dotDensity)
        {
            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(dotDensity.DotExpression))
            {
                list.Add(string.Format("\"dotExpression\":\"{0}\"", dotDensity.DotExpression));
            }
            else
            {
                list.Add("\"dotExpression\":\"\"");
            }

            if (dotDensity.Style != null)
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(dotDensity.Style)));
            }
            else
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            list.Add(string.Format("\"value\":{0}", dotDensity.Value.ToString()));

            list.Add("\"type\":\"DOTDENSITY\"");

            if (dotDensity.MemoryData!=null)
            {
                list.Add( "\"memoryData\":" + dotDensity.ToJson(dotDensity.MemoryData));
            }
            else
            {
                list.Add("\"memoryData\":null");
            }

            json += string.Join(",", list.ToArray());

            json += "}";
            return json;
        }

        internal static ThemeDotDensity FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeDotDensity dotDensity = new ThemeDotDensity();
            dotDensity.DotExpression = json["dotExpression"].GetStringEx();
            dotDensity.Style = ServerStyle.FromJson(json["style"].GetObjectEx());
            dotDensity.Value = json["value"].GetNumberEx();
            return dotDensity;
        }
    }
}
