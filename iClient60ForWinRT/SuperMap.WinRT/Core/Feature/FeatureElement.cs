using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Rendering;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.Foundation;

namespace SuperMap.WinRT.Core
{
    internal class FeatureElement : Control
    {
        public static readonly DependencyProperty PathGeometryProperty = DependencyProperty.Register("PathGeometry", typeof(PathGeometry), typeof(FeatureElement), new PropertyMetadata(null,new PropertyChangedCallback(FeatureElement.OnPathGeometryPropertyChanged)));
        public PathGeometry PathGeometry
        {
            get
            {
                return (PathGeometry)base.GetValue(PathGeometryProperty);
            }
            set
            {
                base.SetValue(PathGeometryProperty, value);
            }
        }
        private static void OnPathGeometryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FeatureElement element = d as FeatureElement;
            if (element != null)
            {
                FrameworkElement pathChild = element.GetPathChild();
                if ((pathChild is Path) && (e.NewValue != null))
                {
                    ((Path)pathChild).Data = e.NewValue as Windows.UI.Xaml.Media.Geometry;
                }
                else if ((pathChild != null) && (e.NewValue != null))
                {
                    pathChild.Clip = e.NewValue as Windows.UI.Xaml.Media.RectangleGeometry;
                }
            }
        }

        private DispatcherTimer hoverTimer;
        private bool pathIsInvalid;
        private WeakReference feature;
        private PointerRoutedEventArgs lastEvent;
        protected internal bool isMouseOver;
        internal event EventHandler TemplateApplied;

        public FeatureElement(Feature f, IRenderer renderer)
        {
            this.ClipBox = Rectangle2D.Empty;
            this.pathIsInvalid = true;
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }
            this.feature = new WeakReference(f);

            ////暂时不加入聚合功能。
            //if (renderer != null && (f.GetValue(Clusterer.ClusterProperty) == null))
            if (renderer != null)
            {
                this.GeoStyle = renderer.GetStyle(f) ?? generateDefaultSyle(f);
            }//renderer的优先级高于Feature自我的
            else
            {
                this.GeoStyle = f.Style ?? generateDefaultSyle(f);
            }
            f.SetBoundedStyle(this.GeoStyle);

            if (this.GeoStyle != null)
            {
                base.Template = this.GeoStyle.ControlTemplate;
            }

