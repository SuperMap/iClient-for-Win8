using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
using Windows.Foundation;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_GetMapStatusResult_Title}</para>
    /// 	<para>${REST_GetMapStatusResult_Description}</para>
    /// </summary>
    public class GetMapStatusResult
    {
        /// <summary>${REST_GetMapStatusResult_constructor_D}</summary>
        internal GetMapStatusResult()
        {
            this.VisibleScales = new List<double>();
        }
        /// <summary>${REST_GetMapStatusResult_attribute_Name_D}</summary>
        public String Name { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Center_D}</summary>
        public Point2D Center { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Scale_D}</summary>
        public double Scale { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_MaxScale_D}</summary>
        public double MaxScale { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_MinScale_D}</summary>
        public double MinScale { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_ViewBounds_D}</summary>
        public Rectangle2D ViewBounds { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Viewer_D}</summary>
        public Rect Viewer { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_PrjCoordSys_D}</summary>
        public PrjCoordSys PrjCoordSys { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsCacheEnabled_D}</summary>
        public bool IsCacheEnabled { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_CustomParams_D}</summary>
        public String CustomParams { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_UserToken_D}</summary>
        public UserInfo UserToken { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_ClipRegion_D}</summary>
        public ServerGeometry ClipRegion { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsClipRegionEnabled_D}</summary>
        public bool IsClipRegionEnabled { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_CustomEntireBounds_D}</summary>
        public Rectangle2D CustomEntireBounds { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsCustomEntireBoundsEnabled_D}</summary>
        public bool IsCustomEntireBoundsEnabled { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Angle_D}</summary>
        public double Angle { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Antialias_D}</summary>
        public bool Antialias { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_BackgroundStyle_D}</summary>
        public ServerStyle BackgroundStyle { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_ColorMode_D}</summary>
        public MapColorMode ColorMode { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_CoordUnit_D}</summary>
        public Unit CoordUnit { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_DistanceUnit_D}</summary>
        public Unit DistanceUnit { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_Description_D}</summary>
        public String Description { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsDynamicProjection_D}</summary>
        public bool IsDynamicProjection { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsMarkerAngleFixed_D}</summary>
        public bool IsMarkerAngleFixed { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_MaxVisibleTextSize_D}</summary>
        public double MaxVisibleTextSize { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_MaxVisibleVertex_D}</summary>
        public int MaxVisibleVertex { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_MinVisibleTextSize_D}</summary>
        public double MinVisibleTextSize { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsOverlapDisplayed_D}</summary>
        public bool IsOverlapDisplayed { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsPaintBackground_D}</summary>
        public bool IsPaintBackground { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsTextAngleFixed_D}</summary>
        public bool IsTextAngleFixed { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_IsTextOrientationFixed_D}</summary>
        public bool IsTextOrientationFixed { get; internal set; }
        /// <summary>${REST_GetMapStatusResult_attribute_VisibleScales_D}</summary>
        public List<double> VisibleScales { get; private set; }
        /// <summary>${REST_GetMapStatusResult_attribute_VisibleScalesEnabled_D}</summary>
        public bool VisibleScalesEnabled { get; private set; }

        internal static GetMapStatusResult FromJson(JsonObject json)
        {
            if (json == null) return null;
            GetMapStatusResult result = new GetMapStatusResult();
            result.Angle = json["angle"].GetNumberEx();
            result.Antialias = json["antialias"].GetBooleanEx();
            result.BackgroundStyle = ServerStyle.FromJson(json["backgroundStyle"].GetObjectEx());
            result.Bounds = JsonHelper.ToRectangle2D(json["bounds"].GetObjectEx());
            result.IsCacheEnabled = json["cacheEnabled"].GetBooleanEx();
            result.Center = JsonHelper.ToPoint2D(json["center"].GetObjectEx());
            result.ClipRegion = ServerGeometry.FromJson(json["clipRegion"].GetObjectEx());
            result.IsClipRegionEnabled = json["clipRegionEnabled"].GetBooleanEx();
            if (json["colorMode"].ValueType != JsonValueType.Null)
            {
                result.ColorMode = (MapColorMode)Enum.Parse(typeof(MapColorMode), json["colorMode"].GetStringEx(), true);
            }
            if (json["coordUnit"].ValueType != JsonValueType.Null)
            {
                result.CoordUnit = (Unit)Enum.Parse(typeof(Unit), json["coordUnit"].GetStringEx(), true);
            }
            result.CustomEntireBounds = JsonHelper.ToRectangle2D(json["customEntireBounds"].GetObjectEx());
            result.IsCustomEntireBoundsEnabled = json["customEntireBoundsEnabled"].GetBooleanEx();
            result.CustomParams = json["customParams"].GetStringEx();
            result.Description = json["description"].GetStringEx();
            if (json["distanceUnit"].ValueType != JsonValueType.Null)
            {
                result.DistanceUnit = (Unit)Enum.Parse(typeof(Unit), json["distanceUnit"].GetStringEx(), true);
            }
            result.IsDynamicProjection = json["dynamicProjection"].GetBooleanEx();
            result.IsMarkerAngleFixed = json["markerAngleFixed"].GetBooleanEx();
            result.MaxScale = json["maxScale"].GetNumberEx();
            result.MaxVisibleTextSize = json["maxVisibleTextSize"].GetNumberEx();
            result.MaxVisibleVertex = (int)json["maxVisibleVertex"].GetNumberEx();
            result.MinScale = json["minScale"].GetNumberEx();
            result.MinVisibleTextSize = json["minVisibleTextSize"].GetNumberEx();
            result.Name = json["name"].GetStringEx();
            result.IsOverlapDisplayed = json["overlapDisplayed"].GetBooleanEx();
            result.IsPaintBackground = json["paintBackground"].GetBooleanEx();
            result.PrjCoordSys = PrjCoordSys.FromJson(json["prjCoordSys"].GetObjectEx());
            result.Scale = json["scale"].GetNumberEx();
            result.IsTextAngleFixed = json["textAngleFixed"].GetBooleanEx();
            result.IsTextOrientationFixed = json["textOrientationFixed"].GetBooleanEx();
            result.UserToken = UserInfo.FromJson(json["userToken"].GetObjectEx());
            result.ViewBounds = JsonHelper.ToRectangle2D(json["viewBounds"].GetObjectEx());
            result.Viewer = JsonHelper.ToRect(json["viewer"].GetObjectEx());

            result.VisibleScalesEnabled = (json["visibleScalesEnabled"] != null ? json["visibleScalesEnabled"].GetBooleanEx() : false);
            if (result.VisibleScalesEnabled && json["visibleScales"] != null)
            {
                try
                {
                    foreach (var item in json["visibleScales"].GetObjectEx())
                    {
                        result.VisibleScales.Add(item.Value.GetNumber());
                    }
                }
                catch (Exception)
                {
                    result.VisibleScales.Clear();
                    result.VisibleScalesEnabled = false;
                }
            }
            return result;
        }
    }
}
