

using System.Collections.Generic;
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesByBufferParameters_Title}</para>
    /// 	<para>${REST_GetFeaturesByBufferParameters_Description}</para>
    /// </summary>
    public class GetFeaturesByBufferParameters : GetFeaturesParametersBase
    {
        /// <summary>${REST_GetFeaturesByBufferParameters_constructor_D}</summary>
        public GetFeaturesByBufferParameters()
        {
        }
        /// <summary>${REST_GetFeaturesByBufferParameters_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }
        /// <summary>${REST_GetFeaturesByBufferParameters_attribute_BufferDistance_D}</summary>
        public double BufferDistance { get; set; }
        /// <summary>${REST_GetFeaturesByBufferParameters_attribute_AttributeFilter_D}</summary>
        public string AttributeFilter { get; set; }
        /// <summary>${REST_GetFeaturesByBufferParameters_attribute_Fields_D}</summary>
        public IList<string> Fields { get; set; }

    }
}
