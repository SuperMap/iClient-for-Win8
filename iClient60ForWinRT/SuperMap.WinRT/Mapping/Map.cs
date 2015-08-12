using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Resources;
using SuperMap.WinRT.Utilities;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Input;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${mapping_Map_Title}</para>
    /// 	<para>${mapping_Map_Description}</para>
    /// </summary>
    [TemplatePart(Name = "RootElement", Type = typeof(Grid))]
    [ContentProperty(Name = "Layers")]
    public sealed partial class Map : Control, INotifyPropertyChanged, IDisposable
    {
        internal static readonly DependencyProperty MapProperty = DependencyProperty.RegisterAttached("Map", typeof(Map), typeof(Map), null);

        private ThrottleTimer zoomThrottleTimer;
        private PanAnimation panHelper;

        private Grid rootElement;
        private Grid backgroundWindow;
        private Canvas transformCanvas;
        private Canvas layerCollectionContainer;
        private RotateTransform rotateTransform;

        /// <summary>
        /// 地图当前分辨率，不能用来作为出图的数据。如果要修改这个值，尽量使用统一的入口来修改。
        /// </summary>
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

        /// <summary>
        /// 目标分辨率，最终出图时以这个分辨率为准。
        /// </summary>
        private double targetResolution;

        private Rectangle2D designViewBounds = Rectangle2D.Empty;
        private bool isDesignMode;

        /// <summary>${mapping_Map_event_progress_D}</summary>
        public event EventHandler<ProgressEventArgs> Progress;
        /// <summary>${mapping_Map_event_viewBoundsChanging_D}</summary>
        public event EventHandler<ViewBoundsEventArgs> ViewBoundsChanging;
        /// <summary>${mapping_Map_event_viewBoundsChanged_D}</summary>
        /// <example>
        /// <code>
        ///  MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
        ///  void MyMap_ViewBoundsChanged(object sender, SuperMap.WinRT.Mapping.ViewBoundsEventArgs e)
        ///  {
        ///    
        ///   }
        ///  </code>
        /// </example>
        public event EventHandler<ViewBoundsEventArgs> ViewBoundsChanged;

        /// <summary>${mapping_Map_event_ScalesChanged_D}</summary>
        public event EventHandler<ScalesEventArgs> ScalesChanged;

        /// <summary>${mapping_Map_event_ResolutionsChanged_D}</summary>
        public event EventHandler<ResolutionsEventArgs> ResolutionsChanged;

        /// <summary>${mapping_Map_attribute_zoomFactor_D}</summary>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }
        /// <summary>${mapping_Map_attribute_field_zoomFactorProperty_D}</summary>
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(Map), new PropertyMetadata(2.0));

        /// <summary>${mapping_Map_attribute_panFactor_D}</summary>
        public double PanFactor
        {
            get { return (double)GetValue(PanFactorProperty); }
            set { SetValue(PanFactorProperty, value); }
        }
        /// <summary>${mapping_Map_attribute_field_panFactorProperty_D}</summary>
        public static readonly DependencyProperty PanFactorProperty =
            DependencyProperty.Register("PanFactor", typeof(double), typeof(Map), new PropertyMetadata(0.1));

        /// <summary>
        /// ${mapping_Map_attribute_MapStatus_D}
        /// </summary>
        public MapStatus MapStatus
        {
            get;
            private set;
        }

        /// <summary>
        /// ${mapping_Map_attribute_MapManipulator_D}
        /// </summary>
        public MapManipulator MapManipulator
        {
            get;
            private set;
        }

        /// <summary>${mapping_Map_constructor_None_D}</summary>
        public Map()
        {
            Theme = null;
            Metadata = new Dictionary<string, string>();
            mapResolution = double.NaN;
            maxResolution = double.MaxValue;
            minResolution = double.Epsilon;
            maxScale = double.MaxValue;
            minScale = double.Epsilon;
            origin = Point2D.Empty;
            base.ManipulationMode = ManipulationModes.All;
            previousViewBounds = Rectangle2D.Empty;
            cacheViewBounds = Rectangle2D.Empty;
            zoomThrottleTimer = new ThrottleTimer(200);
            currentSize = new Size(0, 0);
            base.DefaultStyleKey = typeof(Map);

            Layers = new LayerCollection();
            Popup = new Popup();

            base.Loaded += new RoutedEventHandler(Map_Loaded);

        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            isDesignMode = DesignMode.DesignModeEnabled;
        }

        /// <summary>${mapping_Map_method_ArrangeOverride_D}</summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.UpdateClip(finalSize);
            return base.ArrangeOverride(finalSize);
        }
        /// <summary>${mapping_Map_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
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
            layerCollectionContainer.RenderTransform = new TranslateTransform();
            this.transformCanvas.Children.Add(this.layerCollectionContainer);
            if (base.FlowDirection == FlowDirection.RightToLeft)
            {
                this.transformCanvas.HorizontalAlignment = HorizontalAlignment.Right;
            }
            this.MapStatus = MapStatus.Still;
            this.MapManipulator = MapManipulator.None;
            this.panHelper = new PanAnimation(this);
            this.panHelper.PanAnimationCompleted += this.panHelper_PanAnimationCompleted;
            this.panHelper.Panning += this.panHelper_Panning;
            this.panHelper.PanStarted += panHelper_PanStarted;

            this.ZoomHelper = new ZoomAnimation(this);
            this.ZoomHelper.ZoomStarted += ZoomHelper_ZoomStarted;
            this.ZoomHelper.Zooming += ZoomHelper_Zooming;
            this.ZoomHelper.ZoomCompleted += ZoomHelper_ZoomCompleted;

            base.SizeChanged += new SizeChangedEventHandler(this.Map_SizeChanged);

            BuildMapAction();//在MapActionPart.cs中

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

        void ZoomHelper_ZoomStarted(object sender, EventArgs e)
        {
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.ZoomStarted;
            }

        }

        void ZoomHelper_Zooming(object sender, ZoomAnimationEventArgs e)
        {
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.Zooming;
            }
            Rectangle2D old = ViewBounds;
            Point2D origin = new Point2D(e.NowCenter.X - currentSize.Width / 2 * e.NowResolution, e.NowCenter.Y + currentSize.Height / 2 * e.NowResolution);
            SetOriginAndResolution(e.NowResolution, origin, false);
            this.LoadLayersInView(false, ViewBounds);
            RaiseViewBoundsChanging(new ViewBoundsEventArgs(old, ViewBounds));
        }

        void ZoomHelper_ZoomCompleted(object sender, ZoomAnimationEventArgs e)
        {
            Rectangle2D old = ViewBounds;
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.ZoomCompleted;
            }
            Point2D origin = new Point2D(e.NowCenter.X - currentSize.Width / 2 * targetResolution, e.NowCenter.Y + currentSize.Height / 2 * targetResolution);
            SetOriginAndResolution(targetResolution, origin, true);
            Rectangle2D bounds = ViewBounds;
            this.LoadLayersInView(false, bounds);
            if (this.MapStatus == MapStatus.ZoomCompleted)
            {
                RaiseViewBoundsChanged();
            }
            else
            {
                RaiseViewBoundsChanging(new ViewBoundsEventArgs(old, ViewBounds));
            }
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.Still;
                this.MapManipulator = MapManipulator.None;
            }
            else if (MapManipulator == MapManipulator.Device && MapStatus == MapStatus.ZoomCompleted)
            {
                MapStatus = MapStatus.Still;
                MapManipulator = MapManipulator.None;
            }
        }

        /// <summary>${mapping_Map_attribute_ScreenContainer_D}</summary>
        public Canvas ScreenContainer { get; private set; }

        //获取下一级别的Resolution或Scale:限制了Level，则获取下一个Level对应的尺度；
        //若没有，则返回缩放2倍（ZoomFactor可调整）后的尺度
        //plus表示放大true还是缩小false
        /// <summary>${mapping_Map_method_getNextResolution_D}</summary>
        /// <returns>${mapping_Map_method_getNextResolution_returns}</returns>
        /// <param name="plus">${mapping_Map_method_getNextResolution_param_plus}</param>
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
        /// <summary>${mapping_Map_method_getNextScale_D}</summary>
        /// <returns>${mapping_Map_method_getNextScale_returns}</returns>
        /// <param name="plus">${mapping_Map_method_getNextScale_param_plus}</param>
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


        /// <summary>${mapping_Map_method_panByPixel_D}</summary>
        /// <example>
        ///     <code>
        ///         MyMap.PanByPixel(23, 35);
        ///     </code>
        /// </example>
        /// <param name="pixelX">${mapping_Map_method_panByPixel_param_pixelX}</param>
        /// <param name="pixelY">${mapping_Map_method_panByPixel_param_pixelY}</param>
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
        /// <summary>${mapping_Map_method_openInfoWindow_D}</summary>
        /// <param name="location">${mapping_Map_method_openInfoWindow_param_location}</param>
        /// <param name="element">${mapping_Map_method_openInfoWindow_param_element}</param>
        public void OpenInfoWindow(Point2D location, UIElement element)
        {
            this.OpenInfoWindow(location, 0, 0, element);
        }
        /// <summary>${mapping_Map_method_openInfoWindow_D}</summary>
        ///  <param name="location">${mapping_Map_method_openInfoWindow_param_location}</param>
        ///  <param name="offsetPixelX">${mapping_Map_method_openInfoWindow_param_offsetPixelX}</param>
        ///  <param name="offsetPixelY">${mapping_Map_method_openInfoWindow_param_offsetPixelY}</param>
        /// <param name="element">${mapping_Map_method_openInfoWindow_param_element}</param>
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

        /// <summary>${mapping_Map_method_closeInfoWindow_D}</summary>
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

        private void CalculateStartViewBounds(Size size)
        {
            if (!Rectangle2D.IsNullOrEmpty(cacheViewBounds) && Point2D.IsNullOrEmpty(origin))
            {
                Rectangle2D cacheBounds = cacheViewBounds;
                double width = size.Width;
                double height = size.Height;
                double resWidth = cacheBounds.Width / width;
                double resHeight = cacheBounds.Height / height;
                mapResolution = (resHeight > resWidth) ? resHeight : resWidth;

                double nearestLevelResolution = MathUtil.GetNearest(this.mapResolution, this.Resolutions, MinResolution, MaxResolution);
                mapResolution = nearestLevelResolution;
                targetResolution = this.mapResolution = MathUtil.MinMaxCheck(this.mapResolution, this.MinResolution, this.MaxResolution);
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
            if ((!Rectangle2D.IsNullOrEmpty(this.ViewBounds)) && (this.panHelper != null))
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
            if ((Point2D.IsNullOrEmpty(origin) && (!Rectangle2D.IsNullOrEmpty(this.cacheViewBounds) || Rectangle2D.IsNullOrEmpty(this.ViewBounds))) && (double.IsNaN(this.mapResolution) && (currentSize.Width > 0.0)))
            {
                if (this.Layers != null && !this.Layers.HasPendingLayers)
                {
                    this.Layers_LayersInitialized(sender, args);
                }
            }
            else
            {
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
            if (layerCollectionContainer != null)
            {
                TranslateTransform renderTransform = layerCollectionContainer.RenderTransform as TranslateTransform;
                renderTransform.X = 0;
                renderTransform.Y = 0;
            }
            if (panHelper != null)
            {
                panHelper.Cancel();
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
                        DispatchedHandler handler = new DispatchedHandler(delegate { this.Layers_LayersInitialized(sender, args); });
                        base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
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

        private void LoadLayerInView(bool useTransitions, Rectangle2D drawBounds, Layer layer)
        {
            if (!Rectangle2D.IsNullOrEmpty(drawBounds) && layer.IsInitialized && (layer.Error == null)
      && (layer.MinVisibleResolution <= targetResolution) && (layer.MaxVisibleResolution >= targetResolution)
      && layer.IsVisible && Layers.Contains(layer) && CoordinateReferenceSystem.Equals(layer.CRS, CRS, true))
            {
                layer.Update(new UpdateParameter()
                {
                    UseTransitions = useTransitions,
                    Resolutions = Resolutions,
                    Resolution = mapResolution,
                    ViewBounds = drawBounds,
                    ViewSize = currentSize,
                    LayerOrigin = layer.Container.Origin
                });
            }

        }
        private void LoadLayersInView(bool useTransitions, Rectangle2D drawBounds)
        {
            if ((this.Layers != null) && (!Rectangle2D.IsNullOrEmpty(drawBounds)))
            {
                foreach (Layer layer in this.Layers)
                {
                    this.LoadLayerInView(useTransitions, drawBounds, layer);
                }
            }

        }
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
        /// <summary>${mapping_Map_method_mapToScreen_D}</summary>
        /// <returns>${mapping_Map_method_mapToScreen_return}</returns>
        /// <param name="point2D">${mapping_Map_method_mapToScreen_param_point2D}</param>
        public Point MapToScreen(Point2D point2D)
        {
            if ((this.layerCollectionContainer == null) || (Point2D.IsNullOrEmpty(point2D)))
            {
                return new Point(double.NaN, double.NaN);
            }
            try
            {
                return this.layerCollectionContainer.TransformToVisual(this).TransformPoint(this.mapToPanLayer(point2D));
            }
            catch
            {
                return new Point(double.NaN, double.NaN);
            }
        }

        /// <summary>
        /// 获取地图的偏移量，用作平移用。
        /// </summary>
        /// <returns></returns>
        internal Point GetCurrentOffset()
        {
            if (layerCollectionContainer != null)
            {
                TranslateTransform renderTransform = layerCollectionContainer.RenderTransform as TranslateTransform;
                return new Point(renderTransform.X, renderTransform.Y);
            }
            else
            {
                return new Point(0, 0);
            }
        }

        private void ResetTranslate()
        {
            TranslateTransform renderTransform = layerCollectionContainer.RenderTransform as TranslateTransform;
            renderTransform.X = 0;
            renderTransform.Y = 0;
        }

        void panHelper_PanStarted(object sender, EventArgs e)
        {
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.PanStarted;
            }
        }

        private void panHelper_Panning(object sender, PanAnimationEventArgs e)
        {
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.Panning;
            }
            Rectangle2D oldViewBounds = ViewBounds;
            TranslateTransform renderTransform = layerCollectionContainer.RenderTransform as TranslateTransform;
            renderTransform.X = e.Offset.X;
            renderTransform.Y = e.Offset.Y;
            this.LoadLayersInView(false, this.GetFullViewBounds());
            RaiseViewBoundsChanging(new ViewBoundsEventArgs(oldViewBounds, ViewBounds));
        }

        private void panHelper_PanAnimationCompleted(object sender, PanAnimationEventArgs e)
        {
            Rectangle2D old = ViewBounds;
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.PanCompleted;
            }
            TranslateTransform renderTransform = layerCollectionContainer.RenderTransform as TranslateTransform;
            renderTransform.X = e.Offset.X;
            renderTransform.Y = e.Offset.Y;
            Rectangle2D temp = GetFullViewBounds();
            LoadLayersInView(false, temp);
            if (MapStatus == MapStatus.PanCompleted)
            {
                RaiseViewBoundsChanged();
            }
            else
            {
                RaiseViewBoundsChanging(new ViewBoundsEventArgs(old, ViewBounds));
            }
            if (MapManipulator == MapManipulator.Program)
            {
                this.MapStatus = MapStatus.Still;
                this.MapManipulator = MapManipulator.None;
            }
            else if (MapManipulator == MapManipulator.Device && MapStatus == MapStatus.PanCompleted)
            {
                MapStatus = MapStatus.Still;
                MapManipulator = MapManipulator.None;
            }
        }

        private Point2D panLayerToMap(Point pnt)
        {
            if ((!Point2D.IsNullOrEmpty(this.origin)) && !double.IsNaN(this.mapResolution))
            {
                return new Point2D((pnt.X * this.mapResolution) + this.origin.X, this.origin.Y - (pnt.Y * this.mapResolution));
            }
            return Point2D.Empty;
        }


        /// <summary>${mapping_Map_method_panTo_D}</summary>
        ///     <example>
        ///         <code>
        ///             MyMap.PanTo(new Point2D(20,30));
        ///         </code>
        ///     </example>
        /// <param name="center">${mapping_Map_method_panTo_param_center}</param>
        public void PanTo(Point2D center)
        {
            panTo(center, false);
        }

        private void panTo(Point2D center, bool skipAnimation)
        {
            PanTo(new Rectangle2D(center, center), skipAnimation);
        }

        private void panTo(Rectangle2D bounds)
        {
            PanTo(bounds, false);
        }

        private void PanTo(Rectangle2D bounds, bool skipAnimation)
        {
            if ((this.panHelper != null) && (!Rectangle2D.IsNullOrEmpty(bounds)))
            {
                if (Popup.IsOpen)
                {
                    Popup.IsOpen = false;
                }
                if (ZoomHelper != null)
                {
                    ZoomHelper.Cancel();
                }
                Point2D center = bounds.Center;
                Rectangle2D viewBounds = this.ViewBounds;
                if (Rectangle2D.IsNullOrEmpty(previousViewBounds))
                {
                    previousViewBounds = viewBounds;
                }
                if (!Rectangle2D.IsNullOrEmpty(viewBounds))
                {
                    Point2D point2 = viewBounds.Center;
                    int offsetX = (int)Math.Round((double)((point2.X - center.X) / this.targetResolution));
                    int offsetY = (int)Math.Round((double)((center.Y - point2.Y) / this.targetResolution));
                    MapManipulator = MapManipulator.Program;
                    this.panHelper.DeltaPan(offsetX, offsetY, this.PanDuration);
                }
            }
        }
        /// <summary>
        /// ${mapping_Map_method_ManipulationPan_D}
        /// </summary>
        public void ManipulationPan(int offsetX, int offsetY, MapStatus mapStatus)
        {
            if (this.panHelper != null && (mapStatus == MapStatus.PanStarted || mapStatus == MapStatus.Panning || mapStatus == MapStatus.PanCompleted))
            {
                this.MapManipulator = MapManipulator.Device;
                this.MapStatus = mapStatus;
                if (mapStatus == MapStatus.Panning)
                {
                    this.panHelper.DeltaPan(offsetX, offsetY, TimeSpan.FromMilliseconds(100));
                    if (ZoomHelper != null)
                    {
                        ZoomHelper.Cancel();
                    }
                }
            }
        }

        private void RaiseViewBoundsChanging(ViewBoundsEventArgs args)
        {
            if (this.ViewBoundsChanging != null)
            {
                this.ViewBoundsChanging(this, args);
            }
            this.raisePropertyChanged("ViewBounds");
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
            return this.panLayerToMap(this.rootElement.TransformToVisual(this.layerCollectionContainer).TransformPoint(pnt));
        }

        /// <summary>${mapping_Map_method_screenToMap_D}</summary>
        /// <returns>${mapping_Map_method_screenTomap_return}</returns>
        /// <param name="point">${mapping_Map_method_screenToMap_param_point}</param>
        public Point2D ScreenToMap(Point point)
        {

            if (this.layerCollectionContainer == null)
            {
                return Point2D.Empty;
            }
            try
            {
                return this.panLayerToMap(base.TransformToVisual(this.layerCollectionContainer).TransformPoint(point));
            }
            catch
            {
                return Point2D.Empty;
            }
        }

        /// <summary>
        /// 所有对Map的分辨率的实际修改都要通过这个方法，这个方法在修改分辨率之前，需要重置偏移量和参考原点。
        /// </summary>
        /// <param name="currentResolution"></param>
        /// <param name="currentOrigin"></param>
        /// <param name="resetTransforms"></param>
        private void SetOriginAndResolution(double currentResolution, Point2D currentOrigin, bool resetTransforms)
        {
            Rectangle2D viewBounds = ViewBounds;
            this.origin = currentOrigin;
            this.mapResolution = currentResolution;
            TranslateTransform renderTransform = layerCollectionContainer.RenderTransform as TranslateTransform;
            renderTransform.X = 0;
            renderTransform.Y = 0;
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


        /// <summary>${mapping_Map_method_viewEntire_D}</summary>
        public void ViewEntire()
        {
            ZoomTo(this.Bounds);
        }

        /// <summary>${mapping_Map_method_zoom_D}</summary>
        /// <param name="ratio">${mapping_Map_method_zoom_param_ratio}</param>
        public void Zoom(double ratio)
        {
            if (!double.IsNaN(this.mapResolution))
            {
                Point pnt = this.rootElement.TransformToVisual(this.layerCollectionContainer).TransformPoint(new Point(currentSize.Width / 2.0, currentSize.Height / 2.0));
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

        /// <summary>${mapping_Map_method_zoomTo_D}</summary>
        /// <param name="bounds">${mapping_Map_method_zoomTo_param_bounds}</param>
        public void ZoomTo(Rectangle2D bounds)
        {
            this.zoomTo(bounds, false);
        }
        private void zoomTo(Rectangle2D bounds, bool skipAnimation)
        {
            if ((!Rectangle2D.IsNullOrEmpty(bounds)) && (bounds.Width != 0.0 || bounds.Height != 0.0))
            {
                MapManipulator = MapManipulator.Program;
                Rectangle2D other = this.ViewBounds;
                if (panHelper != null)
                {
                    panHelper.Cancel();
                }

                Point2D point2 = bounds.Center;
                double nearestLevelResolution = MathUtil.MinMaxCheck(Math.Max(bounds.Width / currentSize.Width, bounds.Height / currentSize.Height), this.MinResolution, this.MaxResolution);
                bounds = new Rectangle2D(point2.X - ((currentSize.Width * nearestLevelResolution) * 0.5),
                    point2.Y - ((currentSize.Height * nearestLevelResolution) * 0.5),
                    point2.X + ((currentSize.Width * nearestLevelResolution) * 0.5),
                    point2.Y + ((currentSize.Height * nearestLevelResolution) * 0.5));

                targetResolution = nearestLevelResolution;
                ZoomHelper.DeltaZoom(nearestLevelResolution, bounds.Center, this.ZoomDuration);
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

        /// <overloads>${mapping_Map_method_zoomToResolution_overloads_D}</overloads>
        /// <summary>${mapping_Map_method_zoomToResolution_Double_D}</summary>
        /// <param name="resolution">${mapping_Map_method_zoomToResolution_Double_param_resolution}</param>
        public void ZoomToResolution(double resolution)
        {
            this.ZoomToResolution(resolution, Point2D.Empty);
        }

        /// <summary>${mapping_Map_method_zoomToResolution_Double_Point2D_D}</summary>
        /// <param name="resolution">${mapping_Map_method_zoomToResolution_Double_param_resolution}</param>
        /// <param name="center">${mapping_Map_method_zoomToResolution_Double_Point2D_param_center}</param>
        public void ZoomToResolution(double resolution, Point2D center)
        {
            this.ZoomToResolution(resolution, center, false);
        }

        /// <summary>
        /// ${mapping_Map_method_zoomToResolution_Double_Point2D_Bool_D}
        /// </summary>
        /// <param name="resolution">${mapping_Map_method_zoomToResolution_Double_param_resolution}</param>
        /// <param name="center">${mapping_Map_method_zoomToResolution_Double_Point2D_param_center}</param>
        /// <param name="skipAnimation"></param>
        public void ZoomToResolution(double resolution, Point2D center, bool skipAnimation)
        {
            if (panHelper != null)
            {
                panHelper.Cancel();
            }
            //if (this.CurrentZoomAnimationHandler != null)
            //{
            //    CompositionTarget.Rendering -= this.CurrentZoomAnimationHandler;
            //    this.CurrentZoomAnimationHandler = null;
            //}
            resolution = MathUtil.MinMaxCheck(resolution, MinResolution, MaxResolution);
            targetResolution = resolution;
            double ratio = resolution / this.mapResolution;
            if (!DoubleUtil.AreClose(1.0, ratio) && panHelper != null)//如果同一级别，请pan
            {
                Rectangle2D bounds = ViewBounds;//如果没有初始化好，这个为空,mapResolution也为nan
                if (!Rectangle2D.IsNullOrEmpty(bounds))
                {
                    if (Point2D.IsNullOrEmpty(center))
                    {
                        center = bounds.Center;
                    }//非数字
                    MapManipulator = MapManipulator.Program;
                    ZoomHelper.DeltaZoom(resolution, center, this.ZoomDuration);
                    //Rectangle2D targetBounds = new Rectangle2D(center.X - (currentSize.Width / 2) * resolution, center.Y - (currentSize.Height / 2) * resolution, center.X + (currentSize.Width / 2) * resolution, center.Y + (currentSize.Height / 2) * resolution);

                    //beginZoomToTargetBounds(targetBounds, skipAnimation);
                }
            }
            else
            {
                if (!Point2D.IsNullOrEmpty(center))
                {
                    MapManipulator = MapManipulator.Program;
                    this.Pan(center.X - ViewBounds.Center.X, center.Y - ViewBounds.Center.Y);
                }
            }
        }
        /// <summary>
        /// ${mapping_Map_method_ManipulationZoom_D}
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="center"></param>
        /// <param name="mapStatus"></param>
        public void ManipulationZoom(double resolution, Point2D center, MapStatus mapStatus)
        {
            if (this.ZoomHelper != null && (mapStatus == MapStatus.ZoomStarted || mapStatus == MapStatus.Zooming || mapStatus == MapStatus.ZoomCompleted))
            {
                if (panHelper != null)
                {
                    panHelper.Cancel();
                }
                if (ZoomHelper != null)
                {
                    ZoomHelper.Cancel();
                }
                this.MapManipulator = MapManipulator.Device;
                this.MapStatus = mapStatus;
                targetResolution = resolution;
                this.ZoomHelper.DeltaZoom(resolution, center, TimeSpan.FromMilliseconds(0));
            }
        }

        /// <summary>${mapping_Map_method_pan_D_sl}</summary>
        /// <param name="offsetX">${mapping_Map_method_pan_param_offsetX}</param>
        /// <param name="offsetY">${mapping_Map_method_pan_param_offsetY}</param>
        public void Pan(double offsetX, double offsetY)
        {
            Point2D newCenter = Point2D.Empty;
            newCenter.X = ViewBounds.Center.X + offsetX;
            newCenter.Y = ViewBounds.Center.Y + offsetY;
            this.PanTo(newCenter);
        }

        //属性
        internal Popup Popup { get; private set; }
        /// <summary>${mapping_Map_attribute_metadata_D}</summary>
        public Dictionary<string, string> Metadata { get; set; }

        //TODO:理论上应该也向Resolutions那样可变
        private CoordinateReferenceSystem mapCRS;
        /// <summary>${mapping_Map_attribute_CRS_D}</summary>
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

        /// <summary>${mapping_Map_attribute_bounds_D}</summary>
        public Rectangle2D Bounds
        {
            get
            {
                return Layers.GetBounds(this.CRS);
            }
        }

        /// <summary>${mapping_Map_attribute_viewBounds_D}</summary>
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

                Point currentOffset = GetCurrentOffset();
                double left = this.origin.X + (-currentOffset.X * this.mapResolution);
                double top = this.origin.Y - (-currentOffset.Y * this.mapResolution);
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

        /// <summary>${mapping_Map_attribute_center_D}</summary>
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
        /// <summary>${mapping_Map_attribute_level_D}</summary>
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

        /// <summary>${mapping_Map_method_zoomToLevel_Int_D}</summary>
        /// <overloads>${mapping_Map_method_zoomToLevel_overloads_D}</overloads>
        /// <param name="level">${mapping_Map_method_zoomToLevel_Int_param_level}</param>
        public void ZoomToLevel(int level)
        {
            ZoomToLevel(level, Point2D.Empty);
        }

        /// <summary>${mapping_Map_method_zoomToLevel_Int_Point2D_D}</summary>
        /// <param name="level">${mapping_Map_method_zoomToLevel_Int_param_level}</param>
        /// <param name="center">${mapping_Map_method_zoomToLevel_Int_Point2D_param_center}</param>
        public void ZoomToLevel(int level, Point2D center)
        {
            if (Resolutions != null)
            {
                level = level < 0 ? 0 : level;
                level = level > (Resolutions.Length - 1) ? (Resolutions.Length - 1) : level;
                ZoomToResolution(Resolutions[level], center, false);
            }
        }

        /// <summary>${mapping_Map_method_zoomIn_D}</summary>
        public void ZoomIn()
        {
            if (this != null)
            {
                this.ZoomToResolution(this.GetNextResolution(true));
            }
        }

        /// <summary>${mapping_Map_method_zoomOut_D}</summary>
        public void ZoomOut()
        {
            if (this != null)
            {
                this.ZoomToResolution(this.GetNextResolution(false));
            }
        }

        /// <summary>${mapping_Map_attribute_minResolution_D}</summary>
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
        /// <summary>${mapping_Map_attribute_maxResolution_D}</summary>
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
        /// <summary>${mapping_Map_attribute_Resolution_D}</summary>
        public double Resolution
        {
            get
            {
                if ((this.layerCollectionContainer != null) && (!Rectangle2D.IsNullOrEmpty(this.ViewBounds)) && (base.ActualWidth != 0.0))
                {
                    return mapResolution;
                }
                return double.NaN;
            }
        }

        private bool fromLayers = false;
        private bool hasChanged = false;
        private double[] oldResolutions;
        //fromLayers = true;// && !fromLayers
        /// <summary>${mapping_Map_attribute_Resolutions_D}</summary>
        /// <example>
        /// <code>
        ///  Double[] resolutions = new Double[] {0.28526148969889065,0.14263074484944532,0.071315372424722662,0.035657686212361331,0.017828843106180665,0.0089144215530903327,0.0044572107765451664,0.0022286053882725832,
        ///  0.0011143026941362916,0.00055715134706814579,0.0002785756735340729,0.00013928783676703645,0.000069643918383518224,0.000034821959191759112};
        ///   MyMap.Resolutions = resolutions;
        /// </code>
        /// </example>
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
        /// <summary>${mapping_Map_attribute_Scales_D}</summary>
        /// <example>
        /// <code>
        /// double[] sacles = new double[] {0.000000025,0.00000005, 0.0000001, 0.0000002, 0.0000004, 0.0000008 };
        /// </code>
        /// </example>
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
        /// <summary>${mapping_Map_attribute_minScale_D}</summary>
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
                    return ScaleHelper.ScaleConversion(this.MaxResolution, this.Dpi, this.CRS); ;
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
                    this.MaxResolution = ScaleHelper.ScaleConversion(minScale, this.Dpi, this.CRS); ;
                }//尽管第一次由于dpi为0不行，但设置好，随机设置最大最小，就有作用了
                this.raisePropertyChanged("MinScale");
            }

        }
        /// <summary>${mapping_Map_attribute_maxScale_D}</summary>
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
        /// <summary>${mapping_Map_attribute_Scale_D}</summary>
        public double Scale
        {
            get
            {
                return ScaleHelper.ScaleConversion(this.Resolution, this.Dpi, this.CRS);
            }
        }

        /// <summary>${mapping_Map_method_zoomToScale_Int_D}</summary>
        /// <overloads>${mapping_Map_method_zoomToScale_overloads_D}</overloads>
        /// <param name="scale">${mapping_Map_method_zoomToScale_Int_param_scale}</param>
        public void ZoomToScale(double scale)
        {
            ZoomToScale(this.ClipScale(scale), Point2D.Empty);
        }
        /// <summary>${mapping_Map_method_zoomToScale_Int_Point2D_D}</summary>
        /// <param name="scale">${mapping_Map_method_zoomToScale_Int_param_scale}</param>
        /// <param name="center">${mapping_Map_method_zoomToScale_Int_Point2D_param_center}</param>
        public void ZoomToScale(double scale, Point2D center)
        {
            double res = double.NaN;
            if (this.Dpi != 0.0)
            {
                res = ScaleHelper.ScaleConversion(scale, this.Dpi, this.CRS); ;
            }
            ZoomToResolution(res, center);
        }
        internal double ClipScale(double scale)
        {
            if (!double.IsNaN(this.MinScale))
            {
                scale = Math.Max(scale, this.MinScale);
            }
            if (!double.IsNaN(this.MaxScale))
            {
                scale = Math.Min(scale, this.MaxScale);
            }
            if (scale < double.Epsilon)
            {
                scale = double.Epsilon;
            }
            return scale;
        }
        #endregion

        /// <summary>${mapping_Map_field_PanDurationProperty_D}</summary>
        public static readonly DependencyProperty PanDurationProperty = DependencyProperty.Register("PanDuration", typeof(TimeSpan), typeof(Map), new PropertyMetadata(TimeSpan.FromSeconds(0.25)));
        /// <summary>${mapping_Map_attribute_PanDuration_D}</summary>
        /// <example>
        /// <code>
        /// MyMap.PanDuration = TimeSpan.FromSeconds(3);
        /// </code>
        /// </example>
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
        /// <summary>${mapping_Map_field_ZoomDurationProperty_D}</summary>
        public static readonly DependencyProperty ZoomDurationProperty = DependencyProperty.Register("ZoomDuration", typeof(TimeSpan), typeof(Map), new PropertyMetadata(TimeSpan.FromSeconds(0.25)));
        /// <summary>${mapping_Map_attribute_ZoomDuration_D}</summary>
        /// <example>
        /// <code>
        ///  MyMap.ZoomDuration = TimeSpan.FromSeconds(3);
        /// </code>
        /// </example>
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

        /// <summary>${mapping_Map_field_layersProperty_D}</summary>
        public static readonly DependencyProperty LayersProperty =
          DependencyProperty.Register("Layers", typeof(LayerCollection), typeof(Map), new PropertyMetadata(null, new PropertyChangedCallback(Map.OnLayersPropertyChanged)));
        /// <summary>${mapping_Map_attribute_Layers_D}</summary>
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

        /// <summary>${pubilc_fields_identifies_sl} <see cref="Angle">Angle</see>
        ///         ${pubilc_fields_attachedproperty_sl}</summary>
        public static readonly DependencyProperty AngleProperty =
           DependencyProperty.Register("Angle", typeof(double), typeof(Map), new PropertyMetadata(null, new PropertyChangedCallback(OnAnglePropertyChanged)));
        /// <summary>${mapping_Map_attribute_angle_D}</summary>
        /// <remarks>
        /// 	<para>
        ///     ${mapping_Map_attribute_angle_default}</para>${mapping_Map_attribute_angle_Exception}</remarks>
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

            this.LoadLayersInView(false, this.GetFullViewBounds());
        }

        /// <summary>
        /// 	<see cref="AngleChanged">AngleChanged</see> ${pubilc_delegates_description_sl}</summary>
        public delegate void AngleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e);
        /// <summary>${mapping_Map_event_angleChanged_D}</summary>
        public event AngleChangedEventHandler AngleChanged;
        #endregion


        //在Pan Action中用到，其实可以不用
        internal Canvas TransformCanvas { get { return this.transformCanvas; } }
        internal Grid RootElement { get { return this.rootElement; } }
        internal PanAnimation PanHelper { get { return this.panHelper; } }

        internal ZoomAnimation ZoomHelper { get; set; }

        //一般可set的属性，变化时都触发下
        /// <summary>${mapping_Map_event_propertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void raisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>${mapping_Map_attribute_Theme_D}</summary>
        public Theme.Theme Theme { get; set; }
        /// <summary>
        /// ${mapping_Map_method_Dispose_D}
        /// </summary>
        public void Dispose()
        {
            base.Loaded -= Map_Loaded;
            base.SizeChanged -= Map_SizeChanged;
            Layers.CollectionChanged -= Layers_CollectionChanged;
            Layers.LayersInitialized -= Layers_LayersInitialized;
            Layers.Progress -= layersProgressHandler;
            if (zoomThrottleTimer != null)
            {
                zoomThrottleTimer.Cancel();
            }
            if (mapResizeThrottler != null)
            {
                mapResizeThrottler.Cancel();
            }
            if (panHelper != null)
            {
                panHelper.Panning -= panHelper_Panning;
                panHelper.PanAnimationCompleted -= panHelper_PanAnimationCompleted;
                panHelper.Cancel();
            }
            if (ZoomHelper != null)
            {
                ZoomHelper.Cancel();
                ZoomHelper.ZoomStarted -= ZoomHelper_ZoomStarted;
                ZoomHelper.Zooming -= ZoomHelper_Zooming;
                ZoomHelper.ZoomCompleted -= ZoomHelper_ZoomCompleted;
            }
            if (Action != null)
            {
                Action.Deactivate();
                Action.Dispose();
            }

            for (int i = Layers.Count - 1; i >= 0; i--)
            {
                RemoveMapLayers(Layers[i]);
                Layers.RemoveAt(i);
            }

            QueueSystem.Instance.Dispose();
        }
    }
}
