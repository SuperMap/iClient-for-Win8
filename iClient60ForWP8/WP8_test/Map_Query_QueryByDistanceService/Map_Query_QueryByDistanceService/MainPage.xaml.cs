using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.iServerJava6R;
using SuperMap.WindowsPhone.Core;


namespace Map_Query_QueryByDistanceService
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FeaturesLayer featuresLayer;
        private TiledDynamicRESTLayer tdRESTLayer;
        private Action _action;
        ElementsLayer _eLayer;
        
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World";
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            featuresLayer = MyMap.Layers["MyFeatureLayer"] as FeaturesLayer;
            tdRESTLayer = MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
            _eLayer = MyMap.Layers["ELayer"] as ElementsLayer;
           
            MyMap.Tap+=MyMap_Tap;

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        private void MyMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(_action!=null)
            {
                _action.Tap(e);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_action != null)
            {
                _action.Deactivate();
            }
           // _action = new DrawPoint(MyMap, _eLayer);
            //_action.DrawCompleted+=new EventHandler<DrawEventArgs> (drawCompleted);
           DrawPoint point =new DrawPoint(MyMap,_eLayer);
           _action = point;
           point.DrawCompleted += new EventHandler<DrawEventArgs>(drawCompleted); 
            
        }

        private async void drawCompleted(object sender, DrawEventArgs e)
        {
            QueryByDistanceParameters parm = new QueryByDistanceParameters
            {
                Geometry = e.Geometry,
                Distance = 1,
                ReturnContent = true,
                IsNearest = true,
                ReturnCustomResult = false,
                QueryParameters = new QueryParameterSet
                {
                    ExpectCount = 1,
                    QueryOption=QueryOption.ATTRIBUTEANDGEOMETRY,
                    QueryParams = new List<QueryParameter>() 
                    {
                        new QueryParameter()
                        {   
                            Name = "Countries@World", 
                        }
                    }
                }
            };
            try
            {
                QueryByDistanceService service = new QueryByDistanceService(url);
                var result = await service.ProcessAsync(parm);

                //无查询结果情况
                if (result == null)
                {
                    MessageBox.Show("查询结果为空");
                    return;
                }
                if (featuresLayer.Features.Count > 0)
                {
                    featuresLayer.ClearFeatures();
                }

                foreach (Recordset recordset in result.Recordsets)
                {
                    foreach (Feature f in recordset.Features)
                    {
                        featuresLayer.Features.Add(f);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private async void drawCompleted(object sender, DrawEventArgs e)
        //{
        //    QueryByDistanceParameters parm = new QueryByDistanceParameters
        //    {
        //        Geometry = e.Geometry,
        //        Distance = 1,
        //        ReturnContent = true,
        //        IsNearest = false,
        //        ReturnCustomResult = false,
        //        QueryParameters = new QueryParameterSet
        //        {
        //            ExpectCount = 1,
        //            QueryParams = new List<QueryParameter>() 
        //            { 
        //                new QueryParameter()
        //                {
        //                    Name = "Countries@World", 
        //                }
        //            }
        //        }
        //    };
        //    try 
        //    {
        //        QueryByDistanceService service = new QueryByDistanceService(url);
        //        var result = await service.ProcessAsync(parm);

        //        //无查询结果情况
        //        if(result==null)
        //        {
        //            MessageBox.Show("查询结果为空");
        //            return;
        //        }
        //        if(featuresLayer.Features.Count>0)
        //        {
        //            featuresLayer.ClearFeatures();
        //        }
                
        //        foreach (Recordset recordset in result.Recordsets)
        //        {
        //            foreach (Feature f in recordset.Features)
        //            {
        //                featuresLayer.Features.Add(f);
        //            }
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
                
        //}

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