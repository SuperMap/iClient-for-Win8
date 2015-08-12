using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindLocationAnalystResult_Title}</para>
    /// 	<para>${REST_FindLocationAnalystResult_Description}</para>
    /// </summary>
    public class FindLocationAnalystResult
    {
        internal FindLocationAnalystResult()
        { }
        //TODO:应该直接给一张图片；
        ///// <summary>${REST_FindPathAnalystResult_attribute_MapImage_D}</summary>
        //public NAResultMapImage MapImage { get; private set; }

        /// <summary>${REST_FindLocationAnalystResult_attribute_DemandResults_D}</summary>
        public List<DemandResult> DemandResults { get; private set; }
        /// <summary>${REST_FindLocationAnalystResult_attribute_SupplyResults_D}</summary>
        public List<SupplyResult> SupplyResults { get; private set; }

        /// <summary>${REST_FindLocationAnalystResult_method_fromJson_D}</summary>
        /// <returns>${REST_FindLocationAnalystResult_method_fromJson_Return}</returns>
        /// <param name="json">${REST_FindLocationAnalystResult_method_fromJson_param_jsonObject}</param>
        internal static FindLocationAnalystResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            FindLocationAnalystResult result = new FindLocationAnalystResult();
            //result.MapImage = NAResultMapImage.FromJson((JsonObject)json["mapImage"]);

            if (json["demandResults"].ValueType != JsonValueType.Null && json["demandResults"].GetObjectEx().Count > 0)
            {
                result.DemandResults = new List<DemandResult>();
                for (int i = 0; i < json["demandResults"].GetArray().Count; i++)
                {
                    result.DemandResults.Add(DemandResult.FromJson(json["demandResults"].GetArray()[i].GetObjectEx()));
                }
            }
            if (json["supplyResults"].ValueType != JsonValueType.Null && json["supplyResults"].GetArray().Count > 0)
            {
                result.SupplyResults = new List<SupplyResult>();
                for (int i = 0; i < json["supplyResults"].GetArray().Count; i++)
                {
                    result.SupplyResults.Add(SupplyResult.FromJson(json["supplyResults"].GetArray()[i].GetObjectEx()));
                }
            }

            return result;
        }
    }
}
