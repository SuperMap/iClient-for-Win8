using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.Controls.Resources
{
    //这个类不需要提取，需要在Document X！注释掉
    //类只能用public关键字，其他的关键字会报错。
    public class ResourceClass
    {
        public ResourceClass()
        {
            FeatureDataGrid_Clear = Resource.FeatureDataGrid_Clear;
            FeatureDataGrid_FirstRcd = Resource.FeatureDataGrid_FirstRcd;
            FeatureDataGrid_LastRcd = Resource.FeatureDataGrid_LastRcd;
            FeatureDataGrid_NextRcd = Resource.FeatureDataGrid_NextRcd;
            FeatureDataGrid_Opt = Resource.FeatureDataGrid_Opt;
            FeatureDataGrid_PreviousRcd = Resource.FeatureDataGrid_PreviousRcd;
            FeatureDataGrid_Rcd = Resource.FeatureDataGrid_Rcd;
            FeatureDataGrid_RecordMsg = Resource.FeatureDataGrid_RecordMsg;
            FeatureDataGrid_RemoveSlt = Resource.FeatureDataGrid_RemoveSlt;
            FeatureDataGrid_SltAll = Resource.FeatureDataGrid_SltAll;
            FeatureDataGrid_SwitchSlt = Resource.FeatureDataGrid_SwitchSlt;
            FeatureDataGrid_ZoomFree = Resource.FeatureDataGrid_ZoomFree;
            FeatureDataGrid_ZoomToSlt = Resource.FeatureDataGrid_ZoomToSlt;
            FeatureDataPager_Total = Resource.FeatureDataPager_Total;
            MapHistoryManager_PreView = Resource.MapHistoryManager_PreView;
            MapHistoryManager_NextView = Resource.MapHistoryManager_NextView;
        }

        public string FeatureDataGrid_Clear { get; set; }
        public string FeatureDataGrid_FirstRcd { get; set; }
        public string FeatureDataGrid_LastRcd { get; set; }
        public string FeatureDataGrid_NextRcd { get; set; }
        public string FeatureDataGrid_Opt { get; set; }
        public string FeatureDataGrid_PreviousRcd { get; set; }
        public string FeatureDataGrid_Rcd { get; set; }
        public string FeatureDataGrid_RecordMsg { get; set; }
        public string FeatureDataGrid_RemoveSlt { get; set; }
        public string FeatureDataGrid_SltAll { get; set; }
        public string FeatureDataGrid_SwitchSlt { get; set; }
        public string FeatureDataGrid_ZoomFree { get; set; }
        public string FeatureDataGrid_ZoomToSlt { get; set; }
        public string FeatureDataPager_Total { get; set; }
        public string MapHistoryManager_PreView { get; set; }
        public string MapHistoryManager_NextView { get; set; }
    }
}
