

using System.Collections.Generic;
using SuperMap.WindowsPhone.Core;
using Newtonsoft.Json;
using SuperMap.WindowsPhone.REST.Help;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesByGeometryParameters_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesByGeometryParameters_Description}</para>
    /// </summary>
    public class GetFeaturesByGeometryParameters : GetFeaturesParametersBase
    {
        /// <summary>${WP_REST_GetFeaturesByGeometryParameters_constructor_D}</summary>
        public GetFeaturesByGeometryParameters()
        {
            GetFeatureMode = GetFeatureMode.SPATIAL;
        }
        /// <summary>${WP_REST_GetFeaturesByGeometryParameters_attribute_Geometry_D}</summary>
        [JsonProperty("geometry")]
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }
        /// <summary>${WP_REST_GetFeaturesByGeometryParameters_attribute_SpatialQueryMode_D}</summary>
        [JsonProperty("spatialQueryMode")]
        public SpatialQueryMode SpatialQueryMode { get; set; }
        /// <summary>${WP_REST_GetFeaturesByGeometryParameters_attribute_AttributeFilter_D}</summary>
        [JsonProperty("attributeFilter")]
        public string AttributeFilter { get; set; }
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_attribute_Fields_D}</summary>
        [JsonProperty("queryParameter")]
        public QueryParameter QueryParameter { get; set; }

    }
}
