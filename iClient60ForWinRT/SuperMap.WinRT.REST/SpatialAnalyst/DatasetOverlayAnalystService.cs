using System;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetOverlayAnalystService_Title}</para>
    /// 	<para>${REST_DatasetOverlayAnalystService_Description}</para>
    /// </summary>
    public class DatasetOverlayAnalystService : ServiceBase
    {
        /// <summary>${REST_DatasetOverlayAnalystService_constructor_D}</summary>
        /// <overloads>${REST_DatasetOverlayAnalystService_constructor_overloads_D}</overloads>
        public DatasetOverlayAnalystService()
        {
        }
        /// <summary>${REST_DatasetOverlayAnalystService_constructor_String_D}</summary>
        /// <param name="url">${REST_DatasetOverlayAnalystService_constructor_param_url}</param>
        public DatasetOverlayAnalystService(string url)
            : base(url)
        { }

        /// <summary>${REST_DatasetOverlayAnalystService_method_ProcessAsync_D}</summary>
        /// <param name="Overlayparams">${REST_DatasetOverlayAnalystService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_DatasetOverlayAnalystService_method_ProcessAsync_param_state}</param>
        public async Task<DatasetOverlayAnalystResult> ProcessAsync(DatasetOverlayAnalystParameters Overlayparams)
        {
            GenerateAbsoluteUrl(Overlayparams);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(Overlayparams), true, false, false);
            JsonObject jsonResult = JsonObject.Parse(result);
            return DatasetOverlayAnalystResult.FromJson(jsonResult["recordset"].GetObjectEx());
        }

        private void GenerateAbsoluteUrl(DatasetOverlayAnalystParameters Overlayparams)
        {
            if (Overlayparams == null)
            {
                throw new ArgumentNullException("请求服务参数为空！");
            }

            if (string.IsNullOrEmpty(((DatasetOverlayAnalystParameters)Overlayparams).SourceDataset) || string.IsNullOrEmpty(((DatasetOverlayAnalystParameters)Overlayparams).SourceDataset))
            {
                throw new ArgumentNullException("数据集参数为空");
            }

            if (this.Url == null)
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            //http://192.168.11.154:8090/iserver/services/spatialanalyst-sample/restjsr/spatialanalyst/datasets/SamplesP@Interpolation/overlay
            if (this.Url.EndsWith("/"))
            {
                this.Url += "datasets/" + ((DatasetOverlayAnalystParameters)Overlayparams).SourceDataset + "/overlay.json?returnContent=true&debug=true";
            }
            else
            {
                this.Url += "/datasets/" + ((DatasetOverlayAnalystParameters)Overlayparams).SourceDataset + "/overlay.json?returnContent=true&debug=true";
            }
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(DatasetOverlayAnalystParameters parameters)
        {
            return DatasetOverlayAnalystParameters.ToDictionary((DatasetOverlayAnalystParameters)parameters);
        }

        //private void OverlayAnalystService_Complated(object sender, RequestEventArgs args)
        //{
        //    JsonObject jsonResult = JsonObject.Parse(args.Result);
        //    DatasetOverlayAnalystResult result = DatasetOverlayAnalystResult.FromJson(jsonResult["recordset"].GetObjectEx());
        //    lastResult = result;
        //    DatasetOverlayAnalystArgs e = new DatasetOverlayAnalystArgs(result, args.Result, args.UserState);

        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}
        ///// <summary>${REST_DatasetOverlayAnalystService_event_processCompleted_D}</summary>

        //public event EventHandler<DatasetOverlayAnalystArgs> ProcessCompleted;

        //private DatasetOverlayAnalystResult lastResult;
        ///// <summary>${REST_DatasetOverlayAnalystService_lastResult_D}</summary>
        //public DatasetOverlayAnalystResult LastResult
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
