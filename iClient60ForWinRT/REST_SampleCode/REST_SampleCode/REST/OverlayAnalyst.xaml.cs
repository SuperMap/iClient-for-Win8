using System;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.SpatialAnalyst;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using REST_SampleCode.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class OverlayAnalyst : Page
    {
        private FeaturesLayer featuresLayer;
        TiledDynamicRESTLayer _layer;
        private const string url1 = "http://support.supermap.com.cn:8090/iserver/services/map-jingjin/rest/maps/京津地区土地利用现状图";
        private const string url2 = "http://support.supermap.com.cn:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst";
        private TiledDynamicRESTLayer overlayLayer = new TiledDynamicRESTLayer()
        {
            Url = url1,
            //设置图层是否透明,true 表示透明。
            Transparent = true
        };
        public OverlayAnalyst()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 0.0055641857128792931;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("京津地区土地利用现状图");
            _layer.Failed += OverlayAnalyst_Failed;
            this.Unloaded += OverlayAnalyst_Unloaded;
        }

        void OverlayAnalyst_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= OverlayAnalyst_Failed;
            this.Unloaded -= OverlayAnalyst_Unloaded;
            MyMap.Dispose();
        }

        async void OverlayAnalyst_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            featuresLayer.ClearFeatures();
        }

        private async void DatasetOverlayAnalyst_Click(object sender, RoutedEventArgs e)
        {
            DatasetOverlayAnalystParameters param = new DatasetOverlayAnalystParameters
            {
                Operation = OverlayOperationType.CLIP,
                OperateDataset = "Lake_R@Jingjin",
                SourceDataset = "Landuse_R@Jingjin",

            };
            //与服务器交互
            try
            {
                DatasetOverlayAnalystService datasetOverlayAnalystService = new DatasetOverlayAnalystService(url2);
                var result = await datasetOverlayAnalystService.ProcessAsync(param);

                foreach (Feature feature in result.Recordset.Features)
                {
                    feature.Style = new PredefinedFillStyle
                    {
                        StrokeThickness = 1,
                        Fill = new SolidColorBrush
                        {
                            Color = Colors.Magenta,
                            Opacity = 0.8
                        }
                    };
                }
                featuresLayer.AddFeatureSet(result.Recordset.Features);
            }
            //交互失败
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
