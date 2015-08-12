using REST_SampleCode.Controls;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Theme;
using System;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;



namespace REST_SampleCode
{
    public partial class UIActionsInElementsLayer : Page
    {
        private ElementsLayer elementsLayer;
        private FeaturesLayer featuresLayer;
        TiledDynamicRESTLayer _layer;

        public UIActionsInElementsLayer( )
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

            MyMap.Theme = new RawTheme();

            elementsLayer = MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            featuresLayer = MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += UIActionsInElementsLayer_Failed;
            this.Unloaded += UIActionsInElementsLayer_Unloaded;
        }

        void UIActionsInElementsLayer_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= UIActionsInElementsLayer_Failed;
            this.Unloaded -= UIActionsInElementsLayer_Unloaded;
            MyMap.Dispose();
        }

        private async void UIActionsInElementsLayer_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //绘制Pushpin
        private void btn_Point_Click(object sender , RoutedEventArgs e)
        {
            DrawPoint point = new DrawPoint(this.MyMap);
            this.MyMap.Action = point;
            point.DrawCompleted += new EventHandler<DrawEventArgs>(point_DrawCompleted);
        }

        //绘制点结束后将所绘制的点转化为图钉并加载到ElementsLayer中，在加载前先设置点要素样式
        private void point_DrawCompleted(object sender , DrawEventArgs e)
        {
            this.elementsLayer.AddChild(e.Element as Pushpin);
        }

        //绘制线
        private void btn_Line_Click(object sender , RoutedEventArgs e)
        {
            DrawLine line = new DrawLine(this.MyMap);
            this.MyMap.Action = line;

            line.DrawCompleted += new EventHandler<DrawEventArgs>(line_DrawCompleted);
        }

        //绘制线结束后将所绘制的线转化为线元素并加载到ElementsLayer中，在加载前先设置线元素样式
        private void line_DrawCompleted(object sender , DrawEventArgs e)
        {
            //设置线元素样式
            Feature feature = new Feature
            {
                Geometry = e.Geometry
            };

            this.featuresLayer.AddFeature(feature);
        }

        //绘制多边形
        private void btn_Region_Click(object sender , RoutedEventArgs e)
        {
            DrawPolygon region = new DrawPolygon(this.MyMap);
            this.MyMap.Action = region;
            region.DrawCompleted += new EventHandler<DrawEventArgs>(region_DrawCompleted);
        }

        //绘制多边形结束后将所绘制的多边形转化为面元素并加载到ElementsLayer中，在加载前先设置该元素样式
        private void region_DrawCompleted(object sender , DrawEventArgs e)
        {
            Feature feature = new Feature
            {
                Geometry = e.Geometry
            };

            this.featuresLayer.AddFeature(feature);
        }

        //绘制自由线
        private void btn_FreeHand_Click(object sender , RoutedEventArgs e)
        {
            DrawFreeLine freehand = new DrawFreeLine(this.MyMap);
            this.MyMap.Action = freehand;
            freehand.DrawCompleted += new EventHandler<DrawEventArgs>(freehand_DrawCompleted);
        }

        //绘制自由线结束后将所绘制的自由线转化为线元素并加载到ElementsLayer中，在加载前先设置该元素样式
        private void freehand_DrawCompleted(object sender , DrawEventArgs e)
        {
            Feature feature = new Feature
            {
                Geometry = e.Geometry
            };
            this.featuresLayer.AddFeature(feature);
        }

        //绘制圆
        private void btn_Circle_Click(object sender , RoutedEventArgs e)
        {
            DrawCircle circle = new DrawCircle(this.MyMap);
            this.MyMap.Action = circle;
            circle.DrawCompleted += new EventHandler<DrawEventArgs>(circle_DrawCompleted);
        }

        //绘制圆结束后将所绘制的圆转化为椭圆（silverlight的元素）并加载到ElementsLayer中，在加载前先设置该元素样式
        private void circle_DrawCompleted(object sender , DrawEventArgs e)
        {
            Ellipse ellipse = e.Element as Ellipse;

            //将椭圆加载到ElementsLayer中
            this.elementsLayer.Children.Add(ellipse);
        }

        //绘制矩形
        private void btn_Rectangle_Click(object sender , RoutedEventArgs e)
        {
            DrawRectangle rectangle = new DrawRectangle(this.MyMap);
            this.MyMap.Action = rectangle;
            rectangle.DrawCompleted += new EventHandler<DrawEventArgs>(rectangle_DrawCompleted);
        }

        //绘制矩形结束后将所绘制的矩形转化为面元素并加载到ElementsLayer中，在加载前先设置该元素样式
        private void rectangle_DrawCompleted(object sender , DrawEventArgs e)
        {
            Feature feature = new Feature
            {
                Geometry = e.Geometry
            };

            this.featuresLayer.AddFeature(feature);
        }

        //绘制自由面
        private void btn_FreeRegion_Click(object sender , RoutedEventArgs e)
        {
            DrawFreePolygon polygon = new DrawFreePolygon(this.MyMap);
            this.MyMap.Action = polygon;
            polygon.DrawCompleted += new EventHandler<DrawEventArgs>(polygon_DrawCompleted);
        }

        //绘制自由面结束后将所绘制的自由面转化为面元素并加载到ElementsLayer中
        private void polygon_DrawCompleted(object sender , DrawEventArgs e)
        {
            this.elementsLayer.AddChild(e.Element as PolygonElement);
        }

        //清除ElementsLayer中全部元素
        private void btn_Clear_Click(object sender , RoutedEventArgs e)
        {
            this.elementsLayer.Children.Clear();
            this.featuresLayer.ClearFeatures();
        }

        //平移
        private void btn_Pan_Click(object sender , RoutedEventArgs e)
        {
            this.MyMap.Action = new Pan(this.MyMap);
        }

        //拉框缩小
        private void btn_ZoomOut_Click(object sender , RoutedEventArgs e)
        {
            this.MyMap.Action = new ZoomOut(this.MyMap);
        }

        //拉框放大，并设置拉框的样式
        private void btn_ZoomIn_Click(object sender , RoutedEventArgs e)
        {
            this.MyMap.Action = new ZoomIn(this.MyMap);
        }
    }
}
