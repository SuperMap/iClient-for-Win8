using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;
using SuperMap.WindowsPhone.Service;
using System.Threading.Tasks;
using SuperMap.WindowsPhone.REST.Resources;
using System.Globalization;
using Newtonsoft.Json;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_Query_QueryByBoundsService_Title}</para>
    /// 	<para>${REST_Query_QueryByBoundsService_Description}</para>
    /// </summary>
    public class QueryByBoundsService : ServiceBase
    {
        /// <summary>${WP_REST_Query_QueryByBoundsService_constructor_None_D}</summary>
        /// <overloads>${REST_Query_QueryByBoundsService_constructor_overloads}</overloads>
        public QueryByBoundsService()
        { }
        /// <summary>${WP_REST_Query_QueryByBoundsService_constructor_string_D}</summary>
        /// <param name="url">${REST_Query_QueryByBoundsService_constructor_string_param_url}</param>
        public QueryByBoundsService(string url)
            : base(url)
        { }

        /// <summary>${WP_REST_Query_QueryByBoundsService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${WP_REST_Query_QueryByBoundsService_method_ProcessAsync_param_parameter}</param>
        public async Task<QueryResult> ProcessAsync(QueryByBoundsParameters parameter)
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
            parameter.QueryMode = QueryMode.BoundsQuery;
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
