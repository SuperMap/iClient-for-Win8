using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_GeoCircle_Title}</para>
    /// 	<para>${WP_core_GeoCircle_Description}</para>
    /// </summary>
    [DataContract]
    public class GeoCircle : Geometry
    {
        private ObservableCollection<Point2DCollection> parts;
        private Rectangle2D savedBounds = new Rectangle2D();
        private double radius;
        private Point2D center;
        /// <summary>${WP_core_GeoCircle_constructor_None_D}</summary>
        public GeoCircle()
        {
            this.Parts = new ObservableCollection<Point2DCollection>();
        }

        /// <summary>${WP_core_GeoCircle_constructor_Point2D_double_D}</summary>
        /// <param>${WP_core_GeoCircle_constructor_Point2D_double_param__center}</param>
        /// <param>${WP_core_GeoCircle_constructor_Point2D_double_param__radius}</param>
        public GeoCircle(Point2D _center, double _radius)
        {
            this.Parts = new ObservableCollection<Point2DCollection>();
            if (_center != null)
            {
                this.center = _center;
                this.radius = _radius;
                caculateParts();
            }
        }

        //计算parts
        private void caculateParts()
        {
            double unitsAngle = 360 / 72;
            double startAngle = 0;
            double unitsPI = Math.PI / 180;
            this.parts.Clear();
            Point2DCollection item = new Point2DCollection();
            for (int i = 0; i < 72; i++)
            {
                Point2D p = new Point2D(Math.Cos(startAngle * unitsPI) * this.radius + center.X, Math.Sin(startAngle * unitsPI) * this.radius + center.Y);
                item.Add(p);
                startAngle += unitsAngle;
            }
            this.parts.Add(item);
        }
        /// <summary>${WP_core_GeoCircle_method_clone_D}</summary>
        public override Geometry Clone()
        {
            GeoCircle region = new GeoCircle(this.center, this.radius);
            return region;
        }
        /// <summary>${WP_core_GeoCircle_method_Offset_D}</summary>
        /// <param name="deltaX">${WP_core_GeoCircle_method_Offset_param_deltaX}</param>
        /// <param name="deltaY">${WP_core_GeoCircle_method_Offset_param_deltaY}</param>
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

        //取得圆的bounds.
        /// <summary>${WP_core_GeoCircle_attribute_Bounds_D}</summary>
        public override Rectangle2D Bounds
        {
            get
            {
                if (savedBounds.Width == 0 && savedBounds.Height == 0)
                {
                    Rectangle2D bounds = new Rectangle2D(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);

                    savedBounds = bounds;
                }
                return savedBounds;
            }
        }

        //圆中心点属性
        /// <summary>${WP_core_GeoCircle_attribute_Center_D}</summary>
        public Point2D Center
        {
            get
            {
                return this.center;
            }
            set
            {
                this.center = value;
                if (this.radius != 0)
                {
                    //计算parts
                    this.caculateParts();
                }
            }
        }

        //圆的半径
        /// <summary>${WP_core_GeoCircle_attribute_Radius_D}</summary>
        public double Radius
        {
            get
            {
                return this.radius;
            }
            set
            {
                this.radius = value;
                if (this.center != null)
                {
                    //计算parts
                    this.caculateParts();
                }
            }
        }

        /// <summary>${WP_core_GeoCircle_attribute_Parts_D}</summary>
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
    }
}
