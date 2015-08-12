

using System.Collections.Generic;
using System;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
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
        public string DatasetName { get; private set; }
        /// <summary>${REST_Query_Recordset_attribute_FieldCaptions_D}</summary>
        public List<string> FieldCaptions { get; private set; }
        /// <summary>${REST_Query_Recordset_attribute_Fields_D}</summary>
        public List<string> Fields { get; private set; }
        /// <summary>${REST_Query_Recordset_attribute_FieldTypes_D}</summary>
        public List<FieldType> FieldTypes { get; private set; }
        /// <summary>${REST_Query_Recordset_attribute_Features_D}</summary>
        public FeatureCollection Features { get; private set; }
        /// <summary>${REST_Query_method_FromJson_D}</summary>
        /// <returns>${REST_Query_method_FromJson_return}</returns>
        /// <param name="json">${REST_Query_method_FromJson_param_jsonObject}</param>
        internal static Recordset FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            Recordset result = new Recordset();

            result.DatasetName = json["datasetName"].GetStringEx();
            result.FieldCaptions = JsonHelper.ToStringList(json["fieldCaptions"].GetArray());


            JsonArray fieldtypes = json["fieldTypes"].GetArray();
            if (fieldtypes != null && fieldtypes.Count > 0)
            {
                result.FieldTypes = new List<FieldType>();
                for (int i = 0; i < fieldtypes.Count; i++)
                {
                    result.FieldTypes.Add((FieldType)Enum.Parse(typeof(FieldType), fieldtypes[i].GetStringEx(), true));
                }
            }

            result.Fields = JsonHelper.ToStringList(json["fields"].GetArray());
            List<string> fieldNames = JsonHelper.ToStringList(json["fields"].GetArray());
            JsonArray features = json["features"].GetArray();
            if (features != null && features.Count > 0 )
            {
                result.Features = new FeatureCollection();

                for (int i = 0; i < features.Count; i++)
                {
                    ServerFeature f = ServerFeature.FromJson(features[i].GetObjectEx());

                    f.FieldNames = new List<string>();
                    if (fieldNames != null && fieldNames.Count > 0)
                    {
                        for (int j = 0; j < fieldNames.Count; j++)
                        {
                            f.FieldNames.Add(fieldNames[j]);
                        }
                    }
                    result.Features.Add(f.ToFeature());
                }
            }
            return result;
        }
    }
}
