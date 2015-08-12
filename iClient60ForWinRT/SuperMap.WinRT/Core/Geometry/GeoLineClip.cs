using System;
using System.Collections.Generic;

namespace SuperMap.WinRT.Core
{
    internal class GeoLineClip
    {
        private Rectangle2D boundary;

        internal GeoLineClip(Rectangle2D clipBox)
        {
            if (Rectangle2D.IsNullOrEmpty(clipBox))
            {
                boundary = Rectangle2D.Empty;
            }
            else
            {
                this.boundary = clipBox;
            }
        }

        internal GeoLine Clip(GeoLine line)
        {
            GeoLine newLine = new GeoLine();
            foreach (Point2DCollection points in line.Parts)
            {
                IList<Point2DCollection> list = this.ClipPointCollection(points, this.boundary);
                if ((list != null) && (list.Count > 0))
                {
                    foreach (Point2DCollection points2 in list)
                    {
                        newLine.Parts.Add(points2);
                    }
                    continue;
                }
            }
            return newLine;
        }

        private IList<Point2DCollection> ClipPointCollection(Point2DCollection points, Rectangle2D box)
        {
            List<Point2DCollection> list = new List<Point2DCollection>();
            Rectangle2D rect = points.GetBounds();
            if (((rect.Right < box.Left) || (rect.Top < box.Bottom)) || ((rect.Left > box.Right) || (rect.Bottom > box.Top)))
            {
                return null;
            }//全部在外
            if (((rect.Left >= box.Left) && (rect.Bottom >= box.Bottom)) && ((rect.Right <= box.Right) && (rect.Top <= box.Top)))
            {
                list.Add(points);
                return list;
            }//全部在内
            if (points.Count < 2)
            {
                return null;
            }
            Point2DCollection item = null;
            bool flag = IsWithin(points[0], box);
            if (flag)
            {
                item = new Point2DCollection();
                list.Add(item);
                item.Add(points[0]);
            }
            for (int i = 0; i < (points.Count - 1); i++)
            {
                Point2D point = points[i];
                Point2D p = points[i + 1];
                bool flag2 = IsWithin(p, box);
                if (flag && flag2)
                {
                    item.Add(p);
                }
                else
                {
                    if (flag != flag2)
                    {
                        if (flag)
                        {
                            item.Add(this.ClipLineSegment(point, p, box));
                        }
                        else
                        {
                            item = new Point2DCollection();
                            list.Add(item);
                            item.Add(this.ClipLineSegment(p, point, box));
                            item.Add(p);
                        }
                    }
                    else
                    {
                        Rectangle2D box3 = new Rectangle2D(Math.Min(point.X, p.X), Math.Min(point.Y, p.Y), Math.Max(point.X, p.X), Math.Max(point.Y, p.Y));             
                        if (box3.IntersectsWith(box))
                        {
                            item = new Point2DCollection();
                            list.Add(item);
                            Point2D point3 = EdgeIntersection(point, p, box.Left, false);
                            Point2D point4 = EdgeIntersection(point, p, box.Right, false);
                            Point2D point5 = EdgeIntersection(point, p, box.Bottom, true);
                            Point2D point6 = EdgeIntersection(point, p, box.Top, true);
                            if ((point6.X >= box.Left) && (point6.X <= box.Right))
                            {
                                item.Add(point6);
                            }
                            if ((point5.X >= box.Left) && (point5.X <= box.Right))
                            {
                                item.Add(point5);
                            }
                            if ((point4.Y >= box.Bottom) && (point4.Y <= box.Top))
                            {
                                item.Add(point4);
                            }
                            if ((point3.Y >= box.Bottom) && (point3.Y <= box.Top))
                            {
                                item.Add(point3);
                            }
                            if (item.Count < 2)
                            {
                                list.Remove(item);
                            }
                        }
                    }
                    flag = flag2;
                }
            }
            for (int j = list.Count - 1; j >= 0; j--)
            {
                Point2DCollection points3 = list[j];
                if (points3.Count < 2)
                {
                    list.RemoveAt(j);
                }
            }
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        private Point2D ClipLineSegment(Point2D p0, Point2D p1, Rectangle2D e)
        {
            Point2D point = new Point2D(p1.X, p1.Y);
            if (p1.X < e.Left)
            {
                point = EdgeIntersection(p0, p1, e.Left, false);
            }
            else if (p1.X > e.Right)
            {
                point = EdgeIntersection(p0, p1, e.Right, false);
            }
            if (point.Y < e.Bottom)
            {
                return EdgeIntersection(p0, p1, e.Bottom, true);
            }
            if (point.Y > e.Top)
            {
                point = EdgeIntersection(p0, p1, e.Top, true);
            }
            return point;
        }

        private static Point2D EdgeIntersection(Point2D p0, Point2D p1, double val, bool horizontal)
        {
            Point2D point = new Point2D(p1.X - p0.X, p1.Y - p0.Y);
            if (horizontal)
            {
                return new Point2D(p0.X + ((point.X / point.Y) * (val - p0.Y)), val);
            }
            return new Point2D(val, p0.Y + ((point.Y / point.X) * (val - p0.X)));
        }

        private static bool IsWithin(Point2D p, Rectangle2D e)
        {
            return ((((p.X >= e.Left) && (p.Y >= e.Bottom)) && (p.X <= e.Right)) && (p.Y <= e.Top));
        }
    }
}

