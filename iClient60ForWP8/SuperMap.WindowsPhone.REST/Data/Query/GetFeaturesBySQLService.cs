﻿using SuperMap.WindowsPhone.Service;
using System;
using SuperMap.WindowsPhone.REST.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST.Data
{
    /// <summary>
    /// 	<para>${WP_REST_GetFeaturesBySQLService_Title}</para>
    /// 	<para>${WP_REST_GetFeaturesBySQLService_Description}</para>
    /// </summary>
    public class GetFeaturesBySQLService : ServiceBase
    {
        /// <summary>${WP_REST_GetFeaturesBySQLService_constructor_D}</summary>
        /// <overloads>${WP_REST_GetFeaturesBySQLService_constructor_overloads_D}</overloads>
        public GetFeaturesBySQLService()
        {
        }
        /// <summary>${WP_REST_GetFeaturesBySQLService_constructor_String_D}</summary>
        /// <param name="url">${WP_REST_GetFeaturesBySQLService_constructor_param_url}</param>
        public GetFeaturesBySQLService(string url)
            : base(url)
        {
        }

        /// <summary>${WP_REST_GetFeaturesBySQLService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${WP_REST_GetFeaturesBySQLService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${WP_REST_GetFeaturesBySQLService_method_ProcessAsync_param_state}</param>
        public async Task<GetFeaturesResult> ProcessAsync(GetFeaturesBySQLParameters parameters)
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
            parameters.GetFeatureMode = GetFeatureMode.SQL;
            var resultStr = await base.SubmitRequest(base.Url, HttpRequestMethod.POST,JsonConvert.SerializeObject(parameters));
            GetFeaturesResult result = JsonConvert.DeserializeObject<GetFeaturesResult>(resultStr);
            LastResult = result;
            return result;
        }

        private GetFeaturesResult lastResult;
        /// <summary>${WP_REST_GetFeaturesBySQLService_attribute_LastResult_D}</summary>
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
