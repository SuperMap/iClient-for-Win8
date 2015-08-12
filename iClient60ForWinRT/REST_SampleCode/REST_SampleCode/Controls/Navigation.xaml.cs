using System;
using Windows.UI.Xaml.Controls;
using SuperMap.WinRT.Mapping;
using REST_SampleCode.Controls;
using Windows.Storage;
namespace REST_SampleCode
{
    public partial class Navigation : Page
    {
        TiledDynamicRESTLayer _layer;

        public Navigation()
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
            _layer = MyMap.Layers["restLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += Navigation_Failed;
        }

        async void Navigation_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

    }
}
