
using Windows.Data.Json;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetsOverlayAnalystResult_Title}</para>
    /// 	<para>${REST_DatasetsOverlayAnalystResult_Description}</para>
    /// </summary>
    public class DatasetOverlayAnalystResult
    {
        /// <summary>${REST_DatasetsOverlayAnalystResult_constructor_D}</summary>
        public DatasetOverlayAnalystResult()
        {
        }

        //public string DatasetName
        //{
        //    get;
        //    set;
        //}
        /// <summary>${REST_DatasetsOverlayAnalystResult_attribute_recordset_D}</summary>
        public Recordset Recordset
        {
            get;
            private set;
        }
        /// <summary>${REST_DatasetsOverlayAnalystResult_method_fromJson_D}</summary>
        /// <returns>${REST_DatasetsOverlayAnalystResult_method_fromJson_return}</returns>
        /// <param name="jsonResult">${REST_DatasetsOverlayAnalystResult_method_fromJson_param_jsonObject}</param>
        internal static DatasetOverlayAnalystResult FromJson(JsonObject jsonResult)
        {
            DatasetOverlayAnalystResult result = new DatasetOverlayAnalystResult();
            result.Recordset = Recordset.FromJson(jsonResult);
            return result;
        }
    }
}
