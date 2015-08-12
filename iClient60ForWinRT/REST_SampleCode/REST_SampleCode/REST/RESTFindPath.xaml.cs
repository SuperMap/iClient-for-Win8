using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.NetworkAnalyst;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using REST_SampleCode.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class RESTFindPath : Page
    {
        private ElementsLayer elementsLayer;
        private List<Point2D> points = new List<Point2D>();
        private List<Point2D> barrierPoints = new List<Point2D>();
        private FeaturesLayer featuresLayer;
        TiledDynamicRESTLayer _layer;
        private int i = 0;
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/transportationanalyst-sample/rest/networkanalyst/RoadNet@Changchun";

        public RESTFindPath()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 9.9772839315388211;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            elementsLayer = this.MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("长春市区图");
            _layer.Failed += RESTFindPath_Failed;
            this.Unloaded += RESTFindPath_Unloaded;
        }

        void RESTFindPath_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTFindPath_Failed;
            this.Unloaded -= RESTFindPath_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTFindPath_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //清除 ElementsLayer 和 FeaturesLayer 图层上全部元素
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            elementsLayer.Children.Clear();
            featuresLayer.ClearFeatures();
            points.Clear();
            barrierPoints.Clear();
            resultPanel.Visibility = Visibility.Collapsed;
            i = 0;
        }

        //平移
        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            Pan pan = new Pan(MyMap);
            MyMap.Action = pan;
        }

        //选择路径途经结点或障碍点
        private void SelectPoint_Click(object sender, RoutedEventArgs e)
        {
            DrawPoint node = new DrawPoint(MyMap);
            MyMap.Action = node;
            node.DrawCompleted += new EventHandler<DrawEventArgs>(node_DrawCompleted);
        }

        private void node_DrawCompleted(object sender, DrawEventArgs e)
        {
            if (this.pathPonit.IsChecked.Value)
            {
                //标记途经结点顺序
                i++;
                //将结点以图钉样式加到ElementsLayer上
                Pushpin pushpin = new Pushpin()
                {
                    Location = e.Geometry.Bounds.Center,
                    Content = i.ToString(),
                };
                elementsLayer.AddChild(pushpin);
                //用points数组记录结点坐标
                points.Add(pushpin.Location);
            }

            if (this.barrierPoint.IsChecked.Value)
            {
                Pushpin pushpin = new Pushpin()
                {
                    Location = e.Geometry.Bounds.Center,
                    Content = "Stop",
                    FontSize = 8,
                    Background = new SolidColorBrush(Colors.Black),
                };
                elementsLayer.AddChild(pushpin);
                //用MyBarrierPoint数组记录结点坐标
                barrierPoints.Add(pushpin.Location);
            }
        }

        //最佳路径分析
        async private void PathAnalyst_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count < 2)
            {
                await MessageBox.Show("请指定途经点");
                return;
            }

            if (featuresLayer.Features.Count > 0)
            {
                featuresLayer.Features.Clear();
            }
            //定义 Point2D 类型的参数
            FindPathParameters<Point2D> paramPoint2D = new FindPathParameters<Point2D>
            {
                Nodes = points,
                HasLeastEdgeCount = true,
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
                    TurnWeightField = "TurnCost",
                    BarrierPoints = barrierPoints,
                }
            };

            //与服务器交互
            try
            {
                FindPathService findPathService = new FindPathService(url);
                var result = await findPathService.ProcessAsync(paramPoint2D);

                //路径样式
                PredefinedLineStyle simpleLineStyle = new PredefinedLineStyle();
                simpleLineStyle.Stroke = new SolidColorBrush(Colors.Blue);
                simpleLineStyle.StrokeThickness = 2;
                //服务器返回结果，将最佳路径显示在客户端
                if (result != null)
                {
                    if (result.PathList != null)
                    {
                        foreach (ServerPath p in result.PathList)
                        {
                            //将要素添加到图层
                            featuresLayer.Features.Add(new Feature { Geometry = p.Route.Line, Style = simpleLineStyle });
                            this.length.Text = "路线长度：" + p.Route.Length.ToString();
                            this.cost.Text = "花费:" + p.Weight.ToString();
                        }
                        resultPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        await MessageBox.Show("此设置下无可用路径");
                    }
                }

            }
            //服务器计算失败提示失败信息
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
