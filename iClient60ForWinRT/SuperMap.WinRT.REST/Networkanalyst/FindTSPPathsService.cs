

using System;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTSPPathsService_Title}</para>
    /// 	<para>${REST_FindTSPPathsService_Description}</para>
    /// </summary>
    public class FindTSPPathsService : ServiceBase
    {
        /// <summary>${REST_FindTSPPathsService_constructor_D}</summary>
        /// <overloads>${REST_ClosestFacilitiesService_constructor_overloads_D}</overloads>
        public FindTSPPathsService()
        { }
        /// <summary>${REST_ClosestFacilitiesService_constructor_String_D}</summary>
        /// <param name="url">${REST_FindTSPPathsService_constructor_param_url}</param>
        public FindTSPPathsService(string url)
            : base(url)
        {
        }

        /// <summary>${REST_FindTSPPathsService_method_ProcessAsync_D}</summary>
        /// <param name="state">${REST_FindTSPPathsService_method_ProcessAsync_param_state}</param>
        /// <param name="parameters">${REST_FindTSPPathsService_method_ProcessAsync_param_Parameters}</param>
        public async Task<FindTSPPathsResult> ProcessAsync<T>(T parameters)
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
            this.Url += "tsppath.json?debug=true&_method=GET";

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters),  true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return FindTSPPathsResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters<T>(T parameters)
        {
            if (parameters is FindTSPPathsParameters<int>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindTSPPathsParameters<int>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindTSPPathsParameters<int>).Nodes.Count; i++)
                    {
                        temp.Add((parameters as FindTSPPathsParameters<int>).Nodes[i].ToString());
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("endNodeAssigned", (parameters as FindTSPPathsParameters<int>).EndNodeAssigned.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindTSPPathsParameters<int>).Parameter));

                return dictionary;
            }
            else if (parameters is FindTSPPathsParameters<Point2D>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindTSPPathsParameters<Point2D>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindTSPPathsParameters<Point2D>).Nodes.Count; i++)
                    {
                        temp.Add(string.Format("{0}", JsonHelper.FromPoint2D((parameters as FindTSPPathsParameters<Point2D>).Nodes[i])));
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("endNodeAssigned", (parameters as FindTSPPathsParameters<Point2D>).EndNodeAssigned.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindTSPPathsParameters<Point2D>).Parameter));

                return dictionary;
            }
            else
            {
                throw new ArgumentException("参数只能为TSPPathsParameters<int>或者TSPPathsParameters<Point2D>类型！");
            }
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    FindTSPPathsResult result = FindTSPPathsResult.FromJson(jsonObject);
        //    LastResult = result;
        //    FindTSPPathsEventArgs args = new FindTSPPathsEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(FindTSPPathsEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_FindTSPPathsService_event_processCompleted_D}</summary>

        //public event EventHandler<FindTSPPathsEventArgs> ProcessCompleted;

        //private FindTSPPathsResult lastResult;
        ///// <summary>${REST_FindTSPPathsService_attribute_lastResult_D}</summary>
        //public FindTSPPathsResult LastResult
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
