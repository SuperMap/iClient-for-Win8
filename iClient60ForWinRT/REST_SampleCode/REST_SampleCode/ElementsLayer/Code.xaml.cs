using System;
using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;


namespace REST_SampleCode
{
    public partial class Code : Page
    {
        private ElementsLayer elementsLayer;
        TiledDynamicRESTLayer _layer;

        public Code()
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
            elementsLayer = this.MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
            _layer = MyMap.Layers["DynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += Code_Failed;
            this.Unloaded += Code_Unloaded;
        }

        void Code_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= Code_Unloaded;
            _layer.Failed -= Code_Failed;
            MyMap.Dispose();
        }

        async void Code_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            #region Ellipse
            Ellipse myellipse = new Ellipse();
            myellipse.Fill = new SolidColorBrush(Colors.Red);
            Rectangle2D ellbounds = new Rectangle2D(119.5, 21, 122.5, 26);
            this.elementsLayer.AddChild(myellipse, ellbounds);
            MyEllipseStory.Stop();

            Storyboard.SetTarget(myDoubleAnimation, myellipse);
            MyEllipseStory.Begin();

            #endregion

            #region Pushpin
            Pushpin beijing = new Pushpin();
            beijing.Location = new Point2D(116.2, 39.6);
            this.elementsLayer.AddChild(beijing);

            Pushpin ulanBator = new Pushpin();
            ulanBator.Location = new Point2D(106.5, 47.6);
            ulanBator.Background = new SolidColorBrush(Colors.Blue);
            this.elementsLayer.AddChild(ulanBator);

            Pushpin moscow = new Pushpin();
            moscow.Location = new Point2D(37.4, 55.5);
            moscow.Background = new SolidColorBrush(Colors.Yellow);
            this.elementsLayer.AddChild(moscow);


            Pushpin prague = new Pushpin();
            prague.Location = new Point2D(14.3, 50.1);
            prague.Background = new SolidColorBrush(Colors.Orange);
            this.elementsLayer.AddChild(prague);

            Pushpin berlin = new Pushpin();
            berlin.Location = new Point2D(13.3, 52.3);
            berlin.Background = new SolidColorBrush(Colors.Purple);
            this.elementsLayer.AddChild(berlin);
            #endregion

            #region Polylinebase
            PolygonElement line = new PolygonElement();
            line.Stroke = new SolidColorBrush(Colors.Yellow);
            line.StrokeThickness = 3;
            line.Opacity = 1;
            line.StrokeDashArray = new DoubleCollection { 3, 3 };
            line.StrokeEndLineCap = PenLineCap.Triangle;
            Point2DCollection points = new Point2DCollection();
            points.Add(new Point2D(121.3, 25));
            points.Add(new Point2D(116.2, 39.6));
            points.Add(new Point2D(106.5, 47.6));
            points.Add(new Point2D(37.4, 55.5));
            points.Add(new Point2D(14.3, 50.1));
            points.Add(new Point2D(13.3, 52.3));
            points.Add(new Point2D(0.1, 51.3));
            line.Point2Ds = points;
            this.elementsLayer.AddChild(line);
            #endregion

            #region PolygonElement
            PolygonElement polygon = new PolygonElement();
            polygon.Stroke = new SolidColorBrush(Colors.Purple);
            polygon.Opacity = 0.5;
            polygon.Fill = this.British;
            Point2DCollection gonpoints = new Point2DCollection();
            gonpoints.Add(new Point2D(-8, 61));
            gonpoints.Add(new Point2D(-6, 55));
            gonpoints.Add(new Point2D(-8, 50));
            gonpoints.Add(new Point2D(2, 50));
            gonpoints.Add(new Point2D(1, 61));
            polygon.Point2Ds = gonpoints;
            this.elementsLayer.AddChild(polygon);
            #endregion

            #region Button
            Button btn = new Button();
            btn.Content = "点击";
            Brush btnbackground = new SolidColorBrush(Colors.Blue);
            btn.SetValue(Button.BackgroundProperty, btnbackground);
            btn.Click += new RoutedEventHandler(btn_Click);
            Rectangle2D btnbounds = new Rectangle2D(95, 29, 115, 43);
            btn.SetValue(ElementsLayer.BBoxProperty, btnbounds);
            this.elementsLayer.AddChild(btn);
            #endregion

            MyMap.ViewBoundsChanged -= MyMap_ViewBoundsChanged;
        }

        async private void btn_Click(object sender, RoutedEventArgs e)
        {
            await MessageBox.Show("欢迎来到中国！");
        }
    }
}