using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransferStopInfo_Title}</para>
    /// </summary>
    public class TransferStopInfo
    {
        /// <summary>${REST_TransferStopInfo_constructor_D}</summary>
        public TransferStopInfo()
        {

        }

        /// <summary>${REST_TransferStopInfo_attribute_Alias_D}</summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>${REST_TransferStopInfo_attribute_Id_D}</summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>${REST_TransferStopInfo_attribute_Name_D}</summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>${REST_TransferStopInfo_attribute_Position_D}</summary>
        public Point2D Position
        {
            get;
            set;
        }

        /// <summary>${REST_TransferStopInfo_attribute_StopId_D}</summary>
        public long StopId
        {
            get;
            set;
        }
        /// <summary>${REST_TransferStopInfo_method_FromJson_D}</summary>
        /// <param name="json">${REST_TransferStopInfo_method_FromJson_param_jsonObject}</param>
        internal static TransferStopInfo FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            TransferStopInfo stop = new TransferStopInfo();
            stop.Alias = json["alias"].GetStringEx();
            stop.Id = (int)json["id"].GetNumberEx();
            stop.Name = json["name"].GetStringEx();
            stop.Position = JsonHelper.ToPoint2D(json["position"].GetObjectEx());
            stop.StopId = (long)json["stopID"].GetNumberEx();

            return stop;
        }
    }
}
