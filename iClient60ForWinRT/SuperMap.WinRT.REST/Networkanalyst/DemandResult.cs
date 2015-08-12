using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_DemandResult_Title}</para>
    /// 	<para>${REST_DemandResult_Description}</para>
    /// </summary>
    public class DemandResult
    {
        internal DemandResult()
        { }
        /// <summary>${REST_DemandResult_attribute_ActualResourceValue_D}</summary>
        public double ActualResourceValue { get; private set; }
        /// <summary>${REST_DemandResult_attribute_DemandID_D}</summary>
        public int DemandID { get; private set; }
        /// <summary>${REST_DemandResult_attribute_IsEdge_D}</summary>
        public bool IsEdge { get; private set; }
        /// <summary>${REST_DemandResult_attribute_SupplyCenter_D}</summary>
        public SupplyCenter SupplyCenter { get; private set; }

        /// <summary>${REST_DemandResult_method_fromJson_D}</summary>
        /// <returns>${REST_DemandResult_method_fromJson_Return}</returns>
        /// <param name="json">${REST_DemandResult_method_fromJson_param_jsonObject}</param>
        internal static DemandResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            DemandResult result = new DemandResult();
            result.ActualResourceValue = json["actualResourceValue"].GetNumberEx();
            result.DemandID = (int)json["demandID"].GetNumberEx();
            result.IsEdge = json["isEdge"].GetBooleanEx();
            result.SupplyCenter = SupplyCenter.FromJson(json["supplyCenter"].GetObjectEx());

            return result;
        }
    }
}
