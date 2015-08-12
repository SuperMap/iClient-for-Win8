using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.iServerJava6R;

namespace Map_Query_QueryByGeometryService
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FeaturesLayer _feature;
        private ElementsLayer _elementLayer;
        private TiledDynamicRESTLayer _Layer;
        private Action _action;
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World";
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            _elementLayer = MyMap.Layers["ELayer"] as ElementsLayer;
            _feature = MyMap.Layers["MyFeatureLayer"] as FeaturesLayer;
            double[] resolutions = new double[14];
            double resolution = 0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            MyMap.Tap += MyMap_Tap;
        }

        void MyMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(_action!=null)
            {
                _action.Tap(e);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawPoint point = new DrawPoint(MyMap,_elementLayer);
            _action = point;
            point.DrawCompleted += new EventHandler<DrawEventArgs>(DrawCompleted);
        }

        private async void DrawCompleted(object sender, DrawEventArgs e)
        {
            QueryByGeometryParameters para = new QueryByGeometryParameters
            {
                Geometry = e.Geometry,
                ReturnContent=true,
                SpatialQueryMode = SpatialQueryMode.INTERSECT,
                
                QueryParameters = new QueryParameterSet
                {
                    ResampleExpectCount=0,
                    
                    QueryOption=QueryOption.ATTRIBUTEANDGEOMETRY,
                    QueryParams = new List<QueryParameter>() 
                    {
                        
                        new QueryParameter
                        {
                            Name = "Countries@World", 
                         
                        }
                    }
                }
            };
            try
            {
                QueryByGeometryService service = new QueryByGeometryService(url);
                var result = await service.ProcessAsync(para);
                if(result==null)
                {
                    MessageBox.Show("返回结果没空");
                    return;
                }
                if(_feature.Features.Count>0)
                {
                    _feature.ClearFeatures();
                }
                foreach(Recordset recordset in result.Recordsets)
                {
                    foreach(Feature f in recordset.Features)
                    {
                        _feature.AddFeature(f);
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}