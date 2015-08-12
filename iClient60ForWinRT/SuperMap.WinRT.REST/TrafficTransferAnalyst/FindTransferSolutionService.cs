using System;
using SuperMap.WinRT.Service;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindTransferSolutionService_Title}</para>
    /// </summary>
    public class FindTransferSolutionService : ServiceBase
    {
        /// <summary>${REST_FindTransferSolutionService_constructor_D}</summary>
        public FindTransferSolutionService()
        { }
        /// <summary>${REST_FindTransferSolutionService_constructor_param_url}</summary>
        public FindTransferSolutionService(string url)
            : base(url)
        { }
        /// <summary>${REST_FindTransferSolutionService_method_ProcessAsync_param_Parameters}</summary>
        public async Task<FindTransferSolutionResult> ProcessAsync(FindTransferSolutionsParameter paramer)
        {
            GenerateAbsoluteUrl(paramer);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(paramer), false, false, false);
            JsonObject jsonObject = JsonObject.Parse(result);
            return FindTransferSolutionResult.FromJson(jsonObject);
        }

        private void GenerateAbsoluteUrl(FindTransferSolutionsParameter parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (!this.Url.EndsWith("/"))
            {
                this.Url += "/";
            }
            this.Url += "solutions.rjson?";
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(FindTransferSolutionsParameter parameters)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string jsonPoints = "[";
            List<string> jsonL = new List<string>();
            if (parameters.Points.GetType() == typeof(Point2D[]))
            {
                Point2D[] points = parameters.Points as Point2D[];
                for (int i = 0; i < points.Length; i++)
                {
                    jsonL.Add(JsonHelper.FromPoint2D(points[i]));
                }
            }
            else if (parameters.Points.GetType() == typeof(long[]))
            {
                long[] points = parameters.Points as long[];
                for (int i = 0; i < points.Length; i++)
                {
                    jsonL.Add(points[i].ToString());
                }
            }

            jsonPoints += string.Join(",", jsonL);
            jsonPoints += "]";

            dic["points"] = jsonPoints;
            dic["solutionCount"] = parameters.SolutionCount.ToString();
            dic["transferTactic"] = parameters.TransferTactic.ToString();
            dic["transferPreference"] = parameters.TransferPreference.ToString();
            dic["walkingRatio"] = parameters.WalkingRatio.ToString();
            return dic;
        }

        //private void FindTransferSolutionService_Complated(object sender, RequestEventArgs args)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(args.Result);
        //    FindTransferSolutionResult result = FindTransferSolutionResult.FromJson(jsonObject);

        //    FindTransferSolutionEventArgs e = new FindTransferSolutionEventArgs(result, args.Result, args.UserState);
        //    lastResult = result;
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}
        ///// <summary>${REST_FindTransferSolutionService_event_ProcessCompleted_D}</summary>

        //public event EventHandler<FindTransferSolutionEventArgs> ProcessCompleted;

        //private FindTransferSolutionResult lastResult;
        ///// <summary>${REST_FindTransferSolutionService_attribute_lastResult_D}</summary>
        //public FindTransferSolutionResult LastResult
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
