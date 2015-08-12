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
    /// <summary>${WP_REST_FeatureCollectionConvert_Title}</summary>
    public class FeatureCollectionConverter:JsonConverter
    {
        /// <summary>${WP_REST_FeatureCollectionConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICollection<Feature>).IsAssignableFrom(objectType);
        }
        /// <summary>${WP_REST_FeatureCollectionConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                return null;
            }
            JArray array = JArray.Load(reader);
            if (array != null && array.Count > 0)
            {
                List<Feature> list=new List<Feature>();
                foreach (var item in array)
                {
                    ServerFeature sFeature = item.ToObject<ServerFeature>();
                    list.Add(sFeature.ToFeature());
                }
                return list;
            }
            return null;
        }
        /// <summary>${WP_REST_FeatureCollectionConverter_method_WriteJson_D}</summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ICollection<Feature> list = value as ICollection<Feature>;
            if (list != null && list.Count > 0)
            {
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    ServerFeature sf=item.ToServerFeature();
                    writer.WriteRawValue(JsonConvert.SerializeObject(sf));
                }
                writer.WriteEndArray();
            }
        }
    }
}
