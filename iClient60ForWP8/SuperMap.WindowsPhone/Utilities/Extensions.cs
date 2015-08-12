using System.Collections.ObjectModel;
using SuperMap.WindowsPhone.Core;
using System;

namespace SuperMap.WindowsPhone.Utilities
{
    /// <summary>${WP_utility_Extensions_Title}</summary>
    public static class Extensions
    {
        /// <summary>${WP_utility_Extensions_method_ToPolylineElement_GeoLine_D}</summary>
        public static PolylineElement ToPolylineElement(this GeoLine geoLine)
        {
            if (geoLine != null && geoLine.Parts.Count > 0 && geoLine.Parts[0].Count > 0)
            {
                return new PolylineElement { Point2Ds = geoLine.Parts[0].Clone() };
            }
            return null;
        }
        /// <summary>${WP_utility_Extensions_method_ToGeoLine_D}</summary>
        public static GeoLine ToGeoLine(this PolylineElement polylineElement)
        {
            if (polylineElement != null && polylineElement.Point2Ds != null && polylineElement.Point2Ds.Count > 0)
            {
                GeoLine line = new GeoLine();
                line.Parts.Add(polylineElement.Point2Ds.Clone());
                return line;
            }
            return null;
        }
        /// <summary>${WP_utility_Extensions_method_ToPolygonElement_GeoRegion_D}</summary>
        public static PolygonElement ToPolygonElement(this GeoRegion geoRegion)
        {
            if (geoRegion != null && geoRegion.Parts.Count > 0 && geoRegion.Parts[0].Count > 0)
            {
                return new PolygonElement { Point2Ds = geoRegion.Parts[0].Clone() };
            }
            return null;
        }
        /// <summary>${WP_utility_Extensions_method_ToGeoRegion_PolygonElement_D}</summary>
        public static GeoRegion ToGeoRegion(this PolygonElement polygonElement)
        {
            if (polygonElement != null && polygonElement.Point2Ds != null && polygonElement.Point2Ds.Count > 0)
            {
                GeoRegion region = new GeoRegion();
                region.Parts.Add(polygonElement.Point2Ds.Clone());
                return region;
            }
            return null;
        }

        /// <summary>${WP_utility_Extensions_method_ToGeoRegion_Rectangle2D_D}</summary>
        public static GeoRegion ToGeoRegion(this Rectangle2D bounds)
        {
            if (!Rectangle2D.IsNullOrEmpty(bounds) && bounds.Height != 0 && bounds.Width != 0)
            {
                return new GeoRegion
                {
                    Parts = new ObservableCollection<Point2DCollection> 
                    {
                        new Point2DCollection 
                        { 
                            bounds.BottomLeft, 
                            bounds.BottomRight,
                            bounds.TopRight,
                            bounds.TopLeft,
                            bounds.BottomLeft
                        }
                    }
                };
            }
            return null;
        }
        /// <summary>${WP_utility_Extensions_method_ToPolygonElement_Rectangle2D_D}</summary>
        public static PolygonElement ToPolygonElement(this Rectangle2D bounds)
        {
            if (!Rectangle2D.IsNullOrEmpty(bounds) && bounds.Height != 0 && bounds.Width != 0)
            {
                return new PolygonElement
                {
                    Point2Ds = new Point2DCollection 
                    { 
                        bounds.BottomLeft, 
                        bounds.BottomRight,
                        bounds.TopRight,
                        bounds.TopLeft
                    }
                };
            }
            return null;
        }
        /// <summary>${WP_utility_Extensions_method_Contains_GeoRegion_Point2D_D}</summary>
        /// <param name="region">${WP_utility_Extensions_method_Contains_GeoRegion_Point2D_param_region}</param>
        /// <param name="point">${WP_utility_Extensions_method_Contains_GeoRegion_Point2D_param_point}</param>
        /// <returns>${WP_utility_Extensions_method_Contains_GeoRegion_Point2D_returns_D}</returns>
        public static bool Contains(this GeoRegion region, Point2D point)
        {
            return CheckInRegion(region, point.X, point.Y);
        }
        /// <summary>${WP_utility_Extensions_method_Contains_GeoRegion_double_double_D}</summary>
        /// <param name="region">${WP_utility_Extensions_method_Contains_GeoRegion_double_double_param_region}</param>
        /// <param name="x">${WP_utility_Extensions_method_Contains_GeoRegion_double_double_param_x}</param>
        /// <param name="y">${WP_utility_Extensions_method_Contains_GeoRegion_double_double_param_y}</param>
        /// <returns>${WP_utility_Extensions_method_Contains_GeoRegion_double_double_returns_D}</returns>
        public static bool Contains(this GeoRegion region, double x, double y)
        {
            return CheckInRegion(region, x, y);
        }

