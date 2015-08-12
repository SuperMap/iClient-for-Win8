using Newtonsoft.Json;
using System.Collections.Generic;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesBySQLParameters_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesBySQLParameters_Description}</para>
    /// </summary>
    public class GetFeaturesBySQLParameters:GetFeaturesParametersBase
    {
        /// <summary>${WP_REST_GetFeaturesBySQLParameters_constructor_None_D}</summary>
        public GetFeaturesBySQLParameters()
        {
            GetFeatureMode = GetFeatureMode.SQL;
        }
        /// <summary>${WP_REST_GetFeaturesBySQLParameters_attribute_FilterParameter_D}</summary>
        [JsonProperty("queryParameter")]
        public QueryParameter QueryParameter { get; set; }

        /// <summary>${WP_REST_GetFeaturesParametersBase_attribute_MaxFeatures_D}</summary>
        [JsonProperty("maxFeatures")]
        public int MaxFeatures { get; set; }
    }
}
