

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_FieldType_Tile}</para>
    /// 	<para>${REST_FieldType_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FieldType
    {
        /// <summary>${REST_FieldType_attribute_BOOLEAN_D}</summary>
        BOOLEAN,
        /// <summary>${REST_FieldType_attribute_BYTE_D}</summary>
        BYTE,
        /// <summary>${REST_FieldType_attribute_INT16_D}</summary>
        INT16,
        /// <summary>${REST_FieldType_attribute_INT32_D}</summary>
        INT32,
        /// 
        INT64,
        /// <summary>${REST_FieldType_attribute_SINGLE_D}</summary>
        SINGLE,
        /// <summary>${REST_FieldType_attribute_DOUBLE_D}</summary>
        DOUBLE,
        /// <summary>${REST_FieldType_attribute_DATETIME_D}</summary>
        DATETIME,
        /// <summary>${REST_FieldType_attribute_LONGBINARY_D}</summary>
        LONGBINARY,
        /// <summary>${REST_FieldType_attribute_TEXT_D}</summary>
        TEXT,
        /// <summary>${REST_FieldType_attribute_CHAR_D}</summary>
        CHAR
    }
}
