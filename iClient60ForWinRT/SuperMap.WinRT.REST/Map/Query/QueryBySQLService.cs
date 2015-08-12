

using System.Collections.Generic;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_QueryBySQlService_Tile}</para>
    /// 	<para>${REST_QueryBySQlService_Description}</para>
    /// </summary>
    public class QueryBySQLService : QueryService
    {
        /// <summary>${REST_QueryBySQlService_constructor_None_D}</summary>
        /// <overloads>${REST_QueryBySQlService_constructor_overloads}</overloads>
        public QueryBySQLService() { }

        /// <summary>${REST_QueryBySQlService_constructor_string_url}</summary>
        /// <param name="url">${REST_QueryBySQlService_string_constructor_param_url}</param>
        public QueryBySQLService(string url) : base(url) { }
        /// <summary>${REST_Query_QueryByDistanceService_method_GetParameters_D}</summary>
        /// <param name="parameters">${REST_Query_QueryByDistanceService_method_GetParameters_parameters}</param>
        protected override Dictionary<string, string> GetParameters(QueryParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("queryMode", "\"SqlQuery\"");
            dictionary.Add("queryParameters", QueryParameters.ToJson(parameters));
            return dictionary;
        }
    }
}
