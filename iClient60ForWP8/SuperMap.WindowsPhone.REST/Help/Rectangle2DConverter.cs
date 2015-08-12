using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperMap.WindowsPhone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_Rectangle2DConverter_Title}</summary>
    public class Rectangle2DConverter:JsonConverter
    {
        /// <summary>${WP_REST_Rectangle2DConverter_method_CanConvert_D}</summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Rectangle2D).IsAssignableFrom(objectType);
        }
        /// <summary>${WP_REST_Rectangle2DConverter_method_ReadJson_D}</summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Rectangle2D.Empty;
            }
            Point2DConverter ponitConverter = new Point2DConverter();
            JObject obj = JObject.Load(reader);
            Point2D rightTop = JsonConvert.DeserializeObject<Point2D>(obj["rightTop"].ToString());
            Point2D leftBottom = JsonConvert.DeserializeObject<Point2D>(obj["leftBottom"].ToString());

            return new Rectangle2D(rightTop, leftBottom);
        }
        /// <summary>${WP_REST_Rectangle2DConverter_method_WriteJson_D}</summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Rectangle2D rect = (Rectangle2D)value;
            if (rect != Rectangle2D.Empty)
            {
                Point2DConverter ponitConverter = new Point2DConverter();
                writer.WriteStartObject();
                writer.WritePropertyName("rightTop");
                writer.WriteRawValue(JsonConvert.SerializeObject(rect.TopRight, ponitConverter));
                writer.WritePropertyName("leftBottom");
                writer.WriteRawValue(JsonConvert.SerializeObject(rect.BottomLeft, ponitConverter));
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
