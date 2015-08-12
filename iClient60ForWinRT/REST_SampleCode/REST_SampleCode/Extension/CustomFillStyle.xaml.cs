using System;
using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
namespace REST_SampleCode
{
    public partial class CustomFillStyle : Page
    {
        TiledDynamicRESTLayer _layer;

        public CustomFillStyle()
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
            _layer.Failed += CustomFillStyle_Failed;
            this.Unloaded += CustomFillStyle_Unloaded;
            FeaturesLayer featureslayer = MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            for (int i = 1; i <= 10; i++)
            {
                int j = i * 8 + 60;
                Feature f = new Feature();
                f.Geometry =
                    new GeoRegion
                    {
                        Parts = new System.Collections.ObjectModel.ObservableCollection<Point2DCollection>
                        {
                            new Point2DCollection
                            {
                                new Point2D(j,20),
                                new Point2D(j,40),
                                new Point2D(j+5,40),
                                new Point2D(j+5,20),
                                 new Point2D(j,20)
                            }
                        }
                    };
                f.Style = LayoutRoot.Resources[string.Format("MyFillStyle{0}", i)] as FillStyle;
                featureslayer.Features.Add(f);
            }
        }

        void CustomFillStyle_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= CustomFillStyle_Unloaded;
            _layer.Failed -= CustomFillStyle_Failed;
            MyMap.Dispose();
        }

        async void CustomFillStyle_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }
    }
}
