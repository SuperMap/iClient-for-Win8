using Newtonsoft.Json;
using System.Collections.Generic;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_ServerFeature_Tile}</para>
    /// 	<para>${WP_REST_ServerType_ServerFeature_Description}</para>
    /// </summary>
    public class ServerFeature
    {
        /// <summary>${WP_REST_ServerType_ServerFeature_constructor_D}</summary>
        public ServerFeature()
        {
            FieldNames = new List<string>();
            FieldValues = new List<string>();
        }
        [JsonProperty("ID")]
        internal int ID { get; set; }
        /// <summary>${WP_REST_ServerType_ServerFeature_attribute_FieldNames_D}</summary>
        [JsonProperty("fieldNames")]
        public List<string> FieldNames { get; set; }
        /// <summary>${WP_REST_ServerType_ServerFeature_attribute_FieldValues_D}</summary>
        [JsonProperty("fieldValues")]
        public List<string> FieldValues { get; set; }
        /// <summary>${WP_REST_ServerType_ServerFeature_attribute_Geometry_D}</summary>
        [JsonProperty("geometry")]
        public ServerGeometry Geometry { get; set; }
        
    }
}
