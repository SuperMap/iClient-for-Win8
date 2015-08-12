using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.SpatialAnalyst;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using System;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace REST_SampleCode
{
    public partial class BufferAnalyst : Page
    {
        //京津图层服务地址
        private const string url1 = "http://support.supermap.com.cn:8090/iserver/services/map-jingjin/rest/maps/京津地区土地利用现状图";
        private const string url2 = "http://support.supermap.com.cn:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst";
        private FeaturesLayer resultLayer = new FeaturesLayer();
        TiledDynamicRESTLayer _layer;

        private TiledDynamicRESTLayer bufferLayer = new TiledDynamicRESTLayer()
        {
            Url = url1,
            //设置图层是否透明,true 表示透明。
            Transparent = true
        };
        public BufferAnalyst()
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
            this.MyMap.Layers.Add(resultLayer);
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("京津地区土地利用现状图");
            _layer.Failed += BufferAnalyst_Failed;
            this.Unloaded += BufferAnalyst_Unloaded;
        }

        void BufferAnalyst_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= BufferAnalyst_Failed;
            this.Unloaded -= BufferAnalyst_Unloaded;
            MyMap.Dispose();
        }

        private async void BufferAnalyst_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void DatasetBufferAnalyst_Click(object sender, RoutedEventArgs e)
        {
            resultLayer.ClearFeatures();
            if (string.IsNullOrEmpty(MyTextBox.Text))
            {
                await MessageBox.Show("缓冲半径不能为空");
                return;
            }
            double radio = 0;
            if (!double.TryParse(MyTextBox.Text, out radio))
            {
                await MessageBox.Show("缓冲半径必须为数值");
                return;
            }
            DatasetBufferAnalystParameters param = new DatasetBufferAnalystParameters
            {
                BufferSetting = new BufferSetting
                {
                    EndType = BufferEndType.ROUND,
                    LeftDistance = new BufferDistance
                    {
                        Value = radio
                    },
                    SemicircleLineSegment = 12,

                },
                FilterQueryParameter = new SuperMap.WinRT.REST.FilterParameter
                {
                    AttributeFilter = "SmID=19"
                },
                Dataset = "Landuse_R@Jingjin",

            };
            try
            {
               
                DatasetBufferAnalystService datasetBufferAnalyst = new DatasetBufferAnalystService(url2);
                var result = await datasetBufferAnalyst.ProcessAsync(param);

                if (result.Recordset.Features.Count > 0)
                {
                    foreach (Feature feature in result.Recordset.Features)
                    {
                        feature.Style = new PredefinedFillStyle
                        {
                            StrokeThickness = 1,
                            Fill = new SolidColorBrush
                            {
                                Color = Colors.Red,
                                Opacity = 0.3
                            }
                        };
                    }
                    resultLayer.AddFeatureSet(result.Recordset.Features);
                }
                else
                {
                    await MessageBox.Show("缓冲区分析失败！");                
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            resultLayer.ClearFeatures();
        }
    }
}
