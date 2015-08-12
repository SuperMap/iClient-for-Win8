using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
//暂时不加入聚合功能。
//using SuperMap.WinRT.Clustering;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Rendering;
using SuperMap.WinRT.Resources;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Core;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// ${mapping_FeaturesLayer_Title}<br/>
    /// ${mapping_FeaturesLayer_Description}
    /// </summary>
    [ContentProperty(Name="Features")]
    public class FeaturesLayer : Layer, IEnumerable<Feature>, IEnumerable
    {
        //暂时不加入聚合功能。
        //#region Clusterer 依赖项属性
        ///// <summary>
        /////     ${pubilc_fields_identifies_sl} <see cref="Clusterer">Cluster</see>
        /////     ${pubilc_fields_attachedproperty_sl}
        ///// </summary>
        //public static readonly DependencyProperty ClustererProperty = DependencyProperty.Register("Clusterer", typeof(Clusterer), typeof(FeaturesLayer), new PropertyMetadata(null, new PropertyChangedCallback(FeaturesLayer.OnClustererPropertyChanged)));
        ///// <summary>${mapping_FeaturesLayer_attribute_cluster_D}</summary>
        //public Clusterer Clusterer
        //{
        //    get
        //    {
        //        return (Clusterer)base.GetValue(ClustererProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(ClustererProperty, value);
        //    }
        //}
        //private static void OnClustererPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    FeaturesLayer layer = d as FeaturesLayer;
        //    Clusterer oldValue = e.OldValue as Clusterer;
        //    Clusterer newValue = e.NewValue as Clusterer;
        //    if (oldValue != null)
        //    {
        //        oldValue.ClusteringCompleted -= new EventHandler<Clusterer.ClusterEventArgs>(layer.Clusterer_ClustererCompleted);
        //        oldValue.PropertyChanged -= new PropertyChangedEventHandler(layer.oldValue_PropertyChanged);
        //    }
        //    if (newValue != null)
        //    {
        //        newValue.ClusteringCompleted += new EventHandler<Clusterer.ClusterEventArgs>(layer.Clusterer_ClustererCompleted);
        //        newValue.PropertyChanged += new PropertyChangedEventHandler(layer.oldValue_PropertyChanged);
        //    }
        //    layer.clusterCache = null;
        //    layer.clusterResolution = double.NaN;
        //    layer.Refresh();
        //    layer.OnPropertyChanged("Clusterer");
        //}

        //private void oldValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "RegionCollection")
        //        this.Refresh();
        //}

        //private void Clusterer_ClustererCompleted(object sender, Clusterer.ClusterEventArgs e)
        //{
        //    this.oldClusterCache = this.clusterCache;
        //    this.clusterCache = e.Clusters;
        //    this.Invalidate(true);
        //}

        //#endregion
        private IEnumerable<Feature> oldClusterCache;//让GC自动回收
        private IEnumerable<Feature> clusterCache;
        private double clusterResolution;       //楼上三个Cluster所用

        private Rectangle2D fullBounds = Rectangle2D.Empty;
        private IRenderer renderer;

        /// <summary>${mapping_FeaturesLayer_attribute_features_D}</summary>
        public FeatureCollection Features
        {
            get { return (FeatureCollection)GetValue(FeaturesProperty); }
            set { SetValue(FeaturesProperty, value); }
        }
        /// <summary>${mapping_FeaturesLayer_field_features_D}</summary>
        public static readonly DependencyProperty FeaturesProperty =
            DependencyProperty.Register("Features", typeof(FeatureCollection), typeof(FeaturesLayer), new PropertyMetadata(null,new PropertyChangedCallback(OnFeaturesPropertyChanged)));
        private static void OnFeaturesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FeaturesLayer layer = (FeaturesLayer)d;
            FeatureCollection newValue = (FeatureCollection)e.NewValue;
            FeatureCollection oldValue = (FeatureCollection)e.OldValue;

            if (oldValue != null)
            {
                layer.clusterResolution = double.NaN;
                oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(layer.features_CollectionChanged);
                oldValue.CollectionClearing -= new EventHandler(layer.Features_CollectionClearing);
                foreach (Feature f in oldValue)
                {
                    layer.ClearFeature(f);
                }
                bool flag = layer._selectedFeatures.Count > 0;
                layer._selectedFeatures.Clear();
                if (flag)
                {
                    layer.raiseSelectedFeaturesChange(null, false);
                }
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(layer.features_CollectionChanged);
                newValue.CollectionClearing += new EventHandler(layer.Features_CollectionClearing);
                foreach (Feature f2 in newValue)
                {
                    layer.addfeature(f2);
                }
            }
            //layer.clusterResolution = double.NaN;
            layer.Invalidate(false);
            layer.OnPropertyChanged("Features");
        }


        //私有附加属性
        //一个Feature只能属于一个FeaturesLayer
        internal static readonly DependencyProperty FeaturesLayerProperty = DependencyProperty.RegisterAttached("FeaturesLayer", typeof(FeaturesLayer), typeof(FeaturesLayer), null);
        //Geometry是否需要重绘
        private static readonly DependencyProperty IsGeometryDirtyProperty = DependencyProperty.RegisterAttached("IsGeometryDirty", typeof(bool), typeof(FeaturesLayer), new PropertyMetadata(false));

        #region 附加项属性 ToolTipVerticalOffset、 ToolTipHorizontalOffset 、 ToolTipHideDelay

        /// <summary>
        ///     ${pubilc_fields_identifies_sl} <see cref="GetToolTipHideDelay">ToolTipHideDelayProperty</see>
        ///     ${pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty ToolTipHideDelayProperty =
            DependencyProperty.RegisterAttached("ToolTipHideDelay", typeof(TimeSpan), typeof(FeaturesLayer), new PropertyMetadata(TimeSpan.FromSeconds(0.0)));

        /// <summary>${mapping_FeaturesLayer_method_setToolTipHideDelay_D}</summary>
        /// <param name="o">${mapping_FeaturesLayer_method_setToolTipHideDelay_param_o}</param>
        /// <param name="value">${mapping_FeaturesLayer_method_setToolTipHideDelay_param_value}</param>
        public static void SetToolTipHideDelay(DependencyObject o, TimeSpan value)
        {
            o.SetValue(ToolTipHideDelayProperty, value);
        }

        /// <summary>${mapping_FeaturesLayer_method_getToolTipHideDelay_D}</summary>
        /// <returns>${mapping_FeaturesLayer_method_getToolTipHideDelay_return}</returns>
        /// <param name="o">${mapping_FeaturesLayer_method_getToolTipHideDelay_param_o}</param>
        public static TimeSpan GetToolTipHideDelay(DependencyObject o)
        {
            return (TimeSpan)o.GetValue(ToolTipHideDelayProperty);
        }

        /// <summary>
        ///     ${pubilc_fields_identifies_sl} <see cref="GetToolTipVerticalOffset">ToolTipVerticalOffset</see>
        ///     ${pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty ToolTipVerticalOffsetProperty = DependencyProperty.RegisterAttached("ToolTipVerticalOffset", typeof(double), typeof(FeaturesLayer),
            new PropertyMetadata(10.0, new PropertyChangedCallback(OnToolTipVerticalOffsetPropertyChanged)));

        private static void OnToolTipVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            Popup parent = element.Parent as Popup;
            if (parent != null)
            {
                parent.VerticalOffset += ((double)e.NewValue) - ((double)e.OldValue);
            }
        }
        /// <summary>${mapping_FeaturesLayer_method_SetToolTipVerticalOffset_D}</summary>
        public static void SetToolTipVerticalOffset(FrameworkElement element, double offset)
        {
            element.SetValue(ToolTipVerticalOffsetProperty, offset);
        }
        /// <summary>${mapping_FeaturesLayer_method_GetToolTipVerticalOffset_D}</summary>
        public static double GetToolTipVerticalOffset(FrameworkElement element)
        {
            return (double)element.GetValue(ToolTipVerticalOffsetProperty);
        }

        /// <summary>
        ///     ${pubilc_fields_identifies_sl} <see cref="GetToolTipHorizontalOffset">ToolTipHorizontalOffset</see>
        ///     ${pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty ToolTipHorizontalOffsetProperty = DependencyProperty.RegisterAttached("ToolTipHorizontalOffset", typeof(double), typeof(FeaturesLayer),
            new PropertyMetadata(10.0, new PropertyChangedCallback(OnToolTipHorizontalOffsetPropertyChanged)));
        private static void OnToolTipHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            Popup parent = element.Parent as Popup;
            if (parent != null)
            {
                parent.HorizontalOffset += ((double)e.NewValue) - ((double)e.OldValue);
            }
        }
        /// <summary>${mapping_FeaturesLayer_method_SetToolTipHorizontalOffset_D}</summary>
        public static void SetToolTipHorizontalOffset(FrameworkElement element, double offset)
        {
            element.SetValue(ToolTipHorizontalOffsetProperty, offset);
        }
        /// <summary>${mapping_FeaturesLayer_method_GetToolTipHorizontalOffset_D}</summary>
        public static double GetToolTipHorizontalOffset(FrameworkElement element)
        {
            return (double)element.GetValue(ToolTipHorizontalOffsetProperty);
        }
        #endregion

        /// <summary>${mapping_FeaturesLayer_event_PointerEntered_D}</summary>
        public event EventHandler<FeaturePointerRoutedEventArgs> PointerEntered;
        /// <summary>${mapping_FeaturesLayer_event_PointerExited_D}</summary>
        public event EventHandler<FeaturePointerRoutedEventArgs> PointerExited;
        /// <summary>${mapping_FeaturesLayer_event_PointerPressed_D}</summary>
        public event EventHandler<FeaturePointerRoutedEventArgs> PointerPressed;
        /// <summary>${mapping_FeaturesLayer_event_PointerReleased_D}</summary>
        public event EventHandler<FeaturePointerRoutedEventArgs> PointerReleased;
        /// <summary>${mapping_FeaturesLayer_event_PointerMoved_D}</summary>
        public event EventHandler<FeaturePointerRoutedEventArgs> PointerMoved;

        private Feature currentTooTipFeature;
        private Feature currentFeature;
        private DispatcherTimer tooltipHideTimer = new DispatcherTimer();

        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="FeaturesLayer">FeaturesLayer</see>
        ///     ${pubilc_Constructors_instance}
        /// </summary>
        public FeaturesLayer()
        {
            this.clusterResolution = double.NaN;
            this.Features = new FeatureCollection();
            base.progressWeight = 0.0;
            tooltipHideTimer.Tick += new EventHandler<object>(tooltipHideTimer_Tick);
        }

        internal override void InitContainer()
        {
            Container = new FeaturesLayerContainer(this);
        }

        #region ToolTip
        private void tooltipHideTimer_Tick(object sender, object e)
        {
            FrameworkElement toolTip = (currentFeature != null) ? currentFeature.ToolTip : this.ToolTip;
            this.CloseToolTip(toolTip);

        }
        private void CloseToolTip(FrameworkElement toolTip)
        {
            currentTooTipFeature = null;
            if (toolTip != null)
            {
                toolTip.PointerEntered -= toolTip_PointerEntered;
                toolTip.PointerExited -= toolTip_PointerExited;
            }
            base.Map.CloseToolTip();
        }
        private void ShowToolTip(Feature f, FrameworkElement _toolTip)
        {
            currentTooTipFeature = f;
            if (_toolTip != null)
            {
                _toolTip.PointerEntered += toolTip_PointerEntered;
                _toolTip.PointerExited += toolTip_PointerExited;
            }
            base.Map.ShowToolTip();
        }

        private void toolTip_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement f = sender as FrameworkElement;
            f.PointerExited -= toolTip_PointerExited;
            if (currentTooTipFeature != currentFeature)
            {
                TimeSpan toolTipHideDelay = GetToolTipHideDelay(f);
                StartHideTimer(toolTipHideDelay);
            }
        }

        private void toolTip_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (sender as FrameworkElement).PointerEntered -= toolTip_PointerEntered;
            this.StopHideTimer();
        }
        private void StartHideTimer(TimeSpan delay)
        {
            if (base.Map.Popup.Child != null)
            {
                if (delay.Ticks > 0L)
                {
                    this.tooltipHideTimer.Interval = delay;
                    this.tooltipHideTimer.Start();
                }
                else
                {
                    FrameworkElement toolTip = (currentFeature != null) ? currentFeature.ToolTip : this.ToolTip;
                    this.CloseToolTip(toolTip);
                }
            }
        }
        private void StopHideTimer()
        {
            this.tooltipHideTimer.Stop();
        }

        #endregion

        /// <summary>${mapping_FeaturesLayer_method_clearFeatures_D}</summary>
        public void ClearFeatures()
        {
            if (this.Features != null)
            {
                this.Features.Clear();
            }
        }
        private void ClearFeature(Feature f)
        {
            if (f.GetElementReference() != null)
            {
                f.SetElementReference(null);
            }
            this.Map.CloseToolTip();
            f.ClearValue(FeaturesLayerProperty);
            //暂时不加入聚合功能。
            //if (this.Clusterer != null)
            //{
            //    this.Clusterer = null;
            //}
            f.PropertyChanged -= new PropertyChangedEventHandler(f_PropertyChanged);
            f.AttributeValueChanged -= new EventHandler<DictionaryChangedEventArgs>(f_AttributeValueChanged);
            f.ElementReferenceChanging -= new EventHandler(f_ElementReferenceChanging);
        }

        private void f_AttributeValueChanged(object sender, DictionaryChangedEventArgs e)
        {
            if (this.Renderer != null)
            {
                Feature f = sender as Feature;
                SuperMap.WinRT.Core.Style style = this.renderer.GetStyle(f);
                if (f.dataContext.Style != style)
                {
                    f.dataContext.Style = style;
                    if (style == null)
                    {
                        f.SetElementReference(null);
                    }
                    else
                    {
                        FeatureElement elementReference = f.GetElementReference();
                        if (elementReference != null)
                        {
                            if (elementReference.Template != style.ControlTemplate)
                            {
                                elementReference.Template = style.ControlTemplate;
                            }
                            else
                            {
                                elementReference.DataContext = f.dataContext;
                            }
                        }
                    }
                }
            }
        }

        private void f_ElementReferenceChanging(object sender, EventArgs e)
        {
            FeatureElement elementReference = (sender as Feature).GetElementReference();
            if (elementReference != null && base.Container.Children.Contains(elementReference))
            {
                base.Container.Children.Remove(elementReference);
            }
        }


        //暂时不加入聚合功能。
        //#region cluster 相关
        //private void Cluster(double resolution)
        //{
        //    this.Clusterer.CancelAsync();
        //    this.clusterResolution = resolution;
        //    this.Clusterer.ClusterFeaturesAsync(this.Features, resolution);
        //}

        //private void Cluster_TemplateApplied(FeatureElement element, Feature feature)
        //{
        //    IList<Feature> list = feature.GetValue(Clusterer.ClusterProperty) as IList<Feature>;
        //    if (list != null)
        //    {
        //        FeaturesLayer layer = (element.Parent as LayerContainer).Layer as FeaturesLayer;
        //        if (layer != null)
        //        {
        //            DependencyObject child = VisualTreeHelper.GetChild(element, 0);//这都直接用到了
        //            if (child != null)
        //            {
        //                string clusterChildElements = Clusterer.GetClusterChildElements(child);//附加属性的静态方法
        //                if (!string.IsNullOrEmpty(clusterChildElements))
        //                {
        //                    string[] strArray = clusterChildElements.Split(new char[] { ',' });//拆分xaml
        //                    for (int i = 0; i < Math.Min(strArray.Length, list.Count); i++)
        //                    {
        //                        //FrameworkElement cluster = element.GetChild(strArray[i]);
        //                        //if (cluster != null)
        //                        //{
        //                        //    //Feature f = list[i];
        //                        //    cluster.MouseEnter += (s2, e2) => { this.cluster_MouseEnter(element, null); };
        //                        //    //cluster.Tag = handler;//clear up
        //                        //    ////这儿决定了 在MouseEnter的时候 也可以在Cluster时 也判断出来是哪一个feature
        //                        //    ////因此也仅仅支持ToolTip，而不支持MouseDown
        //                        //}
        //                        FrameworkElement cluster = element.GetChild(strArray[i]);
        //                        if (cluster != null)
        //                        {
        //                            Feature f = list[i];
        //                            cluster.PointerEntered += (s, e) =>
        //                                {
        //                                    this.StopHideTimer();
        //                                    layer.element_MouseHover(layer, f, e);
        //                                };
        //                            cluster.PointerExited += (s1, e1) =>
        //                                {
        //                                    layer.element_MouseLeave(f, e1);
        //                                };
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //#endregion

        internal override bool CheckBeforeDraw(UpdateParameter updateParameter)
        {
            if (Map.MapStatus == MapStatus.Zooming)
            {
                return false;
            }
            return true;
        }

        internal override void Draw(UpdateParameter updateParameter)
        {
            Rectangle2D bounds = ViewBounds;

            if (base.IsInitialized && base.IsVisible && !Rectangle2D.IsNullOrEmpty(bounds) && this.Features != null && Features.Count > 0)
            {
                GeoPoint origin = new GeoPoint(LayerOrigin.X, LayerOrigin.Y);
                double resolution = Resolution;

                IEnumerable<Feature> features = this.Features;
                //暂时不加入聚合功能。
                bool isClusterer = false;
                //if (this.Clusterer != null)
                //{
                //    //如果不曾clusterer过就聚集，如果分辨率变化在3/4和4/3之外就（重新）聚集。
                //    if (((this.clusterCache == null) || double.IsNaN(this.clusterResolution)) || ((this.clusterResolution < (resolution * 0.75)) || (this.clusterResolution > (resolution * 1.33))))
                //    {
                //        this.Cluster(resolution);
                //        return;
                //    }//(重新聚集后就此打回)
                //    features = this.clusterCache;
                //    foreach (Feature feature in features)
                //    {
                //        feature.ElementReferenceChanging += new EventHandler(f_ElementReferenceChanging);
                //    }
                //    isClusterer = true;
                //}

                double num = 20.0 * resolution;
                Rectangle2D biggerBounds = bounds.Inflate(num, num);//上下左右扩张20个像素的距离

                drawFeatures(resolution, origin, biggerBounds, features, isClusterer, renderer);
            }
        }

        private void drawFeatures(double resolution, GeoPoint origin, Rectangle2D biggerBounds, IEnumerable<Feature> features, bool isClusterer, IRenderer renderer)
        {
            if (features != null)
            {
                foreach (Feature feature in features)
                {
                    if (feature.Geometry == null)
                    {
                        feature.SetElementReference(null);
                        continue;
                    }
                    if (feature.Visibility == Visibility.Visible)
                    {
                        Rectangle2D rect = feature.Geometry.Bounds;
                        if (Rectangle2D.IsNullOrEmpty(rect))
                        {
                            feature.SetElementReference(null);
                            continue;
                        }

                        FeatureElement elementReference = feature.GetElementReference();
                        if ((elementReference != null) && (elementReference.Parent != base.Container))
                        {
                            feature.SetElementReference(null);
                            elementReference = null;
                        }
                        if (rect.IntersectsWith(biggerBounds))
                        {
                            if (elementReference != null)
                            {
                                //geometry发生改变，需要重绘
                                if ((bool)feature.GetValue(IsGeometryDirtyProperty))
                                {
                                    elementReference.InvalidatePath(resolution, origin.X, origin.Y);
                                    elementReference.Geometry = feature.Geometry;
                                    if (elementReference.Parent == null)
                                    {
                                        base.Container.Children.Add(elementReference);
                                    }
                                    LayerContainer.SetBounds(elementReference, feature.Geometry.Bounds);

                                    base.Container.ResetGeometryTransform(elementReference);
                                    elementReference.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                    base.Container.InvalidateArrange();
                                    feature.ClearValue(IsGeometryDirtyProperty);
                                }
                                //TODO:缺陷---移除之后就不显示了。
                                //if (elementReference.Visibility == Visibility.Collapsed)
                                //{
                                //    elementReference.Visibility = Visibility.Visible;
                                //    base.Container.ResetGeometryTransform(elementReference);
                                //    base.Container.InvalidateArrange();
                                //}
                                if (elementReference.Geometry is GeoPoint)
                                {
                                    elementReference.Visibility = Visibility.Visible;
                                    base.Container.ResetGeometryTransform(elementReference);
                                    base.Container.InvalidateArrange();
                                }
                                else//线、面
                                {
                                    if (elementReference.Visibility == Visibility.Collapsed)
                                    {
                                        elementReference.Visibility = Visibility.Visible;
                                        base.Container.ResetGeometryTransform(elementReference);
                                        base.Container.InvalidateArrange();
                                    }
                                }
                            }
                            else
                            {
                                this.drawFeature(feature, renderer);//第一次绘制
                                feature.ClearValue(IsGeometryDirtyProperty);

                                if (isClusterer)
                                {
                                    FeatureElement elment = feature.GetElementReference();
                                    if (elment != null)
                                    {
                                        elment.TemplateApplied += new EventHandler(elment_TemplateApplied);
                                    }
                                }
                            }
                            continue;
                        }
                        if (elementReference != null)
                        {
                            elementReference.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void elment_TemplateApplied(object sender, EventArgs e)
        {
            FeatureElement element = sender as FeatureElement;
            element.TemplateApplied -= new EventHandler(this.elment_TemplateApplied);
            if (element.Feature != null)
            {
                //暂时不加入聚合功能。
                //this.Cluster_TemplateApplied(element, element.Feature);
            }
        }

        private void drawFeature(Feature f, IRenderer renderer)
        {
            SuperMap.WinRT.Core.Geometry geometry = f.Geometry;
            FeatureElement element = new FeatureElement(f, renderer);

            if (geometry is GeoPoint && element.GeoStyle is MarkerStyle)
            {
                MarkerStyle style = (MarkerStyle)(element.GeoStyle);
                element.RenderTransform = new TranslateTransform { X = -style.OffsetX, Y = -style.OffsetY };
            }
            else if (geometry is GeoLine && element.GeoStyle is LineStyle)
            {
                element.SetPath();
            }
            else if (geometry is GeoRegion && element.GeoStyle is FillStyle)
            {
                element.SetPath();
            }
            else if (geometry is GeoCircle && element.GeoStyle is FillStyle)
            {
                element.SetPath();
            }
            else
            {
                throw new ArgumentException(ExceptionStrings.InvalidSupportGeometry);
            }

            f.SetElementReference(element);
            if (element != null)
            {
                element.IsHitTestVisible = true;
                element.InvalidatePath(base.Container.Resolution, base.Container.OriginX, base.Container.OriginY);
                LayerContainer.SetBounds(this, geometry.Bounds);
                base.Container.Children.Add(element);
            }
        }

        #region 事件触发
        internal void element_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            (sender as FeatureElement).ReleasePointerCaptures();

            Feature f = (sender as FeatureElement).Feature;
            f.RaisePointerReleased(e);
            if (this.PointerReleased != null)
            {
                this.PointerReleased(this, new FeaturePointerRoutedEventArgs(f, e));
            }
        }
        internal void element_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as FeatureElement).CapturePointer(e.Pointer);

            Feature f = (sender as FeatureElement).Feature;
            f.RaisePointerPressed(e);
            if (this.PointerPressed != null)
            {
                this.PointerPressed(this, new FeaturePointerRoutedEventArgs(f, e));
            }
        }
        internal void element_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Feature f = (sender as FeatureElement).Feature;
            f.RaisePointerMoved(e);
            if (this.PointerMoved != null)
            {
                this.PointerMoved(this, new FeaturePointerRoutedEventArgs(f, e));
            }
        }
        internal void element_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Feature f = null;
            if (sender is Feature)
            {
                f = sender as Feature;
            }
            else
            {
                f = (sender as FeatureElement).Feature;
            }
            f.RaisePointerExited(e);
            if (this.PointerExited != null)
            {
                this.PointerExited(this, new FeaturePointerRoutedEventArgs(f, e));
            }
            if (this.Map != null && this.Map.Popup.IsOpen)
            {
                FrameworkElement tip = f.ToolTip ?? this.ToolTip;
                if (tip != null)
                {
                    TimeSpan t = GetToolTipHideDelay(tip);
                    if (t.Ticks > 0L)
                    {
                        this.StartHideTimer(t);
                    }
                    else
                    {
                        this.CloseToolTip(tip);
                    }
                }
                else
                {
                    this.CloseToolTip(tip);
                }
            }
        }
        internal void element_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Feature f = (sender as FeatureElement).Feature;
            f.RaisePointerEntered(e);
            if (this.PointerEntered != null)
            {
                this.PointerEntered(this, new FeaturePointerRoutedEventArgs(f, e));
            }
            currentFeature = f;
            if (!f.DisableToolTip && base.Map != null)
            {
                if (currentTooTipFeature == null || currentTooTipFeature == f)
                {
                    this.StopHideTimer();
                }
                else
                {
                    FrameworkElement toolTip = f.ToolTip ?? this.ToolTip;
                    this.CloseToolTip(toolTip);
                }
            }
        }
        //ToolTip在MouseOver时出现 用了Map的Popup
        internal void element_PointerHover(object sender, Feature f, PointerRoutedEventArgs e)
        {
            if (!f.DisableToolTip && base.Map != null)
            {
                this.StopHideTimer();
                FrameworkElement element = f.ToolTip ?? this.ToolTip;//如果要素有自己的tip就显示自己的，否则显示整个图层统一的
                if ((element != null) && ((currentTooTipFeature != f || !base.Map.Popup.IsOpen)))
                {
                    base.Map.Popup.Child = element;
                    base.Map.Popup.DataContext = f.Attributes;
                    PointerPoint position = e.GetCurrentPoint(base.Map.Popup.Parent as UIElement);
                    double h = GetToolTipHorizontalOffset(element);
                    double v = GetToolTipVerticalOffset(element);
                    base.Map.Popup.HorizontalOffset = position.Position.X+h;
                    base.Map.Popup.VerticalOffset = position.Position.Y+v;
                    this.ShowToolTip(f, element);
                }
            }
        }

        #endregion

        /// <overloads>${mapping_FeaturesLayer_method_fromFeatures_overloads}</overloads>
        /// <summary>${mapping_FeaturesLayer_method_fromFeaturesIRenderer_D}</summary>
        /// <returns>${mapping_FeaturesLayer_method_fromFeatures_return}</returns>
        /// <param name="features">${mapping_FeaturesLayer_method_fromFeatures_param_features}</param>
        /// <param name="renderer">${mapping_FeaturesLayer_method_fromFeaturesIRenderer_param_renderer}</param>
        public static FeaturesLayer FromFeatures(IEnumerable<Feature> features, IRenderer renderer)
        {
            return new FeaturesLayer
            {
                Renderer = renderer,
                Features = new FeatureCollection(features)
            };
        }
        /// <summary>${mapping_FeaturesLayer_method_fromFeatures_D}</summary>
        /// <returns>${mapping_FeaturesLayer_method_fromFeatures_return}</returns>
        /// <param name="features">${mapping_FeaturesLayer_method_fromFeatures_param_features}</param>
        public static FeaturesLayer FromFeatures(IEnumerable<Feature> features)
        {
            return new FeaturesLayer { Features = new FeatureCollection(features) };
        }
        /// <summary>${mapping_FeaturesLayer_method_fromFeaturesStyle_D}</summary>
        /// <param name="features">${mapping_FeaturesLayer_method_fromFeatures_param_features}</param>
        /// <param name="style">${mapping_FeaturesLayer_method_fromFeaturesStyle_param_style}</param>
        public static FeaturesLayer FromFeatures(IEnumerable<Feature> features, SuperMap.WinRT.Core.Style style)
        {
            FeaturesLayer layer = new FeaturesLayer { Features = new FeatureCollection(features) };
            if (features != null)
            {
                foreach (Feature feature in layer.Features)
                {
                    feature.Style = style;
                }
            }
            return layer;
        }
        /// <summary>${mapping_FeaturesLayer_method_addFeatureSet_D}</summary>
        /// <param name="featureSet">${mapping_FeaturesLayer_method_addFeatureSet_param_featureSet}</param>
        public void AddFeatureSet(FeatureCollection featureSet)
        {
            if (featureSet != null)
            {
                for (var i = featureSet.Count-1; i >=0; i--)
                {
                    Features.Add(featureSet[i]);
                }
            }
        }
        /// <summary>${mapping_FeaturesLayer_method_addFeatureSet_D}</summary>
        /// <param name="featureSet">${mapping_FeaturesLayer_method_addFeatureSet_param_featureSet}</param>
        /// /// <param name="style">${mapping_FeaturesLayer_method_addFeatureSet_param_style}</param>
        public void AddFeatureSet(FeatureCollection featureSet, SuperMap.WinRT.Core.Style style)
        {
            if (featureSet != null)
            {
                for (var i = featureSet.Count - 1; i >= 0; i--)
                {
                    var item = featureSet[i];
                    item.Style = style;
                    Features.Add(item);
                }
            }
        }
        /// <summary>${mapping_FeaturesLayer_method_addFeature_D}</summary>
        /// <param name="feature">${mapping_FeaturesLayer_method_addFeature_param_feature}</param>
        public void AddFeature(Feature feature)
        {
            Features.Add(feature);
        }

        /// <summary>${mapping_FeaturesLayer_method_findFeaturesInHostCoordinatesPoint_D}</summary>
        /// <overloads>${mapping_FeaturesLayer_method_findFeaturesInHostCoordinates_overloads}</overloads>
        /// <returns>${mapping_FeaturesLayer_method_findFeaturesInHostCoordinatesPoint_return}</returns>
        /// <param name="intersectingPoint">
        /// ${mapping_FeaturesLayer_method_findFeaturesInHostCoordinatesPoint_param_intersetingPoint}
        /// </param>
        public IEnumerable<Feature> FindFeaturesInHostCoordinates(Point intersectingPoint)
        {
            if (base.Container == null)
            {
                return new Feature[0];
            }
            IEnumerable<UIElement> elements = VisualTreeHelper.FindElementsInHostCoordinates(intersectingPoint, base.Container);
            return GetFeaturesFromUIElements(elements);
        }
        /// <summary>${mapping_FeaturesLayer_method_findFeaturesInHostCoordinatesRect_D}</summary>
        /// <param name="intersectingRect">
        /// ${mapping_FeaturesLayer_method_findFeaturesInHostCoordinatesRect_param_intersectingRect}
        /// </param>
        public IEnumerable<Feature> FindFeaturesInHostCoordinates(Rect intersectingRect)
        {
            if (base.Container == null)
            {
                return new Feature[0];
            }
            IEnumerable<UIElement> elements = VisualTreeHelper.FindElementsInHostCoordinates(intersectingRect, base.Container);
            return GetFeaturesFromUIElements(elements);
        }

        private IEnumerable<Feature> GetFeaturesFromUIElements(IEnumerable<UIElement> elements)
        {
            foreach (UIElement item in elements)
            {
                if (item is FeatureElement)
                {
                    FeatureElement fe = item as FeatureElement;
                    if (fe.Feature != null)
                    {
                        //暂时不加入聚合功能。
                        //FeatureCollection fc = fe.Feature.GetValue(Clusterer.ClusterProperty) as FeatureCollection;
                        //if (fc != null)
                        //{
                        //    IEnumerator<Feature> temp = fc.GetEnumerator();
                        //    while (temp.MoveNext())
                        //    {
                        //        yield return temp.Current;
                        //    }
                        //}
                        //else
                        //{
                            yield return fe.Feature;
                        //}
                    }
                }
            }
        }

        private void f_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Feature item = sender as Feature;
            if (item != null && this.Features != null && this.Features.Contains(item))
            {
                if ((this.renderer == null) && (e.PropertyName == "Style"))
                {
                    item.SetElementReference(null);
                    this.Invalidate(false);
                }//根据属性name来看做什么动作
                if (e.PropertyName == "Geometry")
                {
                    this.clusterResolution = double.NaN;
                    item.SetValue(IsGeometryDirtyProperty, true);//draw的时候判断
                    this.fullBounds = Rectangle2D.Empty;
                    this.Invalidate(false);
                }//geometry变，引发变化
                if (e.PropertyName == "Selected")
                {
                    this.raiseSelectedFeaturesChange(item, item.Selected);
                }
            }
        }

        private ObservableCollection<Feature> _selectedFeatures = new ObservableCollection<Feature>();
        /// <summary>${mapping_FeaturesLayer_attribute_selectedFeatures_D}</summary>
        public IEnumerable<Feature> SelectedFeatures
        {
            get
            {
                return this._selectedFeatures;
            }
        }

        private void raiseSelectedFeaturesChange(Feature f, bool isSelected)
        {
            if (f != null)
            {
                if (isSelected)
                {
                    this._selectedFeatures.Add(f);
                }
                else
                {
                    this._selectedFeatures.Remove(f);
                }
            }
            base.OnPropertyChanged("SelectedFeatures");
        }

        private bool isInvalidated;
        private bool forceRefreshOnInvalidate;
        private void Invalidate(bool fullRefresh)
        {
            if (!this.isInvalidated)
            {
                CompositionTarget.Rendering += new EventHandler<object>(this.CompositionTarget_Rendering);
                this.isInvalidated = true;
            }
            this.forceRefreshOnInvalidate = fullRefresh || this.forceRefreshOnInvalidate;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            CompositionTarget.Rendering -= new EventHandler<object>(this.CompositionTarget_Rendering);
            DispatchedHandler handler = new DispatchedHandler(delegate
                    {
                        if (this.forceRefreshOnInvalidate)
                        {
                            if (this.oldClusterCache != null)
                            {
                                foreach (Feature f in this.oldClusterCache)
                                {
                                    f.SetElementReference(null);
                                }
                                this.oldClusterCache = null;
                            }
                            else
                            {
                                if (base.Container != null)
                                {
                                    base.Container.Children.Clear();
                                }
                                foreach (Feature f2 in this.Features)
                                {
                                    f2.SetElementReference(null);
                                }
                            }
                        }
                        base.OnLayerChanged();
                        this.isInvalidated = false;
                        this.forceRefreshOnInvalidate = false;

                    });
            base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,handler);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>${mapping_FeaturesLayer_method_GetEnumerator_D}</summary>
        /// <returns>${mapping_FeaturesLayer_method_GetEnumerator_return}</returns>
        /// <remarks>${mapping_FeaturesLayer_method_GetEnumerator_Exception}</remarks>
        public IEnumerator<Feature> GetEnumerator()
        {
            foreach (var item in this)//this从IEnumerable<Feature>继承
            {
                if (item is Feature)
                {
                    yield return (Feature)item;
                }
            }
        }

        private void features_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!Rectangle2D.IsNullOrEmpty(this.fullBounds))
                {
                    UpdateFullBounds(e.NewItems);
                }
            }
            else
            {
                this.fullBounds = Rectangle2D.Empty;
            }
            if (e.OldItems != null)
            {
                foreach (object obj2 in e.OldItems)
                {
                    Feature feature2 = obj2 as Feature;
                    if (feature2 != null)
                    {
                        this.ClearFeature(feature2);
                        if (feature2.Selected)
                        {
                            this.raiseSelectedFeaturesChange(feature2, false);
                        }
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (object obj3 in e.NewItems)
                {
                    this.addfeature(obj3 as Feature);
                }
            }
            this.clusterResolution = double.NaN;
            this.Invalidate(false);
        }
        private void addfeature(Feature f)
        {
            if (f != null)
            {
                object obj2 = f.GetValue(FeaturesLayerProperty);
                if ((obj2 != null) && (obj2 != this))
                {
                    //TODO:资源
                    throw new ArgumentOutOfRangeException(ExceptionStrings.CanntAddToMFLayer);
                }
                f.PropertyChanged += new PropertyChangedEventHandler(this.f_PropertyChanged);
                f.AttributeValueChanged += new EventHandler<DictionaryChangedEventArgs>(f_AttributeValueChanged);
                f.ElementReferenceChanging += new EventHandler(this.f_ElementReferenceChanging);
                if (f.Selected)
                {
                    this.raiseSelectedFeaturesChange(f, true);
                }
                f.SetValue(FeaturesLayerProperty, this);

            }
        }

        private void Features_CollectionClearing(object sender, EventArgs e)
        {
            if (this.Features != null)
            {
                foreach (Feature f in this.Features)
                {
                    this.ClearFeature(f);
                }
            }
        }

        /// <summary>${mapping_FeaturesLayer_method_refresh_D}</summary>
        public override void Refresh()
        {
            this.clusterResolution = double.NaN;
            this.Invalidate(true);
        }

        private void renderer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Refresh();
        }

        //多个几何要素的 即复合对象 的思路
        //private void setMultiPointPosition(Feature feature, FeatureElement element, MultiGeoPoint multiPoint, double resolution, GeoPoint origin)
        //{
        //    FeatureGroupElement element2 = element as FeatureGroupElement;
        //    if ((element2 != null) && (element2.ElementStyle is MarkerStyle))
        //    {
        //        Rectangle2D extent = multiPoint.Bounds;
        //        this.MapToScreen(new GeoPoint(extent.Right, extent.Bottom), resolution, origin);
        //        foreach (GeoPoint point in multiPoint.Points)
        //        {
        //            this.MapToScreen(point, resolution, origin);
        //        }
        //    }
        //}

        private void UpdateFullBounds(IEnumerable items)
        {
            foreach (object obj2 in items)
            {
                Feature feature = (Feature)obj2;

                if (feature != null && feature.Geometry != null)
                {
                    if (Rectangle2D.IsNullOrEmpty(fullBounds))
                    {
                        fullBounds = feature.Geometry.Bounds;
                    }
                    else
                    {
                        fullBounds=fullBounds.Union(feature.Geometry.Bounds);
                    }
                }
            }
        }

        internal override bool ContinuousDraw
        {
            get
            {
                return true;
            }
        }

        /// <summary>${mapping_Layer_attribute_bounds_D}</summary>
        public override Rectangle2D Bounds
        {
            get
            {
                if ((Rectangle2D.IsNullOrEmpty(this.fullBounds)) && (this.Features.Count > 0))
                {
                    UpdateFullBounds(this.Features);
                }
                return this.fullBounds;
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>${mapping_FeaturesLayer_attribute_isHitTestVisible_D}</summary>
        public bool IsHitTestVisible
        {
            get
            {
                return base.Container.IsHitTestVisible;
            }
            set
            {
                base.Container.IsHitTestVisible = value;
            }
        }
        /// <summary>${mapping_FeaturesLayer_attribute_toolTip_D}</summary>
        public FrameworkElement ToolTip { get; set; }

        /// <summary>${mapping_FeaturesLayer_attribute_renderer_D}</summary>
        /// <remarks>${mapping_FeaturesLayer_attribute_renderer_remarks}</remarks>
        public IRenderer Renderer
        {
            get
            {
                return this.renderer;
            }
            set
            {
                if (renderer != value)
                {
                    if ((this.renderer != null) && (this.renderer is INotifyPropertyChanged))
                    {
                        (this.renderer as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(this.renderer_PropertyChanged);
                    }
                    if (this.Features != null)
                    {
                        foreach (Feature item in Features)
                        {
                            if (value == null)
                            {
                                item.dataContext.Style = item.Style;
                            }
                            item.SetElementReference(null);
                        }
                    }
                    this.renderer = value;
                    if ((this.renderer != null) && (this.renderer is INotifyPropertyChanged))
                    {
                        (this.renderer as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(this.renderer_PropertyChanged);
                    }
                    this.Invalidate(false);
                }
            }
        }
    }
}

