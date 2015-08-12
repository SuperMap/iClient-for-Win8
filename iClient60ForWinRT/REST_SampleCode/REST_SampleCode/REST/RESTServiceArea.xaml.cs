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
    public partial class RESTServiceArea : Page
    {
        private ElementsLayer elementsLayer;
        private Point2DCollection points = new Point2DCollection();
        private FeaturesLayer featuresLayer;
        private int i = 0;
       
        TiledDynamicRESTLayer _layer;

        public RESTServiceArea()
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
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("长春市区图");
            _layer.Failed += RESTServiceArea_Failed;
            this.Unloaded += RESTServiceArea_Unloaded;
        }

        void RESTServiceArea_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTServiceArea_Failed;
            this.Unloaded -= RESTServiceArea_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTServiceArea_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }
        
        //平移
        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            Pan pan = new Pan(MyMap);
            MyMap.Action = pan;
        }

        //选择服务区站点
        private void SelectPoint_Click(object sender, RoutedEventArgs e)
        {
            DrawPoint node = new DrawPoint(MyMap);
            MyMap.Action = node;
            node.DrawCompleted += new EventHandler<DrawEventArgs>(node_DrawCompleted);
        }

        private void node_DrawCompleted(object sender, DrawEventArgs e)
        {
            //标记服务区站点;

            //将站点以图钉样式加到ElementsLayer上
            Pushpin pushpin = new Pushpin();
            pushpin.Location = e.Geometry.Bounds.Center;
            pushpin.Content = (points.Count + 1).ToString();
            pushpin.Background = new SolidColorBrush(Colors.Red);
            elementsLayer.AddChild(pushpin);

            //用points数组记录结点坐标
            points.Add(pushpin.Location);





        }

        async private void PathAnalyst_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count == 0)
            {
                await MessageBox.Show("请指定服务点");
                return;
            }

            if (string.IsNullOrEmpty(MyTextBox.Text))
            {
                await MessageBox.Show("请填写服务半径");
                return;
            }
            double radius;
            if (double.TryParse(MyTextBox.Text, out radius))
            {
                if (radius <= 0)
                {
                    await MessageBox.Show("服务半径必须大于0");
                    return;
                }
            }
            else
            {
                await MessageBox.Show("服务半径必须为数值");
                return;
            }
            //listweights用来添加服务半径，list记录点，点与半径一一对应
            //每次新添加的点用新添加的服务半径进行分析，以前的几个点的服务半径不变
            List<double> listweights = new List<double>();
            List<Point2D> list = new List<Point2D>();
            for(int count=i;count<points.Count;count++)
            {
                list.Add(points[count]);
                listweights.Add(radius);
            }
            //记录点的个数，下次分析从此点开始进行分析，之前的点仍用原服务半径
            i=points.Count;


            //定义 Point2D 类型的参数
            FindServiceAreasParameters<Point2D> paramPoint2D = new FindServiceAreasParameters<Point2D>
            {
                Centers = list,
                Weights = listweights,
                Parameter = new TransportationAnalystParameter
                {
                    ResultSetting = new TransportationAnalystResultSetting
                    {
                        ReturnEdgeFeatures = true,
                        ReturnEdgeGeometry = true,
                        ReturnEdgeIDs = true,
                        ReturnPathGuides = true,
                        ReturnRoutes = true,
                    },
                    WeightFieldName = "length",
                    TurnWeightField = "TurnCost",
                }
            };

            //与服务器交互
            try
            {
                FindServiceAreasService findServiceAreaService = new FindServiceAreasService("http://support.supermap.com.cn:8090/iserver/services/components-rest/rest/networkanalyst/RoadNet@Changchun");
                var result = await findServiceAreaService.ProcessAsync(paramPoint2D);
               
                foreach (SuperMap.WinRT.REST.NetworkAnalyst.ServiceArea p in result.ServiceAreaList)
                {

                    //将要素添加到图层
                    PredefinedFillStyle style = new PredefinedFillStyle();
                    style.Fill = new SolidColorBrush(Color.FromArgb(120, 179, 235, 246));
                    Feature area = new Feature();
                    area.Geometry = p.ServiceRegion;
                    area.Style = style;
                    featuresLayer.AddFeature(area);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //清除 ElementsLayer 和 FeaturesLayer 图层上的全部元素
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            elementsLayer.Children.Clear();
            featuresLayer.ClearFeatures();
            points.Clear();
			i=0;
        }
    }
}
