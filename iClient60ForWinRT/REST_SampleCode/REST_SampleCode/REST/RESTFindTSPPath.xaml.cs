using System;
using System.Collections.Generic;
using System.Windows;

using System.Windows.Input;

using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.NetworkAnalyst;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using REST_SampleCode.Controls;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class RESTFindTSPPath : Page
    {
        private ElementsLayer elementsLayer;
        private FeaturesLayer featuresLayer;
        private List<Point2D> points = new List<Point2D>();
        private int i = 0;
        TiledDynamicRESTLayer _layer;
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/components-rest/rest/networkanalyst/RoadNet@Changchun";

        public RESTFindTSPPath()
        {
            InitializeComponent();
            double [] resolutions=new double[14];
            double resolution = 9.9772839315388211;
            for (int i = 0; i < 14;i++ )
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            elementsLayer = this.MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            featuresLayer = this.MyMap.Layers["MyFeaturesLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("长春市区图");
            _layer.Failed += RESTFindTSPPath_Failed;
            this.Unloaded += RESTFindTSPPath_Unloaded;
        }

        void RESTFindTSPPath_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTFindTSPPath_Failed;
            this.Unloaded -= RESTFindTSPPath_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTFindTSPPath_Failed(object sender, LayerFailedEventArgs e)
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
            //标记途经结点顺序
            i++;

            //将结点以图钉样式加到ElementsLayer上
            Pushpin pushpin = new Pushpin();
            pushpin.Location = e.Geometry.Bounds.Center;
            pushpin.Content = i.ToString();
            elementsLayer.AddChild(pushpin);

            //用points数组记录结点坐标
            points.Add(pushpin.Location);
        }

        async private void PathAnalyst_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count == 0)
            {
                await MessageBox.Show("请指定途经点");
                return;
            }

            FindTSPPathsParameters<Point2D> paramPoint2D = new FindTSPPathsParameters<Point2D>
            {
                EndNodeAssigned = (bool)terminal.IsChecked,
                Nodes = points,
                Parameter = new TransportationAnalystParameter
                {
                    TurnWeightField = "TurnCost",
                    WeightFieldName = "length",
                    ResultSetting = new TransportationAnalystResultSetting
                    {
                        ReturnEdgeFeatures = true,
                        ReturnEdgeGeometry = true,
                        ReturnEdgeIDs = true,
                        ReturnNodeFeatures = true,
                        ReturnNodeGeometry = true,
                        ReturnNodeIDs = true,
                        ReturnPathGuides = true,
                        ReturnRoutes = true,
                    }
                },
            };
            //与服务器交互
            try
            {
                FindTSPPathsService findTSPPathsService = new FindTSPPathsService(url);
                var result = await findTSPPathsService.ProcessAsync(paramPoint2D);

                //路径样式
                PredefinedLineStyle simpleLineStyle = new PredefinedLineStyle();
                simpleLineStyle.Stroke = new SolidColorBrush(Colors.Blue);
                simpleLineStyle.StrokeThickness = 2;

                if (result != null && result.TSPPathList != null)
                {
                    foreach (ServerPath p in result.TSPPathList)
                    {
                        //将要素添加到图层上
                        featuresLayer.Features.Add(new Feature { Geometry = p.Route.Line, Style = simpleLineStyle });
                    }
                }
            }
            //服务器计算失败提示失败信息
            catch (Exception ex)
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

        //清除 ElementsLayer 和 FeaturesLayer 图层上全部元素
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            elementsLayer.Children.Clear();
            featuresLayer.ClearFeatures();
            points.Clear();
            i = 0;
        }
    }
}
