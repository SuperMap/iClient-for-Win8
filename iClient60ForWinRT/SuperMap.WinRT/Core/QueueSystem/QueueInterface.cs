using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 执行者。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal interface IExecutant<T> where T:IData
    {
        /// <summary>
        /// 执行者的状态，为Free表示处于空闲状态。
        /// </summary>
        ExecuteStatus Status { get; set; }

        /// <summary>
        /// 正在处理的数据。
        /// </summary>
        T Data { get; }

        /// <summary>
        /// 具体操作。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Doing(T data);

        Func<T, Task<T>> HowToDo { get; set; }

        /// <summary>
        /// 执行完成后的通知事件。
        /// </summary>
        event ExecutionComplatedHandler<T> ExecutionCompleted;
    }

    /// <summary>
    /// 传递的数据。
    /// </summary>
    internal interface IData
    {
        /// <summary>
        /// 数据所处的状态。
        /// </summary>
        DataStatus Status { get; set; }
        /// <summary>
        /// 是否处理成功。
        /// </summary>
        bool IsSuccess { get; set; }
    }

}
