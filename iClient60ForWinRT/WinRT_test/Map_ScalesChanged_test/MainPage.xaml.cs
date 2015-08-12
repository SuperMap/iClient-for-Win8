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

namespace Map_ScalesChanged_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            double[] scales = new double[14];
            double scale = 0.00000128;
            for (int i = 0; i < 14; i++)
            {
                scales[i] = scale;
                scale /= 2;
            }
            MyMap.Scales = scales;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
            MyMap.ScalesChanged += MyMap_ScalesChanged;
        }

        void MyMap_ScalesChanged(object sender, SuperMap.WinRT.Mapping.ScalesEventArgs e)
        {
            this.scale_changed.Text = "scalechanged: " + MyMap.Scale.ToString();
        }

        void MyMap_ViewBoundsChanged(object sender, SuperMap.WinRT.Mapping.ViewBoundsEventArgs e)
        {
            this.scale.Text = "scale: " + MyMap.Scale.ToString();
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
            double[] scales = new double[14];
            double scale = 0.00006;
            for (int i = 0; i < 14; i++)
            {
                scales[i] = scale;
                scale /= 2;
            }
            MyMap.Scales = scales;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double[] scales = new double[14];
            double scale = 0.0000089;
            for (int i = 0; i < 14; i++)
            {
                scales[i] = scale;
                scale /= 2;
            }
            MyMap.Scales = scales;
        }
    }
}
