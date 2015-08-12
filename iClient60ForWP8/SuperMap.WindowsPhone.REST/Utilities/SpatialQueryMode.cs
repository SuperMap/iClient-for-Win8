using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// ${WP_REST_SpatialQueryMode_title}
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SpatialQueryMode
    {
        /// <summary>${WP_REST_SpatialQueryMode_attribute_NONE_D}</summary>
        NONE,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_CONTAIN_D}</summary>
        CONTAIN,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_CROSS_D}</summary>
        CROSS,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_DISJOINT_D}</summary>
        DISJOINT,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_IDENTITY_D}</summary>
        IDENTITY,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_INTERSECT_D}</summary>
        INTERSECT,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_OVERLAP_D}</summary>
        OVERLAP,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_TOUCH_D}</summary>
        TOUCH,
        /// <summary>${WP_REST_SpatialQueryMode_attribute_WITHIN_D}</summary>
        WITHIN
    }
}
