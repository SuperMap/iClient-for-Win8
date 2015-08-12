

using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ClosestFacilityPath_Title}</para>
    /// 	<para>${REST_ClosestFacilityPath_Description}</para>
    /// </summary>
    public class ClosestFacilityPath : ServerPath
    {
        internal ClosestFacilityPath()
        { }
        //实际类型为 Point2D or int
        /// <summary>${REST_ClosestFacilityPath_attribute_Facility_D}</summary>
        public object Facility { get; private set; }
        /// <summary>${REST_ClosestFacilityPath_attribute_FacilityIndex_D}</summary>
        public int FacilityIndex { get; private set; }

        /// <summary>${REST_ClosestFacilityPath_method_fromJson_D}</summary>
        /// <returns>${REST_ClosestFacilityPath_method_fromJson_return}</returns>
        /// <param name="json">${REST_ClosestFacilityPath_method_fromJson_param_jsonObject}</param>
        internal static ClosestFacilityPath FromJson(JsonObject json)
        {
            if (json != null)
            {
                ClosestFacilityPath result = new ClosestFacilityPath();

                if (json["facility"].ValueType==JsonValueType.Number)
                {
                    result.Facility = (int)json["facility"].GetNumberEx();
                }
                else
                {
                    result.Facility = JsonHelper.ToPoint2D(json["facility"].GetObjectEx());
                }
                result.FacilityIndex = (int)json["facilityIndex"].GetNumberEx();

                //对应父类中的属性；
                ServerPath path = ServerPath.ServerPathFromJson(json);
                result.EdgeFeatures = path.EdgeFeatures;
                result.EdgeIDs = path.EdgeIDs;
                result.NodeFeatures = path.NodeFeatures;
                result.NodeIDs = path.NodeIDs;
                result.PathGuideItems = path.PathGuideItems;
                result.Route = path.Route;
                result.StopWeights = path.StopWeights;
                result.Weight = path.Weight;

                return result;
            }
            return null;
        }
    }
}
