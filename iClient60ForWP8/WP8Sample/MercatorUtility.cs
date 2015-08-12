using System;
using System.Windows;
using SuperMap.WindowsPhone.Core;

namespace WP8Sample
{
    public static class MercatorUtility
    {
        public const double EarthCircumferenceInMeters = 40075016.685578488;
        public const double HalfEarthCircumferenceInMeters = EarthCircumferenceInMeters / 2;
        public const double EarthRadiusInMeters = 6378137.0;
        public const double MercatorLatitudeLimit = 85.051128;

        //public const double Deg2Rrd = 0.0174532922519943;

        public static double DegreesToRadians(double deg)
        {
            return ((deg * Math.PI) / 180.0);
        }
        public static double RadiansToDegrees(double rad)
        {
            return ((rad / Math.PI) * 180.0);
        }

        internal static Point Point2DToLogicalPoint(Point2D point)
        {
            double num;
            if (point.Y > 85.051128)
            {
                num = 0.0;
            }
            else if (point.Y < -85.051128)
            {
                num = 1.0;
            }
            else
            {
                double num2 = Math.Sin((point.X * 3.1415926535897931) / 180.0);
                num = 0.5 - (Math.Log((1.0 + num2) / (1.0 - num2)) / 12.566370614359173);
            }
            return new Point((point.X + 180.0) / 360.0, num);
        }
        internal static Point2D LogicalPointToPoint2D(Point logicalPoint)
        {
            return new Point2D(90.0 - ((360.0 * Math.Atan(Math.Exp(((logicalPoint.Y * 2.0) - 1.0) * 3.1415926535897931))) / 3.1415926535897931), (logicalPoint.X * 360.0) - 180.0);
        }
        //就是说分辨率
        internal static double MetersPerPixel(Size viewSize, double zoomLevel, Point2D point)
        {
            double num = EarthCircumferenceInMeters / (Math.Pow(2.0, zoomLevel - 1.0) * viewSize.Width);//按正方形了...
            return (Math.Cos(DegreesToRadians(point.Y)) * num);
        }

        public static Point2D MetersToLatLon(Point2D point)
        {
            double lon = point.X / HalfEarthCircumferenceInMeters * 180.0;
            double lat = point.Y / HalfEarthCircumferenceInMeters * 180.0;
            lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(lat * Math.PI / 180.0)) - Math.PI / 2);
            return new Point2D(lon, lat);
        }
        public static Point2D LatLonToMeters(Point2D point)
        {
            double mx = point.X / 180.0 * HalfEarthCircumferenceInMeters;
            double my = Math.Log(Math.Tan((90 + point.Y) * Math.PI / 360.0)) / (Math.PI / 180.0);
            my = my / 180.0 * HalfEarthCircumferenceInMeters;
            return new Point2D(mx, my);
        }
    }
}

