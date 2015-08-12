using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Actions;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_OverviewMap_Title}</para>
    /// 	<para>${controls_OverviewMap_Description}</para>
    /// 	<para><img src="OverviewMap.png"/></para>
    /// </summary>
    [TemplatePart(Name = "OuterRect", Type = typeof(Grid))]
    [TemplatePart(Name = "OvMap", Type = typeof(Map))]
    [TemplatePart(Name = "OvRect", Type = typeof(Grid))]
    [TemplatePart(Name = "ArrowButton", Type = typeof(Button))]
    [TemplatePart(Name = "ArrowButtonCopy", Type = typeof(Button))]
    [ContentProperty(Name = "Layer")]
    public partial class OverviewMap : Control
    {
        private Button arrowButton;
        private Button arrowButtonCopy;

        private Grid outerRect;
        private Map ovMap;
        private Grid ovRect;
        private Rectangle2D lastMapViewBounds = Rectangle2D.Empty;
        private Point startPoint;
        double offsetLeft = 0;
        double offsetTop = 0;
        private bool isDragging = false;


        /// <summary>${controls_OverviewMap_constructor_None_D}</summary>
        public OverviewMap()
        {
            DefaultStyleKey = typeof(OverviewMap);
            this.Layers = new LayerCollection();
            this.Layers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Layers_CollectionChanged);
        }

        /// <summary>${controls_OverviewMap_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            outerRect = GetTemplateChild("OuterRect") as Grid;
            outerRect.Width = base.Width;
            outerRect.Height = base.Height;

            arrowButton = GetTemplateChild("ArrowButton") as Button;
            if (arrowButton != null)
            {
                arrowButton.Click += new RoutedEventHandler(Button_Click);
            }
            arrowButtonCopy = GetTemplateChild("ArrowButtonCopy") as Button;
            arrowButtonCopy.Click += new RoutedEventHandler(Button_Click);
            ovMap = GetTemplateChild("OvMap") as Map;
            if (ovMap == null)
            {
                throw new ArgumentNullException("OvMap");
            }
            ovMap.MinResolution = double.Epsilon;
            ovMap.MaxResolution = double.MaxValue;

            if (Map != null)
            {
                ovMap.Action = new OvPan(ovMap, Map);

                //确保在Map上设置了Resolution将其传到ovMap上；
                //if (Map.Resolutions != null)
                //{
                //    ovMap.Resolutions = Map.Resolutions;
                //}
            }

            ovMap.Width = base.Width - 16;
            ovMap.Height = base.Height - 16;
            ovMap.ViewBoundsChanged += (s, e) => { UpdateOvRect(); };
            if (Layers != null && Layers.Count > 0)  //只要Layers有值就以Layers为准
            {
                ovMap.Layers.Clear();
                foreach (var item in Layers)
                {
                    ovMap.Layers.Add(item);
                }
            }
            else if (Layer != null)
            {
                ovMap.Layers.Add(Layer);
            }

            ovRect = GetTemplateChild("OvRect") as Grid;
            if (ovRect != null)
            {
                ovRect.PointerPressed += ovRect_PointerPressed;
            }
            UpdateOvRect();

            if (CollapsedInTheInitialization)
            {
                outerRect.Visibility = Visibility.Collapsed;
                arrowButtonCopy.Visibility = Visibility.Collapsed;
            }
            else
            {
                outerRect.Visibility = Visibility.Visible;
                arrowButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (arrowButton.Visibility == Visibility.Visible)
            {
                arrowButton.Visibility = Visibility.Collapsed;
                arrowButtonCopy.Visibility = Visibility.Visible;

                outerRect.Visibility = Visibility.Visible;
                VisualStateManager.GoToState(this, "Expanded", true);
            }
            else
            {
                arrowButton.Visibility = Visibility.Visible;
                arrowButtonCopy.Visibility = Visibility.Collapsed;
                VisualStateManager.GoToState(this, "Collapsed", true);
            }
        }

        #region Properties

        /// <summary>${controls_OverviewMap_field_MapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(Map), typeof(OverviewMap), new PropertyMetadata(null,OnMapPropertyChanged));
        private static void OnMapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OverviewMap overviewMap = d as OverviewMap;
            Map oldMap = e.OldValue as Map;
            if (oldMap != null)
            {
                if (overviewMap.ovMap != null)
                {
                    overviewMap.ovMap.Layers.Clear();
                }
                oldMap.ViewBoundsChanged -= (sender, args) =>
                {
                    overviewMap.UpdateOvMap();
                    //overviewMap.UpdateOvRect();
                };

            }
            Map newMap = e.NewValue as Map;
            if (newMap != null)
            {
                newMap.ViewBoundsChanged += (sender, args) =>
                {
                    overviewMap.UpdateOvMap();
                    //overviewMap.UpdateOvRect();
                };

                if (overviewMap.ovMap != null && overviewMap.Layers != null && overviewMap.Layers.Count > 0)
                {
                    overviewMap.ovMap.Layers.Clear();
                    foreach (var item in overviewMap.Layers)
                    {
                        overviewMap.ovMap.Layers.Add(item);
                    }
                    overviewMap.ovMap.Action = new OvPan(overviewMap.ovMap, newMap);
                }
                else if (overviewMap.Layer != null && overviewMap.ovMap != null)
                {
                    overviewMap.ovMap.Layers.Clear();
                    overviewMap.ovMap.Layers.Add(overviewMap.Layer);
                    overviewMap.ovMap.Action = new OvPan(overviewMap.ovMap, newMap);
                }
            }
        }

        /// <summary>${controls_OverviewMap_attribute_Map_D}</summary>
        public Map Map
        {
            get { return (Map)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        /// <summary>${controls_OverviewMap_field_LayerProperty_D}</summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register("Layer", typeof(Layer), typeof(OverviewMap), new PropertyMetadata(null,OnLayerPropertyChanged));
        private static void OnLayerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OverviewMap overviewMap = d as OverviewMap;
            Layer oldLayer = e.OldValue as Layer;
            if (overviewMap.ovMap != null)
            {
                if (overviewMap.Layer != null && (overviewMap.Layers == null || overviewMap.Layers.Count < 1))  //Layers初始不是空
                {
                    overviewMap.ovMap.Layers.Clear();   //清除ovMap上的图层。
                    overviewMap.ovMap.Layers.Add(overviewMap.Layer);  //添加新图层。

                    if (overviewMap.Layer.IsInitialized)
                    {
                        overviewMap.Layer_LayersInitialized(overviewMap.Layer, null);
                    }
                    else
                    {
                        overviewMap.Layer.Initialized += overviewMap.Layer_LayersInitialized;
                    }
                }
                else if (overviewMap.Layers != null)//把Layers里面的值给鹰眼地图
                {

                    overviewMap.ovMap.Layers.Clear();

                    foreach (var item in overviewMap.Layers)
                    {
                        overviewMap.ovMap.Layers.Add(item);

                        if (item.IsInitialized)
                        {
                            overviewMap.Layer_LayersInitialized(item, null);
                        }
                        else
                        {
                            item.Initialized += overviewMap.Layer_LayersInitialized;
                        }
                    }
                }

                if (oldLayer != null)
                {
                    oldLayer.Initialized -= overviewMap.Layer_LayersInitialized;
                }
            }
        }

        /// <summary>${controls_OverviewMap_attribute_Layer_D}</summary>
        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        /// <summary>${controls_OverviewMap_attribute_Layers_D}</summary>
        public LayerCollection Layers    //更换了该类的内容属性，原先通过xaml设置的layer现在默认设置Layers。
        {                                //通过cs代码设置Layer，属性值无变化。
            get
            {
                return (LayerCollection)GetValue(LayersProperty);
            }
            set
            {
                SetValue(LayersProperty, value);
            }
        }
        /// <summary>${controls_OverviewMap_field_LayersProperty_D}</summary>
        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register("LayersProperty", typeof(LayerCollection), typeof(OverviewMap), new PropertyMetadata(null,new PropertyChangedCallback(OverviewMap.OnLayersPropertyChanged)));

        private static void OnLayersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OverviewMap overviewMap = d as OverviewMap;
            LayerCollection oldLayers = e.OldValue as LayerCollection;
            LayerCollection newLayers = e.NewValue as LayerCollection;

            if (overviewMap.ovMap != null)
            {
                overviewMap.ovMap.Layers.Clear();
                if (overviewMap.Layers != null)
                {
                    newLayers.CollectionChanged += overviewMap.Layers_CollectionChanged;
                    overviewMap.ovMap.Layers.Clear();

                    foreach (var item in overviewMap.Layers)
                    {
                        overviewMap.ovMap.Layers.Add(item);

                        if (item.IsInitialized)
                        {
                            overviewMap.Layer_LayersInitialized(item, null);
                        }
                        else
                        {
                            item.Initialized += overviewMap.Layer_LayersInitialized;
                        }
                    }
                }
                else  //TODO: 看看Layer属性有没有值。
                {
                    if (overviewMap.Layer != null)
                    {
                        if (overviewMap.Layer.IsInitialized)
                        {
                            overviewMap.Layer_LayersInitialized(overviewMap.Layer, null);
                        }
                        else
                        {
                            overviewMap.Layer.Initialized += overviewMap.Layer_LayersInitialized;
                        }
                    }
                }
            }

            if (oldLayers != null && oldLayers.Count > 0)
            {
                foreach (var item in oldLayers)
                {
                    item.Initialized -= overviewMap.Layer_LayersInitialized;
                }
                oldLayers.CollectionChanged -= overviewMap.Layers_CollectionChanged;
            }
        }

        private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //当Layers的count为>0时，鹰眼地图显示的是Layers里面的图层。否则查看Layer属性是否有效。
            if (this.ovMap != null && this.Layers != null && this.Layers.Count > 0)
            {
                this.ovMap.Layers.Clear();   //清除鹰眼地图中的图层，添加Layers里面的图层。
                foreach (var item in this.Layers)
                {
                    this.ovMap.Layers.Add(item);
                    if (item.IsInitialized)
                    {
                        this.Layer_LayersInitialized(item, null);
                    }
                    else
                    {
                        item.Initialized += Layer_LayersInitialized;
                    }
                }
            }
            else  //把Layer里面的值给鹰眼中的地图
            {
                if (this.Layer != null)
                {
                    if (this.Layer.IsInitialized)
                    {
                        this.Layer_LayersInitialized(this.Layer, null);
                    }
                    else
                    {
                        this.Layer.Initialized += Layer_LayersInitialized;
                    }
                }
            }

            UpdateOvMap();
        }

        /// <summary>${controls_OverviewMap_attribute_DisableOverviewMapZoom_D}</summary>
        public bool DisableOverviewMapZoom
        {
            get { return (bool)GetValue(DisableOverviewMapZoomProperty); }
            set { SetValue(DisableOverviewMapZoomProperty, value); }
        }

        /// <summary>${controls_OverviewMap_field_DisableOverviewMapZoomProperty_D}</summary>
        public static readonly DependencyProperty DisableOverviewMapZoomProperty =
            DependencyProperty.Register("DisableOverviewMapZoom", typeof(bool), typeof(OverviewMap), new PropertyMetadata(true, new PropertyChangedCallback(OnDisableOverviewMapZoomPropertyChanged)));

        private static void OnDisableOverviewMapZoomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as OverviewMap).UpdateOvMap();
        }
        /// <summary>${controls_OverviewMap_attribute_CollapsedInTheInitialization_D}</summary>
        public bool CollapsedInTheInitialization { get; set; }

        #endregion

        /// <summary>${controls_OverviewMap_method_ArrangeOverride_D}</summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };
            return base.ArrangeOverride(finalSize);
        }

        private void Layer_LayersInitialized(object sender, EventArgs args)
        {
            UpdateOvMap();
        }

        #region 鹰眼联动
        private void UpdateOvMap()
        {
            if (Map == null || ovMap == null || Rectangle2D.IsNullOrEmpty(Map.ViewBounds) || Rectangle2D.IsNullOrEmpty(ovMap.ViewBounds))
            {
                if (ovRect != null)
                {
                    ovRect.Visibility = Visibility.Collapsed;
                }
                return;
            }
            double mapWidth = Map.ViewBounds.Width;
            double mapHeight = Map.ViewBounds.Height;
            double ovWidth = ovMap.ViewBounds.Width;
            double ovHeight = ovMap.ViewBounds.Height;
            double widthRatio = mapWidth / ovWidth;
            double heightRatio = mapHeight / ovHeight;
            double minRatio = 0.15;
            double maxRatio = 0.8;

            if (mapWidth >= Map.Bounds.Width)
            {
                ovMap.ZoomTo(Map.ViewBounds);
                return;
            }//主视图较小

            bool pan = (Math.Abs(mapWidth - lastMapViewBounds.Width) < 1e-6 && Math.Abs(mapHeight - lastMapViewBounds.Height) < 1e-6);
            if (pan)//只是平移
            {
                double halfWidth = ovWidth / 2;
                double halfHeight = ovHeight / 2;
                Point2D newCenter = Map.ViewBounds.Center;
                ovMap.PanTo(newCenter);
            }
            else
            {
                if ((widthRatio <= minRatio || heightRatio <= minRatio || widthRatio >= maxRatio || heightRatio >= maxRatio) && DisableOverviewMapZoom)
                {
                    if (ovRect != null)
                    {
                        ovRect.Visibility = Visibility.Collapsed;
                    }

                    if (Map.Bounds.Width > mapWidth * 3)
                    {
                        Rectangle2D viewBounds = new Rectangle2D(Map.ViewBounds.Left - mapWidth, Map.ViewBounds.Bottom - mapHeight, Map.ViewBounds.Right + mapWidth, Map.ViewBounds.Top + mapHeight);
                        ovMap.ZoomTo(viewBounds);
                    }
                    else
                    {
                        ovMap.ZoomTo(Map.ViewBounds);
                    }

                }//根据主视图的大小，对鹰眼图进行一定的缩放
                else
                {
                    ovMap.PanTo(Map.ViewBounds.Center);
                }//鹰眼图并不发生缩放
            }//zoom
        }

        //private void UpdateOvMap(object sender, ViewBoundsEventArgs e)
        //{
        //    UpdateOvMap();
        //}

        #endregion

        private void UpdateOvRect()
        {
            if (Map == null || ovMap == null || Rectangle2D.IsNullOrEmpty(ovMap.ViewBounds) || ovRect == null)
            {
                return;
            }

            Rectangle2D viewBounds = Map.ViewBounds;
            if (Rectangle2D.IsNullOrEmpty(viewBounds))
            {
                ovRect.Visibility = Visibility.Collapsed;
                return;
            }

            Point2D pt1 = new Point2D(viewBounds.Left, viewBounds.Top);
            Point2D pt2 = new Point2D(viewBounds.Right, viewBounds.Bottom);
            Point topLeft = ovMap.MapToScreen(pt1);
            Point bottomRight = ovMap.MapToScreen(pt2);

            if (!double.IsNaN(topLeft.X) || !double.IsNaN(topLeft.Y) || !double.IsNaN(bottomRight.X) || !double.IsNaN(bottomRight.Y))
            {
                ovRect.Margin = new Thickness(topLeft.X, topLeft.Y, 0, 0);
                ovRect.Width = bottomRight.X - topLeft.X;
                ovRect.Height = bottomRight.Y - topLeft.Y;
                if (topLeft.X < 0 || topLeft.Y < 0)
                {
                    ovRect.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ovRect.Visibility = Visibility.Visible;
                }

            }
            else
            {
                ovRect.Visibility = Visibility.Collapsed;
            }
            lastMapViewBounds = viewBounds;

        }

        private void UpdateMap()
        {
            if (ovRect != null)
            {
                double aoiLeft = ovRect.Margin.Left;
                double aoiTop = ovRect.Margin.Top;
                Point2D pt = ovMap.ScreenToMap(new Point(aoiLeft, aoiTop));

                Point2D pnt = new Point2D(pt.X + Map.ViewBounds.Width / 2, pt.Y - Map.ViewBounds.Height / 2);
                Map.PanTo(pnt);
            }
        }

        #region 鹰眼中移动索引框
        private void ovRect_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isDragging = true;
            startPoint = e.GetCurrentPoint(this).Position;
            offsetLeft = startPoint.X - ovRect.Margin.Left;
            offsetTop = startPoint.Y - ovRect.Margin.Top;
            ovRect.PointerMoved += ovRect_PointerMoved;
            ovRect.PointerReleased += ovRect_PointerReleased;
            ovRect.CapturePointer(e.Pointer);
        }

        private void ovRect_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                ovRect.PointerMoved -= ovRect_PointerMoved;
                ovRect.PointerReleased -= ovRect_PointerReleased;
                UpdateMap();
                isDragging = false;
                ovRect.ReleasePointerCaptures();
            }
        }

        private void ovRect_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                Point pos = e.GetCurrentPoint(this).Position;
                ovRect.Margin = new Thickness(pos.X - offsetLeft, pos.Y - offsetTop, 0, 0);
            }
        }
        #endregion

    }
}
