using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransferGuideItem_Title}</para>
    /// 	<para>${REST_TransferGuideItem_Description}</para>
    /// </summary>
    public class TransferGuideItem
    {
        /// <summary>${REST_TransferGuideItem_constructor_D}</summary>
        public TransferGuideItem()
        {

        }

        /// <summary>${REST_TransferGuideItem_attribute_Distance_D}</summary>
        public double Distance
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_EndIndex_D}</summary>
        public int EndIndex
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_EndPosition_D}</summary>
        public Point2D EndPosition
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_EndStopName_D}</summary>
        public string EndStopName
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_IsWalking_D}</summary>
        public bool IsWalking
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_LineName_D}</summary>
        public string LineName
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_PassStopCount_D}</summary>
        public int PassStopCount
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_Route_D}</summary>
        public SuperMap.WinRT.Core.Geometry Route
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_StartIndex_D}</summary>
        public int StartIndex
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_StartPosition_D}</summary>
        public Point2D StartPosition
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuideItem_attribute_StartStopName_D}</summary>
        public string StartStopName
        {
            get;
            set;
        }
        /// <summary>${REST_TransferGuideItem_method_FromJson_D}</summary>
        /// <param name="json">${REST_TransferGuideItem_method_FromJson_param_jsonObject}</param>
        internal static TransferGuideItem FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            TransferGuideItem item = new TransferGuideItem();
            item.Distance = json["distance"].GetNumberEx();
            item.EndIndex = (int)json["endIndex"].GetNumberEx();
            item.EndPosition = JsonHelper.ToPoint2D(json["endPosition"].GetObjectEx());
            item.EndStopName = json["endStopName"].GetStringEx();
            item.IsWalking = json["isWalking"].GetBooleanEx();
            item.LineName = json["lineName"].GetStringEx();
            item.PassStopCount = (int)json["passStopCount"].GetNumberEx();
            item.Route = Bridge.ToGeometry(ServerGeometry.FromJson(json["route"].GetObjectEx()));
            item.StartIndex = (int)json["startIndex"].GetNumberEx();
            item.StartPosition = JsonHelper.ToPoint2D(json["startPosition"].GetObjectEx());
            item.StartStopName = json["startStopName"].GetStringEx();

            return item;
        }
    }
}
