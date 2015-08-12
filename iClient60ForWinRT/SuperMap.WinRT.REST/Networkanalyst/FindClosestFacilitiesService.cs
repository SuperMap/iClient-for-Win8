

using System;
using System.Collections.Generic;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.Utilities;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ClosestFacilitiesService_Title}</para>
    /// 	<para>${REST_ClosestFacilitiesService_Description}</para>
    /// </summary>
    public class FindClosestFacilitiesService : ServiceBase
    {
        /// <summary>${REST_ClosestFacilitiesService_constructor_D}</summary>
        /// <overloads>${REST_ClosestFacilitiesService_constructor_overloads_D}</overloads>
        public FindClosestFacilitiesService()
        { }
        /// <summary>${REST_ClosestFacilitiesService_constructor_String_D}</summary>
        /// <param name="url">${REST_ClosestFacilitiesService_constructor_param_url}</param>
        public FindClosestFacilitiesService(string url)
            : base(url)
        { }

        /// <summary>${REST_ClosestFacilitiesService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_ClosestFacilitiesService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_ClosestFacilitiesService_method_ProcessAsync_param_state}</param>
        public async Task<FindClosestFacilitiesResult> ProcessAsync<T>(T parameters)
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
            //http://localhost:8090/iserver/services/transportationanalyst-sample/rest/networkanalyst/RoadNet@Changchun.json
            this.Url += "closestfacility.json?debug=true&_method=GET";

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters),  true);
            JsonObject jsonObject = JsonObject.Parse(result);
            return  FindClosestFacilitiesResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters<T>(T parameters)
        {
            if (parameters is FindClosestFacilitiesParameters<Point2D>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindClosestFacilitiesParameters<Point2D>).Facilities != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindClosestFacilitiesParameters<Point2D>).Facilities.Count; i++)
                    {
                        temp.Add(string.Format("{0}", JsonHelper.FromPoint2D((parameters as FindClosestFacilitiesParameters<Point2D>).Facilities[i])));
                    }
                    dictionary.Add("facilities", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("event", JsonHelper.FromPoint2D((parameters as FindClosestFacilitiesParameters<Point2D>).Event));
                dictionary.Add("expectFacilityCount", (parameters as FindClosestFacilitiesParameters<Point2D>).ExpectFacilityCount.ToString());
                dictionary.Add("fromEvent", (parameters as FindClosestFacilitiesParameters<Point2D>).FromEvent.ToString().ToLower());
                dictionary.Add("maxWeight", (parameters as FindClosestFacilitiesParameters<Point2D>).MaxWeight.ToString());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as FindClosestFacilitiesParameters<Point2D>).Parameter));
                return dictionary;
            }
            else if (parameters is FindClosestFacilitiesParameters<int>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as FindClosestFacilitiesParameters<int>).Facilities != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as FindClosestFacilitiesParameters<int>).Facilities.Count; i++)
                    {
                        temp.Add((parameters as FindClosestFacilitiesParameters<int>).Facilities[i].ToString());
                    }
                    dictionary.Add("facilities", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("event", (parameters as FindClosestFacilitiesParameters<int>).Event.ToString());
                dictionary.Add("expectFacilityCount", (parameters as FindClosestFacilitiesParameters<int>).ExpectFacilityCount.ToString());
                dictionary.Add("fromEvent", (parameters as FindClosestFacilitiesParameters<int>).FromEvent.ToString().ToLower());
                dictionary.Add("maxWeight", (parameters as FindClosestFacilitiesParameters<int>).MaxWeight.ToString());
                dictionary.Add("parameter", TransportationAnalystParameter.ToJson(((parameters as FindClosestFacilitiesParameters<int>).Parameter)));
                return dictionary;
            }
            else
            {
                throw new ArgumentException("参数只能为ClosestFacilitiesParameters<int>或者ClosestFacilitiesParameters<Point2D>类型！");
            }
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    FindClosestFacilitiesResult result = FindClosestFacilitiesResult.FromJson(jsonObject);
        //    LastResult = result;
        //    FindClosestFacilitiesEventArgs args = new FindClosestFacilitiesEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(FindClosestFacilitiesEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_ClosestFacilitiesService_event_processCompleted_D}</summary>
        
        //public event EventHandler<FindClosestFacilitiesEventArgs> ProcessCompleted;

        //private FindClosestFacilitiesResult lastResult;
        ///// <summary>${REST_ClosestFacilitiesService_attribute_lastResult_D}</summary>
        //public FindClosestFacilitiesResult LastResult
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
