using System.Collections.Generic;
using System;
using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_Query_QueryResult_Title}</para>
    /// 	<para>${REST_Query_QueryResult_Description}</para>
    /// </summary>
    public class QueryResult
    {
        internal QueryResult()
        {
            Recordsets = new List<Recordset>();
        }

        /// <summary>${REST_Query_ResultSet_attribute_totalCount_D}</summary>
        [JsonProperty("totalCount")]
        public int TotalCount { get; internal set; }
        /// <summary>${REST_Query_ResultSet_attribute_currentCount_D}</summary>
        [JsonProperty("currentCount")]
        public int CurrentCount { get; internal set; }
        /// <summary>${REST_Query_ResultSet_attribute_customResponse_D}</summary>
        [JsonProperty("customResponse")]
        public string CustomResponse { get; internal set; }
        /// <summary>${REST_Query_ResultSet_attribute_recordSets_D}</summary>
        [JsonProperty("recordsets")]
        public List<Recordset> Recordsets { get; internal set; }

        //当返回结果是资源时，用到此项；
        /// <summary>${REST_Query_ResultSet_attribute_ResourceInfo_D}</summary>
        [JsonIgnore()]
        public ResourceInfo ResourceInfo { get; internal set; }

    }
}
