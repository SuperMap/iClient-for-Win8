using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Controls.Resources;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_InfoWindow_Title}</para>
    /// 	<para>${controls_InfoWindow_Description}</para>
    /// </summary>
    public class InfoWindow : ContentControl
    {
        private Point screenPoint;
        private StackPanel window;
        private double windowWidth;
        private double windowHeight;
        private bool isOnApplyTemplate;
        private Map Map;
        private Button CloseButton;
        private InfoWindow()
        {
        }
        /// <summary>${controls_InfoWindow_constructor_None_D}</summary>
        public InfoWindow(Map map)
            : this()
        {
            if (map == null)
            { throw new ArgumentNullException("map", Resource.InfoWindow_MapParam); }

            this.Map = map;
            base.DefaultStyleKey = typeof(InfoWindow);
            Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.SizeChanged += new SizeChangedEventHandler(InfoWindow_SizeChanged);
            this.Map.LayoutUpdated += new EventHandler<object>(Map_LayoutUpdated);
        }

        private void Map_LayoutUpdated(object sender, object e)
        {
            if (this.Map.ActualWidth == 0.0)
            {
                CompositionTarget.Rendering -= new EventHandler<object>(CompositionTarget_Rendering);
                this.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>${controls_InfoWindow_event_OnMouseLeftButtonDown_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnMouseLeftButtonDown_param_e}</param>
        protected override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            base.OnPointerPressed(e);
        }
        /// <summary>${controls_InfoWindow_event_OnMouseLeftButtonUp_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnMouseLeftButtonUp_param_e}</param>
        protected override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            base.OnPointerReleased(e);
        }
        /// <summary>${controls_InfoWindow_event_OnMouseWheel_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnMouseWheel_param_e}</param>
        protected override void OnPointerWheelChanged(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            base.OnPointerWheelChanged(e);
        }
        /// <summary>${controls_InfoWindow_event_OnKeyDown_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnKeyDown_param_e}</param>
        protected override void OnKeyDown(Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            e.Handled = true;
            base.OnKeyDown(e);
        }
        /// <summary>${controls_InfoWindow_event_OnKeyUp_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnKeyUp_param_e}</param>
        protected override void OnKeyUp(Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            e.Handled = true;
            base.OnKeyUp(e);
        }
        /// <summary>${controls_InfoWindow_event_OnDragEnter_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnDragEnter_param_e}</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            e.Handled = true;
            base.OnDragEnter(e);
        }
        /// <summary>${controls_InfoWindow_event_OnDragLeave_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnDragLeave_param_e}</param>
        protected override void OnDragLeave(DragEventArgs e)
        {
            e.Handled = true;
            base.OnDragLeave(e);
        }
        /// <summary>${controls_InfoWindow_event_OnDragOver_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnDragOver_param_e}</param>
        protected override void OnDragOver(DragEventArgs e)
        {
            e.Handled = true;
            base.OnDragOver(e);
        }
        /// <summary>${controls_InfoWindow_event_OnDrop_D}</summary>
        /// <param name="e">${controls_InfoWindow_event_OnDrop_param_e}</param>
        protected override void OnDrop(DragEventArgs e)
        {
            e.Handled = true;
            base.OnDrop(e);
        }

        //该事件在OnApplyTemplate方法完成后触发。
        private void InfoWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustPisition();
        }
        /// <summary>${controls_InfoWindow_method_OnApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CloseButton = base.GetTemplateChild("CloseButton") as Button;
            this.window = base.GetTemplateChild("window") as StackPanel;

            if (this.CloseButton != null)
                this.CloseButton.Click += new RoutedEventHandler(CloseButton_Click);

            isOnApplyTemplate = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseInfoWindow();
        }
        /// <overloads>${controls_InfoWindow_method_ShowInfoWindow_overload_D}</overloads>
        /// <summary>${controls_InfoWindow_method_ShowInfoWindow_None_D}</summary>
        public void ShowInfoWindow()
        {
            if (!Point2D.IsNullOrEmpty(location))
            {
                ShowInfoWindow(location);
            }
        }
        /// <summary>${controls_InfoWindow_method_ShowInfoWindowPoint2D_D}</summary>
        /// <param name="location">${controls_InfoWindow_method_ShowInfoWindow_param_location}</param>
        public void ShowInfoWindow(Point2D location)
        {
            ShowInfoWindow(location, offset);
        }
        /// <summary>${controls_InfoWindow_method_ShowInfoWindowPo_D}</summary>
        /// <param name="loc">${controls_InfoWindow_method_ShowInfoWindow_param_loc}</param>
        /// <param name="off">${controls_InfoWindow_method_ShowInfoWindow_param_off}</param>
        public void ShowInfoWindow(Point2D loc, Point off)
        {
            this.location = loc;
            this.offset = off;
            this.Visibility = Visibility.Visible;
            //ChangePosition();这句话导致了无法获取对象树中的window对象的问题。
            CompositionTarget.Rendering -= new EventHandler<object>(CompositionTarget_Rendering);
            CompositionTarget.Rendering += new EventHandler<object>(CompositionTarget_Rendering);

            if (isOnApplyTemplate)
            {
                AdjustPisition();
            }
        }
        /// <summary>${controls_InfoWindow_method_CloseInfoWindow_D}</summary>
        public void CloseInfoWindow()
        {
            //Map.InfoWindowContatainer.Children.Clear();
            this.Visibility = Visibility.Collapsed;
            CompositionTarget.Rendering -= new EventHandler<object>(CompositionTarget_Rendering);
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            ChangePosition();
        }

        private void ChangePosition()
        {
            screenPoint = Map.MapToScreen(location);
            this.windowHeight = window.ActualHeight;
            this.windowWidth = window.ActualWidth;

            this.SetValue(Canvas.LeftProperty, screenPoint.X + offset.X);
            this.SetValue(Canvas.TopProperty, screenPoint.Y + offset.Y - windowHeight);
        }

        #region 自适应区域
        //平移和缩放地图时不调用
        private void AdjustPisition()
        {
            screenPoint = Map.MapToScreen(location);
            this.windowHeight = window.ActualHeight;
            this.windowWidth = window.ActualWidth;

            //获取即将显示window的中心点（相对于屏幕的），获取地图的中心点（map）
            Point windowScreenCenter = new Point(screenPoint.X + windowWidth / 2, screenPoint.Y - windowHeight / 2);
            Point windowScreenLeftTop = new Point(screenPoint.X, screenPoint.Y - windowHeight);
            Rect mapRect = new Rect(new Point(0, 0), new Size(this.Map.ActualWidth, this.Map.ActualHeight));
            Rect windowRect = new Rect(windowScreenLeftTop, new Size(this.ActualWidth, this.ActualHeight));

            Point2D windowCenterInMapPoint2D = this.Map.ScreenToMap(windowScreenCenter);
            Point2D showPositionInMap;

            //只判断window比地图小，且window与地图相交和包含的情况
            if (Map.ActualHeight >= this.ActualHeight && Map.ActualWidth >= this.ActualWidth)
            {
                //只要有相交就平移地图显示整个窗口
                showPositionInMap = CheckPosition(mapRect, windowRect, windowScreenCenter);

                Point2D windowCenterInMap = this.Map.ScreenToMap(windowScreenCenter);
                this.Map.Pan(windowCenterInMap.X - showPositionInMap.X, windowCenterInMap.Y - showPositionInMap.Y);
            }
        }

        private Point2D PanToLeftTop(Point leftTop)
        {
            return this.Map.ScreenToMap(new Point());
        }

        //九个区域进行判断,除去完全在外面的情况
        private Point2D CheckPosition(Rect mapRect, Rect windowRect, Point windowCenterInScreen)
        {
            if (windowRect.Left < mapRect.Left && windowRect.Top < mapRect.Top) //左上
            {
                return PanFromLeftTop(windowCenterInScreen);
            }
            else if (windowRect.Left < mapRect.Left && windowRect.Bottom > mapRect.Bottom)//左下
            {
                return PanFromLeftBottom(windowCenterInScreen);
            }
            else if (windowRect.Bottom > mapRect.Bottom && windowRect.Right > mapRect.Right)//右下
            {
                return PanFromRightBottom(windowCenterInScreen);
            }
            else if (windowRect.Right > mapRect.Right && windowRect.Top < mapRect.Top)//右上
            {
                return PanFromRightTop(windowCenterInScreen);
            }
            else if (windowRect.Left < mapRect.Left)    //左中
            {
                return PanFromLeftMiddle(windowCenterInScreen);
            }
            else if (windowRect.Bottom > mapRect.Bottom) //下中
            {
                return PanFromBottomMiddle(windowCenterInScreen);
            }
            else if (windowRect.Right > mapRect.Right)//右中
            {
                return PanFromRightMiddle(windowCenterInScreen);
            }
            else if (windowRect.Top < mapRect.Top) //上中
            {
                return PanFromTopMiddle(windowCenterInScreen);
            }

            //完全包含在地图控件里时不考虑平移了
            return PanFromCenter(windowCenterInScreen);
        }

        private Point2D PanFromTopMiddle(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(centerPoint.X, windowHeight / 2));
        }

        private Point2D PanFromRightTop(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(this.Map.ActualWidth - windowWidth / 2, windowHeight / 2));
        }

        private Point2D PanFromRightMiddle(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(this.Map.ActualWidth - windowWidth / 2, centerPoint.Y));
        }

        private Point2D PanFromRightBottom(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(this.Map.ActualWidth - windowWidth / 2, this.Map.ActualHeight - windowWidth / 2));
        }

        private Point2D PanFromBottomMiddle(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(centerPoint.X, this.Map.ActualHeight - windowHeight / 2));
        }

        private Point2D PanFromLeftBottom(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(windowWidth / 2, this.Map.ActualHeight - windowHeight / 2));
        }

        private Point2D PanFromLeftMiddle(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(windowWidth / 2, centerPoint.Y));
        }

        private Point2D PanFromLeftTop(Point centerPoint)
        {
            return this.Map.ScreenToMap(new Point(windowWidth / 2, windowHeight / 2));
        }

        private Point2D PanFromCenter(Point centerPoint)
        {
            return this.Map.ScreenToMap(centerPoint);
        }
        #endregion

        //property
        private Point2D location = new Point2D();
        /// <summary>${controls_InfoWindow_attribute_Location_D}</summary>
        public Point2D Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        private Point offset = new Point();
        /// <summary>${controls_InfoWindow_attribute_Offset_D}</summary>
        public Point Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
            }
        }
        /// <summary>${controls_InfoWindow_attribute_Title_D}</summary>
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>${controls_InfoWindow_attribute_TitleProperty_D}</summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(object), typeof(InfoWindow), null);
    }
}
