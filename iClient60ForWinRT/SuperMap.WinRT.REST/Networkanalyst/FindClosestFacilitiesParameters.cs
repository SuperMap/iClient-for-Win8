


using SuperMap.WinRT.Core;
using System.Collections.Generic;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    //T只能是Point2D和int类型；
    /// <summary>
    /// 	<para>${REST_ClosestFacilitiesParameters_Title}</para>
    /// 	<para>${REST_ClosestFacilitiesParameters_Description}</para>
    /// </summary>
    /// <remarks>${REST_ClosestFacilitiesParameters_Remarks}</remarks>
    ///  <typeparam name="T">${REST_ClosestFacilitiesParameters_param_T}</typeparam>
    public class FindClosestFacilitiesParameters<T>
    {
        /// <summary>${REST_ClosestFacilitiesParameters_constructor_D}</summary>
        public FindClosestFacilitiesParameters()
        {
            ExpectFacilityCount = 1;
        }

        /// <summary>${REST_ClosestFacilitiesParameters_attribute_Event_D}</summary>
        public T Event { get; set; }

        /// <summary>${REST_ClosestFacilitiesParameters_attribute_Facilities_D}</summary>
        public IList<T> Facilities { get; set; }

        /// <summary>${REST_ClosestFacilitiesParameters_attribute_ExpectFacilityCount_D}</summary>
        public int ExpectFacilityCount { get; set; }

        /// <summary>${REST_ClosestFacilitiesParameters_attribute_FromEvent_D}</summary>
        public bool FromEvent { get; set; }

        /// <summary>${REST_ClosestFacilitiesParameters_attribute_MaxWeight_D}</summary>
        public double MaxWeight { get; set; }

        /// <summary>${REST_ClosestFacilitiesParameters_attribute_Parameter_D}</summary>
        public TransportationAnalystParameter Parameter { get; set; }

    }
}
