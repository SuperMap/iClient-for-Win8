using REST_SampleCode.Controls;
using SuperMap.WinRT.Mapping;
using System;
using Windows.Devices.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace REST_SampleCode
{
    public partial class TiledDynamicRESTLayerTest : Page
    {
        TiledDynamicRESTLayer _layer;

        public TiledDynamicRESTLayerTest()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution=0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            MyMap.ViewBoundsChanged += new EventHandler<SuperMap.WinRT.Mapping.ViewBoundsEventArgs>(MyMap_ViewBoundsChanged);
            _layer=MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += TiledDynamicRESTLayerTest_Failed;
            this.Unloaded += TiledDynamicRESTLayerTest_Unloaded;
        }

        void TiledDynamicRESTLayerTest_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _layer.Failed -= TiledDynamicRESTLayerTest_Failed;
            this.Unloaded -= TiledDynamicRESTLayerTest_Unloaded;
            MyMap.Dispose();
        }

        async void TiledDynamicRESTLayerTest_Failed(object sender, SuperMap.WinRT.Mapping.LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        void MyMap_ViewBoundsChanged(object sender, SuperMap.WinRT.Mapping.ViewBoundsEventArgs e)
        {
            this.scale.Text = "Scale：" + MyMap.Scale.ToString();
            this.center.Text = "Center：" + MyMap.Center.ToString();
            this.viewbounds.Text = "ViewBounds：" + MyMap.ViewBounds.ToString();
        }
    }
}
