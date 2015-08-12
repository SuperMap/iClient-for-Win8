using System.Collections.Generic;
using SuperMap.WinRT.Core;

namespace SuperMap.WinRT.OGC
{
    /// <summary>${mapping_WMSLayerInfo_Tile}</summary>
    public class WMSLayerInfo
    {
        /// <summary>${mapping_WMSLayerInfo_attribute_Abstract_D}</summary>
        public string Abstract { get; internal set; }
        /// <summary>${mapping_WMSLayerInfo_attribute_SubLayers_D}</summary>
        public IList<WMSLayerInfo> SubLayers { get; internal set; }
        /// <summary>${mapping_WMSLayerInfo_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; internal set; }
        /// <summary>${mapping_WMSLayerInfo_attribute_Name_D}</summary>
        public string Name { get; internal set; }
        /// <summary>${mapping_WMSLayerInfo_attribute_Title_D}</summary>
        public string Title { get; internal set; }
    }
}
