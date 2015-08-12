using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// ${WP_REST_DatasourceConnectionInfo_title}
    /// </summary>
    public class DatasourceConnectionInfo
    {
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_Alias_D}</summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_Connect_D}</summary>
        [JsonProperty("connect")]
        public bool Connect { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_DataBase_D}</summary>
        [JsonProperty("dataBase")]
        public string DataBase { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_Driver_D}</summary>
        [JsonProperty("driver")]
        public string Driver { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_EngineType_D}</summary>
        [JsonProperty("engineType")]
        public EngineType EngineType { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_Exclusive_D}</summary>
        [JsonProperty("exclusive")]
        public bool Exclusive { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_OpenLinkTable_D}</summary>
        public bool OpenLinkTable { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_Password_D}</summary>
        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_ReadOnly_D}</summary>
        [JsonProperty("readOnly")]
        public bool ReadOnly { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_Server_D}</summary>
        [JsonProperty("server")]
        public string Server { get; set; }
        /// <summary>${WP_REST_DatasourceConnectInfo_attribute_User_D}</summary>
        [JsonProperty("user")]
        public string User { get; set; }
    }
}
