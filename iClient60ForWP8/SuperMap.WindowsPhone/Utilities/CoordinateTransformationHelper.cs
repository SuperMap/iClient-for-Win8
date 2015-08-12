using System.Windows;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Utilities
{  
    internal static class CoordinateTransformationHelper
    {
        public static Point MapToScreen(Point2D pt2D, Point2D origin, double resolution)
        {
            if ((!Point2D.IsNullOrEmpty(origin)) && !double.IsNaN(resolution))
            {
                return new Point((pt2D.X - origin.X) / resolution, (origin.Y - pt2D.Y) / resolution);
            }
            return new Point(double.NaN, double.NaN);
        }
    }
}
