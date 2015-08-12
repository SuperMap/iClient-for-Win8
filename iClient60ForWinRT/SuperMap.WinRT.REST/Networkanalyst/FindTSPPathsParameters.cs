

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTSPPathsParameters_Title}</para>
    /// 	<para>${REST_FindTSPPathsParameters_Description}</para>
    /// </summary>
    /// <typeparam name="T">${REST_FindTSPPathsParameters_param_T}</typeparam>
    public class FindTSPPathsParameters<T>
    {
        /// <summary>${REST_FindTSPPathsParameters_constructor_D}</summary>
        public FindTSPPathsParameters()
        {
        }
        /// <summary>${REST_FindTSPPathsParameters_attribute_Nodes_D}</summary>
        public List<T> Nodes { get; set; }

        /// <summary>${REST_FindTSPPathsParameters_attribute_EndNodeAssigned_D}</summary>
        public bool EndNodeAssigned { get; set; }
        /// <summary>${REST_FindTSPPathsParameters_attribute_Parameter_D}</summary>
        public TransportationAnalystParameter Parameter { get; set; }
    }
}
