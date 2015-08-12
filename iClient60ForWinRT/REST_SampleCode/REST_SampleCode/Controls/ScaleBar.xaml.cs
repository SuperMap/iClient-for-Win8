using System;
using REST_SampleCode.Controls;
using SuperMap.WinRT.Mapping;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace REST_SampleCode
{
    public partial class ScaleBar : Page
    {
        TiledDynamicRESTLayer _layer;

        public ScaleBar()
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
            _layer.Failed += ScaleBar_Failed;


        }

        async void ScaleBar_Failed(object sender, SuperMap.WinRT.Mapping.LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

    }
}
