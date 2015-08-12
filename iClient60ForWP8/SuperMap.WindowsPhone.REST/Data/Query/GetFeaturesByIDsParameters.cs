

using System.Collections.Generic;
using SuperMap.WindowsPhone.Core;
using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesByIDsParameters_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesByIDsParameters_Description}</para>
    /// </summary>
    public class GetFeaturesByIDsParameters:GetFeaturesParametersBase
    {
        /// <summary>${WP_REST_GetFeaturesByGeometryParameters_constructor_D}</summary>
        public GetFeaturesByIDsParameters()
        {
            GetFeatureMode = GetFeatureMode.ID;
        }
        /// <summary>${WP_REST_GetFeaturesByGeometryParameters_attribute_IDs_D}</summary>
        [JsonProperty("ids")]
        public IList<int> IDs { get; set; }
        /// <summary>${WP_REST_GetFeaturesByBufferParameters_attribute_Fields_D}</summary>
        [JsonProperty("queryParameter")]
        public QueryParameter QueryParameter { get; set; }
    }
}
