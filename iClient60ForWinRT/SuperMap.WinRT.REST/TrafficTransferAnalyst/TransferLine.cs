using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.TrafficTransferAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransferLine_Title}</para>
    /// </summary>
    public class TransferLine
    {
        /// <summary>${REST_TransferLine_constructor_D}</summary>
        public TransferLine()
        {

        }

        /// <summary>${REST_TransferLine_attribute_EndStopIndex_D}</summary>
        public int EndStopIndex
        {
            get;
            set;
        }

        /// <summary>${REST_TransferLine_attribute_EndStopName_D}</summary>
        public string EndStopName
        {
            get;
            set;
        }

        /// <summary>${REST_TransferLine_attribute_LineID_D}</summary>
        public int LineID
        {
            get;
            set;
        }

        /// <summary>${REST_TransferLine_attribute_LineName_D}</summary>
        public string LineName
        {
            get;
            set;
        }

        /// <summary>${REST_TransferLine_attribute_StartStopIndex_D}</summary>
        public int StartStopIndex
        {
            get;
            set;
        }

        /// <summary>${REST_TransferLine_attribute_StartStopName_D}</summary>
        public string StartStopName
        {
            get;
            set;
        }
        /// <summary>${REST_TransferLine_method_FromJson_D}</summary>
        /// <param name="json">${REST_TransferLine_method_FromJson_param_jsonObject}</param>
        internal static TransferLine FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            TransferLine line = new TransferLine();
            line.EndStopIndex = (int)json["endStopIndex"].GetNumberEx();
            line.EndStopName = json["endStopName"].GetStringEx();
            line.LineID = (int)json["lineID"].GetNumberEx();
            line.LineName = json["lineName"].GetStringEx();
            line.StartStopIndex = (int)json["startStopIndex"].GetNumberEx();
            line.StartStopName = json["startStopName"].GetStringEx();

            return line;
        }

        internal static string ToJson(TransferLine line)
        {
            if (line == null)
            {
                return null;
            }
            List<string> list = new List<string>();
            list.Add(string.Format("\"{0}\":{1}", "endStopIndex", line.EndStopIndex));
            if (!string.IsNullOrEmpty(line.EndStopName))
            {
                list.Add(string.Format("\"{0}\":\"{1}\"", "endStopName", line.EndStopName));
            }
            list.Add(string.Format("\"{0}\":{1}", "lineID", line.LineID));
            if (!string.IsNullOrEmpty(line.LineName))
            {
                list.Add(string.Format("\"{0}\":\"{1}\"", "lineName", line.LineName));
            }
            list.Add(string.Format("\"{0}\":{1}", "startStopIndex", line.StartStopIndex));
            if (!string.IsNullOrEmpty(line.StartStopName))
            {
                list.Add(string.Format("\"{0}\":\"{1}\"", "startStopName", line.StartStopName));
            }
            return "{" + string.Join(",", list) + "}";
        }
    }
}
