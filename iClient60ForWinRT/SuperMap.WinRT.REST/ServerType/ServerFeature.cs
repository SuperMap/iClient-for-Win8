using System.Collections.Generic;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ServerType_ServerFeature_Tile}</para>
    /// 	<para>${REST_ServerType_ServerFeature_Description}</para>
    /// </summary>
    public class ServerFeature
    {
        /// <summary>${REST_ServerType_ServerFeature_constructor_D}</summary>
        public ServerFeature()
        {
            FieldNames = new List<string>();
            FieldValues = new List<string>();
        }

        internal int ID { get; set; }
        /// <summary>${REST_ServerType_ServerFeature_attribute_FieldNames_D}</summary>
        public List<string> FieldNames { get; set; }
        /// <summary>${REST_ServerType_ServerFeature_attribute_FieldValues_D}</summary>
        public List<string> FieldValues { get; set; }
        /// <summary>${REST_ServerType_ServerFeature_attribute_Geometry_D}</summary>
        public ServerGeometry Geometry { get; set; }
        /// <summary>${REST_ServerType_method_FromJson_D}</summary>
        /// <returns>${REST_ServerType_method_FromJson_return}</returns>
        /// <param name="json">${REST_ServerType_method_FromJson_param_jsonObject}</param>
        internal static ServerFeature FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new ServerFeature
            {
                FieldNames = JsonHelper.ToStringList(json["fieldNames"].GetArray()),
                FieldValues = JsonHelper.ToStringList(json["fieldValues"].GetArray()),
                Geometry = ServerGeometry.FromJson(json["geometry"].GetObjectEx())
            };
        }
    }
}
