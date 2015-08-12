using SuperMap.Web.Core;

namespace SuperMap.Web.OGC
{
    /// <summary> ${mapping_FeatureType_Tile} </summary>
    public class WFSFeatureType
    {
        /// <summary> ${mapping_FeatureType_constructor_D} </summary>
        public WFSFeatureType()
        { }
        /// <summary> ${mapping_FeatureType_attribute_Name_D} </summary>
        public string TypeName { get; internal set; }
        /// <summary> ${mapping_FeatureType_attribute_Title_D} </summary>
        public string Title { get; internal set; }
        /// <summary> ${mapping_FeatureType_attribute_SRS_D} </summary>
        public CoordinateReferenceSystem SRS { get; internal set; }
        /// <summary> ${mapping_FeatureType_attribute_Bounds_D} </summary>
        public Rectangle2D Bounds { get; internal set; }
    }
}
