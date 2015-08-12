using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_Magnifier_Title}</para>
    /// 	<para>${controls_Magnifier_Description}</para>
    /// 	<para><img src="magnifier.png"/></para>
    /// </summary>
    [TemplatePart(Name = "MagMap" , Type = typeof(Map))]
    [ContentProperty(Name="Layer")]
    public class Magnifier : Control
    {
        //起始位置
        private Point beginPosition;
        //放大镜的地图
        private Map magnifierMap;
        //当前位置
        private Point currentPosition;
        private bool isDragOn;
        private double lastResolution = double.NaN;

        /// <summary>${controls_Magnifier_constructor_None_D}</summary>
        public Magnifier( )
        {
            DefaultStyleKey = typeof(Magnifier);
        }
        /// <summary>${controls_Magnifier_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            magnifierMap = GetTemplateChild("MagMap") as Map;
            if (magnifierMap == null)
            {
                throw new ArgumentNullException("MagMap");
            }
            //TiledCacheLayer需要
            magnifierMap.Layers.LayersInitialized += (s4 , e4) =>
            {
                ToSetMagnifierLevel();
                ToUpdateMagnifierMapCenterPosition();
            };
            if (Layer != null)
            {
                magnifierMap.Layers.Add(Layer);
            }
            magnifierMap.MinResolution = double.Epsilon;
            magnifierMap.MaxResolution = double.MaxValue;

            //使Map上的Resolutions和magMap的Resolutions保持一致；
            if (Map != null && Map.Resolutions != null)
            {
                magnifierMap.Resolutions = Map.Resolutions;
            }
            PointerPressed += (s , e) =>
            {
                isDragOn = true;
                beginPosition = e.GetCurrentPoint(null).Position;
                CapturePointer(e.Pointer);
            };
            PointerMoved += (s2 , e2) =>
            {
                if (this.isDragOn)
                {
                    currentPosition = e2.GetCurrentPoint(null).Position;
                    Convert.ToDouble(base.GetValue(Canvas.LeftProperty));
                    Convert.ToDouble(base.GetValue(Canvas.TopProperty));
                    MoveTo(currentPosition.X - beginPosition.X , currentPosition.Y - beginPosition.Y);
                    beginPosition = currentPosition;
                    ToUpdateMagnifierMapCenterPosition();
                }
            };
            PointerReleased += (s1 , e1) =>
            {
                if (this.isDragOn)
                {
                    ReleasePointerCaptures();
                    isDragOn = false;
                }
            };

            Opacity = 1.0;
            if (( Visibility == Visibility.Visible ) && ( Map != null ))
            {
                DispatchedHandler handler = new DispatchedHandler(delegate
                {
                    ToSetMagnifierLevel();
                    ToUpdateMagnifierMapCenterPosition();
                });
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
            }
        }

        /// <summary>${controls_Magnifier_method_ArrangeOverride_D}</summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (base.Visibility == Visibility.Visible)
            {
                DispatchedHandler handler = new DispatchedHandler(delegate
                {
                    ToSetMagnifierLevel();
                    ToUpdateMagnifierMapCenterPosition();
                });
                base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
            }
            return base.ArrangeOverride(finalSize);
        }

        private void newValue_ViewBoundsChanged(object sender , ViewBoundsEventArgs e)
        {
            if (Map.Resolution != lastResolution)
            {
                ToSetMagnifierLevel();
            }
            ToUpdateMagnifierMapCenterPosition();
        }
        private void newValue_ViewBoundsChanging(object sender , ViewBoundsEventArgs e)
        {
            ToUpdateMagnifierMapCenterPosition();
        }

        private void MoveTo(double x , double y)
        {
            //定义transform
            Transform transform = base.RenderTransform;
            TransformGroup transformGroup = transform as TransformGroup;
            MatrixTransform matrixTransform = transform as MatrixTransform;
            TranslateTransform translateTransform = transform as TranslateTransform;
            //移动
            if (translateTransform == null)
            {
                if (transformGroup == null)
                {
                    if (matrixTransform == null)
                    {
                        TransformGroup gp = new TransformGroup();
                        translateTransform = new TranslateTransform();
                        if (transform != null)
                        {
                            gp.Children.Add(transform);
                        }
                        gp.Children.Add(translateTransform);
                        base.RenderTransform = gp;
                    }
                    else
                    {
                        Matrix matrix = matrixTransform.Matrix;
                        matrix.OffsetX += x;
                        matrix.OffsetY += y;
                        base.RenderTransform = new MatrixTransform() { Matrix = matrix };
                        return;
                    }
                }
                else
                {
                    if (transformGroup.Children.Count > 0)
                    {
                        translateTransform = transformGroup.Children[transformGroup.Children.Count - 1] as TranslateTransform;
                    }
                    if (translateTransform == null)
                    {
                        translateTransform = new TranslateTransform();
                        transformGroup.Children.Add(translateTransform);
                    }
                }
            }
            //添加移动的数值
            translateTransform.X += x;
            translateTransform.Y += y;
        }

        //设置放大镜的比例尺级别
        private void ToSetMagnifierLevel( )
        {
            if (( Map != null ) && ( !Map.ViewBounds.IsEmpty ))
            {
                double resolution = Map.Resolution;
                //缩放级别设置
                double num2 = resolution / ZoomFactor; 
                if (( magnifierMap != null ))
                {
                    if (this.magnifierMap.ViewBounds.IsEmpty)
                    {
                        Point2D center = Map.ViewBounds.Center;
                        this.magnifierMap.ViewBounds = new Rectangle2D(center.X - ( magnifierMap.ActualWidth * resolution ) , center.Y - ( magnifierMap.ActualWidth * resolution ) , center.X + ( magnifierMap.ActualWidth * resolution ) , center.Y + ( magnifierMap.ActualWidth * resolution ));
                    }
                    magnifierMap.ZoomToResolution(num2);
                }
            }
        }
        //更新放大镜地图的中心位置
        private void ToUpdateMagnifierMapCenterPosition( )
        {
            if (( base.Visibility != Visibility.Collapsed ) && ( ( magnifierMap != null ) && ( Map != null ) ))
            {
                Point2D pt = Map.ScreenToMap(base.TransformToVisual(Map).TransformPoint(new Point(base.RenderSize.Width * 0.5 , base.RenderSize.Height * 0.5)));
                if (!pt.IsEmpty)
                {
                    magnifierMap.PanTo(pt);
                }
            }
        }
        /// <summary>${controls_Magnifier_attribute_Layer_D}</summary>
        public TiledLayer Layer
        {
            get
            {
                return (TiledLayer)base.GetValue(LayerProperty);
            }
            set
            {
                base.SetValue(LayerProperty , value);
            }
        }
        /// <summary>${controls_Magnifier_field_LayerProperty_D}</summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register("Layer" , typeof(TiledLayer) , typeof(Magnifier) , new PropertyMetadata(null,new PropertyChangedCallback(Magnifier.OnLayerPropertyChanged)));
        private static void OnLayerPropertyChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            Magnifier magnifier = d as Magnifier;
            if (magnifier.magnifierMap != null)
            {
                magnifier.magnifierMap.Layers.Clear();
                if (magnifier.Layer != null)
                {
                    magnifier.magnifierMap.Layers.Add(magnifier.Layer);
                }
            }
        }
        /// <summary>${controls_Magnifier_attribute_Map_D}</summary>
        public Map Map
        {
            get
            {
                return (Map)base.GetValue(MapProperty);
            }
            set
            {
                base.SetValue(MapProperty , value);
            }
        }
        /// <summary>${controls_Magnifier_field_MapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map" , typeof(Map) , typeof(Magnifier) , new PropertyMetadata(null,new PropertyChangedCallback(Magnifier.OnMapPropertyChanged)));
        private static void OnMapPropertyChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            Magnifier magnifier = d as Magnifier;
            Map oldValue = e.OldValue as Map;
            if (oldValue != null)
            {
                if (magnifier.magnifierMap != null)
                {
                    magnifier.magnifierMap.Layers.Clear();
                }
                oldValue.ViewBoundsChanged -= new EventHandler<ViewBoundsEventArgs>(magnifier.newValue_ViewBoundsChanged);
                oldValue.ViewBoundsChanging -= new EventHandler<ViewBoundsEventArgs>(magnifier.newValue_ViewBoundsChanging);
            }
            Map newValue = e.NewValue as Map;
            if (newValue != null)
            {
                if (( magnifier.Layer != null ) && ( magnifier.magnifierMap != null ))
                {
                    magnifier.magnifierMap.Layers.Add(magnifier.Layer);
                }
                newValue.ViewBoundsChanged += new EventHandler<ViewBoundsEventArgs>(magnifier.newValue_ViewBoundsChanged);
                newValue.ViewBoundsChanging += new EventHandler<ViewBoundsEventArgs>(magnifier.newValue_ViewBoundsChanging);
            }
        }
        /// <summary>${controls_Magnifier_attribute_ZoomFactor_D}</summary>
        public double ZoomFactor
        {
            get
            {
                return (double)base.GetValue(ZoomFactorProperty);
            }
            set
            {
                value = ( value <= 20 && value >= 1 ) ? value : 2;
                base.SetValue(ZoomFactorProperty , value);
            }

        }
        /// <summary>${controls_Magnifier_field_ZoomFactorProperty_D}</summary>
        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register("ZoomFactor" , typeof(double) , typeof(Magnifier) , new PropertyMetadata(3.0 , new PropertyChangedCallback(Magnifier.OnZoomFactorPropertyChanged)));
        private static void OnZoomFactorPropertyChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            ( d as Magnifier ).ToSetMagnifierLevel();
        }

    }
}
