
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_GeometryType_Tile}</para>
    /// 	<para>${WP_REST_ServerType_GeometryType_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServerGeometryType 
    {
        /// <summary>${WP_REST_ServerType_GeometryType_attribute_LINE_D}</summary>
        LINE,
        /// <summary>${WP_REST_ServerType_GeometryType_attribute_LINEM_D}</summary>
        LINEM,
        /// <summary>${WP_REST_ServerType_GeometryType_attribute_POINT_D}</summary>
        POINT,
        /// <summary>${WP_REST_ServerType_GeometryType_attribute_REGION_D}</summary>
        REGION,
        /// <summary>${WP_REST_ServerType_GeometryType_attribute_TEXT_D}</summary>
        TEXT,
        /// <summary>${WP_REST_ServerType_GeometryType_attribute_UNKNOWN_D}</summary>
        UNKNOWN
    }
}
