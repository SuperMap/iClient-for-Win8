 
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetExtractParams_Title}</para>
    /// 	<para>${REST_DatasetExtractParams_Description}</para>
    /// </summary>
    public class DatasetSurfaceAnalystParameters : SurfaceAnalystParameters
    {
        /// <summary>${REST_DatasetExtractParams_constructor_D}</summary>
        public DatasetSurfaceAnalystParameters()
        { }
        ///// <summary>${REST_DatasetExtractParams_attribute_ExtractParamsSetting_D}</summary>
        //public SurfaceAnalystParametersSetting ParametersSetting
        //{
        //    get;
        //    set;
        //}

        /// <summary>${REST_DatasetExtractParams_attribute_FilterQueryParam_D}</summary>
        public FilterParameter FilterQueryParam
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetExtractParams_attribute_ZValuedFieldName_D}</summary>
        public string ZValueFieldName
        {
            get;
            set;
        }
        ///// <summary>${REST_DatasetExtractParams_attribute_MaxReturnRecordCount_D}</summary>
        //public int MaxReturnRecordCount
        //{
        //    get;
        //    set;
        //}

        /// <summary>${REST_DatasetExtractParams_attribute_DatasetName_D}</summary>
        public string Dataset
        {
            get;
            set;
        }

        internal static System.Collections.Generic.Dictionary<string, string> ToDictionary(DatasetSurfaceAnalystParameters datasetSurfaceAnalystParams)
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>();

            if (datasetSurfaceAnalystParams.ParametersSetting != null)
            {
                dict.Add("extractParameter", SurfaceAnalystParametersSetting.ToJson(datasetSurfaceAnalystParams.ParametersSetting));
            }
            else
            {
                dict.Add("extractParameter", SurfaceAnalystParametersSetting.ToJson(new SurfaceAnalystParametersSetting()));
            }

            string resultSetting = string.Format("\"dataReturnMode\":\"RECORDSET_ONLY\",\"expectCount\":{0}", datasetSurfaceAnalystParams.MaxReturnRecordCount);
            resultSetting = "{" + resultSetting + "}";
            dict.Add("resultSetting", resultSetting);

            if (datasetSurfaceAnalystParams.FilterQueryParam != null)
            {
                dict.Add("filterQueryParameter", FilterParameter.ToJson(datasetSurfaceAnalystParams.FilterQueryParam));
            }

            if (!string.IsNullOrEmpty(datasetSurfaceAnalystParams.ZValueFieldName) && !string.IsNullOrWhiteSpace(datasetSurfaceAnalystParams.ZValueFieldName))
            {
                dict.Add("zValueFieldName", "\"" + datasetSurfaceAnalystParams.ZValueFieldName + "\"");
            }
            else
            {
                dict.Add("zValueFieldName","\"\"");
            }

            dict.Add("resolution", datasetSurfaceAnalystParams.Resolution.ToString());
            return dict;
        }
    }
}
