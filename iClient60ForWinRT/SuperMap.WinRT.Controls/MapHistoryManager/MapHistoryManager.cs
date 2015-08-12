using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Mapping;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_MapHistoryManager_Title}</para>
    /// 	<para>${controls_MapHistoryManager_Description}</para>
    /// </summary>
    [TemplatePart(Name = "PreViewButton", Type = typeof(Button))]
    [TemplatePart(Name = "NextViewButton", Type = typeof(Button))]
    public class MapHistoryManager : Control
    {
        Button preViewButton;
        Button nextViewButton;
        /// <summary>${controls_MapHistoryManager_constructor_None_D}</summary>
        public MapHistoryManager()
        {
            this.DefaultStyleKey = typeof(MapHistoryManager);
        }
        /// <summary>${controls_MapHistoryManager_method_OnApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            preViewButton = this.GetTemplateChild("PreViewButton") as Button;
            nextViewButton = this.GetTemplateChild("NextViewButton") as Button;

            if (preViewButton != null)
            {
                preViewButton.Click += new RoutedEventHandler(ViewButton_Click);
            }
            if (nextViewButton != null)
            {
                nextViewButton.Click += new RoutedEventHandler(ViewButton_Click);
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == preViewButton)
            {
                if (_currentViewBoundsIndex != 0)
                {
                    _currentViewBoundsIndex--;
                    if (_currentViewBoundsIndex == 0)
                    {
                        preViewButton.IsEnabled = false;
                    }
                    _newViewBounds = false;
                    Map.IsHitTestVisible = false;
                    Map.ZoomTo(_viewBoundsHistory[_currentViewBoundsIndex]);
                    if (!nextViewButton.IsEnabled)
                    {
                        nextViewButton.IsEnabled = true;
                    }
                }
            }
            else if (_currentViewBoundsIndex < (_viewBoundsHistory.Count - 1))
            {
                _currentViewBoundsIndex++;
                if (_currentViewBoundsIndex == (_viewBoundsHistory.Count - 1))
                {
                    nextViewButton.IsEnabled = false;
                }
                _newViewBounds = false;
                Map.IsHitTestVisible = false;
                Map.ZoomTo(_viewBoundsHistory[_currentViewBoundsIndex]);
                if (!preViewButton.IsEnabled)
                {
                    preViewButton.IsEnabled = true;
                }
            }
        }

        private static int _currentViewBoundsIndex = 0;
        private static List<Rectangle2D> _viewBoundsHistory = new List<Rectangle2D>();
        private static bool _newViewBounds = true;
        /// <summary>${controls_MapHistoryManager_event_MapProperty_D}</summary>
        public static DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(Mapping.Map), typeof(MapHistoryManager), new PropertyMetadata(null, new PropertyChangedCallback(MapHistoryManager.MapProperty_Changed)));
        /// <summary>${controls_MapHistoryManager_method_MapProperty_Changed_D}</summary>
        private static void MapProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapHistoryManager historyManager = d as MapHistoryManager;
            Map oldMap = e.OldValue as Map;
            Map newMap = e.NewValue as Map;

            if (oldMap != null)
            {
                oldMap.ViewBoundsChanged -= new EventHandler<ViewBoundsEventArgs>(historyManager.Map_ViewBoundsChanged);
            }
            if (newMap != null)
            {
                newMap.ViewBoundsChanged += new EventHandler<ViewBoundsEventArgs>(historyManager.Map_ViewBoundsChanged);
            }
        }

        private void Map_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            if (Rectangle2D.IsNullOrEmpty(e.OldViewBounds) && preViewButton != null && nextViewButton != null)
            {
                MapHistoryManager._viewBoundsHistory.Add(e.NewViewBounds);
            }
            else if (_newViewBounds && preViewButton != null && nextViewButton != null)
            {
                _currentViewBoundsIndex++;
                if (_viewBoundsHistory.Count - _currentViewBoundsIndex > 0)
                {
                    _viewBoundsHistory.RemoveRange(_currentViewBoundsIndex, _viewBoundsHistory.Count - _currentViewBoundsIndex);
                }
                if (nextViewButton.IsEnabled)
                {
                    nextViewButton.IsEnabled = false;
                }

                _viewBoundsHistory.Add(e.NewViewBounds);

                if (!preViewButton.IsEnabled)
                {
                    preViewButton.IsEnabled = true;
                }
            }
            else
            {
                Map.IsHitTestVisible = true;
                _newViewBounds = true;
            }
        }
        /// <summary>${controls_MapHistoryManager_attribute__Map_D}</summary>
        public Map Map
        {
            get
            {
                return (Map)GetValue(MapProperty);
            }
            set
            {
                SetValue(MapProperty, value);
            }
        }
        /// <summary>${controls_MapHistoryManager_method_MapProperty_PreViewImageProperty_D}</summary>
        public static DependencyProperty PreViewImageProperty =
            DependencyProperty.Register("PreViewImage", typeof(BitmapImage), typeof(MapHistoryManager), new PropertyMetadata(new BitmapImage(
                new System.Uri("ms-appx:///SuperMap.WinRT.Controls/Images/preview.png", UriKind.RelativeOrAbsolute))
                ));
        /// <summary>${controls_MapHistoryManager_method_MapProperty_PreViewImage_D}</summary>
        public BitmapImage PreViewImage
        {
            get
            {
                return (BitmapImage)GetValue(PreViewImageProperty);
            }
            set
            {
                SetValue(PreViewImageProperty, value);
            }
        }
        /// <summary>${controls_MapHistoryManager_method_MapProperty_NextViewImageProperty_D}</summary>
        public static DependencyProperty NextViewImageProperty =
            DependencyProperty.Register("NextViewImage", typeof(BitmapImage), typeof(MapHistoryManager), new PropertyMetadata(new BitmapImage(
                new System.Uri("ms-appx:///SuperMap.WinRT.Controls/Images/nextview.png", UriKind.RelativeOrAbsolute))
                ));
        /// <summary>${controls_MapHistoryManager_method_MapProperty_NextViewImage_D}</summary>
        public BitmapImage NextViewImage
        {
            get
            {
                return (BitmapImage)GetValue(NextViewImageProperty);
            }
            set
            {
                SetValue(NextViewImageProperty, value);
            }
        }
    }
}
