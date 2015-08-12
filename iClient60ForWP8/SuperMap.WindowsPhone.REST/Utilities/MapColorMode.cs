

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_MapColorMode_Tile}</para>
    /// 	<para>${WP_REST_ServerType_MapColorMode_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MapColorMode
    {
        /// <summary>${WP_REST_ServerType_MapColorMode_attribute_BLACK_WHITE_REVERSE_D}</summary>
        BLACK_WHITE_REVERSE,
        /// <summary>${WP_REST_ServerType_MapColorMode_attribute_BLACKWHITE_D}</summary>
        BLACKWHITE,
        /// <summary>${WP_REST_ServerType_MapColorMode_attribute_DEFAULT_D}</summary>
        DEFAULT,
        /// <summary>${WP_REST_ServerType_MapColorMode_attribute_GRAY_D}</summary>
        GRAY,
        /// <summary>${WP_REST_ServerType_MapColorMode_attribute_ONLY_BLACK_WHITE_REVERSE_D}</summary>
        ONLY_BLACK_WHITE_REVERSE
    }
}
