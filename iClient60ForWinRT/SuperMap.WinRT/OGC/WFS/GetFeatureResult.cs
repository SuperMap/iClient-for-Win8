using System.Collections.Generic;
using SuperMap.WinRT.Core;

namespace SuperMap.WinRT.OGC
{
    /// <summary>
    /// 	<para>${mapping_GetFeatureResult_Tile}</para>
    /// 	<para>${mapping_GetFeatureResult_Description}</para>
    /// </summary>
    public class GetWFSFeatureResult
    {
        /// <summary> ${mapping_GetFeatureResult_constructor_D} </summary>
        public GetWFSFeatureResult()
        {
            FeaturePair = new Dictionary<string, FeatureCollection>();
        }
        /// <summary> ${mapping_GetFeatureResult_attribute_Bounds_D} </summary>
        public Rectangle2D Bounds { get; internal set; }
        /// <summary> ${mapping_GetFeatureResult_attribute_FeaturePair_D} </summary>
        public Dictionary<string, FeatureCollection> FeaturePair { get; private set; }
    }
}
