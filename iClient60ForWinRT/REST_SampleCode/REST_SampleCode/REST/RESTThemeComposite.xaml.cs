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
using Windows.Foundation;
using System.Net.Http;
using Windows.Storage;

namespace REST_SampleCode
{
    public partial class RESTThemeComposite : Page
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

        public RESTThemeComposite()
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
            _layer.Failed += RESTThemeComposite_Failed;
            this.Unloaded += RESTThemeComposite_Unloaded;
        }

        void RESTThemeComposite_Unloaded(object sender, RoutedEventArgs e)
        {
            _layer.Failed -= RESTThemeComposite_Failed;
            this.Unloaded -= RESTThemeComposite_Unloaded;
            MyMap.Dispose();
        }

        private async void RESTThemeComposite_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void commit_Click(object sender, RoutedEventArgs e)
        {
            //专题图子项数组
            List<ThemeUniqueItem> items = new List<ThemeUniqueItem> 
            {
                //专题图子项
                 new ThemeUniqueItem
                 {
                     //SmID字段值
                     Unique="1", 
                     Style=new ServerStyle
                     {
                         FillForeColor = new Color {R=1,G=128,B=171},
                         LineWidth = 0.1                       
                     },
                     Visible=true,
                 },
                 new ThemeUniqueItem
                 {
                     //SmID字段值
                     Unique="247",
                     Style=new ServerStyle
                     {
                         FillForeColor= new Color {R=192,G=214,B=54},
                         LineWidth = 0.1
                     },
                     Visible=true,
                 }
            };
            //设置其他 SmID 字段显示风格
            ThemeUnique themeUnique = new ThemeUnique
            {
                Items = items,
                UniqueExpression = "SmID",
                DefaultStyle = new ServerStyle
                {
                    FillOpaqueRate = 100,
                    FillForeColor = new Color { R = 250, G = 237, B = 195 },
                    LineWidth = 0.1,
                    FillBackOpaque = true,
                    FillBackColor = Colors.Transparent
                }
            };

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
            ThemeParameters compostionThemeParameters = new ThemeParameters
            {
                DatasetName = "Countries",
                DataSourceName = "World",
                Themes = new List<Theme> { themeDotDensity, themeUnique }
            };
            // //与服务器交互成功
            try
            {
                ThemeService compositeService = new ThemeService(url);
                var result =  await compositeService.ProcessAsync(compostionThemeParameters);
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
