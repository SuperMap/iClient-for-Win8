using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Map_ResolutionsChanged_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            double[] resulutions = new double[] {0.28526148969889065,0.14263074484944532,0.071315372424722662,0.035657686212361331,0.017828843106180665,0.0089144215530903327,0.0044572107765451664,0.0022286053882725832,
 0.0011143026941362916,0.00055715134706814579,0.0002785756735340729,0.00013928783676703645,0.000069643918383518224,0.000034821959191759112 };
            MyMap.Resolutions = resulutions;

            MyMap.ResolutionsChanged += MyMap_ResolutionsChanged;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
        }

        void MyMap_ViewBoundsChanged(object sender, SuperMap.WinRT.Mapping.ViewBoundsEventArgs e)
        {
            this.resolution.Text = "resolution:  " + MyMap.Resolution.ToString();
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
 //           double[] resulutions = new double[] {0.64,0.32,0.16,0.08,0.04,0.02,0.01,0.005,
 //0.0025,0.00125,0.000625,0.0003125,0.00015625,0.000078125 };
            double[] resolutions = new double[14];
            double resolution = 0.64;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            Debug.WriteLine("resolution",this.resolution);


        }

        void MyMap_ResolutionsChanged(object sender, SuperMap.WinRT.Mapping.ResolutionsEventArgs e)
        {
            this.resolution_changed.Text = "resolution_changed: " + MyMap.Resolution.ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double[] resolutions = new double[14];
            double resolution = 0.138;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
        }
    }
}
