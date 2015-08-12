
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_ExtractResult_Title}</para>
    /// 	<para>${REST_ExtractResult_Description}</para>
    /// </summary>
    public class SurfaceAnalystResult
    {
        /// <summary>${REST_ExtractResult_constructor_D}</summary>
        internal SurfaceAnalystResult()
        { }
        /// <summary>${REST_ExtractResult_attribute_recordset_D}</summary>
        public Recordset Recordset
        {
            get;
            private set;
        }
        /// <summary>${REST_ExtractResult_method_fromJson_D}</summary>
        /// <returns>${REST_ExtractResult_method_fromJson_return}</returns>
        /// <param name="jsonResult">${REST_ExtractResult_method_fromJson_param_jsonObject}</param>
        internal static SurfaceAnalystResult FromJson(JsonObject jsonResult)
        {
            SurfaceAnalystResult result = new SurfaceAnalystResult();
            result.Recordset = Recordset.FromJson(jsonResult);
            return result;
        }
    }
}
