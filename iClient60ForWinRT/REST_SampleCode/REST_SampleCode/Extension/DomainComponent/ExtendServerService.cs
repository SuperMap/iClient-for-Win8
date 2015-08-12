using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using System;
using System.Threading.Tasks;

namespace serverExtend
{
    public class ExtendServerService : ServiceBase
    {
        public ExtendServerService()
        {
        }

        public ExtendServerService(string url)
            : base(url)
        {
        }


        //public async Task<ExtendServerResult> ProcessAsync(ExtendServerParameters parameters)
        //{
        //    return await ProcessAsync(parameters, null);
        //}

        public async Task<ExtendServerResult> ProcessAsync(ExtendServerParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            base.Url += string.Format(".json?arg0={0}", parameters.Arg);

            var result = await base.SubmitRequest(base.Url, null, false);
            return ExtendServerResult.FromJson(result);
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    ExtendServerResult result = ExtendServerResult.FromJson(e.Result);
        //    lastResult = result;
        //    ExtendServerEventArgs args = new ExtendServerEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(ExtendServerEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        //public event EventHandler<ExtendServerEventArgs> ProcessCompleted;

        //private ExtendServerResult lastResult;
        //public ExtendServerResult LastResult
        //{
        //    get
        //    {
        //        return lastResult;
        //    }
        //    private set
        //    {
        //        lastResult = value;
        //        base.OnPropertyChanged("LastResult");
        //    }
        //}
    }
}
