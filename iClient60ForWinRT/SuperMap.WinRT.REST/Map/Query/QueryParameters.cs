

using System.Collections.Generic;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Query_queryParamerters_Title}</para>
    /// 	<para>${REST_Query_queryParamerters_Description}</para>
    /// </summary>
    public abstract class QueryParameters
    {
        /// <summary>${REST_Query_QueryParamerters_constructor_None_D}</summary>
        public QueryParameters()
        {
            ExpectCount = 100000;
            NetworkType = ServerGeometryType.LINE;
            QueryOption = QueryOption.ATTRIBUTEANDGEOMETRY;
            StartRecord = 0;
            ReturnContent = true;
            HoldTime = int.MinValue;
        }

        /// <summary>${REST_Query_attribute_CustomParams_D}</summary>
        public string CustomParams { get; set; }

        /// <summary>${REST_Query_attribute_ExpectCount_D}</summary>
        public int ExpectCount { get; set; }

        /// <summary>${REST_Query_attribute_NetworkType_D}</summary>
        public ServerGeometryType NetworkType { get; set; }

        /// <summary>${REST_Query_attribute_QueryOption_D}</summary>
        public QueryOption QueryOption { get; set; }

        /// <summary>${REST_Query_attribute_FilterParameters_D}</summary>
        public IList<FilterParameter> FilterParameters { get; set; }

        /// <summary>${REST_Query_attribute_StartRecord_D}</summary>
        public int StartRecord { get; set; }
        /// <summary>${REST_Query_attribute_ReturnContent_D}</summary>
        public bool ReturnContent { get; set; }
        /// <summary>${REST_Query_attribute_holdTime_D}</summary>
        public int HoldTime { get; set; }


        //
        internal static string ToJson(QueryParameters param)
        {
            if (param == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(param.CustomParams))
            {
                list.Add(string.Format("\"customParams\":\"{0}\"", param.CustomParams));
            }
            else
            {
                list.Add("\"customParams\":null");
            }

            if (param.HoldTime != int.MinValue)
            {
                list.Add(string.Format("\"holdTime\":{0}", param.HoldTime));
            }

            list.Add(string.Format("\"expectCount\":{0}", param.ExpectCount));
            list.Add(string.Format("\"startRecord\":{0}", param.StartRecord));
            list.Add(string.Format("\"networkType\":\"{0}\"", param.NetworkType.ToString()));
            list.Add(string.Format("\"queryOption\":\"{0}\"", param.QueryOption.ToString()));


            IList<FilterParameter> queryParams = param.FilterParameters;
            if (queryParams != null && queryParams.Count > 0)
            {
                List<string> layerParams = new List<string>();

                for (int i = 0; i < queryParams.Count; i++)
                {
                    layerParams.Add(FilterParameter.ToJson(param.FilterParameters[i]));
                }
                string temp = "[" + string.Join(",", layerParams.ToArray()) + "]";
                list.Add(string.Format("\"queryParams\":{0}", temp));
            }


            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }
    }
}
