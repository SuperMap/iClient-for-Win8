

using System.Collections.Generic;
using System;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Query_QueryResult_Title}</para>
    /// 	<para>${REST_Query_QueryResult_Description}</para>
    /// </summary>
    public class QueryResult
    {
        internal QueryResult()
        {
            Recordsets = new List<Recordset>();
        }

        /// <summary>${REST_Query_ResultSet_attribute_totalCount_D}</summary>
        public int TotalCount { get; private set; }
        /// <summary>${REST_Query_ResultSet_attribute_currentCount_D}</summary>
        public int CurrentCount { get; private set; }
        /// <summary>${REST_Query_ResultSet_attribute_customResponse_D}</summary>
        public string CustomResponse { get; private set; }
        /// <summary>${REST_Query_ResultSet_attribute_recordSets_D}</summary>
        public IList<Recordset> Recordsets { get; private set; }

        //当返回结果是资源时，用到此项；
        /// <summary>${REST_Query_ResultSet_attribute_ResourceInfo_D}</summary>
        public ResourceInfo ResourceInfo { get; private set; }


        /// <summary>${REST_ResultSet_method_fromJson_D}</summary>
        /// <returns>${REST_ResultSet_method_fromJson_return}</returns>
        /// <param name="json">${REST_ResultSet_method_fromJson_param_jsonObject}</param>
        internal static QueryResult FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            QueryResult result = new QueryResult();

            if (json.ContainsKey("totalCount") && json.ContainsKey("currentCount") && json.ContainsKey("customResponse") && json.ContainsKey("recordsets"))
            {
                result.TotalCount = (int)json["totalCount"].GetNumberEx();

                if ((int)json["totalCount"].GetNumberEx() == 0)
                {
                    return null;
                }

                result.CurrentCount = (int)json["currentCount"].GetNumberEx();
                result.CustomResponse = json["customResponse"].GetStringEx();

                JsonArray recordsets = json["recordsets"].GetArray();
                if (recordsets != null && recordsets.Count > 0)
                {
                    result.Recordsets = new List<Recordset>();

                    for (int i = 0; i < recordsets.Count; i++)
                    {
                        result.Recordsets.Add(Recordset.FromJson(recordsets[i].GetObjectEx()));
                    }
                }
                return result;
            }
            else if (json.ContainsKey("succeed") && json.ContainsKey("newResourceLocation"))
            {
                ResourceInfo info = new ResourceInfo();
                info.Succeed = json["succeed"].GetBooleanEx();
                info.NewResourceLocation = json["newResourceLocation"].GetStringEx();
                info.NewResourceID = json["newResourceID"].GetStringEx();
                result.ResourceInfo = info;
                return result;
            }
            return null;
        }
    }
}
