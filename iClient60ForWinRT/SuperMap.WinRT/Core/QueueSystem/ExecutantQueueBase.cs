using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 执行队列，包含所有的执行者。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal abstract class ExecutantQueueBase<T> where T:IData
    {
        private object _thisLock = new object();
        protected IExecutant<T>[] _executants;

        public ExecutantQueueBase(int count,Func<T,Task<T>> howToDo)
        {
            if (count <= 0)
            {
                throw new Exception(Resources.ExceptionStrings.CountLessThanOne);
            }
        }

        protected virtual void InitQueue()
        {
            for (int i = 0; i < Count; i++)
            {
                _executants[i].ExecutionCompleted += ExecutantQueueBase_ExecutionCompleted;
            }
        }

        void ExecutantQueueBase_ExecutionCompleted(IExecutant<T> Executant, ExecutionComplatedEventArgs<T> args)
        {
            OnExecutionCompleted(args.Result);
        }

        public int Count
        {
            get { return _executants.Length; }
        }

        /// <summary>
        /// 放入一个待处理数据。如果成功放入，返回true，否则返回false。
        /// </summary>
        /// <returns></returns>
        public bool InputData(T data)
        {
            lock (_thisLock)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (_executants[i] != null && _executants[i].Status == ExecuteStatus.Free)
                    {
                        _executants[i].Doing(data);
                        return true;
                    }
                }
            }
            return false;
        }

        public event EventHandler<ExecutionComplatedEventArgs<T>> ExecutionCompleted;

        protected virtual void OnExecutionCompleted(T result)
        {
            if (ExecutionCompleted != null)
            {
                ExecutionCompleted(this, new ExecutionComplatedEventArgs<T>(result));
            }
        }

    }

}
