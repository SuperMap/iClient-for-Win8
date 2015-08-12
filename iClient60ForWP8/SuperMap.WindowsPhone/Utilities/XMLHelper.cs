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
using System.Xml.Linq;
using SuperMap.WindowsPhone.Core;
using System.Globalization;
using System.Collections.Generic;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary><para>${WP_utility_XMLHelper_Title}</para></summary>
    public static class XMLHelper
    {
        private static string gml = "http://www.opengis.net/gml";
        /// <summary>${WP_utility_XMLHelper_method_FromGeometry_D}</summary>
        public static XElement FromGeometry(SuperMap.WindowsPhone.Core.Geometry geo)
        {
            if (geo == null)
            {
                return null;
            }

            XElement xe = null;
            if (geo is GeoPoint)
            {
                xe = new XElement("{" + gml + "}Point");
                XElement coordinates = new XElement("{" + gml + "}coordinates", XMLHelper.GetCoorStr((geo as GeoPoint).Location));
                xe.Add(coordinates);
            }
            else if (geo is GeoLine)
            {
                //xe = new XElement(XName.Get("MultiLineString", gml), new XAttribute("{" + defaultNS + "}gml", gml));
                xe = new XElement("{" + gml + "}MultiLineString");
                foreach (Point2DCollection item in (geo as GeoLine).Parts)
                {
                    XElement coordinates = new XElement("{" + gml + "}coordinates", XMLHelper.GetCoorStr(item));
                    XElement LineString = new XElement("{" + gml + "}LineString", coordinates);
                    xe.Add(new XElement("{" + gml + "}lineStringMember", LineString));
                }
            }
            else if (geo is GeoRegion)
            {
                xe = new XElement("{" + gml + "}MultiPolygon");
                foreach (Point2DCollection item in (geo as GeoRegion).Parts)
                {
                    XElement coordinates = new XElement("{" + gml + "}coordinates", XMLHelper.GetCoorStr(item));
                    XElement linearRing = new XElement("{" + gml + "}LinearRing", coordinates);
                    XElement outerBoundaryIs = new XElement("{" + gml + "}outerBoundaryIs", linearRing);
                    XElement polygon = new XElement("{" + gml + "}Polygon", outerBoundaryIs);
                    xe.Add(new XElement("{" + gml + "}polygonMember", polygon));
                }
            }
            else
            { }
            return xe;
        }

        //预留接口
        private static GeoPoint ToGeoPoint(XElement xe)
        {
            GeoPoint point = new GeoPoint();
            return point;
        }

        //预留接口
        private static GeoLine ToGeoLine(XElement xe)
        {
            GeoLine line = new GeoLine();
            return line;
        }

        //预留接口
        private static GeoRegion ToGeoRegion(XElement xe)
        {
            GeoRegion region = new GeoRegion();
            return region;
        }

        private static string GetCoorStr(Point2D p)
        {
            string str = string.Empty;
            str = p.ToString(CultureInfo.InvariantCulture);
            return str;
        }

        private static string GetCoorStr(Point2DCollection ps)
        {
            string str = string.Empty;
            List<string> strCol = new List<string>();
            if (ps != null)
            {
                foreach (Point2D item in ps)
                {
                    strCol.Add(item.ToString(CultureInfo.InvariantCulture));
                }

                str = string.Join(" ", strCol);
            }

            return str;
        }
    }
}
