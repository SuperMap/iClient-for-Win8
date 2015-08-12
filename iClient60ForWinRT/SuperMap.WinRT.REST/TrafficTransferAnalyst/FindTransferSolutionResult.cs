using System;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTransferSolutionResult_Title}</para>
    /// </summary>
    public class FindTransferSolutionResult
    {
        /// <summary>${REST_FindTransferSolutionResult_constructor_D}</summary>
        public FindTransferSolutionResult()
        {

        }
        /// <summary>${REST_FindTransferSolutionResult_attribute_DefaultGuide_D}</summary>
        public TransferGuide DefaultGuide
        {
            get;
            set;
        }
        /// <summary>${REST_FindTransferSolutionResult_attribute_SolutionItems_D}</summary>
        public TransferSolution[] SolutionItems
        {
            get;
            set;
        }
        /// <summary>${REST_FindTransferSolutionResult_method_FromJson_D}</summary>
        /// <param name="json">${REST_FindTransferSolutionResult_method_FromJson_param_jsonObject}</param>
        internal static FindTransferSolutionResult FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            FindTransferSolutionResult result = new FindTransferSolutionResult();
            result.DefaultGuide = TransferGuide.FromJson(json["defaultGuide"].GetObjectEx());
            JsonArray items = json["solutionItems"].GetArray();
            if (items != null && items.Count > 0)
            {
                result.SolutionItems = new TransferSolution[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    result.SolutionItems[i] = TransferSolution.FromJson(items[i].GetObjectEx());
                }
            }

            return result;
        }
    }
}
