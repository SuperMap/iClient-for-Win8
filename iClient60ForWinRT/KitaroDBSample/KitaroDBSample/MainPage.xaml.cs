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
using REST_SampleCode.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace KitaroDBSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TiledDynamicRESTLayer _tdLayer;
        public MainPage()
        {
            this.InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
           _tdLayer=MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
           _tdLayer.LocalStorage = new OfflineKitaroDB(Windows.Storage.ApplicationData.Current.LocalFolder.Path+"\\World.ism");
           _tdLayer.Failed += TiledDynamicRESTLayerTest_Failed;
           this.Unloaded += TiledDynamicRESTLayerTest_Unloaded;
        }

        void TiledDynamicRESTLayerTest_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _tdLayer.Failed -= TiledDynamicRESTLayerTest_Failed;
            this.Unloaded -= TiledDynamicRESTLayerTest_Unloaded;
            MyMap.Dispose();
        }

        async void TiledDynamicRESTLayerTest_Failed(object sender, SuperMap.WinRT.Mapping.LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
