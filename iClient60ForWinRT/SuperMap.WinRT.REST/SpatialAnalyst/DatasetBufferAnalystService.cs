using System;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetBufferAnalystService_Title}</para>
    /// 	<para>${REST_DatasetBufferAnalystService_Description}</para>
    /// </summary>
    public class DatasetBufferAnalystService : ServiceBase
    {
        /// <summary>${REST_DatasetBufferAnalystService_constructor_None_D}</summary>
        /// <overloads>${REST_DatasetBufferAnalystService_constructor_overloads_D}</overloads>
        public DatasetBufferAnalystService()
        { }
        /// <summary>${REST_DatasetBufferAnalystService_constructor_String_D}</summary>
        /// <param name="url">${REST_DatasetBufferAnalystService_constructor_param_url}</param>
        public DatasetBufferAnalystService(string url)
            : base(url)
        { }

        /// <summary>${REST_DatasetBufferAnalystService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_DatasetBufferAnalystService_method_ProcessAsync_param_parameter}</param>
        /// <param name="state">${REST_DatasetBufferAnalystService_method_ProcessAsync_param_state}</param>
        public async Task<DatasetBufferAnalystResult> ProcessAsync(DatasetBufferAnalystParameters parameters)
        {
            GenerateAbsoluteUrl(parameters);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(parameters), true, false, false);
            JsonObject resultJson = JsonObject.Parse(result);

            return DatasetBufferAnalystResult.FromJson(resultJson["recordset"].GetObjectEx());
        }

        private void GenerateAbsoluteUrl(DatasetBufferAnalystParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("请求服务参数为空！");
            }

            if (string.IsNullOrEmpty(parameters.Dataset) || string.IsNullOrEmpty(parameters.Dataset))
            {
                throw new ArgumentNullException("数据集参数为空");
            }

            if (this.Url == null)
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            //http://192.168.11.154:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst/datasets/SamplesP@Interpolation/buffer.json
            if (this.Url.EndsWith("/"))
            {
                this.Url += "datasets/" + parameters.Dataset + "/buffer.json?debug=true&returnContent=true";
            }
            else
            {
                this.Url += "/datasets/" + parameters.Dataset + "/buffer.json?debug=true&returnContent=true";
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(DatasetBufferAnalystParameters parameters)
        {
            return DatasetBufferAnalystParameters.ToDictionary(parameters);
        }

        //private void BufferAnalystService_Complated(object sender, RequestEventArgs args)
        //{
        //    DatasetBufferAnalystArgs e = new DatasetBufferAnalystArgs(CheckResult(args), args.Result, args.UserState);
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}

        //private DatasetBufferAnalystResult CheckResult(RequestEventArgs args)
        //{
        //    JsonObject resultJson = JsonObject.Parse(args.Result);

        //    DatasetBufferAnalystResult result = DatasetBufferAnalystResult.FromJson(resultJson["recordset"].GetObjectEx());
        //    lastResult = result;
        //    return result;
        //}
        ///// <summary>${REST_DatasetBufferAnalystService_event_processCompleted_D}</summary>

        //public event EventHandler<DatasetBufferAnalystArgs> ProcessCompleted;

        //private DatasetBufferAnalystResult lastResult;
        ///// <summary>${REST_DatasetBufferAnalystService_lastResult_D}</summary>
        //public DatasetBufferAnalystResult LastResult
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
