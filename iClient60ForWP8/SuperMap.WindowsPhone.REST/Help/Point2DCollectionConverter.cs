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
    /// <summary>${WP_REST_Point2DCollectionConverter_Title}</summary>
    public class Point2DCollectionConverter:JsonConverter
    {
        /// <summary>${WP_REST_Point2DCollectionConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ICollection<Point2D>).IsAssignableFrom(objectType);
        }
        /// <summary>${WP_REST_Point2DCollectionConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                return null;
            }
            JArray array = JArray.Load(reader);
            if (array != null && array.Count > 0)
            {
                List<Point2D> list = new List<Point2D>();
                if (serializer == null)
                {
                    serializer = new JsonSerializer();
                }
                if (serializer.Converters.Count <= 0)
                {
                    serializer.Converters.Add(new Point2DConverter());
                }
                foreach (var item in array)
                {
                    Point2D point = item.ToObject<Point2D>(serializer);
                    list.Add(point);
                }
                return list;
            }
            return null;
        }
        /// <summary>
        /// ${WP_REST_Point2DCollectionConverter_method_WriteJson_D}
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ICollection<Point2D> list = value as ICollection<Point2D>;
            if (list != null && list.Count > 0)
            {
                Point2DConverter converter=new Point2DConverter();
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    writer.WriteRawValue(JsonConvert.SerializeObject(item, converter));
                }
                writer.WriteEndArray();
            }
        }
    }
}
