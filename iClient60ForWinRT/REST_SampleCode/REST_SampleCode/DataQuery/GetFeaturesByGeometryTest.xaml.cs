using SuperMap.WinRT.Actions;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.REST.Data;
using SuperMap.WinRT.Mapping;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using SuperMap.WinRT.Service;
using REST_SampleCode.Controls;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class GetFeaturesByGeometryTest : Page
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/data-world/rest/data/featureResults";
        private FeaturesLayer featureslayer;
        TiledDynamicRESTLayer _layer;

        public GetFeaturesByGeometryTest()
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
            _layer.Failed += GetFeaturesByGeometryTest_Failed;
            this.Unloaded += GetFeaturesByGeometryTest_Unloaded;
        }

        void GetFeaturesByGeometryTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= GetFeaturesByGeometryTest_Unloaded;
            _layer.Failed -= GetFeaturesByGeometryTest_Failed;
            MyMap.Dispose();
        }

        async void GetFeaturesByGeometryTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void Point_Click(object sender, RoutedEventArgs e)
        {
            DrawPoint dp = new DrawPoint(MyMap);
            dp.DrawCompleted += DrawCompleted;
            MyMap.Action = dp;
        }

        async void DrawCompleted(object sender, DrawEventArgs e)
        {
            if (e.Geometry == null)
            {
                return;
            }

            GetFeaturesByGeometryParameters param = new GetFeaturesByGeometryParameters
            {
                DatasetNames = new List<string> { "World:Countries" },
                SpatialQueryMode = SpatialQueryMode.INTERSECT,
                Geometry = e.Geometry
            };
            //与服务端交互
            try
            {
                GetFeaturesByGeometryService service = new GetFeaturesByGeometryService(url);
                var result = await service.ProcessAsync(param);
                featureslayer.ClearFeatures();
                if (result != null)
                {
                    featureslayer.AddFeatureSet(result.Features);
                }
                else
                {
                    await MessageBox.Show("查询结果为空!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Line_Click(object sender, RoutedEventArgs e)
        {
            DrawLine dl = new DrawLine(MyMap);
            dl.DrawCompleted += DrawCompleted;
            MyMap.Action = dl;
        }

        private void Region_Click(object sender, RoutedEventArgs e)
        {
            DrawPolygon dpl = new DrawPolygon(MyMap);
            dpl.DrawCompleted += DrawCompleted;
            MyMap.Action = dpl;
        }

        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Action = new Pan(MyMap);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            featureslayer.ClearFeatures();
        }

    }
}
