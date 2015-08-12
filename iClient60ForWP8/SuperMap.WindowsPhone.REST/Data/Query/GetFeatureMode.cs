using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    /// <summary>${WP_REST_GetFeatureMode_Title}</summary>
    internal enum GetFeatureMode
    {
        /// <summary>${WP_REST_GetFeatureMode_attribute_BOUNDS_D}</summary>
        BOUNDS,
        /// <summary>${WP_REST_GetFeatureMode_attribute_BOUNDSATTRIBUTEFILTER_D}</summary>
        BOUNDS_ATTRIBUTEFILTER,
        /// <summary>${WP_REST_GetFeatureMode_attribute_BUFFER_D}</summary>
        BUFFER,
        /// <summary>${WP_REST_GetFeatureMode_attribute_BUFFERATTRIBUTEFILTER_D}</summary>
        BUFFER_ATTRIBUTEFILTER,
        /// <summary>${WP_REST_GetFeatureMode_attribute_ID_D}</summary>
        ID,
        /// <summary>${WP_REST_GetFeatureMode_attribute_SPATIAL_D}</summary>
        SPATIAL,
        /// <summary>${WP_REST_GetFeatureMode_attribute_SPATIALATTRIBUTEFILTER_D}</summary>
        SPATIAL_ATTRIBUTEFILTER,
        /// <summary>${WP_REST_GetFeatureMode_attribute_SQL_D}</summary>
        SQL
    }
}
