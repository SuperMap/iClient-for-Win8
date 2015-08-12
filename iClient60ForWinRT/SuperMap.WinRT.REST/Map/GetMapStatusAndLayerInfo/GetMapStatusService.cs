using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_GetMapStatusService_Title}</para>
    /// 	<para>${REST_GetMapStatusService_Description}</para>
    /// </summary>
    public class GetMapStatusService : ServiceBase
    {
        private int wkid;
        /// <summary>${REST_GetMapStatusService_Title}</summary>
        /// <overloads>${REST_GetMapStatusService_Description}</overloads>
        public GetMapStatusService()
        { }
        /// <summary>${REST_GetMapStatusService_constructor_String_D}</summary>
        /// <param name="url">${REST_GetMapStatusService_constructor_String_param_url}</param>
        public GetMapStatusService(string url)
            : base(url)
        { }

        public GetMapStatusService(string url, int wkid)
            : base(url)
        {
            this.wkid = wkid;
        }

        //url=http://localhost:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图
        //absoluteurl=http://192.168.11.11:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图.rjson
        /// <summary>${REST_GetMapStatusService_method_ProcessAsync_D}</summary>
        /// <param name="state">${REST_GetMapStatusService_method_ProcessAsync_param_state}</param>
        public async Task<GetMapStatusResult> ProcessAsync()
        {
            CheckUrl();

            var result = await base.SubmitRequest(Url, null, false, false, false);
            JsonObject json = JsonObject.Parse(result);
            return GetMapStatusResult.FromJson(json);
        }

        private void CheckUrl()
        {
            if (Url.EndsWith("/"))
            {
                Url.TrimEnd(new char[] { '/' });
            }
            if (this.wkid > 0)
            {
                this.Url = this.Url + ".json?prjCoordSys={\"epsgCode\":" + this.wkid + "}";
            }
            else
            {
                this.Url = this.Url + ".json";
            }
        }

        //private void Request_Completed(object sender, RequestEventArgs e)
        //{
        //    //不做e.Error的判断
        //    if (e.Result != null && !string.IsNullOrEmpty(e.Result))
        //    {
        //        JsonObject json = JsonObject.Parse(e.Result);
        //        GetMapStatusResult result = GetMapStatusResult.FromJson(json);
        //        lastResult = result;
        //        GetMapStatusEventArgs args = new GetMapStatusEventArgs(result, e.Result, e.UserState);
        //        OnProcessCompleted(args);
        //    }
        //}
        ///// <summary>${REST_GetMapStatusService_event_processCompleted_D}</summary>
        //public event EventHandler<GetMapStatusEventArgs> ProcessCompleted;

        //private void OnProcessCompleted(GetMapStatusEventArgs e)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}

        //private GetMapStatusResult lastResult;
        ///// <summary>${REST_GetMapStatusResult_attribute_lastResult_D}</summary>
        //public GetMapStatusResult LastResult
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
