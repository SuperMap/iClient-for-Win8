using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_TextAlignment_Title}</para>
    /// 	<para>${REST_TextAlignment_Description}</para>
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_BASELINELEFT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentBaseLineLeft.bmp"/>
        /// 	</para>
        /// </summary>
        BASELINECENTER,
        //基准线居中对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_BASELINECENTER_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentBaseLineCenter.bmp"/>
        /// 	</para>
        /// </summary>
        BASELINELEFT,
        //基准线左对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_BASELINERIGHT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentBaseLineRight.bmp"/>
        /// 	</para>
        /// </summary>
        BASELINERIGHT,
        //基准线右对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_BOTTOMCENTER_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentBottomCenter.bmp"/>
        /// 	</para>
        /// </summary>
        BOTTOMCENTER,
        //底部居中对齐。 
        /// <summary>
        /// 	<para> ${REST_TextAlignment_attribute_BOTTOMLEFT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentBottomLeft.bmp"/>
        /// 	</para>
        /// </summary>
        BOTTOMLEFT,
        //左下角对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_BOTTOMRIGHT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentBottomRight.bmp"/>
        /// 	</para>
        /// </summary>
        BOTTOMRIGHT,
        //右下角对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_MIDDLECENTER_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentMiddleCenter.bmp"/>
        /// 	</para>
        /// </summary>
        MIDDLECENTER,
        //中心对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_MIDDLELEFT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentMiddleLeft.bmp"/>
        /// 	</para>
        /// </summary>
        MIDDLELEFT,
        //左中对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_MIDDLERIGHT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentMiddleRight.bmp"/>
        /// 	</para>
        /// </summary>
        MIDDLERIGHT,
        //右中对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_TOPCENTER_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentTopCenter.bmp"/>
        /// 	</para>
        /// </summary>
        TOPCENTER,
        //顶部居中对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_TOPLEFT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentTopLeft.bmp"/>
        /// 	</para>
        /// </summary>
        TOPLEFT,
        //左上角对齐。 
        /// <summary>
        /// 	<para>${REST_TextAlignment_attribute_TOPRIGHT_D}</para>
        /// 	<para>
        /// 		<img src="ServerTextAlignmentTopRight.bmp"/>
        /// 	</para>
        /// </summary>
        TOPRIGHT,
        //右上角对齐。 
    }
}
