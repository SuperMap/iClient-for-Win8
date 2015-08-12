

using System.Collections.Generic;
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesByIDsParameters_Title}</para>
    /// 	<para>${REST_GetFeaturesByIDsParameters_Description}</para>
    /// </summary>
    public class GetFeaturesByIDsParameters:GetFeaturesParametersBase
    {
        /// <summary>${REST_GetFeaturesByGeometryParameters_constructor_D}</summary>
        public GetFeaturesByIDsParameters()
        {
        }
        /// <summary>${REST_GetFeaturesByGeometryParameters_attribute_IDs_D}</summary>
        public IList<int> IDs { get; set; }
        /// <summary>${REST_GetFeaturesByBufferParameters_attribute_Fields_D}</summary>
        public IList<string> Fields { get; set; }
    }
}
