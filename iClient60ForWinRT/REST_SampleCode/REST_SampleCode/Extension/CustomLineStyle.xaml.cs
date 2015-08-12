using System;
using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
namespace REST_SampleCode
{
    public partial class CustomLineStyle : Page
    {
        TiledDynamicRESTLayer _layer;

        public CustomLineStyle()
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
            _layer.Failed += CustomLineStyle_Failed;
            this.Unloaded += CustomLineStyle_Unloaded;
            MyMap.ViewBounds = new Rectangle2D(75.0, 5.0, 138.0, 55.0);
            FeaturesLayer featureslayer = MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            for (int i = 1; i <= 10; i++)
            {
                int j = i * 7 + 70;
                Feature f = new Feature();
                f.Geometry =
                    new GeoLine
                    {
                        Parts = new System.Collections.ObjectModel.ObservableCollection<Point2DCollection>
                        {
                            new Point2DCollection
                            {
                                new Point2D(j,10),
                                new Point2D(j,50)
                            }
                        }
                    };
                f.Style = LayoutRoot.Resources[string.Format("MyLineStyle{0}", i)] as LineStyle;
                featureslayer.Features.Add(f);
            }
        }

        void CustomLineStyle_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= CustomLineStyle_Unloaded;
            _layer.Failed -= CustomLineStyle_Failed;
            MyMap.Dispose();
        }

        async void CustomLineStyle_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

    }
}
