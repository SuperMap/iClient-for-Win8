using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// ${WP_REST_QueryParameter_title}
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        /// ${WP_REST_QueryParamter_constructor_D}
        /// </summary>
        public QueryParameter()
        {

        }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_AttributeFilter_D}
        /// </summary>
        [JsonProperty("attributeFilter")]
        public string AttributeFilter { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_Fields_D}
        /// </summary>
        [JsonProperty("fields")]
        public List<string> Fields { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_GroupBy_D}
        /// </summary>
        [JsonProperty("groupBy")]
        public string GroupBy { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_Ids_D}
        /// </summary>
        [JsonProperty("ids")]
        public List<int> Ids { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_JoinItems_D}
        /// </summary>
        [JsonProperty("joinItems")]
        public List<JoinItem> JoinItems { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_LinkItems_D}
        /// </summary>
        [JsonProperty("linkItems")]
        public List<LinkItem> LinkItems { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_Name_D}
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// ${WP_REST_QueryParamter_attribute_OrderBy_D}
        /// </summary>
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }
    }
}
