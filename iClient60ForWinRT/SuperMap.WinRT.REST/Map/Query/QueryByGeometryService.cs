

using System.Collections.Generic;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_QueryByGeometryService_Tile}</para>
    /// 	<para>${REST_QueryByGeometryService_Description}</para>
    /// </summary>
    public class QueryByGeometryService : QueryService
    {
        /// <summary>${REST_QueryByGeometryService_constructor_None_D}</summary>
        /// <overloads>${REST_QueryByGeometryService_constructor_overloads}</overloads>
        public QueryByGeometryService() { }

        /// <summary>${REST_QueryByGeometryService_string_constructor}</summary>
        /// <param name="url">${REST_QueryByGeometryService_string_constructor_param_url}</param>
        public QueryByGeometryService(string url) : base(url) { }
        /// <summary>${REST_Query_QueryByDistanceService_method_GetParameters_D}</summary>
        /// <param name="parameters">${REST_Query_QueryByDistanceService_method_GetParameters_parameters}</param>
        protected override Dictionary<string, string> GetParameters(QueryParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("queryMode", "\"SpatialQuery\"");
            dictionary.Add("queryParameters", QueryParameters.ToJson(parameters));
            dictionary.Add("geometry", ServerGeometry.ToJson(((QueryByGeometryParameters)parameters).Geometry.ToServerGeometry()));
            dictionary.Add("spatialQueryMode", string.Format("\"{0}\"", ((QueryByGeometryParameters)parameters).SpatialQueryMode));

            return dictionary;
        }
    }
}
