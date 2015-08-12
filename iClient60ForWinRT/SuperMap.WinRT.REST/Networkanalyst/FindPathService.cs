

using SuperMap.WinRT.Service;
using SuperMap.WinRT.Core;
using System;
using SuperMap.WinRT.REST.Resources;
using System.Collections.Generic;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using System.Threading.Tasks;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindPathService_Title}</para>
    /// 	<para>${REST_FindPathService_Description}</para>
    /// </summary>
    public class FindPathService : ServiceBase
    {
        /// <summary>${REST_FindPathService_constructor_D}</summary>
        /// <overloads>
        /// 	<div>
        ///         ${REST_FindPathService_constructor_overloads_D}
        ///     </div>
        /// </overloads>
        public FindPathService()
        { }
        /// <summary>${REST_FindPathService_constructor_String_D}</summary>
        /// <param name="url">${REST_FindPathService_constructor_param_url}</param>
        public FindPathService(string url)
            : base(url)
        { }

        /// <summary>${REST_FindPathService_method_ProcessAsync_Point2D_D}</summary>
        /// <param name="parameters">${REST_FindPathService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_FindPathService_method_ProcessAsync_param_state}</param>
        public async Task<FindPathResult> ProcessAsync<T>(T parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            if (!base.Url.EndsWith("/"))
            {
                this.Url += '/';
            }

            this.Url += "path.json?debug=true&_method=GET";

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return FindPathResult.FromJson(jsonObject);
        }
        private Dictionary<string, string> GetParameters<T>(T parameters)
        {
            if (parameters is FindPathParameters<Point2D>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindPathParameters<Point2D>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindPathParameters<Point2D>).Nodes.Count; i++)
                    {
                        temp.Add(string.Format("{0}", JsonHelper.FromPoint2D((parameters as FindPathParameters<Point2D>).Nodes[i])));
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("hasLeastEdgeCount", (parameters as FindPathParameters<Point2D>).HasLeastEdgeCount.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindPathParameters<Point2D>).Parameter));

                return dictionary;
            }
            else if (parameters is FindPathParameters<int>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindPathParameters<int>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindPathParameters<int>).Nodes.Count; i++)
                    {
                        temp.Add((parameters as FindPathParameters<int>).Nodes[i].ToString());
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("hasLeastEdgeCount", (parameters as FindPathParameters<int>).HasLeastEdgeCount.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindPathParameters<int>).Parameter));

                return dictionary;
            }
            else
            {
                throw new ArgumentException("参数只能为FindPathParameters<int>或者FindPathParameters<Point2D>类型！");
            }
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    FindPathResult result = FindPathResult.FromJson(jsonObject);
        //    LastResult = result;
        //    FindPathEventArgs args = new FindPathEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(FindPathEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_FindPathService_event_processCompleted_D}</summary>

        //public event EventHandler<FindPathEventArgs> ProcessCompleted;

        //private FindPathResult lastResult;
        ///// <summary>${REST_FindPathService_attribute_lastResult_D}</summary>
        //public FindPathResult LastResult
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
