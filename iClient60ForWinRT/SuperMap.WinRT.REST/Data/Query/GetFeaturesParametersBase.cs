using System.Collections.Generic;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesParametersBase_Title}</para>
    /// 	<para>${REST_GetFeaturesParametersBase_Description}</para>
    /// </summary>
    public abstract class GetFeaturesParametersBase
    {
        /// <summary><para>${REST_GetFeaturesParametersBase_constructor_D}</para></summary>
        public GetFeaturesParametersBase()
        {
            FromIndex = 0;
            ToIndex = -1;
        }

        /// <summary>${REST_GetFeaturesParametersBase_attribute_DatasetNames_D}</summary>
        public IList<string> DatasetNames { get; set; }

        /// <summary>${REST_GetFeaturesParametersBase_attribute_FromIndex_D}</summary>
        public int FromIndex { get; set; }
        /// <summary>${REST_GetFeaturesParametersBase_attribute_ToIndex_D}</summary>
        public int ToIndex { get; set; }
        
        
    }
}
