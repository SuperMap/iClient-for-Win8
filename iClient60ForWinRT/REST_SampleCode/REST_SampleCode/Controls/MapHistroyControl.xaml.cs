using System;
using SuperMap.WinRT.Mapping;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace REST_SampleCode
{
    public partial class MapHistroyControl : Page
    {
        TiledDynamicRESTLayer _layer;

        public MapHistroyControl( )
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
        }

    }
}
