using REST_SampleCode.Controls;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.REST.Data;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace REST_SampleCode
{
    public partial class GetFeaturesBySQLTest : Page
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/data-world/rest/data/featureResults";
        private FeaturesLayer flayer;
        TiledDynamicRESTLayer _layer;

        public GetFeaturesBySQLTest()
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
            MyTextBox.Text = "SMID<10";
            flayer = MyMap.Layers["FeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["restLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += GetFeaturesBySQLTest_Failed;
            this.Unloaded += GetFeaturesBySQLTest_Unloaded;
        }

        void GetFeaturesBySQLTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= GetFeaturesBySQLTest_Unloaded;
            _layer.Failed -= GetFeaturesBySQLTest_Failed;
            MyMap.Dispose();
        }

        async void GetFeaturesBySQLTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void GetFeaturesBySQLTest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MyTextBox.Text) || string.IsNullOrWhiteSpace(MyTextBox.Text))
            {
                await MessageBox.Show("请输入查询条件");
                return;
            }
            GetFeaturesBySQLParameters param = new GetFeaturesBySQLParameters
            {
                DatasetNames = new List<string> { "World:Capitals" },
                FilterParameter = new FilterParameter
                {
                    AttributeFilter = MyTextBox.Text,
                }
            };

            //调用方式1
            try
            {
                GetFeaturesBySQLService ser = new GetFeaturesBySQLService(url);
                var result = await ser.ProcessAsync(param);
                flayer.ClearFeatures();
                if (result.FeatureCount > 0)
                {
                    flayer.AddFeatureSet(result.Features);
                }
                else
                {
                    await MessageBox.Show("查询结果为空");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
