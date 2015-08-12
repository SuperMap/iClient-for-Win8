using System.Windows;
using Microsoft.Phone.Controls;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.iServerJava6R;
using System.Collections.Generic;
using System;
using SuperMap.WindowsPhone.Core;

namespace Map_Query_QueryBySQLService
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string url = "http://support.supermap.com.cn:8090/iserver/services/map-world/rest/maps/World";
        private FeaturesLayer _fLayer;
        private ElementsLayer _eLayer;
        private TiledDynamicRESTLayer _DLayer;
        private Action _action;

        public MainPage()
        {
            InitializeComponent();
            _fLayer = MyMap.Layers["MyFeatureLayer"] as FeaturesLayer;
            _eLayer = MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            _DLayer = MyMap.Layers["TDRESTLayer"] as TiledDynamicRESTLayer;
            MyMap.Tap += MyMap_Tap;

        }

        void MyMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (_action != null)
            {
                _action.Tap(e);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            QueryBySQLParameters para = new QueryBySQLParameters
            {
                ReturnContent=true,
                QueryParameters = new QueryParameterSet
                {
                    QueryOption=QueryOption.ATTRIBUTEANDGEOMETRY,
                    QueryParams = new List<QueryParameter>()
                    {
                        new QueryParameter
                        {
                            Name="Countries@World",
                            AttributeFilter=MyTextBox.Text
                        }
                    }
                }
            };
            try
            {
                QueryBySQLService service = new QueryBySQLService(url);
                var result = await service.ProcessAsync(para);
                if(result==null)
                {
                    MessageBox.Show("返回结果没空");
                    return;
                }
                foreach(Recordset recordset in result.Recordsets)
                {
                    foreach(Feature f in recordset.Features )
                    {
                        _fLayer.AddFeature(f);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}