using SuperMap.WinRT.Core;
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

namespace Map_Scales_test
{
   
    public sealed partial class MainPage : Page
    {
        TiledDynamicRESTLayer layer;
        public MainPage()
        {
            this.InitializeComponent();
            layer=MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
           
            //double[] sacles = new double[] {0.000000025,0.00000005, 0.0000001, 0.0000002, 0.0000004, 0.0000008 };
            MyMap.Scales = new double[] { 6.4e-9, 1.1e-8, 3.0e-8, 2.4e-7 };
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
            MyMap.Scales = new double[] { 5e-5, 1.25e-4, 2e-4, 5e-4 };
           
            
        }

      

        void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            this.scale.Text = "scale: " + MyMap.Scale.ToString();
            this.viewBounds.Text = "viewBounds: " + MyMap.ViewBounds.ToString();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             
            MyMap.PanTo(new Point2D(0,0));
        }
    }
}
