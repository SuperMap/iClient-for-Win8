using System.Collections.Generic;

namespace SuperMap.WinRT.OGC
{
    /// <summary>${mapping_GetWMTSCapabilitiesResult_Tile}</summary>
    public class GetWMTSCapabilitiesResult
    {
        internal GetWMTSCapabilitiesResult()
        {
            LayerInfos = new List<WMTSLayerInfo>();
            WMTSTileMatrixSetInfo = new List<WMTSTileMatrixSetInfo>();
        }
       
        /// <summary>${mapping_GetWMTSCapabilitiesResult_attribute_Version_D}</summary>
        public string Version { get; internal set; }
        
        /// <summary>${mapping_GetWMTSCapabilitiesResult_attribute_WMTSTileMatrixSetInfo_D}</summary>
        public List<WMTSTileMatrixSetInfo> WMTSTileMatrixSetInfo { get; internal set; }

        /// <summary>${mapping_GetWMTSCapabilitiesResult_attribute_LayerInfos_D}</summary>
        public List<WMTSLayerInfo> LayerInfos { get; internal set; }
    }
}
