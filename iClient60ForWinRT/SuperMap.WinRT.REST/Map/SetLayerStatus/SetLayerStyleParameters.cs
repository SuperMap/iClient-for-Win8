using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${iServer6_SetLayerStyleParameters_Title}</para>
    /// </summary>
    public class SetLayerStyleParameters
    {
        /// <summary>${iServer6_SetLayerStyleParameters_constructor_D}</summary>
        public SetLayerStyleParameters()
        {
            Style = new ServerStyle();
            HoldTime = 10;
        }
        /// <summary>${iServer6_SetLayerStyleParameters_attribute_LayerName_D}</summary>
        public string LayerName { get; set; }
        /// <summary>${iServer6_SetLayerStyleParameters_attribute_Style_D}</summary>
        public ServerStyle Style { get; set; }
        /// <summary>${iServer6_SetLayerStyleParameters_attribute_ResourceID_D}</summary>
        public string ResourceID { get; set; }
        /// <summary>${iServer6_SetLayerStyleParameters_attribute_HoldTime_D}</summary>
        public int HoldTime { get; set; }
    }
}
