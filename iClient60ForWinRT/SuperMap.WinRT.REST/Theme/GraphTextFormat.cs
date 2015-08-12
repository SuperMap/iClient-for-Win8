using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeGraphTextFormat_Title}</para>
    /// 	<para>${REST_ThemeGraphTextFormat_Description}</para>
    /// </summary>
    public enum GraphTextFormat
    {
        /// <summary>${REST_ThemeGraphTextFormat_attribute_Caption_D}</summary>
        CAPTION,          //标题。
        /// <summary>${REST_ThemeGraphTextFormat_attribute_CaptionPercent_D}</summary>
        CAPTION_PERCENT,          //标题 + 百分数。 
        /// <summary>${REST_ThemeGraphTextFormat_attribute_CaptionValue_D}</summary>
        CAPTION_VALUE,          //标题 + 实际数值。 
        /// <summary>${REST_ThemeGraphTextFormat_attribute_Percent_D}</summary>
        PERCENT,          //百分数。 
        /// <summary>${REST_ThemeGraphTextFormat_attribute_Value_D}</summary>
        VALUE,          //实际数值。 
    }
}
