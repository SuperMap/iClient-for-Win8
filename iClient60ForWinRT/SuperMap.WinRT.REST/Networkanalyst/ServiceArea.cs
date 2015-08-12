using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ServiceArea_Title}</para>
    /// 	<para>${REST_ServiceArea_Description}</para>
    /// </summary>
    public class ServiceArea
    {
        internal ServiceArea()
        { }
        /// <summary>${REST_ServiceArea_attribute_EdgeFeatures_D}</summary>
        public FeatureCollection EdgeFeatures { get; private set; }
        /// <summary>${REST_ServiceArea_attribute_EdgeIDs_D}</summary>
        public List<int> EdgeIDs { get; private set; }
        /// <summary>${REST_ServiceArea_attribute_NodeFeatures_D}</summary>
        public FeatureCollection NodeFeatures { get; private set; }
        /// <summary>${REST_ServiceArea_attribute_NodeIDs_D}</summary>
        public List<int> NodeIDs { get; private set; }
        /// <summary>${REST_ServiceArea_attribute_Routes_D}</summary>
        public List<Route> Routes { get; private set; }
        /// <summary>${REST_ServiceArea_attribute_ServiceRegion_D}</summary>
        public Geometry ServiceRegion { get; private set; }

        /// <summary>${REST_ServiceArea_method_FromJson_D}</summary>
        /// <returns>${REST_ServiceArea_method_FromJson_return}</returns>
        /// <param name="json">${REST_ServiceArea_method_FromJson_param_jsonObject}</param>
        internal static ServiceArea FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            ServiceArea result = new ServiceArea();

            if (json["edgeFeatures"].ValueType != JsonValueType.Null)
            {
                result.EdgeFeatures = new FeatureCollection();
                for (int i = 0; i < json["edgeFeatures"].GetArray().Count; i++)
                {
                    result.EdgeFeatures.Add(ServerFeature.FromJson(json["edgeFeatures"].GetArray()[i].GetObjectEx()).ToFeature());
                }
            }
            if (json["edgeIDs"].ValueType != JsonValueType.Null)
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
            if (json["routes"].ValueType != JsonValueType.Null)
            {
                result.Routes = new List<Route>();
                for (int i = 0; i < json["routes"].GetArray().Count; i++)
                {
                    result.Routes.Add(Route.FromJson(json["routes"].GetArray()[i].GetObjectEx()));
                }
            }
            result.ServiceRegion = ServerGeometry.FromJson(json["serviceRegion"].GetObjectEx()).ToGeometry();

            return result;
        }
    }
}
