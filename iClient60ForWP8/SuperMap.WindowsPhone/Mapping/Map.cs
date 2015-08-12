using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Resources;
using SuperMap.WindowsPhone.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_Map_Title}</para>
    /// 	<para>${WP_mapping_Map_Description}</para>
    /// </summary>
    [TemplatePart(Name = "RootElement", Type = typeof(Grid))]
    [ContentProperty("Layers")]
    public sealed partial class Map : Control, INotifyPropertyChanged
    {
        internal static readonly DependencyProperty MapProperty = DependencyProperty.RegisterAttached("Map", typeof(Map), typeof(Map), null);
        internal static readonly DependencyProperty LastLayerViewBoundsProperty = DependencyProperty.RegisterAttached("LastLayerViewBounds", typeof(Rectangle2D), typeof(Map), new PropertyMetadata(Rectangle2D.Empty));

        private PanAnimation panHelper;

        private Grid rootElement;
        private Grid backgroundWindow;
        private Canvas transformCanvas;
        private Canvas layerCollectionContainer;
        private RotateTransform rotateTransform;

        /// <summary>
        /// 当分辨率发生变化时，记录上一次的分辨率。
        /// 暂时作为多点缩放时的辅助字段，后续一起迁移到Action中。
        /// </summary>
        private double oldResolution;

        private double mapResolution;
        private double[] resolutions;
        private double maxResolution;
        private double minResolution;
        private bool isMinResolutionSet;
        private bool isMaxResolutionSet;

        private double[] oldScales;

        private Size currentSize;

        private Point2D origin;
        private Rectangle2D cacheViewBounds;//第一次
        private Rectangle2D previousViewBounds;//前一视图

        private EventHandler CurrentZoomAnimationHandler;
        private double targetResolution;

        private Rectangle2D designViewBounds = Rectangle2D.Empty;
        private bool isDesignMode;

        /// <summary>${WP_mapping_Map_event_progress_D}</summary>
        public event EventHandler<ProgressEventArgs> Progress;
        /// <summary>${WP_mapping_Map_event_viewBoundsChanging_D}</summary>
        public event EventHandler<ViewBoundsEventArgs> ViewBoundsChanging;
        /// <summary>${WP_mapping_Map_event_viewBoundsChanged_D}</summary>
        public event EventHandler<ViewBoundsEventArgs> ViewBoundsChanged;

        /// <summary>${WP_mapping_Map_event_ScalesChanged_D}</summary>
        public event EventHandler<ScalesEventArgs> ScalesChanged;

        /// <summary>${WP_mapping_Map_event_ResolutionsChanged_D}</summary>
        public event EventHandler<ResolutionsEventArgs> ResolutionsChanged;

        /// <summary>${WP_mapping_Map_event_ZoomEffectEnabled_D}</summary>
        public bool ZoomEffectEnabled { get; set; }


        /// <summary>${WP_mapping_Map_attribute_isDoubleTap_D}</summary>
        public bool IsDoubleTap
        {
            get
            {
                return (bool)base.GetValue(ISDoubleTapProperty);
            }
            set
            {
                base.SetValue(ISDoubleTapProperty, value);
            }
        }
        /// <summary>${WP_mapping_Map_attribute_field_isDoubleTapProperty_D}</summary>
        public static readonly DependencyProperty ISDoubleTapProperty = DependencyProperty.Register("IsDoubleTap", typeof(bool), typeof(Map), new PropertyMetadata(true));


        private ZoomEffect zoomEffect;

        /// <summary>${WP_mapping_Map_attribute_zoomFactor_D}</summary>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }
        /// <summary>${WP_mapping_Map_attribute_field_zoomFactorProperty_D}</summary>
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(Map), new PropertyMetadata(2.0));

        /// <summary>${WP_mapping_Map_attribute_panFactor_D}</summary>
        public double PanFactor
        {
            get { return (double)GetValue(PanFactorProperty); }
            set { SetValue(PanFactorProperty, value); }
        }
        /// <summary>${WP_mapping_Map_attribute_field_panFactorProperty_D}</summary>
        public static readonly DependencyProperty PanFactorProperty =
            DependencyProperty.Register("PanFactor", typeof(double), typeof(Map), new PropertyMetadata(0.1));


        /// <summary>${WP_mapping_Map_constructor_None_D}</summary>
        public Map()
        {
            ZoomEffectEnabled = true;
            Theme = null;
            Metadata = new Dictionary<string, string>();
            mapResolution = double.NaN;
            maxResolution = double.MaxValue;
            minResolution = double.Epsilon;
            maxScale = double.MaxValue;
            minScale = double.Epsilon;
            origin = Point2D.Empty;
            previousViewBounds = Rectangle2D.Empty;
            cacheViewBounds = Rectangle2D.Empty;
            currentSize = new Size(0, 0);
            base.DefaultStyleKey = typeof(Map);

            Layers = new LayerCollection();
            Popup = new Popup();

            base.Loaded += new RoutedEventHandler(Map_Loaded);

        }


        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            isDesignMode = DesignerProperties.GetIsInDesignMode(this);
        }

        /// <summary>${WP_mapping_Map_method_ArrangeOverride_D}</summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.UpdateClip(finalSize);
            return base.ArrangeOverride(finalSize);
        }
        /// <summary>${WP_mapping_Map_method_onApplyTemplate_D}</summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if ((this.rootElement != null) && this.rootElement.Children.Contains(this.Popup))
            {
                this.rootElement.Children.Remove(this.Popup);
            }//Map变化
            this.rootElement = base.GetTemplateChild("RootElement") as Grid;
            if (this.rootElement == null)
            {
                throw new ArgumentException(ExceptionStrings.RootElementIsNull);
            }
            Uri source = null;

            if (((!this.isDesignMode && (source != null)) && (source.IsAbsoluteUri && !source.Scheme.Equals(Uri.UriSchemeHttp))) && !source.Scheme.Equals(Uri.UriSchemeHttps))
            {
                this.CreateErrorMessage(ExceptionStrings.SchemeError);
            }
            else
            {
                this.backgroundWindow = new Grid();
                this.backgroundWindow.Background = new SolidColorBrush(Color.FromArgb(0, 0xff, 0xff, 0xff));
                this.rootElement.Children.Add(this.backgroundWindow);

                this.rotateTransform = new RotateTransform() { Angle = this.Angle };
                this.transformCanvas = new Canvas()
                {
                    RenderTransform = this.rotateTransform,
                    RenderTransformOrigin = new Point(0.5, 0.5)
                };
                this.rootElement.Children.Add(this.transformCanvas);
                this.layerCollectionContainer = new Canvas();
                this.transformCanvas.Children.Add(this.layerCollectionContainer);
                if (base.FlowDirection == FlowDirection.RightToLeft)
                {
                    this.transformCanvas.HorizontalAlignment = HorizontalAlignment.Right;
                }

                this.panHelper = new PanAnimation(this.layerCollectionContainer);
                this.panHelper.PanAnimationCompleted += this.panHelper_PanAnimationCompleted;
                this.panHelper.Panning += this.panHelper_Panning;

                base.SizeChanged += new SizeChangedEventHandler(this.Map_SizeChanged);

                BuildMapAction();//在MapActionPart.cs中

                //添加缩放效果
                if (ZoomEffectEnabled)
                {
                    zoomEffectContainer = new Canvas();
                    zoomEffect = new ZoomEffect();
                    zoomEffectContainer.Children.Add(zoomEffect);
                    rootElement.Children.Add(zoomEffectContainer);
                }

                ScreenContainer = new Canvas();
                rootElement.Children.Add(ScreenContainer);
                this.rootElement.Children.Add(this.Popup);

                if ((this.Layers != null) && (this.Layers.Count > 0))
                {
                    if (!this.Layers.HasPendingLayers)
                    {
                        this.Layers_LayersInitialized(this.Layers, null);
                    }
                    else
                    {
                        foreach (Layer layer in this.Layers)
                        {
                            if (!layer.IsInitialized)
                            {
                                layer.Initialize();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>${WP_mapping_Map_attribute_ScreenContainer_D}</summary>
        public Canvas ScreenContainer { get; private set; }

        //添加缩放动画的容器
        private Canvas zoomEffectContainer;


        //获取下一级别的Resolution或Scale:限制了Level，则获取下一个Level对应的尺度；
        //若没有，则返回缩放2倍（ZoomFactor可调整）后的尺度
        //plus表示放大true还是缩小false
        /// <summary>${WP_mapping_Map_method_getNextResolution_D}</summary>
        /// <returns>${WP_mapping_Map_method_getNextResolution_returns}</returns>
        /// <param name="plus">${WP_mapping_Map_method_getNextResolution_param_plus}</param>
        public double GetNextResolution(bool plus)
        {
            double newResolution;
            int level = this.Level;
            if (level != -1)
            {
                if (plus)
                {
                    if (level < this.Resolutions.Length - 1)
                    {
                        level++;
                    }
                }
                else
                {
                    if (level > 0)
                    {
                        level--;
                    }
                }
                newResolution = this.Resolutions[level];
            }
            else
            {
                if (plus)
                {
                    newResolution = this.targetResolution / ZoomFactor;
                }
                else
                {
                    newResolution = this.targetResolution * ZoomFactor;
                }
            }

            //这里有问题,当接近时就不能缩放了
            //if (DoubleUtil.AreClose(newResolution, this.Resolution) || newResolution < MinResolution || newResolution > MaxResolution)
            if (newResolution < MinResolution || newResolution > MaxResolution)
            {
                return this.Resolution;
            }
            else
            {
                return newResolution;
            }
        }
        /// <summary>${WP_mapping_Map_method_getNextScale_D}</summary>
        /// <returns>${WP_mapping_Map_method_getNextScale_returns}</returns>
        /// <param name="plus">${WP_mapping_Map_method_getNextScale_param_plus}</param>
        public double GetNextScale(bool plus)
        {
            if (this.Dpi != 0.0)
            {
                double newRes = this.GetNextResolution(plus);
                return ScaleHelper.ScaleConversion(newRes, this.Dpi, this.CRS);
            }
            else
            {
                return double.NaN;
            }
        }


        /// <summary>${WP_mapping_Map_method_panByPixel_D}</summary>
        /// <param name="pixelX">${WP_mapping_Map_method_panByPixel_param_pixelX}</param>
        /// <param name="pixelY">${WP_mapping_Map_method_panByPixel_param_pixelY}</param>
        public void PanByPixel(double pixelX, double pixelY)
        {
            Pan(Resolution * pixelX, -Resolution * pixelY);
        }

        //用ElementsLayer添加不用管缩放问题；
        //用ScreenLayer需要处理缩放？平移？还可以处理避让？  
        //TODO:当在地图边缘的时候可适当移动地图
        //缩放过程中没有，完毕后有；平移一直有
        //边缘规避esri
        #region InfoWindow
        private ElementsLayer windowsLayer;
        /// <summary>${WP_mapping_Map_method_openInfoWindow_D}</summary>
        /// <param name="location">${WP_mapping_Map_method_openInfoWindow_param_location}</param>
        /// <param name="element">${WP_mapping_Map_method_openInfoWindow_param_element}</param>
        public void OpenInfoWindow(Point2D location, UIElement element)
        {
            this.OpenInfoWindow(location, 0, 0, element);
        }
        /// <summary>${WP_mapping_Map_method_openInfoWindow_D}</summary>
        ///  <param name="location">${WP_mapping_Map_method_openInfoWindow_param_location}</param>
        ///  <param name="offsetPixelX">${WP_mapping_Map_method_openInfoWindow_param_offsetPixelX}</param>
        ///  <param name="offsetPixelY">${WP_mapping_Map_method_openInfoWindow_param_offsetPixelY}</param>
        /// <param name="element">${WP_mapping_Map_method_openInfoWindow_param_element}</param>
        public void OpenInfoWindow(Point2D location, double offsetPixelX, double offsetPixelY, UIElement element)
        {
            if (Layers.Count == 0)
            {
                return;
            }//待补充其他异常消除
            if (windowsLayer == null)
            {
                windowsLayer = new ElementsLayer();
                Layers.Add(windowsLayer);
            }
            windowsLayer.Children.Clear();
            Point originPixel = this.MapToScreen(location);
            Point offsetPixel = new Point(originPixel.X + offsetPixelX, originPixel.Y + offsetPixelY);
            Point2D offsetLocation = this.ScreenToMap(offsetPixel);
            windowsLayer.AddChild(element, offsetLocation);
        }

        /// <summary>${WP_mapping_Map_method_closeInfoWindow_D}</summary>
        public void CloseInfoWindow()
        {
            if (windowsLayer != null)
            {
                Layers.Remove(windowsLayer);
                windowsLayer = null;
            }
        }

        #endregion

        private void AssignLayerContainer(Layer layer)
        {
            if (layer.Container.Parent == null && layer.IsInitialized && layer.Error == null)
            {
                layer.Container.OriginX = this.origin.X;
                layer.Container.OriginY = this.origin.Y;
                layer.Container.Resolution = this.mapResolution;
                this.InsertLayerContainer(layer);
            }
        }
        private void InsertLayerContainer(Layer layer)
        {
            int index = 0;
            int num = Layers.IndexOf(layer);
            if (num > 0)
            {
                for (int i = num - 1; i >= 0; i--)
                {
                    Layer layer2 = this.Layers[i];
                    if ((layer2.Container != null) && (layer2.Container.Parent == this.layerCollectionContainer))
                    {
                        index = this.layerCollectionContainer.Children.IndexOf(layer2.Container) + 1;
                        break;
                    }
                }
            }
            layerCollectionContainer.Children.Insert(index, layer.Container);
        }

        private void beginZoomToTargetBounds(Rectangle2D targetBounds, bool skipAnimation)
        {
            if (this.Layers == null)
            {
                return;
            }
            Rectangle2D startBounds = this.ViewBounds;
            if (Rectangle2D.IsNullOrEmpty(this.previousViewBounds))
            {
                this.previousViewBounds = startBounds;
            }

            double startResolution = this.mapResolution;
            if (((targetBounds.Width != 0.0) || (targetBounds.Height != 0.0)) && (!Rectangle2D.IsNullOrEmpty(targetBounds)))
            {
                //this.targetResolution = this.ClipResolution(Math.Max((double)(targetBounds.Width / viewSize.Width), (double)(targetBounds.Height / viewSize.Height)));
                this.targetResolution = Math.Max((double)(targetBounds.Width / currentSize.Width), (double)(targetBounds.Height / currentSize.Height));
                this.targetResolution = MathUtil.MinMaxCheck(targetResolution, MinResolution, MaxResolution);
                Point2D startOrigin = new Point2D(startBounds.Left, startBounds.Top);
                Point2D center = targetBounds.Center;
                if ((base.ActualHeight == 0.0) || (base.ActualWidth == 0.0))
                {
                    skipAnimation = true;
                }

                Point2D targetOrigin = new Point2D(targetBounds.Left, targetBounds.Top);

                this.Popup.IsOpen = false;

                if (this.CurrentZoomAnimationHandler != null)
                {
                    CompositionTarget.Rendering -= this.CurrentZoomAnimationHandler;
                    this.CurrentZoomAnimationHandler = null;
                }

                skipAnimation = skipAnimation || (this.ZoomDuration.Ticks == 0L);
                if (skipAnimation)
                {
                    this.panHelper.ResetTranslate();
                    this.SetOriginAndResolution(this.targetResolution, targetOrigin, true);
                }
                else
                {
                    TimeSpan? startTime = null;
                    bool resetPan = false;
                    this.CurrentZoomAnimationHandler = delegate(object s, EventArgs e)
                    {
                        RenderingEventArgs args = (RenderingEventArgs)e;
                        if (!startTime.HasValue)
                        {
                            startTime = new TimeSpan?(args.RenderingTime);
                        }
                        else
                        {
                            double t = args.RenderingTime.TotalMilliseconds - startTime.Value.TotalMilliseconds;
                            double totalMilliseconds = this.ZoomDuration.TotalMilliseconds;
                            double num3 = QuinticEaseOut(t, 0.0, 1.0, totalMilliseconds);//动画
                            this.mapResolution = ((this.targetResolution - startResolution) * num3) + startResolution;
                            Point2D currentOrigin = new Point2D(((targetOrigin.X - startOrigin.X) * num3) + startOrigin.X, ((targetOrigin.Y - startOrigin.Y) * num3) + startOrigin.Y);
                            if (t >= totalMilliseconds)
                            {
                                CompositionTarget.Rendering -= this.CurrentZoomAnimationHandler;
                                this.CurrentZoomAnimationHandler = null;
                                currentOrigin = targetOrigin;
                                this.mapResolution = this.targetResolution;

                                this.Dispatcher.BeginInvoke(delegate
                                {
                                    this.SetOriginAndResolution(this.mapResolution, currentOrigin, true);
                                    Rectangle2D rect = this.GetFullViewBounds();
                                    if (Rectangle2D.IsNullOrEmpty(rect))
                                    {
                                        this.LoadLayersInView(false, targetBounds);
                                    }
                                    else
                                    {
                                        this.LoadLayersInView(targetBounds.IntersectsWith(rect), rect);
                                    }
                                    this.RaiseViewBoundsChanged();
                                });
                            }
                            else
                            {
                                if (!resetPan)
                                {
                                    this.panHelper.ResetTranslate();
                                    resetPan = true;
                                }
                                this.SetOriginAndResolution(this.mapResolution, currentOrigin, false);
                                if (this.ViewBoundsChanging != null)
                                {
                                    this.ViewBoundsChanging(this, new ViewBoundsEventArgs(startBounds, this.ViewBounds));
                                }
                            }
                        }
                    };

                    CompositionTarget.Rendering += this.CurrentZoomAnimationHandler;
                }
                foreach (Layer layer in this.Layers)
                {
                    if ((layer.Container != null) && !(layer is TiledLayer))
                    {
                        layer.Container.MarkOutdated(!skipAnimation);
                    }
                }

                if (this.mapResizeThrottler != null)
                {
                    this.mapResizeThrottler.Cancel();
                }
                if (skipAnimation)
                {
                    this.LoadLayersInView(false, targetBounds);
                    this.RaiseViewBoundsChanged();
                }
            }
        }

        private void CalculateStartViewBounds(Size size)
        {
            if (!Rectangle2D.IsNullOrEmpty(cacheViewBounds) && Point2D.IsNullOrEmpty(origin))
            {
                Rectangle2D cacheBounds = cacheViewBounds;
                Point currentOffset = this.panHelper.GetCurrentOffset();
                double x = currentOffset.X;
                double y = currentOffset.Y;
                double width = size.Width;
                double height = size.Height;
                double resWidth = cacheBounds.Width / width;
                double resHeight = cacheBounds.Height / height;
                mapResolution = (resHeight > resWidth) ? resHeight : resWidth;

                double nearestLevelResolution = MathUtil.GetNearest(this.mapResolution, this.Resolutions, MinResolution, MaxResolution);
                mapResolution = nearestLevelResolution;
                targetResolution = this.mapResolution = MathUtil.MinMaxCheck(this.mapResolution, MinResolution, MaxResolution);
                //origin = new Point2D(bounds.Left + bounds.Width * 0.5 - width * 0.5 * mapResolution, bounds.Top - bounds.Height * 0.5 + height * 0.5 * mapResolution);
                origin = new Point2D(cacheBounds.Center.X - width * 0.5 * mapResolution, cacheBounds.Center.Y + height * 0.5 * mapResolution);
                //地图的中心 加 减 ViewSize大小乘以分辨率 即 初始化Origin
                cacheViewBounds = ViewBounds;//由origin和resolution重新算出来的VB赋给cacheVB，这是真正的VB
                RaiseViewBoundsChanged();
            }
        }

        #region TooTip@FeaturesLayer
        internal void CloseToolTip()
        {
            this.Popup.IsOpen = false;
        }
        internal void ShowToolTip()
        {
            this.Popup.IsOpen = true;
        }
        #endregion

        private void CreateErrorMessage(string message)
        {
            Grid grid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Border border = new Border
            {
                Background = new SolidColorBrush(Colors.LightGray),
                CornerRadius = new CornerRadius(10.0),
                Padding = new Thickness(10.0),
                Child = new TextBlock { Text = message }
            };
            grid.Children.Add(border);
            this.rootElement.Children.Add(grid);
        }

        private Rectangle2D GetFullViewBounds()
        {
            if (double.IsNaN(this.mapResolution))
            {
                return Rectangle2D.Empty;
            }
            Point2D point = ScreenToMap(new Point(0.0, 0.0));
            Point2D point2 = ScreenToMap(new Point(0.0, currentSize.Height));
            Point2D point3 = ScreenToMap(new Point(currentSize.Width, currentSize.Height));
            Point2D point4 = ScreenToMap(new Point(currentSize.Width, 0.0));
            if (Point2D.IsNullOrEmpty(point) || Point2D.IsNullOrEmpty(point2) || Point2D.IsNullOrEmpty(point3) || Point2D.IsNullOrEmpty(point4))
            {
                return Rectangle2D.Empty;
            }
            //TODO:直接union点
            Rectangle2D pointBounds = new Rectangle2D(point, point);
            Rectangle2D point2Bounds = new Rectangle2D(point2, point2);
            Rectangle2D point3Bounds = new Rectangle2D(point3, point3);
            Rectangle2D point4Bounds = new Rectangle2D(point4, point4);
            Rectangle2D bounds = pointBounds;
            bounds = bounds.Union(point2Bounds);
            bounds = bounds.Union(point3Bounds);
            bounds = bounds.Union(point4Bounds);
            return bounds;
        }

        private void layer_Initialized(object sender, EventArgs e)
        {
            if ((!Rectangle2D.IsNullOrEmpty(this.ViewBounds)) && (this.panHelper != null) && (this.CurrentZoomAnimationHandler == null))
            {
                if ((currentSize.Height > 0.0) && (currentSize.Width > 0.0))
                {
                    this.CalculateStartViewBounds(currentSize);
                    if ((!Rectangle2D.IsNullOrEmpty(this.ViewBounds)) && !double.IsNaN(this.mapResolution))
                    {
                        this.LoadLayerInView(false, this.ViewBounds, sender as Layer);
                    }
                }
            }
        }
        private void layer_OnLayerChanged(object sender, EventArgs args)
        {
            if ((Point2D.IsNullOrEmpty(this.origin) && (!Rectangle2D.IsNullOrEmpty(this.cacheViewBounds) || Rectangle2D.IsNullOrEmpty(this.ViewBounds))) && (double.IsNaN(this.mapResolution) && (currentSize.Width > 0.0)))
            {
                if (this.Layers != null && !this.Layers.HasPendingLayers)
                {
                    this.Layers_LayersInitialized(sender, args);
                }
            }
            else
            {
                (sender as Layer).ClearValue(LastLayerViewBoundsProperty);
                Rectangle2D bounds = this.GetFullViewBounds();
                if (!Rectangle2D.IsNullOrEmpty(bounds))
                {
                    this.LoadLayerInView(false, bounds, sender as Layer);
                }
            }
        }

        //当清空所有图层时,把Map的各项参数恢复默认值。
        private void ResetMapStatus()
        {
            origin = Point2D.Empty;//保证了 ViewBounds = Rectangle2D.Empty;
            mapResolution = targetResolution = double.NaN;
            cacheViewBounds = Rectangle2D.Empty;//从world到长春
            previousViewBounds = Rectangle2D.Empty;
            if (this.panHelper != null)
            {
                this.panHelper.ResetTranslate();
            }
            CRS = null;
            resolutions = null;
            scales = null;
            maxResolution = double.MaxValue;
            minResolution = double.Epsilon;

            maxScale = double.MaxValue;
            minScale = double.Epsilon;

            CloseToolTip();
            fromLayers = false;
            hasChanged = false;

            RaiseViewBoundsChanged();
        }

        private List<Layer> layers;
        private void Layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList oldItems = e.OldItems;
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {

                oldItems = this.layers;
                ResetMapStatus();
                //但从 road 到 aerial 会怎么样呢? 请用Replace
            }
            //if (e.Action == NotifyCollectionChangedAction.Replace)
            //{
            //    origin = Point2D.Empty;
            //    cacheViewBounds = Rectangle2D.Empty;
            //}//如果仅仅替换图层，其他已有图层的坐标也不对，所以不会发生该种情况
            layers = new List<Layer>(this.Layers);
            if (oldItems != null)
            {
                foreach (object obj2 in oldItems)
                {
                    Layer layer = obj2 as Layer;
                    if (layer != null)
                    {
                        RemoveMapLayers(layer);
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (object obj4 in e.NewItems)
                {
                    Layer layer2 = obj4 as Layer;
                    if (layer2 != null)
                    {
                        Map map = layer2.GetValue(MapProperty) as Map;
                        if ((map != null) && (map != this))
                        {
                            throw new ArgumentException(ExceptionStrings.LayerToMap);
                        }
                        if (map == null)
                        {
                            layer2.SetValue(MapProperty, this);
                        }
                        layer2.LayerChanged += new Layer.LayerChangedHandler(this.layer_OnLayerChanged);
                        layer2.Initialized += new EventHandler<EventArgs>(this.layer_Initialized);
                        if (!layer2.IsInitialized && (this.panHelper != null))
                        {
                            layer2.Initialize();
                        }
                    }
                }
            }
            bool flag = true;
            foreach (Layer layer3 in this.Layers)
            {
                if (!layer3.IsInitialized)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                this.Layers_LayersInitialized(sender, null);
            }
        }

        private void RemoveMapLayers(Layer _layer)
        {
            _layer.LayerChanged -= new Layer.LayerChangedHandler(this.layer_OnLayerChanged);
            _layer.Initialized -= new EventHandler<EventArgs>(this.layer_Initialized);
            _layer.CancelLoad();
            _layer.Container.Children.Clear();
            if ((this.layerCollectionContainer != null) && this.layerCollectionContainer.Children.Contains(_layer.Container))
            {
                this.layerCollectionContainer.Children.Remove(_layer.Container);
            }
            _layer.ClearValue(MapProperty);//用的都是Clear
            _layer.ClearValue(LastLayerViewBoundsProperty);

            if (((_layer is FeaturesLayer) && (this.Popup.Child != null)) && ((_layer as FeaturesLayer).ToolTip == this.Popup.Child))
            {
                this.CloseToolTip();
            }
        }
        private void Layers_LayersInitialized(object sender, EventArgs args)
        {
            if (this.panHelper != null)
            {
                hasChanged = true;

                if (Point2D.IsNullOrEmpty(origin))
                {
                    Rectangle2D bounds = this.Bounds;
                    if (Rectangle2D.IsNullOrEmpty(bounds))
                    {
                        return;//只有一个图层的时候，要求必需有bounds
                    }
                    if (!Rectangle2D.IsNullOrEmpty(ViewBounds))
                    {
                        bounds = ViewBounds;
                    }
                    cacheViewBounds = bounds;//赋给cacheVB
                    if ((currentSize.Width == 0.0) || (currentSize.Height == 0.0))
                    {
                        base.Dispatcher.BeginInvoke(delegate { this.Layers_LayersInitialized(sender, args); });
                        return;
                    }
                }

                //比如用TiledDynamicLayer 也没设置scalse，却只设置了MaxScale和MinScale,那么也设下最大最小吧
                if (isMaxScaleSet && !isMinResolutionSet)
                {
                    this.MinResolution = ScaleHelper.ScaleConversion(this.MaxScale, this.Dpi, this.CRS);
                }

                if (isMinScaleSet && !isMaxResolutionSet)
                {
                    this.MaxResolution = ScaleHelper.ScaleConversion(this.MinScale, this.Dpi, this.CRS);
                }


                //放到scales的Set里? 在那里时，dpi基本为0，所以这里还得设置
                if (this.Scales != null)//不是后设的时候,优先级高
                {
                    fromLayers = false;//用了scales说明肯定不是从layer获取
                    if (this.Dpi != 0.0)    //TieldWMS用SuperMap时无法知道dpi，所以即便设置了scales也没用
                    {
                        this.Resolutions = ScaleHelper.ConversionBetweenScalesAndResulotions(this.Scales, this.Dpi, this.CRS);

                        if (isMaxScaleSet)
                        {
                            this.MinResolution = ScaleHelper.ScaleConversion(this.MaxScale, this.Dpi, this.CRS); ;
                        }

                        if (isMinScaleSet)
                        {
                            this.MaxResolution = ScaleHelper.ScaleConversion(this.MinScale, this.Dpi, this.CRS); ;
                        }
                    }
                }

                if (this.Resolutions != null)
                {
                    if (!this.isMinResolutionSet)
                    {
                        this.MinResolution = this.Resolutions[this.Resolutions.Length - 1];
                    }
                    if (!this.isMaxResolutionSet)
                    {
                        this.MaxResolution = this.Resolutions[0];
                    }
                }

                this.CalculateStartViewBounds(currentSize);

                foreach (Layer layer in this.Layers)
                {
                    this.AssignLayerContainer(layer);
                }

                if ((this.Resolutions != null) && (((this.Resolution < this.MinResolution) || (this.Resolution > this.MaxResolution))))
                {
                    double res = MathUtil.GetNearest(this.mapResolution, this.Resolutions, MinResolution, MaxResolution);
                    if (res != this.mapResolution)
                    {
                        this.ZoomToResolution(res);
                    }
                }
                else
                {
                    this.LoadLayersInView(false, this.GetFullViewBounds());
                }
                oldScales = this.Scales;
                oldResolutions = this.Resolutions;
            }

        }
        //旋转时，panning时，用到该函数，并不需要重新全部载入图片，有的tile可重用
        private void LoadContinuousLayersInView(bool useTransitions)
        {
            Rectangle2D bounds = this.GetFullViewBounds();
            if (this.Layers != null && !Rectangle2D.IsNullOrEmpty(bounds))
            {
                foreach (Layer layer in this.Layers)
                {
                    if (layer.ContinuousDraw)
                    {
                        this.LoadLayerInView(useTransitions, bounds, layer);
                    }//TiledLayer和FeaturesLayer ?ElementsLayer?
                }
            }
        }
        private void LoadLayerInView(bool useTransitions, Rectangle2D drawBounds, Layer layer)
        {
            if (!Rectangle2D.IsNullOrEmpty(drawBounds) && layer.IsInitialized && (layer.Error == null)
      && (layer.MinVisibleResolution <= mapResolution) && (layer.MaxVisibleResolution >= mapResolution)
      && layer.IsVisible && Layers.Contains(layer) && CoordinateReferenceSystem.Equals(layer.CRS, CRS, true))
            {
                Rectangle2D last = (Rectangle2D)(layer.GetValue(LastLayerViewBoundsProperty));
                if (Rectangle2D.IsNullOrEmpty(last) || !drawBounds.Equals(last))
                {
                    layer.Draw(new DrawParameter()
                    {
                        UseTransitions = useTransitions,
                        Resoluitons = Resolutions,
                        Resolution = targetResolution,
                        ViewBounds = drawBounds,
                        ViewSize = currentSize,
                        LayerOrigin = layer.Container.Origin
                    });
                    layer.SetValue(LastLayerViewBoundsProperty, drawBounds);
                }
            }

        }
        private void LoadLayersInView(bool useTransitions, Rectangle2D drawBounds)
        {
            if ((this.Layers != null) && (this.CurrentZoomAnimationHandler == null) && (!Rectangle2D.IsNullOrEmpty(drawBounds)))
            {
                foreach (Layer layer in this.Layers)
                {
                    //在这里控制的话就没有考虑到平移等情况，这个判断应该放到唯一的入口！
                    //if (CoordinateReferenceSystem.Equals(layer.CRS, CRS, true))
                    //{
                    this.LoadLayerInView(useTransitions, drawBounds, layer);
                    //}
                }
            }

        }
        internal bool isChanged { get; set; }
        private ThrottleTimer mapResizeThrottler;
        private void Map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            currentSize = e.NewSize;

            ScreenContainer.Width = e.NewSize.Width;
            ScreenContainer.Height = e.NewSize.Height;
            if (this.mapResizeThrottler == null)
            {
                this.mapResizeThrottler = new ThrottleTimer(250, delegate
                {
                    if (this.isDesignMode)
                    {
                        this.zoomTo(this.designViewBounds, true);
                    }
                    else
                    {
                        this.LoadLayersInView(false, this.GetFullViewBounds());
                    }
                    this.RaiseViewBoundsChanged();
                });
            }
            this.mapResizeThrottler.Invoke();
        }

        private Point mapToPanLayer(Point2D pt)
        {
            return CoordinateTransformationHelper.MapToScreen(pt, this.origin, this.mapResolution);
        }
        /// <summary>${WP_mapping_Map_method_mapToScreen_D}</summary>
        /// <returns>${WP_mapping_Map_method_mapToScreen_return}</returns>
        /// <param name="point2D">${WP_mapping_Map_method_mapToScreen_param_point2D}</param>
        public Point MapToScreen(Point2D point2D)
        {
            if ((this.layerCollectionContainer == null) || (Point2D.IsNullOrEmpty(point2D)))
            {
                return new Point(double.NaN, double.NaN);
            }
            try
            {
                return this.layerCollectionContainer.TransformToVisual(this).Transform(this.mapToPanLayer(point2D));
            }
            catch
            {
                return new Point(double.NaN, double.NaN);
            }
        }

        private void panHelper_PanAnimationCompleted(object sender, EventArgs e)
        {
            LoadLayersInView(false, GetFullViewBounds());
            RaiseViewBoundsChanged();
        }

        private void panHelper_Panning(object sender, EventArgs e)
        {
            LoadContinuousLayersInView(false);

            if (ViewBoundsChanging != null)
            {
                ViewBoundsChanging(this, new ViewBoundsEventArgs(previousViewBounds, ViewBounds));
            }
            this.raisePropertyChanged("ViewBounds");
        }

        private Point2D panLayerToMap(Point pnt)
        {
            if ((!Point2D.IsNullOrEmpty(this.origin)) && !double.IsNaN(this.mapResolution))
            {
                return new Point2D((pnt.X * this.mapResolution) + this.origin.X, this.origin.Y - (pnt.Y * this.mapResolution));
            }
            return Point2D.Empty;
        }


        /// <summary>${WP_mapping_Map_method_panTo_D}</summary>
        /// <param name="center">${WP_mapping_Map_method_panTo_param_center}</param>
        public void PanTo(Point2D center)
        {
            panTo(center, false);
        }

        private void panTo(Point2D center, bool skipAnimation)
        {
            panTo(new Rectangle2D(center, center), skipAnimation);
        }

        private void panTo(Rectangle2D bounds)
        {
            panTo(bounds, false);
        }

        private void panTo(Rectangle2D bounds, bool skipAnimation)
        {
            //if (!ConstrainedViewBounds.IsEmpty)
            //{
            //    bounds.Intersect(ConstrainedViewBounds);
            //}

            if ((this.panHelper != null) && (!Rectangle2D.IsNullOrEmpty(bounds)))
            {
                if (Popup.IsOpen)
                {
                    Popup.IsOpen = false;
                }
                this.panHelper.Stop(true);
                Point2D center = bounds.Center;
                Rectangle2D viewBounds = this.ViewBounds;
                if (Rectangle2D.IsNullOrEmpty(previousViewBounds))
                {
                    previousViewBounds = viewBounds;
                }
                if (!Rectangle2D.IsNullOrEmpty(viewBounds))
                {
                    if (!skipAnimation)
                    {
                        Rectangle2D rect2D = viewBounds.Inflate(viewBounds.Width, viewBounds.Height);
                        skipAnimation = !rect2D.Contains(center);
                    }
                    if (!skipAnimation)
                    {
                        Point2D point2 = viewBounds.Center;
                        this.panHelper.DeltaPan((int)Math.Round((double)((point2.X - center.X) / this.mapResolution)), (int)Math.Round((double)((center.Y - point2.Y) / this.mapResolution)), this.PanDuration, false);
                        if (this.PanDuration.Ticks == 0L)
                        {
                            this.RaiseViewBoundsChanged();
                        }//等于0，不动画。//在动画里和动画结束已经触发。
                    }
                    else
                    {
                        Point2D currentOrigin = new Point2D(center.X - (viewBounds.Width * 0.5), center.Y + (viewBounds.Height * 0.5));
                        this.SetOriginAndResolution(this.mapResolution, currentOrigin, true);
                        this.panHelper.ResetTranslate();
                        this.origin = currentOrigin;
                        this.LoadLayersInView(false, this.GetFullViewBounds());
                        this.RaiseViewBoundsChanged();
                    }
                }
            }
        }

        private static double QuinticEaseOut(double t, double b, double c, double d)
        {
            t /= d;
            t--;
            return ((-c * ((((t * t) * t) * t) - 1.0)) + b);
        }

        private void RaiseViewBoundsChanged()
        {
            if (this.mapResizeThrottler != null)
            {
                this.mapResizeThrottler.Cancel();
            }
            EventHandler<ViewBoundsEventArgs> viewBoundsChanged = this.ViewBoundsChanged;
            if (viewBoundsChanged != null)
            {
                Rectangle2D viewBounds = this.ViewBounds;
                if (!Rectangle2D.IsNullOrEmpty(viewBounds) && this.previousViewBounds != viewBounds)
                {
                    viewBoundsChanged(this, new ViewBoundsEventArgs(this.previousViewBounds, viewBounds));
                    this.raisePropertyChanged("ViewBounds");
                }
                this.previousViewBounds = viewBounds;
                this.cacheViewBounds = viewBounds;
            }

        }

        private Point2D rootElementToMap(Point pnt)
        {
            return this.panLayerToMap(this.rootElement.TransformToVisual(this.layerCollectionContainer).Transform(pnt));
        }

        /// <summary>${WP_mapping_Map_method_screenToMap_D}</summary>
        /// <returns>${WP_mapping_Map_method_screenTomap_return}</returns>
        /// <param name="point">${WP_mapping_Map_method_screenToMap_param_point}</param>
        public Point2D ScreenToMap(Point point)
        {

            if (this.layerCollectionContainer == null)
            {
                return Point2D.Empty;
            }
            try
            {
                return this.panLayerToMap(base.TransformToVisual(this.layerCollectionContainer).Transform(point));
            }
            catch
            {
                return Point2D.Empty;
            }
        }

        private void SetOriginAndResolution(double currentResolution, Point2D currentOrigin, bool resetTransforms)
        {
            this.origin = currentOrigin;
            this.mapResolution = currentResolution;
            foreach (UIElement element in this.layerCollectionContainer.Children)
            {
                LayerContainer container = element as LayerContainer;
                container.OriginX = currentOrigin.X;
                container.OriginY = currentOrigin.Y;
                container.Resolution = currentResolution;
                if (resetTransforms)
                {
                    container.ResetGeometryTransforms();
                }
            }
        }

        private bool isClipPropertySet;
        private RectangleGeometry clippingRectangle;
        private void UpdateClip(Size arrangeSize)
        {
            if (!this.isClipPropertySet && (this.rootElement != null))
            {
                this.clippingRectangle = new RectangleGeometry();
                this.rootElement.Clip = this.clippingRectangle;
                this.isClipPropertySet = true;
            }
            if (this.clippingRectangle != null)
            {
                this.clippingRectangle.Rect = new Rect(0.0, 0.0, arrangeSize.Width, arrangeSize.Height);
            }
        }


        /// <summary>${WP_mapping_Map_method_viewEntire_D}</summary>
        public void ViewEntire()
        {
            ZoomTo(this.Bounds);
        }

        /// <summary>${WP_mapping_Map_method_zoom_D}</summary>
        /// <param name="ratio">${WP_mapping_Map_method_zoom_param_ratio}</param>
        public void Zoom(double ratio)
        {
            if (!double.IsNaN(this.mapResolution))
            {
                Point pnt = this.rootElement.TransformToVisual(this.layerCollectionContainer).Transform(new Point(currentSize.Width / 2.0, currentSize.Height / 2.0));
                pnt.X = Math.Round(pnt.X);
                pnt.Y = Math.Round(pnt.Y);
                Point2D center = this.panLayerToMap(pnt);
                if (!Point2D.IsNullOrEmpty(center))
                {
                    this.zoomAbout(ratio, center, false);
                }
            }
        }
        private void zoomAbout(double factor, Point2D center, bool skipAnimation)
        {
            if (!double.IsNaN(factor) && factor > 0)
            {
                if (Point2D.IsNullOrEmpty(center))
                {
                    return;
                }
                this.ZoomToResolution(this.mapResolution / factor, center, skipAnimation);
            }
        }

        /// <summary>${WP_mapping_Map_method_zoomTo_D}</summary>
        /// <param name="bounds">${WP_mapping_Map_method_zoomTo_param_bounds}</param>
        public void ZoomTo(Rectangle2D bounds)
        {
            this.zoomTo(bounds, false);
        }
        private void zoomTo(Rectangle2D bounds, bool skipAnimation)
        {
            if ((!Rectangle2D.IsNullOrEmpty(bounds)) && (bounds.Width != 0.0 || bounds.Height != 0.0))
            {
                Rectangle2D other = this.ViewBounds;
                if (Rectangle2D.IsNullOrEmpty(other))
                {
                    this.mapResolution = this.targetResolution = MathUtil.MinMaxCheck(Math.Max(bounds.Width / currentSize.Width, bounds.Height / currentSize.Height), MinResolution, MaxResolution);
                    Point2D center = bounds.Center;
                    this.origin = new Point2D(center.X - ((currentSize.Width * 0.5) * this.mapResolution), center.Y + ((currentSize.Height * 0.5) * this.mapResolution));
                    this.LoadLayersInView(false, this.GetFullViewBounds());
                }
                else if (double.IsNaN(this.mapResolution))
                {
                    this.cacheViewBounds = bounds;
                }//终于判断了
                else
                {
                    if (!bounds.IntersectsWith(other))
                    {
                        skipAnimation = true;
                    }//有相交的跳过动画

                    Point2D point2 = bounds.Center;
                    double nearestLevelResolution = MathUtil.GetNearest(MathUtil.MinMaxCheck(Math.Max(bounds.Width / currentSize.Width, bounds.Height / currentSize.Height), MinResolution, MaxResolution), this.Resolutions, MinResolution, MaxResolution);
                    bounds = new Rectangle2D(point2.X - ((currentSize.Width * nearestLevelResolution) * 0.5),
                        point2.Y - ((currentSize.Height * nearestLevelResolution) * 0.5),
                        point2.X + ((currentSize.Width * nearestLevelResolution) * 0.5),
                        point2.Y + ((currentSize.Height * nearestLevelResolution) * 0.5));


                    bounds = getAdjustedViewBounds(bounds, currentSize);

                    bool pan = DoubleUtil.AreClose(1.0, bounds.Width / other.Width);
                    if (!pan && (bounds.Width != 0.0 || bounds.Height != 0.0))
                    {
                        beginZoomToTargetBounds(bounds, skipAnimation);
                    }
                    else
                    {
                        //PanTo(bounds.Center, skipAnimation);//pan到这个点的bounds其实
                        panTo(bounds, skipAnimation);//10/3/12
                    }
                }
            }
        }

        private static Rectangle2D getAdjustedViewBounds(Rectangle2D viewBounds, Size viewSize)
        {
            double width = viewBounds.Width;
            double height = viewBounds.Height;
            double ratio = viewSize.Height / viewSize.Width;
            if ((height / width) > ratio)
            {
                width = height / ratio;
            }
            else
            {
                height = width * ratio;
            }
            return Rectangle2D.CreateFromXYWidthHeight(viewBounds.Center.X - (width / 2.0), viewBounds.Center.Y - (height / 2.0), width, height);
        }

        /// <overloads>${WP_mapping_Map_method_zoomToResolution_overloads_D}</overloads>
        /// <summary>${WP_mapping_Map_method_zoomToResolution_Double_D}</summary>
        /// <param name="resolution">${WP_mapping_Map_method_zoomToResolution_Double_param_resolution}</param>
        public void ZoomToResolution(double resolution)
        {
            this.ZoomToResolution(resolution, Point2D.Empty);
        }

        /// <summary>${WP_mapping_Map_method_zoomToResolution_Double_Point2D_D}</summary>
        /// <param name="resolution">${WP_mapping_Map_method_zoomToResolution_Double_param_resolution}</param>
        /// <param name="center">${WP_mapping_Map_method_zoomToResolution_Double_Point2D_param_center}</param>
        public void ZoomToResolution(double resolution, Point2D center)
        {
            this.ZoomToResolution(resolution, center, false);
        }

        public void ZoomToResolution(double resolution, Point2D center, bool skipAnimation)
        {
            resolution = MathUtil.MinMaxCheck(resolution, MinResolution, MaxResolution);

            double ratio = resolution / this.mapResolution;
            if (!DoubleUtil.AreClose(1.0, ratio) && panHelper != null)//如果同一级别，请pan
            {
                panHelper.Stop(true);
                Rectangle2D bounds = ViewBounds;//如果没有初始化好，这个为空,mapResolution也为nan
                if (!Rectangle2D.IsNullOrEmpty(bounds))
                {
                    if (Point2D.IsNullOrEmpty(center))
                    {
                        center = bounds.Center;
                    }//非数字

                    Rectangle2D targetBounds = new Rectangle2D(center.X - (currentSize.Width / 2) * resolution, center.Y - (currentSize.Height / 2) * resolution, center.X + (currentSize.Width / 2) * resolution, center.Y + (currentSize.Height / 2) * resolution);

                    beginZoomToTargetBounds(targetBounds, skipAnimation);
                }
            }
            else
            {
                if (!Point2D.IsNullOrEmpty(center))
                {
                    this.PanTo(center);
                }
            }
        }

        /// <summary>${WP_mapping_Map_method_pan_D_sl}</summary>
        /// <param name="offsetX">${WP_mapping_Map_method_pan_param_offsetX}</param>
        /// <param name="offsetY">${WP_mapping_Map_method_pan_param_offsetY}</param>
        public void Pan(double offsetX, double offsetY)
        {
            Point2D newCenter = Point2D.Empty;
            newCenter.X = ViewBounds.Center.X + offsetX;
            newCenter.Y = ViewBounds.Center.Y + offsetY;
            this.PanTo(newCenter);
        }

        //属性
        internal Popup Popup { get; private set; }
        /// <summary>${WP_mapping_Map_attribute_metadata_D}</summary>
        public Dictionary<string, string> Metadata { get; set; }

        //TODO:理论上应该也向Resolutions那样可变
        private CoordinateReferenceSystem mapCRS;
        /// <summary>${WP_mapping_Map_attribute_CRS_D}</summary>
        /// <seealso cref="Core.CoordinateReferenceSystem">CoordinateReferenceSystem Class</seealso>
        public CoordinateReferenceSystem CRS
        {
            get
            {
                if (mapCRS == null)
                {
                    if (Layers != null)
                    {
                        mapCRS = Layers.GetCRS();
                    }
                    //this.raisePropertyChanged("CRS");
                }

                return mapCRS;
            }
            set
            {
                mapCRS = value;
                this.raisePropertyChanged("CRS");
            }
        }

        /// <summary>${WP_mapping_Map_attribute_bounds_D}</summary>
        public Rectangle2D Bounds
        {
            get
            {
                return Layers.GetBounds(this.CRS);
            }
        }

        /// <summary>${WP_mapping_Map_attribute_viewBounds_D}</summary>
        public Rectangle2D ViewBounds
        {
            get
            {
                if (this.isDesignMode)
                {
                    return this.designViewBounds;
                }

                if (Point2D.IsNullOrEmpty(origin) || double.IsNaN(mapResolution) || currentSize.Width == 0.0 || currentSize.Height == 0.0)
                {
                    return cacheViewBounds;
                }

                Point currentOffset = this.panHelper.GetCurrentOffset();
                double left = this.origin.X + (currentOffset.X * this.mapResolution);
                double top = this.origin.Y - (currentOffset.Y * this.mapResolution);

                return new Rectangle2D(left, top - currentSize.Height * mapResolution, left + currentSize.Width * mapResolution, top);
            }
            set
            {
                if (this.isDesignMode)
                {
                    if (!Rectangle2D.IsNullOrEmpty(value))
                    {
                        this.designViewBounds = value;
                        this.zoomTo(value, true);
                    }
                    else
                    {
                        this.zoomTo(this.Layers.GetBounds(this.CRS), true);
                        this.designViewBounds = Rectangle2D.Empty;
                    }
                }
                else
                {
                    if (currentSize.Width == 0.0 || currentSize.Height == 0.0 || this.layerCollectionContainer == null || this.isDesignMode)
                    {
                        cacheViewBounds = value;//第一次赋值 
                    }
                    else
                    {
                        this.zoomTo(value, true);
                    }
                }
            }
        }

        /// <summary>${WP_mapping_Map_attribute_center_D}</summary>
        public Point2D Center
        {
            get
            {
                if ((this.layerCollectionContainer != null) && (!Rectangle2D.IsNullOrEmpty(this.ViewBounds)))
                {
                    return this.ViewBounds.Center;
                }
                return Point2D.Empty;
            }
        }
        /// <summary>${WP_mapping_Map_attribute_level_D}</summary>
        public int Level
        {
            get
            {
                if (this.Resolutions != null)
                {
                    return MathUtil.GetNearestIndex(Resolution, Resolutions, MinResolution, MaxResolution);
                }
                return -1;
            }
        }

        /// <summary>${WP_mapping_Map_method_zoomToLevel_Int_D}</summary>
        /// <overloads>${WP_mapping_Map_method_zoomToLevel_overloads_D}</overloads>
        /// <param name="level">${WP_mapping_Map_method_zoomToLevel_Int_param_level}</param>
        public void ZoomToLevel(int level)
        {
            ZoomToLevel(level, Point2D.Empty);
        }

        /// <summary>${WP_mapping_Map_method_zoomToLevel_Int_Point2D_D}</summary>
        /// <param name="level">${WP_mapping_Map_method_zoomToLevel_Int_param_level}</param>
        /// <param name="center">${WP_mapping_Map_method_zoomToLevel_Int_Point2D_param_center}</param>
        public void ZoomToLevel(int level, Point2D center)
        {
            if (Resolutions != null)
            {
                level = level < 0 ? 0 : level;
                level = level > (Resolutions.Length - 1) ? (Resolutions.Length - 1) : level;
                ZoomToResolution(Resolutions[level], center, false);
            }
        }

        /// <summary>${WP_mapping_Map_method_zoomIn_D}</summary>
        public void ZoomIn()
        {
            if (this != null)
            {
                this.ZoomToResolution(this.GetNextResolution(true));
            }
        }

        /// <summary>${WP_mapping_Map_method_zoomOut_D}</summary>
        public void ZoomOut()
        {
            if (this != null)
            {
                this.ZoomToResolution(this.GetNextResolution(false));
            }
        }

        /// <summary>${WP_mapping_Map_attribute_minResolution_D}</summary>
        public double MinResolution
        {
            get
            {
                return minResolution;
            }
            set
            {
                if ((value < double.Epsilon) || (!double.IsNaN(this.MaxResolution) && value > this.MaxResolution))
                {
                    //TODO:资源化
                    throw new ArgumentOutOfRangeException("value", "MinResolution值越界");
                }
                minResolution = value;
                isMinResolutionSet = true;
                this.raisePropertyChanged("MinResolution");
            }

        }
        /// <summary>${WP_mapping_Map_attribute_maxResolution_D}</summary>
        public double MaxResolution
        {
            get
            {
                return maxResolution;
            }
            set
            {
                if (value < double.Epsilon || (!double.IsNaN(this.MinResolution) && value < this.MinResolution))
                {
                    //TODO:资源化
                    throw new ArgumentOutOfRangeException("value", "MaxResolution值越界");
                }
                maxResolution = value;
                isMaxResolutionSet = true;
                this.raisePropertyChanged("MaxResolution");
            }
        }
        /// <summary>${WP_mapping_Map_attribute_Resolution_D}</summary>
        public double Resolution
        {
            get
            {
                if ((this.layerCollectionContainer != null) && (!Rectangle2D.IsNullOrEmpty(this.ViewBounds)) && (base.ActualWidth != 0.0))
                {
                    return (this.ViewBounds.Width / base.ActualWidth);
                }
                return double.NaN;
            }
        }

        private bool fromLayers = false;
        private bool hasChanged = false;
        private double[] oldResolutions;
        //fromLayers = true;// && !fromLayers
        /// <summary>${WP_mapping_Map_attribute_Resolutions_D}</summary>
        [TypeConverter(typeof(DoubleArrayConverter))]
        public double[] Resolutions
        {
            get
            {
                if (resolutions == null)
                {
                    if (this.Layers != null)
                    {
                        resolutions = Layers.GetResolutions(this.CRS);
                    }
                    //MaxResolution = resolutions[0];
                    //MinResolution = resolutions[resolutions.Length - 1];
                    fromLayers = true;
                    hasChanged = false;
                }//Map没有设置，从Layers上取
                if (resolutions != null && fromLayers && hasChanged)
                {
                    //如果在Map上设置了就不在从Layer上获取了；（此时从Layer上的Resolution为null）
                    if (!isMapSetResolutions)
                    {
                        resolutions = Layers.GetResolutions(this.CRS);
                        if (resolutions == null)
                        {
                            fromLayers = true;
                            return null;
                        }
                    }
                    hasChanged = false;
                    MaxResolution = resolutions[0];
                    MinResolution = resolutions[resolutions.Length - 1];

                }//尽管有值，但这个值是从Layers取的，并且发生变化了，再重新取值

                return resolutions;
            }
            set
            {
                resolutions = ScaleHelper.CheckAndSortResolutions(value);

                if (resolutions != null)
                {
                    if (!fromLayers)
                    {
                        ScaleHelper.CheckResolutionsMatching(resolutions, Layers.GetResolutions(this.CRS));
                    }
                    MaxResolution = resolutions[0];
                    MinResolution = resolutions[resolutions.Length - 1];
                }

                if (ResolutionsChanged != null && !MathUtil.CheckNewAndOldEqual(oldResolutions, resolutions))
                {
                    ResolutionsChanged(this, new ResolutionsEventArgs(oldResolutions, resolutions));
                }

                this.raisePropertyChanged("Resolutions");
                oldResolutions = resolutions;
                isMapSetResolutions = true;
            }
        }
        private bool isMapSetResolutions = false;

        #region Scale 系列
        private double minScale = double.NaN;
        private double maxScale = double.NaN;
        private double[] scales;
        private bool isMaxScaleSet;
        private bool isMinScaleSet;

        private double Dpi
        {
            get
            {
                if (this.Layers != null)
                {
                    return Layers.GetDpi();
                }
                return 0;
            }
        }
        /// <summary>${WP_mapping_Map_attribute_Scales_D}</summary>
        [TypeConverter(typeof(DoubleArrayConverter))]
        public double[] Scales
        {
            get
            {
                return scales;
            }
            set
            {

                scales = ScaleHelper.CheckAndSortScales(value);

                if (scales != null)
                {
                    MinScale = scales[0];
                    MaxScale = scales[scales.Length - 1];
                }


                //在这里设置下Res吧。还是有用的，比如在Layers初始化之后来设置了下
                if (this.Dpi != 0.0)
                {
                    this.Resolutions = ScaleHelper.ConversionBetweenScalesAndResulotions(this.scales, this.Dpi, this.CRS);

                    if (isMaxScaleSet)
                    {
                        this.MinResolution = ScaleHelper.ScaleConversion(this.MaxScale, this.Dpi, this.CRS); ;
                    }

                    if (isMinScaleSet)
                    {
                        this.MaxResolution = ScaleHelper.ScaleConversion(this.MinScale, this.Dpi, this.CRS); ;
                    }
                }
                this.raisePropertyChanged("Scales");

                //当Layer初始化时，Scales发生变化；还有就是Scales在Set的时候发生变化；
                bool isEqual = MathUtil.CheckNewAndOldEqual(this.scales, oldScales);
                if (this.ScalesChanged != null && !isEqual)
                {
                    this.ScalesChanged(this, new ScalesEventArgs(oldScales, this.scales));
                }
                oldScales = this.scales;
            }
        }
        /// <summary>${WP_mapping_Map_attribute_minScale_D}</summary>
        public double MinScale
        {
            get
            {
                if (!double.IsNaN(minScale))
                {
                    return minScale;
                }
                if (this.Dpi != 0.0)
                {
                    return ScaleHelper.ScaleConversion(this.MaxResolution, this.Dpi, this.CRS);
                }
                return double.NaN;
            }
            set
            {
                if ((value < double.Epsilon) || (!double.IsNaN(this.MaxScale) && value > this.MaxScale))
                {
                    //TODO:资源化
                    throw new ArgumentOutOfRangeException("value", "MinScale值越界");
                }
                minScale = value;
                isMinScaleSet = true;
                if (this.Dpi != 0.0)
                {
                    this.MaxResolution = ScaleHelper.ScaleConversion(minScale, this.Dpi, this.CRS);
                }//尽管第一次由于dpi为0不行，但设置好，随机设置最大最小，就有作用了
                this.raisePropertyChanged("MinScale");
            }

        }
        /// <summary>${WP_mapping_Map_attribute_maxScale_D}</summary>
        public double MaxScale
        {
            get
            {
                if (!double.IsNaN(maxScale))
                {
                    return maxScale;
                }
                if (this.Dpi != 0.0)
                {
                    return ScaleHelper.ScaleConversion(this.MinResolution, this.Dpi, this.CRS);
                }
                return double.NaN;
            }
            set
            {
                if ((value < double.Epsilon) || (!double.IsNaN(this.MinScale) && value < this.MinScale))
                {
                    //TODO:资源化
                    throw new ArgumentOutOfRangeException("value", "MaxScale值越界");
                }
                maxScale = value;
                isMaxScaleSet = true;

                if (this.Dpi != 0.0)
                {
                    this.MinResolution = ScaleHelper.ScaleConversion(maxScale, this.Dpi, this.CRS);
                }//尽管第一次由于dpi为0不行，但设置好，随机设置最大最小，就有作用了
                this.raisePropertyChanged("MaxScale");
            }
        }
        /// <summary>${WP_mapping_Map_attribute_Scale_D}</summary>
        public double Scale
        {
            get
            {
                return ScaleHelper.ScaleConversion(this.Resolution, this.Dpi, this.CRS);
            }
        }

        /// <summary>${WP_mapping_Map_method_zoomToScale_Int_D}</summary>
        /// <overloads>${WP_mapping_Map_method_zoomToScale_overloads_D}</overloads>
        /// <param name="scale">${WP_mapping_Map_method_zoomToScale_Int_param_scale}</param>
        public void ZoomToScale(double scale)
        {
            ZoomToScale(MathUtil.MinMaxCheck(scale, MinScale, MaxScale), Point2D.Empty);
        }
        /// <summary>${WP_mapping_Map_method_zoomToScale_Int_Point2D_D}</summary>
        /// <param name="scale">${WP_mapping_Map_method_zoomToScale_Int_param_scale}</param>
        /// <param name="center">${WP_mapping_Map_method_zoomToScale_Int_Point2D_param_center}</param>
        public void ZoomToScale(double scale, Point2D center)
        {
            double res = double.NaN;
            if (this.Dpi != 0.0)
            {
                res = ScaleHelper.ScaleConversion(scale, this.Dpi, this.CRS);
            }
            ZoomToResolution(res, center);
        }

        #endregion

        /// <summary>${WP_mapping_Map_field_PanDurationProperty_D}</summary>
        public static readonly DependencyProperty PanDurationProperty = DependencyProperty.Register("PanDuration", typeof(TimeSpan), typeof(Map), new PropertyMetadata(TimeSpan.FromSeconds(0.25)));
        /// <summary>${WP_mapping_Map_attribute_PanDuration_D}</summary>
        public TimeSpan PanDuration
        {
            get
            {
                return (TimeSpan)base.GetValue(PanDurationProperty);
            }
            set
            {
                base.SetValue(PanDurationProperty, value);
            }
        }
        /// <summary>${WP_mapping_Map_field_ZoomDurationProperty_D}</summary>
        public static readonly DependencyProperty ZoomDurationProperty = DependencyProperty.Register("ZoomDuration", typeof(TimeSpan), typeof(Map), new PropertyMetadata(TimeSpan.FromSeconds(0.25)));
        /// <summary>${WP_mapping_Map_attribute_ZoomDuration_D}</summary>
        public TimeSpan ZoomDuration
        {
            get
            {
                return (TimeSpan)base.GetValue(ZoomDurationProperty);
            }
            set
            {
                base.SetValue(ZoomDurationProperty, value);
            }
        }

        /// <summary>${WP_mapping_Map_field_layersProperty_D}</summary>
        public static readonly DependencyProperty LayersProperty =
          DependencyProperty.Register("Layers", typeof(LayerCollection), typeof(Map), new PropertyMetadata(new PropertyChangedCallback(Map.OnLayersPropertyChanged)));
        /// <summary>${WP_mapping_Map_attribute_Layers_D}</summary>
        public LayerCollection Layers
        {
            get
            {
                return (LayerCollection)GetValue(LayersProperty);
            }
            set
            {
                base.SetValue(LayersProperty, value);
            }
        }

        private static void OnLayersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map map = d as Map;
            LayerCollection newValue = e.NewValue as LayerCollection;
            LayerCollection oldValue = e.OldValue as LayerCollection;
            if (oldValue != null)
            {
                oldValue.Progress -= new EventHandler<ProgressEventArgs>(map.layersProgressHandler);
                oldValue.LayersInitialized -= new EventHandler(map.Layers_LayersInitialized);
                oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(map.Layers_CollectionChanged);
                foreach (Layer layer in oldValue)
                {
                    map.RemoveMapLayers(layer);
                }
                map.ResetMapStatus();
            }
            if (newValue != null)
            {
                newValue.Progress += new EventHandler<ProgressEventArgs>(map.layersProgressHandler);
                newValue.LayersInitialized += new EventHandler(map.Layers_LayersInitialized);
                newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(map.Layers_CollectionChanged);
                foreach (Layer layer2 in newValue)
                {
                    Map map2 = layer2.GetValue(MapProperty) as Map;
                    if ((map2 != null) && (map2 != map))
                    {
                        throw new ArgumentException(ExceptionStrings.LayerToMap);
                    }
                    layer2.SetValue(MapProperty, map);
                    layer2.LayerChanged += new Layer.LayerChangedHandler(map.layer_OnLayerChanged);
                    layer2.Initialized += new EventHandler<EventArgs>(map.layer_Initialized);//如何使用非静态方法
                    if (!layer2.IsInitialized && (map.panHelper != null))
                    {
                        layer2.Initialize();
                    }
                }
            }
            map.raisePropertyChanged("Layers");
        }

        private void layersProgressHandler(object sender, ProgressEventArgs args)
        {
            if (this.Progress != null)
            {
                this.Progress(this, args);
            }
        }

        #region 旋转相关
        private ThrottleTimer mapRotateThrottler;

        /// <summary>${WP_pubilc_fields_identifies_sl} <see cref="Angle">Angle</see>
        ///         ${WP_pubilc_fields_attachedproperty_sl}</summary>
        public static readonly DependencyProperty AngleProperty =
           DependencyProperty.Register("Angle", typeof(double), typeof(Map), new PropertyMetadata(new PropertyChangedCallback(OnAnglePropertyChanged)));
        /// <summary>${WP_mapping_Map_attribute_angle_D}</summary>
        /// <remarks>
        /// 	<para>
        ///     ${WP_mapping_Map_attribute_angle_default}</para>${WP_mapping_Map_attribute_angle_Exception}</remarks>
        public double Angle
        {
            get
            {
                return (double)base.GetValue(AngleProperty);
            }
            set
            {
                while (value > 360.0)
                {
                    value -= 360.0;
                }
                while (value < 0.0)
                {
                    value += 360.0;
                }
                base.SetValue(AngleProperty, value);
            }
        }
        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map map = d as Map;
            double newValue = (double)e.NewValue;
            while (newValue > 180.0)
            {
                newValue -= 360.0;
            }
            while (newValue < -180.0)
            {
                newValue += 360.0;
            }
            if (map.rotateTransform != null)
            {
                map.rotateTransform.Angle = newValue;
            }
            map.RaiseAngleChanged(e);
            map.raisePropertyChanged("Angle");
        }
        private void RaiseAngleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.AngleChanged != null)
            {
                this.AngleChanged(this, e);
            }
            this.LoadContinuousLayersInView(false);
            if (this.mapRotateThrottler == null)
            {
                this.mapRotateThrottler = new ThrottleTimer(250, delegate
                {
                    this.LoadLayersInView(false, this.GetFullViewBounds());
                });
            }
            this.mapRotateThrottler.Invoke();
        }

        /// <summary>
        /// 	<see cref="AngleChanged">AngleChanged</see> ${WP_pubilc_delegates_description_sl}</summary>
        public delegate void AngleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e);
        /// <summary>${WP_mapping_Map_event_angleChanged_D}</summary>
        public event AngleChangedEventHandler AngleChanged;
        #endregion


        //在Pan Action中用到，其实可以不用
        internal Canvas TransformCanvas { get { return this.transformCanvas; } }
        internal Grid RootElement { get { return this.rootElement; } }
        internal PanAnimation PanHelper { get { return this.panHelper; } }
        internal void PanCompleted()
        {
            this.panHelper_PanAnimationCompleted(null, null);
        }

        //一般可set的属性，变化时都触发下
        /// <summary>${WP_mapping_Map_event_propertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void raisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>${WP_mapping_Map_attribute_Theme_D}</summary>
        public Theme.Theme Theme { get; set; }


    }
    //这个类，必须用public，否则报错。
    /// <summary>
    /// 	<para>${WP_mapping_ZoomEffect_Title}</para>
    /// 	<para>${WP_mapping_ZoomEffect_Description}</para>
    /// </summary>
    public class ZoomEffect : Control
    {
        private Storyboard zoomIn;
        private Storyboard zommOut;
        private Grid root;

        /// <summary>${WP_mapping_ZoomEffect_constructor_None_D}</summary>
        public ZoomEffect()
        {
            DefaultStyleKey = typeof(ZoomEffect);
        }
        /// <summary>${WP_mapping_ZoomEffect_mathod_OnApplyTemplate_D}</summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            root = this.GetTemplateChild("root") as Grid;
            if (root != null)
            {
                zoomIn = (Storyboard)root.Resources["zoomoutAnimation"];
                zommOut = (Storyboard)root.Resources["zoomanimation2"];
            }
        }
        /// <summary>${WP_mapping_ZoomEffect_mathod_beginZoomin_D}</summary>
        public void beginZoomin()
        {
            zommOut.Stop();
            zoomIn.Begin();
        }
        /// <summary>${WP_mapping_ZoomEffect_mathod_beiginZoomout_D}</summary>
        public void beiginZoomout()
        {
            zoomIn.Stop();
            zommOut.Begin();
        }
    }
}
