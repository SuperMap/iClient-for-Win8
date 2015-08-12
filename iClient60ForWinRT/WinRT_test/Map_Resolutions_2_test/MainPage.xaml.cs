using SuperMap.WinRT.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Map_Resolutions_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TiledDynamicRESTLayer layer;
        public MainPage()
        {
            this.InitializeComponent();
            layer=MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
            double[] resolutions = new double[14];
            double resolution = 0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            double[] resolutions1 = new double[14];
            double resolution1 = 0.18526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions1;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
        }

        void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            this.resolution.Text = "Resolution: " + MyMap.Resolution.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
