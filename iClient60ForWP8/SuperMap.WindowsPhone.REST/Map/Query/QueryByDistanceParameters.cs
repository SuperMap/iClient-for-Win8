

using Newtonsoft.Json;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.REST.Help;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_QueryByDistanceParameters_Tile}</para>
    /// 	<para>${REST_QueryByDistanceParameters_Description}</para>
    /// </summary>
    public class QueryByDistanceParameters : QueryParameterBase
    {
        /// <summary>${REST_QueryByDistanceParameters_constructor_None_D}</summary>
        public QueryByDistanceParameters()
        { }
        /// <summary>${REST_QueryByDistanceParameters_attribute_Distance_D}</summary>
        [JsonProperty("distance")]
        public double Distance { get; set; }

        /// <summary>${REST_QueryByDistanceParameters_attribute_Geometry_D}</summary>
        [JsonProperty("geometry")]
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }

        /// <summary>${REST_QueryByDistanceParameters_attribute_IsNearest_D}</summary>
        [JsonIgnore()]
        public bool IsNearest { get; set; }
    }
}
