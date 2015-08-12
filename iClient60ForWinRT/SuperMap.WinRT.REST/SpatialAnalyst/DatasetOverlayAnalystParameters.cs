
using SuperMap.WinRT.Core;
using System.Collections.Generic;
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_DatasetOverlayAnalystParms_Title}</para>
    /// 	<para>${REST_DatasetOverlayAnalystParms_Description}</para>
    /// </summary>
    public class DatasetOverlayAnalystParameters : OverlayAnalystParameters
    {
        /// <summary>${REST_DatasetOverlayAnalystParms_constructor_D}</summary>
        public DatasetOverlayAnalystParameters()
        {
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_sourceDatasetFilter_D}</summary>
        public FilterParameter SourceDatasetFilter
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_operateDataset_D}</summary>
        public string OperateDataset
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_operateDatasetFilter_D}</summary>
        public FilterParameter OperateDatasetFilter
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_operateRegions_D}</summary>
        public IList<Geometry> OperateRegions
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_sourceDatasetFields_D}</summary>
        public IList<string> SourceDatasetFields
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_operateDatasetFields_D}</summary>
        public IList<string> OperateDatasetFields
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_maxReturnRecordCount_D}</summary>
        public int MaxReturnRecordCount
        {
            get;
            set;
        }        
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_DatasetName_D}</summary>
        public string SourceDataset
        {
            get;
            set;
        }
        /// <summary>${REST_DatasetOverlayAnalystParms_attribute_Tolerance_D}</summary>
        public double Tolerance { get; set; }

        internal static System.Collections.Generic.Dictionary<string, string> ToDictionary(DatasetOverlayAnalystParameters datasetOverlayParams)
        {
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();

            if (!string.IsNullOrEmpty(datasetOverlayParams.OperateDataset) && !string.IsNullOrWhiteSpace(datasetOverlayParams.OperateDataset))
            {
                dict.Add("operateDataset", "\"" + datasetOverlayParams.OperateDataset + "\"");
            }
            else
            {
                dict.Add("operateDataset", "\"\"");
            }

            dict.Add("operation", "\"" + datasetOverlayParams.Operation.ToString() + "\"");

            string dataReturnOption = "{\"dataReturnMode\": \"RECORDSET_ONLY\",\"deleteExistResultDataset\": true,";
            dataReturnOption += string.Format("\"expectCount\":{0}", datasetOverlayParams.MaxReturnRecordCount);
            dataReturnOption += "}";
            dict.Add("dataReturnOption", dataReturnOption);

            if (datasetOverlayParams.SourceDatasetFilter != null)
            {
                dict.Add("sourceDatasetFilter", FilterParameter.ToJson(datasetOverlayParams.SourceDatasetFilter));
            }
            else
            {
                dict.Add("sourceDatasetFilter", FilterParameter.ToJson(new FilterParameter()));
            }

            if (datasetOverlayParams.OperateDatasetFilter != null)
            {
                dict.Add("operateDatasetFilter", FilterParameter.ToJson(datasetOverlayParams.OperateDatasetFilter));
            }
            else
            {
                dict.Add("operateDatasetFilter", FilterParameter.ToJson(new FilterParameter()));
            }

            if (datasetOverlayParams.OperateDatasetFields != null)
            {
                string operateFields = "[";
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                foreach (var item in datasetOverlayParams.OperateDatasetFields)
                {
                    list.Add("\"" + item + "\"");
                }
                operateFields += string.Join(",", list.ToArray());
                operateFields += "]";

                dict.Add("operateDatasetFields", operateFields);
            }
            else
            {
                dict.Add("operateDatasetFields", "[]");
            }

            if (datasetOverlayParams.SourceDatasetFields != null)
            {
                string sourceFields = "[";
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                foreach (var item in datasetOverlayParams.SourceDatasetFields)
                {
                    list.Add("\"" + item + "\"");
                }
                sourceFields += string.Join(",", list.ToArray());
                sourceFields += "]";

                dict.Add("sourceDatasetFields", sourceFields);
            }
            else
            {
                dict.Add("sourceDatasetFields", "[]");
            }

            if (datasetOverlayParams.OperateRegions != null)
            {
                string Regions = "[";
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                foreach (var item in datasetOverlayParams.OperateRegions)
                {
                    list.Add(ServerGeometry.ToJson(item.ToServerGeometry()));
                }
                Regions += string.Join(",", list.ToArray());
                Regions += "]";

                dict.Add("operateRegions", Regions);
            }
            else
            {
                dict.Add("operateRegions", "[]");
            }

            dict.Add("tolerance", datasetOverlayParams.Tolerance.ToString());
            return dict;
        }
    }
}
