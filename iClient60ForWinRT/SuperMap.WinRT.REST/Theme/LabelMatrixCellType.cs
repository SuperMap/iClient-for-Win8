using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_LabelMatrixCellType_Title}</para>
    /// 	<para>${REST_LabelMatrixCellType_Description}</para>
    /// </summary>
    enum LabelMatrixCellType
    {
        /// <summary>${REST_LabelMatrixCellType_attribute_IMAGE_D}</summary>
        IMAGE,          //图片类型的矩阵标签元素
        /// <summary>${REST_LabelMatrixCellType_attribute_SYMBOL_D}</summary>
        SYMBOL,          //符号类型的矩阵标签元素
        /// <summary>${REST_LabelMatrixCellType_attribute_THEME_D}</summary>
        THEME,          //专题图类型的矩阵标签元素 
    }
}
