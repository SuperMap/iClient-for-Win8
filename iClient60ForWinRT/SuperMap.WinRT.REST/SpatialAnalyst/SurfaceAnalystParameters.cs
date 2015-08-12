
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_ExtractParameters_Title}</para>
    /// 	<para>${REST_ExtractParameters_Description}</para>
    /// </summary>
    public class SurfaceAnalystParameters
    {
        /// <summary>${REST_ExtractParameters_constructor_D}</summary>
        public SurfaceAnalystParameters()
        { }
        /// <summary>${REST_ExtractParamsSetting_attribute_ExtractMethod_D}</summary>
        public SurfaceAnalystMethod SurfaceAnalystMethod
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_Resolution_D}</summary>
        public double Resolution
        {
            get;
            set;
        }
        /// <summary>${REST_SurfaceAnalystParameters_attribute_ExtractParamsSetting_D}</summary>
        public SurfaceAnalystParametersSetting ParametersSetting
        {
            get;
            set;
        }

        /// <summary>${REST_SurfaceAnalystParameters_attribute_MaxReturnRecordCount_D}</summary>
        public int MaxReturnRecordCount
        {
            get;
            set;
        }
    }
}
