using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SuperMap.Web.Actions;
using SuperMap.Web.Core;
using SuperMap.Web.iServerJava6R;
using SuperMap.Web.iServerJava6R.Data;
using SuperMap.Web.Mapping;
using SuperMap.Web.Service;

namespace iServerJava6R_SampleCode
{
    public partial class EditFeatures : Page
    {
        private FeaturesLayer drawLayer;
        private FeaturesLayer tempLayer;
        private List<int> ids = new List<int>();
        private List<int> featureIDs;
        private FeatureCollection features;
        private TiledDynamicRESTLayer layer;
        private Feature tempFeature;
        public int SmID;
        private string url1 = "http://localhost:8090/iserver/services/data-jingjin/rest/data/featureResults";
        private string url2 = "http://localhost:8090/iserver/services/data-jingjin/rest/data/datasources/name/Jingjin/datasets/name/Landuse_R/features";

        //该类与"Landuse"数据集的属性数据字段对应，用于记录属性值，并实现与DataGrid绑定
        public class LanduseData
        {
            public string SmID { get; set; }
            public string SmUserID { get; set; }
            public string SmArea { get; set; }
            public string SmPerimeter { get; set; }
            public string LandType { get; set; }
        }

        public EditFeatures()
        {
            InitializeComponent();
            drawLayer = MyMap.Layers["DrawLayer"] as FeaturesLayer;
            tempLayer = MyMap.Layers["TempLayer"] as FeaturesLayer;
            layer = MyMap.Layers["MyLayer"] as TiledDynamicRESTLayer;
            features = new FeatureCollection();
            featureIDs = new List<int>();
        }

        #region 选择地物
        //选择地物按钮触发事件
        private void SelectFeature_Click(object sender, RoutedEventArgs e)
        {
            DrawPoint selectfeature = new DrawPoint(MyMap, Cursor);
            MyMap.Action = selectfeature;
            selectfeature.DrawCompleted += new EventHandler<DrawEventArgs>(selectfeature_DrawCompleted);
        }

        private void selectfeature_DrawCompleted(object sender, DrawEventArgs e)
        {
            EditFeature.IsEnabled = true;
            DeleteFeature.IsEnabled = true;

            //查询当前鼠标点击目标
            queryFeature(e.Geometry);
        }

        //对目标地物进行查询
        private void queryFeature(SuperMap.Web.Core.Geometry geo)
        {
            GetFeaturesByGeometryParameters param = new GetFeaturesByGeometryParameters
            {
                DatasetNames = new string[] { "Jingjin:Landuse_R" },
                SpatialQueryMode = SpatialQueryMode.INTERSECT,
                Geometry = geo,
            };
            GetFeaturesByGeometryService query = new GetFeaturesByGeometryService(url1);
            query.ProcessAsync(param);
            query.ProcessCompleted += new EventHandler<GetFeaturesEventArgs>(query_ProcessCompleted);
            query.Failed += new EventHandler<ServiceFailedEventArgs>(query_Failed);
        }

        //查询失败
        private void query_Failed(object sender, ServiceFailedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }

        //查询成功高亮显示选中地物，并显示相关属性信息
        private void query_ProcessCompleted(object sender, GetFeaturesEventArgs e)
        {
            //点击处没有地物的情况
            if (e.Result == null)
            {
                MessageBox.Show("查询结果为空");
                return;
            }
            //判断是否存在当前选择
            if (featureIDs.Contains(Convert.ToInt32(e.Result.Features[0].Attributes["SMID"].ToString())))
            {
                return;
            }
            featureIDs.Add(Convert.ToInt32(e.Result.Features[0].Attributes["SMID"].ToString()));
            //点击处存在地物时，用蓝色边显示该地物
            drawLayer.AddFeatureSet(e.Result.Features);
        }
        #endregion

        #region 编辑地物
        //选择地物
        private void EditFeature_Click(object sender, RoutedEventArgs e)
        {
            //编辑地物形状
            Edit editfeature = new Edit(MyMap, drawLayer);
            MyMap.Action = editfeature;
            editfeature.GeometryEdit += new EventHandler<Edit.GeometryEditEventArgs>(editfeature_GeometryEdit);
        }

        //更新地物形状
        private void editfeature_GeometryEdit(object sender, Edit.GeometryEditEventArgs e)
        {
            if (e.Action == SuperMap.Web.Actions.Edit.GeometryEditAction.EditCompleted)
            {
                features.Clear();
                if (tempFeature == null)
                {
                    tempFeature = new Feature();
                }
                tempFeature.Geometry = e.Feature.Geometry;
                SmID = Convert.ToInt32(e.Feature.Attributes["SMID"].ToString());
                features.Add(tempFeature);

                EditFeaturesParameters updateParameters = new EditFeaturesParameters
                {
                    EditType = EditType.UPDATA,
                    Features = features,
                    IDs = new List<int>() { SmID },
                };

                EditFeaturesService editService = new EditFeaturesService(url2);
                editService.ProcessAsync(updateParameters);
                editService.ProcessCompleted += new EventHandler<EditFeaturesEventArgs>(updateService_ProcessCompleted);
                editService.Failed += new EventHandler<ServiceFailedEventArgs>(updateService_Failed);
            }
        }

