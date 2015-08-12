using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Core
{
    internal enum ExecuteStatus
    {
        Busy,
        Free
    }

    /// <summary>
    /// 数据所处的状态。
    /// </summary>
    internal enum DataStatus
    {
        /// <summary>
        /// 数据处于空闲状态。
        /// </summary>
        Free,
        /// <summary>
        /// 数据已经被校验完成，处于等待被处理状态。
        /// </summary>
        Waiting,
        /// <summary>
        /// 数据正在被处理。
        /// </summary>
        Executing,
        /// <summary>
        /// 数据被处理完成。
        /// </summary>
        Complated
    }
}
