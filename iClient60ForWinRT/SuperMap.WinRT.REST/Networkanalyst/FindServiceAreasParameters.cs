

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ServiceAreaParameters_Title}</para>
    /// 	<para>${REST_ServiceAreaParameters_Description}</para>
    /// </summary>
    /// <typeparam name="T">${REST_FindServiceAreaParameters_param_T}</typeparam>
    public class FindServiceAreasParameters<T>
    {
        /// <summary>${REST_ServiceAreaParameters_constructor_D}</summary>
        public FindServiceAreasParameters()
        { }
        //T的类型只能为int或Point2D
        /// <summary>${REST_ServiceAreaParameters_attribute_Centers}</summary>
        public IList<T> Centers { get; set; }
        /// <summary>${REST_ServiceAreaParameters_attribute_Weights}</summary>
        public IList<double> Weights { get; set; }

        /// <summary>${REST_ServiceAreaParameters_attribute_IsFromCenter}</summary>
        public bool IsFromCenter { get; set; }
        /// <summary>${REST_ServiceAreaParameters_attribute_IsCenterMutuallyExclusive}</summary>
        public bool IsCenterMutuallyExclusive { get; set; }
        /// <summary>${REST_ServiceAreaParameters_attribute_Parameter}</summary>
        public TransportationAnalystParameter Parameter { get; set; }

    }
}
