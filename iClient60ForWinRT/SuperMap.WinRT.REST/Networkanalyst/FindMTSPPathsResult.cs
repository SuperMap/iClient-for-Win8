using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindMTSPPathsResult_Title}</para>
    /// 	<para>${REST_FindMTSPPathsResult_Description}</para>
    /// </summary>
    public class FindMTSPPathsResult
    {
        internal FindMTSPPathsResult()
        { }
        ///// <summary>${REST_FindMTSPPathsResult_attribute_MapImage_D}</summary>
        //public NAResultMapImage MapImage { get; private set; }
        /// <summary>${REST_FindMTSPPathsResult_attribute_MTSPathList_D}</summary>
        public List<MTSPPath> MTSPathList { get; private set; }


        /// <summary>${REST_FindMTSPPathsResult_method_fromJson_D}</summary>
        /// <returns>${REST_FindMTSPPathsResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_FindMTSPPathsResult_method_fromJson_param_jsonObject}</param>
        internal static FindMTSPPathsResult FromJson(JsonObject json)
        {
            if (json == null) return null;

            FindMTSPPathsResult result = new FindMTSPPathsResult();

            //result.MapImage = NAResultMapImage.FromJson((JsonObject)json["mapImage"]);
            if (json["pathList"].ValueType !=JsonValueType.Null)
            {
                result.MTSPathList = new List<MTSPPath>();
                for (int i = 0; i < json["pathList"].GetArray().Count; i++)
                {
                    result.MTSPathList.Add(MTSPPath.FromJson(json["pathList"].GetArray()[i].GetObjectEx()));
                }
            }

            return result;
        }
    }
}
