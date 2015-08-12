using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_Path_Title}</para>
    /// 	<para>${REST_Path_Description}</para>
    /// </summary>
    public class ServerPath
    {
        /// <summary>${REST_Path_constructor_D}</summary>
        internal ServerPath()
        { }
        /// <summary>${REST_Path_attribute_edgeFeatures_D}</summary>
        public FeatureCollection EdgeFeatures { get; protected set; }
        /// <summary>${REST_Path_attribute_edgeIDs_D}</summary>
        public List<int> EdgeIDs { get; protected set; }
        /// <summary>${REST_Path_attribute_nodeFeatures_D}</summary>
        public FeatureCollection NodeFeatures { get; protected set; }
        /// <summary>${REST_Path_attribute_nodeIDs_D}</summary>
        public List<int> NodeIDs { get; protected set; }
        /// <summary>${REST_Path_attribute_pathGuideItems_D}</summary>
        public List<PathGuideItem> PathGuideItems { get; protected set; }
        /// <summary>${REST_Path_attribute_route_D}</summary>
        public Route Route { get; protected set; }
        /// <summary>${REST_Path_attribute_stopWeights_D}</summary>
        public List<double> StopWeights { get; protected set; }
        /// <summary>${REST_Path_attribute_weight_D}</summary>
        public double Weight { get; protected set; }
        /// <summary>${REST_Path_method_fromJson_D}</summary>
        /// <returns>${REST_Path_method_fromJson_return}</returns>
        /// <param name="json">${REST_Path_method_fromJson_param_jsonObject}</param>
        public static ServerPath ServerPathFromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            ServerPath result = new ServerPath();
            if (json["edgeFeatures"].ValueType !=JsonValueType.Null)
            {
                result.EdgeFeatures = new FeatureCollection();
                for (int i = 0; i < json["edgeFeatures"].GetArray().Count; i++)
                {
                    result.EdgeFeatures.Add(ServerFeature.FromJson(json["edgeFeatures"].GetArray()[i].GetObjectEx()).ToFeature());
                }
            }
            if (json["edgeIDs"].ValueType !=JsonValueType.Null)
            {
                result.EdgeIDs = new List<int>();
                for (int i = 0; i < json["edgeIDs"].GetArray().Count; i++)
                {
                    result.EdgeIDs.Add((int)json["edgeIDs"].GetArray()[i].GetNumberEx());
                }
            }
            if (json["nodeFeatures"].ValueType != JsonValueType.Null)
            {
                result.NodeFeatures = new FeatureCollection();
                for (int i = 0; i < json["nodeFeatures"].GetArray().Count; i++)
                {
                    result.NodeFeatures.Add(ServerFeature.FromJson(json["nodeFeatures"].GetArray()[i].GetObjectEx()).ToFeature());
                }
            }
            if (json["nodeIDs"].ValueType != JsonValueType.Null)
            {
                result.NodeIDs = new List<int>();
                for (int i = 0; i < json["nodeIDs"].GetArray().Count; i++)
                {
                    result.NodeIDs.Add((int)json["nodeIDs"].GetArray()[i].GetNumberEx());
                }
            }
            if (json["pathGuideItems"].ValueType != JsonValueType.Null)
            {
                result.PathGuideItems = new List<PathGuideItem>();
                for (int i = 0; i < json["pathGuideItems"].GetArray().Count; i++)
                {
                    result.PathGuideItems.Add(PathGuideItem.FromJson(json["pathGuideItems"].GetArray()[i].GetObjectEx()));
                }
            }
            if (json["route"].ValueType != JsonValueType.Null)
            {
                result.Route = Route.FromJson(json["route"].GetObjectEx());
            }
            if (json["stopWeights"].ValueType != JsonValueType.Null)
            {
                result.StopWeights = new List<double>();
                for (int i = 0; i < json["stopWeights"].GetArray().Count; i++)
                {
                    result.StopWeights.Add(json["stopWeights"].GetArray()[i].GetNumberEx());
                }
            }
            result.Weight = json["weight"].GetNumberEx();

            return result;
        }


    }
}
