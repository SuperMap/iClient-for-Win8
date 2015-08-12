using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 查询参数集合类。
    /// </summary>
    public class QueryParameterSet
    {
        public QueryParameterSet()
        {
            ResampleExpectCount = -1;
        }

        /// <summary>
        /// 自定义参数，供扩展使用。
        /// </summary>
        [JsonProperty("customParams")]
        public string CustomParams { get; set; }

        /// <summary>
        /// 查询记录期望返回结果记录，该值大于0。
        /// </summary>
        [JsonProperty("expectCount")]
        public int ExpectCount { get; set; }

        /// <summary>
        /// 网络数据集对应的查询类型，分为点和线两种类型，默认为线几何对象类型，即LINE。
        /// </summary>
        [JsonProperty("networkType")]
        public ServerGeometryType NetworkType { get; set; }

        /// <summary>
        /// 查询结果选项对象，用于指定查询结果中包含的内容。
        /// </summary>
        [JsonProperty("queryOption")]
        public QueryOption QueryOption { get; set; }

        /// <summary>
        /// 查询参数数组。
        /// </summary>
        [JsonProperty("queryParams")]
        public List<QueryParameter> QueryParams { get; set; }

        /// <summary>
        /// 查询每个要素期望的重采样后返回的二维坐标对的数目，默认值为-1，表示不设置。
        /// </summary>
        [JsonProperty("resampleExpectCount")]
        public int ResampleExpectCount { get; set; }

        /// <summary>
        /// 查询起始记录位置，默认为0。
        /// </summary>
        [JsonProperty("startRecord")]
        public int StartRecord { get; set; }
    }
}
