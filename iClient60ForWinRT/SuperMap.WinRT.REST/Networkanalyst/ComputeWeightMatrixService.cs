

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
    /// 	<para>${REST_ComputeWeightMatrixService_Title}</para>
    /// 	<para>${REST_ComputeWeightMatrixService_Description}</para>
    /// </summary>
    public class ComputeWeightMatrixService : ServiceBase
    {
        /// <summary>${REST_ComputeWeightMatrixService_constructor_D}</summary>
        /// <overloads>${REST_ComputeWeightMatrixService_constructor_overloads_D}</overloads>
        public ComputeWeightMatrixService()
        { }
        /// <summary>${REST_ComputeWeightMatrixService_constructor_String_D}</summary>
        /// <param name="url">${REST_ComputeWeightMatrixService_constructor_param_url}</param>
        public ComputeWeightMatrixService(string url)
            : base(url)
        {
        }

        /// <summary>${REST_ComputeWeightMatrixService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_ComputeWeightMatrixService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_ComputeWeightMatrixService_method_ProcessAsync_param_state}</param>
        public async Task<ComputeWeightMatrixResult> ProcessAsync<T>(T parameters)
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
            base.Url += "weightmatrix.json?debug=true&_method=GET";

            var result = await base.SubmitRequest(base.Url, GetParameters(parameters), true);
            JsonArray jsonObject = JsonArray.Parse(result);
            return ComputeWeightMatrixResult.FromJson(jsonObject);
        }

        private Dictionary<string, string> GetParameters<T>(T parameters)
        {
            if (parameters is ComputeWeightMatrixParameters<Point2D>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as ComputeWeightMatrixParameters<Point2D>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as ComputeWeightMatrixParameters<Point2D>).Nodes.Count; i++)
                    {
                        temp.Add(string.Format("{0}", JsonHelper.FromPoint2D((parameters as ComputeWeightMatrixParameters<Point2D>).Nodes[i])));
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as ComputeWeightMatrixParameters<Point2D>).Parameter));
                return dictionary;
            }
            else if (parameters is ComputeWeightMatrixParameters<int>)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if ((parameters as ComputeWeightMatrixParameters<int>).Nodes != null)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < (parameters as ComputeWeightMatrixParameters<int>).Nodes.Count; i++)
                    {
                        temp.Add((parameters as ComputeWeightMatrixParameters<int>).Nodes[i].ToString());
                    }
                    dictionary.Add("nodes", string.Format("[{0}]", string.Join(",", temp.ToArray())));
                }

                dictionary.Add("parameter", TransportationAnalystParameter.ToJson((parameters as ComputeWeightMatrixParameters<int>).Parameter));
                return dictionary;
            }
            else
            {
                throw new ArgumentException("参数只能为WeightMatrixParameters<int>或者WeightMatrixParameters<Point2D>类型！");
            }
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    JsonArray jsonObject = JsonArray.Parse(e.Result);
        //    ComputeWeightMatrixResult result = ComputeWeightMatrixResult.FromJson(jsonObject);
        //    LastResult = result;
        //    ComputeWeightMatrixEventArgs args = new ComputeWeightMatrixEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);
        //}

        //private void OnProcessCompleted(ComputeWeightMatrixEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_ComputeWeightMatrixService_event_processCompleted_D}</summary>

        //public event EventHandler<ComputeWeightMatrixEventArgs> ProcessCompleted;

        //private ComputeWeightMatrixResult lastResult;
        ///// <summary>${REST_ComputeWeightMatrixService_attribute_lastResult_D}</summary>
        //public ComputeWeightMatrixResult LastResult
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