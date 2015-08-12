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
using Windows.System.Threading;

namespace REST_SampleCode
{
    public partial class RESTThemeUnique : Page
    {
        TiledDynamicRESTLayer _layer;
        //京津图层服务地址
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-jingjin/rest/maps/京津地区土地利用现状图";

        //定义专题图层
        private TiledDynamicRESTLayer themeLayer = new TiledDynamicRESTLayer()
        {
            Url = url,
            IsVisible = false,
            //设置图层是否透明,true 表示透明。
            Transparent = true,
        };

        public RESTThemeUnique()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 0.0055641857128792931;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            _layer = MyMap.Layers["tiledDynamicRESTLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("京津地区土地利用现状图");
            _layer.Failed += RESTThemeUnique_Failed;
            this.Unloaded += RESTThemeUnique_Unloaded;
        }

        void RESTThemeUnique_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTThemeUnique_Failed;
            this.Unloaded -= RESTThemeUnique_Unloaded;
            MyMap.Dispose();
        }

        async void RESTThemeUnique_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //点击生成专题图触发事件
        private async void commit_Click(object sender, RoutedEventArgs e)
        {
            //专题图子项数组
            List<ThemeUniqueItem> items = new List<ThemeUniqueItem>
            {
                //专题图子项
                new ThemeUniqueItem
                {
                    Unique = "城市",
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=157,G=127,B=255},
                        LineWidth = 0.05   
                    }
                },

                new ThemeUniqueItem
                {
                    Unique = "旱地",
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=250,G=237,B=195},
                        LineWidth = 0.05
                    }
                },

                new ThemeUniqueItem
                {
                    Unique="水浇地",
                    Visible=true,
                    Style=new ServerStyle 
                    {
                        FillForeColor = new Color {R=59,G=188,B=230},
                        LineWidth = 0.05
                    }
                },

                new ThemeUniqueItem
                {
                    Unique = "湖泊水库",
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=1,G=128,B=171},
                        LineWidth = 0.05
                    }
                },

                new ThemeUniqueItem
                {
                    Unique = "水田",
                    Visible = true ,
                    Style = new ServerStyle 
                    {
                       FillForeColor = new Color {R=167,G=219,B=232},
                       LineWidth = 0.05
                    }
                },

                new ThemeUniqueItem
                {
                    Unique = "草地",
                    Visible = true,
                    Style = new ServerStyle 
                    {
                        FillForeColor = new Color {R=192,G=214,B=54},
                        LineWidth = 0.05
                    }
                },
            };
            //设置其他土地利用类型显示风格
            ThemeUnique themeUnique = new ThemeUnique
            {
                Items = items,
                UniqueExpression = "LandType",
                DefaultStyle = new ServerStyle
                {
                    FillOpaqueRate = 100,
                    FillForeColor = new Color
                    {
                        R = 80,
                        G = 130,
                        B = 255
                    },
                    FillBackOpaque = true,
                    LineWidth = 0.05,

                }
            };
            //专题图参数对象
            ThemeParameters themeUniqueParameters = new ThemeParameters
            {
                Themes = new List<Theme> { themeUnique },
                //数据集名称
                DatasetName = "Landuse_R",
                //数据源名称
                DataSourceName = "Jingjin"
            };

            //与服务器交互
            try
            {
                ThemeService themeUniqueService = new ThemeService(url);
                var result = await themeUniqueService.ProcessAsync(themeUniqueParameters);

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
            //交互失败
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
