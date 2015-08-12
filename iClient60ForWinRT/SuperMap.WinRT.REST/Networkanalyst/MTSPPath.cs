using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_MTSPPath_Title}</para>
    /// 	<para>${REST_MTSPPath_Description}</para>
    /// </summary>
    public class MTSPPath :TSPPath
    {
        internal MTSPPath()
        { }

        /// <summary>${REST_MTSPPath_attribute_Center_D}</summary>
        public object Center { get; private set; }
        /// <summary>${REST_MTSPPath_attribute_NodesVisited_D}</summary>
        public List<object> NodesVisited { get; private set; }

        /// <summary>${REST_MTSPPath_method_fromJson_D}</summary>
        /// <returns>${REST_MTSPPath_method_fromJson_return}</returns>
        /// <param name="json">${REST_MTSPPath_method_fromJson_param_jsonObject}</param>
        internal static MTSPPath FromJson(JsonObject json)
        {
            if (json == null)
                return null;
            MTSPPath result = new MTSPPath();
            result.Center = json["center"];

            if (json["nodesVisited"].ValueType !=JsonValueType.Null)
            {
                result.NodesVisited = new List<object>();
                for (int i = 0; i < json["nodesVisited"].GetArray().Count; i++)
                {
                    result.NodesVisited.Add(json["nodesVisited"].GetArray()[i].Stringify());
                }
            }

            if (json["stopIndexes"].ValueType !=JsonValueType.Null)
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
