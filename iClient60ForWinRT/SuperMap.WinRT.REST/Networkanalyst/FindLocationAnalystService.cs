

using System;
using System.Collections.Generic;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindLocationService_Title}</para>
    /// 	<para>${REST_FindLocationService_Description}</para>
    /// </summary>
    public class FindLocationAnalystService : ServiceBase
    {
        /// <summary>${REST_FindLocationService_constructor_D}</summary>
        /// <overloads>${REST_FindLocationService_constructor_overloads_D}</overloads>
        public FindLocationAnalystService()
        { }
        /// <summary>${REST_FindLocationService_constructor_String_D}</summary>
        /// <param name="url">${REST_FindLocationService_constructor_param_url}</param>
        public FindLocationAnalystService(string url)
            : base(url)
        {
        }

        /// <summary>${REST_FindLocationService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_FindLocationService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_FindLocationService_method_ProcessAsync_param_state}</param>
        public async Task<FindLocationAnalystResult> ProcessAsync(FindLocationAnalystParameters parameters)
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
                base.Url += '/';
            }
            //参数必须放在 URI 中
            base.Url += "location.json?debug=true&_method=GET";            
                        
           var result=await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonObject jsonObject = JsonObject.Parse(result);
           return FindLocationAnalystResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters(FindLocationAnalystParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            if (parameters.SupplyCenters != null && parameters.SupplyCenters.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < parameters.SupplyCenters.Count; i++)
                {
                    temp.Add(string.Format("{0}", SupplyCenter.ToJson(parameters.SupplyCenters[i])));
                }
                dictionary.Add("supplyCenters",string.Format("[{0}]", string.Join(",", temp.ToArray())));
            }

            dictionary.Add("isFromCenter", parameters.IsFromCenter.ToString().ToLower());
            dictionary.Add("expectedSupplyCenterCount", parameters.ExpectedSupplyCenterCount.ToString());
            //dictionary.Add("nodeDemandField", parameters.NodeDemandField);
            dictionary.Add("weightName", parameters.WeightName);
            dictionary.Add("turnWeightField", parameters.TurnWeightField);
            dictionary.Add("returnEdgeFeatures", parameters.ReturnEdgeFeature.ToString().ToLower());
            dictionary.Add("returnEdgeGeometry", parameters.ReturnEdgeGeometry.ToString().ToLower());
            dictionary.Add("returnNodeFeatures", parameters.ReturnNodeFeature.ToString().ToLower());

            return dictionary;
        }
        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(e.Result);
        //    FindLocationAnalystResult result = FindLocationAnalystResult.FromJson(jsonObject);
        //    LastResult = result;
        //    FindLocationAnalystEventArgs args = new FindLocationAnalystEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(FindLocationAnalystEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_FindLocationService_event_processCompleted_D}</summary>
        
        //public event EventHandler<FindLocationAnalystEventArgs> ProcessCompleted;

        //private FindLocationAnalystResult lastResult;
        ///// <summary>${REST_FindLocationService_attribute_lastResult_D}</summary>
        //public FindLocationAnalystResult LastResult
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
