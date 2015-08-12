using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MBTilesLayerSample_WP8.Resources;
using MBTilesLayerSample;
using SuperMap.WindowsPhone.Mapping;

namespace MBTilesLayerSample_WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            MBTilesLayer _mbLayer = new MBTilesLayer();
            OfflineMBTiles _mbTiles = new OfflineMBTiles();
            _mbLayer.LocalStorage = _mbTiles;
            _mbTiles.MBTilesPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\China.mbtiles";
            MyMap.Layers.Add(_mbLayer);
            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomIn();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomOut();
        }
    }
}