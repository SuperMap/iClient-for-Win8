

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Query_SpatialQueryMode_Title}</para>
    /// 	<para>${REST_Query_SpatialQueryMode_Description}</para>
    /// 	<para>
    /// 		<table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="5" cellpadding="1" align="center">
    /// 			<tbody>
    /// 				<tr>
    /// 					<td>${iServer2_SpatialQueryMode_R1C1}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R1C2}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R1C3}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R1C4}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R1C5}</td>
    /// 				</tr>
    /// 				<tr>
    /// 					<td>${iServer2_SpatialQueryMode_R2C1}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R2C2}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R2C2}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R2C4}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R2C5}</td>
    /// 				</tr>
    /// 				<tr>
    /// 					<td>${iServer2_SpatialQueryMode_R3C1}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R3C2}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R3C3}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R3C4}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R3C5}</td>
    /// 				</tr>
    /// 				<tr>
    /// 					<td>${iServer2_SpatialQueryMode_R4C1}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R4C2}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R4C3}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R4C4}</td>
    /// 					<td>${iServer2_SpatialQueryMode_R4C5}</td>
    /// 				</tr>
    /// 			</tbody>
    /// 		</table>
    /// 	</para>
    /// </summary>
    public enum SpatialQueryMode
    {
        /// <summary>${REST_Query_SpatialQueryMode_attribute_NONE_D}</summary>
        NONE,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_IDENTITY_D}</para>
        /// 	<para>
        /// 	  <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td><img src="Identity_PP.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td></td>
        /// 					<td><img src="Identity_LL.png"/></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 					<td><img src="Identity_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>
        /// 	</para>
        /// </summary>
        IDENTITY,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_DISJOINT_D}</para>
        /// 	 <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td><img src="Disjoint_PP.png"/></td>
        /// 					<td><img src="Disjoint_PL.png"/></td>
        /// 					<td><img src="Disjoint_PR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td><img src="Disjoint_LP.png"/></td>
        /// 					<td><img src="Disjoint_LL.png"/></td>
        /// 					<td><img src="Disjoint_LR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td><img src="Disjoint_RP.png"/></td>
        /// 					<td><img src="Disjoint_RL.png"/></td>
        /// 					<td><img src="Disjoint_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>
        /// </summary>
        DISJOINT,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_INTERSECT_D}</para>
        /// 	<para>
        /// 	    <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td><img src="Intersect_PP.png"/></td>
        /// 					<td><img src="Intersect_PL.png"/></td>
        /// 					<td><img src="Intersect_PR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td><img src="Intersect_LP.png"/></td>
        /// 					<td><img src="Intersect_LL.png"/></td>
        /// 					<td><img src="Intersect_LR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td><img src="Intersect_RP.png"/></td>
        /// 					<td><img src="Intersect_RL.png"/></td>
        /// 					<td><img src="Intersect_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>
        /// </para>
        /// </summary>
        INTERSECT,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_TOUCH_D}</para>
        /// 	<para>
        ///         <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td></td>
        /// 					<td><img src="Touch_PL.png"/></td>
        /// 					<td><img src="Touch_PR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td><img src="Touch_LP.png"/></td>
        /// 					<td><img src="Touch_LL.png"/></td>
        /// 					<td><img src="Touch_LR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td><img src="Touch_RP.png"/></td>
        /// 					<td><img src="Touch_RL.png"/></td>
        /// 					<td><img src="Touch_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>	
        /// </para>
        /// </summary>
        TOUCH,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_OVERLAP_D}</para>
        /// 	<para>
        /// 	    <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td></td>
        /// 					<td><img src="Overlap_LL.png"/></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 					<td><img src="Overlap_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>
        /// </para>
        /// </summary>
        OVERLAP,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_CROSS_D}</para>
        /// 	<para>
        ///     <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td></td>
        /// 					<td><img src="Cross_LL.png"/></td>
        /// 					<td><img src="Cross_LR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>	
        /// </para>
        /// </summary>
        CROSS,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_WITHIN_D}</para>
        /// 	<para>
        ///         <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td><img src="Within_PP.png"/></td>
        /// 					<td><img src="Within_PL.png"/></td>
        /// 					<td><img src="Within_PR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td></td>
        /// 					<td><img src="Within_LL.png"/></td>
        /// 					<td><img src="Within_LR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 					<td><img src="Within_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>	
        /// </para>
        /// </summary>
        WITHIN,
        /// <summary>
        /// 	<para>${REST_Query_SpatialQueryMode_attribute_CONTAIN_D}</para>
        /// 	<para>
        ///         <table class="Tablehead" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 95%; BORDER-BOTTOM: 1px solid; BACKGROUND-COLOR: #efeff7" cellspacing="1" cols="4" cellpadding="1" align="center">
        /// 			<tbody>
        /// 				<tr>
        /// 					<td rowspan="2">${iServer2_SpatialQueryMode_R1C1}</td>
        /// 					<td colspan="3">${iServer2_SpatialQueryMode_R1C2}</td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SdP.png"/></td>
        /// 					<td><img src="SdL.png"/></td>
        /// 					<td><img src="SdR.png"/></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SP.png"/></td>
        /// 					<td><img src="Contain_PP.png"/></td>
        /// 					<td></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SL.png"/></td>
        /// 					<td><img src="Contain_LP.png"/></td>
        /// 					<td><img src="Contain_LL.png"/></td>
        /// 					<td></td>
        /// 				</tr>
        /// 				<tr>
        /// 					<td><img src="SR.png"/></td>
        /// 					<td><img src="Contain_RP.png"/></td>
        /// 					<td><img src="Contain_RL.png"/></td>
        /// 					<td><img src="Contain_RR.png"/></td>
        /// 				</tr>
        /// 			</tbody>
        /// 		</table>	
        /// </para>
        /// </summary>
        CONTAIN
    }
}
