using REST_SampleCode.Controls;
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
    public partial class DataBinding : Page
    {
        private Random random = new Random();
        private const double ChinaLeft = 70;
        private const double ChinaRight = 130;
        private const double ChinaBottom = 4;
        private const double ChinaTop = 50;

        public DataBinding()
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
            this.LayoutRoot.Loaded += new RoutedEventHandler(LayoutRoot_Loaded);
            MyMap.Layers["tiledLayer"].Failed += DataBinding_Failed;
            this.Unloaded += DataBinding_Unloaded;
        }

        void DataBinding_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= DataBinding_Unloaded;
            this.LayoutRoot.Loaded -= new RoutedEventHandler(LayoutRoot_Loaded);
            MyMap.Layers["tiledLayer"].Failed -= DataBinding_Failed;
            MyMap.Dispose();
        }

        async void DataBinding_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //设置椭圆的颜色
        private Brush GetRandomColor()
        {
            byte r = (byte)random.Next(255);
            byte g = (byte)random.Next(255);
            byte b = (byte)random.Next(255);
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            FeaturesLayer featureslayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            MarkerStyle style = this.LayoutRoot.Resources["customStyle"] as MarkerStyle;

            //将feature的ID、Fill和Size属性绑定到随机生成50个椭圆
            for (int i = 0; i < 50; i++)
            {
                double x = random.NextDouble() * (ChinaRight - ChinaLeft) + ChinaLeft;
                double y = random.NextDouble() * (ChinaTop - ChinaBottom) + ChinaBottom;
                Feature feature = new Feature() { Geometry = new GeoPoint(x, y), Style = style };
                feature.Attributes.Add("ID", i);
                feature.Attributes.Add("Size", random.NextDouble() * 20);
                feature.Attributes.Add("Fill", GetRandomColor());
                featureslayer.Features.Add(feature);
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomTo(new Rectangle2D(75, 5, 138, 55));
        }
    }
}
