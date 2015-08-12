using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_GeoRegion_Title}</para>
    /// 	<para>${core_GeoRegion_Description}</para>
    /// </summary>
    [DataContract]
    public class GeoRegion : Geometry
    {
        private static double SMALL_VALUE = 1.0E-10;
        private ObservableCollection<Point2DCollection> parts;
        private Rectangle2D savedBounds = new Rectangle2D();
        /// <summary>${core_GeoRegion_constructor_None_D}</summary>
        public GeoRegion()
        {
            this.Parts = new ObservableCollection<Point2DCollection>();
        }
        /// <summary>${core_GeoRegion_method_clone_D}</summary>
        public override Geometry Clone()
        {
            GeoRegion region = new GeoRegion();
            if (this.Parts != null)
            {
                foreach (Point2DCollection points in this.Parts)
                {
                    if (points != null)
                    {
                        Point2DCollection item = new Point2DCollection();
                        foreach (Point2D point in points)
                        {
                            if (point != null)
                            {
                                item.Add(point.Clone());
                            }
                        }
                        region.Parts.Add(item);
                        continue;
                    }
                    region.Parts.Add(null);
                }
            }
            return region;
        }
        /// <summary>${core_GeoRegion_method_Offset_D}</summary>
        /// <param name="deltaX">${core_GeoRegion_method_Offset_param_deltaX}</param>
        /// /// <param name="deltaY">${core_GeoRegion_method_Offset_param_deltaY}</param>
        public override void Offset(double deltaX, double deltaY)
        {
            foreach (var item in this.Parts)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    item[i] = item[i].Offset(deltaX, deltaY); ;
                }
            }
        }

        private void coll_PointChanged(object sender, EventArgs e)
        {
            if (this.Parts.Contains(sender as Point2DCollection))
            {
                savedBounds = new Rectangle2D();
                base.RaiseGeometryChanged();
            }
        }

        private void Parts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                RemoveGeoPointCollectionEvents(e.OldItems);
            }
            if (e.NewItems != null)
            {
                AddGeoPointCollectionEvents(e.NewItems);
            }
            savedBounds = new Rectangle2D();
            base.RaiseGeometryChanged();
        }

        /// <summary>${core_GeoRegion_attribute_bounds_D}</summary>
        public override Rectangle2D Bounds
        {
            get
            {
                if (savedBounds.Width == 0 && savedBounds.Height == 0)
                {
                    Rectangle2D bounds = Rectangle2D.Empty;
                    foreach (Point2DCollection points in this.Parts)
                    {
                        bounds=bounds.Union(points.GetBounds());
                    }

                    savedBounds = bounds;
                }
                return savedBounds;
            }
        }

        /// <summary>${core_GeoRegion_attribute_parts_D}</summary>
        [DataMember]
        public ObservableCollection<Point2DCollection> Parts
        {
            get
            {
                return this.parts;
            }
            set
            {
                if (this.parts != null)
                {
                    this.parts.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Parts_CollectionChanged);
                    this.RemoveGeoPointCollectionEvents(this.parts);
                }
                this.parts = value;
                if (this.parts != null)
                {
                    this.parts.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Parts_CollectionChanged);
                    this.AddGeoPointCollectionEvents(this.parts);
                }
                savedBounds = new Rectangle2D();
            }
        }

        private void AddGeoPointCollectionEvents(IEnumerable items)
        {
            foreach (object obj3 in items)
            {
                Point2DCollection points2 = obj3 as Point2DCollection;
                if (points2 != null)
                {
                    points2.Point2DChanged += coll_PointChanged;
                    points2.CollectionChanged += new NotifyCollectionChangedEventHandler(this.coll_PointChanged);
                }
            }
        }

        private void RemoveGeoPointCollectionEvents(IEnumerable items)
        {
            foreach (object obj2 in items)
            {
                Point2DCollection points = obj2 as Point2DCollection;
                if (points != null)
                {
                    points.Point2DChanged -= coll_PointChanged;
                    points.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.coll_PointChanged);
                }
            }
        }

        /// <summary>${core_GeoRegion_attribute_Center_D}</summary>
        public Point2D Center
        {
            get
            {
                return CalcPolygonCenter();
            }
        }

        private Point2D CalcPolygonCenter()
        {
            Point2DCollection point2Ds = new Point2DCollection();
            Rectangle2D bounds = this.Bounds;
            Point2D center = Point2D.Empty;

            if (this.Parts == null || this.Parts.Count == 0 || Rectangle2D.IsNullOrEmpty(bounds))
            {
                return center;
            }

            int[] parts = new int[this.Parts.Count];

            int point2DCount = 0;
            foreach (var item in this.Parts)
            {
                foreach (var item1 in item)
                {
                    point2Ds.Add(item1);
                }
                parts[point2DCount] = item.Count;
                point2DCount++;
            }


            int i, crossNum, totalPoints;
            double x1, y1, x2, y2;
            double x3, y3, x4, y4;
            double dist, maxLength;
            int maxSegmentNo;
            double sign1 = 0, sign2 = 0;
            bool bAddRepPoint;

            int n = 0;
            int m = 0;
            int lPointCount = 0;
            double yMax;
            double yMin;

            int polyCountsMax = parts[0];
            int flag = 0;
            for (n = 0; n < parts.Length; n++)
            {
                if (polyCountsMax <= parts[n])
                {
                    polyCountsMax = parts[n];
                    flag = n;
                }
            }

            for (n = 0; n < flag; n++)
            {
                lPointCount += parts[n];
            }

            yMax = point2Ds[lPointCount].Y;
            yMin = point2Ds[lPointCount].Y;

            for (m = 0; m < parts[flag]; m++)
            {
                if (yMax <= point2Ds[lPointCount + m].Y)
                {
                    yMax = point2Ds[lPointCount + m].Y;
                }
                if (yMin >= point2Ds[lPointCount + m].Y)
                {
                    yMin = point2Ds[lPointCount + m].Y;
                }
            }

            y1 = (yMax + yMin) / 2;
            y2 = y1;
            x1 = bounds.BottomLeft.X - Math.Abs(bounds.TopRight.X - bounds.BottomLeft.X);
            x2 = bounds.TopRight.X + Math.Abs(bounds.TopRight.X - bounds.BottomLeft.X);

            Point2D pntResult;
            totalPoints = 0;
            crossNum = 0;

            double[] pSPointX = new double[1];

            double y0 = 0;

            for (int n1 = 0; n1 < parts.Length; n1++)
            {
                int crossOfN = 0;

                for (int j1 = 1; j1 < parts[n1]; j1++)
                {
                    x3 = point2Ds[totalPoints + j1 - 1].X;
                    y3 = point2Ds[totalPoints + j1 - 1].Y;
                    x4 = point2Ds[totalPoints + j1].X;
                    y4 = point2Ds[totalPoints + j1].Y;

                    if (((y4 - y3) > SMALL_VALUE) || ((y4 - y3) < -SMALL_VALUE))
                    {
                        bAddRepPoint = true;

                        if (((y3 - y1) > -SMALL_VALUE) && ((y3 - y1) < SMALL_VALUE))
                        {
                            y1 = y3;
                            y2 = y1;
                        }
                        else if (((y4 - y1) > -SMALL_VALUE) && ((y4 - y1) < SMALL_VALUE))
                        {
                            y1 = y4;
                            y2 = y1;
                        }

                        int nSpy = -1;
                        if (j1 == 1)
                        {
                            sign1 = point2Ds[totalPoints + parts[n1] - 2].Y - point2Ds[totalPoints].Y;
                            nSpy = parts[n1] - 2;
                        }
                        else
                        {
                            sign1 = (point2Ds[totalPoints + j1 - 2].Y - point2Ds[totalPoints + j1 - 1].Y);
                            nSpy = j1 - 2;
                        }

                        sign2 = y4 - y3;
                        if (((y3 - y1) > -SMALL_VALUE) && ((y3 - y1) < SMALL_VALUE))
                        {
                            while ((sign1 > -SMALL_VALUE) && (sign1 < SMALL_VALUE))
                            {
                                nSpy--;
                                if (nSpy < 0)
                                {
                                    nSpy = parts[n1] - 2;
                                }
                                sign1 = point2Ds[totalPoints + nSpy].Y - point2Ds[totalPoints + j1 - 1].Y;
                            }
                            bAddRepPoint = ((sign1 * sign2) > 0);
                        }
                    }
                    else
                    {
                        bAddRepPoint = false;
                    }

                    if ((bAddRepPoint) && (this.IntersectLineSect(x1, y1, x2, y2, x3, y3, x4, y4, out pntResult)))
                    {
                        double[] tempPointX = new double[crossNum + 1];
                        int iLength = pSPointX.Length;
                        Array.Copy(pSPointX, 0, tempPointX, 0, iLength);

                        pSPointX = tempPointX;

                        pSPointX[crossNum] = pntResult.X;
                        crossOfN++;
                        crossNum++;

                        if (crossOfN > 1 && pSPointX[crossNum - 1] == pSPointX[crossNum - 2] && (y0 - y3) * (y4 - y3) < 0)
                        {
                            crossNum--;
                            crossOfN--;
                        }
                        y0 = y3;
                    }
                }

                if (crossNum - 1 >= 0 && crossNum - 1 < pSPointX.Length && pSPointX[crossNum - 1] == pSPointX[0])
                {
                    y0 = point2Ds[totalPoints + 1].Y;
                    y3 = point2Ds[totalPoints].Y;
                    y4 = point2Ds[totalPoints + parts[n1] - 1].Y;
                    if ((y0 - y3) * (y4 - y3) < 0)
                    {
                        crossNum--;
                    }
                }

                totalPoints = totalPoints + Math.Abs(parts[n1]);
            }

            if (((crossNum % 2) == 0) && (crossNum >= 2))
            {
                this.QuickSort(pSPointX, 0, (crossNum - 1));

                maxLength = -1.7E+308;
                maxSegmentNo = -1;
                for (i = 0; i <= (crossNum / 2 - 1); i++)
                {
                    dist = pSPointX[2 * i + 1] - pSPointX[2 * i];
                    if (dist > maxLength)
                    {
                        maxLength = dist;
                        maxSegmentNo = i;
                    }
                }
                center.X = (pSPointX[2 * maxSegmentNo + 1] + pSPointX[2 * maxSegmentNo]) / 2;
                center.Y = y1;
            }

            return center;
        }

        private void QuickSort(double[] d, int nStart, int nEnd)
        {
            if (nEnd - nStart < 1)
            {
                return;
            }

            double dTemp;
            if (nEnd - nStart == 1)
            {
                if (d[nStart] > d[nEnd])
                {
                    dTemp = d[nStart];
                    d[nStart] = d[nEnd];
                    d[nEnd] = dTemp;
                }
                return;
            }

            double dPivot = this.Median(d, nStart, nEnd);

            int nLeft, nRight;
            nLeft = nStart;
            nRight = nEnd - 1;
            while (nLeft < nRight)
            {
                while (d[++nLeft] < dPivot)
                {
                    ;
                }
                while (d[--nRight] > dPivot)
                {
                    ;
                }

                if (nLeft < nRight)
                {
                    dTemp = d[nLeft];
                    d[nLeft] = d[nRight];
                    d[nRight] = dTemp;
                }
            }

            d[nEnd - 1] = d[nLeft];
            d[nLeft] = dPivot;

            if (nRight > nStart)
            {
                this.QuickSort(d, nStart, nRight);
            }
            if (nLeft < nEnd)
            {
                this.QuickSort(d, nLeft, nEnd);
            }
        }

        private double Median(double[] d, int nStart, int nEnd)
        {
            int nMid = nStart / 2 + nEnd / 2;
            double dTemp;

            if (d[nStart] > d[nEnd])
            {
                dTemp = d[nStart];
                d[nStart] = d[nEnd];
                d[nEnd] = dTemp;
            }

            if (d[nStart] > d[nMid])
            {
                dTemp = d[nStart];
                d[nStart] = d[nMid];
                d[nMid] = dTemp;
            }

            if (d[nMid] > d[nEnd])
            {
                dTemp = d[nMid];
                d[nMid] = d[nEnd];
                d[nEnd] = dTemp;
            }

            dTemp = d[nEnd - 1];
            d[nEnd - 1] = d[nMid];
            d[nMid] = dTemp;

            return d[nEnd - 1];
        }

        private bool IntersectLineSect(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out Point2D result)
        {
            result = Point2D.Empty;
            bool flag = false;
            double de = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

            if (Math.Abs(de) > SMALL_VALUE)
            {
                double ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / de;

                if ((ub > (-SMALL_VALUE)) && ub < (1 + SMALL_VALUE))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }

            if (flag)
            {
                if (Math.Abs(x1 - x2) < SMALL_VALUE)
                {
                    result.X = x1;
                    result.Y = ((y3 - y4) * x1 + x3 * y4 - x4 * y3) / (x3 - x4);
                }
                else if (Math.Abs(x3 - x4) < SMALL_VALUE)
                {
                    result.X = x3;
                    result.Y = ((x1 - y2) * x3 + x1 * y2 - x2 * y1) / (x1 - x2);
                }
                else if (Math.Abs(y1 - y2) < SMALL_VALUE)
                {
                    result.Y = y1;
                    result.X = ((x3 - x4) * y1 + y3 * x4 - y4 * x3) / (y3 - y4);
                }
                else if (Math.Abs(y3 - y4) < SMALL_VALUE)
                {
                    result.Y = y3;
                    result.X = ((x1 - x2) * y3 + y1 * x2 - y2 * x1) / (y1 - y2);
                }
                else
                {
                    double k1 = (y1 - y2) / (x1 - x2);
                    double k2 = (y3 - y4) / (x3 - x4);
                    double b1 = (x2 * y1 - y2 * x1) / (x2 - x1);
                    double b2 = (x4 * y3 - y4 * x3) / (x4 - x3);

                    result.X = (b2 - b1) / (k1 - k2);
                    result.Y = (b1 * k2 - b2 * k1) / (k2 - k1);
                }
            }

            return flag;
        }
        /// <summary>${core_GeoRegion_method_GetBounds_D}</summary>
        public Rectangle2D GetBounds()
        {
            Point2DCollection point2Ds = new Point2DCollection();
            Rectangle2D bounds = this.Bounds;

            int[] parts = new int[this.Parts.Count];

            int point2DCount = 0;
            foreach (var item in this.Parts)
            {
                foreach (var item1 in item)
                {
                    point2Ds.Add(item1);
                }
                parts[point2DCount] = item.Count;
                point2DCount++;
            }

            if (point2Ds == null || point2Ds.Count == 0)
            {
                //throw new IllegalArgumentException(resource.getMessage("Geometry.getBounds.nopoints")); // %
                return Rectangle2D.Empty;
            }
            else
            {
                double left, bottom, right, top;
                left = point2Ds[0].X;
                bottom = point2Ds[0].Y;
                right = left;
                top = bottom;
                int iLength = point2Ds.Count;
                for (int i = 1; i < iLength; i++)
                {
                    if (point2Ds[i].X < left)
                    {
                        left = point2Ds[i].X;
                    }
                    if (point2Ds[i].Y < bottom)
                    {
                        bottom = point2Ds[i].Y;
                    }
                    if (point2Ds[i].X > right)
                    {
                        right = point2Ds[i].X;
                    }
                    if (point2Ds[i].Y > top)
                    {
                        top = point2Ds[i].Y;
                    }
                }
                return new Rectangle2D(left, bottom, right, top);
            }
        }
    }
}
