using REST_SampleCode.Controls;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using System;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace REST_SampleCode
{
    public partial class RESTGeometryQueryTest : Page
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World";

        //定义一个矢量图层，用于显示查询结果
        private FeaturesLayer flayer;
        TiledDynamicRESTLayer _layer;

        //创建一个查询结果高亮图层
        HighlightLayer highlayer = new HighlightLayer(url);

        //判断是否已经加载了高亮图层
        private bool isAdded;

        //判断是否使用高亮图层
        private bool notHighlight;

        public RESTGeometryQueryTest()
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
            flayer = MyMap.Layers["FeaturesLayer"] as FeaturesLayer;

            //当在 FeaturesLayer 上通过鼠标左键点击触发的事件
            flayer.PointerPressed += flayer_MouseLeftButtonDown;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += RESTGeometryQueryTest_Failed;
            this.Unloaded += RESTGeometryQueryTest_Unloaded;
        }

        void RESTGeometryQueryTest_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTGeometryQueryTest_Failed;
            this.Unloaded -= RESTGeometryQueryTest_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTGeometryQueryTest_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //绘制点操作，用于点查询
        private void Point_Click(object sender, RoutedEventArgs e)
        {
            DrawPoint point = new DrawPoint(MyMap);
            MyMap.Action = point;

            //绘制结束调用 DrawCompleted 函数
            point.DrawCompleted += new EventHandler<DrawEventArgs>(DrawCompleted);

        }

        //绘制线操作，用于线查询
        private void Line_Click(object sender, RoutedEventArgs e)
        {
            DrawLine line = new DrawLine(MyMap);
            MyMap.Action = line;

            //绘制结束调用 DrawCompleted 函数
            line.DrawCompleted += new EventHandler<DrawEventArgs>(DrawCompleted);
        }

        //绘制面操作，用于面查询
        private void Region_Click(object sender, RoutedEventArgs e)
        {
            DrawPolygon polygon = new DrawPolygon(MyMap);
            MyMap.Action = polygon;

            //绘制结束调用 DrawCompleted 函数
            polygon.DrawCompleted += new EventHandler<DrawEventArgs>(DrawCompleted);
        }

        //几何对象绘制结束触发事件
        private async void DrawCompleted(object sender, DrawEventArgs e)
        {


            //设置几何查询参数，FilterParameters、Geometry 和 SpatialQueryMode 为必设属性
            QueryByGeometryParameters parameter = new QueryByGeometryParameters
            {
                FilterParameters = new List<FilterParameter>() 
                {             
                        new FilterParameter()
                       {
                           Name = "Countries@World",                          
                       }
                },
                Geometry = e.Geometry,
                SpatialQueryMode = SpatialQueryMode.INTERSECT,

                //设置是返回查询结果资源（false）还是返回查询结果记录集（true）
                ReturnContent = notHighlight
            };

            //与服务器交互
            try
            {
                QueryByGeometryService service = new QueryByGeometryService(url);
                var result = await service.ProcessAsync(parameter);
                //无查询结果的情况
                if (result == null)
                {
                    await MessageBox.Show("查询结果为空！");
                    return;
                }

                //如果当前矢量图层中存在上一次的查询地物，则清除
                if (flayer.Features.Count > 0)
                {
                    flayer.Features.Clear();
                }

                highlayer.IsVisible = false;
                highlayer.Refresh();

                //当 ReturnContent = false 且有查询结果时，返回结果中的 ResourceInfo 不为空
                //当 ReturnContent = true 且有查询结果时，返回结果中的 Recordsets 不为空，ResourceInfo 为空
                if (result.ResourceInfo != null)
                {
                    highlayer.IsVisible = true;

                    //获取高亮图片在服务器上存储的资源 ID 号
                    highlayer.QueryResultID = result.ResourceInfo.NewResourceID;

                    //设置服务端高亮图层的样式
                    highlayer.Style = new ServerStyle() { FillForeColor = new Color { R = 0, G = 160, B = 0 }, LineWidth = 0, FillOpaqueRate = 90 };

                    //该判断控制只加载一个高亮图层
                    if (!isAdded)
                    {
                        //刷新高亮图层
                        highlayer.Refresh();
                        this.MyMap.Layers.Add(highlayer);
                        isAdded = true;
                    }
                }
                else
                {


                    //将查询结果记录集中的数据转换为矢量要素并在矢量图层中显示
                    foreach (Recordset recordset in result.Recordsets)
                    {
                        foreach (Feature f in recordset.Features)
                        {
                            flayer.Features.Add(f);
                        }
                    }

                    //将查询到的地物赋予 XMAL 中自定义样式
                    foreach (Feature item in flayer.Features)
                    {
                        if (item.Geometry is GeoLine)
                        {
                            item.Style = LineSelectStyle;
                        }
                        else if (item.Geometry is GeoRegion)
                        {
                            item.Style = SelectStyle;
                        }
                        else
                        {
                            item.Style = new PredefinedMarkerStyle() { Color = new SolidColorBrush(Colors.Blue), Size = 20, Symbol = PredefinedMarkerStyle.MarkerSymbol.Diamond };
                        }
                    }
                }
            }
            //服务器查询失败
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void flayer_MouseLeftButtonDown(object sender, FeaturePointerRoutedEventArgs args)
        {
            //如果矢量已选中，再点击变为非选中状态，反之亦然
            args.Feature.Selected = !args.Feature.Selected;

            args.Handler = true;
        }
        //界面中下拉框发生改变时触发
        private void mycb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                //当选择"使用高亮图层显示结果"项时
                case 0:
                    notHighlight = false;
                    break;

                //当选择"使用矢量图层显示结果"项时
                case 1:
                    notHighlight = true;
                    break;
            }
        }

        //平移操作
        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            Pan p = new Pan(MyMap);
            MyMap.Action = p;
        }

        //清除所有要素、隐藏高亮图层和 FeatureDataGrid 控件
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            flayer.Features.Clear();
            highlayer.Refresh();
            highlayer.IsVisible = false;
        }
    }
}
