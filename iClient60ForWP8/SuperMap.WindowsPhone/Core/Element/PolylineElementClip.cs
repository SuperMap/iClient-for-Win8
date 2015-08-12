using System;
using System.Collections.Generic;

namespace SuperMap.WindowsPhone.Core
{
    internal class PolylineElementClip
    {
        private Rectangle2D boundary;

        internal PolylineElementClip(Rectangle2D clipBox)
        {
            this.boundary = clipBox;
        }

        internal Point2DCollection Clip(Point2DCollection line)
        {
            Point2DCollection newLine = new Point2DCollection();

            IList<Point2DCollection> list = this.ClipPointCollection(line, this.boundary);
            if ((list != null) && (list.Count > 0))
            {
                foreach (Point2DCollection points in list)
                {
                    foreach (Point2D item in points)
                    {
                        newLine.Add(item);
                    }
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
            }//全部在外,不相交
            if (box.Contains(rect))
            {
                list.Add(points);
                return list;
            }//全部在内
            if (points.Count < 2)
            {
                return null;
            }
            Point2DCollection item = null;
            bool flag = box.Contains(points[0]);
            if (flag)
            {
                item = new Point2DCollection();
                list.Add(item);
                item.Add(points[0]);
            }//第一个点在内
            for (int i = 0; i < (points.Count - 1); i++)
            {
                Point2D head = points[i];
                Point2D end = points[i + 1];
                bool flag2 = box.Contains(end);
                if (flag && flag2)
                {
                    item.Add(end);
                }//同时在内。那么else则有三种情况,head内end外，head外edn内，hean外end外
                else
                {
                    if (flag != flag2)
                    {
                        if (flag)
                        {
                            item.Add(this.ClipLineSegment(head, end, box));
                        }//head内end外，添加交点
                        else
                        {
                            item = new Point2DCollection();
                            list.Add(item);
                            item.Add(this.ClipLineSegment(head, end, box));
                            item.Add(end);
                        }//head外edn内，添加交点和end点
                    }
                    else
                    {
                        Rectangle2D segmentBox = new Rectangle2D(Math.Min(head.X, end.X), Math.Min(head.Y, end.Y), Math.Max(head.X, end.X), Math.Max(head.Y, end.Y));
                        if (segmentBox.IntersectsWith(box))
                        {
                            item = new Point2DCollection();
                            list.Add(item);
                            //与四条边框求交点,在中间就添加上去
                            Point2D point3 = EdgeIntersection(head, end, box.Left, false);
                            Point2D point4 = EdgeIntersection(head, end, box.Right, false);
                            Point2D point5 = EdgeIntersection(head, end, box.Bottom, true);
                            Point2D point6 = EdgeIntersection(head, end, box.Top, true);
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

                        }//hean外end外，添加两个交点
                    }
                    flag = flag2;
                }
            }
            for (int j = list.Count - 1; j >= 0; j--)
            {
                Point2DCollection pts = list[j];
                if (pts.Count < 2)
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

        private Point2D ClipLineSegment(Point2D p0, Point2D p1, Rectangle2D box)
        {
            Point2D point = new Point2D(p1.X, p1.Y);
            if (p1.X < box.Left)
            {
                point = EdgeIntersection(p0, p1, box.Left, false);
            }
            else if (p1.X > box.Right)
            {
                point = EdgeIntersection(p0, p1, box.Right, false);
            }
            if (point.Y < box.Bottom)
            {
                return EdgeIntersection(p0, p1, box.Bottom, true);
            }
            if (point.Y > box.Top)
            {
                point = EdgeIntersection(p0, p1, box.Top, true);
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
    }
}
