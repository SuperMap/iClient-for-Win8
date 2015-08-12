using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;

namespace SuperMap.WinRT.Resources
{
    public class ExceptionStrings
    {
        private static ResourceMap resourceStringMap = ResourceManager.Current.MainResourceMap.GetSubtree("SuperMap.WinRT/ExceptionStrings");

        public static string String1
        {
            get
            {
                return resourceStringMap.GetValue("String1").ValueAsString;
            }
        }
        /// <summary>
        ///   查找类似 该元素附加项属性ElementsLayer.BBox没有设置 的本地化字符串。
        /// </summary>
        public static string BboxIsNotSet
        {
            get
            {
                return resourceStringMap.GetValue("BboxIsNotSet").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 不能将Feature加到多个FeaturesLayer上 的本地化字符串。
        /// </summary>
        public static string CanntAddToMFLayer
        {
            get
            {
                return resourceStringMap.GetValue("CanntAddToMFLayer").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 CRS为空 的本地化字符串。
        /// </summary>
        public static string CRSIsNull
        {
            get
            {
                return resourceStringMap.GetValue("CRSIsNull").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 目标类型为空 的本地化字符串。
        /// </summary>
        public static string DestinationTypeIsNull
        {
            get
            {
                return resourceStringMap.GetValue("DestinationTypeIsNull").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 ElementsLayer单独使用时无法添加ShapeBase。 的本地化字符串。
        /// </summary>
        public static string ElementsLayerIsAlone
        {
            get
            {
                return resourceStringMap.GetValue("ElementsLayerIsAlone").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 高度将小于0 的本地化字符串。
        /// </summary>
        public static string HeightLessThanZero
        {
            get
            {
                return resourceStringMap.GetValue("HeightLessThanZero").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 Map的尺度设置与TiledLayer尺度设置不匹配 的本地化字符串。
        /// </summary>
        public static string InvalidMatch
        {
            get
            {
                return resourceStringMap.GetValue("InvalidMatch").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 MultiplicationConverter不支持该类型 {0} 的本地化字符串。
        /// </summary>
        public static string InvalidMultiplicationConverter
        {
            get
            {
                return resourceStringMap.GetValue("InvalidMultiplicationConverter").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 分辨率数组不能设置为空 的本地化字符串。
        /// </summary>
        public static string InvalidResolutions
        {
            get
            {
                return resourceStringMap.GetValue("InvalidResolutions").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 比例尺数组不能设置为空 的本地化字符串。
        /// </summary>
        public static string InvalidScales
        {
            get
            {
                return resourceStringMap.GetValue("InvalidScales").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 无法转换为String Array 的本地化字符串。
        /// </summary>
        public static string InvalidStringArray
        {
            get
            {
                return resourceStringMap.GetValue("InvalidStringArray").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 暂不支持该Geometry类型 的本地化字符串。
        /// </summary>
        public static string InvalidSupportGeometry
        {
            get
            {
                return resourceStringMap.GetValue("InvalidSupportGeometry").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 无法成功转换为CoordinateReferenceSystem 的本地化字符串。
        /// </summary>
        public static string InvalidToCRS
        {
            get
            {
                return resourceStringMap.GetValue("InvalidToCRS").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 无法成功转换为DoubleArray 的本地化字符串。
        /// </summary>
        public static string InvalidToDoubleArray
        {
            get
            {
                return resourceStringMap.GetValue("InvalidToDoubleArray").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 无效的URI模式宿主 的本地化字符串。
        /// </summary>
        public static string InvalidURISchemeHost
        {
            get
            {
                return resourceStringMap.GetValue("InvalidURISchemeHost").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 强烈程度的范围大于1或者小于0 的本地化字符串。
        /// </summary>
        public static string IvalidIntensity
        {
            get
            {
                return resourceStringMap.GetValue("IvalidIntensity").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 该Layer已经隶属于其他Map 的本地化字符串。
        /// </summary>
        public static string LayerToMap
        {
            get
            {
                return resourceStringMap.GetValue("LayerToMap").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 请求繁忙 的本地化字符串。
        /// </summary>
        public static string OperationIsBusy
        {
            get
            {
                return resourceStringMap.GetValue("OperationIsBusy").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 参数错误 的本地化字符串。
        /// </summary>
        public static string ParametersError
        {
            get
            {
                return resourceStringMap.GetValue("ParametersError").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 文本解析失败 的本地化字符串。
        /// </summary>
        public static string ParseFailed
        {
            get
            {
                return resourceStringMap.GetValue("ParseFailed").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 Point2Ds为空 的本地化字符串。
        /// </summary>
        public static string Point2DsIsNull
        {
            get
            {
                return resourceStringMap.GetValue("Point2DsIsNull").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 请确认Web服务器根目录下存在跨域策略文件，服务地址等参数是否正确。 的本地化字符串。
        /// </summary>
        public static string PolicyFileAvailable
        {
            get
            {
                return resourceStringMap.GetValue("PolicyFileAvailable").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 半径范围小于1 的本地化字符串。
        /// </summary>
        public static string RadiusLessThanOne
        {
            get
            {
                return resourceStringMap.GetValue("RadiusLessThanOne").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 right必须大于left 的本地化字符串。
        /// </summary>
        public static string RightLessThanLeft
        {
            get
            {
                return resourceStringMap.GetValue("RightLessThanLeft").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 RootElement为空 的本地化字符串。
        /// </summary>
        public static string RootElementIsNull
        {
            get
            {
                return resourceStringMap.GetValue("RootElementIsNull").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 必须以HTTP(S)模式启动应用程序，请确认把您的网站设为启动项目。 的本地化字符串。
        /// </summary>
        public static string SchemeError
        {
            get
            {
                return resourceStringMap.GetValue("SchemeError").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 服务地址错误 的本地化字符串。
        /// </summary>
        public static string ServiceAddressError
        {
            get
            {
                return resourceStringMap.GetValue("ServiceAddressError").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 请确认您的地图服务已经启动，地图名称等参数是否正确。 的本地化字符串。
        /// </summary>
        public static string ServiceStarted
        {
            get
            {
                return resourceStringMap.GetValue("ServiceStarted").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 坐标解析失败 的本地化字符串。
        /// </summary>
        public static string TextParseFailed
        {
            get
            {
                return resourceStringMap.GetValue("TextParseFailed").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 top必须大于bottom. 的本地化字符串。
        /// </summary>
        public static string TopLessThanBottom
        {
            get
            {
                return resourceStringMap.GetValue("TopLessThanBottom").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 宽度小于0 的本地化字符串。
        /// </summary>
        public static string WidthLessThanZero
        {
            get
            {
                return resourceStringMap.GetValue("WidthLessThanZero").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 数量小于1 的本地化字符串。
        /// </summary>
        public static string CountLessThanOne
        {
            get
            {
                return resourceStringMap.GetValue("CountLessThanOne").ValueAsString;
            }
        }
    }
}
