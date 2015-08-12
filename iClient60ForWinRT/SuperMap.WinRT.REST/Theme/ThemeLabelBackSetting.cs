using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeLabelBackGround_Title}</para>
    /// 	<para>${REST_ThemeLabelBackGround_Description}</para>
    /// </summary>
    public class ThemeLabelBackSetting
    {
        /// <summary>${REST_ThemeLabelBackGround_constructor_D}</summary>
        public ThemeLabelBackSetting()
        {
            //this.LabelBackShape = LabelBackShape.ROUNDRECT;
        }

        /// <summary>${REST_ThemeLabelBackGround_attribute_labelBackShape_D}</summary>
        public LabelBackShape LabelBackShape
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelBackGround_attribute_backStyle_D}</summary>
        public ServerStyle BackStyle
        {
            get;
            set;
        }

        internal static string ToJson(ThemeLabelBackSetting labelBackSetting)
        {
            string json = "";
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

            if (labelBackSetting.BackStyle != null)
            {
                list.Add(string.Format("\"backStyle\":{0}", ServerStyle.ToJson(labelBackSetting.BackStyle)));
            }
            else
            {
                list.Add(string.Format("\"backStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            list.Add(string.Format("\"labelBackShape\":\"{0}\"", labelBackSetting.LabelBackShape));
            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeLabelBackSetting FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeLabelBackSetting backSetting = new ThemeLabelBackSetting();

            if (json["backStyle"] != null)
            {
                backSetting.BackStyle = ServerStyle.FromJson(json["backStyle"].GetObjectEx());
            }

            if (json["labelBackShape"] != null)
            {
                backSetting.LabelBackShape = (LabelBackShape)Enum.Parse(typeof(LabelBackShape), json["labelBackShape"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }
            return backSetting;
        }
    }
}
