
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransferLines_Title}</para>
    /// </summary>
    public class TransferLines
    {
        /// <summary>${REST_TransferLines_constructor_D}</summary>
        public TransferLines()
        {

        }
        /// <summary>${REST_TransferLines_attribute_LineItems_D}</summary>
        public TransferLine[] LineItems
        {
            get;
            set;
        }
        /// <summary>${REST_TransferLines_method_FromJson_D}</summary>
        /// <param name="json">${REST_TransferLines_method_FromJson_param_jsonObject}</param>
        internal static TransferLines FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            TransferLines lines = new TransferLines();
            JsonArray items = (JsonArray)json["lineItems"];
            if (items != null && items.Count > 0)
            {
                lines.LineItems = new TransferLine[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    lines.LineItems[i] = TransferLine.FromJson(items[i].GetObject());
                }
            }
            return lines;
        }
    }
}
