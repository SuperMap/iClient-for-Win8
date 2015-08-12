using SuperMap.WinRT.Core;
using System.Collections.Generic;
using System;
using SuperMap.WinRT.Utilities;
using System.Globalization;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ServerType_ServerGeometry_Tile}</para>
    /// 	<para>${REST_ServerType_ServerGeometry_Description}</para>
    /// </summary>
    public class ServerGeometry
    {
        internal ServerGeometry()
        { }

        /// <summary>${REST_ServerType_ServerGeometry_attribute_id_D}</summary>
        public int ID { get; set; }
        /// <summary>${REST_ServerType_ServerGeometry_attribute_parts_D}</summary>
        public IList<int> Parts { get; set; }
        /// <summary>${REST_ServerType_ServerGeometry_attribute_Points_D}</summary>
        public Point2DCollection Points { get; set; }
        /// <summary>${REST_ServerType_ServerGeometry_attribute_Style_D}</summary>
        public ServerStyle Style { get; set; }
        /// <summary>${REST_ServerType_ServerGeometry_attribute_type_D}</summary>
        public ServerGeometryType Type { get; set; }

        internal static string ToJson(ServerGeometry serverGeometry)
        {
            if (serverGeometry == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format(CultureInfo.InvariantCulture, "\"type\":\"{0}\"", serverGeometry.Type));
            list.Add(string.Format(CultureInfo.InvariantCulture, "\"id\":{0}", serverGeometry.ID));

            if (serverGeometry.Parts != null && serverGeometry.Parts.Count > 0)
            {
                List<string> parts = new List<string>();
                foreach (int i in serverGeometry.Parts)
                {
                    parts.Add(i.ToString(CultureInfo.InvariantCulture));
                }
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"parts\":[{0}]", string.Join(",", parts.ToArray())));
            }
            else
            {
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"parts\":null"));
            }
            if (serverGeometry.Points != null && serverGeometry.Points.Count > 0)
            {
                List<string> ps = new List<string>();
                foreach (Point2D p in serverGeometry.Points)
                {
                    ps.Add(JsonHelper.FromPoint2D(p));
                }
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"points\":[{0}]", string.Join(",", ps.ToArray())));
            }
            else
            {
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"points\":null"));
            }

            if (serverGeometry.Style != null)
            {
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"style\":{0}", ServerStyle.ToJson(serverGeometry.Style)));
            }
            else
            {
                list.Add(string.Format(CultureInfo.InvariantCulture, "\"style\":null"));
            }

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }
        /// <summary>${REST_ServerType_method_FromJson_D}</summary>
        /// <returns>${REST_ServerType_method_FromJson_return}</returns>
        /// <param name="json">${REST_ServerType_method_FromJson_param_jsonObject}</param>
        internal static ServerGeometry FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            ServerGeometry geometry = new ServerGeometry();
            ServerGeometryType type;
            bool isSuccess = Enum.TryParse<ServerGeometryType>(json["type"].GetStringEx(), true, out type);

            if (isSuccess)
            {
                geometry.Type = type;
            }
            else
            {
                return geometry;
            }
            geometry.ID = (int)json["id"].GetNumberEx();
            geometry.Style = ServerStyle.FromJson(json["style"].GetObjectEx());

            JsonArray parts = json["parts"].GetArray();
            if (parts != null && parts.Count > 0)
            {
                geometry.Parts = new List<int>();
                for (int i = 0; i < parts.Count; i++)
                {
                    geometry.Parts.Add((int)parts[i].GetNumberEx());
                }
            }

            JsonArray point2Ds = json["points"].GetArray();
            if (point2Ds != null && point2Ds.Count > 0)
            {
                geometry.Points = new Point2DCollection();
                for (int i = 0; i < point2Ds.Count; i++)
                {
                    geometry.Points.Add(JsonHelper.ToPoint2D(point2Ds[i].GetObjectEx()));
                }
            }
            return geometry;
        }

        internal GeoPoint ToGeoPoint()
        {
            if (Points != null && Type == ServerGeometryType.POINT)
            {
                return new GeoPoint(Points[0].X, Points[0].Y);
            }
            return null;
        }

        internal GeoLine ToGeoLine()
        {
            if (this.Parts != null)
            {
                List<Point2DCollection> pss = new List<Point2DCollection>();
                Point2DCollection copy = new Point2DCollection();
                foreach (Point2D item in this.Points)
                {
                    copy.Add(item);
                }
                for (int i = 0; i < this.Parts.Count; i++)
                {
                    Point2DCollection temp = new Point2DCollection();
                    for (int j = 0; j < this.Parts[i]; j++)
                    {
                        temp.Add(copy[j]);
                    }
                    pss.Add(temp);

                    copy.RemoveRange(0, this.Parts[i]);
                }

                GeoLine line = new GeoLine();
                foreach (Point2DCollection item in pss)
                {
                    line.Parts.Add(item);
                }
                return line;
            }
            return null;
        }

        internal GeoRegion ToGeoRegion()
        {
            if (this.Parts != null)
            {
                List<Point2DCollection> pss = new List<Point2DCollection>();
                Point2DCollection copy = new Point2DCollection();
                foreach (Point2D item in this.Points)
                {
                    copy.Add(item);
                }
                for (int i = 0; i < this.Parts.Count; i++)
                {
                    Point2DCollection temp = new Point2DCollection();
                    for (int j = 0; j < this.Parts[i]; j++)
                    {
                        temp.Add(copy[j]);
                    }
                    pss.Add(temp);
                    copy.RemoveRange(0, this.Parts[i]);
                }

                GeoRegion region = new GeoRegion();
                foreach (Point2DCollection item in pss)
                {
                    region.Parts.Add(item);
                }
                return region;
            }
            return null;
        }
    }
}
