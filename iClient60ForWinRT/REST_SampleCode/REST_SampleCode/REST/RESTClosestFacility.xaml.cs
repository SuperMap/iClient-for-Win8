using REST_SampleCode.Controls;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.NetworkAnalyst;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using System;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace REST_SampleCode
{
    public partial class RESTClosestFacility : Page
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/transportationanalyst-sample/rest/networkanalyst/RoadNet@Changchun";
        private ElementsLayer elementsLayerE;
        private ElementsLayer elementsLayerF;
        private FeaturesLayer featuresLayer;
        private Point2DCollection points = new Point2DCollection();
        private Point2D eventp = new Point2D();

        private bool flag = false;
        TiledDynamicRESTLayer _layer;

        public RESTClosestFacility()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 9.9772839315388211;
            for (int i = 0; i < 14;i++ )
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            elementsLayerE = this.MyMap.Layers["MyElementsLayerE"] as ElementsLayer;
            elementsLayerF = this.MyMap.Layers["MyElementsLayerF"] as ElementsLayer;
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("长春市区图");
            _layer.Failed += RESTClosestFacility_Failed;
            this.Unloaded += RESTClosestFacility_Unloaded;
        }

        void RESTClosestFacility_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTClosestFacility_Failed;
            this.Unloaded -= RESTClosestFacility_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTClosestFacility_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void SelectPoint_Click(object sender, RoutedEventArgs e)
        {
            DrawPoint node = new DrawPoint(MyMap);
            MyMap.Action = node;
            node.DrawCompleted += new EventHandler<DrawEventArgs>(node_DrawCompleted);
        }

        private void node_DrawCompleted(object sender, DrawEventArgs e)
        {
            Pushpin pushpin = new Pushpin();
            

            //将事件点以图钉样式加到ElementsLayerE上
            if (eventPonit.IsChecked == true)
            {
                //因为事件点只有一个，因此判断是否存在事件点，如果存在就先将以前的事件点删除
                if (flag == true)
                {
                    elementsLayerE.Children.Clear();
                    featuresLayer.ClearFeatures();
                }
                pushpin.Location = e.Geometry.Bounds.Center;
                pushpin.Content = "E";
                pushpin.Background = new SolidColorBrush(Colors.Red);
                elementsLayerE.AddChild(pushpin);
                flag = true;

                //记录事件点坐标
                eventp = pushpin.Location;
            }

            //将设施点以图钉样式加到ElementsLayer上
            if (FacilityPoint.IsChecked == true)
            {
                pushpin.Location = e.Geometry.Bounds.Center;
                pushpin.Content = "F";
                pushpin.Background = new SolidColorBrush(Colors.Purple);
                elementsLayerF.AddChild(pushpin);

                //用 points 数组记录设施点坐标
                points.Add(pushpin.Location);
            }
        }

        //最近设施查找
        async private void FindClosetFacility_Click(object sender, RoutedEventArgs e)
        {
            if (featuresLayer.Features.Count > 0)
            {
                featuresLayer.Features.Clear();
            }

            if (Point2D.IsNullOrEmpty(eventp))
            {
                await MessageBox.Show("请指定事件点");
                return;
            }

            if (points.Count == 0)
            {
               await  MessageBox.Show("请指设施点");
                return;
            }

            FindClosestFacilitiesParameters<Point2D> paramPoint2D = new FindClosestFacilitiesParameters<Point2D>
            {
                Event = eventp,
                Facilities = points,
                FromEvent = true,
                MaxWeight = 0,
                ExpectFacilityCount = 1,
                Parameter = new TransportationAnalystParameter
                {
                    ResultSetting = new TransportationAnalystResultSetting
                    {
                        ReturnEdgeFeatures = true,
                        ReturnEdgeGeometry = true,
                        ReturnEdgeIDs = true,
                        ReturnNodeFeatures = true,
                        ReturnNodeGeometry = true,
                        ReturnNodeIDs = true,
                        ReturnPathGuides = true,
                        ReturnRoutes = true
                    },
                    WeightFieldName = "length",
                    TurnWeightField = "TurnCost"
                }
            };
            try
            {
                FindClosestFacilitiesService findclosestFacilitiesService = new FindClosestFacilitiesService(url);
                var result = await findclosestFacilitiesService.ProcessAsync(paramPoint2D);
                //路径样式
                PredefinedLineStyle simpleLineStyle = new PredefinedLineStyle();
                simpleLineStyle.Stroke = new SolidColorBrush(Colors.Blue);
                simpleLineStyle.StrokeThickness = 2;

                if (result != null && result.FacilityPathList != null)
                {
                    foreach (ServerPath path in result.FacilityPathList)
                    {
                        featuresLayer.Features.Add(new Feature { Geometry = path.Route.Line, Style = simpleLineStyle });
                    }
                }
                else { await MessageBox.Show("查询结果为空"); }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         

        //平移
        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            Pan pan = new Pan(MyMap);
            MyMap.Action = pan;
        }

        //清除图层上的全部元素并清空点坐标记录
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            elementsLayerF.Children.Clear();
            elementsLayerE.Children.Clear();
            featuresLayer.ClearFeatures();
            points.Clear();
            eventp = Point2D.Empty;
            flag = false;
        }
    }
}
