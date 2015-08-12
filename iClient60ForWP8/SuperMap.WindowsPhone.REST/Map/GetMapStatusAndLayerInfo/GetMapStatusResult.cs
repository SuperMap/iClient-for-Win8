using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_GetMapStatusResult_Title}</para>
    /// 	<para>${WP_REST_GetMapStatusResult_Description}</para>
    /// </summary>
    public class GetMapStatusResult
    {
        /// <summary>${WP_REST_GetMapStatusResult_constructor_D}</summary>
        internal GetMapStatusResult()
        {
            
        }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public String Name { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Center_D}</summary>
        [JsonProperty("center")]
        [JsonConverter(typeof(Point2DConverter))]
        public Point2D Center { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Scale_D}</summary>
        [JsonProperty("scale")]
        public double Scale { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_MaxScale_D}</summary>
        [JsonProperty("maxScale")]
        public double MaxScale { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_MinScale_D}</summary>
        [JsonProperty("minScale")]
        public double MinScale { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Bounds_D}</summary>
        [JsonProperty("bounds")]
        [JsonConverter(typeof(Rectangle2DConverter))]
        public Rectangle2D Bounds { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_ViewBounds_D}</summary>
        [JsonProperty("viewBounds")]
        [JsonConverter(typeof(Rectangle2DConverter))]
        public Rectangle2D ViewBounds { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Viewer_D}</summary>
        [JsonProperty("viewer")]
        [JsonConverter(typeof(RectConverter))]
        public Rect Viewer { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_PrjCoordSys_D}</summary>
        [JsonProperty("prjCoordSys")]
        public PrjCoordSys PrjCoordSys { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_CustomParams_D}</summary>
        [JsonProperty("customParams")]
        public String CustomParams { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_UserToken_D}</summary>
        [JsonProperty("userToken")]
        public UserInfo UserToken { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_ClipRegion_D}</summary>
        [JsonProperty("clipRegion")]
        public ServerGeometry ClipRegion { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsClipRegionEnabled_D}</summary>
        [JsonProperty("clipRegionEnabled")]
        public bool IsClipRegionEnabled { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_CustomEntireBounds_D}</summary>
        [JsonProperty("customEntireBounds")]
        [JsonConverter(typeof(Rectangle2DConverter))]
        public Rectangle2D CustomEntireBounds { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsCustomEntireBoundsEnabled_D}</summary>
        [JsonProperty("customEntireBoundsEnabled")]
        public bool IsCustomEntireBoundsEnabled { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Angle_D}</summary>
        [JsonProperty("angle")]
        public double Angle { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Antialias_D}</summary>
        [JsonProperty("antialias")]
        public bool Antialias { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_BackgroundStyle_D}</summary>
        [JsonProperty("backgroundStyle")]
        public ServerStyle BackgroundStyle { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_ColorMode_D}</summary>
        [JsonProperty("colorMode")]
        public MapColorMode ColorMode { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_CoordUnit_D}</summary>
        [JsonProperty("coordUnit")]
        public Unit CoordUnit { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_DistanceUnit_D}</summary>
        [JsonProperty("distanceUnit")]
        public Unit DistanceUnit { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_Description_D}</summary>
        [JsonProperty("description")]
        public String Description { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsDynamicProjection_D}</summary>
        [JsonProperty("dynamicProjection")]
        public bool IsDynamicProjection { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsMarkerAngleFixed_D}</summary>
        [JsonProperty("markerAngleFixed")]
        public bool IsMarkerAngleFixed { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_MaxVisibleTextSize_D}</summary>
        [JsonProperty("maxVisibleTextSize")]
        public double MaxVisibleTextSize { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_MaxVisibleVertex_D}</summary>
        [JsonProperty("maxVisibleVertex")]
        public int MaxVisibleVertex { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_MinVisibleTextSize_D}</summary>
        [JsonProperty("minVisibleTextSize")]
        public double MinVisibleTextSize { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsOverlapDisplayed_D}</summary>
        [JsonProperty("overlapDisplayed")]
        public bool IsOverlapDisplayed { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsPaintBackground_D}</summary>
        [JsonProperty("paintBackground")]
        public bool IsPaintBackground { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsTextAngleFixed_D}</summary>
        [JsonProperty("textAngleFixed")]
        public bool IsTextAngleFixed { get; internal set; }
        /// <summary>${WP_REST_GetMapStatusResult_attribute_IsTextOrientationFixed_D}</summary>
        [JsonProperty("textOrientationFixed")]
        public bool IsTextOrientationFixed { get; internal set; }

    }
}
