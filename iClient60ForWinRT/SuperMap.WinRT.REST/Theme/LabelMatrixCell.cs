using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_LabelMatrixCell_Title}</para>
    /// 	<para>${REST_LabelMatrixCell_Description}</para>
    /// </summary>
    public class LabelMatrixCell
    {
        /// <summary>${REST_LabelMatrixCell_constructor_D}</summary>
        public LabelMatrixCell() { }
        /// <summary>${REST_LabelMatrixCell_attribute_Type_D}</summary>
        internal LabelMatrixCellType Type { get; set; }
    }
}