        private void updateService_Failed(object sender, ServiceFailedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }

        //编辑地物形状成功
        private void updateService_ProcessCompleted(object sender, EditFeaturesEventArgs e)
        {
            if (e.Result.Succeed)
            {
                drawLayer.ClearFeatures();
                layer.Refresh();
                featureIDs.Clear();
                MessageBox.Show("编辑地物形状成功");
            }

            EditFeature.IsEnabled = false;
            DeleteFeature.IsEnabled = false;
        }

        private void MyDataGrid_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
            EditFeaturesParameters updateParameters = new EditFeaturesParameters
            {
                EditType = EditType.UPDATA,
                Features = new FeatureCollection { MyDataGrid.SelectedFeatures[0] },
                IDs = new List<int> { Convert.ToInt32(MyDataGrid.SelectedFeatures[0].Attributes["SMID"].ToString()) }
            };

            EditFeaturesService updateByAttributesService = new EditFeaturesService(url2);
            updateByAttributesService.ProcessAsync(updateParameters);
            updateByAttributesService.ProcessCompleted += new EventHandler<EditFeaturesEventArgs>(updateByAttributesService_ProcessCompleted);
            updateByAttributesService.Failed += new EventHandler<ServiceFailedEventArgs>(updateByAttributesService_Failed);
        }

        private void updateByAttributesService_Failed(object sender, ServiceFailedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }

        //编辑地物属性成功
        private void updateByAttributesService_ProcessCompleted(object sender, EditFeaturesEventArgs e)
        {
            if (e.Result.Succeed)
            {
                layer.Refresh();
                MessageBox.Show("编辑地物属性成功");
            }
        }
        #endregion

        #region 添加地物
        //在图层中添加新地物
        private void AddFeature_Click(object sender, RoutedEventArgs e)
        {
            DrawPolygon polygon = new DrawPolygon(MyMap);
            MyMap.Action = polygon;
            polygon.DrawCompleted += new EventHandler<DrawEventArgs>(polygon_DrawCompleted);
            tempLayer.ClearFeatures();
        }

        private void polygon_DrawCompleted(object sender, DrawEventArgs e)
        {
            //将绘制的地物显示在TempLayer中
            Feature f = new Feature()
            {
                Geometry = e.Geometry,
                Style = new PredefinedFillStyle()
                {
                    Fill = new SolidColorBrush(Colors.Green)
                },
            };

            tempLayer.AddFeature(f);

            EditFeaturesParameters param = new EditFeaturesParameters
            {
                EditType = EditType.ADD,
                Features = new FeatureCollection() { f }
            };

            //与服务器交互
            EditFeaturesService editService = new EditFeaturesService(url2);
            editService.ProcessAsync(param);
            editService.Failed += new EventHandler<ServiceFailedEventArgs>(editService_Failed);
            editService.ProcessCompleted += new EventHandler<EditFeaturesEventArgs>(editService_ProcessCompleted);
        }

        //与服务器交互成功
        private void editService_ProcessCompleted(object sender, EditFeaturesEventArgs e)
        {
            if (e.Result.Succeed)
            {
                MessageBox.Show("添加地物成功");
                tempLayer.ClearFeatures();
                drawLayer.ClearFeatures();
                featureIDs.Clear();
                layer.Refresh();
            }
        }

        //与服务器交互失败提示失败信息
        private void editService_Failed(object sender, ServiceFailedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }
        #endregion

        #region 删除地物
        //删除选中地物
        private void DeleteFeature_Click(object sender, RoutedEventArgs e)
        {

            EditFeaturesParameters param = new EditFeaturesParameters
            {
                EditType = EditType.DELETE,
                IDs = featureIDs,
            };
            EditFeaturesService deleteService = new EditFeaturesService(url2);
            deleteService.ProcessAsync(param);
            deleteService.ProcessCompleted += new EventHandler<EditFeaturesEventArgs>(deleteService_ProcessCompleted);
            deleteService.Failed += new EventHandler<ServiceFailedEventArgs>(deleteService_Failed);
        }

        private void deleteService_Failed(object sender, ServiceFailedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }

        //删除地物成功
        private void deleteService_ProcessCompleted(object sender, EditFeaturesEventArgs e)
        {
            if (e.Result.Succeed)
            {
                MessageBox.Show("删除地物成功");
                layer.Refresh();
                drawLayer.ClearFeatures();
            }

            EditFeature.IsEnabled = false;
            DeleteFeature.IsEnabled = false;
            featureIDs.Clear();
        }
        #endregion

        #region 其他工具条
        //平移
        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            Pan p = new Pan(MyMap, Cursors.Hand);
            MyMap.Action = p;
        }

        //清除高亮
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            drawLayer.ClearFeatures();
            featureIDs.Clear();
            this.EditFeature.IsEnabled = false;
            this.DeleteFeature.IsEnabled = false;
        }
        #endregion
    }
}
