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
    /// ${WP_REST_JoinType_title}
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum JoinType
    {
        /// <summary>
        /// ${WP_REST_JoinType_attribute_INNERJOIN_D}
        /// </summary>
        INNERJOIN,
        /// <summary>
        /// ${WP_REST_JoinType_attribute_LEFTJOIN_D}
        /// </summary>
        LEFTJOIN
    }
}
