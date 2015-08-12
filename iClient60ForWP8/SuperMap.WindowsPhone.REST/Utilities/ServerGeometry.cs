using System.Collections.Generic;
using System;
using System.Globalization;
using Newtonsoft.Json;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_ServerGeometry_Tile}</para>
    /// 	<para>${WP_REST_ServerType_ServerGeometry_Description}</para>
    /// </summary>
    public class ServerGeometry
    {
        internal ServerGeometry()
        { }

        /// <summary>${WP_REST_ServerType_ServerGeometry_attribute_id_D}</summary>
        [JsonProperty("id")]
        public int ID { get; set; }
        /// <summary>${WP_REST_ServerType_ServerGeometry_attribute_parts_D}</summary>
        [JsonProperty("parts")]
        public IList<int> Parts { get; set; }
        /// <summary>${WP_REST_ServerType_ServerGeometry_attribute_Points_D}</summary>
        [JsonProperty("points")]
        [JsonConverter(typeof(SuperMap.WindowsPhone.REST.Help.Point2DCollectionConverter))]
        public List<Point2D> Points { get; set; }
        /// <summary>${WP_REST_ServerType_ServerGeometry_attribute_Style_D}</summary>
        [JsonProperty("style")]
        public ServerStyle Style { get; set; }
        /// <summary>${WP_REST_ServerType_ServerGeometry_attribute_type_D}</summary>
        [JsonProperty("type")]
        public ServerGeometryType Type { get; set; }

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
