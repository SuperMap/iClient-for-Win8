using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using REST_SampleCode.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class SetLayerStatusTest : Page
    {
        private List<LayerStatus> layersStatus;
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World";
        private string tempLayerID = string.Empty;
        private TiledDynamicRESTLayer layer;

        public SetLayerStatusTest()
        {
            InitializeComponent();
            getLayerInfo();
            this.Unloaded += SetLayerStatusTest_Unloaded;
        }

        void SetLayerStatusTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= SetLayerStatusTest_Unloaded;
            layer.Failed -= layer_Failed;
            MyMap.Dispose();
        }

        private async System.Threading.Tasks.Task getLayerInfo()
        {
            layersStatus = new List<LayerStatus>();
            layer = new TiledDynamicRESTLayer
            {
                Url = url,
                EnableServerCaching = false,
            };
            
            layer.Failed += layer_Failed;
            this.MyMap.Layers.Add(layer);

            //获取图层信息
            try
            {
                GetLayersInfoService getInfoServer = new GetLayersInfoService(url);
                var result = await getInfoServer.ProcessAsync();

                if (result.LayersInfo.Count > 0)
                {
                    foreach (var layer1 in result.LayersInfo)
                    {
                        layersStatus.Add(new LayerStatus { IsVisible = layer1.IsVisible, LayerName = layer1.Name });
                    }
                    await MyMap.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            layersList.ItemsSource = layersStatus;
                        });
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void layer_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void isVisible_Click(object sender, RoutedEventArgs e)
        {
            SetLayerStatusParameters parameters = new SetLayerStatusParameters
            {
                HoldTime = 30,
                LayerStatusList = layersStatus,
                ResourceID = tempLayerID
            };
            //与服务端交互
            try
            {
                SetLayerStatusService setLayersStatus = new SetLayerStatusService(url);
                var result = await setLayersStatus.ProcessAsync(parameters);
                if (result.IsSucceed)
                {
                    tempLayerID = result.NewResourceID;
                    layer.LayersID = result.NewResourceID;
                    layer.Refresh();

                }
            }
            //交互失败
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}