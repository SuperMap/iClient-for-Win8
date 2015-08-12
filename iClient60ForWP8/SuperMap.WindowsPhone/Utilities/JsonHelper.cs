using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Json;
using System.Text;
using System.Windows;
using SuperMap.Web.Core;

namespace SuperMap.Web.Utilities
{
    /// <summary><para>${utility_JsonHelper_Title}</para></summary>
    public static class JsonHelper
    {
        /// <summary>${utility_JsonHelper_method_FromRectangle2D_D}</summary>
        public static string FromRectangle2D(Rectangle2D rectangle2D)
        {
            if (rectangle2D.IsEmpty)
            {
                return "{}";
            }
            return string.Format(CultureInfo.InvariantCulture, "{{\"rightTop\":{{\"y\":{0},\"x\":{1}}},\"leftBottom\":{{\"y\":{2},\"x\":{3}}}}}", rectangle2D.Top, rectangle2D.Right, rectangle2D.Bottom, rectangle2D.Left);
        }

        /// <summary>${utility_JsonHelper_method_FromPoint2D_D}</summary>
        public static string FromPoint2D(Point2D point)
        {
            if (point.IsEmpty)
            {
                return "{}"; //string.Empty? null?
            }
            return string.Format(CultureInfo.InvariantCulture, "{{\"x\":{0},\"y\":{1}}}", point.X, point.Y);
        }

        public static string FromPoint2DWithTag(Point2D point,string tagName,bool isNumber)
        {
            if (point.IsEmpty)
            {
                return "{}"; //string.Empty? null?
            }
            string format;
            if (isNumber)
            {
                format = "{{\"x\":{0},\"y\":{1},\"{2}\":{3}}}";
            }
            else
            {
                format = "{{\"x\":{0},\"y\":{1},\"{2}\":\"{3}\"}}";
            }
            return string.Format(CultureInfo.InvariantCulture, format, point.X, point.Y,tagName,point.Tag.ToStringEx());
        }

        /// <summary>${utility_JsonHelper_method_FromGeoPoint_D}</summary>
        public static string FromGeoPoint(GeoPoint point)
        {
            if (point == null)
            {
                return "{}";
            }
            return string.Format(CultureInfo.InvariantCulture, "{{\"x\":{0},\"y\":{1}}}", point.X, point.Y);
        }

        /// <summary>${utility_JsonHelper_method_FromPoint2DCollection_D}</summary>
        public static string FromPoint2DCollection(Point2DCollection points)
        {
            if (points == null || points.Count < 1)
            {
                return "[{}]";
            }
            StringBuilder stringBuilder = new StringBuilder("[");

            for (int i = 0; i < points.Count - 1; i++)
            {
                stringBuilder.Append(FromPoint2D(points[i]));
                stringBuilder.Append(",");
            }
            stringBuilder.Append(FromPoint2D(points[points.Count - 1]));
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        /// <summary>${utility_JsonHelper_method_FromIList_D}</summary>
        public static string FromIList(IList param)
        {
            if (param.Count < 1 || param == null)
                return "[]";

            string json = "[";
            List<string> list = new List<string>();

            if (param[0] is string)
            {
                for (int i = 0; i < param.Count; i++)
                {
                    list.Add(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", param[i]));
                }
            }
            else
            {
                for (int i = 0; i < param.Count; i++)
                {
                    list.Add(param[i].ToString());
                }
            }
            json += string.Join(",", list.ToArray());
            json += "]";

            return json;
        }


        /// <summary>${utility_JsonHelper_method_ToRectangle2D_D}</summary>
        public static Rectangle2D ToRectangle2D(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return Rectangle2D.Empty;
            }
            if (!jsonObject.ContainsKey("leftBottom") || !jsonObject.ContainsKey("rightTop"))
            {
                return Rectangle2D.Empty;
            }
            double mbMinX = (double)jsonObject["leftBottom"]["x"];
            double mbMinY = (double)jsonObject["leftBottom"]["y"];
            double mbMaxX = (double)jsonObject["rightTop"]["x"];
            double mbMaxY = (double)jsonObject["rightTop"]["y"];
            return new Rectangle2D(mbMinX, mbMinY, mbMaxX, mbMaxY);
        }

        /// <summary>${utility_JsonHelper_method_ToPoint2D_D}</summary>
        public static Point2D ToPoint2D(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return Point2D.Empty;
            }
            if (!jsonObject.ContainsKey("x") || !jsonObject.ContainsKey("y"))
            {
                return Point2D.Empty;
            }
            return new Point2D() { X = (double)jsonObject["x"], Y = (double)jsonObject["y"] };
        }

        /// <summary>${utility_JsonHelper_method_ToStringList_D}</summary>
        public static List<string> ToStringList(JsonArray jsonArray)
        {
            if (jsonArray != null && jsonArray.Count > 0)
            {
                List<string> stringList = new List<string>();
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    stringList.Add((string)jsonArray[i]);
                }
                return stringList;
            }
            return null;
        }

        /// <summary>${utility_JsonHelper_method_ToRect_D}</summary>
        public static Rect ToRect(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return Rect.Empty;
            }
            if (!jsonObject.ContainsKey("leftTop") || !jsonObject.ContainsKey("rightBottom"))
            {
                return Rect.Empty;
            }
            double viewerMinX = (double)jsonObject["leftTop"]["x"];
            double viewerMinY = (double)jsonObject["leftTop"]["y"];
            double viewerMaxX = (double)jsonObject["rightBottom"]["x"];
            double viewerMaxY = (double)jsonObject["rightBottom"]["y"];
            return new Rect(new Point(viewerMinX, viewerMinY), new Point(viewerMaxX, viewerMaxY));
        }

        /// <summary>${utility_JsonHelper_method_ToCRS_D}</summary>
        public static CoordinateReferenceSystem ToCRS(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return null;
            }
            if (!jsonObject.ContainsKey("pJCoordSysType") || !jsonObject.ContainsKey("coordUnits"))
            {
                return null;
            }
            int wkid = (int)jsonObject["pJCoordSysType"];
            int unitID = (int)jsonObject["coordUnits"];
            Unit unit = (Unit)unitID;
            return new CoordinateReferenceSystem(wkid, unit);
        }
    }
}
