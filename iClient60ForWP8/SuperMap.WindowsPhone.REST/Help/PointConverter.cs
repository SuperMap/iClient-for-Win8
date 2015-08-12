using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_PointConverter_Title}</summary>
    public class PointConverter : JsonConverter
    {
        /// <summary>${WP_REST_PointConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Point).IsAssignableFrom(objectType);
        }
        /// <summary>${WP_REST_PointConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new Point();
            }
            Point point = new Point();
            JObject obj = JObject.Load(reader);
            point.X = double.Parse(obj["x"].ToString(), CultureInfo.InvariantCulture);
            point.Y = double.Parse(obj["y"].ToString(), CultureInfo.InvariantCulture);
            return point;
        }
        /// <summary>${WP_REST_PointConverter_method_WriteJson_D}</summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Point point = (Point)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(point.X);
            writer.WritePropertyName("y");
            writer.WriteValue(point.Y);
            writer.WriteEndObject();
        }
    }
}
