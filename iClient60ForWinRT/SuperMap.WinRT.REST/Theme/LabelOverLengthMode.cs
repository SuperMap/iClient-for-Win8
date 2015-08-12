using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_LabelOverLengthMode_Title}</para>
    /// 	<para>${REST_LabelOverLengthMode_Description}</para>
    /// </summary>
    public enum LabelOverLengthMode
    {
        /// <summary>${REST_LabelOverLengthMode_attribute_NEWLINE_D}</summary>
        NEWLINE,          //换行显示。
        /// <summary>${REST_LabelOverLengthMode_attribute_NONE_D}</summary>
        NONE,          //对超长标签不进行处理。
        /// <summary>${REST_LabelOverLengthMode_attribute_OMIT_D}</summary>
        OMIT,          //省略超出部分。
    }
}
