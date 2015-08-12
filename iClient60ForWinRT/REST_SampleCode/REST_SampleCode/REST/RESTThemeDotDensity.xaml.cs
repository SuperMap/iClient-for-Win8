using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using REST_SampleCode.Controls;
using Windows.UI;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class RESTThemeDotDensity : Page
    {
        TiledDynamicRESTLayer _layer;
        //世界图层服务地址
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World";

        //定义专题图层
        private TiledDynamicRESTLayer themeLayer = new TiledDynamicRESTLayer()
        {
            Url = url,
            IsVisible=false,
            //设置图层是否透明,true 表示透明。 
            Transparent = true
        };

        public RESTThemeDotDensity()
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
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += RESTThemeDotDensity_Failed;
            this.Unloaded += RESTThemeDotDensity_Unloaded;
        }

        void RESTThemeDotDensity_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTThemeDotDensity_Failed;
            this.Unloaded -= RESTThemeDotDensity_Unloaded;
            MyMap.Dispose();
        }

        async void RESTThemeDotDensity_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //点击生成专题图触发事件
        private async void commit_Click(object sender, RoutedEventArgs e)
        {
            ThemeDotDensity themeDotDensity = new ThemeDotDensity
            {
                //专题图中每一个点所代表的数值
                Value = 10000000,
                //1994年人口字段
                DotExpression = "Pop_1994",
                Style = new ServerStyle
                {
                    //设置点符号
                    MarkerSize = 2,
                    MarkerSymbolID = 1,
                    FillForeColor = Color.FromArgb(255, 0, 180, 150),

                }
            };
            //专题图参数设置
            ThemeParameters dotDensityParams = new ThemeParameters
            {
                //数据集名称
                DatasetName = "Countries",
                //数据源名称
                DataSourceName = "World",
                Themes = new List<Theme> 
                {
                    themeDotDensity
                }
            };
            //与服务器交互成功
            try
            {
                ThemeService dotDensityService = new ThemeService(url);
                var result = await dotDensityService.ProcessAsync(dotDensityParams);
                //显示专题图。专题图在服务端为一个资源，每个资源都有一个 ID 号和一个 url
                //要显示专题图即将资源结果的 ID 号赋值给图层的 layersID 属性即可
                themeLayer.LayersID = result.ResourceInfo.NewResourceID;

                if (!this.MyMap.Layers.Contains(themeLayer))
                {
                    //加载专题图图层
                    this.MyMap.Layers.Add(themeLayer);
                }
                if (!themeLayer.IsVisible)
                {
                    themeLayer.IsVisible = true;
                }
            }
            //与服务器交互失败
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //移除专题图
        async private void remove_Click(object sender, RoutedEventArgs e)
        {
            if (!themeLayer.IsVisible)
            {
                await MessageBox.Show("请先添加一幅专题图！");
            }
            else
            {
                themeLayer.IsVisible = false;
            }
        }
    }
}
