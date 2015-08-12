using System;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransferGuide_Title}</para>
    /// </summary>
    public class TransferGuide
    {
        /// <summary>${REST_TransferGuide_constructor_D}</summary>
        public TransferGuide()
        {

        }

        /// <summary>${REST_TransferGuide_attribute_Count_D}</summary>
        public int Count
        {
            get;
            set;
        }


        /// <summary>${REST_TransferGuide_attribute_Items_D}</summary>
        public TransferGuideItem[] Items
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuide_attribute_TotalDistance_D}</summary>
        public double TotalDistance
        {
            get;
            set;
        }

        /// <summary>${REST_TransferGuide_attribute_TransferCount_D}</summary>
        public int TransferCount
        {
            get;
            set;
        }
        /// <summary>${REST_TransferGuide_method_FromJson_D}</summary>
        /// <param name="json">${REST_TransferGuide_method_FromJson_param_jsonObject}</param>
        internal static TransferGuide FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            TransferGuide guide = new TransferGuide();
            guide.Count = (int)json["count"].GetNumberEx();
            guide.TotalDistance = json["totalDistance"].GetNumberEx();
            guide.TransferCount = (int)json["transferCount"].GetNumberEx();
            JsonArray items = json["items"].GetArray();
            if (items != null && items.Count > 0)
            {
                guide.Items = new TransferGuideItem[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    guide.Items[i] = TransferGuideItem.FromJson(items[i].GetObjectEx());
                }
            }

            return guide;
        }
    }
}
