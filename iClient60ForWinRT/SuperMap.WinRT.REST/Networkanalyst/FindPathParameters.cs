

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindPathParameters_Title}</para>
    /// 	<para>${REST_FindPathParameters_Description}</para>
    /// </summary>
    /// <remarks>${REST_FindPathParameters_Remarks}</remarks>
    /// <typeparam name="T">${REST_FindPathParameters_param_T}</typeparam>
    public class FindPathParameters<T>
    {
        /// <summary>${REST_FindPathParameters_constructor_D}</summary>
        public FindPathParameters()
        { }

        /// <summary>${REST_FindPathParameters_attribute_Nodes_D}</summary>
        public IList<T> Nodes { get; set; }


        /// <summary><para>${REST_FindPathParameters_attribute_HasLeastEdgeCount_D}</para>
        /// <para><img src="FindPathLeastEdges.bmp"/></para>
        /// </summary>
        /// 
        public bool HasLeastEdgeCount { get; set; }

        /// <summary>${REST_FindPathParameters_attribute_Parameter_D}</summary>
        public TransportationAnalystParameter Parameter { get; set; }

    }
}
