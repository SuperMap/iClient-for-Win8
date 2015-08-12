

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
    /// 	<para>${REST_FindMTSPPathsService_Title}</para>
    /// 	<para>${REST_FindMTSPPathsService_Description}</para>
    /// 	</summary>
    public class FindMTSPPathsService : ServiceBase
    {
        /// <summary>${REST_FindMTSPPathsService_constructor_D}</summary>
        /// <overloads>${REST_FindMTSPPathsService_constructor_overloads_D}</overloads>
        public FindMTSPPathsService()
        { }
        /// <summary>${REST_FindMTSPPathsService_constructor_String_D}</summary>
        /// <param name="url">${REST_FindMTSPPathsService_constructor_param_url}</param>
        public FindMTSPPathsService(string url)
            : base(url)
        { }

        /// <summary>${REST_FindMTSPPathsService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_FindMTSPPathsService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_FindMTSPPathsService_method_ProcessAsync_param_state}</param>
        /// 	<example>
        /// 	    <code>
        /// 	         FindMTSPPathsParameters<Point2D> para = new FindMTSPPathsParameters<Point2D>
        ///              {
        ///                  Centers = new List<Point2D>(fpoints),
        ///                  Nodes = new List<Point2D>(mpoints),
        ///                  Parameter = new TransportationAnalystParameter
        ///                  {
        ///                      TurnWeightField = "Turncost",
        ///                      WeightFieldName = "length",
        ///                      ResultSetting = new TransportationAnalystResultSetting
        ///                      {
        ///                          ReturnEdgeFeatures = true,
        ///                          ReturnEdgeGeometry = true,
        ///                          ReturnEdgeIDs = true,
        ///                          ReturnNodeFeatures = true,
        ///                          ReturnNodeGeometry = true,
        ///                          ReturnNodeIDs = true,
        ///                          ReturnPathGuides = true,
        ///                          ReturnRoutes = true
        ///                       }
        ///                  }
        ///               };
        ///               FindMTSPPathsService service = new FindMTSPPathsService(url);
        ///               var result = service.ProcessAsync(para);
        ///         </code>
        ///     </example>
        public async Task<FindMTSPPathsResult> ProcessAsync<T>(T parameters)
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

            this.Url += "mtsppath.json?debug=true&_method=GET";

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return FindMTSPPathsResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters<T>(T parameters)
        {
            if (parameters is FindMTSPPathsParameters<Point2D>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                if ((parameters as FindMTSPPathsParameters<Point2D>).Centers != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindMTSPPathsParameters<Point2D>).Centers.Count; i++)
                    {
                        temp.Add(string.Format("{0}", JsonHelper.FromPoint2D((parameters as FindMTSPPathsParameters<Point2D>).Centers[i])));
                    }
                    dictionary.Add("centers", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                if ((parameters as FindMTSPPathsParameters<Point2D>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindMTSPPathsParameters<Point2D>).Nodes.Count; i++)
                    {
                        temp.Add(string.Format("{0}", JsonHelper.FromPoint2D((parameters as FindMTSPPathsParameters<Point2D>).Nodes[i])));
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("hasLeastTotalCost", (parameters as FindMTSPPathsParameters<Point2D>).HasLeastTotalCost.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindMTSPPathsParameters<Point2D>).Parameter));
                return dictionary;
            }
            else if (parameters is FindMTSPPathsParameters<int>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                if ((parameters as FindMTSPPathsParameters<int>).Centers != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindMTSPPathsParameters<int>).Centers.Count; i++)
                    {
                        temp.Add((parameters as FindMTSPPathsParameters<int>).Centers[i].ToString());
                    }
                    dictionary.Add("centers", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                if ((parameters as FindMTSPPathsParameters<int>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindMTSPPathsParameters<int>).Nodes.Count; i++)
                    {
                        temp.Add((parameters as FindMTSPPathsParameters<int>).Nodes[i].ToString());
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("hasLeastTotalCost", (parameters as FindMTSPPathsParameters<int>).HasLeastTotalCost.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindMTSPPathsParameters<int>).Parameter));
                return dictionary;
            }
            else
            {
                throw new ArgumentException("参数只能为MTSPPathsParameters<int>或者MTSPPathsParameters<Point2D>类型！");
            }
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    FindMTSPPathsResult result = FindMTSPPathsResult.FromJson(jsonObject);
        //    LastResult = result;
        //    FindMTSPPathsEventArgs args = new FindMTSPPathsEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(FindMTSPPathsEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_FindMTSPPathsService_event_processCompleted_D}</summary>

        //public event EventHandler<FindMTSPPathsEventArgs> ProcessCompleted;

        //private FindMTSPPathsResult lastResult;
        ///// <summary>${REST_FindMTSPPathsService_attribute_LastResult_D}</summary>
        //public FindMTSPPathsResult LastResult
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
