using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Clustering
{
    internal static class ClusterUtil
    {
        internal static Brush InterpolateColor(double value, int max, LinearGradientBrush brush)
        {
            if (((brush == null) || (brush.GradientStops == null)) || (brush.GradientStops.Count == 0))
            {
                return new SolidColorBrush(Colors.Black);
            }
            if (brush.GradientStops.Count != 1)
            {
                List<GradientStop> list = new List<GradientStop>(brush.GradientStops.Count);
                foreach (GradientStop stop in brush.GradientStops)
                {
                    list.Add(stop);
                }
                list.Sort((Comparison<GradientStop>)((g1, g2) => g1.Offset.CompareTo(g2.Offset)));
                if (brush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                {
                    value /= (double)max;
                }
                if (list[0].Offset > value)
                {
                    return new SolidColorBrush(brush.GradientStops[0].Color);
                }
                if (list[list.Count - 1].Offset < value)
                {
                    return new SolidColorBrush(brush.GradientStops[list.Count - 1].Color);
                }
                int num2 = 0;
                for (num2 = 1; num2 < list.Count; num2++)
                {
                    if (list[num2].Offset >= value)
                    {
                        Color color = list[num2 - 1].Color;
                        Color color2 = list[num2].Color;
                        double num3 = (value - list[num2 - 1].Offset) / (list[num2].Offset - list[num2 - 1].Offset);
                        byte r = (byte)Math.Round((double)((color.R * (1.0 - num3)) + (color2.R * num3)));
                        byte g = (byte)Math.Round((double)((color.G * (1.0 - num3)) + (color2.G * num3)));
                        byte b = (byte)Math.Round((double)((color.B * (1.0 - num3)) + (color2.B * num3)));
                        byte a = (byte)Math.Round((double)((color.A * (1.0 - num3)) + (color2.A * num3)));
                        return new SolidColorBrush(Color.FromArgb(a, r, g, b));
                    }
                }
            }
            return new SolidColorBrush(brush.GradientStops[0].Color);
        }

        //返回当前i的id，并构造最终的clusterIDs
        public static string BuildClusterIDs(StringBuilder clusterIDs, int i)
        {
            string id = string.Format(CultureInfo.InvariantCulture, "elm{0}", i);
            if (clusterIDs.Length > 0)
            {
                clusterIDs.Append(',');
            }
            clusterIDs.Append(id);//elm0,elm1,elm2...
            return id;
        }

        //依次替换outGridStyle里的{}字符，并构建最终的OutGrids
        public static void BuildOutGrids(string outGridStyle, string margin, double angle, string id, StringBuilder outGrids)
        {
            string grid = outGridStyle.Replace("{Angle}", angle.ToString(CultureInfo.InvariantCulture));
            grid = grid.Replace("{Margin}", margin);
            grid = grid.Replace("{ID}", id);
            outGrids.Append(grid);
        }
    }
}
