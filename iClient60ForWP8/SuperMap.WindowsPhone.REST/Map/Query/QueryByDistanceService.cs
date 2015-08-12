using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WindowsPhone.REST.Resources;
using SuperMap.WindowsPhone.Service;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_QueryByDistanceService_Tile}</para>
    /// 	<para>${WP_REST_QueryByDistanceService_Description}</para>
    /// </summary>
    public class QueryByDistanceService : ServiceBase
    {

        /// <summary>${WP_REST_QueryByDistanceService_constructor_None_D}</summary>
        /// <overloads>${WP_REST_QueryByDistanceService_constructor_overloads}</overloads>
        public QueryByDistanceService() { }

        /// <summary>${WP_REST_QueryByDistanceService_string_constructor}</summary>
        public QueryByDistanceService(string url) : base(url) { }

        /// <summary>${WP_REST_QueryByDistanceService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${WP_REST_QueryByDistanceService_method_ProcessAsync_param_parameter}</param>
        public async Task<QueryResult> ProcessAsync(QueryByDistanceParameters parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            if (!base.Url.EndsWith("/"))
            {
                base.Url += '/';
            }
            base.Url += string.Format("queryResults.json?returnContent={0}&returnCustomResult={1}", parameter.ReturnContent.ToString(CultureInfo.InvariantCulture).ToLower(), parameter.ReturnCustomResult.ToString(CultureInfo.InvariantCulture).ToLower());
            if (parameter.IsNearest)
            {
                parameter.QueryMode = QueryMode.FindNearest;
            }
            else
            {
                parameter.QueryMode = QueryMode.DistanceQuery;
            }
            
            string jsonParam = JsonConvert.SerializeObject(parameter);
            var resultStr = await base.SubmitRequest(base.Url, HttpRequestMethod.POST, jsonParam);
            QueryResult result = null;
            if (!parameter.ReturnContent)
            {
                result = new QueryResult();
                result.ResourceInfo = JsonConvert.DeserializeObject<ResourceInfo>(resultStr);
            }
            else
            {
                result = JsonConvert.DeserializeObject<QueryResult>(resultStr);
            }
            LastResult = result;
            return result;
        }

        private QueryResult lastResult;


        /// <summary>
        /// ${WP_REST_Query_QueryService_attribute_LastResult_D}
        /// </summary>
        public QueryResult LastResult
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
