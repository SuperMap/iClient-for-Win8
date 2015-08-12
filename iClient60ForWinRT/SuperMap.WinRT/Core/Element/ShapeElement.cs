using System.Collections.Specialized;
using System.Windows;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// <para>${mapping_ShapeBase_Title}</para>
    /// <para>${mapping_ShapeBase_Description}</para>
    /// </summary>
    /// <remarks>${mapping_ShapeBase_Remarks}</remarks>
    public abstract class ShapeElement : UserControl
    {
        private Shape shape;

        /// <summary>${mapping_ShapeBase_attribute_Point2Ds_D}</summary>
        public Point2DCollection Point2Ds
        {
            get { return (Point2DCollection)GetValue(Point2DsProperty); }
            set { SetValue(Point2DsProperty, value); }
        }
        /// <summary>${mapping_ShapeBase_field_Point2DsProperty_D}</summary>
        public static readonly DependencyProperty Point2DsProperty =
            DependencyProperty.Register("Point2DCollection", typeof(Point2DCollection), typeof(ShapeElement), new PropertyMetadata(null,new PropertyChangedCallback(Point2Ds_Changed)));
        private static void Point2Ds_Changed(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ShapeElement sb = o as ShapeElement;
            if (sb != null)
            {
                Point2DCollection oldValue = e.OldValue as Point2DCollection;
                if (oldValue != null)
                {
                    oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(sb.Point2Ds_CollectionChanged);
                }
                Point2DCollection newValue = e.NewValue as Point2DCollection;
                if (newValue != null)
                {
                    newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(sb.Point2Ds_CollectionChanged);
                }
                sb.ScreenUpdated();
            }
        }
        private void Point2Ds_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ScreenUpdated();
        }

        private void ScreenUpdated()
        {
            pathIsInvalid = true;
            SetPath();
            ElementsLayer.SetBBox(this, this.ClippedBounds);
        }
        /// <summary>${mapping_ShapeElement_Constructors_D}</summary>
        protected ShapeElement(Shape shape)
        {
            shape.Stretch = Stretch.None;
            shape.RenderTransform = new ScaleTransform();
            base.Content = this.shape = shape;
            pathIsInvalid = true;

            this.ClipBox = Rectangle2D.Empty;
        }
        /// <summary>${mapping_ShapeBase_method_MeasureOverride_D}</summary>
        /// <param name="availableSize">${mapping_ShapeBase_method_MeasureOverride_param_availableSize}</param>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.SetPath();
            Rectangle2D bounds = this.ClippedBounds;
            if (!Rectangle2D.IsNullOrEmpty(bounds))
            {
                //return base.MeasureOverride(availableSize);
                return new Size(bounds.Width / this.Resolution, bounds.Height / this.Resolution);
            }
            else
            {
                return new Size(0.0, 0.0);
            }
        }

        internal void SetPath()
        {
            if (this.pathIsInvalid)
            {
                Point2DCollection point2Ds = this.ClippedPoint2Ds ?? this.Point2Ds;
                if (point2Ds != null)
                {
                    Rectangle2D bounds = this.ClippedBounds;
                    if (!Rectangle2D.IsNullOrEmpty(bounds))
                    {
                        PointCollection points = new PointCollection();
                        Point topLeft = MapToScreen(new Point2D(bounds.Left, bounds.Top));
                        foreach (Point2D point2d in point2Ds)
                        {
                            Point point = this.MapToScreen(point2d);
                            point.X -= topLeft.X;
                            point.Y -= topLeft.Y;
                            points.Add(point);
                        }
                        this.ScreenPoints = points;
                    }
                }
                else
                {
                    this.ScreenPoints.Clear();
                    //throw new ArgumentNullException("Point2Ds");
                }
                this.pathIsInvalid = false;
            }
        }

        internal void ClearClip()
        {
            this.ClipBox = Rectangle2D.Empty;
            if (this.ClippedPoint2Ds!= null)
            {
                this.ClippedPoint2Ds = null;
                this.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
            }
        }

        internal void SetClip(Point2DCollection clippedPoint2Ds, Rectangle2D clipbox)
        {
            this.ClippedPoint2Ds = clippedPoint2Ds;
            this.ClipBox = clipbox;
            this.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
        }

        internal double OriginY { get; private set; }
        internal double OriginX { get; private set; }
        internal double Resolution { get; private set; }

        internal Rectangle2D ClipBox { get; private set; }
        internal Point2DCollection ClippedPoint2Ds { get; private set; }
        internal Rectangle2D ClippedBounds 
        {
            get
            {
                if(ClippedPoint2Ds == null)
                {
                    //return Rectangle2D.Empty;
                    return this.Bounds;//直接返回正确可用的bounds
                }
                else
                {
                    return this.ClippedPoint2Ds.GetBounds();
                }
            }
        }

        private Point MapToScreen(Point2D pt)
        {
            return CoordinateTransformationHelper.MapToScreen(pt, new Point2D(OriginX, OriginY), this.Resolution);
        }

        private bool pathIsInvalid;
        internal void InvalidatePath(double resolution, double originX, double originY)
        {
            this.pathIsInvalid = true;
            this.Resolution = resolution;
            this.OriginX = originX;
            this.OriginY = originY;
            base.InvalidateMeasure();
            base.InvalidateArrange();
        }

        //protected override AutomationPeer OnCreateAutomationPeer()
        //{
        //    return new ShapeBaseAutomationPeer(this);
        //}

        //属性
        /// <summary>${mapping_ShapeBase_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds
        {
            get
            {
                return this.Point2Ds.GetBounds();
            }
        }
        /// <summary>${mapping_PolygonBase_attribute_ScreenPoints_D}</summary>
        protected abstract PointCollection ScreenPoints { get; set; }
        /// <summary>${mapping_ShapeBase_attribute_EncapsulatedShape_D}</summary>
        public Shape EncapsulatedShape
        {
            get { return this.shape; }
        }

        /// <summary>${mapping_ShapeBase_attribute_Fill_D}</summary>
        public Brush Fill
        {
            get
            {
                return (Brush)this.shape.GetValue(Shape.FillProperty);
            }
            set
            {
                this.shape.SetValue(Shape.FillProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_Opacity_D}</summary>
        public new double Opacity
        {
            get
            {
                return base.Opacity;
            }
            set
            {
                base.Opacity = value;
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_Stroke_D}</summary>
        public Brush Stroke
        {
            get
            {
                return (Brush)this.shape.GetValue(Shape.StrokeProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeDashArray_D}</summary>
        public DoubleCollection StrokeDashArray
        {
            get
            {
                return (DoubleCollection)this.shape.GetValue(Shape.StrokeDashArrayProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeDashArrayProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeDashCap_D}</summary>
        public PenLineCap StrokeDashCap
        {
            get
            {
                return (PenLineCap)this.shape.GetValue(Shape.StrokeDashCapProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeDashCapProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeDashOffset_D}</summary>
        public double StrokeDashOffset
        {
            get
            {
                return (double)this.shape.GetValue(Shape.StrokeDashOffsetProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeDashOffsetProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeEndLineCap_D}</summary>
        public PenLineCap StrokeEndLineCap
        {
            get
            {
                return (PenLineCap)this.shape.GetValue(Shape.StrokeEndLineCapProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeEndLineCapProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeLineJoin_D}</summary>
        public PenLineJoin StrokeLineJoin
        {
            get
            {
                return (PenLineJoin)this.shape.GetValue(Shape.StrokeLineJoinProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeLineJoinProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeMiterLimit_D}</summary>
        public double StrokeMiterLimit
        {
            get
            {
                return (double)this.shape.GetValue(Shape.StrokeMiterLimitProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeMiterLimitProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeStartLineCap_D}</summary>
        public PenLineCap StrokeStartLineCap
        {
            get
            {
                return (PenLineCap)this.shape.GetValue(Shape.StrokeStartLineCapProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeStartLineCapProperty, value);
            }
        }

        /// <summary>${mapping_ShapeBase_attribute_StrokeThickness_D}</summary>
        public double StrokeThickness
        {
            get
            {
                return (double)this.shape.GetValue(Shape.StrokeThicknessProperty);
            }
            set
            {
                this.shape.SetValue(Shape.StrokeThicknessProperty, value);
            }
        }
    }
}
