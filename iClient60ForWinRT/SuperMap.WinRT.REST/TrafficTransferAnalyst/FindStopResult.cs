using System;
using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindStopResult_Title}</para>
    /// </summary>
    public class FindStopResult
    {
        /// <summary>${REST_FindStopResult_attribute_Stops_D}</summary>
        public List<TransferStopInfo> Stops { get;internal set; }

        /// <summary>${REST_FindStopResult_constructor_D}</summary>
        public FindStopResult()
        { }
        /// <summary>${REST_FindStopResult_constructor_TransferStopInfo_D}</summary>
        /// <param name="stops">${REST_FindStopResult_constructor_param_stops}</param>
        public FindStopResult(List<TransferStopInfo> stops)
        {
            Stops = stops;
        }
        /// <summary>${REST_FindStopResult_method_FromJson_D}</summary>
        /// <param name="json">${REST_FindStopResult_method_FromJson_param_jsonObject}</param>
        internal static FindStopResult FromJson(JsonArray json)
        {
            if (json == null)
            {
                return null;
            }

            FindStopResult result = new FindStopResult();
            if (json != null && json.Count > 0)
            {
                result.Stops = new List<TransferStopInfo>();
                for (int i = 0; i < json.Count; i++)
                {
                    TransferStopInfo stop = TransferStopInfo.FromJson(json[i].GetObjectEx());
                    result.Stops.Add(stop);
                }
            }

            return result;
        }
    }
}
