using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;

namespace SuperMap.WinRT.REST.Resources
{
    public class ExceptionStrings
    {
        private static ResourceMap resourceStringMap = ResourceManager.Current.MainResourceMap.GetSubtree("SuperMap.WinRT.REST/ExceptionStrings");

        /// <summary>
        ///   查找类似 参数类型错误 的本地化字符串。
        /// </summary>
        public static string ArgumentError
        {
            get
            {
                return resourceStringMap.GetValue("ArgumentError").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 参数为空 的本地化字符串。
        /// </summary>
        public static string ArgumentIsNull
        {
            get
            {
                return resourceStringMap.GetValue("ArgumentIsNull").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 QueryResultID、HighlightTargetSetID必选其一 的本地化字符串。
        /// </summary>
        public static string InvalidArgument
        {
            get
            {
                return resourceStringMap.GetValue("InvalidArgument").ValueAsString;
            }
        }

        /// <summary>
        ///   查找类似 Url不可用或者无效 的本地化字符串。
        /// </summary>
        public static string InvalidUrl
        {
            get
            {
                return resourceStringMap.GetValue("InvalidUrl").ValueAsString;
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
        ///   查找类似 服务地址错误 的本地化字符串。
        /// </summary>
        public static string ServiceAddressError
        {
            get
            {
                return resourceStringMap.GetValue("ServiceAddressError").ValueAsString;
            }
        }
    }
}
