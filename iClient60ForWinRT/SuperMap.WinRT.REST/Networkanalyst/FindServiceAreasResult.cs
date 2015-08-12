
using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ServiceAreasResult_Title}</para>
    /// 	<para>${REST_ServiceAreasResult_Description}</para>
    /// </summary>
    public class FindServiceAreasResult
    {
        internal FindServiceAreasResult()
        {
        }
        ///// <summary>${REST_ServiceAreasResult_attribute_MapImage_D}</summary>
        //public NAResultMapImage MapImage { get; private set; }
        /// <summary>${REST_ServiceAreasResult_attribute_ServiceAreaList_D}</summary>
        public List<ServiceArea> ServiceAreaList { get; private set; }

        /// <summary>${REST_ServiceAreasResult_method_FromJson_D}</summary>
        /// <returns>${REST_ServiceAreasResult_method_FromJson_return}</returns>
        /// <param name="json">${REST_ServiceAreasResult_method_FromJson_param_jsonObject}</param>
        internal static FindServiceAreasResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            FindServiceAreasResult result = new FindServiceAreasResult();
            //result.MapImage = NAResultMapImage.FromJson((JsonObject)json["mapImage"]);
            if (json["serviceAreaList"].ValueType != JsonValueType.Null && json["serviceAreaList"].GetArray().Count > 0)
            {
                result.ServiceAreaList = new List<ServiceArea>();
                for (int i = 0; i < json["serviceAreaList"].GetArray().Count; i++)
                {
                    result.ServiceAreaList.Add(ServiceArea.FromJson(json["serviceAreaList"].GetArray()[i].GetObjectEx()));
                }
            }

            return result;
        }
    }
}
