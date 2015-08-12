using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeLabelText_Title}</para>
    /// 	<para>${REST_ThemeLabelText_Description}</para>
    /// </summary>
    public class ThemeLabelText
    {
        /// <summary>${REST_ThemeLabelText_constructor_D}</summary>
        public ThemeLabelText()
        {
        }

        /// <summary>${REST_ThemeLabelText_attribute_MaxTextHeight_D}</summary>
        public int MaxTextHeight
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelText_attribute_MaxTextWidth_D}</summary>
        public int MaxTextWidth
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelText_attribute_MinTextWidth_D}</summary>
        public int MinTextHeight
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelText_attribute_MinTextHeight_D}</summary>
        public int MinTextWidth
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelText_attribute_UniformSytle_D}</summary>
        public ServerTextStyle UniformStyle
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelText_attribute_UniformMixedSytle_D}</summary>
        public LabelMixedTextStyle UniformMixedStyle
        {
            get;
            set;
        }

        internal static string ToJson(ThemeLabelText labelText)
        {
            string json = "";

            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

            list.Add(string.Format("\"minTextHeight\":{0}", labelText.MinTextHeight.ToString()));
            list.Add(string.Format("\"maxTextWidth\":{0}", labelText.MaxTextWidth.ToString()));
            list.Add(string.Format("\"minTextWidth\":{0}", labelText.MinTextWidth.ToString()));
            list.Add(string.Format("\"maxTextHeight\":{0}", labelText.MaxTextHeight.ToString()));

            if (labelText.UniformStyle != null)
            {
                list.Add(string.Format("\"uniformStyle\":{0}", ServerTextStyle.ToJson(labelText.UniformStyle)));
            }
            else
            {
                list.Add(string.Format("\"uniformStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
            }

            if (labelText.UniformMixedStyle != null)
            {
                list.Add(string.Format("\"uniformMixedStyle\":{0}", LabelMixedTextStyle.ToJson(labelText.UniformMixedStyle)));
            }
            else
            {
                list.Add("\"uniformMixedStyle\":null");
            }

            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeLabelText FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            ThemeLabelText labelText = new ThemeLabelText();
            labelText.MaxTextHeight = (int)json["maxTextHeight"].GetNumberEx();
            labelText.MaxTextWidth = (int)json["maxTextWidth"].GetNumberEx();
            labelText.MinTextHeight = (int)json["minTextHeight"].GetNumberEx();
            labelText.MinTextWidth = (int)json["minTextWidth"].GetNumberEx();
            labelText.UniformMixedStyle = LabelMixedTextStyle.FromJson(json["uniformMixedStyle"].GetObjectEx());
            labelText.UniformStyle = ServerTextStyle.FromJson(json["uniformStyle"].GetObjectEx());
            return labelText;
        }
    }
}
