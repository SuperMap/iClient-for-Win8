

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindMTSPPathsParameters_Title}</para>
    /// 	<para>${REST_FindMTSPPathsParameters_Description}</para>
    /// 	<para>
    /// 		<list type="table">
    /// 			<item>
    /// 				<term><img src="MTSPPathsR.bmp"/></term>
    /// 				<description><img src="MTSPPathsL.bmp"/></description>
    /// 			</item>
    /// 		</list>
    /// 	</para>
    /// </summary>
    /// <typeparam name="T">${REST_FindMTSPPathsParameters_param_T}</typeparam>
    public class FindMTSPPathsParameters<T>
    {
        /// <summary>${REST_FindMTSPPathsParameters_constructor_D}</summary>
        public FindMTSPPathsParameters()
        { }

        /// <summary>${REST_FindMTSPPathsParameters_attribute_centers_D}</summary>
        public IList<T> Centers { get; set; }
        /// <summary>${REST_FindMTSPPathsParameters_attribute_Nodes_D}</summary>
        public IList<T> Nodes { get; set; }

        /// <summary>${REST_FindMTSPPathsParameters_attribute_HasLeastTotalCost_D}</summary>
        public bool HasLeastTotalCost { get; set; }
        /// <summary>${REST_FindMTSPPathsParameters_attribute_Parameter_D}</summary>
        public TransportationAnalystParameter Parameter { get; set; }
    }
}
