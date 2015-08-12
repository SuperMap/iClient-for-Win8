using Newtonsoft.Json;
using System.Collections.Generic;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesParametersBase_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesParametersBase_Description}</para>
    /// </summary>
    public abstract class GetFeaturesParametersBase
    {
        /// <summary><para>${WP_REST_GetFeaturesParametersBase_constructor_D}</para></summary>
        public GetFeaturesParametersBase()
        {
            FromIndex = 0;
            ToIndex = -1;
        }

        /// <summary>${WP_REST_GetFeaturesParametersBase_attribute_DatasetNames_D}</summary>
        [JsonProperty("datasetNames")]
        public IList<string> DatasetNames { get; set; }

        /// <summary>${WP_REST_GetFeaturesParametersBase_attribute_FromIndex_D}</summary>
        [JsonIgnore()]
        public int FromIndex { get; set; }
        /// <summary>${WP_REST_GetFeaturesParametersBase_attribute_ToIndex_D}</summary>
        [JsonIgnore()]
        public int ToIndex { get; set; }

        [JsonProperty("getFeatureMode")]
        internal GetFeatureMode GetFeatureMode { get; set; }
    }
}
