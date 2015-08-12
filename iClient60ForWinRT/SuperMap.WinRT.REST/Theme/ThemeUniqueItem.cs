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
    /// 	<para>${REST_ThemeUniqueItem_Title}</para>
    /// 	<para>${REST_ThemeUniqueItem_Description}</para>
    /// </summary>
    public class ThemeUniqueItem
    {
        /// <summary>${REST_ThemeUniqueItem_constructor_D}</summary>
        public ThemeUniqueItem()
        {
            Visible = true;
        }

        /// <summary>${REST_ThemeUniqueItem_attribute_caption_D}</summary>
        public string Caption
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeUniqueItem_attribute_unique_D}</summary>
        public string Unique
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeUniqueItem_attribute_style_D}</summary>
        public ServerStyle Style
        {
            get;
            set;
        }


        /// <summary>${REST_ThemeUniqueItem_attribute_visible_D}</summary>
        public bool Visible
        {
            get;
            set;
        }

        internal static string ToJson(ThemeUniqueItem item)
        {
            string json = "{";

            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(item.Caption))
            {
                list.Add(string.Format("\"caption\":\"{0}\"", item.Caption));
            }
            else
            {
                list.Add("\"caption\":\"\"");
            }

            //默认值这边儿是个问题，又不要判断呢？
            list.Add(string.Format("\"visible\":{0}", item.Visible.ToString().ToLower()));

            if (item.Style != null)
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(item.Style)));
            }
            else
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (!string.IsNullOrEmpty(item.Unique))
            {
                list.Add(string.Format("\"unique\":\"{0}\"", item.Unique));
            }
            else
            {
                list.Add("\"unique\":\"\"");
            }

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ThemeUniqueItem FromJson(JsonValue jsonValue)
        {
            if (jsonValue == null) { return null; };
            ThemeUniqueItem item = new ThemeUniqueItem();

            item.Caption = jsonValue.GetObject()["caption"].GetStringEx();
            item.Style = ServerStyle.FromJson(jsonValue.GetObject()["style"].GetObjectEx());
            item.Unique = jsonValue.GetObject()["unique"].GetStringEx();
            item.Visible = jsonValue.GetObject()["visible"].GetBooleanEx();
            return item;
        }
    }
}
