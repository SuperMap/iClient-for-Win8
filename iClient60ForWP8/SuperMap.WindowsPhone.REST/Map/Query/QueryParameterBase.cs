

using Newtonsoft.Json;
using System.Collections.Generic;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_Query_queryParamerters_Title}</para>
    /// 	<para>${REST_Query_queryParamerters_Description}</para>
    /// </summary>
    public abstract class QueryParameterBase
    {
        /// <summary>${REST_Query_QueryParamerters_constructor_None_D}</summary>
        public QueryParameterBase()
        {

        }

        /// <summary>
        /// 是否返回查询结果的 Bounds 信息，当 returnContent=false 时有效。 
        /// 如果为 true，返回查询结果的 Bounds 信息（响应结构中的 customResult 字段）。 
        /// 如果为 false，则不返回 Bounds 信息。默认为 false。 
        /// </summary>
        [JsonIgnore()]
        public bool ReturnCustomResult { get; set; }
        /// <summary>${REST_Query_attribute_ReturnContent_D}</summary>
        [JsonIgnore()]
        public bool ReturnContent { get; set; }
        [JsonProperty("queryMode")]
        internal QueryMode QueryMode { get; set; }

        [JsonProperty("queryParameters")]
        public QueryParameterSet QueryParameters { get; set; }
    }
}
