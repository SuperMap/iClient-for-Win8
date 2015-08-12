

using System.Collections.Generic;
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesByGeometryParameters_Title}</para>
    /// 	<para>${REST_GetFeaturesByGeometryParameters_Description}</para>
    /// </summary>
    public class GetFeaturesByGeometryParameters : GetFeaturesParametersBase
    {
        /// <summary>${REST_GetFeaturesByGeometryParameters_constructor_D}</summary>
        public GetFeaturesByGeometryParameters()
        {
        }
        /// <summary>${REST_GetFeaturesByGeometryParameters_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }
        /// <summary>${REST_GetFeaturesByGeometryParameters_attribute_SpatialQueryMode_D}</summary>
        public SpatialQueryMode SpatialQueryMode { get; set; }
        /// <summary>${REST_GetFeaturesByGeometryParameters_attribute_AttributeFilter_D}</summary>
        public string AttributeFilter { get; set; }
        /// <summary>${REST_GetFeaturesByBufferParameters_attribute_Fields_D}</summary>
        public IList<string> Fields { get; set; }

    }
}
