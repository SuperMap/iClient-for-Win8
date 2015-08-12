
using SuperMap.WinRT.Core;
using System.Globalization;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryOverlayAnalystResult_Title}</para>
    /// 	<para>${REST_GeometryOverlayAnalystResult_Description}</para>
    /// </summary>
    public class GeometryOverlayAnalystResult : SpatialAnalystResult
    {
        /// <summary>${REST_GeometryOverlayAnalystResult_constructor_D}</summary>
        public GeometryOverlayAnalystResult()
        { }
        /// <summary>${REST_GeometryOverlayAnalystResult_attribute_ResultGeometry_D}</summary>
        public Geometry ResultGeometry
        {
            get;
            private set;
        }

        /// <summary>${REST_GeometryOverlayAnalystResult_method_fromJson_D}</summary>
        /// <returns>${REST_GeometryOverlayAnalystResult_method_fromJson_return}</returns>
        /// <param name="jsonResult">${REST_GeometryOverlayAnalystResult_method_fromJson_param_jsonObject }</param>
        internal static GeometryOverlayAnalystResult FromJson(JsonObject jsonResult)
        {
            GeometryOverlayAnalystResult result = new GeometryOverlayAnalystResult();
            result.ResultGeometry = (ServerGeometry.FromJson(jsonResult)).ToGeometry();

            return result;
        }
    }
}
