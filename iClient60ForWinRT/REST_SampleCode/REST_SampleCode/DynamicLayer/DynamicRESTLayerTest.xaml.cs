
using REST_SampleCode.Controls;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace REST_SampleCode
{
    public partial class DynamicRESTLayerTest : Page
    {
        private DynamicRESTLayer restLayer;

        public DynamicRESTLayerTest()
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
            restLayer = this.MyMap.Layers["restLayer"] as DynamicRESTLayer;
            restLayer.Failed += restLayer_Failed;
            this.Unloaded += DynamicRESTLayerTest_Unloaded;
        }

        void DynamicRESTLayerTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= DynamicRESTLayerTest_Unloaded;
            restLayer.Failed -= restLayer_Failed;
            MyMap.Dispose();
        }

        async void restLayer_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void clipRegion_Click(object sender,RoutedEventArgs e)
        {
            DrawPolygon region = new DrawPolygon(this.MyMap);
            this.MyMap.Action = region;
            region.DrawCompleted += new System.EventHandler<DrawEventArgs>(region_DrawCompleted);
        }

        void region_DrawCompleted(object sender, DrawEventArgs e)
        {
            Feature feature = new Feature();
            feature.Geometry = e.Geometry;
            restLayer.ClipRegion = e.Geometry as GeoRegion;
            restLayer.Refresh();
            this.MyMap.Action = new Pan(this.MyMap);
        }
    }
}
