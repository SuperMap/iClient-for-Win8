using System;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_ExtractService_Title}</para>
    /// 	<para>${REST_ExtractService_Description}</para>
    /// </summary>
    public class SurfaceAnalystService : ServiceBase
    {
        /// <summary>${REST_ExtractService_constructor_None_D}</summary>
        /// <overloads>${REST_ExtractService_constructor_overloads_D}</overloads>
        public SurfaceAnalystService()
        { }
        /// <summary>${REST_ExtractService_constructor_String_D}</summary>
        /// <param name="url">${REST_ExtractService_constructor_param_url}</param>
        public SurfaceAnalystService(string url)
            : base(url)
        { }

        /// <summary>${REST_ExtractService_method_ProcessAsync_D}</summary>
        /// <param name="surfaceAnalystParameters">${REST_ExtractService_method_ProcessAsync_param_parameter}</param>
        /// <param name="state">${REST_ExtractService_method_ProcessAsync_param_state}</param>
        public async Task<SurfaceAnalystResult> ProcessAsync(SurfaceAnalystParameters surfaceAnalystParameters)
        {
            GenerateAbsoluteUrl(surfaceAnalystParameters);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(surfaceAnalystParameters), true, false, false);
            JsonObject json = JsonObject.Parse(result);
            return SurfaceAnalystResult.FromJson(json["recordset"].GetObjectEx());
        }

        private void GenerateAbsoluteUrl(SurfaceAnalystParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("请求服务参数为空！");
            }

            if (parameters is DatasetSurfaceAnalystParameters)
            {
                if (string.IsNullOrEmpty(((DatasetSurfaceAnalystParameters)parameters).Dataset) || string.IsNullOrEmpty(((DatasetSurfaceAnalystParameters)parameters).Dataset))
                {
                    throw new ArgumentNullException("数据集参数为空");
                }

                if (this.Url == null)
                {
                    throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
                }

                //http://192.168.11.154:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst/datasets/Temp5000@Interpolation/isoline
                if (this.Url.EndsWith("/"))
                {
                    this.Url += "datasets/" + ((DatasetSurfaceAnalystParameters)parameters).Dataset + "/" + ((DatasetSurfaceAnalystParameters)parameters).SurfaceAnalystMethod.ToString().ToLower() + ".json?returnContent=true&debug=true";
                }
                else
                {
                    this.Url += "/datasets/" + ((DatasetSurfaceAnalystParameters)parameters).Dataset + "/" + ((DatasetSurfaceAnalystParameters)parameters).SurfaceAnalystMethod.ToString().ToLower() + ".json?returnContent=true&debug=true";
                }
            }
            else if (parameters is GeometrySurfaceAnalystParameters)
            {
                //http://192.168.11.154:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst/geometry/isoline
                if (this.Url.EndsWith("/"))
                {
                    this.Url += "geometry/" + ((GeometrySurfaceAnalystParameters)parameters).SurfaceAnalystMethod.ToString().ToLower() + ".json?returnContent=true&debug=true";
                }
                else
                {
                    this.Url += "/geometry/" + ((GeometrySurfaceAnalystParameters)parameters).SurfaceAnalystMethod.ToString().ToLower() + ".json?returnContent=true&debug=true";
                }
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(SurfaceAnalystParameters parameters)
        {
            if (parameters is DatasetSurfaceAnalystParameters)
            {
                return DatasetSurfaceAnalystParameters.ToDictionary((DatasetSurfaceAnalystParameters)parameters);
            }
            else if (parameters is GeometrySurfaceAnalystParameters)
            {
                return GeometrySurfaceAnalystParameters.ToDictionary((GeometrySurfaceAnalystParameters)parameters);
            }
            return new System.Collections.Generic.Dictionary<string, string>();
        }

        //private void SurfaceAnalystService_Complated(object sender, RequestEventArgs args)
        //{
        //    JsonObject json = JsonObject.Parse(args.Result);
        //    SurfaceAnalystResult result = SurfaceAnalystResult.FromJson(json["recordset"].GetObjectEx());
        //    SurfaceAnalystEventArgs e = new SurfaceAnalystEventArgs(result, args.Result, args.UserState);
        //    lastResult = result;
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}
        ///// <summary>${REST_ExtractService_event_processCompleted_D}</summary>

        //public event EventHandler<SurfaceAnalystEventArgs> ProcessCompleted;

        //private SurfaceAnalystResult lastResult;
        ///// <summary>${REST_ExtractService_lastResult_D}</summary>
        //public SurfaceAnalystResult LastResult
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
