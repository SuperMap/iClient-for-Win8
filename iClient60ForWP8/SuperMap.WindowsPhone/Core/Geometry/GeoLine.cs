using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_GeoLine_Title}</para>
    /// 	<para>${WP_core_GeoLine_Description}</para>
    /// </summary>
    [DataContract]
    public class GeoLine : Geometry
    {
        private ObservableCollection<Point2DCollection> parts;

        /// <summary>${WP_core_GeoLine_constructor_None_D}</summary>
        public GeoLine()
        {
            this.Parts = new ObservableCollection<Point2DCollection>();
        }

        /// <summary>${WP_core_GeoLine_method_clone_D}</summary>
        public override Geometry Clone()
        {
            GeoLine line = new GeoLine();
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
                        line.Parts.Add(item);
                        continue;
                    }
                    line.Parts.Add(null);
                }
            }
            return line;
        }


        private void coll_PointChanged(object sender, EventArgs e)
        {
            if (this.Parts.Contains(sender as Point2DCollection))
            {
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
            base.RaiseGeometryChanged();
        }

        /// <summary>${WP_core_GeoLine_method_Offset_D}</summary>
        /// <param name="deltaX">${WP_core_GeoLine_method_Offset_param_deltaX}</param>
        /// /// <param name="deltaY">${WP_core_GeoLine_method_Offset_param_deltaY}</param>
        public override void Offset(double deltaX, double deltaY)
        {
            foreach (var item in this.Parts)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    item[i] = item[i].Offset(deltaX, deltaY);
                }
            }
        }

        /// <summary>${WP_core_GeoLine_attribute_bounds_D}</summary>
        public override Rectangle2D Bounds
        {
            get
            {
                Rectangle2D bounds = Rectangle2D.Empty;
                foreach (Point2DCollection points in this.Parts)
                {
                    bounds=bounds.Union(points.GetBounds());
                }
                return bounds;
            }
        }

        /// <summary>${WP_core_GeoLine_attribute_parts_D}</summary>
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
                    points2.CollectionChanged += coll_PointChanged;
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
                    points.CollectionChanged -= coll_PointChanged;
                }
            }
        }

        /// <summary>${WP_core_GeoRegion_attribute_Center_D}</summary>
        public Point2D Center
        {
            get
            {
                return GetCenter();
            }
        }

        private Point2D GetCenter()
        {
            Point2D center = Point2D.Empty;
            int maxCountIndex = 0;
            if (this.Parts != null && this.Parts.Count > 0)
            {
                for (int i = 1; i < this.Parts.Count; i++)
                {
                    if (this.Parts[maxCountIndex].Count < this.Parts[i].Count)
                    {
                        maxCountIndex = i;
                    }
                }

                if (this.Parts[maxCountIndex].Count == 2)
                {
                    center.X = (this.Parts[maxCountIndex][0].X + this.Parts[maxCountIndex][1].X) / 2.0;
                    center.Y = (this.Parts[maxCountIndex][0].Y + this.Parts[maxCountIndex][1].Y) / 2.0;
                }
                else
                {
                    int centerPointPosition = this.Parts[maxCountIndex].Count / 2;
                    center.X = this.Parts[maxCountIndex][centerPointPosition].X;
                    center.Y = this.Parts[maxCountIndex][centerPointPosition].Y;
                }
            }
            return center;
        }
    }
}
