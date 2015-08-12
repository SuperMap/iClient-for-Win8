using REST_SampleCode.Controls;
using SuperMap.WinRT.REST.Data;
using SuperMap.WinRT.Mapping;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace REST_SampleCode
{
    public partial class GetFeaturesByIDsTest : Page
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/data-world/rest/data/featureResults";
        private FeaturesLayer flayer;
        TiledDynamicRESTLayer _layer;

        public GetFeaturesByIDsTest()
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
            flayer = MyMap.Layers["FeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["restLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += GetFeaturesByIDsTest_Failed;
            this.Unloaded += GetFeaturesByIDsTest_Unloaded;
        }

        void GetFeaturesByIDsTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= GetFeaturesByIDsTest_Unloaded;
            _layer.Failed -= GetFeaturesByIDsTest_Failed;
            MyMap.Dispose();
        }

        async void GetFeaturesByIDsTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void GetFeaturesByIDsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MyTextBox.Text) || string.IsNullOrWhiteSpace(MyTextBox.Text))
            {
                await MessageBox.Show("请输入查询条件");
                return;
            }
            //用逗号和空格分开的都行。
            string[] str = MyTextBox.Text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (str.Length <= 0)
            {
                await MessageBox.Show("请输入查询条件");
                return;
            }
            List<int> ids = new List<int>();

            for (int i = 0; i < str.Length; i++)
            {
                int value = -1;
                if (Int32.TryParse(str[i], out value))
                {
                    ids.Add(value);
                }
                else
                {
                    await MessageBox.Show("ID必须为数字");
                    return;
                }
            }
            GetFeaturesByIDsParameters param = new GetFeaturesByIDsParameters
            {
                DatasetNames = new List<string> { "World:Capitals" },
                IDs = ids
            };
            //与服务器交互
            try
            {
                GetFeaturesByIDsService ser = new GetFeaturesByIDsService(url);
                var result = await ser.ProcessAsync(param);
                flayer.ClearFeatures();
                if (result != null)
                {
                    flayer.AddFeatureSet(result.Features);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
