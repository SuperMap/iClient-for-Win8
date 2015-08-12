using System.Collections.Generic;
using SuperMap.WindowsPhone.Core;
using Newtonsoft.Json;
using SuperMap.WindowsPhone.REST.Help;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesByBufferParameters_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesByBufferParameters_Description}</para>
    /// </summary>
    public class GetFeaturesByBufferParameters : GetFeaturesParametersBase
    {
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_constructor_D}</summary>
        public GetFeaturesByBufferParameters()
        {
            GetFeatureMode = GetFeatureMode.BUFFER;
        }
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_attribute_Geometry_D}</summary>
        [JsonProperty("geometry")]
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_attribute_BufferDistance_D}</summary>
        [JsonProperty("bufferDistance")]
        public double BufferDistance { get; set; }
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_attribute_AttributeFilter_D}</summary>
        [JsonProperty("attributeFilter")]
        public string AttributeFilter { get; set; }
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_attribute_Fields_D}</summary>
        [JsonProperty("queryParameter")]
        public QueryParameter QueryParameter { get; set; }

    }
}
