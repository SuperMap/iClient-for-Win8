using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.UI;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_UGCImageLayer_Title}</summary>
    public class UGCImageLayer : UGCLayer
    {
        /// <summary>${REST_UGCImageLayer_constructor_D}</summary>
        public UGCImageLayer() { }
        /// <summary>${REST_UGCImageLayer_attribute_Brightness_D}</summary>
        public int Brightness { get; set; }//           影像图层的亮度。 
        /// <summary>${REST_UGCImageLayer_attribute_ColorSpaceType_D}</summary>
        public ColorSpaceType ColorSpaceType { get; set; }//           返回影像图层的色彩显示模式。
        /// <summary>${REST_UGCImageLayer_attribute_Contrast_D}</summary>
        public int Contrast { get; set; }//           影像图层的对比度。
        /// <summary>${REST_UGCImageLayer_attribute_DisplayBandIndexes_D}</summary>
        public IList<int> DisplayBandIndexes { get; set; }//           返回当前影像图层显示的波段索引。 
        /// <summary>${REST_UGCImageLayer_attribute_Transparent_D}</summary>
        public bool Transparent { get; set; }//           是否背景透明。
        /// <summary>${REST_UGCImageLayer_attribute_TransparentColor_D}</summary>
        public Color TransparentColor { get; set; }//           返回背景透明色。 
    }
}
