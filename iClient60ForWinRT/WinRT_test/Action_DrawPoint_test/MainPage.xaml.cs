using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Theme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Action_DrawPoint_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ElementsLayer _elayer;
        TiledDynamicRESTLayer _layer;
        FeaturesLayer _fLayer;
        public MainPage()
        {
            this.InitializeComponent();
            _elayer = MyMap.Layers["Elayer"] as ElementsLayer;
            _layer = MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
            _fLayer = MyMap.Layers["FLayer"] as FeaturesLayer;
        }

        void MyMap_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawPoint point = new DrawPoint(this.MyMap);
            //point.Color = new SolidColorBrush(Colors.Red);
            //point.Size = 12;
            //point.Opacity = 0.3;
            Theme theme = new Theme
            {
                 Color=new SolidColorBrush(Colors.Yellow),
                  StrokeThickness=9,
                   Fill=new SolidColorBrush(Colors.Azure),
                 Stroke = new SolidColorBrush(Colors.Black)
            };
            MyMap.Theme = theme;
            point.Color = MyMap.Theme.Color;
            point.Opacity = 0.3;
            point.Size = 36;
            MyMap.Action = point;
            point.DrawCompleted += new EventHandler<DrawEventArgs>(Point_DrawCompleted);
           
            
            
        }

        private void Point_DrawCompleted(object sender, DrawEventArgs e)
        {
            Feature feature = new Feature
            {
                Geometry = e.Geometry
            };


            _fLayer.AddFeature(feature);
            
            //_elayer .AddChild(e.Element);
        }
    }
}
