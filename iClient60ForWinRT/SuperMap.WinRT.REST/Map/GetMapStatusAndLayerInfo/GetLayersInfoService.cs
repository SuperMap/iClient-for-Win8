using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Service;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_GetLayersInfoService_Title}</para>
    /// 	<para>${REST_GetLayersInfoService_Description}</para>
    /// </summary>
    public class GetLayersInfoService : ServiceBase
    {
        /// <summary>${REST_GetLayersInfoService_constructor_None_D}</summary>
        /// <overloads>${REST_GetLayersInfoService_constructor_overloads_D}</overloads>
        public GetLayersInfoService()
        { }
        /// <summary>${REST_GetLayersInfoService_constructor_String_D}</summary>
        /// <param name="url">${REST_GetLayersInfoService_constructor_String_param_url}</param>
        public GetLayersInfoService(string url)
            : base(url)
        { }

        //url=http://localhost:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图
        //absoluteurl=http://localhost:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图/layers.rjson
        /// <summary>${REST_GetLayersInfoService_method_ProcessAsync_D}</summary>
        /// <param name="state">${REST_GetLayersInfoService_method_ProcessAsync_param_state}</param>
        public async Task<GetLayersInfoResult> ProcessAsync()
        {
            CheckUrl();

            var result = await base.SubmitRequest(Url, null, false, false, false);
            return GetLayersInfoResult.FromJson(result);
        }

        private void CheckUrl()
        {
            if (Url.EndsWith("/"))
            {
                Url.TrimEnd(new char[] { '/' });
            }

            this.Url = this.Url + "/layers.json";
        }

        //private void Request_Completed(object sender, RequestEventArgs e)
        //{
        //    //不做e.Error的判断
        //    if (e.Result != null && !string.IsNullOrEmpty(e.Result))
        //    {
        //        GetLayersInfoResult result = GetLayersInfoResult.FromJson(e.Result);
        //        lastResult = result;
        //        GetLayersInfoEventArgs args = new GetLayersInfoEventArgs(result, e.Result, e.UserState);
        //        OnProcessCompleted(args);
        //    }
        //}
        ///// <summary>${REST_GetLayersInfoService_event_processCompleted_D}</summary>
        //public event EventHandler<GetLayersInfoEventArgs> ProcessCompleted;

        //private void OnProcessCompleted(GetLayersInfoEventArgs e)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}

        //private GetLayersInfoResult lastResult;
        ///// <summary>${REST_GetLayersInfoResult_attribute_lastResult_D}</summary>
        //public GetLayersInfoResult LastResult
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
