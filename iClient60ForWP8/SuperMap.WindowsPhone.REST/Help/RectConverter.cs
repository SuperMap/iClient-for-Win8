using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_Rectangle2DConverter_Title}</summary>
    public class RectConverter : JsonConverter
    {
        /// <summary>${WP_REST_RectConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Rect).IsAssignableFrom(objectType);
        }

        /// <summary>${WP_REST_RectConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new Rect();
            }
            PointConverter ponitConverter = new PointConverter();
            JObject obj = JObject.Load(reader);
            Point rightBottom = JsonConvert.DeserializeObject<Point>(obj["rightBottom"].ToString());
            Point leftTop = JsonConvert.DeserializeObject<Point>(obj["leftTop"].ToString());

            return new Rect(rightBottom, leftTop);
        }
        /// <summary>${WP_REST_RectConverter_method_WriteJson_D}</summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Rect rect = (Rect)value;
            PointConverter ponitConverter = new PointConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("rightBottom");
            writer.WriteRawValue(JsonConvert.SerializeObject(new Point(rect.Right,rect.Bottom), ponitConverter));
            writer.WritePropertyName("leftTop");
            writer.WriteRawValue(JsonConvert.SerializeObject(new Point(rect.Left, rect.Top), ponitConverter));
            writer.WriteEndObject();
        }
    }
}
