using System;
using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
namespace REST_SampleCode
{
    public partial class CustomMarkerStyle : Page
    {
        TiledDynamicRESTLayer _layer;

        public CustomMarkerStyle()
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
            _layer.Failed += CustomMarkerStyle_Failed;
            this.Unloaded += CustomMarkerStyle_Unloaded;
            FeaturesLayer featureslayer = MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;

            for (int i = 1; i <= 27; i++)
            {
                double x;
                double y;
                if (i <= 9)
                {
                    x = 80 + 5 * i;
                    y = 40;
                }
                else if (i > 9 && i <= 18)
                {
                    x = 80 + 5 * (i - 9);
                    y = 30;
                }
                else
                {
                    x = 80 + 5 * (i - 18);
                    y = 20;
                }

                Feature f = new Feature();
                f.Geometry = new GeoPoint(x, y);
                f.Style = LayoutRoot.Resources[string.Format("MyMarkerStyle{0}", i)] as MarkerStyle;
                featureslayer.Features.Add(f);
            }
        }

        void CustomMarkerStyle_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= CustomMarkerStyle_Unloaded;
            _layer.Failed -= CustomMarkerStyle_Failed;
            MyMap.Dispose();
        }

        async void CustomMarkerStyle_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

    }
}
