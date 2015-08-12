using System;
using SuperMap.WinRT.Mapping;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
namespace REST_SampleCode
{
    public partial class TiledWMTSLayerTest : Page
    {
        TiledWMTSLayer _layer;

        public TiledWMTSLayerTest()
        {
            InitializeComponent();
            _layer = MyMap.Layers["WMTSLayer"] as TiledWMTSLayer;
            _layer.LocalStorage = new OfflineStorage("WorldWMTS");
            this.Unloaded += TiledWMTSLayerTest_Unloaded;
        }

        void TiledWMTSLayerTest_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= TiledWMTSLayerTest_Unloaded;
            MyMap.Dispose();
        }

    }
}
