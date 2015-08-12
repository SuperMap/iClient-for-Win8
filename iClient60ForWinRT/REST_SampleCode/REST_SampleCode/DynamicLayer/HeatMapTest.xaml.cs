using System;
using System.Windows;
using System.Windows.Controls;
using SuperMap.Web.Mapping;
using SuperMap.Web.iServerJava6R;
using System.Collections.Generic;
using SuperMap.Web.Service;

namespace iServerJava6R_SampleCode
{
    public partial class HeatMapTest : Page
    {
        private const string url = "http://localhost:8090/iserver/services/components-rest/rest/maps/世界地图";

        HeatMapLayer layer;

        HeatPointCollection hpCollection;

        HeatPointCollection hpCollectionGeoRadius;
        public HeatMapTest()
        {
            InitializeComponent();

            layer = MyMap.Layers["heatMap"] as HeatMapLayer;
            this.Loaded += new RoutedEventHandler(HeatMapTest_Loaded);
        }

        //加载热点图
        private void HeatMapTest_Loaded(object sender, RoutedEventArgs e)
        {
            QueryByBoundsParameters parameter = new QueryByBoundsParameters
            {
                FilterParameters = new List<FilterParameter>() 
                {             
                        new FilterParameter()
                        {
                            Name = "Capitals@World",                          
                        }
                },
                Bounds = new SuperMap.Web.Core.Rectangle2D(-180, -90, 180, 90)
            };

            QueryByBoundsService service = new QueryByBoundsService(url);
            service.ProcessAsync(parameter);
            service.ProcessCompleted += new EventHandler<QueryEventArgs>(service_ProcessCompleted);
            service.Failed += new EventHandler<ServiceFailedEventArgs>(service_Failed);
        }

        private void service_Failed(object sender, ServiceFailedEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorMsg);
        }

        //将查询结果加入HeatMapLayer图层中
        private void service_ProcessCompleted(object sender, QueryEventArgs e)
        {
            if (e.Result == null)
            {
                return;
            }

            hpCollection = new HeatPointCollection();
            hpCollectionGeoRadius = new HeatPointCollection();
            if (e.Result.Recordsets != null)
            {
                foreach (Recordset item in e.Result.Recordsets)
                {
                    foreach (var item1 in item.Features)
                    {
                        HeatPoint heatPoint = new HeatPoint()
                        {
                            X = item1.Geometry.Bounds.Center.X,
                            Y = item1.Geometry.Bounds.Center.Y,
                            Value = 10,
                        };
                        HeatPoint heatPointGeoRadius = new HeatPoint()
                        {
                            X = item1.Geometry.Bounds.Center.X,
                            Y = item1.Geometry.Bounds.Center.Y,
                            Value = 10,
                            GeoRadius = 3

                        };
                        hpCollectionGeoRadius.Add(heatPointGeoRadius);
                        hpCollection.Add(heatPoint);
                    }
                }
            }
            layer.HeatPoints = hpCollection;
            layer.Radius = (int)sliderRadius.Value;
            layer.Intensity = sliderIntensity.Value;
        }

        //半径选择滑动条触发事件
        private void sliderRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MyMap == null) return;
            layer.Radius = (int)e.NewValue;
        }

        //强度选择滑动条触发事件
        private void sliderIntensity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MyMap == null) return;
            layer.Intensity = (double)e.NewValue;
        }

        //以地理半径显示热点图
        private void checkGeoRadius_Click(object sender, RoutedEventArgs e)
        {
            if (checkGeoRadius.IsChecked == true)
            {
                layer.HeatPoints = hpCollectionGeoRadius;
            }
            else
            {
                layer.HeatPoints = hpCollection;
            }
        }
    }
}
