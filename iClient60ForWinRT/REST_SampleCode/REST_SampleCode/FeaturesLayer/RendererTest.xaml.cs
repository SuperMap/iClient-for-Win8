using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Rendering;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace REST_SampleCode
{
    public partial class RendererTest : Page
    {
        private Random random = new Random();
        private string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World Map";
        FeaturesLayer uniqueRenderLayer;
        FeaturesLayer rangeRendererLayer;
        FeaturesLayer uniformRendererLayer;
        DynamicRESTLayer _layer;

        public RendererTest()
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
            MyMap.Loaded += new RoutedEventHandler(MyMapControl_Loaded);

            uniqueRenderLayer = MyMap.Layers["uniqueRenderLayer"] as FeaturesLayer;
            rangeRendererLayer = MyMap.Layers["rangeRendererLayer"] as FeaturesLayer;
            uniformRendererLayer = MyMap.Layers["uniformRendererLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["DynamicRESTLayer"] as DynamicRESTLayer;
            _layer.Failed += RendererTest_Failed;
            this.Unloaded += RendererTest_Unloaded;
        }

        void RendererTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= RendererTest_Unloaded;
            _layer.Failed -= RendererTest_Failed;
            MyMap.Dispose();
        }

        async void RendererTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void MyMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                double x = this.random.Next(-180, 0);
                double y = this.random.Next(-90, 90);
                Feature feature2 = new Feature();
                feature2.Geometry = new GeoPoint(x, y);
                Feature feature = feature2;
                feature.Attributes.Add("Ranking", this.random.NextDouble());
                TextBlock block = new TextBlock();
                block.Text = "Ranking:" + feature.Attributes["Ranking"].ToString();
                block.Foreground = new SolidColorBrush(Colors.Red);
                feature.ToolTip = block;
                rangeRendererLayer.Features.Add(feature);
            }

            DouUiqueRenderSearch();
            DoUniformRendererSearch();
        }

        private async void DouUiqueRenderSearch()
        {
            List<FilterParameter> list = new List<FilterParameter>();

            list.Add(new FilterParameter
            {
                Name = "Countries@World",
                AttributeFilter = "SmID=39 OR SmID=45 OR SmID=54"
            });

            QueryBySQLParameters parameters = new QueryBySQLParameters
            {
                FilterParameters = list
            };
            //与服务端交互
            try
            {
                QueryBySQLService service = new QueryBySQLService(url);
                var result = await service.ProcessAsync(parameters);
                if (result == null)
                {
                    await MessageBox.Show("没有查询到结果!");
                    return;
                }

                int i = 0;
                foreach (Recordset item in result.Recordsets)
                {
                    foreach (Feature feature in item.Features)
                    {
                        feature.ToolTip = new TextBlock { Text = i++.ToString() };
                        uniqueRenderLayer.Features.Add(feature);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void DoUniformRendererSearch()
        {
            List<FilterParameter> list = new List<FilterParameter>();

            list.Add(new FilterParameter
            {
                Name = "Countries@World",
                AttributeFilter = "SmID=43 OR SmID=78 OR SmID=71"
            });

            QueryBySQLParameters parameters = new QueryBySQLParameters
            {
                FilterParameters = list
            };
            //与服务端交互
            try
            {
                QueryBySQLService service1 = new QueryBySQLService(this.url);
                var result = await service1.ProcessAsync(parameters);
                if (result == null)
                {
                    await MessageBox.Show("没有查询到结果!");
                    return;
                }

                foreach (Recordset item in result.Recordsets)
                {
                    foreach (Feature feature in item.Features)
                    {
                        uniformRendererLayer.Features.Add(feature);
                    }
                }

                uniformRendererLayer.Renderer = new UniformRenderer
                {
                    FillStyle = new FillStyle
                    {
                        Fill = new SolidColorBrush(Color.FromArgb(0xff, 0xe5, 0xe5, 0x45)),
                        Stroke = new SolidColorBrush(Color.FromArgb(0xff, 0x7f, 0x80, 0x13))
                    }
                };
            }
            //与服务端交互失败
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
