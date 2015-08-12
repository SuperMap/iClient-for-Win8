using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperMap.WindowsPhone.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_Point2DConverter_Title}</summary>
    public class Point2DConverter:JsonConverter
    {
        /// <summary>${WP_REST_Point2DConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Point2D).IsAssignableFrom(objectType);
        }
        /// <summary>${WP_REST_Point2DConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Point2D.Empty;
            }
            Point2D point = Point2D.Empty;
            JObject obj = JObject.Load(reader);
            point.X = double.Parse(obj["x"].ToString(), CultureInfo.InvariantCulture);
            point.Y = double.Parse(obj["y"].ToString(), CultureInfo.InvariantCulture);
            return point;
        }
        /// <summary>${WP_REST_Point2DConverter_method_WriteJson_D}</summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Point2D point = (Point2D)value;
            if (point != Point2D.Empty)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(point.X);
                writer.WritePropertyName("y");
                writer.WriteValue(point.Y);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }

        }
    }
}
