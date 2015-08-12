using System.Collections.Generic;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.REST.Resources;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeService_Title}</para>
    /// 	<para>${REST_ThemeService_Description}</para>
    /// </summary>
    public class ThemeService : ServiceBase
    {
        private string serviceMapName;
        /// <summary>${REST_ThemeService_constructor_D}</summary>
        public ThemeService()
        {
        }

        /// <summary>${REST_ThemeService_constructor_param_url}</summary>
        public ThemeService(string url)
            : base(url)
        {
            serviceMapName = GetServiceMapName();
        }

        private string GetServiceMapName()
        {
            //http://192.168.11.11:8090/iserver/services/map-world/rest/maps/World Map
            string mapName = this.Url;

            if (this.Url.EndsWith("/"))
            {
                mapName.TrimEnd('/');
            }

            mapName = mapName.Substring(mapName.LastIndexOf('/') + 1);
            return mapName;
        }

        /// <summary>${REST_ThemeService_method_ProcessAsync_param_Parameters}</summary>
        public async Task<ThemeResult> ProcessAsync(ThemeParameters themeParameters)
        {
            ValidationUrlAndParametres(themeParameters);
            CombineAbsolutePath();

            var result = await base.SubmitRequest(base.Url, GetDictionaryParameters(themeParameters), true, false, true);
            return ThemeResult.FromJson(result);
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(ThemeParameters themeParameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("type", "\"UGC\"");

            dictionary.Add("subLayers", ThemeParameters.ToJson(themeParameters));
            dictionary.Add("name", "\"" + serviceMapName + "\"");

            return dictionary;
        }

        private void ValidationUrlAndParametres(ThemeParameters themeParameters)
        {
            if (themeParameters == null)
            {
                throw new ArgumentNullException("请求服务参数为空！");
            }

            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }
        }

        private void CombineAbsolutePath()
        {
            if (this.Url.EndsWith("/"))
            {
                this.Url += "tempLayersSet.json?debug=true&";
            }
            else
            {
                this.Url += "/tempLayersSet.json?debug=true&";
            }
        }

        //private void ThemeRequest_Complated(object sender, RequestEventArgs e)
        //{
        //    ThemeResult result = ThemeResult.FromJson(e.Result);
        //    lastResult = result;
        //    ThemeEventArgs args = new ThemeEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        ///// <summary>${REST_ThemeService_event_ProcessCompleted_D}</summary>
        //public event EventHandler<ThemeEventArgs> ProcessCompleted;

        //private void OnProcessCompleted(ThemeEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, args);
        //    }
        //}

        //private ThemeResult lastResult;
        ///// <summary>${REST_ThemeService_attribute_lastResult_D}</summary>
        //public ThemeResult LastResult
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
