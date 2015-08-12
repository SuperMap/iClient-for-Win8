

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_PrimeMeridianType_Title}</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrimeMeridianType
    {
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_ATHENS_D}</summary>
        PRIMEMERIDIAN_ATHENS,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_BERN_D}</summary>
        PRIMEMERIDIAN_BERN,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_BOGOTA_D}</summary>
        PRIMEMERIDIAN_BOGOTA,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_BRUSSELS_D}</summary>
        PRIMEMERIDIAN_BRUSSELS,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_FERRO_D}</summary>
        PRIMEMERIDIAN_FERRO,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_GREENWICH_D}</summary>
        PRIMEMERIDIAN_GREENWICH,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_JAKARTA_D}</summary>
        PRIMEMERIDIAN_JAKARTA,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_LISBON_D}</summary>
        PRIMEMERIDIAN_LISBON,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_MADRID_D}</summary>
        PRIMEMERIDIAN_MADRID,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_PARIS_D}</summary>
        PRIMEMERIDIAN_PARIS,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_ROME_D}</summary>
        PRIMEMERIDIAN_ROME,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_STOCKHOLM_D}</summary>
        PRIMEMERIDIAN_STOCKHOLM,
        /// <summary>${WP_REST_PrimeMeridianType_attribute_PRIMEMERIDIAN_PRIMEMERIDIAN_USER_DEFINED_D}</summary>
        PRIMEMERIDIAN_USER_DEFINED
    }
}
