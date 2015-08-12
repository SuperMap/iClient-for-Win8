using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperMap.WindowsPhone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST.Help
{
    /// <summary>${WP_REST_GeometryConverter_Title}</summary>
    public class GeometryConverter:JsonConverter
    {
        /// <summary>${WP_REST_GeometryConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Geometry).IsAssignableFrom(objectType);
        }
        /// <summary>${WP_REST_GeometryConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            JObject jobj = JObject.Load(reader);
            ServerGeometry sg = jobj.ToObject<ServerGeometry>();
            return sg.ToGeometry();
            
        }
        /// <summary>${WP_REST_GeometryConverter_method_WriteJson_D}</summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Geometry geometry = value as Geometry;
            if (geometry != null)
            {
                ServerGeometry sg = geometry.ToServerGeometry();
                writer.WriteRawValue(JsonConvert.SerializeObject(sg));
            }
        }
    }
}
