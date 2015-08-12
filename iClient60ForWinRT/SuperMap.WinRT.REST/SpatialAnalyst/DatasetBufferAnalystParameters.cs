
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetBufferAnalystParams_Title}</para>
    /// 	<para>${REST_DatasetBufferAnalystParams_Description}</para>
    /// </summary>
    public class DatasetBufferAnalystParameters : BufferAnalystParameters
    {
        /// <summary>${REST_DatasetBufferAnalystParams_constructor_D}</summary>
        public DatasetBufferAnalystParameters()
        {
        }
        /// <summary>${REST_DatasetBufferAnalystParams_attribute_filterQueryParameter_D}</summary>
        public FilterParameter FilterQueryParameter
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetBufferAnalystParams_attribute_maxReturnRecordCount_D}</summary>
        public int MaxReturnRecordCount
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetBufferAnalystParams_attribute_isAttributeRetained_D}</summary>
        public bool IsAttributeRetained
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetBufferAnalystParams_attribute_isUnion_D}</summary>
        public bool IsUnion
        {
            get;
            set;
        }

        /// <summary>${REST_DatasetBufferAnalystParams_attribute_datasetsName_D}</summary>
        public string Dataset
        {
            get;
            set;
        }

        internal static System.Collections.Generic.Dictionary<string, string> ToDictionary(DatasetBufferAnalystParameters datasetBufferParams)
        {
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
            dict.Add("isAttributeRetained", datasetBufferParams.IsAttributeRetained.ToString().ToLower());
            dict.Add("isUnion", datasetBufferParams.IsUnion.ToString().ToLower());

            string dataReturnOption = "{\"dataReturnMode\": \"RECORDSET_ONLY\",\"deleteExistResultDataset\": true,";
            dataReturnOption += string.Format("\"expectCount\":{0}", datasetBufferParams.MaxReturnRecordCount);
            dataReturnOption += "}";
            dict.Add("dataReturnOption", dataReturnOption);

            if (datasetBufferParams.FilterQueryParameter != null)
            {
                dict.Add("filterQueryParameter", FilterParameter.ToJson(datasetBufferParams.FilterQueryParameter));
            }
            else
            {
                dict.Add("filterQueryParameter", FilterParameter.ToJson(new FilterParameter()));
            }

            if (datasetBufferParams.BufferSetting != null)
            {
                dict.Add("bufferAnalystParameter", BufferSetting.ToJson(datasetBufferParams.BufferSetting));
            }
            else
            {
                dict.Add("bufferAnalystParameter", BufferSetting.ToJson(new BufferSetting()));
            }

            return dict;
        }
    }
}
