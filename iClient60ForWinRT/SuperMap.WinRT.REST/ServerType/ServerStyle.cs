using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Data.Json;
using Windows.UI;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ServerType_ServerStyle_Tile}</para>
    /// 	<para>${REST_ServerType_ServerStyle_Description}</para>
    /// </summary>
    public class ServerStyle
    {
        /// <summary>${REST_ServerType_ServerStyle_constructor_None_D}</summary>
        public ServerStyle()
        {
            FillBackColor = new Color { R = 255, G = 255, B = 255 };
            FillForeColor = new Color { R = 255, G = 0, B = 0 };
            LineColor = new Color { R = 0, G = 0, B = 0 };
            FillOpaqueRate = 100;
            LineWidth = 1.0;
            MarkerSize = 1;
            MarkerSymbolID = 0;
        }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillBackOpaque_D}</summary>
        public bool FillBackOpaque { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_fillBackColor_D}</summary>
        public Color FillBackColor { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillForeColor_D}</summary>
        public Color FillForeColor { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillGradientOffsetRatioX_D}</summary>
        public double FillGradientOffsetRatioX { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillGradientOffsetRatioY_D}</summary>
        public double FillGradientOffsetRatioY { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillOpaqueRate_D}</summary>
        public int FillOpaqueRate { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillGradientMode_D}</summary>
        public FillGradientMode FillGradientMode { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_FillSymbolID_D}</summary>
        public int FillSymbolID { get; set; }
        /// <summary>
        /// 	<para>${REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_1}</para>
        /// 	<para><img style="WIDTH: 136px; HEIGHT: 83px" height="90" src="fillGradientAngleLinear0.bmp" width="159"/></para>
        /// 	<para>${REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_2}</para>
        /// 	<para><img style="WIDTH: 139px; HEIGHT: 81px" height="90" src="fillGradientAngleLinear180.bmp" width="159"/></para>
        /// 	<para>${REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_3}</para>
        /// 	<para><img style="WIDTH: 142px; HEIGHT: 82px" height="82" src="fillGradientAngleLinear90.bmp" width="145"/></para>
        /// 	<para>${REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_4}</para>
        /// 	<para><img style="WIDTH: 144px; HEIGHT: 89px" height="91" src="fillGradientAngleLinear270.bmp" width="158"/></para>
        /// 	<para>${REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_5}</para>
        /// 	<para><img src="fillGradientAngleConical0.bmp"/><img src="fillGradientAngleConical90.bmp"/></para>
        /// 	<para>${REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_6}</para>
        /// </summary>
        public double FillGradientAngle { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_LineSymbolID_D}</summary>
        public int LineSymbolID { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_LineWidth_D}</summary>
        public double LineWidth { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_LineColor_D}</summary>
        public Color LineColor { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_MarkerAngle_D}</summary>
        public double MarkerAngle { get; set; }
        /// <summary>${REST_ServerType_ServerStyle_attribute_MarkerSize_D}</summary>
        public double MarkerSize
        {
            get;
            set;
        }

        /// <summary>${REST_ServerType_ServerStyle_attribute_MarkerSymbolID_D}</summary>
        public int MarkerSymbolID { get; set; }


        internal static string ToJson(ServerStyle result)
        {
            if (result == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillBackOpaque\":{0}", result.FillBackOpaque.ToString().ToLower()));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"lineWidth\":{0}", result.LineWidth.ToString(CultureInfo.InvariantCulture)));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillBackColor\":{0}", ServerColor.ToJson(result.FillBackColor.ToServerColor())));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillForeColor\":{0}", ServerColor.ToJson(result.FillForeColor.ToServerColor())));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"markerAngle\":{0}", result.MarkerAngle.ToString(CultureInfo.InvariantCulture)));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"markerSize\":{0}", result.MarkerSize.ToString(CultureInfo.InvariantCulture)));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillGradientOffsetRatioX\":{0}", result.FillGradientOffsetRatioX.ToString(CultureInfo.InvariantCulture)));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillGradientOffsetRatioY\":{0}", result.FillGradientOffsetRatioY.ToString(CultureInfo.InvariantCulture)));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"lineColor\":{0}", ServerColor.ToJson(result.LineColor.ToServerColor())));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillOpaqueRate\":{0}", result.FillOpaqueRate));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillGradientMode\":\"{0}\"", result.FillGradientMode));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillSymbolID\":{0}", result.FillSymbolID));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"fillGradientAngle\":{0}", result.FillGradientAngle.ToString(CultureInfo.InvariantCulture)));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"markerSymbolID\":{0}", result.MarkerSymbolID));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"lineSymbolID\":{0}", result.LineSymbolID));


            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ServerStyle FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            return new ServerStyle
            {
                FillBackOpaque = json["fillBackOpaque"].GetBooleanEx(),
                LineWidth = json["lineWidth"].GetNumberEx(),
                FillBackColor = ServerColor.FromJson(json["fillBackColor"].GetObjectEx()).ToColor(),
                FillForeColor = ServerColor.FromJson(json["fillForeColor"].GetObjectEx()).ToColor(),
                MarkerAngle = json["markerAngle"].GetNumberEx(),
                MarkerSize = json["markerSize"].GetNumberEx(),
                FillGradientOffsetRatioX = json["fillGradientOffsetRatioX"].GetNumberEx(),
                FillGradientOffsetRatioY = json["fillGradientOffsetRatioY"].GetNumberEx(),
                LineColor = ServerColor.FromJson(json["lineColor"].GetObjectEx()).ToColor(),
                FillOpaqueRate = (int)json["fillOpaqueRate"].GetNumberEx(),
                FillGradientMode = (FillGradientMode)Enum.Parse(typeof(FillGradientMode), json["fillGradientMode"].GetStringEx(), true),
                FillSymbolID = (int)json["fillSymbolID"].GetNumberEx(),
                FillGradientAngle = json["fillGradientAngle"].GetNumberEx(),
                MarkerSymbolID = (int)json["markerSymbolID"].GetNumberEx(),
                LineSymbolID = (int)json["lineSymbolID"].GetNumberEx()
            };
        }
    }
}
