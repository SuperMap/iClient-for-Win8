using SuperMap.WinRT.Core;
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

namespace Map_PanTo_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Point2D p;
        public MainPage()
        {
            this.InitializeComponent();
            MyMap.PanFactor = 0.3;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
        }

        void MyMap_ViewBoundsChanged(object sender, SuperMap.WinRT.Mapping.ViewBoundsEventArgs e)
        {
            this.viewBounds.Text = "viewBounds" + MyMap.ViewBounds.ToString();
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
            p = new Point2D(20,30);
            MyMap.PanTo(p);
        }
    }
}
