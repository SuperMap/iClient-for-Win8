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
    /// 	<para>${REST_ThemeLabelItem_Title}</para>
    /// 	<para>${REST_ThemeLabelItem_Description}</para>
    /// </summary>
    public class ThemeLabelItem
    {
        /// <summary>${REST_ThemeLabelItem_constructor_D}</summary>
        public ThemeLabelItem()
        {
            Visible = true;
        }

        /// <summary>${REST_ThemeLabelItem_attribute_caption_D}</summary>
        public string Caption
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelItem_attribute_start_D}</summary>
        public double Start
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelItem_attribute_end_D}</summary>
        public double End
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelItem_attribute_visible_D}</summary>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelItem_attribute_style_D}</summary>
        public ServerTextStyle Style
        {
            get;
            set;
        }

        internal static string ToJson(ThemeLabelItem themeLabelItem)
        {
            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(themeLabelItem.Caption))
            {
                list.Add(string.Format("\"caption\":\"{0}\"", themeLabelItem.Caption));
            }
            else
            {
                list.Add("\"caption\":\"\"");
            }

            if (themeLabelItem.Style != null)
            {
                list.Add(string.Format("\"style\":{0}", ServerTextStyle.ToJson(themeLabelItem.Style)));
            }
            else
            {
                list.Add(string.Format("\"style\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
            }

            list.Add(string.Format("\"visible\":{0}", themeLabelItem.Visible.ToString().ToLower()));

            list.Add(string.Format("\"end\":\"{0}\"", themeLabelItem.End.ToString()));

            list.Add(string.Format("\"start\":\"{0}\"", themeLabelItem.Start.ToString()));

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ThemeLabelItem FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeLabelItem item = new ThemeLabelItem();
            item.Caption = json["caption"].GetStringEx();
            item.End = json["end"].GetNumberEx();
            item.Start = json["start"].GetNumberEx();
            if (json["style"] != null)
            {
                item.Style = ServerTextStyle.FromJson(json["style"].GetObjectEx());
            }
            item.Visible = json["visible"].GetBooleanEx();
            return item;
        }

    }
}
