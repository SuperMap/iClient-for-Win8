

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
    /// 	<para>${REST_ServiceAreasService_Title}</para>
    /// 	<para>${REST_ServiceAreasService_Description}</para>
    /// </summary>
    public class FindServiceAreasService : ServiceBase
    {
        /// <summary>${REST_ServiceAreasService_constructor_D}</summary>
        /// <overloads>${REST_ServiceAreasService_constructor_overloads_D}</overloads>
        public FindServiceAreasService()
        {
        }
        /// <summary>${REST_ServiceAreasService_constructor_String_D}</summary>
        /// <param name="url">${REST_ServiceAreasService_constructor_param_url}</param>
        public FindServiceAreasService(string url)
            : base(url)
        {
        }

        /// <summary>${REST_ServiceAreasService_method_ProcessAsync_D}</summary>
        /// <param name="state">${REST_ServiceAreasService_method_ProcessAsync_param_state}</param>
        /// <param name="parameters">${REST_ServiceAreasService_method_ProcessAsync_param_Parameter}</param>
        public async Task<FindServiceAreasResult> ProcessAsync<T>(T parameters)
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
            //参数必须放在 URI 中
            this.Url += "servicearea.json?debug=true&_method=GET";

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return FindServiceAreasResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters<T>(T parameters)
        {
            if (parameters is FindServiceAreasParameters<Point2D>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindServiceAreasParameters<Point2D>).Centers != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindServiceAreasParameters<Point2D>).Centers.Count; i++)
                    {
                        temp.Add(JsonHelper.FromPoint2D((parameters as FindServiceAreasParameters<Point2D>).Centers[i]));
                    }
                    dictionary.Add("centers", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                if ((parameters as FindServiceAreasParameters<Point2D>).Weights != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindServiceAreasParameters<Point2D>).Weights.Count; i++)
                    {
                        temp.Add((parameters as FindServiceAreasParameters<Point2D>).Weights[i].ToString());
                    }
                    dictionary.Add("weights", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("isFromCenter", (parameters as FindServiceAreasParameters<Point2D>).IsFromCenter.ToString().ToLower());
                dictionary.Add("isCenterMutuallyExclusive", (parameters as FindServiceAreasParameters<Point2D>).IsCenterMutuallyExclusive.ToString().ToLower());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindServiceAreasParameters<Point2D>).Parameter));
                return dictionary;
            }
            else if (parameters is FindServiceAreasParameters<int>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindServiceAreasParameters<int>).Centers != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindServiceAreasParameters<int>).Centers.Count; i++)
                    {
                        temp.Add((parameters as FindServiceAreasParameters<int>).Centers[i].ToString());
                    }
                    dictionary.Add("centers", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                if ((parameters as FindServiceAreasParameters<int>).Weights != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindServiceAreasParameters<int>).Weights.Count; i++)
                    {
                        temp.Add((parameters as FindServiceAreasParameters<int>).Weights[i].ToString());
                    }
                    dictionary.Add("weights", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("isFromCenter", (parameters as FindServiceAreasParameters<int>).IsFromCenter.ToString().ToLower());
                dictionary.Add("isCenterMutuallyExclusive", (parameters as FindServiceAreasParameters<int>).IsCenterMutuallyExclusive.ToString().ToLower());

                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindServiceAreasParameters<int>).Parameter));
                return dictionary;
            }
            else
            {
                throw new ArgumentException("参数只能为ServiceAreasParameters<int>或者ServiceAreasParameters<Point2D>类型！");
            }
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    FindServiceAreasResult result = FindServiceAreasResult.FromJson(jsonObject);
        //    LastResult = result;
        //    FindServiceAreasEventArgs args = new FindServiceAreasEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(FindServiceAreasEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_ServiceAreasService_event_processCompleted_D}</summary>

        //public event EventHandler<FindServiceAreasEventArgs> ProcessCompleted;

        //private FindServiceAreasResult lastResult;
        ///// <summary>${REST_ServiceAreasService_attribute_lastResult_D}</summary>
        //public FindServiceAreasResult LastResult
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
