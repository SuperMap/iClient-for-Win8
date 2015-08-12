using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// ${WP_REST_JoinItem_Title}
    /// </summary>
    public class JoinItem
    {
        /// <summary>
        /// ${WP_REST_JoinItem_constructor_D}
        /// </summary>
        public JoinItem()
        {

        }
        /// <summary>
        /// ${WP_REST_JoinItem_attribute_ForeignTableName_D}
        /// </summary>
        [JsonProperty("foreignTableName")]
        public string ForeignTableName { get; set; }
        /// <summary>
        /// ${WP_REST_JoinItem_attribute_JoinFilter_D}
        /// </summary>
        [JsonProperty("joinFilter")]
        public string JoinFilter { get; set; }
        /// <summary>
        /// ${WP_REST_JoinItem_attribute_JoinType_D}
        /// </summary>
        [JsonProperty("joinType")]
        public JoinType JoinType { get; set; }
    }
}
