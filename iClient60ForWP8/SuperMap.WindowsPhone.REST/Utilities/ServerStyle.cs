using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_ServerStyle_Tile}</para>
    /// 	<para>${WP_REST_ServerType_ServerStyle_Description}</para>
    /// </summary>
    public class ServerStyle
    {
        /// <summary>${WP_REST_ServerType_ServerStyle_constructor_None_D}</summary>
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
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillBackOpaque_D}</summary>
        [JsonProperty("fillBackOpaque")]
        public bool FillBackOpaque { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_fillBackColor_D}</summary>
        [JsonProperty("fillBackColor")]
        public Color FillBackColor { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillForeColor_D}</summary>
        [JsonProperty("fillForeColor")]
        public Color FillForeColor { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillGradientOffsetRatioX_D}</summary>
        [JsonProperty("fillGradientOffsetRatioX")]
        public double FillGradientOffsetRatioX { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillGradientOffsetRatioY_D}</summary>
        [JsonProperty("fillGradientOffsetRatioY")]
        public double FillGradientOffsetRatioY { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillOpaqueRate_D}</summary>
        [JsonProperty("fillOpaqueRate")]
        public int FillOpaqueRate { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillGradientMode_D}</summary>
        [JsonProperty("fillGradientMode")]
        public FillGradientMode FillGradientMode { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_FillSymbolID_D}</summary>
        [JsonProperty("fillSymbolID")]
        public int FillSymbolID { get; set; }
        /// <summary>
        /// 	<para>${WP_REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_1}</para>
        /// 	<para><img style="WIDTH: 136px; HEIGHT: 83px" height="90" src="fillGradientAngleLinear0.bmp" width="159"/></para>
        /// 	<para>${WP_REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_2}</para>
        /// 	<para><img style="WIDTH: 139px; HEIGHT: 81px" height="90" src="fillGradientAngleLinear180.bmp" width="159"/></para>
        /// 	<para>${WP_REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_3}</para>
        /// 	<para><img style="WIDTH: 142px; HEIGHT: 82px" height="82" src="fillGradientAngleLinear90.bmp" width="145"/></para>
        /// 	<para>${WP_REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_4}</para>
        /// 	<para><img style="WIDTH: 144px; HEIGHT: 89px" height="91" src="fillGradientAngleLinear270.bmp" width="158"/></para>
        /// 	<para>${WP_REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_5}</para>
        /// 	<para><img src="fillGradientAngleConical0.bmp"/><img src="fillGradientAngleConical90.bmp"/></para>
        /// 	<para>${WP_REST_ServerType_ServerStyle_attribute_FillGradientAngle_D_6}</para>
        /// </summary>
        [JsonProperty("fillGradientAngle")]
        public double FillGradientAngle { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_LineSymbolID_D}</summary>
        [JsonProperty("lineSymbolID")]
        public int LineSymbolID { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_LineWidth_D}</summary>
        [JsonProperty("lineWidth")]
        public double LineWidth { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_LineColor_D}</summary>
        [JsonProperty("lineColor")]
        public Color LineColor { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_MarkerAngle_D}</summary>
        [JsonProperty("markerAngle")]
        public double MarkerAngle { get; set; }
        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_MarkerSize_D}</summary>
        [JsonProperty("markerSize")]
        public double MarkerSize
        {
            get;
            set;
        }

        /// <summary>${WP_REST_ServerType_ServerStyle_attribute_MarkerSymbolID_D}</summary>
        [JsonProperty("markerSymbolID")]
        public int MarkerSymbolID { get; set; }

    }
}
