using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Query_QueryService_Title}</para>
    /// 	<para>${REST_Query_QueryService_Description}</para>
    /// </summary>
    public abstract class QueryService : ServiceBase
    {
        /// <summary>${REST_Query_QueryService_constructor_None_D}</summary>
        /// <overloads>${REST_Query_QueryService_constructor_overloads}</overloads>
        public QueryService() { }

        /// <summary>${REST_Query_QueryService_constructor_string_D}</summary>
        /// <param name="url">${REST_Query_QueryService_constructor_string_param_url}</param>
        public QueryService(string url)
            : base(url)
        { }

        /// <summary>${REST_Query_QueryService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_Query_QueryService_ProcessAsync_param_parameters}</param>
        /// <param name="state">${REST_Query_QueryService_ProcessAsync_param_state}</param>
        public async Task<QueryResult> ProcessAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            if (!this.Url.EndsWith("/"))
            {
                this.Url += '/';
            }
            //参数必须放在 URI 中
            this.Url += string.Format("queryResults.json?returnContent={0}&debug=true", parameters.ReturnContent.ToString().ToLower());

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return QueryResult.FromJson(jsonObject);
        }
        /// <summary>${REST_Query_QueryService_method_GetParameters_D}</summary>
        /// <param name="parameters">${REST_Query_QueryService_method_GetParameters_parameters}</param>
        protected abstract Dictionary<string, string> GetParameters(QueryParameters parameters);


        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    QueryResult result = QueryResult.FromJson(jsonObject);
        //    LastResult = result;
        //    QueryEventArgs args = new QueryEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(QueryEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        //Application.Current.RootVisual.Dispatcher.BeginInvoke(ProcessCompleted, new object[] { this, args });
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_Query_QueryService_event_ProcessCompleted_D}</summary>
        //public event EventHandler<QueryEventArgs> ProcessCompleted;


        //private QueryResult lastResult;
        ///// <summary>${REST_Query_QueryService_attribute_LastResult_D}</summary>
        //public QueryResult LastResult
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
