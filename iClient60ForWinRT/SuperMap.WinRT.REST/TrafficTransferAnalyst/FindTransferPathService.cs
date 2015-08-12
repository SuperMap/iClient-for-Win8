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
    /// 	<para>${REST_FindTransferPathService_Title}</para>
    /// </summary>
    public class FindTransferPathService : ServiceBase
    {
        /// <summary>${REST_FindTransferPathService_constructor_D}</summary>
        public FindTransferPathService()
        { }
        /// <summary>${REST_FindTransferPathService_constructor_param_url}</summary>
        public FindTransferPathService(string url)
            : base(url)
        { }
        /// <summary>${REST_FindTransferPathService_method_ProcessAsync_param_Parameters}</summary>
        public async Task<TransferGuide> ProcessAsync(FindTransferPathParameter paramer)
        {
            GenerateAbsoluteUrl(paramer);
            var result = await base.SubmitRequest(this.Url, GetDictionaryParameters(paramer), false, false, false);
            JsonObject jsonObject = JsonObject.Parse(result);
            return TransferGuide.FromJson(jsonObject);
        }

        private void GenerateAbsoluteUrl(FindTransferPathParameter parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (!this.Url.EndsWith("/"))
            {
                this.Url += "/";
            }
            this.Url += "path.rjson?";
        }

        private System.Collections.Generic.Dictionary<string, string> GetDictionaryParameters(FindTransferPathParameter parameters)
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
            TransferLine[] lines = new TransferLine[parameters.TransferLines.Length];
            for (int i = 0; i < parameters.TransferLines.Length; i++)
            {
                lines[i] = new TransferLine();
                lines[i].EndStopIndex = parameters.TransferLines[i].EndStopIndex;
                lines[i].LineID = parameters.TransferLines[i].LineID;
                lines[i].StartStopIndex = parameters.TransferLines[i].StartStopIndex;
            }

            string jsonLines = "[";
            List<string> lJsonLine = new List<string>();
            foreach (TransferLine line in lines)
            {
                lJsonLine.Add(TransferLine.ToJson(line));
            }
            jsonLines += string.Join(",", lJsonLine);
            jsonLines += "]";
            dic["transferLines"] = jsonLines;
            return dic;
        }

        //private void FindTransferPathService_Complated(object sender, RequestEventArgs args)
        //{
        //    JsonObject jsonObject = JsonObject.Parse(args.Result);
        //    TransferGuide result = TransferGuide.FromJson(jsonObject);

        //    FindTransferPathEventArgs e = new FindTransferPathEventArgs(result, args.Result, args.UserState);
        //    lastResult = result;
        //    if (ProcessCompleted != null)
        //    {
        //        ProcessCompleted(this, e);
        //    }
        //}
        ///// <summary>${REST_FindTransferPathService_event_ProcessCompleted_D}</summary>

        //public event EventHandler<FindTransferPathEventArgs> ProcessCompleted;

        //private TransferGuide lastResult;
        ///// <summary>${REST_FindTransferPathService_attribute_lastResult_D}</summary>
        //public TransferGuide LastResult
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
