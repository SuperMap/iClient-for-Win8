using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ClosestFacilityResult_Title}</para>
    /// 	<para>${REST_ClosestFacilityResult_Description}</para>
    /// </summary>
    public class FindClosestFacilitiesResult
    {
        internal FindClosestFacilitiesResult()
        { }

        ///// <summary>${REST_ClosestFacilitiesEventArgs_attribute_MapImage_D}</summary>
        //public NAResultMapImage MapImage { get; private set; }

        /// <summary>${REST_ClosestFacilitiesEventArgs_attribute_FacilityPathList_D}</summary>
        public List<ClosestFacilityPath> FacilityPathList { get; private set; }

        /// <summary>${REST_ClosestFacilityResult_method_fromJson_D}</summary>
        /// <returns>${REST_ClosestFacilityResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_ClosestFacilityResult_method_fromJson_param_jsonObject}</param>
        internal static FindClosestFacilitiesResult FromJson(JsonObject json)
        {
            if (json != null)
            {
                FindClosestFacilitiesResult result = new FindClosestFacilitiesResult();
                //result.MapImage = NAResultMapImage.FromJson((JsonObject)json["mapImage"]);

                if (json["facilityPathList"].ValueType !=JsonValueType.Null)
                {
                    result.FacilityPathList = new List<ClosestFacilityPath>();
                    for (int i = 0; i < json["facilityPathList"].GetArray().Count; i++)
                    {
                        result.FacilityPathList.Add(ClosestFacilityPath.FromJson(json["facilityPathList"].GetArray()[i].GetObjectEx()));
                    }
                }

                return result;
            }

            return null;
        }


    }
}
