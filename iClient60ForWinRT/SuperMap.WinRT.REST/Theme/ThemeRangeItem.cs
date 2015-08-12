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
    /// 	<para>${REST_ThemeRangeItem_Title}</para>
    /// 	<para>${REST_ThemeRangeItem_Description}</para>
    /// </summary>
    public class ThemeRangeItem
    {
        /// <summary>${REST_ThemeRangeItem_constructor_D}</summary>
        public ThemeRangeItem()
        {
            this.Visible = true;
        }

        /// <summary>${REST_ThemeRangeItem_attribute_caption_D}</summary>
        public string Caption
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeRangeItem_attribute_Start_D}</summary>
        public double Start
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeRangeItem_attribute_End_D}</summary>
        public double End
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeRangeItem_attribute_visible_D}</summary>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeRangeItem_attribute_style_D}</summary>
        public ServerStyle Style
        {
            get;
            set;
        }

        internal static string ToJson(ThemeRangeItem themeRangeItem)
        {
            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(themeRangeItem.Caption))
            {
                list.Add(string.Format("\"caption\":\"{0}\"", themeRangeItem.Caption));
            }
            else
            {
                list.Add("\"caption\":\"\"");
            }

            if (themeRangeItem.Style != null)
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(themeRangeItem.Style)));
            }
            else
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            list.Add(string.Format("\"visible\":{0}", themeRangeItem.Visible.ToString().ToLower()));

            list.Add(string.Format("\"end\":{0}", themeRangeItem.End.ToString()));

            list.Add(string.Format("\"start\":{0}", themeRangeItem.Start.ToString()));

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ThemeRangeItem FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeRangeItem item = new ThemeRangeItem();

            item.Caption = json["caption"].GetStringEx();
            item.End = json["end"].GetNumberEx();
            item.Start = (json["start"].GetNumberEx());
            if (json["style"] != null)
            {
                item.Style = ServerStyle.FromJson(json["style"].GetObjectEx());
            }
            item.Visible = json["visible"].GetBooleanEx();
            return item;
        }
    }
}
