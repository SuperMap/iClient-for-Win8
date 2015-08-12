using System;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_SupplyResult_Title}</para>
    /// 	<para>${REST_SupplyResult_Description}</para>
    /// </summary>
    public class SupplyResult
    {
        internal SupplyResult()
        { }

        /// <summary><para>${REST_SupplyResult_attribute_ActualResourceValue_D}</para></summary>
        public double ActualResourceValue { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_AverageWeight_D}</summary>
        public double AverageWeight { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_DemandCount_D}</summary>
        public int DemandCount { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_MaxWeight_D}</summary>
        public double MaxWeight { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_NodeID_D}</summary>
        public int NodeID { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_ResourceValue_D}</summary>
        public double ResourceValue { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_TotalWeights_D}</summary>
        public double TotalWeights { get; private set; }
        /// <summary>${REST_SupplyResult_attribute_Type_D}</summary>
        public SupplyCenterType Type { get; private set; }

        /// <summary>${REST_SupplyResult_method_fromJson_D}</summary>
        /// <returns>${REST_SupplyResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_SupplyResult_method_fromJson_param_jsonObject}</param>
        internal static SupplyResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            SupplyResult result = new SupplyResult();
            result.ActualResourceValue = json["actualResourceValue"].GetNumberEx();
            result.AverageWeight = json["averageWeight"].GetNumberEx();
            result.DemandCount = (int)json["demandCount"].GetNumberEx();
            result.MaxWeight = json["maxWeight"].GetNumberEx();
            result.NodeID = (int)json["nodeID"].GetNumberEx();
            result.ResourceValue = json["resourceValue"].GetNumberEx();
            result.TotalWeights = json["totalWeights"].GetNumberEx();
            result.Type = (SupplyCenterType)Enum.Parse(typeof(SupplyCenterType), json["type"].GetStringEx(), true);

            return result;
        }
    }
}