            this.Geometry = f.Geometry;
        }

        private static Style generateDefaultSyle(Feature f)
        {
            if (f.Geometry is GeoPoint)
            {
                Style pmstyle = null;
                if (f.Layer.Map.Theme == null)
                {
                    pmstyle = new PredefinedMarkerStyle() { Color = new SolidColorBrush(Colors.Red), Size = 10 };
                }
                else
                {
                    pmstyle = new PredefinedMarkerStyle() { Color = f.Layer.Map.Theme.Color, Size = f.Layer.Map.Theme.Size };
                }
                return pmstyle;
            }
            else if (f.Geometry is GeoLine)
            {
                Style plstyle = null;
                if (f.Layer.Map.Theme == null)
                {
                    plstyle = new PredefinedLineStyle() { Stroke = new SolidColorBrush(Color.FromArgb(99, 255, 0, 0)), StrokeThickness = 2 };
                }
                else
                {
                    plstyle = new PredefinedLineStyle() { Stroke = f.Layer.Map.Theme.Stroke, StrokeThickness = f.Layer.Map.Theme.StrokeThickness };
                }
                return plstyle;
            }
            else
            {
                Style pfstyle = null;
                if (f.Layer.Map.Theme == null)
                {
                    pfstyle = new FillStyle() { Fill = new SolidColorBrush(Color.FromArgb(99, 255, 0, 0)) };
                }
                else
                {
                    pfstyle = new FillStyle() { Fill = f.Layer.Map.Theme.Fill, Stroke = f.Layer.Map.Theme.Stroke };
                }
                return pfstyle;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if ((this.Geometry == null) || (this.Geometry is GeoPoint))
            {
                return base.MeasureOverride(availableSize);
            }
            this.SetPath();
            Rectangle2D bounds = this.Geometry.Bounds;
            if (!Rectangle2D.IsNullOrEmpty(bounds))
            {
                return new Size(bounds.Width / this.Resolution, bounds.Height / this.Resolution);
            }
            return new Size(0.0, 0.0);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.ChangeVisualState(false);
            if (this.TemplateApplied != null)
            {
                this.TemplateApplied(this, new EventArgs());
            }
        }

        internal virtual void ChangeVisualState(bool useTransitions)
        {
            if (this.isMouseOver)
            {
                if (!this.GoToState(useTransitions, "MouseOver"))
                {
                    this.GoToState(useTransitions, "Normal");
                }
            }
            else
            {
                this.GoToState(useTransitions, "Normal");
            }
            if (this.Selected)
            {
                if (!this.GoToState(useTransitions, "Selected"))
                {
                    this.GoToState(useTransitions, "Unselected");
                }
            }
            else
            {
                this.GoToState(useTransitions, "Unselected");
            }
        }

        internal void ClearClip()
        {
            this.ClipBox = Rectangle2D.Empty;
            if (this.ClippedGeometry != null)
            {
                this.ClippedGeometry = null;
                this.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
            }
        }

        private void CreateHoverTimer()
        {
            this.hoverTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500.0) };
            this.hoverTimer.Tick += new EventHandler<object>(this.hoverTimer_Tick);
        }

        internal FrameworkElement GetChild(string name)
        {
            return (base.GetTemplateChild(name) as FrameworkElement);
        }

        internal FrameworkElement GetPathChild()
        {
            return this.GetChild("Element");//ControlTemplate中必须用Element
        }

        protected internal bool GoToState(bool useTransitions, string stateName)
        {
            return VisualStateManager.GoToState(this, stateName, useTransitions);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            this.FeatureElement_PointerEntered(this, e);
        }
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            this.FeatureElement_PointerExited(this, e);
        }
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            Feature f = this.Feature;
            if (f != null && f.Layer != null)
            {
                f.Layer.element_PointerPressed(this, e);
            }
        }
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            Feature f = this.Feature;
            if (f != null && f.Layer != null)
            {
                f.Layer.element_PointerReleased(this, e);
            }
        }
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            this.FeatureElement_PointerMoved(this, e);
        }

        protected virtual void FeatureElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.SetMouseOver(true);
            if (this.hoverTimer == null)
            {
                this.CreateHoverTimer();
                this.lastEvent = e;
            }
            this.hoverTimer.Start();

            Feature f = this.Feature;
            if (f != null && f.Layer != null)
            {
                f.Layer.element_PointerEntered(this, e);
            }
        }
        protected virtual void FeatureElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.SetMouseOver(false);
            this.lastEvent = null;
            if (this.hoverTimer != null)
            {
                this.hoverTimer.Stop();
            }
            Feature f = this.Feature;
            if (f != null && f.Layer != null)
            {
                f.Layer.element_PointerExited(this, e);
            }

        }
        private void FeatureElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if ((this.hoverTimer != null) && this.hoverTimer.IsEnabled)
            {
                this.lastEvent = e;
            }
            Feature f = this.Feature;
            if (f != null && f.Layer != null)
            {
                f.Layer.element_PointerMoved(this, e);
            }
        }

        private void hoverTimer_Tick(object sender, object e)
        {
            this.hoverTimer.Stop();
            if (this.lastEvent != null)
            {
                Feature f = this.Feature;
                if (f != null && f.Layer != null)
                {
                    f.Layer.element_PointerHover(this, f, this.lastEvent);
                }
            }
        }

        internal void InvalidatePath(double resolution, double originX, double originY)
        {
            this.pathIsInvalid = true;
            this.Resolution = resolution;
            this.OriginX = originX;
            this.OriginY = originY;
            base.InvalidateMeasure();
            base.InvalidateArrange();
        }

        protected Point MapPointToScreen(Point2D pt)
        {
            return CoordinateTransformationHelper.MapToScreen(pt, new Point2D(OriginX, OriginY), this.Resolution);
        }

        internal void SetClip(SuperMap.WinRT.Core.Geometry clippedGeometry, Rectangle2D clipbox)
        {
            this.ClippedGeometry = clippedGeometry;
            this.ClipBox = clipbox;
            this.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
        }

        internal void SetPath()
        {
            if (this.pathIsInvalid)
            {
                if (this.GetPathChild() != null)
                {
                    Geometry geometry = this.ClippedGeometry ?? this.Geometry;
                    if (geometry is GeoLine)
                    {
                        this.PathGeometry = this.BuildGeoLine((GeoLine)geometry);
                    }
                    else if (geometry is GeoRegion)
                    {
                        this.PathGeometry = this.BuildGeoRegion((GeoRegion)geometry);
                    }
                    else if (geometry is GeoCircle)
                    {
                        this.PathGeometry = this.BuildGeoCircle((GeoCircle)geometry);
                    }
                }
                this.pathIsInvalid = false;
            }
        }

        private PathGeometry BuildGeoRegion(SuperMap.WinRT.Core.GeoRegion geoRegion)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = null;
            Rectangle2D bounds = geoRegion.Bounds;
            if (!Rectangle2D.IsNullOrEmpty(bounds))
            {
                Point topLeft = this.MapPointToScreen(new Point2D(bounds.Left, bounds.Top));
                foreach (Point2DCollection points in geoRegion.Parts)
                {
                    if (points.Count >= 3)
                    {
                        figure = new PathFigure();
                        //是否要再加一次第一个点。
                        //figure.IsClosed = true;
                        Point point = this.MapPointToScreen(points[0]);
                        point.X -= topLeft.X;
                        point.Y -= topLeft.Y;
                        figure.StartPoint = point;
                        for (int i = 1; i < points.Count; i++)
                        {
                            point = this.MapPointToScreen(points[i]);
                            point.X -= topLeft.X;
                            point.Y -= topLeft.Y;
                            figure.Segments.Add(new LineSegment { Point = point });
                        }
                        geometry.Figures.Add(figure);
                    }
                }
            }
            geometry.Transform = new ScaleTransform();
            return geometry;
        }

        private PathGeometry BuildGeoCircle(SuperMap.WinRT.Core.GeoCircle geoRegion)
        {
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = null;
            Rectangle2D bounds = geoRegion.Bounds;
            if (!Rectangle2D.IsNullOrEmpty(bounds))
            {
                Point topLeft = this.MapPointToScreen(new Point2D(bounds.Left, bounds.Top));
                foreach (Point2DCollection points in geoRegion.Parts)
                {
                    if (points.Count >= 3)
                    {
                        figure = new PathFigure();
                        //是否要再加一次第一个点。
                        //figure.IsClosed = true;
                        Point point = this.MapPointToScreen(points[0]);
                        point.X -= topLeft.X;
                        point.Y -= topLeft.Y;
                        figure.StartPoint = point;
                        for (int i = 1; i < points.Count; i++)
                        {
                            point = this.MapPointToScreen(points[i]);
                            point.X -= topLeft.X;
                            point.Y -= topLeft.Y;
                            figure.Segments.Add(new LineSegment { Point = point });
                        }
                        figure.IsClosed = true;
                        geometry.Figures.Add(figure);
                    }
                }
            }
            geometry.Transform = new ScaleTransform();
            return geometry;
        }

        private PathGeometry BuildGeoLine(SuperMap.WinRT.Core.GeoLine geoLine)
        {
            PathGeometry geometry = new PathGeometry();
            Rectangle2D bounds = geoLine.Bounds;
            if (!Rectangle2D.IsNullOrEmpty(bounds))
            {
                Point topLeft = this.MapPointToScreen(new Point2D(bounds.Left, bounds.Top));
                foreach (Point2DCollection points in geoLine.Parts)
                {
                    if (points.Count >= 2)
                    {
                        PathFigure figure = new PathFigure();
                        Point point = this.MapPointToScreen(points[0]);
                        point.X -= topLeft.X;
                        point.Y -= topLeft.Y;
                        figure.StartPoint = point;
                        for (int i = 1; i < points.Count; i++)
                        {
                            point = this.MapPointToScreen(points[i]);
                            point.X -= topLeft.X;
                            point.Y -= topLeft.Y;
                            figure.Segments.Add(new LineSegment { Point = point });
                        }
                        geometry.Figures.Add(figure);
                    }
                }
            }
            geometry.Transform = new ScaleTransform();
            return geometry;
        }

        private void SetMouseOver(bool isOver)
        {
            this.isMouseOver = isOver;
            this.ChangeVisualState(true);
        }
        public Rectangle2D ClipBox { get; private set; }
        public SuperMap.WinRT.Core.Geometry ClippedGeometry { get; private set; }
        public SuperMap.WinRT.Core.Geometry Geometry { get; internal set; }

        internal double OriginY { get; private set; }
        internal double OriginX { get; private set; }
        internal double Resolution { get; private set; }

        private bool Selected
        {
            get
            {
                return (this.feature.IsAlive && ((bool)(this.feature.Target as Feature).GetValue(Feature.SelectedProperty)));
            }
        }

        internal Feature Feature
        {
            get
            {
                if (this.feature.IsAlive)
                {
                    return (this.feature.Target as Feature);
                }
                return null;
            }
        }

        internal SuperMap.WinRT.Core.Style GeoStyle { get; set; }
    }
}