        internal static bool CheckInRegion(GeoRegion region, double x, double y)
        {
            Point2DCollection points = region.Parts[0];

            double pValue = double.NaN;
            int i = 0;
            int j = 0;

            double yValue = double.NaN;
            int m = 0;
            int n = 0;

            double iPointX = double.NaN;
            double iPointY = double.NaN;
            double jPointX = double.NaN;
            double jPointY = double.NaN;

            int k = 0;
            int p = 0;

            yValue = points[0].Y - points[(points.Count - 1)].Y;
            if (yValue < 0)
            {
                p = 1;
            }
            else if (yValue > 0)
            {
                p = 0;
            }
            else
            {
                m = points.Count - 2;
                n = m + 1;
                while (points[m].Y == points[n].Y)
                {
                    m--;
                    n--;
                    if (m == 0)
                    {
                        return true;
                    }
                }
                yValue = points[n].Y - points[m].Y;
                if (yValue < 0)
                {
                    p = 1;
                }
                else if (yValue > 0)
                {
                    p = 0;
                }
            }


            //使多边形封闭
            int count = points.Count;
            i = 0;
            j = count - 1;
            while (i < count)
            {
                iPointX = points[j].X;
                iPointY = points[j].Y;
                jPointX = points[i].X;
                jPointY = points[i].Y;
                if (y > iPointY)
                {
                    if (y < jPointY)
                    {
                        pValue = (y - iPointY) * (jPointX - iPointX) / (jPointY - iPointY) + iPointX;
                        if (x < pValue)
                        {
                            k++;
                        }
                        else if (x == pValue)
                        {
                            return true;
                        }
                    }
                    else if (x == jPointY)
                    {
                        p = 0;
                    }
                }
                else if (y < iPointY)
                {
                    if (y > jPointY)
                    {
                        pValue = (y - iPointY) * (jPointX - iPointX) / (jPointY - iPointY) + iPointX;
                        if (x < pValue)
                        {
                            k++;
                        }
                        else if (x == pValue)
                        {
                            return true;
                        }
                    }
                    else if (y == jPointY)
                    {
                        p = 1;
                    }
                }
                else
                {
                    if (x == iPointX)
                    {
                        return true;
                    }
                    if (y < jPointY)
                    {
                        if (p != 1)
                        {
                            if (x < iPointX)
                            {
                                k++;
                            }
                        }
                    }
                    else if (y > jPointY)
                    {
                        if (p > 0)
                        {
                            if (x < iPointX)
                            {
                                k++;
                            }
                        }
                    }
                    else
                    {
                        if (x > iPointX && x <= jPointX)
                        {
                            return true;
                        }
                        if (x < iPointX && x >= jPointX)
                        {
                            return true;
                        }
                    }
                }
                j = i;
                i++;
            }

            if (k % 2 != 0)
            {
                return true;
            }
            return false;
        }

        //从底层拷贝的判断点是否在面内，貌似速度没有扫描线算法快啊。
        internal static bool CheckInRegion1(GeoRegion region, double x, double y)
        {
            // 判断是否在边线上。
            // 设点为Q，线段为P1P2 ，判断点Q在该线段上的依据是：( Q - P1 ) × ( P2 - P1 ) = 0 且 Q 在以
            // P1，P2为对角顶点的矩形内。前者保证Q点在直线P1P2上，后者是保证Q点不在线段P1P2的延长线或反向延长线上

            Point2D p1;
            Point2D p2;
            bool isInLine = false;
            for (int i = 0; i < region.Parts.Count; i++)
            {
                for (int j = 0; j < region.Parts[i].Count; j++)
                {
                    p1 = region.Parts[i][j];
                    if (j == region.Parts[i].Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        p2 = region.Parts[i][j + 1];
                    }
                    if (p1.X == p2.X && p1.Y == p2.Y) // 同一点
                    {
                        continue;
                    }
                    double cross = (x - p1.X) * (p2.Y - p1.Y) - (y - p1.Y) * (p2.X - p1.X);

                    bool xIsInLine = x <= Math.Max(p1.X, p2.X) && x >= Math.Min(p1.X, p2.X);
                    bool yIsInLine = y <= Math.Max(p1.Y, p2.Y) && y >= Math.Min(p1.Y, p2.Y);
                    if (cross == 0 && xIsInLine && yIsInLine)
                    {
                        isInLine = true;
                        break;
                    }
                }

                if (isInLine)
                {
                    break;
                }
            }
            if (isInLine)
            {
                return true;
            }

            bool oddNODES = false;

            for (int p = 0; p < region.Parts.Count; p++)
            {
                int polySides = region.Parts[p].Count;
                double[] polyX = new double[polySides];
                double[] polyY = new double[polySides];
                int j = 0;

                for (int i = 0; i < polySides; i++)
                {
                    polyX[i] = region.Parts[p][i].X;
                    polyY[i] = region.Parts[p][i].Y;
                }

                for (int i = 0; i < polySides; i++)
                {
                    j++;
                    if (j == polySides)
                    {
                        j = 0;
                    }
                    bool innerY = polyY[i] < y && polyY[j] >= y;
                    bool innerY2 = polyY[j] < y && polyY[i] >= y;
                    innerY = innerY || innerY2;
                    if (innerY && (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x))
                    {
                        oddNODES = !oddNODES;
                    }
                }

                // 已经在某一部分的内部，所以不需要继续判断。
                if (oddNODES)
                {
                    return oddNODES;
                }
            }

            return oddNODES;
        }
    }
}
