using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace REST_SampleCode
{
    public partial class CustomGeoCircle : Page
    {
        TiledDynamicRESTLayer _layer;

        public CustomGeoCircle()
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

            GeoCircle circle = new GeoCircle();
            circle.Center = new GeoPoint(0, 0);
            circle.Radius = 20;

            feature.Geometry = circle;
            featuresLayer.Features.Add(feature);
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += CustomGeoCircle_Failed;
            this.Unloaded += CustomGeoCircle_Unloaded;
        }

        void CustomGeoCircle_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Unloaded -= CustomGeoCircle_Unloaded;
            _layer.Failed -= CustomGeoCircle_Failed;
            MyMap.Dispose();
        }

        async void CustomGeoCircle_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        public class GeoCircle : GeoRegion
        {
            private double radius = double.NaN;
            private GeoPoint center = null;
            private int pointCount = 359;

            public double Radius
            {
                get
                {
                    return radius;
                }
                set
                {
                    radius = value;
                    CreateRing();
                }
            }

            new public GeoPoint Center
            {
                get
                {
                    return center;
                }
                set
                {
                    center = value;
                    CreateRing();
                }
            }

            public int PointCount
            {
                get
                {
                    return pointCount;
                }
                set
                {
                    pointCount = value;
                    CreateRing();
                }
            }

            private void CreateRing()
            {
                this.Parts.Clear();
                if (!double.IsNaN(Radius) && Radius > 0 && Center != null && PointCount > 2)
                {
                    Point2DCollection pnts = new Point2DCollection();
                    for (int i = 0; i < PointCount; i++)
                    {
                        double rad = 2 * Math.PI / PointCount * i;
                        double x = Math.Cos(rad) * radius + Center.X;
                        double y = Math.Sin(rad) * radius + Center.Y;

                        pnts.Add(new Point2D(x, y));
                    }
                    double x0 = radius + Center.X;
                    double y0 = Center.Y;
                    pnts.Add(new Point2D(x0, y0));
                    this.Parts.Add(pnts);
                }
            }
        }

    }
}
