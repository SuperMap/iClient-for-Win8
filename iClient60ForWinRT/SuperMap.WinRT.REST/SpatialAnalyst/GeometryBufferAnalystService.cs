using System;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryBufferAnalystService_Title}</para>
    /// 	<para>${REST_GeometryBufferAnalystService_Description}</para>
    /// </summary>
    public class GeometryBufferAnalystService : ServiceBase
    {
        /// <summary>${REST_GeometryBufferAnalystService_constructor_None_D}</summary>
        /// <overloads>${REST_GeometryBufferAnalystService_constructor_overloads_D}</overloads>
        public GeometryBufferAnalystService()
        { }
        /// <summary>${REST_GeometryBufferAnalystService_constructor_String_D}</summary>
        /// <param name="url">${REST_GeometryBufferAnalystService_constructor_param_url}</param>
        public GeometryBufferAnalystService(string url)
            : base(url)
        { }
        /// <summary>${REST_GeometryBufferAnalystService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_GeometryBufferAnalystService_method_ProcessAsync_param_parameter}</param>
        /// <param name="state">${REST_GeometryBufferAnalystService_method_ProcessAsync_param_state}</param>
        public async Task<GeometryBufferAnalystResult> ProcessAsync(GeometryBufferAnalystParameters parameters)
        {
            GenerateAbsoluteUrl(parameters);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(parameters), true, false, false);
            JsonObject resultJson = JsonObject.Parse(result);
            return GeometryBufferAnalystResult.FromJson(resultJson);
        }

        private void GenerateAbsoluteUrl(GeometryBufferAnalystParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("请求服务参数为空！");
            }

            //http://192.168.11.154:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst/geometry/buffer
            if (this.Url.EndsWith("/"))
            {
                this.Url += "geometry/buffer.json?debug=true&returnContent=true";
            }
            else
            {
                this.Url += "/geometry/buffer.json?debug=true&returnContent=true";
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(GeometryBufferAnalystParameters parameters)
        {
            return GeometryBufferAnalystParameters.ToDictionary(parameters);
        }

        //private void BufferAnalystService_Complated(object sender, RequestEventArgs args)
        //{
        //    GeometryBufferAnalystArgs e = new GeometryBufferAnalystArgs(CheckResult(args), args.Result, args.UserState);
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}

        //private GeometryBufferAnalystResult CheckResult(RequestEventArgs args)
        //{
        //    JsonObject resultJson = JsonObject.Parse(args.Result);
        //    GeometryBufferAnalystResult result = GeometryBufferAnalystResult.FromJson(resultJson);
        //    lastResult = result;
        //    return result;
        //}
        ///// <summary>${REST_DatasetBufferAnalystService_event_processCompleted_D}</summary>

        //public event EventHandler<GeometryBufferAnalystArgs> ProcessCompleted;

        //private GeometryBufferAnalystResult lastResult;
        ///// <summary>${REST_GeometryBufferAnalystService_lastResult_D}</summary>
        //public GeometryBufferAnalystResult LastResult
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
