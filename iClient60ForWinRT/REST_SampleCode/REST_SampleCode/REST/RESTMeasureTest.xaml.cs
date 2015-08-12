using System;
using System.Windows;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using REST_SampleCode.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class RESTMeasureTest : Page
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/世界地图";
        private FeaturesLayer featuresLayer;
        private ElementsLayer elementsLayer;
        TiledDynamicRESTLayer _layer;

        public RESTMeasureTest()
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
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            elementsLayer = this.MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += RESTMeasureTest_Failed;
            this.Unloaded += RESTMeasureTest_Unloaded;
        }

        void RESTMeasureTest_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTMeasureTest_Failed;
            this.Unloaded -= RESTMeasureTest_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTMeasureTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void mybtn_Click(object sender, RoutedEventArgs e)
        {
            this.featuresLayer.ClearFeatures();
            this.elementsLayer.Children.Clear();

            DrawLine line = new DrawLine(MyMap);
            MyMap.Action = line;
            line.DrawCompleted += new EventHandler<DrawEventArgs>(line_DrawCompleted);
        }
        private void line_DrawCompleted(object sender, DrawEventArgs e)
        {
            if (e.Geometry == null)
            {
                return;
            }

            //将线标绘在客户端要素图层
            PredefinedLineStyle lineStyle = new PredefinedLineStyle { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 3 };
            Feature feature = new Feature
            {
                Geometry = e.Geometry,
                Style = lineStyle
            };
            featuresLayer.Features.Add(feature);

            Measure(e.Geometry);
        }

        private void mybtn2_Click(object sender, RoutedEventArgs e)
        {
            this.featuresLayer.ClearFeatures();
            this.elementsLayer.Children.Clear();

            DrawPolygon polygon = new DrawPolygon(MyMap);
            MyMap.Action = polygon;
            polygon.DrawCompleted += new EventHandler<DrawEventArgs>(polygon_DrawCompleted);
        }

        private void polygon_DrawCompleted(object sender, DrawEventArgs e)
        {
            if (e.Geometry == null)
            {
                return;
            }

            //将面标绘在客户端任意图层
            PolygonElement polygon = e.Element as PolygonElement;
            polygon.Opacity = 0.618;
            polygon.StrokeThickness = 1;
            polygon.Fill = new SolidColorBrush(Colors.LightGray);
            this.elementsLayer.Children.Add(polygon);

            Measure(e.Geometry);
        }

        private async void Measure(SuperMap.WinRT.Core.Geometry geo)
        {
            MeasureParameters parameters = new MeasureParameters { Geometry = geo, Unit = Unit.Kilometer };

            try
            {
                MeasureService measureService = new MeasureService(url);
                var result = await measureService.ProcessAsync(parameters);
                if (result.Distance == -1)
                {
                    await MessageBox.Show(result.Area.ToString() + "平方千米");
                }
                else if (result.Area == -1)
                {
                    await MessageBox.Show(result.Distance.ToString() + "千米");
                }
                else
                {
                    await MessageBox.Show("量算没有结果！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pan_Click(object sender, RoutedEventArgs e)
        {

            Pan pan = new Pan(MyMap);
            MyMap.Action = pan;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            elementsLayer.Children.Clear();
            featuresLayer.ClearFeatures();
        }
    }
}
