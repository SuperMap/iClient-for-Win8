using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.UI;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_UGCGridLayer_Title}</summary>
    public class UGCGridLayer : UGCLayer
    {
        /// <summary>${REST_UGCGridLayer_constructor_D}</summary>
        public UGCGridLayer() { }
        /// <summary>${REST_UGCGridLayer_attribute_Colors_D}</summary>
        public IList<Color> Colors { get; set; }//           颜色表对象。
        /// <summary>${REST_UGCGridLayer_attribute_DashStyle_D}</summary>
        public ServerStyle DashStyle { get; set; }//           格网虚线的样式。
        /// <summary>${REST_UGCGridLayer_attribute_GridType_D}</summary>
        public GridType GridType { get; set; }//           格网类型。 
        /// <summary>${REST_UGCGridLayer_attribute_HorizontalSpacing_D}</summary>
        public double HorizontalSpacing { get; set; }//           格网水平间隔大小。
        /// <summary>${REST_UGCGridLayer_attribute_SizeFixed_D}</summary>
        public bool SizeFixed { get; set; }//           格网是否固定大小，如果不固定大小，则格网随着地图缩放。 
        /// <summary>${REST_UGCGridLayer_attribute_SolidStyle_D}</summary>
        public ServerStyle SolidStyle { get; set; }//           格网实线的样式。
        /// <summary>${REST_UGCGridLayer_attribute_SpecialColor_D}</summary>
        public Color SpecialColor { get; set; }//           栅格数据集无值数据的颜色。
        /// <summary>${REST_UGCGridLayer_attribute_SpecialValue_D}</summary>
        public double SpecialValue { get; set; }//           图层的特殊值。 
        /// <summary>${REST_UGCGridLayer_attribute_VerticalSpacing_D}</summary>
        public double VerticalSpacing { get; set; }//           格网垂直间隔大小。 
    }
}
