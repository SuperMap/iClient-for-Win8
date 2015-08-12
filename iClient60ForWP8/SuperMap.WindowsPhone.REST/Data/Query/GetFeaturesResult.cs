using SuperMap.WindowsPhone.Utilities;
using SuperMap.WindowsPhone.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using SuperMap.WindowsPhone.REST;
using SuperMap.WindowsPhone.REST.Help;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesResult_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesResult_Description}</para>
    /// </summary>
    public class GetFeaturesResult
    {
        internal GetFeaturesResult()
        {
        }
        /// <summary>${WP_REST_GetFeaturesResult_attribute_FeatureCount_D}</summary>
        [JsonProperty("featureCount")]
        public int FeatureCount { get; internal set; }
        /// <summary>${WP_REST_GetFeaturesResult_attribute_Features_D}</summary>

        [JsonProperty("features")]
        [JsonConverter(typeof(FeatureCollectionConverter))]
        public List<Feature> Features { get; internal set; }

    }
}
