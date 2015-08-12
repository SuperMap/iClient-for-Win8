using System;
using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System.Collections.ObjectModel;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace REST_SampleCode
{
    public partial class AddByCode : Page
    {
        private FeaturesLayer featuresLayer;
        TiledDynamicRESTLayer _layer;

        public AddByCode()
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
            this.MyMap.Loaded += new RoutedEventHandler(MyMap_Loaded);
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += AddByCode_Failed;
            this.Unloaded += AddByCode_Unloaded;
        }

        void AddByCode_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= AddByCode_Unloaded;
            _layer.Failed -= AddByCode_Failed;
            MyMap.Dispose();
        }

        async void AddByCode_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            #region 使用预定义点符号
            Feature featurePoint = new Feature();
            GeoPoint point = new GeoPoint();
            point.X = 116.2;
            point.Y = 39.6;
            PredefinedMarkerStyle simpleMarkerStyle = new PredefinedMarkerStyle();
            simpleMarkerStyle.Color = new SolidColorBrush(Colors.Red);
            simpleMarkerStyle.Size = 20;
            simpleMarkerStyle.Symbol = SuperMap.WinRT.Core.PredefinedMarkerStyle.MarkerSymbol.Star;
            featurePoint.Style = simpleMarkerStyle;
            featurePoint.Geometry = point;
            featuresLayer.Features.Add(featurePoint);
            #endregion

            #region 使用预定义线符号
            Feature featureLine = new Feature();
            Point2DCollection points = new Point2DCollection();
            points.Add(new Point2D(116.2, 39.6));
            points.Add(new Point2D(90, 50));
            points.Add(new Point2D(50, 25));
            points.Add(new Point2D(-80, 45));
            points.Add(new Point2D(-100, 38));
            ObservableCollection<Point2DCollection> path = new ObservableCollection<Point2DCollection>();
            path.Add(points);
            GeoLine geoLine = new GeoLine();
            geoLine.Parts = path;

            PredefinedLineStyle simpleLineStyle = new PredefinedLineStyle();
            simpleLineStyle.Stroke = new SolidColorBrush(Colors.Black);
            simpleLineStyle.StrokeThickness = 1;
            simpleLineStyle.StrokeDashArray = new DoubleCollection { 3, 1 };

            featureLine.Geometry = geoLine;
            featureLine.Style = simpleLineStyle;
            featuresLayer.Features.Add(featureLine);
            #endregion

            #region 使用预定义面符号
            Feature featureRegion = new Feature();
            Point2DCollection pointsRegion = new Point2DCollection();
            pointsRegion.Add(new Point2D(-8, 61));
            pointsRegion.Add(new Point2D(-6, 55));
            pointsRegion.Add(new Point2D(-8, 50));
            pointsRegion.Add(new Point2D(2, 50));
            pointsRegion.Add(new Point2D(1, 61));
            pointsRegion.Add(new Point2D(-8, 61));
            ObservableCollection<Point2DCollection> pRegion = new ObservableCollection<Point2DCollection>();
            pRegion.Add(pointsRegion);
            GeoRegion geoRegion = new GeoRegion();
            geoRegion.Parts = pRegion;

            PredefinedFillStyle simpleFillStyle = new PredefinedFillStyle();
            simpleFillStyle.StrokeThickness = 1;
            simpleFillStyle.Stroke = new SolidColorBrush(Colors.Black);
            simpleFillStyle.Fill = new SolidColorBrush(Colors.Yellow);

            featureRegion.Geometry = geoRegion;
            featureRegion.Style = simpleFillStyle;
            featuresLayer.Features.Add(featureRegion);
            #endregion

            #region 添加文本
            Feature featureText = new Feature();
            GeoPoint text = new GeoPoint();
            text.X = 5;
            text.Y = 10;

            TextStyle textStyle = new TextStyle();
            textStyle.Text = "Africa";
            textStyle.FontSize = 40;
            textStyle.Foreground = new SolidColorBrush(Colors.Blue);

            featureText.Geometry = text;
            featureText.Style = textStyle;
            featuresLayer.Features.Add(featureText);
            #endregion
        }
    }
}
