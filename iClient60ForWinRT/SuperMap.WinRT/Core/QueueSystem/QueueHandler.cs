using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Core
{
    internal delegate void ExecutionComplatedHandler<T>(IExecutant<T> Executant, ExecutionComplatedEventArgs<T> args) where T:IData;

    internal class ExecutionComplatedEventArgs<T> : EventArgs where T:IData
    {
        public ExecutionComplatedEventArgs(T result)
        {
            Result = result;
        }

        public T Result
        {
            get;
            private set;
        }
    }

}
