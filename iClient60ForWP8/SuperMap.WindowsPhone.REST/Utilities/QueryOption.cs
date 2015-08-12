

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_Query_QueryOption_Title}</para>
    /// 	<para>${REST_Query_QueryOption_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QueryOption
    {
        /// <summary>${REST_Query_QueryOption_attribute_ATTRIBUTE_D}</summary>
        ATTRIBUTE,
        /// <summary>${REST_Query_QueryOption_attribute_ATTRIBUTEANDGEOMETRY_D}</summary>
        ATTRIBUTEANDGEOMETRY,
        /// <summary>${REST_Query_QueryOption_attribute_GEOMETRY_D}</summary>
        GEOMETRY
    }
}
