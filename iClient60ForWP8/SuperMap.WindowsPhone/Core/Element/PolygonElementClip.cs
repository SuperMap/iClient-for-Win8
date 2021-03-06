﻿
namespace SuperMap.WindowsPhone.Core
{
    internal class PolygonElementClip
    {
        private Rectangle2D boundary;
        private Edge[] Edges;

        internal PolygonElementClip(Rectangle2D clipBox)
        {
            this.boundary = clipBox;
            this.Edges = new Edge[] 
            {   
                new Edge { IsHorisontal = false, IsLeft = true, Value = this.boundary.Left },
                new Edge { IsHorisontal = false, IsLeft = false, Value = this.boundary.Right },
                new Edge { IsHorisontal = true, IsLeft = true, Value = this.boundary.Bottom }, 
                new Edge { IsHorisontal = true, IsLeft = false, Value = this.boundary.Top }
            };
        }

        internal Point2DCollection Clip(Point2DCollection pts)
        {
            Point2DCollection newPoints ;
            newPoints = pts;
            if (pts.GetBounds().IntersectsWith(this.boundary))
            {
                foreach (Edge edge in this.Edges)
                {
                    newPoints = ClipRing(newPoints, edge);
                }
            }
            return newPoints;
        }

        private static Point2DCollection ClipRing(Point2DCollection ring, Edge edge)
        {
            if ((ring == null) || (ring.Count < 2))
            {
                return null;
            }
            Point2DCollection points = new Point2DCollection();
            Point2D lastPoint = ring[ring.Count - 1];
            for (int i = 0; i < ring.Count; i++)
            {
                Point2D point = ring[i];
                if (Inside(point, edge))
                {
                    if (Inside(lastPoint, edge))
                    {
                        points.Add(point);
                    }
                    else
                    {
                        Point2D item = EdgeIntersection(lastPoint, point, edge);
                        points.Add(item);
                        points.Add(point);
                    }
                }
                else if (Inside(lastPoint, edge))
                {
                    Point2D item = EdgeIntersection(lastPoint, point, edge);
                    points.Add(item);
                }
                lastPoint = point;
            }
            return points;
        }

        //相对某边Edge，该点GeoPoint是否在"内侧"
        private static bool Inside(Point2D p0, Edge edge)
        {
            if (edge.IsHorisontal)
            {
                if (edge.IsLeft)
                {
                    return (p0.Y > edge.Value);//bottom
                }
                return (p0.Y < edge.Value);//top
            }
            if (edge.IsLeft)
            {
                return (p0.X > edge.Value);//left
            }
            return (p0.X < edge.Value);//right
        }

        //两点与线相交的那个点
        private static Point2D EdgeIntersection(Point2D p0, Point2D p1, Edge edge)
        {
            Point2D point = new Point2D(p1.X - p0.X, p1.Y - p0.Y);
            if (edge.IsHorisontal)
            {
                return new Point2D(p0.X + ((point.X / point.Y) * (edge.Value - p0.Y)), edge.Value);
            }
            return new Point2D(edge.Value, p0.Y + ((point.Y / point.X) * (edge.Value - p0.X)));
        }

        private class Edge
        {
            public bool IsHorisontal { get; set; }

            public bool IsLeft { get; set; }

            public double Value { get; set; }
        }
    }
}
