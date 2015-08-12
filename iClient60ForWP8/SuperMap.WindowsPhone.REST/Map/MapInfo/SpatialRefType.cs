

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_SpatialRefType_Title}</para>
    /// 	<para>${WP_REST_SpatialRefType_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SpatialRefType
    {
        /// <summary>${WP_REST_SpatialRefType_attribute_SPATIALREF_EARTH_LONGITUDE_LATITUDE_D}</summary>
        SPATIALREF_EARTH_LONGITUDE_LATITUDE,
        /// <summary>${WP_REST_SpatialRefType_attribute_SPATIALREF_EARTH_PROJECTION_D}</summary>
        SPATIALREF_EARTH_PROJECTION,
        /// <summary>${WP_REST_SpatialRefType_attribute_SPATIALREF_NONEARTH_D}</summary>
        SPATIALREF_NONEARTH
    }
}
