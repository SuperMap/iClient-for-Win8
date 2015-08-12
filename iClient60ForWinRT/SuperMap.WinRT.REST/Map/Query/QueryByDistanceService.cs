using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.REST.Resources;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_QueryByDistanceService_Tile}</para>
    /// 	<para>${REST_QueryByDistanceService_Description}</para>
    /// </summary>
    public class QueryByDistanceService : QueryService
    {

        /// <summary>${REST_QueryByDistanceService_constructor_None_D}</summary>
        /// <overloads>${REST_QueryByDistanceService_constructor_overloads}</overloads>
        public QueryByDistanceService() { }

        /// <summary>${REST_QueryByDistanceService_string_constructor}</summary>
        public QueryByDistanceService(string url) : base(url) { }

        /// <summary>${REST_Query_QueryByDistanceService_method_GetParameters_D}</summary>
        /// <param name="parameters">${REST_Query_QueryByDistanceService_method_GetParameters_parameters}</param>
        protected override Dictionary<string, string> GetParameters(QueryParameters parameters)
        {

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (((QueryByDistanceParameters)parameters).IsNearest)
            {
                dictionary.Add("queryMode", "\"FindNearest\"");
            }
            else
            {
                dictionary.Add("queryMode", "\"DistanceQuery\"");
            }
            dictionary.Add("queryParameters", QueryParameters.ToJson(parameters));
            dictionary.Add("geometry", ServerGeometry.ToJson(((QueryByDistanceParameters)parameters).Geometry.ToServerGeometry()));
            dictionary.Add("distance", ((QueryByDistanceParameters)parameters).Distance.ToString());

            return dictionary;
        }
    }
}
