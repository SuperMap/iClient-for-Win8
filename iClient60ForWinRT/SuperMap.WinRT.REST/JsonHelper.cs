using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
using Windows.Foundation;
using System;

namespace SuperMap.WinRT.REST
{
    /// <summary><para>${utility_JsonHelper_Title}</para></summary>
    public static class JsonHelper
    {
        /// <summary>${utility_JsonHelper_method_FromRectangle2D_D}</summary>
        public static string FromRectangle2D(Rectangle2D rectangle2D)
        {
            if (Rectangle2D.IsNullOrEmpty(rectangle2D))
            {
                return "{}";
            }
            return string.Format(CultureInfo.InvariantCulture, "{{\"rightTop\":{{\"y\":{0},\"x\":{1}}},\"leftBottom\":{{\"y\":{2},\"x\":{3}}}}}", rectangle2D.Top, rectangle2D.Right, rectangle2D.Bottom, rectangle2D.Left);
        }

        /// <summary>${utility_JsonHelper_method_FromPoint2D_D}</summary>
        public static string FromPoint2D(Point2D point)
        {
            if (Point2D.IsNullOrEmpty(point))
            {
                return "{}"; //string.Empty? null?
            }
            return string.Format(CultureInfo.InvariantCulture, "{{\"x\":{0},\"y\":{1}}}", point.X, point.Y);
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
            double mbMinX = (jsonObject["leftBottom"].GetObjectEx())["x"].GetNumberEx();
            double mbMinY = (jsonObject["leftBottom"].GetObjectEx())["y"].GetNumberEx();
            double mbMaxX = (jsonObject["rightTop"].GetObjectEx())["x"].GetNumberEx();
            double mbMaxY = (jsonObject["rightTop"].GetObjectEx())["y"].GetNumberEx();
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
            return new Point2D() { X = jsonObject["x"].GetNumberEx(), Y = jsonObject["y"].GetNumberEx() };
        }

        /// <summary>${utility_JsonHelper_method_ToStringList_D}</summary>
        public static List<string> ToStringList(JsonArray jsonArray)
        {
            if (jsonArray != null && jsonArray.Count > 0)
            {
                List<string> stringList = new List<string>();
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    stringList.Add(jsonArray[i].GetStringEx());
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
            double viewerMinX = (jsonObject["leftTop"].GetObjectEx())["x"].GetNumberEx();
            double viewerMinY = (jsonObject["leftTop"].GetObjectEx())["y"].GetNumberEx();
            double viewerMaxX = (jsonObject["rightBottom"].GetObjectEx())["x"].GetNumberEx();
            double viewerMaxY = (jsonObject["rightBottom"].GetObjectEx())["y"].GetNumberEx();
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
            int wkid = (int)jsonObject["pJCoordSysType"].GetNumberEx();
            int unitID = (int)jsonObject["coordUnits"].GetNumberEx();
            Unit unit = (Unit)unitID;
            return new CoordinateReferenceSystem(wkid, unit);
        }
        /// <summary>
        /// ${utility_JsonHelper_method_GetObjectEx_D}
        /// </summary>
        public static JsonObject GetObjectEx(this IJsonValue json)
        {
            if (json.ValueType == JsonValueType.Null)
            {
                return null;
            }
            else
            {
                return json.GetObject();
            }
        }
        /// <summary>
        /// ${utility_JsonHelper_method_GetStringEx_D}
        /// </summary>
        public static string GetStringEx(this IJsonValue json)
        {
            if (json.ValueType == JsonValueType.Null)
            {
                return string.Empty;
            }
            else
            {
                return json.GetString();
            }
        }
        /// <summary>
        /// ${utility_JsonHelper_method_GetNumberEx_D}
        /// </summary>
        public static double GetNumberEx(this IJsonValue json)
        {
            if (json.ValueType == JsonValueType.Null)
            {
                return 0;
            }
            else
            {
                return json.GetNumber();
            }
        }
        /// <summary>
        /// ${utility_JsonHelper_method_GetBooleanEx_D}
        /// </summary>
        public static Boolean GetBooleanEx(this IJsonValue json)
        {
            if (json.ValueType == JsonValueType.Null)
            {
                return false ;
            }
            else
            {
                return json.GetBoolean();
               
            }
        }

    }
}
