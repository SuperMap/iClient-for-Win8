using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindPathResult_Title}</para>
    /// 	<para>${REST_FindPathResult_Description}</para>
    /// </summary>
    public class FindPathResult
    {
        internal FindPathResult()
        { }

        ///// <summary>${REST_FindPathResult_attribute_mapImage_D}</summary>
        //public NAResultMapImage MapImage { get; private set; }
        /// <summary>${REST_FindPathResult_attribute_pathList_D}</summary>
        public List<ServerPath> PathList { get; private set; }


        /// <summary>${REST_FindPathResult_method_fromJson_D}</summary>
        /// <returns>${REST_FindPathResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_FindPathResult_method_fromJson_param_jsonObject}</param>
        internal static FindPathResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            FindPathResult result = new FindPathResult();

           // result.MapImage = NAResultMapImage.FromJson((JsonObject)json["mapImage"]);
            if (json["pathList"].ValueType !=JsonValueType.Null)
            {
                result.PathList = new List<ServerPath>();
                for (int i = 0; i < json["pathList"].GetArray().Count; i++)
                {
                    result.PathList.Add(ServerPath.ServerPathFromJson(json["pathList"].GetArray()[i].GetObjectEx()));
                }
            }

            return result;
        }
    }
}
