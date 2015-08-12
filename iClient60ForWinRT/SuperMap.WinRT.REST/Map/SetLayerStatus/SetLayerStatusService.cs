using System.Collections.Generic;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.REST.Resources;
using System.Text;
using Windows.Data.Json;
using System.IO;
using SuperMap.WinRT.Utilities;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${iServer6_SetLayerStatusService_Title}</para>
    /// 	<para>${iServer6_SetLayerStatusService_Description}</para>
    /// </summary>
    public class SetLayerStatusService : ServiceBase
    {
        private string mapUrl = string.Empty;
        private string mapName = string.Empty;
        private SetLayerStatusParameters requestParameters;
        private JsonObject tempLayerInfoJson;
        private string resourceID;

        /// <summary>${iServer6_SetLayerStatusService_constructor_None_D}</summary>
        /// <overloads>${iServer6_SetLayerStatusService_constructor_overloads}</overloads>
        public SetLayerStatusService()
        {
        }
        /// <summary>${iServer6_SetLayerStatusService_constructor_String_D}</summary>
        /// <param name="url">${iServer6_SetLayerStatusService_constructor_String_param_url}</param>
        public SetLayerStatusService(string url)
            : base(url)
        {
        }

        /// <summary>${iServer6_SetLayerStatusService_method_processAsync_D}</summary>
        /// <param name="parameters">${iServer6_SetLayerStatusService_method_processAsync_param_parameters}</param>
        /// <param name="state">${iServer6_SetLayerStatusService_method_processAsync_param_state}</param>
        public async Task<SetLayerResult> ProcessAsync(SetLayerStatusParameters parameters)
        {
            mapUrl = this.Url;

            requestParameters = parameters;
            ValidationUrlAndParametres();
            resourceID = requestParameters.ResourceID;
            GetMapName();

            if (!string.IsNullOrEmpty(resourceID))//直接put
            {
                var json = await base.SubmitRequest(GetPutRequestUrl(), GetDictionaryParameters(), true, false, true);
                SetLayerResult setLayerResult = SetLayerResult.FromJson(json);
                setLayerResult.NewResourceID = resourceID.ToString();
                return setLayerResult;
            }
            else   //先post再put
            {
                string getTempLayerSetUrl = Url;
                getTempLayerSetUrl += "/tempLayersSet.json";
                HttpWebRequest request = HttpWebRequest.CreateHttp(getTempLayerSetUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
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

        private string GetPostRequestUrl()
        {
            string firstUrl = this.mapUrl + "/tempLayersSet.json?debug=true&holdTime=" + requestParameters.HoldTime.ToString();
            return firstUrl;
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
                    var json = await base.SubmitRequest(GetPutRequestUrl(), GetDictionaryParameters(), true, false, true);
                    SetLayerResult setLayerResult = SetLayerResult.FromJson(json);
                    setLayerResult.NewResourceID = resourceID.ToString();
                    return setLayerResult;
                }
            }
            return null;
        }

        private string GetPutRequestUrl()
        {
            //http://192.168.11.154:8090/iserver/services/map-world/rest/maps/World Map/tempLayersSet/7/World Map
            string secondUrl = this.mapUrl + "/tempLayersSet/" + resourceID + ".json?_method=PUT&reference=" + resourceID + "&elementRemain=true";
            return secondUrl;
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("type", "\"UGC\"");

            if (requestParameters.LayerStatusList != null && requestParameters.LayerStatusList.Count > 0)
            {
                dictionary.Add("subLayers", SetLayerStatusParameters.ToJson(requestParameters));
            }
            dictionary.Add("queryable", "false");
            dictionary.Add("name", "\"" + mapName + "\"");

            return dictionary;
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
        ///// <summary>${iServer6_SetLayerStatusService_event_processCompleted_D}</summary>
        //public event EventHandler<SetLayerEventArgs> ProcessCompleted;

        //private void OnProcessCompleted(SetLayerEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, args);
        //    }
        //}

        //private SetLayerResult lastResult;
        ///// <summary>${iServer6_SetLayerStatusService_attribute_lastResult_D}</summary>
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

