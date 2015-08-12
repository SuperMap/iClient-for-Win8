

using Newtonsoft.Json;
using SuperMap.WindowsPhone.REST.Resources;
using SuperMap.WindowsPhone.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_QueryBySQlService_Tile}</para>
    /// 	<para>${WP_REST_QueryBySQlService_Description}</para>
    /// </summary>
    public class QueryBySQLService : ServiceBase
    {
        /// <summary>${WP_REST_QueryBySQlService_constructor_None_D}</summary>
        /// <overloads>${WP_REST_QueryBySQlService_constructor_overloads}</overloads>
        public QueryBySQLService() { }

        /// <summary>${WP_REST_QueryBySQlService_constructor_string_url}</summary>
        /// <param name="url">${WP_REST_QueryBySQlService_string_constructor_param_url}</param>
        public QueryBySQLService(string url) : base(url) { }

        /// <summary>${REST_QueryBySQlService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_QueryBySQlService_method_ProcessAsync_param_parameter}</param>
        public async Task<QueryResult> ProcessAsync(QueryBySQLParameters parameter)
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

            parameter.QueryMode = QueryMode.SqlQuery;

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
