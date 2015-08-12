using SuperMap.WinRT.Utilities;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransferSolution_Title}</para>
    /// </summary>
    public class TransferSolution
    {
        /// <summary>${REST_TransferSolution_constructor_D}</summary>
        public TransferSolution()
        {

        }
        /// <summary>${REST_TransferSolution_attribute_TransferCount_D}</summary>
        public int TransferCount
        {
            get;
            set;
        }
        /// <summary>${REST_TransferSolution_attribute_LinesItems_D}</summary>
        public TransferLines[] LinesItems
        {
            get;
            set;
        }
        /// <summary>${REST_TransferSolution_method_FromJson_D}</summary>
        /// <param name="json">${REST_TransferSolution_method_FromJson_param_jsonObject}</param>
        internal static TransferSolution FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            TransferSolution solution = new TransferSolution();
            solution.TransferCount = (int)json["transferCount"].GetNumberEx();
            JsonArray items = json["linesItems"].GetArray();
            if (items != null && items.Count > 0)
            {
                solution.LinesItems = new TransferLines[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    solution.LinesItems[i] = TransferLines.FromJson(items[i].GetObjectEx());
                }
            }

            return solution;
        }
    }
}
