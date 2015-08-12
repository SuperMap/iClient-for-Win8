using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_TSPPath_Title}</para>
    /// 	<para>${REST_TSPPath_Description}</para>
    /// </summary>
    public class TSPPath : ServerPath
    {
        internal TSPPath()
        { }

        /// <summary>${REST_TSPPath_attribute_StopIndexes_D}</summary>
        public List<int> StopIndexes { get; protected set; }

        /// <summary>${REST_TSPPath_method_fromJson_D}</summary>
        /// <returns>${REST_TSPPath_method_fromJson_return}</returns>
        /// <param name="json">${REST_TSPPath_method_fromJson_param_jsonObject}</param>
        public static TSPPath TSPPathFromJson(JsonObject json)
        {
            if (json == null)
                return null;

            TSPPath result = new TSPPath();
            if (json["stopIndexes"].ValueType != JsonValueType.Null)
            {
                result.StopIndexes = new List<int>();
                for (int i = 0; i < json["stopIndexes"].GetArray().Count; i++)
                {
                    result.StopIndexes.Add((int)json["stopIndexes"].GetArray()[i].GetNumberEx());
                }
            }

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
    }
}
