using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_AlongLineDirection_Title}</para>
    /// 	<para>${REST_AlongLineDirection_Description}</para>
    /// </summary>
    public enum AlongLineDirection
    {
        /// <summary>${REST_AlongLineDirection_attribute_NORMAL_D}</summary>
        ALONG_LINE_NORMAL,           //沿线的法线方向放置标签。
        /// <summary>${REST_AlongLineDirection_attribute_LB_TO_RT_D}</summary>
        LEFT_BOTTOM_TO_RIGHT_TOP,           //从上到下，从左到右放置。
        /// <summary>${REST_AlongLineDirection_attribute_LT_TO_RB_D}</summary>
        LEFT_TOP_TO_RIGHT_BOTTOM,           //从上到下，从右到左放置。
        /// <summary>${REST_AlongLineDirection_attribute_RB_TO_LT_D}</summary>
        RIGHT_BOTTOM_TO_LEFT_TOP,           //从下到上，从左到右放置。
        /// <summary>${REST_AlongLineDirection_attribute_RT_TO_LB_D}</summary>
        RIGHT_TOP_TO_LEFT_BOTTOM,           //从下到上，从右到左放置。
    }
}
