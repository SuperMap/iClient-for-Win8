using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
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

namespace Action_DrawLine_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ElementsLayer _eLayer;
        FeaturesLayer _fLayer;
        TiledDynamicRESTLayer _layer;
        public MainPage()
        {
            this.InitializeComponent();
            _eLayer = MyMap.Layers["ELayer"] as ElementsLayer;
            _fLayer=MyMap.Layers["FLayer"] as FeaturesLayer;
            _layer=MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawLine line = new DrawLine(this.MyMap);
            MyMap.Action = line;
           // line.Fill = new SolidColorBrush(Colors.Yellow);
            line.StrokeThickness = 12;
            line.DrawCompleted += new EventHandler<DrawEventArgs>(Line_DrawLineCompleted);
            line.Opacity = 0.3;
        }

        private void Line_DrawLineCompleted(object sender, DrawEventArgs e)
        {
           // _eLayer.AddChild(e.Element);
            Feature f = new Feature
            {
                Geometry = e.Geometry
            };
            _fLayer.AddFeature(f);
        }
    }
}
