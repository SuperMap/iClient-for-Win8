using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;

namespace SuperMap.WinRT.Controls.Resources
{
    internal class Resource
    {
        private static ResourceMap resourceStringMap = ResourceManager.Current.MainResourceMap.GetSubtree("SuperMap.WinRT.Controls/Resource");


        /// <summary>
        ///   查找类似 全部删除 的本地化字符串。
        /// </summary>
        internal static string Bookmark_Clear
        {
            get
            {
                return resourceStringMap.GetValue("Bookmark_Clear").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 删除 的本地化字符串。
        /// </summary>
        internal static string Bookmark_Remove
        {
            get
            {
                return resourceStringMap.GetValue("Bookmark_Remove").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 书签 的本地化字符串。
        /// </summary>
        internal static string Bookmark_Title
        {
            get
            {
                return resourceStringMap.GetValue("Bookmark_Title").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 当FeatureDataForm属性IsReadOnly为True时，不能更新 的本地化字符串。
        /// </summary>
        internal static string FeatureDataForm_Cannot_Update
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataForm_Cannot_Update").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 清除选择 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_Clear
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_Clear").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 第一条记录 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_FirstRcd
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_FirstRcd").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 索引必须大于等于0 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_Index_Range_Exception
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_Index_Range_Exception").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 最后一条记录 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_LastRcd
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_LastRcd").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 下一条记录 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_NextRcd
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_NextRcd").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 集合中无元素 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_No_Item_Exception
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_No_Item_Exception").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 不能更新 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_No_Update_Exception
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_No_Update_Exception").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似  选项... 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_Opt
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_Opt").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 上一条记录 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_PreviousRcd
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_PreviousRcd").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 记录： 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_Rcd
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_Rcd").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 共选中{1}条记录中的{0}条 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_RecordMsg
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_RecordMsg").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 删除所选项 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_RemoveSlt
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_RemoveSlt").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 更新要素失败 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_Row_Editing_Exception
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_Row_Editing_Exception").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 选中全部 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_SltAll
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_SltAll").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 反向选择 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_SwitchSlt
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_SwitchSlt").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 自动漫游到所选项 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_ZoomFree
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_ZoomFree").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 缩放到所选项 的本地化字符串。
        /// </summary>
        internal static string FeatureDataGrid_ZoomToSlt
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataGrid_ZoomToSlt").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 共： 的本地化字符串。
        /// </summary>
        internal static string FeatureDataPager_Total
        {
            get
            {
                return resourceStringMap.GetValue("FeatureDataPager_Total").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 map 参数不能为空 的本地化字符串。
        /// </summary>
        internal static string InfoWindow_MapParam
        {
            get
            {
                return resourceStringMap.GetValue("InfoWindow_MapParam").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 缩放到后一视图 的本地化字符串。
        /// </summary>
        internal static string MapHistoryManager_NextView
        {
            get
            {
                return resourceStringMap.GetValue("MapHistoryManager_NextView").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 缩放到前一视图 的本地化字符串。
        /// </summary>
        internal static string MapHistoryManager_PreView
        {
            get
            {
                return resourceStringMap.GetValue("MapHistoryManager_PreView").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 英尺 的本地化字符串。
        /// </summary>
        internal static string ScaleBar_Foot
        {
            get
            {
                return resourceStringMap.GetValue("ScaleBar_Foot").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 公里 的本地化字符串。
        /// </summary>
        internal static string ScaleBar_Kilometer
        {
            get
            {
                return resourceStringMap.GetValue("ScaleBar_Kilometer").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 米 的本地化字符串。
        /// </summary>
        internal static string ScaleBar_Meter
        {
            get
            {
                return resourceStringMap.GetValue("ScaleBar_Meter").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 英里 的本地化字符串。
        /// </summary>
        internal static string ScaleBar_Mile
        {
            get
            {
                return resourceStringMap.GetValue("ScaleBar_Mile").ValueAsString;
            }
        }
    }
}
