using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum QueryMode
    {
        /// <summary>
        /// 范围查询。
        /// </summary>
        BoundsQuery,

        /// <summary>
        /// 海图属性查询。
        /// </summary>
        ChartAttributeQuery,

        /// <summary>
        /// 海图bounds查询。
        /// </summary>
        ChartBoundsQuery,

        /// <summary>
        /// 距离查询。
        /// </summary>
        DistanceQuery,

        /// <summary>
        /// 最近地物查询。
        /// </summary>
        FindNearest,

        /// <summary>
        /// 空间查询。
        /// </summary>
        SpatialQuery,

        /// <summary>
        /// SQL 查询。
        /// </summary>
        SqlQuery
    }
}
