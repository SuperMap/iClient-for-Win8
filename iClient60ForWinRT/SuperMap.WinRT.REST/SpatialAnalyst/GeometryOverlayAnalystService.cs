using System;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryOverlayAnalystService_Title}</para>
    /// 	<para>${REST_GeometryOverlayAnalystService_Description}</para>
    /// </summary>
    public class GeometryOverlayAnalystService : ServiceBase
    {
        /// <summary>${REST_GeometryOverlayAnalystService_constructor_D}</summary>
        /// <overloads>${REST_GeometryOverlayAnalystService_constructor_overloads_D}</overloads>
        public GeometryOverlayAnalystService()
        {
        }
        /// <summary>${REST_GeometryOverlayAnalystService_constructor_String_D}</summary>
        /// <param name="url">${REST_GeometryOverlayAnalystService_constructor_param_url}</param>
        public GeometryOverlayAnalystService(string url)
            : base(url)
        { }

        /// <summary>${REST_GeometryOverlayAnalystService_method_ProcessAsync_D}</summary>
        /// <param name="Overlayparams">${REST_GeometryOverlayAnalystService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_GeometryOverlayAnalystService_method_ProcessAsync_param_state}</param>
        public async Task<GeometryOverlayAnalystResult> ProcessAsync(GeometryOverlayAnalystParameters Overlayparams)
        {
            GenerateAbsoluteUrl(Overlayparams);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(Overlayparams), true, false, false);
            JsonObject json = JsonObject.Parse(result);

            return GeometryOverlayAnalystResult.FromJson(json["resultGeometry"].GetObjectEx());
        }

        private void GenerateAbsoluteUrl(GeometryOverlayAnalystParameters Overlayparams)
        {
            if (Overlayparams == null)
            {
                throw new ArgumentNullException("请求服务参数为空！");
            }

            //http://192.168.11.154:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst/geometry/overlay
            if (this.Url.EndsWith("/"))
            {
                this.Url += "geometry/overlay.json?debug=true&returnContent=true";
            }
            else
            {
                this.Url += "/geometry/overlay.json?debug=true&returnContent=true";
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(GeometryOverlayAnalystParameters parameters)
        {
            return GeometryOverlayAnalystParameters.ToDictionary((GeometryOverlayAnalystParameters)parameters);
        }

        //private void OverlayAnalystService_Complated(object sender, RequestEventArgs args)
        //{
        //    JsonObject json = JsonObject.Parse(args.Result);

        //    GeometryOverlayAnalystResult result = GeometryOverlayAnalystResult.FromJson(json["resultGeometry"].GetObjectEx());
        //    lastResult = result;
        //    GeometryOverlayAnalystArgs e = new GeometryOverlayAnalystArgs(result, args.Result, args.UserState);

        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}
        ///// <summary>${REST_GeometryOverlayAnalystService_event_processCompleted_D}</summary>

        //public event EventHandler<GeometryOverlayAnalystArgs> ProcessCompleted;

        //private GeometryOverlayAnalystResult lastResult;
        ///// <summary>${REST_GeometryOverlayAnalystService_lastResult_D}</summary>
        //public GeometryOverlayAnalystResult LastResult
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
