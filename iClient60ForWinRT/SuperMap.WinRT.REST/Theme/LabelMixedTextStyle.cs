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
    /// 	<para>${REST_LabelMixedTextStyle_Title}</para>
    /// 	<para>${REST_LabelMixedTextStyle_Description}</para>
    /// </summary>
    public class LabelMixedTextStyle
    {
        /// <summary>${REST_LabelMixedTextStyle_constructor_D}</summary>
        public LabelMixedTextStyle()
        { 
        }

        /// <summary>${REST_LabelMixedTextStyle_attribute_DefaultStyle_D}</summary>
        public ServerTextStyle DefaultStyle
        {
            get;
            set;
        }

        /// <summary>${REST_LabelMixedTextStyle_attribute_Sparator_D}</summary>
        public string Separator
        {
            get;
            set;
        }

        /// <summary>${REST_LabelMixedTextStyle_attribute_SeparatorEnabled_D}</summary>
        public bool SeparatorEnabled
        {
            get;
            set;
        }

        /// <summary>${REST_LabelMixedTextStyle_attribute_SplitIndexes_D}</summary>
        public IList<int> SplitIndexes
        {
            get;
            set;
        }

        /// <summary>${REST_LabelMixedTextStyle_attribute_Styles_D}</summary>
        public IList<ServerTextStyle> Styles
        {
            get;
            set;
        }

        internal static string ToJson(LabelMixedTextStyle labelMixedTextStyle)
        {
            string json = "{";
            System.Collections.Generic.List<string> list = new List<string>();

            if (labelMixedTextStyle.DefaultStyle != null)
            {
                list.Add(string.Format("\"defaultStyle\":{0}", ServerTextStyle.ToJson(labelMixedTextStyle.DefaultStyle)));
            }
            else
            {
                list.Add(string.Format("\"defaultStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
            }

            if (!string.IsNullOrEmpty(labelMixedTextStyle.Separator))
            {
                list.Add(string.Format("\"separator\":\"{0}\"", labelMixedTextStyle.Separator));
            }
            else
            {
                list.Add("\"separator\":\"\"");
            }

            list.Add(string.Format("\"separatorEnabled\":{0}", labelMixedTextStyle.SeparatorEnabled.ToString().ToLower()));

            if (labelMixedTextStyle.SplitIndexes != null && labelMixedTextStyle.SplitIndexes.Count > 0)
            {
                List<string> splitList = new List<string>();
                for (int i = 0; i < labelMixedTextStyle.SplitIndexes.Count; i++)
                {
                    splitList.Add(string.Format("{0}", labelMixedTextStyle.SplitIndexes[i].ToString()));
                }
                list.Add(string.Format("\"splitIndexes\":[{0}]", string.Join(",", splitList.ToArray())));
            }
            else
            {
                list.Add("\"splitIndexes\":[]");
            }

            if (labelMixedTextStyle.Styles != null && labelMixedTextStyle.Styles.Count > 0)
            {
                List<string> stylesList = new List<string>();
                foreach (var item in labelMixedTextStyle.Styles)
                {
                    stylesList.Add(ServerTextStyle.ToJson(item));
                }

                list.Add(string.Format("\"styles\":[{0}]", string.Join(",", stylesList.ToArray())));
            }
            else
            {
                list.Add("\"styles\":[]");
            }
            json += string.Join(",", list.ToArray());

            json += "}";
            return json;
        }

        internal static LabelMixedTextStyle FromJson(JsonObject json)
        {
            if (json == null) return null;
            LabelMixedTextStyle textStyle = new LabelMixedTextStyle();
            textStyle.DefaultStyle = ServerTextStyle.FromJson(json["defaultStyle"].GetObjectEx());
            textStyle.Separator = json["separator"].GetStringEx();
            textStyle.SeparatorEnabled = json["separatorEnabled"].GetBooleanEx();

            if (json["splitIndexes"].GetArray() != null && (json["splitIndexes"].GetArray()).Count > 0)
            {
                List<int> list = new List<int>();
                foreach (JsonValue item in json["splitIndexes"].GetArray())
                {
                    list.Add((int)item.GetNumber());
                }
                textStyle.SplitIndexes = list;
            }

            if (json["styles"] != null)
            {
                List<ServerTextStyle> textStyleList = new List<ServerTextStyle>();
                foreach (JsonObject item in (JsonArray)json["styles"])
                {
                    textStyleList.Add(ServerTextStyle.FromJson(item));
                }
                textStyle.Styles = textStyleList;
            }

            return textStyle;
        }
    }
}
