using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>${WP_core_Point2DCollection_Description}</summary>
    [TypeConverter(typeof(Point2DCollectionConverter))]
    public class Point2DCollection : ObservableCollection<Point2D>
    {
        private Rectangle2D bounds = Rectangle2D.Empty;

        internal event EventHandler Point2DChanged;
        private void RaisePoint2DChanged()
        {
            this.bounds = Rectangle2D.Empty;
            var temp = this.Point2DChanged;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        /// <summary>${WP_core_Point2DCollection_constructor_None_D}</summary>
        public Point2DCollection()
        {
            this.CollectionChanged += new NotifyCollectionChangedEventHandler(Point2DCollection_CollectionChanged);
        }

        private void Point2DCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                this.bounds = Rectangle2D.Empty;
            }
            if (e.OldItems != null)
            {
                foreach (object obj2 in e.OldItems)
                {
                    Point2D point = (Point2D)obj2;
                    if (point != null)
                    {
                        point.PropertyChanged -= point2_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (object obj3 in e.NewItems)
                {
                    //Point2D point2 = (Point2D)obj3;
                    if (obj3 != null)
                    {
                        Point2D point2 = new Point2D();
                        point2.PropertyChanged += new PropertyChangedEventHandler(point2_PropertyChanged);
                        point2.X = ((Point2D)obj3).X;
                        point2.Y = ((Point2D)obj3).Y;
                        if (!Rectangle2D.IsNullOrEmpty(this.bounds))
                        {
                            this.bounds = this.bounds.Union(point2);
                        }
                    }
                }
            }
        }

        private void point2_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Point2D item = (Point2D)sender;

            if (e.PropertyName == "Point2D")
            {
                RaisePoint2DChanged();
            }
        }
        /// <summary>${WP_core_Point2DCollection_method_Clone_D}</summary>
        public Point2DCollection Clone()
        {
            if (this != null)
            {
                Point2DCollection pts = new Point2DCollection();
                foreach (Point2D pt in this)
                {
                    pts.Add(pt);
                }
                return pts;
            }
            return null;
        }


        private Rectangle2D CalculateBounds()
        {
            Rectangle2D bounds = Rectangle2D.Empty;
            if (this != null)
            {
                foreach (var item in this)
                {
                    bounds = bounds.Union(item);
                }
            }
            return bounds;
        }

        /// <summary>${WP_core_Point2DCollection_method_getBounds_D}</summary>
        public Rectangle2D GetBounds()
        {
            return this.CalculateBounds();
        }

        /// <summary>${WP_core_Point2DCollection_method_removeRange_D}</summary>
        /// <param name="index">${WP_core_Point2DCollection_method_removeRange_param_index}</param>
        /// <param name="count">${WP_core_Point2DCollection_method_removeRange_param_count}</param>
        public void RemoveRange(int index, int count)
        {
            if ((index < 0) || (count < 0))
            {
                throw new ArgumentOutOfRangeException("index or count less than zero.");
            }
            if ((this.Count - index) < count)
            {
                throw new ArgumentException();
            }
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    this.RemoveAt(index);
                }
            }

            //if (index >= 0 && count > 0 && ((this.Count - index) >= count))
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        this.RemoveAt(index);
            //    }
            //}
        }
    }
}
