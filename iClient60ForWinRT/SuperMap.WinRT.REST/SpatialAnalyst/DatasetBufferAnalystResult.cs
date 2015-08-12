
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetBufferAnalystResult_Title}</para>
    /// 	<para>${REST_DatasetBufferAnalystResult_Description}</para>
    /// </summary>
    public class DatasetBufferAnalystResult : SpatialAnalystResult
    {
        /// <summary>${REST_DatasetBufferAnalystResult_constructor_D}</summary>
        public DatasetBufferAnalystResult()
        { }

        //public string DatasetName
        //{
        //    get;
        //    set;
        //}
        /// <summary>${REST_DatasetBufferAnalystResult_attribute_recordset_D}</summary>
        public Recordset Recordset
        {
            get;
            private set;
        }

        /// <summary>${REST_DatasetBufferAnalystResult_method_fromJson_D}</summary>
        /// <returns>${REST_DatasetBufferAnalystResult_method_fromJson_return}</returns>
        /// <param name="jsonResult">${REST_DatasetBufferAnalystResult_method_fromJson_param_jsonObject}</param>
        internal static DatasetBufferAnalystResult FromJson(JsonObject jsonResult)
        {
            DatasetBufferAnalystResult result = new DatasetBufferAnalystResult();
            result.Recordset = Recordset.FromJson(jsonResult);
            return result;
        }

    }
}
