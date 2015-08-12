using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Service;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using REST_SampleCode.Controls;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class RESTThemeRange : Page
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

        public RESTThemeRange()
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
            _layer.Failed += RESTThemeRange_Failed;
            this.Unloaded += RESTThemeRange_Unloaded;
        }

        void RESTThemeRange_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTThemeRange_Failed;
            this.Unloaded -= RESTThemeRange_Unloaded;
            MyMap.Dispose();
        }

        async void RESTThemeRange_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //点击生成专题图触发事件
        private async void commit_Click(object sender, RoutedEventArgs e)
        {
            //设置专题图子项
            List<ThemeRangeItem> themeRangeItems = new List<ThemeRangeItem>
            {
                new ThemeRangeItem
                {
                    Start = 0.0,
                    End = 59973,
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=216,G=244,B=254},
                        LineWidth = 0.1,
                    }
                },
                
                new ThemeRangeItem
                {
                    Start = 59973,
                    End = 1097234,
                    Visible = true ,
                    Style = new ServerStyle 
                    {
                         FillForeColor = new Color {R=131,G=232,B=252},
                         LineWidth = 0.1 ,
                    }
                },

                new ThemeRangeItem
                {
                    Start = 1097234,
                    End = 5245515,
                    Visible = true ,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=112,G=212,B=243},
                        LineWidth = 0.1,
                    }
                },
                 
                new ThemeRangeItem
                {
                    Start = 5245515,
                    End = 17250390,
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=23,G=198,B=238},
                        LineWidth = 0.1,
                    }
                },

                new ThemeRangeItem
                {
                    Start = 17250390,
                    End = 894608700,
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=0,G=187,B=236},
                        LineWidth = 0.1 ,
                    }
                },

                new ThemeRangeItem
                {
                    Start = 894608700,
                    End =  12E+8,
                    Visible = true ,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=0,G=133,B=236},
                        LineWidth = 0.1 ,
                    }
                }
            };
            //设置范围分段专题图字段、分段模式、分段参数和子项数组
            ThemeRange themeRange = new ThemeRange
            {
                RangeExpression = "Pop_1994",
                RangeMode = RangeMode.EQUALINTERVAL,
                RangeParameter = 6,
                Items = themeRangeItems
            };
            //设置专题图参数
            ThemeParameters themeRangeParams = new ThemeParameters
            {
                //数据集名称
                DatasetName = "Countries",
                //数据源名称
                DataSourceName = "World",
                Themes = new List<Theme> { themeRange }
            };
            //与服务器交互
            try
            {
                ThemeService themeRangeServie = new ThemeService(url);
                var result = await themeRangeServie.ProcessAsync(themeRangeParams);
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
