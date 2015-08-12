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
    public partial class SurfaceAnalyst : Page
    {
        private FeaturesLayer featuresLayer;
        TiledDynamicRESTLayer _layer;
        private const string url1 = "http://support.supermap.com.cn:8090/iserver/services/map-temperature/rest/maps/全国温度变化图";
        private const string url2 = "http://support.supermap.com.cn:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst";

        public SurfaceAnalyst()
        {
            InitializeComponent();
                
            double[] resolutions = new double[6];
            double resolution = 5304.9925191265474;
            for (int i = 0; i < 6; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("全国温度变化图");
            _layer.Failed += SurfaceAnalyst_Failed;
            this.Unloaded += SurfaceAnalyst_Unloaded;
        }

        void SurfaceAnalyst_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= SurfaceAnalyst_Failed;
            this.Unloaded -= SurfaceAnalyst_Unloaded;
            MyMap.Dispose();
        }

        async void SurfaceAnalyst_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            featuresLayer.ClearFeatures();
        }

        private async void datasetsIsoline_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(MyTextBox1.Text))
            {
                await MessageBox.Show("基准值不能为空");
                return;
            }
            double datumValue = 0;
            if(!double.TryParse(MyTextBox1.Text,out datumValue))
            {
                await MessageBox.Show("基准值必须为数值");
                return;
            }
            if(string.IsNullOrEmpty(MyTextBox2.Text))
            {
                await MessageBox.Show("等距值不能为空");
                return;
            }
            double interval = 0;
            if(!double.TryParse(MyTextBox2.Text,out interval))
            {
                await MessageBox.Show("等距值必须为数值");
                return;
            }
                

            DatasetSurfaceAnalystParameters param = new DatasetSurfaceAnalystParameters
            {
                Dataset = "SamplesP@Interpolation",
                SurfaceAnalystMethod = SurfaceAnalystMethod.ISOLINE,
                ZValueFieldName = "AVG_TMP",
                Resolution = 3000,
                ParametersSetting = new SurfaceAnalystParametersSetting
                {
                    ResampleTolerance = 0.7,
                    SmoothMethod = SmoothMethod.BSPLINE,
                    DatumValue = Convert.ToDouble(MyTextBox1.Text),
                    Interval = Convert.ToDouble(MyTextBox2.Text),
                    Smoothness = 3
                },
            };
            try
            {
                SurfaceAnalystService datasetIsolineService = new SurfaceAnalystService(url2);
                var result = await datasetIsolineService.ProcessAsync(param);
                if (result.Recordset.Features == null || result.Recordset.Features.Count == 0)
                {
                    await MessageBox.Show("获取等值线数目为零！");
                }
                else
                {
                    foreach (var item in result.Recordset.Features)
                    {
                        item.Style = new PredefinedLineStyle
                        {
                            Stroke = new SolidColorBrush
                            {
                                Color = Colors.Magenta,
                                Opacity = 0.6
                            },
                            StrokeThickness = 2
                        };
                        featuresLayer.AddFeature(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
