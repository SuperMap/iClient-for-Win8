using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_UGCThemeLayer_Title}</summary>
    public class UGCThemeLayer : UGCLayer
    {
        /// <summary>${REST_UGCThemeLayer_constructor_D}</summary>
        public UGCThemeLayer() { }
        /// <summary>${REST_UGCThemeLayer_attribute_Theme_D}</summary>
        public Theme Theme{ get; set; }
        /// <summary>${REST_UGCThemeLayer_attribute_ThemeType_D}</summary>
        public ThemeType ThemeType{get;set;}
    }
}
