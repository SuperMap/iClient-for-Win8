using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SuperMap.WindowsPhone.Core;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SuperMap.WindowsPhone.Mapping
{
    internal sealed class LayerContainer : Panel
    {
        private bool delayClean;
        private int progress;
        private List<Rectangle2D> orignalBounds;

        internal LayerContainer(Layer layer)
        {
            this.Layer = layer;
            base.Opacity = layer.Opacity;
            this.progress = layer.progress;
            this.orignalBounds = new List<Rectangle2D>();
            layer.Progress += new EventHandler<ProgressEventArgs>(this.layer_Progress);
            layer.PropertyChanged += new PropertyChangedEventHandler(this.layer_PropertyChanged);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement element in base.Children)
            {
                if (element.Visibility == Visibility.Visible)
                {
                    element.Measure(availableSize);
                }
            }
            return base.MeasureOverride(availableSize);
        }


        //比较计算后bounds和每个bounds的相交
        private bool computeIntersectWithAll(Rectangle2D bounds)
        {
            var boundsCollection = (Layer as ElementsLayer).BoundsCollection.Values;
            foreach (Rectangle2D rect in boundsCollection)
            {
                if (rect != null)
                {
                    if (bounds.IntersectsWith(rect))
                    {
                        return true;
                    }
                }
            }
            foreach (Rectangle2D rect in orignalBounds)
            {
                if (rect != null)
                {
                    if (bounds.IntersectsWith(rect))
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        //获取是否需要变换bounds 以及bounds是否可以变换
        private Rectangle2D isBoundsChange(Rectangle2D bounds, double referenceLength, Size referenceSize, out bool isIntersect, out bool isGetNewBounds)
        {
            //初始化要修改的范围
            isIntersect = false;
            isGetNewBounds = false;
            var boundsCollection = (Layer as ElementsLayer).BoundsCollection.Values;
            var radius = referenceLength * 0.7;
            Rectangle2D newBounds = new Rectangle2D();
            if (referenceSize.Width == 0 && referenceSize.Height == 0)
            {
                referenceSize.Width = bounds.Width;
                referenceSize.Height = bounds.Height;
            }
            else
            {
                referenceSize.Width *= this.Resolution;
                referenceSize.Height *= this.Resolution;

            }

            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        newBounds = new Rectangle2D(bounds.Left + referenceSize.Width / 2, bounds.Bottom + radius, bounds.Right + referenceSize.Width / 2, bounds.Top + radius);
                        break;
                    case 1:
                        newBounds = new Rectangle2D(bounds.Left + referenceSize.Width / 2 + radius, bounds.Bottom, bounds.Right + referenceSize.Width / 2 + radius, bounds.Top);
                        break;
                    case 2:
                        newBounds = new Rectangle2D(bounds.Left + referenceSize.Width / 2, bounds.Bottom - radius, bounds.Right + referenceSize.Width / 2, bounds.Top - radius);
                        break;
                    case 3:
                        newBounds = new Rectangle2D(bounds.Left, bounds.Bottom - radius, bounds.Right, bounds.Top - radius);
                        break;
                    case 4:
                        newBounds = new Rectangle2D(bounds.Left - referenceSize.Width / 2, bounds.Bottom - radius, bounds.Right - referenceSize.Width / 2, bounds.Top - radius);
                        break;
                    case 5:
                        newBounds = new Rectangle2D(bounds.Left - referenceSize.Width / 2 - radius, bounds.Bottom, bounds.Right - referenceSize.Width / 2 - radius, bounds.Top);
                        break;
                    case 6:
                        newBounds = new Rectangle2D(bounds.Left - referenceSize.Width / 2, bounds.Bottom + radius, bounds.Right - referenceSize.Width / 2, bounds.Top + radius);
                        break;
                    case 7:
                        newBounds = new Rectangle2D(bounds.Left, bounds.Bottom + radius, bounds.Right, bounds.Top + radius);
                        break;
                }

                if (computeIntersectWithAll(newBounds))
                {
                    isIntersect = true;
                    if (i == 1 || i == 5)
                    {
                        continue;
                    }

                    newBounds=new Rectangle2D(newBounds.Left, newBounds.Bottom + referenceSize.Height, newBounds.Right, newBounds.Top + referenceSize.Height);
                    if (computeIntersectWithAll(newBounds))
                    {
                        newBounds=new Rectangle2D(newBounds.Left, newBounds.Bottom - 2 * referenceSize.Height, newBounds.Right, newBounds.Top - 2 * referenceSize.Height);
                        if (computeIntersectWithAll(newBounds))
                        {
                            continue;
                        }
                        else
                        {
                            isGetNewBounds = true;
                            return newBounds;
                        }
                    }
                    else
                    {
                        isGetNewBounds = true;
                        return newBounds;

                    }
                }
                else
                {
                    isGetNewBounds = true;
                    return newBounds;

                }
            }
            return bounds;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double resolution = this.Resolution;
            Point2D origin = new Point2D(this.OriginX, this.OriginY);
            Rectangle2D clipBox = new Rectangle2D(
                origin.X - (resolution * 4191.0),
                origin.Y - (resolution * 4191.0),
                origin.X + (resolution * 4191.0),
                origin.Y + (resolution * 4191.0));
            if (this.Layer is ElementsLayer) (Layer as ElementsLayer).BoundsCollection.Clear();
            if (this.Layer is ElementsLayer) orignalBounds.Clear();
            bool istrue = this.Layer is ElementsLayer ? (this.Layer as ElementsLayer).IsAutoAvoidance : false;
            if (istrue)
            {
                foreach (UIElement element in base.Children)
                {
                    Rectangle2D b = GetBounds(element);
                    double referenceLength = (Layer as ElementsLayer).GetReferenceLength(element) * resolution;
                    if (b.BottomLeft == b.TopRight)
                    {
                        b = new Rectangle2D(b.Left - (referenceLength) / 2, b.Bottom - (element.DesiredSize.Height * resolution) / 2,
                                            b.Left + (referenceLength) / 2, b.Bottom + (element.DesiredSize.Height * resolution) / 2);
                    }
                    orignalBounds.Add(b);
                }
            }

            foreach (UIElement element in base.Children)
            {
                if (element.Visibility != Visibility.Visible)
                {
                    continue;
                }
                if (element is FeatureElement)
                {
                    this.ArrangeFeature(element as FeatureElement, clipBox);
                    continue;
                }
                if (element is ShapeElement)
                {
                    this.ArrangeShapeBase(element as ShapeElement, clipBox);
                    continue;
                }
                Rectangle2D b = GetBounds(element);

                if (istrue)
                {
                    if (b.BottomLeft == b.TopRight)
                    {
                        b = new Rectangle2D(b.Left - (element.DesiredSize.Width * resolution) / 2, b.Bottom - (element.DesiredSize.Height * resolution) / 2,
                                            b.Left + (element.DesiredSize.Width * resolution) / 2, b.Bottom + (element.DesiredSize.Height * resolution) / 2);
                    }
                    bool isIntersect;
                    bool isGetNewBounds;
                    double referenceLength = (Layer as ElementsLayer).GetReferenceLength(element) * resolution;
                    Size referenceSize = (Layer as ElementsLayer).GetReferenceSize(element);

                    b = isBoundsChange(b, referenceLength, referenceSize, out isIntersect, out isGetNewBounds);

                    if (isIntersect)
                    {
                        if (isGetNewBounds)
                        {
                            (element as UIElement).Opacity = 1;
                        }
                        else
                        {
                            (element as UIElement).Opacity = 0;
                        }
                    }
                    else
                    {
                        (element as UIElement).Opacity = 1;
                    }

                    (Layer as ElementsLayer).BoundsCollection.Add(element, b);
                }

                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double pixelX = (b.Left - origin.X) / resolution;
                    double pixelY = (origin.Y - b.Top) / resolution;
                    double pixelWidth = b.Width / resolution;
                    double pixelHeight = b.Height / resolution;
                    //&& 
                    if (this.Layer is TiledLayer)
                    {
                        pixelWidth++;
                        pixelHeight++;
                        element.UseLayoutRounding = false;
                    }

                    if (((!double.IsInfinity(pixelWidth) && !double.IsInfinity(pixelHeight)) && (!double.IsInfinity(pixelX) && !double.IsInfinity(pixelY))) && ((!double.IsNaN(pixelWidth) && !double.IsNaN(pixelHeight)) && (!double.IsNaN(pixelX) && !double.IsNaN(pixelY))))
                    {
                        double width;
                        double height;
                        if (((element is FrameworkElement) && (pixelWidth != 0.0)) && (pixelHeight != 0.0))
                        {
                            (element as FrameworkElement).Width = pixelWidth;
                            (element as FrameworkElement).Height = pixelHeight;
                        }
                        if ((pixelWidth != 0.0) || (pixelHeight != 0.0))
                        {
                            element.Arrange(new Rect(pixelX, pixelY, pixelWidth, pixelHeight));
                            continue;
                        }
                        //相当于左上，右下，中心等位置的设定。
                        //设在某个点，bbox的宽和高都是0，比如pushpin\textbox等
                        switch (((HorizontalAlignment)element.GetValue(FrameworkElement.HorizontalAlignmentProperty)))
                        {
                            case HorizontalAlignment.Left:
                                width = element.DesiredSize.Width;
                                break;

                            case HorizontalAlignment.Right:
                                width = 0.0;
                                break;

                            default:
                                width = element.DesiredSize.Width * 0.5;
                                break;
                        }
                        switch (((VerticalAlignment)element.GetValue(FrameworkElement.VerticalAlignmentProperty)))
                        {
                            case VerticalAlignment.Top:
                                height = element.DesiredSize.Height;
                                break;

                            case VerticalAlignment.Bottom:
                                height = 0.0;
                                break;

                            default:
                                height = element.DesiredSize.Height * 0.5;
                                break;
                        }
                        element.Arrange(new Rect(new Point(pixelX - width, pixelY - height), element.DesiredSize));
                    }
                }
            }
            return base.ArrangeOverride(finalSize);
        }

        private void ArrangeShapeBase(ShapeElement sb, Rectangle2D clipBox)
        {
            if (sb.Visibility != Visibility.Collapsed)
            {
                Rectangle2D b = GetBounds(sb);
                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double x = (b.Left - this.Origin.X) / this.Resolution;
                    double y = (this.Origin.Y - b.Top) / this.Resolution;

                    if (((b.Width > 0.0) || (b.Height > 0.0)) && (sb.Point2Ds != null))
                    {
                        double ratio = sb.Resolution / this.Resolution;
                        this.SetClipShapeBase(sb, clipBox);
                        if (sb.ClippedPoint2Ds != null)
                        {
                            b = sb.ClippedPoint2Ds.GetBounds();
                            if (Rectangle2D.IsNullOrEmpty(b))
                            {
                                return;
                            }
                            x = (b.Left - this.Origin.X) / this.Resolution;
                            y = (this.Origin.Y - b.Top) / this.Resolution;
                        }

                        if (sb.EncapsulatedShape.RenderTransform is ScaleTransform)
                        {
                            (sb.EncapsulatedShape.RenderTransform as ScaleTransform).ScaleX = (sb.EncapsulatedShape.RenderTransform as ScaleTransform).ScaleY = ratio;
                        }
                        else
                        {
                            sb.EncapsulatedShape.RenderTransform = new ScaleTransform { ScaleX = ratio, ScaleY = ratio };
                        }
                        double num4 = b.Width / sb.Resolution * ratio + 10.0;
                        double num5 = b.Height / sb.Resolution * ratio + 10.0;
                        num4 = Math.Min(32000.0, num4);
                        num5 = Math.Min(32000.0, num5);
                        sb.Arrange(new Rect(x, y, num4, num5));
                    }
                    else
                    {
                        sb.Arrange(new Rect(new Point(x, y), sb.DesiredSize));
                    }
                }
            }
        }

        private void ArrangeFeature(FeatureElement elm, Rectangle2D clipBox)
        {
            if (elm.Visibility != Visibility.Collapsed)
            {
                Rectangle2D b = GetBounds(elm);
                if (!Rectangle2D.IsNullOrEmpty(b))
                {
                    double x = (b.Left - this.Origin.X) / this.Resolution;
                    double y = (this.Origin.Y - b.Top) / this.Resolution;
                    if (((b.Width > 0.0) || (b.Height > 0.0)) && (elm.PathGeometry != null))
                    {
                        double ratio = elm.Resolution / this.Resolution;
                        this.SetClip(elm, clipBox);
                        if (elm.ClippedGeometry != null)
                        {
                            b = elm.ClippedGeometry.Bounds;
                            if (Rectangle2D.IsNullOrEmpty(b))
                            {
                                return;
                            }
                            x = (b.Left - this.Origin.X) / this.Resolution;
                            y = (this.Origin.Y - b.Top) / this.Resolution;
                        }
                        if (elm.PathGeometry.Transform is ScaleTransform)
                        {
                            (elm.PathGeometry.Transform as ScaleTransform).ScaleX = (elm.PathGeometry.Transform as ScaleTransform).ScaleY = ratio;
                        }
                        else
                        {
                            elm.PathGeometry.Transform = new ScaleTransform { ScaleX = ratio, ScaleY = ratio };
                        }

                        double num4 = ((b.Width / elm.Resolution) * ratio) + 10.0;
                        double num5 = ((b.Height / elm.Resolution) * ratio) + 10.0;//这也已经加了10.0
                        num4 = Math.Min(32000.0, num4);
                        num5 = Math.Min(32000.0, num5);
                        elm.Arrange(new Rect(x, y, num4, num5));
                    }
                    else
                    {
                        elm.Arrange(new Rect(new Point(x, y), elm.DesiredSize));
                    }
                }
            }
        }

        //int最大值32765的一半16382.5,最大支持16383
        private void SetClip(FeatureElement fe, Rectangle2D clipBox)
        {
            Rectangle2D bounds = fe.Geometry.Bounds;
            if ((Rectangle2D.IsNullOrEmpty(bounds)) || ((bounds.Width / this.Resolution) < 16383.5))
            {
                if (fe.ClippedGeometry != null)
                {
                    fe.ClearClip();
                }
            }
            else if (((Rectangle2D.IsNullOrEmpty(fe.ClipBox)) || !clipBox.Within(fe.ClipBox)) || ((fe.ClipBox.Width / this.Resolution) >= 16383.5))
            {
                if (fe.Geometry is GeoRegion)
                {
                    fe.SetClip(new GeoRegionClip(clipBox).Clip(fe.Geometry as GeoRegion), clipBox);
                }
                else if (fe.Geometry is GeoLine)
                {
                    fe.SetClip(new GeoLineClip(clipBox).Clip(fe.Geometry as GeoLine), clipBox);
                }
            }
        }

        //对于SL自带的Rectangle暂不做处理
        private void SetClipShapeBase(ShapeElement sb, Rectangle2D clipBox)
        {
            Rectangle2D bounds = sb.Point2Ds.GetBounds();
            if ((Rectangle2D.IsNullOrEmpty(bounds)) || ((bounds.Width / this.Resolution) < 16383.5))
            {
                if (sb.ClippedPoint2Ds != null)
                {
                    sb.ClearClip();
                }
            }
            else if (((Rectangle2D.IsNullOrEmpty(sb.ClipBox)) || !clipBox.Within(sb.ClipBox)) || ((sb.ClipBox.Width / this.Resolution) >= 16383.5))
            {
                if (sb is PolygonElement)
                {
                    sb.SetClip(new PolygonElementClip(clipBox).Clip(sb.Point2Ds), clipBox);
                }
                else if (sb is PolylineElement)
                {
                    sb.SetClip(new PolylineElementClip(clipBox).Clip(sb.Point2Ds), clipBox);
                }
            }
        }


        internal void ResetGeometryTransforms()
        {
            if (this.Layer is FeaturesLayer)
            {
                foreach (UIElement element in base.Children)
                {
                    if (element is FeatureElement)
                    {
                        this.ResetGeometryTransform((FeatureElement)element);
                    }
                }
                base.InvalidateMeasure();
            }
            if (this.Layer is ElementsLayer)
            {
                foreach (UIElement element in base.Children)
                {
                    if (element is ShapeElement)
                    {
                        ShapeElement elm = (ShapeElement)element;
                        this.ResetShapeBaseTransform(elm);
                    }
                }
                base.InvalidateMeasure();
            }
        }

        internal void ResetGeometryTransform(FeatureElement elm)
        {
            if (((elm.OriginX != this.Origin.X) || (elm.OriginY != this.Origin.Y)) || (elm.Resolution != this.Resolution))
            {
                elm.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
            }
        }

        internal void ResetShapeBaseTransform(ShapeElement sb)
        {
            if (((sb.OriginX != this.Origin.X) || (sb.OriginY != this.Origin.Y)) || (sb.Resolution != this.Resolution))
            {
                sb.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
            }
        }

        internal void MarkOutdated(bool wait)
        {
            if (!(this.Layer is FeaturesLayer))
            {
                this.delayClean = wait;
                foreach (UIElement element in base.Children)
                {
                    element.SetValue(OutdatedProperty, true);
                }
                if (!this.delayClean && (this.progress == 100))
                {
                    this.CleanUp();
                }
            }
        }
        private void CleanUp()
        {
            for (int i = base.Children.Count - 1; i >= 0; i--)
            {
                UIElement element = base.Children[i];
                if ((bool)element.GetValue(OutdatedProperty))
                {
                    base.Children.Remove(element);
                    if (element is Image)
                    {
                        Image image = element as Image;
                        if (image.Source is BitmapImage)
                        {
                            ((BitmapImage)image.Source).UriSource = null;
                        }
                        image.Source = null;
                    }
                }
            }
        }

        private void layer_Progress(object sender, ProgressEventArgs args)
        {
            if (this == this.Layer.Container)
            {
                this.progress = args.Progress;
            }
            if ((args.Progress == 100) && !this.delayClean)
            {
                this.CleanUp();
            }
        }
        private void layer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Layer layer = sender as Layer;
            if (e.PropertyName == "IsVisible")
            {
                if (!layer.IsVisible)
                {
                    layer.Container.Visibility = Visibility.Collapsed;
                }
                else if (((layer.Container.Resolution < layer.MinVisibleResolution) && !double.IsNaN(layer.MinVisibleResolution)) || ((layer.Container.Resolution > layer.MaxVisibleResolution) && !double.IsNaN(layer.MaxVisibleResolution)))
                {
                    layer.Container.Visibility = Visibility.Collapsed;
                }
                else
                {
                    layer.Container.Visibility = Visibility.Visible;
                }
            }
            else if (e.PropertyName == "Opacity")
            {
                base.Opacity = layer.Opacity;
            }
        }

        public Point2D Origin
        {
            get
            {
                return new Point2D(this.OriginX, this.OriginY);
            }
        }
        private static void OnOriginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LayerContainer container = d as LayerContainer;
            if (container != null)
            {
                container.InvalidateArrange();
            }
        }//X,Y共用

        public static readonly DependencyProperty OriginXProperty = DependencyProperty.Register("OriginX", typeof(double), typeof(LayerContainer), new PropertyMetadata(new PropertyChangedCallback(OnOriginPropertyChanged)));
        public double OriginX
        {
            get
            {
                return (double)base.GetValue(OriginXProperty);
            }
            set
            {
                base.SetValue(OriginXProperty, value);
            }
        }

        public static readonly DependencyProperty OriginYProperty = DependencyProperty.Register("OriginY", typeof(double), typeof(LayerContainer), new PropertyMetadata(new PropertyChangedCallback(OnOriginPropertyChanged)));
        public double OriginY
        {
            get
            {
                return (double)base.GetValue(OriginYProperty);
            }
            set
            {
                base.SetValue(OriginYProperty, value);
            }
        }

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(double), typeof(LayerContainer), new PropertyMetadata(new PropertyChangedCallback(OnResolutionPropertyChanged)));
        public double Resolution
        {
            get
            {
                return (double)base.GetValue(ResolutionProperty);
            }
            set
            {
                base.SetValue(ResolutionProperty, value);
            }
        }
        private static void OnResolutionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LayerContainer container = d as LayerContainer;

            if (container != null)
            {
                if ((container.Layer != null) && container.Layer.IsVisible)
                {
                    //在这里判断当分辨率发生变化，是否超出最大，最小可见比例尺的可见性；
                    //设置了最大、小可见分辨率！
                    bool isCollapsed = false;
                    if (container.Layer.MinVisibleResolution != double.Epsilon || !double.IsPositiveInfinity(container.Layer.MaxVisibleResolution))
                    {
                        isCollapsed = (container.Resolution < container.Layer.MinVisibleResolution) || (container.Resolution > container.Layer.MaxVisibleResolution);
                    }  //如果设置了分辨率；
                    else if (container.Layer.Map != null)
                    {
                        isCollapsed = container.Layer.Map.Scale < container.Layer.MinVisibleScale || container.Layer.Map.Scale > container.Layer.MaxVisibleScale;
                    }//如果设置了比例尺; 主要为了在Map初始化完成之前设置了layer的最大最小比例尺

                    if (isCollapsed)
                    {
                        container.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        container.Visibility = Visibility.Visible;
                    }

                }
                container.InvalidateArrange();
            }
        }

        //isOld
        internal static readonly DependencyProperty OutdatedProperty = DependencyProperty.RegisterAttached("Outdated", typeof(bool), typeof(LayerContainer), new PropertyMetadata(false));

        //每个Element的bbox
        #region Bounds (Attached DependencyProperty)

        internal static readonly DependencyProperty BoundsProperty =
            DependencyProperty.RegisterAttached("Bounds", typeof(Rectangle2D), typeof(LayerContainer), new PropertyMetadata(Rectangle2D.Empty, new PropertyChangedCallback(OnBoundsChanged)));
        public static void SetBounds(DependencyObject o, Rectangle2D value)
        {
            o.SetValue(BoundsProperty, value);
        }
        public static Rectangle2D GetBounds(DependencyObject o)
        {
            return (Rectangle2D)o.GetValue(BoundsProperty);
        }
        private static void OnBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement)
            {
                FrameworkElement element = (FrameworkElement)d;
                LayerContainer parent = element.Parent as LayerContainer;
                if ((parent != null) && (parent.Layer is ElementsLayer))
                {
                    Rectangle2D oldValue = (Rectangle2D)e.OldValue;
                    Rectangle2D newValue = (Rectangle2D)e.NewValue;
                    if (Rectangle2D.IsNullOrEmpty(newValue))
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                    else if (!Rectangle2D.IsNullOrEmpty(oldValue) && oldValue.Width == newValue.Width && oldValue.Height == newValue.Height)
                    {
                        parent.InvalidateArrange();
                    }
                    else
                    {
                        parent.InvalidateMeasure();
                    }
                }
            }
        }

        #endregion

        internal Layer Layer { get; private set; }

    }
}
