using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace REST_SampleCode
{
    public partial class CustomGeoRectangle : Page
    {
        TiledDynamicRESTLayer _layer;

        public CustomGeoRectangle()
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
            FeaturesLayer featuresLayer = MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            Feature feature = new Feature();

            feature.Geometry = new GeoRectangle(new Point2D(10, -10), new Point2D(30, 30));
            featuresLayer.Features.Add(feature);
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += CustomGeoRectangle_Failed;
            this.Unloaded += CustomGeoRectangle_Unloaded;
        }

        void CustomGeoRectangle_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= CustomGeoRectangle_Unloaded;
            _layer.Failed -= CustomGeoRectangle_Failed;
            MyMap.Dispose();
        }

        async void CustomGeoRectangle_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        public class GeoRectangle : GeoRegion
        {
            public GeoRectangle(Point2D point1, Point2D point2)
            {
                CreateRectangle(point1, point2);
            }

            private void CreateRectangle(Point2D p1, Point2D p2)
            {
                double xmin = Math.Min(p1.X, p2.X);
                double xmax = Math.Max(p1.X, p2.X);
                double ymin = Math.Min(p1.Y, p2.Y);
                double ymax = Math.Max(p1.Y, p2.Y);

                Point2DCollection ps = new Point2DCollection();
                ps.Add(new Point2D(xmin, ymin));
                ps.Add(new Point2D(xmin, ymax));
                ps.Add(new Point2D(xmax, ymax));
                ps.Add(new Point2D(xmax, ymin));
                ps.Add(new Point2D(xmin, ymin));

                this.Parts.Add(ps);
            }
        }

    }
}
