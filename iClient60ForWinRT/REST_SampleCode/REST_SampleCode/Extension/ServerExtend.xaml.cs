using REST_SampleCode.Controls;
using serverExtend;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace REST_SampleCode
{
    public partial class ServerExtend : Page
    {
        public ServerExtend()
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
            MyMap.Layers["DynamicRESTLayer"].Failed += ServerExtend_Failed;
            this.Unloaded += ServerExtend_Unloaded;
            ExecuteExtentService();
        }

        void ServerExtend_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= ServerExtend_Unloaded;
            MyMap.Layers["DynamicRESTLayer"].Failed -= ServerExtend_Failed;
            MyMap.Dispose();
        }

        async void ServerExtend_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void ExecuteExtentService()
        {
            try
            {
                string url = "http://support.supermap.com.cn:8090/iserver/services/Temperature/rest/domainComponents/Temperature/getTemperatureResult";
                ExtendServerParameters param = new ExtendServerParameters() { Arg = "北京" };
                ExtendServerService service = new ExtendServerService(url);
                ExtendServerResult e = await service.ProcessAsync(param);

                Feature feature = new Feature();
                feature.Geometry = new GeoPoint(116, 40);
                feature.Style = MyMarkerStyle;

                Grid grid = new Grid();
                grid.Background = new SolidColorBrush(Color.FromArgb(99, 00, 00, 00));
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(5);
                sp.Children.Add(new TextBlock { Text = e.Extendresult, Foreground = new SolidColorBrush(Colors.Red), FontSize = 20 });
                grid.Children.Add(sp);

                feature.ToolTip = grid;
                FeaturesLayer layer = new FeaturesLayer();
                layer.Features.Add(feature);
                MyMap.Layers.Add(layer);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
