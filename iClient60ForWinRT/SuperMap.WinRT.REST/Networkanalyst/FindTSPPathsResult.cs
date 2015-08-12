using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTSPPathsResult_Title}</para>
    /// 	<para>${REST_FindTSPPathsResult_Description}</para>
    /// </summary>
    public class FindTSPPathsResult
    {
        internal FindTSPPathsResult()
        {
        }
        ///// <summary>${REST_FindTSPPathsResult_attribute_MapImage_D}</summary>
        //public NAResultMapImage MapImage { get; private set; }
        /// <summary>${REST_FindTSPPathsResult_attribute_tspPathList_D}</summary>
        public List<TSPPath> TSPPathList { get; private set; }

        /// <summary>${REST_FindTSPPathsResult_method_fromJson_D}</summary>
        /// <returns>${REST_FindTSPPathsResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_FindTSPPathsResult_method_fromJson_param_jsonObject}</param>
        internal static FindTSPPathsResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            FindTSPPathsResult result = new FindTSPPathsResult();
            //result.MapImage = NAResultMapImage.FromJson((JsonObject)json["mapImage"]);
            if (json["tspPathList"].ValueType != JsonValueType.Null && json["tspPathList"].GetArray().Count > 0)
            {
                result.TSPPathList = new List<TSPPath>();
                for (int i = 0; i < json["tspPathList"].GetArray().Count; i++)
                {
                    result.TSPPathList.Add(TSPPath.TSPPathFromJson(json["tspPathList"].GetArray()[i].GetObjectEx()));
                }
            }
            return result;
        }
    }
}
