using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Core
{
    internal abstract class QueueSystemBase<T> where T : IData
    {
        private IList<T> _queue;
        private IList<T> _successfulData;
        private ExecutantQueueBase<T> _executingQueue;
        public QueueSystemBase()
        {
            _queue = new List<T>();
            _successfulData = new List<T>();
        }

        protected IList<T> Queue
        {
            get { return _queue; }
        }

        protected IList<T> SuccessfulData
        {
            get { return _successfulData; }
        }

        protected ExecutantQueueBase<T> ExecutingQueue
        {
            get { return _executingQueue; }
            set { _executingQueue = value; }
        }

        protected virtual void InitManager()
        {
            _executingQueue.ExecutionCompleted += _executingQueue_ExecutionCompleted;
        }

        void _executingQueue_ExecutionCompleted(object sender, ExecutionComplatedEventArgs<T> e)
        {
            AfterExecute(e.Result);
            OnExecutionCompleted(e.Result);
            lock (this.Queue)
            {
                DoNext();
            }
        }

        /// <summary>
        /// 向系统中加入新的数据。
        /// </summary>
        /// <param name="datas"></param>
        public abstract Task InputData(IList<T> datas);

        /// <summary>
        /// 从队列中获取一个数据，开始处理。
        /// </summary>
        /// <returns></returns>
        protected virtual bool DoNext()
        {
            bool successful = false;
            if (Queue == null || Queue.Count <= 0)
            {
                return successful;
            }
            T temp = default(T);
            foreach (T data in Queue)
            {
                if (data.Status == DataStatus.Waiting)
                {
                    data.Status = DataStatus.Executing;
                    temp = data;
                    break;
                }
            }

            if (temp != null)
            {
                successful = _executingQueue.InputData(temp);
                if (!successful)
                {
                    temp.Status = DataStatus.Waiting;
                }
            }
            return successful;
        }

        /// <summary>
        /// 在获取到处理完成的数据后，进行后续的处理。
        /// </summary>
        /// <param name="data"></param>
        protected virtual void AfterExecute(T data)
        {
            if (Queue.Contains(data))
            {
                Queue.Remove(data);
            }

            if (!SuccessfulData.Contains(data) && data != null)
            {
                SuccessfulData.Add(data);
            }
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
