using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_NavControl_Title}</para>
    /// 	<para>${controls_NavControl_Description}</para>
    /// </summary>
    [TemplatePart(Name = "PanUpElement" , Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PanDownElement" , Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PanLeftElement" , Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PanRightElement" , Type = typeof(RepeatButton))]
    [TemplatePart(Name = "ViewEntireElement" , Type = typeof(Button))]

    [TemplatePart(Name = "RotateRingElement" , Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "TransformRotateElement" , Type = typeof(RotateTransform))]

    [TemplatePart(Name = "ZoomSlider" , Type = typeof(Slider))]
    [TemplatePart(Name = "ZoomInElement" , Type = typeof(RepeatButton))]
    [TemplatePart(Name = "ZoomOutElement" , Type = typeof(RepeatButton))]
    public class Navigation : Control
    {
        /// <summary>${controls_NavControl_attribute_mapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map" , typeof(Map) , typeof(Navigation) , new PropertyMetadata(null,new PropertyChangedCallback(OnMapPropertyChanged)));
        /// <summary>${controls_NavControl_attribute_map_D}</summary>
        public Map Map
        {
            get
            {
                return ( base.GetValue(MapProperty) as Map );
            }
            set
            {
                base.SetValue(MapProperty , value);
            }
        }
        private static void OnMapPropertyChanged(DependencyObject d , DependencyPropertyChangedEventArgs e)
        {
            Navigation navgation = d as Navigation;
            Map oldValue = e.OldValue as Map;
            Map newValue = e.NewValue as Map;

            if (oldValue != null)
            {
                oldValue.AngleChanged -= navgation.angleChangedHandler;
                oldValue.ViewBoundsChanged -= new EventHandler<ViewBoundsEventArgs>(navgation.Map_ViewBoundsChanged);
                oldValue.ViewBoundsChanging -= new EventHandler<ViewBoundsEventArgs>(navgation.Map_ViewBoundsChanging);
                if (newValue.Layers != null)
                {
                    newValue.Layers.LayersInitialized -= new EventHandler(navgation.Layers_LayersInitialized);
                }

                newValue.ResolutionsChanged -= new EventHandler<ResolutionsEventArgs>(navgation.newValue_ResolutionsChanged);

            }
            if (navgation.angleChangedHandler == null)
            {
                navgation.angleChangedHandler = new Map.AngleChangedEventHandler(navgation.Map_RotationChanged);
            }
            if (newValue != null)
            {
                newValue.AngleChanged += navgation.angleChangedHandler;

                newValue.ViewBoundsChanging += new EventHandler<ViewBoundsEventArgs>(navgation.Map_ViewBoundsChanging);
                newValue.ViewBoundsChanged += new EventHandler<ViewBoundsEventArgs>(navgation.Map_ViewBoundsChanged);
                if (newValue.Layers != null)
                {
                    newValue.Layers.LayersInitialized += new EventHandler(navgation.Layers_LayersInitialized);
                }

                newValue.ResolutionsChanged += new EventHandler<ResolutionsEventArgs>(navgation.newValue_ResolutionsChanged);
            }
            CompositionTarget.Rendering += navgation.CompositionTarget_Rendering;
        }

        private void newValue_ResolutionsChanged(object sender , ResolutionsEventArgs e)
        {
            this.SetupZoom();
        }
        private void Layers_LayersInitialized(object sender , EventArgs args)
        {
            this.SetupZoom();
        }

        private RepeatButton panDownElement;
        private RepeatButton panLeftElement;
        private RepeatButton panRightElement;
        private RepeatButton panUpElement;
        private Button viewEntireElement;

        private FrameworkElement rotateRingElement;
        private RotateTransform transformRotateElement;

        private Slider zoomSliderElement;
        private RepeatButton zoomInElement;
        private RepeatButton zoomOutElement;

        private Map.AngleChangedEventHandler angleChangedHandler;

        /// <summary>${controls_NavControl_constructor_None_D}</summary>
        public Navigation( )
        {
            base.DefaultStyleKey = typeof(Navigation);
        }

        /// <summary>${controls_NavControl_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rotateRingElement = GetTemplateChild("RotateRingElement") as FrameworkElement;
            transformRotateElement = GetTemplateChild("TransformRotateElement") as RotateTransform;

            panLeftElement = GetTemplateChild("PanLeftElement") as RepeatButton;
            panRightElement = GetTemplateChild("PanRightElement") as RepeatButton;
            panUpElement = GetTemplateChild("PanUpElement") as RepeatButton;
            panDownElement = GetTemplateChild("PanDownElement") as RepeatButton;
            viewEntireElement = GetTemplateChild("ViewEntireElement") as Button;


            zoomSliderElement = GetTemplateChild("ZoomSlider") as Slider;
            zoomInElement = GetTemplateChild("ZoomInElement") as RepeatButton;
            zoomOutElement = GetTemplateChild("ZoomOutElement") as RepeatButton;

            #region 上下左右
            if (this.panLeftElement != null)
            {
                this.panLeftElement.Click += (sender , args) =>
                           {
                               if (( this.Map != null ) && ( sender != null ))
                               {
                                   this.Map.Pan(-this.Map.ViewBounds.Width * Map.PanFactor , 0);
                               }
                           };
            }

            if (this.panRightElement != null)
            {
                this.panRightElement.Click += (sender , args) =>
                {
                    if (( this.Map != null ) && ( sender != null ))
                    {
                        this.Map.Pan(this.Map.ViewBounds.Width * this.Map.PanFactor , 0);
                    }
                };
            }
            if (this.panDownElement != null)
            {
                this.panDownElement.Click += (sender , args) =>
                {
                    if (( this.Map != null ) && ( sender != null ))
                    {
                        this.Map.Pan(0 , -this.Map.ViewBounds.Height * this.Map.PanFactor);
                    }
                };
            }

            if (this.panUpElement != null)
            {
                this.panUpElement.Click += (sender , args) =>
                {
                    if (( this.Map != null ) && ( sender != null ))
                    {
                        this.Map.Pan(0 , this.Map.ViewBounds.Height * this.Map.PanFactor);
                    }
                };
            }
            #endregion

            SetupZoom();

            if (zoomSliderElement != null)
            {
                zoomSliderElement.PointerEntered += zoomSliderElement_PointerEntered;
                zoomSliderElement.PointerExited += zoomSliderElement_PointerExited;
                zoomSliderElement.ValueChanged += ZoomSlider_ValueChanged;
            }
            if (zoomInElement != null)
            {
                zoomInElement.Click += new RoutedEventHandler(this.ZoomInButton_Click);
            }
            if (zoomOutElement != null)
            {
                zoomOutElement.Click += new RoutedEventHandler(this.ZoomOutButton_Click);
            }

            if (rotateRingElement != null && this.transformRotateElement != null)
            {
                rotateRingElement.PointerPressed += _rotateRingPointerPressed;
                rotateRingElement.PointerMoved += _rotateRingPointerMoved;
                rotateRingElement.PointerReleased += _rotateRingPointerReleased;
            }
            if (viewEntireElement != null)
            {
                viewEntireElement.Click += new RoutedEventHandler(this.ZoomViewEntire_Click);
            }

        }
        private void CompositionTarget_Rendering(object sender , object e)
        {
            if (this.Map != null)
            {
                this.transformRotateElement.Angle = this.Map.Angle;
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
        }

        #region 旋转
        private double rotatingangle;
        private bool isRotating;
        private double XP;
        private double YP;
        private const double DCTime = 222.0;
        private long lastMC = 0L;
        //private void rotateRingElement_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    ResetNorth();
        //    e.Handled = true;
        //}//用双击辅助类暂时不行

        private void _rotateRingPointerPressed(object s, PointerRoutedEventArgs args)
        {
            args.Handled = true;
            FrameworkElement r = s as FrameworkElement;
            if (r != null)
            {
                PointerPoint point = args.GetCurrentPoint(r);
                isRotating = true;
                r.CapturePointer(args.Pointer);
                XP = point.Position.X - ( r.ActualWidth / 2 );
                YP = point.Position.Y - ( r.ActualHeight / 2 );
            }
            rotatingangle = transformRotateElement.Angle;
        }
        //private void RotateRing_MouseLeftButtonDown(object sender , MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //    var ui = sender as FrameworkElement;
        //    if (ui != null)
        //    {
        //        isRotating = true;
        //        ui.CaptureMouse();
        //        lastXPosition = e.GetPosition(ui).X - ( ui.ActualWidth / 2 );
        //        lastYPosition = e.GetPosition(ui).Y - ( ui.ActualHeight / 2 );
        //    }
        //    angle = transformRotateElement.Angle;

        //}
        private void _rotateRingPointerMoved(object s, PointerRoutedEventArgs args)
        {
            if (isRotating)
            {
                FrameworkElement fe = s as FrameworkElement;
                if (fe != null)
                {
                    PointerPoint point = args.GetCurrentPoint(fe);
                    //double x = ( args.GetPosition(fe).X - ( fe.ActualWidth / 2 ) );
                    //double y = ( args.GetPosition(fe).Y - ( fe.ActualWidth / 2 ) );
                    //double theta = (Math.Atan2(y , x) - Math.Atan2(YP , XP));
                    //rotatingangle = rotatingangle + ad;
                    rotatingangle = rotatingangle + Math.Round(( Math.Atan2(( point.Position.Y - ( fe.ActualWidth / 2 ) ) , ( point.Position.X - ( fe.ActualWidth / 2 ) )) - Math.Atan2(YP , XP) ) * 180 / Math.PI);

                    if (Map != null)
                    {
                        Map.Angle = rotatingangle;
                    }

                    transformRotateElement.Angle = rotatingangle;
                }
            }
        }
        //private void RotateRing_MouseMove(object sender , MouseEventArgs e)
        //{
        //    if (isRotating)
        //    {
        //        var ui = sender as FrameworkElement;
        //        if (ui != null)
        //        {
        //            double x = ( e.GetPosition(ui).X - ( ui.ActualWidth / 2 ) );
        //            double y = ( e.GetPosition(ui).Y - ( ui.ActualWidth / 2 ) );
        //            double theta = Math.Atan2(y , x) - Math.Atan2(lastYPosition , lastXPosition);
        //            double angleInDegrees = Math.Round(theta * 180 / Math.PI);

        //            angle = angle + angleInDegrees;

        //            if (Map != null)
        //            {
        //                Map.Angle = angle;
        //            }

        //            transformRotateElement.Angle = angle;
        //        }
        //    }

        //}
        private void _rotateRingPointerReleased(object s, PointerRoutedEventArgs args)
        {
            long t = DateTime.Now.Ticks - this.lastMC;
            if (TimeSpan.FromTicks(t).TotalMilliseconds < DCTime)
            {
                ResetNorth();
                lastMC = 0L;
            }
            else
            {
                lastMC = DateTime.Now.Ticks;
            }
            args.Handled = true;
            FrameworkElement fe = s as FrameworkElement;
            if (fe != null)
            {
                isRotating = false;
                fe.ReleasePointerCaptures();
            }
        }
        //private void RotateRing_MouseLeftButtonUp(object sender , MouseButtonEventArgs e)
        //{
        //    long num = DateTime.Now.Ticks - this.lastMC;
        //    if (TimeSpan.FromTicks(num).TotalMilliseconds < DCTime)
        //    {
        //        ResetNorth();
        //        lastMC = 0L;
        //    }
        //    else
        //    {
        //        lastMC = DateTime.Now.Ticks;
        //    }
        //    e.Handled = true;
        //    var ui = sender as FrameworkElement;
        //    if (ui != null)
        //    {
        //        isRotating = false;
        //        ui.ReleaseMouseCapture();
        //    }

        //}

        private void Map_RotationChanged(object sender , DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            if (( this.transformRotateElement != null ) && ( this.transformRotateElement.Angle != newValue ))
            {
                this.transformRotateElement.Angle = newValue;
            }
            this.rotatingangle = newValue;
        }
        private void ResetNorth( )
        {
            Storyboard sb = new Storyboard();
            sb.Duration = TimeSpan.FromMilliseconds(500.0);
            DoubleAnimationUsingKeyFrames frames= new DoubleAnimationUsingKeyFrames();
            SplineDoubleKeyFrame frame2 = new SplineDoubleKeyFrame();
            frame2.KeyTime = sb.Duration.TimeSpan;
            frame2.Value = 0.0;
            KeySpline spline = new KeySpline();
            spline.ControlPoint1 = new Point(0.0 , 0.1);
            spline.ControlPoint2 = new Point(0.1 , 1.0);
            frame2.KeySpline = spline;
            SplineDoubleKeyFrame frame = frame2;
            frames.KeyFrames.Add(frame);
            if (this.Map.Angle > 180)
            {
                frame.Value = 360.0;
            }//正转到北
            if (this.Map.Angle <= 180)
            {
                frame.Value = 0.0;
            }//转回去
            frames.SetValue(Storyboard.TargetPropertyProperty , "Angle");
            sb.Children.Add(frames);
            Storyboard.SetTarget(frames , this.Map);
            sb.Begin();
        }
        #endregion

        private void ZoomViewEntire_Click(object sender , RoutedEventArgs e)
        {
            if (this.Map != null)
            {
                this.Map.ViewEntire();
            }
        }

        private void zoomSliderElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            isSliderGetFocused = false;
        }

        private void zoomSliderElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            isSliderGetFocused = true;
        }

        private void SetupZoom( )
        {
            if (Map == null || this.zoomSliderElement == null)
            {
                return;
            }

            if (this.Map.Scales == null && this.Map.Resolutions == null && !hasTiledCachedLayer())
            {
                this.zoomSliderElement.Visibility = Visibility.Collapsed;
            }
            else if (this.Map.Resolutions != null)
            {
                this.mapResolutions = this.Map.Resolutions;
                this.zoomSliderElement.Visibility = Visibility.Visible;
                this.zoomSliderElement.Minimum = 0.0;
                this.zoomSliderElement.Maximum = this.mapResolutions.Length - 1;
                this.zoomSliderElement.SmallChange = 1.0;
                double value = resolutionToValue(Map.Resolution);
                if (value >= 0.0)
                {
                    this.zoomSliderElement.Value = value;
                }
            }
        }
        private double[] mapResolutions;
        private bool hasTiledCachedLayer( )
        {
            foreach (Layer layer in this.Map.Layers)
            {
                if (layer is TiledCachedLayer)
                {
                    return true;
                }
            }
            return false;
        }
        private void ZoomInButton_Click(object sender , RoutedEventArgs e)
        {
            if (Map != null)
            {
                this.Map.ZoomIn();
            }
            if (( this.zoomSliderElement != null ) && ( this.zoomSliderElement.Visibility == Visibility.Visible ))
            {
                zoomSliderElement.Value++;
                isThumbDragging = true;//相当于拖动滑块一下
            }

        }

        private void ZoomOutButton_Click(object sender , RoutedEventArgs e)
        {
            if (Map != null)
            {
                this.Map.ZoomOut();
            }
            if (( this.zoomSliderElement != null ) && ( this.zoomSliderElement.Visibility == Visibility.Visible ))
            {
                zoomSliderElement.Value--;
                isThumbDragging = true;//相当于拖动滑块一下
            }
        }

        private bool isThumbDragging;
        private void ZoomSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int index = Convert.ToInt32(Math.Round(e.NewValue));

            if (this.Map != null && isSliderGetFocused)
            {
                this.Map.ZoomToLevel(index);
                isThumbDragging = true;
                this.zoomSliderElement.Value = index;
            }

        }
        private bool isSliderGetFocused;
        private void Map_ViewBoundsChanging(object sender , ViewBoundsEventArgs e)
        {
            SuppressSlider();
        }

        private void Map_ViewBoundsChanged(object sender , ViewBoundsEventArgs e)
        {
            SuppressSlider();
            this.isThumbDragging = false;
        }
        private void SuppressSlider( )
        {
            if (( this.zoomSliderElement != null ) && !this.isThumbDragging)
            {
                double value = resolutionToValue(Map.Resolution);
                if (( value >= 0.0 ) && ( this.zoomSliderElement.Value != value ))
                {
                    this.zoomSliderElement.Value = value;
                }
            }
        }
        private double resolutionToValue(double resolution)
        {
            if (double.IsNaN(resolution) || mapResolutions == null)
            {
                return -1.0;
            }
            for (int i = 0 ; i < ( this.mapResolutions.Length - 1 ) ; i++)
            {
                double big = this.mapResolutions[i];
                double small = this.mapResolutions[i + 1];
                if (resolution >= big)
                {
                    return i;
                }
                if (( resolution < big ) && ( resolution > small ))
                {
                    return ( i + ( ( big - resolution ) / ( big - small ) ) );
                }//介于之间，算下占 零点几
            }
            return this.mapResolutions.Length - 1;
        }
    }
}