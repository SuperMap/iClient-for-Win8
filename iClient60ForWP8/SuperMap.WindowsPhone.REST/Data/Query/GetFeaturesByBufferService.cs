using SuperMap.WindowsPhone.Service;
using System;
using System.Collections.Generic;
using SuperMap.WindowsPhone.REST.Resources;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesByBufferService_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesByBufferService_Description}</para>
    /// </summary>
    public class GetFeaturesByBufferService : ServiceBase
    {
        /// <summary>${WP_REST_GetFeaturesByBufferService_constructor_D}</summary>
        /// <overloads>${WP_REST_GetFeaturesByBufferService_constructor_overloads_D}</overloads>
        public GetFeaturesByBufferService()
        {
        }

        /// <summary>${WP_REST_GetFeaturesByBufferService_constructor_String_D}</summary>
        /// <param name="url">${WP_REST_GetFeaturesByBufferService_constructor_param_url}</param>
        public GetFeaturesByBufferService(string url)
            : base(url)
        {
        }        

        /// <summary>${WP_REST_GetFeaturesByBufferService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${WP_REST_GetFeaturesByBufferService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${WP_REST_GetFeaturesByBufferService_method_ProcessAsync_param_state}</param>
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
            base.Url += string.Format(".json?returnContent=true&fromIndex={0}&toIndex={1}", parameters.FromIndex, parameters.ToIndex);
            if (!string.IsNullOrEmpty(parameters.AttributeFilter))
            {
                parameters.GetFeatureMode = GetFeatureMode.BUFFER_ATTRIBUTEFILTER;
            }
            else
            {
                parameters.GetFeatureMode = GetFeatureMode.BUFFER;
            }
            string jsonParam = JsonConvert.SerializeObject(parameters);
            var resultStr = await base.SubmitRequest(base.Url, HttpRequestMethod.POST, jsonParam);
            GetFeaturesResult result=JsonConvert.DeserializeObject<GetFeaturesResult>(resultStr);
            LastResult = result;
            return result;
        }

        private GetFeaturesResult lastResult;
        /// <summary>${WP_REST_GetFeaturesByBufferService_attribute_LastResult_D}</summary>
        public GetFeaturesResult LastResult
        {
            get
            {
                return lastResult;
            }
            private set
            {
                lastResult = value;
                base.OnPropertyChanged("LastResult");
            }
        }
    }
}
