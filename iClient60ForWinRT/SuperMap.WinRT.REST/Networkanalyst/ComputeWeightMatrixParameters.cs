

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ComputeWeightMatrixParameters_Title}</para>
    /// 	<para>${REST_ComputeWeightMatrixParameters_Description}</para>
    /// </summary>
    /// <typeparam name="T">${REST_ComputeWeightMatrixParameters_param_T}</typeparam>
    public class ComputeWeightMatrixParameters<T>
    {
        /// <summary>${REST_ComputeWeightMatrixParameters_constructor_D}</summary>
        public ComputeWeightMatrixParameters()
        {
        }
        /// <summary>${REST_ComputeWeightMatrixParameters_attribute_Nodes_D}</summary>
        public IList<T> Nodes { get; set; }

        /// <summary>${REST_ComputeWeightMatrixParameters_attribute_parameter_D}</summary>
        public TransportationAnalystParameter Parameter { get; set; }
    }
}
