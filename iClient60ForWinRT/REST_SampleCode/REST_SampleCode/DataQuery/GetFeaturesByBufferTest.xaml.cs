using SuperMap.WinRT.Actions;
using SuperMap.WinRT.REST.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml.Controls;
using REST_SampleCode.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class GetFeaturesByBufferTest : Page
    {
        private FeaturesLayer featureslayer;
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/data-world/rest/data/featureResults";
        TiledDynamicRESTLayer _layer;

        public GetFeaturesByBufferTest()
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
            featureslayer = MyMap.Layers["FeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["restLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += GetFeaturesByBufferTest_Failed;
            this.Unloaded += GetFeaturesByBufferTest_Unloaded;
        }

        void GetFeaturesByBufferTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= GetFeaturesByBufferTest_Unloaded;
            _layer.Failed -= GetFeaturesByBufferTest_Failed;
            MyMap.Dispose();
        }

        async void GetFeaturesByBufferTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Action = new Pan(MyMap);
        }

        private void Region_Click(object sender, RoutedEventArgs e)
        {
            DrawPolygon dp = new DrawPolygon(MyMap);
            dp.DrawCompleted += new EventHandler<DrawEventArgs>(dp_DrawCompleted);
            MyMap.Action = dp;
        }

        async void dp_DrawCompleted(object sender, DrawEventArgs e)
        {
            if (e.Geometry == null)
            {
                return;
            }
            GetFeaturesByBufferParameters param = new GetFeaturesByBufferParameters
            {
                DatasetNames = new List<string> { "World:Countries" },
                BufferDistance = 10,
                Geometry = e.Geometry,
            };
            //与服务器交互
            try
            {
                GetFeaturesByBufferService ser = new GetFeaturesByBufferService(url);
                var result = await ser.ProcessAsync(param);
                if (result != null)
                {
                    featureslayer.AddFeatureSet(result.Features);
                }
            }
            //与服务器交互失败
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
