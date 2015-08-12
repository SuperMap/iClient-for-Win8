using System.Collections.Generic;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.REST.Resources;
using System.Text;
using System.IO;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${iServer6_SetLayerStyleService_Title}</para>
    /// 	<para>${iServer6_SetLayerStyleService_Description}</para>
    /// </summary>
    public class SetLayerStyleService : ServiceBase
    {
        private string mapUrl = string.Empty; //http://192.168.11.154:8090/iserver/services/map-jingjin/rest/maps/京津地区人口分布图/tempLayersSet
        private string mapName = string.Empty;
        private SetLayerStyleParameters requestParameters;
        private JsonObject tempLayerInfoJson;
        private string resourceID;

        /// <summary>${iServer6_SetLayerStyleService_constructor_None_D}</summary>
        /// <overloads>${iServer6_SetLayerStyleService_constructor_overloads}</overloads>
        public SetLayerStyleService()
        {
        }
        /// <summary>${iServer6_SetLayerStyleService_constructor_String_D}</summary>
        /// <param name="url">${iServer6_SetLayerStyleService_constructor_String_param_url}</param>
        public SetLayerStyleService(string url)
            : base(url)
        {
        }

        /// <summary>${iServer6_SetLayerStyleService_method_processAsync_D}</summary>
        /// <param name="parameters">${iServer6_SetLayerStyleService_method_processAsync_param_parameters}</param>
        /// <param name="state">${iServer6_SetLayerStyleService_method_processAsync_param_state}</param>
        public async Task<SetLayerResult> ProcessAsync(SetLayerStyleParameters parameters)
        {
            mapUrl = base.Url;

            requestParameters = parameters;
            ValidationUrlAndParametres();
            resourceID = requestParameters.ResourceID;
            GetMapName();

            if (!string.IsNullOrEmpty(resourceID))//直接put
            {
                var json = await base.SubmitRequest(GetPutRequestUrl(), requestParameters.Style.ToDictionary(), true, false, false);
                SetLayerResult setLayerResult = SetLayerResult.FromJson(json);
                setLayerResult.NewResourceID = resourceID.ToString();
                return setLayerResult;
            }
            else   //先post再put
            {
                string getTempLayerSetUrl = Url;
                getTempLayerSetUrl += "/tempLayersSet.json?holdTime=" + parameters.HoldTime;
                HttpWebRequest request = HttpWebRequest.CreateHttp(getTempLayerSetUrl);
                request.Headers["Content-Type"] = "application/x-www-form-urlencoded;charset=UTF-8";
                request.Method = "POST";
                try
                {
                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string result = await reader.ReadToEndAsync();
                    reader.Dispose();
                    return await PostRequest_Complated(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void ValidationUrlAndParametres()
        {
            if (this.Url == null)
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }
            if (requestParameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
        }

        private void GetMapName()
        {
            if (mapUrl.EndsWith("/"))
            {
                this.mapUrl = mapUrl.TrimEnd(new char[] { '/' });
            }

            int index = this.mapUrl.LastIndexOf('/');
            mapName = this.mapUrl.Substring(index + 1);
        }

        private async Task<SetLayerResult> PostRequest_Complated(string result)
        {
            if (result.Contains("succeed") && result.Contains("newResourceID"))
            {
                tempLayerInfoJson = JsonObject.Parse(result);
                if (tempLayerInfoJson["succeed"].GetBooleanEx())
                {
                    resourceID = tempLayerInfoJson["newResourceID"].GetStringEx();

                    //执行put
                    var json = await base.SubmitRequest(GetPutRequestUrl(), requestParameters.Style.ToDictionary(), true, false, false);
                    SetLayerResult setLayerResult = SetLayerResult.FromJson(json);
                    setLayerResult.NewResourceID = resourceID.ToString();
                    return setLayerResult;
                }
            }
            return null;
        }

        private string GetPutRequestUrl()
        {
            //http://192.168.11.153:8090/iserver/services/map-world/rest/maps/World Map/tempLayersSet/1/Ocean@World@@World Map/style.rjson
            string secondUrl = this.mapUrl + "/tempLayersSet/" + resourceID + "/" + requestParameters.LayerName + "@@" + mapName + "/style.json?_method=PUT&reference=" + resourceID + "&elementRemain=true";
            return secondUrl;
        }

        //private void SetLayerStatusRequest_Complated(object sender, RequestEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        SetLayerResult result = SetLayerResult.FromJson(e.Result);
        //        result.NewResourceID = resourceID.ToString();
        //        lastResult = result;
        //        SetLayerEventArgs args = new SetLayerEventArgs(result, e.Result, e.UserState);
        //        OnProcessCompleted(args);
        //    }
        //}
        ///// <summary>${iServer6_SetLayerStyleService_event_processCompleted_D}</summary>
        //public event EventHandler<SetLayerEventArgs> ProcessCompleted;

        //private void OnProcessCompleted(SetLayerEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, args);
        //    }
        //}

        //private SetLayerResult lastResult;
        ///// <summary>${iServer6_SetLayerStyleService_event_processCompleted_D}</summary>
        //public SetLayerResult LastResult
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

