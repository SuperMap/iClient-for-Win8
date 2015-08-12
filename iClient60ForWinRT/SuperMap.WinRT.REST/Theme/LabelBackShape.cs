using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_LabelBackShape_Title}</para>
    /// 	<para>${REST_LabelBackShape_Description}</para>
    /// </summary>
    public enum LabelBackShape
    {
        /// <summary>${REST_LabelBackShape_attribute_NONE_D}</summary>
        NONE,           //空背景。
        /// <summary>${REST_LabelBackShape_attribute_DIAMOND_D}</summary>
        DIAMOND,           //菱形背景。
        /// <summary>${REST_LabelBackShape_attribute_ELLIPSE_D}</summary>
        ELLIPSE,           //椭圆形背景。
        /// <summary>${REST_LabelBackShape_attribute_MARKER_D}</summary>
        MARKER,           //符号背景。
        /// <summary>${REST_LabelBackShape_attribute_RECT_D}</summary>
        RECT,           //矩形背景。
        /// <summary>${REST_LabelBackShape_attribute_ROUNDRECT_D}</summary>
        ROUNDRECT,           //圆角矩形背景。
        /// <summary>${REST_LabelBackShape_attribute_TRIANGLE_D}</summary>
        TRIANGLE,           //三角形背景。
    }
}
