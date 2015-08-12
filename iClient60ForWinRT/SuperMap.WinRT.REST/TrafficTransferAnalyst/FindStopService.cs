using System;
using SuperMap.WinRT.Service;
using System.Collections.Generic;
using Windows.Data.Json;
using System.Net;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindStopService_Title}</para>
    /// </summary>
    public class FindStopService : ServiceBase
    {
        /// <summary>${REST_FindStopService_constructor_D}</summary>
        public FindStopService()
        { }

        /// <summary>${REST_FindStopService_constructor_param_url}</summary>
        public FindStopService(string url)
            : base(url)
        { }

        /// <summary>${REST_FindStopService_method_ProcessAsync_param_Parameters}</summary>
        public async Task<FindStopResult> ProcessAsync(FindStopParameter paramer)
        {
            GenerateAbsoluteUrl(paramer);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(paramer), false, false, false);
            JsonArray jsonObject = JsonArray.Parse(result);
            return FindStopResult.FromJson(jsonObject);
        }

        private void GenerateAbsoluteUrl(FindStopParameter parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (!this.Url.EndsWith("/"))
            {
                this.Url += "/";
            }
            this.Url += string.Format("stops/keyword/{0}.rjson?", WebUtility.UrlEncode(parameters.KeyWord));
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(FindStopParameter parameters)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["returnPosition"] = parameters.ReturnPosition ? "true" : "false";
            return dic;
        }

        //private void FindStopService_Complated(object sender, RequestEventArgs args)
        //{
        //    JsonArray jsonObject = JsonArray.Parse(args.Result);
        //    FindStopResult result = FindStopResult.FromJson(jsonObject);
        //    FindStopEventArgs e = new FindStopEventArgs(result, args.Result, args.UserState);
        //    lastResult = result;
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}
        ///// <summary>${REST_FindStopService_event_ProcessCompleted_D}</summary>

        //public event EventHandler<FindStopEventArgs> ProcessCompleted;

        //private FindStopResult lastResult;
        ///// <summary>${REST_FindStopService_attribute_lastResult_D}</summary>
        //public FindStopResult LastResult
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
