using System.Collections.Generic;
using System;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;
using Newtonsoft.Json;
using SuperMap.WindowsPhone.REST.Help;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_Query_Recordset_Title}</para>
    /// 	<para>${REST_Query_Recordset_Description}</para>
    /// </summary>
    public class Recordset
    {
        internal Recordset()
        { }

        /// <summary>${REST_Query_Recordset_attribute_DatasetName_D}</summary>
        [JsonProperty("datasetName")]
        public string DatasetName { get; internal set; }
        /// <summary>${REST_Query_Recordset_attribute_FieldCaptions_D}</summary>
        [JsonProperty("fieldCaptions")]
        public List<string> FieldCaptions { get; internal set; }
        /// <summary>${REST_Query_Recordset_attribute_Fields_D}</summary>
        [JsonProperty("fields")]
        public List<string> Fields { get; internal set; }
        /// <summary>${REST_Query_Recordset_attribute_FieldTypes_D}</summary>
        [JsonProperty("fieldTypes")]
        public List<FieldType> FieldTypes { get; internal set; }
        /// <summary>${REST_Query_Recordset_attribute_Features_D}</summary>
        [JsonProperty("features")]
        [JsonConverter(typeof(FeatureCollectionConverter))]
        public List<Feature> Features { get; internal set; }
        
    }
}
