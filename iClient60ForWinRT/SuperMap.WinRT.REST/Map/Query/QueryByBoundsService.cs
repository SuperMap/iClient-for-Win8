using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Query_QueryByBoundsService_Title}</para>
    /// 	<para>${REST_Query_QueryByBoundsService_Description}</para>
    /// </summary>
    public class QueryByBoundsService : QueryService
    {
        /// <summary>${REST_Query_QueryByBoundsService_constructor_None_D}</summary>
        /// <overloads>${REST_Query_QueryByBoundsService_constructor_overloads}</overloads>
        public QueryByBoundsService()
        { }
        /// <summary>${REST_Query_QueryByBoundsService_constructor_string_D}</summary>
        /// <param name="url">${REST_Query_QueryByBoundsService_constructor_string_param_url}</param>
        public QueryByBoundsService(string url)
            : base(url)
        { }
        /// <summary>${REST_Query_QueryByBoundsService_method_GetParameters}</summary>
        /// <param name="parameters">${REST_Query_QueryByBoundsService_method_GetParameters_parameters}</param>
        protected override Dictionary<string, string> GetParameters(QueryParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("queryMode", "\"BoundsQuery\"");
            dictionary.Add("queryParameters", QueryParameters.ToJson(parameters));
            if (!Rectangle2D.IsNullOrEmpty(((QueryByBoundsParameters)parameters).Bounds))
            {
                dictionary.Add("bounds", JsonHelper.FromRectangle2D(((QueryByBoundsParameters)parameters).Bounds));
            }
            return dictionary;
        }
    }
}
