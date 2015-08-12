using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SuperMap.WinRT.Core
{
    /// <summary>${core_Point2DCollection_Description}</summary>
    /// <example>
    /// <code>
    /// Point2DCollection points = new Point2DCollection();
    /// </code>
    /// </example>
    public class Point2DCollection : ObservableCollection<Point2D>
    {
        private Rectangle2D _bounds = Rectangle2D.Empty;
        bool _collectionChanged = false;

        internal event EventHandler Point2DChanged;
        private void RaisePoint2DChanged()
        {
            var temp = this.Point2DChanged;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        /// <summary>${core_Point2DCollection_constructor_None_D}</summary>
        public Point2DCollection()
        {
            this.CollectionChanged += new NotifyCollectionChangedEventHandler(Point2DCollection_CollectionChanged);
        }

        private void Point2DCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _collectionChanged = true;
            if (e.OldItems != null)
            {
                foreach (object obj in e.OldItems)
                {
                    Point2D point = (Point2D)obj;
                    if (point != null)
                    {
                        point.PropertyChanged -= point_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (object obj in e.NewItems)
                {
                    if (obj != null)
                    {
                        Point2D point = (Point2D)obj;
                        if (point != null)
                        {
                            point.PropertyChanged += point_PropertyChanged;
                        }
                    }
                }
            }
        }

        private void point_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Point2D item = (Point2D)sender;

            if (e.PropertyName == "Point2D")
            {
                _collectionChanged = true;
                RaisePoint2DChanged();
            }
        }
        /// <summary>${core_Point2DCollection_method_Clone_D}</summary>
        public Point2DCollection Clone()
        {
            Point2DCollection pts = new Point2DCollection();
            foreach (Point2D pt in this)
            {
                pts.Add(pt.Clone());
            }
            return pts;
        }


        private Rectangle2D CalculateBounds()
        {
            Rectangle2D bounds = Rectangle2D.Empty;
            if (_collectionChanged)
            {
                foreach (var item in this)
                {
                    bounds = bounds.Union(item);
                }
                _bounds = bounds;
                _collectionChanged = false;
            }
            return _bounds;
        }

        /// <summary>${core_Point2DCollection_method_getBounds_D}</summary>
        public Rectangle2D GetBounds()
        {
            return this.CalculateBounds();
        }

        /// <summary>${core_Point2DCollection_method_removeRange_D}</summary>
        /// <param name="index">${core_Point2DCollection_method_removeRange_param_index}</param>
        /// <param name="count">${core_Point2DCollection_method_removeRange_param_count}</param>
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
