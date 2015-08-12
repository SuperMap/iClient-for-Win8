using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_LinkItem_title}</summary>
    public class LinkItem
    {
        /// <summary>
        /// ${WP_REST_LinkItem_constructor_D}
        /// </summary>
        public LinkItem()
        {

        }
        /// <summary>${WP_REST_LinkItem_attribute_DatasourceConnectionInfo_D}</summary>
        [JsonProperty("datasourceConnectionInfo")]
        public DatasourceConnectionInfo DatasourceConnectionInfo { get; set; }

        /// <summary>${WP_REST_LinkItem_attribute_ForeignKeys_D}</summary>
        [JsonProperty("foreignKeys")]
        public List<string> ForeignKeys { get; set; }
        /// <summary>${WP_REST_LinkItem_attribute_ForeignTable_D}</summary>
        [JsonProperty("foreignTable")]
        public string ForeignTable { get; set; }
        /// <summary>${WP_REST_LinkItem_attribute_LinkFields_D}</summary>
        [JsonProperty("linkFields")]
        public List<string> LinkFields { get; set; }
        /// <summary>${WP_REST_LinkItem_attribute_LinkFilter_D}</summary>
        [JsonProperty("linkFilter")]
        public string LinkFilter { get; set; }
        /// <summary>${WP_REST_LinkItem_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_LinkItem_attribute_PrimaryKeys_D}</summary>
        [JsonProperty("primaryKeys")]
        public List<string> PrimaryKeys { get; set; }
    }
}
