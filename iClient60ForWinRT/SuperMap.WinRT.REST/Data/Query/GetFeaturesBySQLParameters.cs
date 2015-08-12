

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesBySQLParameters_Title}</para>
    /// 	<para>${REST_GetFeaturesBySQLParameters_Description}</para>
    /// </summary>
    public class GetFeaturesBySQLParameters:GetFeaturesParametersBase
    {
        /// <summary>${REST_GetFeaturesBySQLParameters_constructor_None_D}</summary>
        public GetFeaturesBySQLParameters()
        {
        }
        /// <summary>${REST_GetFeaturesBySQLParameters_attribute_FilterParameter_D}</summary>
        public FilterParameter FilterParameter { get; set; }

        /// <summary>${REST_GetFeaturesParametersBase_attribute_MaxFeatures_D}</summary>
        public int MaxFeatures { get; set; }
    }
}
