using SuperMap.WinRT.Utilities;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryBufferAnalystResult_Title}</para>
    /// 	<para>${REST_GeometryBufferAnalystResult_Description}</para>
    /// </summary>
    public class GeometryBufferAnalystResult : SpatialAnalystResult
    {
        /// <summary>${REST_GeometryBufferAnalystResult_constructor_D}</summary>
        public GeometryBufferAnalystResult()
        { }

        //public ImageResult Image
        //{
        //    get;
        //    set;
        //}

        //public string Message
        //{
        //    get;
        //    set;
        //}
        /// <summary>${REST_GeometryBufferAnalystResult_attribute_ResultGeometry_D}</summary>
        public Geometry ResultGeometry
        {
            get;
            private set;
        }
        /// <summary>${REST_GeometryBufferAnalystResult_method_fromJson_D}</summary>
        /// <returns>${REST_GeometryBufferAnalystResult_method_fromJson_return}</returns>
        /// <param name="jsonResult">${REST_GeometryBufferAnalystResult_method_fromJson_param_jsonObject}</param>
        internal static GeometryBufferAnalystResult FromJson(JsonObject jsonResult)
        {
            GeometryBufferAnalystResult result = new GeometryBufferAnalystResult();
            result.ResultGeometry = (ServerGeometry.FromJson(jsonResult["resultGeometry"].GetObjectEx())).ToGeometry();
            //result.Image = ImageResult.FromJson((System.Json.JsonObject)jsonResult["image"]);
            return result;
        }
    }
}
