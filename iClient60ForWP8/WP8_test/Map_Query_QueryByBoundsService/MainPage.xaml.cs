using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.iServerJava6R;
using SuperMap.WindowsPhone.Core;


namespace Map_Query_QueryByBoundsService
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string url = "http://192.168.169.52:8090/iserver/services/map-world/rest/maps/World";
        private FeaturesLayer _fLayer;
        private ElementsLayer _eLayer;
        private TiledDynamicRESTLayer _DLayer;
        private Action _action;

        Rectangle2D bounds = new Rectangle2D(88.653846153846,34.615384615385,120.19230769231,55);
        public MainPage()
        {
            InitializeComponent();
            _fLayer = MyMap.Layers["MyFeatureLayer"] as FeaturesLayer;
            _eLayer = MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            _DLayer = MyMap.Layers["TDRESTLayer"] as TiledDynamicRESTLayer;
            MyMap.MouseLeftButtonDown += MyMap_MouseLeftButtonDown;
            MyMap.MouseMove += MyMap_MouseMove;
            MyMap.MouseLeftButtonUp += MyMap_MouseLeftButtonUp;
            
        }

        void MyMap_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_action != null)
            {
                _action.OnMouseLeftButtonUp(e);
            }
        }

        void MyMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_action != null)
            {
                _action.OnMouseMove(e);
            }
        }

        void MyMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_action != null)
            {
                _action.OnMouseLeftButtonDown(e);
            }
        }

       

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            QueryByBoundsParameters para = new QueryByBoundsParameters
            {
                Bounds = bounds,
                ReturnContent = true,
                QueryParameters = new QueryParameterSet
                {
                    QueryOption = QueryOption.ATTRIBUTEANDGEOMETRY,
                    ExpectCount = 1000,
                    QueryParams = new List<QueryParameter>()
                    {
                        new QueryParameter
                        {
                            Name="Capitals@World.1"
                        }
                    }
                }
            };
            try
            {
                QueryByBoundsService service = new QueryByBoundsService(url);
                var result = await service.ProcessAsync(para);
                if (result == null)
                {
                    MessageBox.Show("返回结果没空");
                    return;
                }
                if (_fLayer.Features.Count > 0)
                {
                    _fLayer.ClearFeatures();
                }
                foreach (Recordset recordset in result.Recordsets)
                {
                    foreach (Feature f in recordset.Features)
                    {
                        _fLayer.AddFeature(f);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
           

        }

        //private async void DrawCompleted(object sender, DrawEventArgs e)
        //{
        //    QueryByBoundsParameters para = new QueryByBoundsParameters
        //    {
        //        Bounds = bounds,
        //        ReturnContent = true,
        //        QueryParameters = new QueryParameterSet
        //        {
        //            QueryOption = QueryOption.ATTRIBUTEANDGEOMETRY,
        //            ExpectCount = 1,
        //            QueryParams = new List<QueryParameter>()
        //            {
        //                new QueryParameter
        //                {
        //                    Name="Capitals@World.1"
        //                }
        //            }
        //        }
        //    };
        //    try
        //    {
        //        QueryByBoundsService service = new QueryByBoundsService(url);
        //        var result = await service.ProcessAsync(para);
        //        if (result == null)
        //        {
        //            MessageBox.Show("返回结果没空");
        //            return;
        //        }
        //        if (_fLayer.Features.Count > 0)
        //        {
        //            _fLayer.ClearFeatures();
        //        }
        //        foreach (Recordset recordset in result.Recordsets)
        //        {
        //            foreach (Feature f in recordset.Features)
        //            {
        //                _fLayer.AddFeature(f);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}
    }
}