using System.Collections.Generic;
using SuperMap.WinRT.Core;
using System.Collections;
using Windows.UI;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Bridge_Title}</para>
    /// 	<para>${REST_Bridge_Description}</para>
    /// </summary>
    public static class Bridge
    {

        /// <summary>${REST_Bridge_method_GeometryToServerGeometry_D}</summary>
        /// <returns>${REST_Bridge_method_GeometryToServerGeometry_return}</returns>
        /// <param name="geo">${REST_Bridge_method_GeometryToServerGeometry_param_geo}</param>
        public static ServerGeometry ToServerGeometry(this SuperMap.WinRT.Core.Geometry geo)
        {
            if (geo == null)
            {
                return null;
            }

            ServerGeometry sg = new ServerGeometry();

            Point2DCollection list = new Point2DCollection();
            List<int> parts = new List<int>();

            if (geo is GeoRegion)
            {
                for (int i = 0; i < ((GeoRegion)geo).Parts.Count; i++)
                {
                    for (int j = 0; j < ((GeoRegion)geo).Parts[i].Count; j++)
                    {
                        list.Add(new Point2D(((GeoRegion)geo).Parts[i][j].X, ((GeoRegion)geo).Parts[i][j].Y));
                    }
                    parts.Add(((GeoRegion)geo).Parts[i].Count);
                }
                sg.Type = ServerGeometryType.REGION;
            }

            if (geo is GeoCircle)
            {
                for (int i = 0; i < ((GeoCircle)geo).Parts.Count; i++)
                {
                    for (int j = 0; j < ((GeoCircle)geo).Parts[i].Count; j++)
                    {
                        list.Add(new Point2D(((GeoCircle)geo).Parts[i][j].X, ((GeoCircle)geo).Parts[i][j].Y));
                    }
                    parts.Add(((GeoCircle)geo).Parts[i].Count);
                }
                sg.Type = ServerGeometryType.REGION;
            }

            if (geo is GeoLine)
            {
                for (int i = 0; i < ((GeoLine)geo).Parts.Count; i++)
                {
                    for (int j = 0; j < ((GeoLine)geo).Parts[i].Count; j++)
                    {
                        list.Add(new Point2D(((GeoLine)geo).Parts[i][j].X, ((GeoLine)geo).Parts[i][j].Y));
                    }
                    parts.Add(((GeoLine)geo).Parts[i].Count);
                }
                sg.Type = ServerGeometryType.LINE;
            }

            if (geo is GeoPoint)
            {
                list.Add(new Point2D(((GeoPoint)geo).X, ((GeoPoint)geo).Y));
                parts.Add(list.Count);
                sg.Type = ServerGeometryType.POINT;
            }

            sg.Points = list;
            sg.Parts = parts;
            sg.ID = -1;
            return sg;
        }

        /// <summary>${REST_Bridge_method_ServerGeometryToGeometry_D}</summary>
        /// <returns>${REST_Bridge_method_ServerGeometryToGeometry_return}</returns>
        /// <param name="geo">${REST_Bridge_method_ServerGeometryToGeometry_param_geo}</param>
        public static SuperMap.WinRT.Core.Geometry ToGeometry(this ServerGeometry geo)
        {
            if (geo != null)
            {
                if (geo.Type == ServerGeometryType.POINT)
                {
                    return geo.ToGeoPoint();
                }
                else if (geo.Type == ServerGeometryType.REGION)
                {
                    return geo.ToGeoRegion();
                }
                else if (geo.Type == ServerGeometryType.LINE || geo.Type == ServerGeometryType.LINEM)
                {
                    return geo.ToGeoLine();
                }
            }
            return null;
        }

        /// <summary>${REST_Bridge_method_ColorToServerColor_D}</summary>
        /// <returns>${REST_Bridge_method_ColorToServerColor_return}</returns>
        /// <param name="color">${REST_Bridge_method_ColorToServerColor_param_color}</param>
        public static ServerColor ToServerColor(this Color color)
        {
            if (color != null)
            {
                return new ServerColor(color.R, color.G, color.B);
            }
            return null;
        }

        /// <summary>${REST_Bridge_method_ServerColorToColor_D}</summary>
        /// <returns>${REST_Bridge_method_ServerColorToColor_return}</returns>
        /// <param name="color">${REST_Bridge_method_ServerColorToColor_param_color}</param>
        public static Color ToColor(this ServerColor color)
        {
            if (color != null)
            {
                return new Color() { R = (byte)color.Red, G = (byte)color.Green, B = (byte)color.Blue, A = 255 };
            }
            //如果传入的ServerColor是null，则返回透明；
            return Color.FromArgb(0, 0, 0, 0);
        }

        //public static ServerStyle StyleToServerStyle(SuperMap.WinRT.Core.Style Style)
        //{
        //    return null;
        //}

        /// <summary>${REST_Bridge_method_ToFeature}</summary>
        /// <param name="feature">${REST_Bridge_method_ToFeature_param_feature}</param>
        /// <returns>${REST_Bridge_method_ToFeature_return}</returns>
        public static Feature ToFeature(this ServerFeature feature)
        {
            if (feature != null)
            {
                Feature f = new Feature();
                if (feature.Geometry != null)
                {
                    f.Geometry = feature.Geometry.ToGeometry();
                }
                if (feature.FieldNames != null && feature.FieldNames.Count > 0 && feature.FieldValues != null && feature.FieldValues.Count > 0)
                {

                    for (int i = 0; i < feature.FieldNames.Count; i++)
                    {
                        f.Attributes.Add(feature.FieldNames[i].ToString(), feature.FieldValues[i]);
                    }
                }
                return f;
            }
            return null;
        }

        /// <summary>${REST_Bridge_method_ToServerFeature}</summary>
        /// <param name="feature">${REST_Bridge_method_ToServerFeature_param_feature}</param>
        /// <returns>${REST_Bridge_method_ToServerFeature_return}</returns>
        public static ServerFeature ToServerFeature(this Feature feature)
        {
            if (feature != null)
            {
                ServerFeature f = new ServerFeature();
                f.FieldNames = new List<string>();
                f.FieldValues = new List<string>();

                if (feature.Geometry != null)
                {
                    f.Geometry = feature.Geometry.ToServerGeometry();
                }
                if (feature.Attributes != null && feature.Attributes.Count > 0)
                {
                    foreach (KeyValuePair<string, object> kv in feature.Attributes)
                    {
                        f.FieldNames.Add(kv.Key);

                        f.FieldValues.Add(kv.Value != null ? kv.Value.ToString() : "null");
                    }
                }
                return f;
            }
            return null;
        }
        /// <summary>${REST_Bridge_method_ToDictionary_D}</summary>
        /// <param name="style">${REST_Bridge_method_ToDictionary_param_style}</param>
        /// <returns>${REST_Bridge_method_ToDictionary_return}</returns>
        internal static Dictionary<string, string> ToDictionary(this ServerStyle style)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("fillBackOpaque", style.FillBackOpaque.ToString().ToLower());
            dict.Add("lineWidth", style.LineWidth.ToString());
            dict.Add("fillBackColor", ServerColor.ToJson(style.FillBackColor.ToServerColor()));
            dict.Add("fillForeColor", ServerColor.ToJson(style.FillForeColor.ToServerColor()));
            dict.Add("markerAngle", style.MarkerAngle.ToString());
            dict.Add("markerSize", style.MarkerSize.ToString());
            dict.Add("fillGradientOffsetRatioX", style.FillGradientOffsetRatioX.ToString());
            dict.Add("fillGradientOffsetRatioY", style.FillGradientOffsetRatioY.ToString());
            dict.Add("lineColor", ServerColor.ToJson(style.LineColor.ToServerColor()));
            dict.Add("fillOpaqueRate", style.FillOpaqueRate.ToString());
            dict.Add("fillGradientMode", "\"" + style.FillGradientMode.ToString() + "\"");
            dict.Add("fillSymbolID", style.FillSymbolID.ToString());
            dict.Add("fillGradientAngle", style.FillGradientAngle.ToString());
            dict.Add("markerSymbolID", style.MarkerSymbolID.ToString());
            dict.Add("lineSymbolID", style.LineSymbolID.ToString());
            return dict;
        }
    }
}
