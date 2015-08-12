

using Newtonsoft.Json;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.REST.Help;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_QueryByGeometryParameters_Tile}</para>
    /// 	<para>${REST_QueryByGeometryParameters_Description}</para>
    /// </summary>
    public class QueryByGeometryParameters:QueryParameterBase
    {
        /// <summary>${REST_QueryByGeometryParameters_constructor_None_D}</summary>
        public QueryByGeometryParameters()
        { }

        /// <summary>${REST_QueryByGeometryParameters_attribute_Geometry_D}</summary>
        [JsonProperty("geometry")]
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }
        /// <summary>${REST_QueryByGeometryParameters_attribute_SpatialQueryMode_D}</summary>
        [JsonProperty("spatialQueryMode")]
        public SpatialQueryMode SpatialQueryMode { get; set; }

    }
}
