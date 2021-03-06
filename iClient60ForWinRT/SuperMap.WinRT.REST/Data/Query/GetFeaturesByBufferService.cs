﻿

using SuperMap.WinRT.Service;
using System;
using System.Collections.Generic;
using SuperMap.WinRT.REST.Resources;
using Windows.Data.Json;
using System.Threading.Tasks;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesByBufferService_Title}</para>
    /// 	<para>${REST_GetFeaturesByBufferService_Description}</para>
    /// </summary>
    public class GetFeaturesByBufferService : ServiceBase
    {
        /// <summary>${REST_GetFeaturesByBufferService_constructor_D}</summary>
        /// <overloads>${REST_GetFeaturesByBufferService_constructor_overloads_D}</overloads>
        public GetFeaturesByBufferService()
        {
        }

        /// <summary>${REST_GetFeaturesByBufferService_constructor_String_D}</summary>
        /// <param name="url">${REST_GetFeaturesByBufferService_constructor_param_url}</param>
        public GetFeaturesByBufferService(string url)
            : base(url)
        {
        }        

        /// <summary>${REST_GetFeaturesByBufferService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_GetFeaturesByBufferService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_GetFeaturesByBufferService_method_ProcessAsync_param_state}</param>
        public async Task<GetFeaturesResult> ProcessAsync(GetFeaturesByBufferParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }
            //base.Url += ".json?returnContent=true";
            base.Url += string.Format(".json?returnContent=true&debug=true&fromIndex={0}&toIndex={1}", parameters.FromIndex, parameters.ToIndex);

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return GetFeaturesResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters(GetFeaturesByBufferParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(parameters.AttributeFilter))
            {
                dictionary.Add("getFeatureMode", "\"BUFFER\"");
            }
            else
            {
                dictionary.Add("getFeatureMode", "\"BUFFER_ATTRIBUTEFILTER\"");
                dictionary.Add("attributeFilter", parameters.AttributeFilter);
            }
            if (parameters.DatasetNames != null && parameters.DatasetNames.Count > 0)
            {
                string jsonDatasetNames = "[";
                List<string> list = new List<string>();
                for (int i = 0; i < parameters.DatasetNames.Count; i++)
                {
                    list.Add(string.Format("\"{0}\"", parameters.DatasetNames[i]));
                }
                jsonDatasetNames += string.Join(",", list.ToArray());
                jsonDatasetNames += "]";

                dictionary.Add("datasetNames", jsonDatasetNames);
            }

            if (parameters.Geometry != null)
            {
                dictionary.Add("geometry", ServerGeometry.ToJson(Bridge.ToServerGeometry(parameters.Geometry)));
            }
            else
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            dictionary.Add("bufferDistance", parameters.BufferDistance.ToString());

            if (parameters.Fields != null && parameters.Fields.Count > 0)
            {
                FilterParameter fp = new FilterParameter();
                fp.Fields = parameters.Fields;
                dictionary.Add("queryParameter", FilterParameter.ToJson(fp));
            }

            return dictionary;
        }
        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    GetFeaturesResult result = GetFeaturesResult.FromJson(jsonObject);
        //    LastResult = result;
        //    GetFeaturesEventArgs args = new GetFeaturesEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(GetFeaturesEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_GetFeaturesByBufferService_event_processCompleted_D}</summary>

        //public event EventHandler<GetFeaturesEventArgs> ProcessCompleted;


        //private GetFeaturesResult lastResult;
        ///// <summary>${REST_GetFeaturesByBufferService_attribute_LastResult_D}</summary>
        //public GetFeaturesResult LastResult
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
