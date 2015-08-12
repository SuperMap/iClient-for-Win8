using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Data.Json;
using Windows.UI;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_ServerTextStyle_Title}</summary>
    public class ServerTextStyle
    {
        /// <summary>${REST_ServerTextStyle_constructor_D}</summary>
        public ServerTextStyle()
        {
            FontHeight = 10;
            FontWeight = 400;
            FontWidth = 0;
            FontName = "微软雅黑";
            SizeFixed = true;
        }

        /// <summary>${REST_ServerTextStyle_attribute_align_D}</summary>
        public TextAlignment Align { get; set; }             //文本的对齐方式。 

        /// <summary>${REST_ServerTextStyle_attribute_backColor_D}</summary>
        public Color BackColor { get; set; }                        //文本的背景色。 

        /// <summary>${REST_ServerTextStyle_attribute_backOpaque_D}</summary>
        public bool BackOpaque { get; set; }                        //文本背景是否不透明，true 表示文本背景不透明。 

        /// <summary>${REST_ServerTextStyle_attribute_bold_D}</summary>
        public bool Bold { get; set; }                              //文本是否为粗体字，true 表示为粗体。 

        ///// <summary>${REST_ServerTextStyle_attribute_fixedTextSize_D}</summary>
        //public int FixedTextSize { get; set; }                      //固定文本的大小尺寸。 

        /// <summary>${REST_ServerTextStyle_attribute_fontHeight_D}</summary>
        public double FontHeight { get; set; }                      //文本字体的高度。 

        /// <summary>${REST_ServerTextStyle_attribute_fontName_D}</summary>
        public string FontName { get; set; }                        //文本字体的名称。 

        ///// <summary>${REST_ServerTextStyle_attribute_fontScale_D}</summary>
        //public double FontScale { get; set; }                       //文本字体的缩放比例。 

        /// <summary>${REST_ServerTextStyle_attribute_fontWeight_D}</summary>
        public int FontWeight { get; set; }                         //文本字体的磅数，表示粗体的具体数值。 

        /// <summary>${REST_ServerTextStyle_attribute_fontWidth_D}</summary>
        public double FontWidth { get; set; }                       //文本字体的宽度。 

        /// <summary>${REST_ServerTextStyle_attribute_foreColor_D}</summary>
        public Color ForeColor { get; set; }                        //文本的前景色。 

        /// <summary>${REST_ServerTextStyle_attribute_italic_D}</summary>
        public bool Italic { get; set; }                            //文本是否采用斜体，true 表示采用斜体。 

        /// <summary>${REST_ServerTextStyle_attribute_italicAngle_D}</summary>
        public double ItalicAngle { get; set; }                     //字体倾斜角度，正负度之间，以度为单位，精确到0.1度。 

        /// <summary>${REST_ServerTextStyle_attribute_opaqueRate_D}</summary>
        public int OpaqueRate { get; set; }                         //注记文字的不透明度。 

        /// <summary>${REST_ServerTextStyle_attribute_outline_D}</summary>
        public bool Outline { get; set; }                           //是否以轮廓的方式来显示文本的背景。 

        /// <summary>${REST_ServerTextStyle_attribute_rotation_D}</summary>
        public double Rotation { get; set; }                        //文本旋转的角度。 

        /// <summary>${REST_ServerTextStyle_attribute_shadow_D}</summary>
        public bool Shadow { get; set; }                            //文本是否有阴影。 

        /// <summary>${REST_ServerTextStyle_attribute_sizeFixed_D}</summary>
        public bool SizeFixed { get; set; }                         //文本大小是否固定。 

        /// <summary>${REST_ServerTextStyle_attribute_strikeout_D}</summary>
        public bool Strikeout { get; set; }                         //文本字体是否加删除线。 

        /// <summary>${REST_ServerTextStyle_attribute_underline_D}</summary>
        public bool Underline { get; set; }                         //文本字体是否加下划线。         

        internal static string ToJson(ServerTextStyle serverTextStyle)
        {
            List<string> jsonlist = new List<string>();
            string json = "{";

            jsonlist.Add(string.Format("\"align\":\"{0}\"", serverTextStyle.Align.ToString()));
            //backcolor格式不对，提交时将此注释删除
            jsonlist.Add(string.Format("\"backColor\":{0}", ServerColor.ToJson(serverTextStyle.BackColor.ToServerColor())));
            jsonlist.Add(string.Format("\"backOpaque\":\"{0}\"", serverTextStyle.BackOpaque.ToString().ToLower()));
            jsonlist.Add(string.Format("\"bold\":\"{0}\"", serverTextStyle.Bold.ToString().ToLower()));
            //jsonlist.Add(string.Format("\"fixedTextSize\":\"{0}\"", serverTextStyle.FixedTextSize.ToString()));
            jsonlist.Add(string.Format("\"fontHeight\":\"{0}\"", serverTextStyle.FontHeight.ToString()));
            jsonlist.Add(string.Format("\"fontName\":\"{0}\"", serverTextStyle.FontName));
            //jsonlist.Add(string.Format("\"fontScale\":\"{0}\"", serverTextStyle.FontScale.ToString()));
            jsonlist.Add(string.Format("\"fontWeight\":\"{0}\"", serverTextStyle.FontWeight.ToString()));
            jsonlist.Add(string.Format("\"fontWidth\":\"{0}\"", serverTextStyle.FontWidth.ToString()));
            //foreColor格式不对，提交时将此注释删除
            jsonlist.Add(string.Format("\"foreColor\":{0}", ServerColor.ToJson(serverTextStyle.ForeColor.ToServerColor())));
            jsonlist.Add(string.Format("\"italic\":\"{0}\"", serverTextStyle.Italic.ToString().ToLower()));
            jsonlist.Add(string.Format("\"italicAngle\":\"{0}\"", serverTextStyle.ItalicAngle.ToString()));
            jsonlist.Add(string.Format("\"opaqueRate\":\"{0}\"", serverTextStyle.OpaqueRate.ToString()));
            jsonlist.Add(string.Format("\"outline\":\"{0}\"", serverTextStyle.Outline.ToString().ToLower()));
            jsonlist.Add(string.Format("\"rotation\":\"{0}\"", serverTextStyle.Rotation.ToString()));
            jsonlist.Add(string.Format("\"shadow\":\"{0}\"", serverTextStyle.Shadow.ToString().ToLower()));
            jsonlist.Add(string.Format("\"sizeFixed\":\"{0}\"", serverTextStyle.SizeFixed.ToString().ToLower()));
            jsonlist.Add(string.Format("\"strikeout\":\"{0}\"", serverTextStyle.Strikeout.ToString().ToLower()));
            jsonlist.Add(string.Format("\"underline\":\"{0}\"", serverTextStyle.Underline.ToString().ToLower()));

            json += string.Join(",", jsonlist.ToArray());
            json += "}";
            return json;
        }

        internal static ServerTextStyle FromJson(JsonObject json)
        {
            if (json == null) return null;
            ServerTextStyle textStyle = new ServerTextStyle();

            textStyle.Align = (TextAlignment)Enum.Parse(typeof(TextAlignment), json["align"].GetStringEx(), true);
            textStyle.BackColor = ServerColor.FromJson(json["backColor"].GetObjectEx()).ToColor();
            textStyle.BackOpaque = json["backOpaque"].GetBooleanEx();
            textStyle.Bold = json["bold"].GetBooleanEx();
            textStyle.FontHeight = json["fontHeight"].GetNumberEx();
            textStyle.FontName = json["fontName"].GetStringEx();
            textStyle.FontWeight = (int)json["fontWeight"].GetNumberEx();
            textStyle.FontWidth = json["fontWidth"].GetNumberEx();
            textStyle.ForeColor = ServerColor.FromJson(json["foreColor"].GetObjectEx()).ToColor();
            textStyle.Italic = json["italic"].GetBooleanEx();
            textStyle.ItalicAngle = json["italicAngle"].GetNumberEx();
            textStyle.OpaqueRate = (int)json["opaqueRate"].GetNumberEx();
            textStyle.Outline = json["outline"].GetBooleanEx();
            textStyle.Rotation = json["rotation"].GetNumberEx();
            textStyle.Shadow = json["shadow"].GetBooleanEx();
            textStyle.SizeFixed = json["sizeFixed"].GetBooleanEx();
            textStyle.Strikeout = json["strikeout"].GetBooleanEx();
            textStyle.Underline = json["underline"].GetBooleanEx();
            return textStyle;
        }
    }
}
