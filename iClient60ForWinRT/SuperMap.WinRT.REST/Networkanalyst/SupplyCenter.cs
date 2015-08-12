using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_SupplyCenter_Title}</para>
    /// 	<para>${REST_SupplyCenter_Description}</para>
    /// </summary>
    public class SupplyCenter
    {
        /// <summary>${REST_SupplyCenter_constructor_D}</summary>
        public SupplyCenter()
        { }
        /// <summary>${REST_SupplyCenter_attribute_maxWeight_D}</summary>
        public double MaxWeight { get; set; }
        /// <summary>${REST_SupplyCenter_attribute_nodeID_D}</summary>
        public int NodeID { get; set; }
        /// <summary>${REST_SupplyCenter_attribute_resourceValue_D}</summary>
        public double ResourceValue { get; set; }
        /// <summary>${REST_SupplyCenter_attribute_type_D}</summary>
        public SupplyCenterType Type { get; set; }

        internal static string ToJson(SupplyCenter param)
        {
            if (param == null)
                return null;

            string json = "{";
            List<string> list = new List<string>();
            list.Add(string.Format("\"maxWeight\":{0}", param.MaxWeight));
            list.Add(string.Format("\"nodeID\":{0}", param.NodeID));
            list.Add(string.Format("\"resourceValue\":{0}", param.ResourceValue));
            list.Add(string.Format("\"type\":{0}", param.Type.ToString()));

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }
        /// <summary>${REST_SupplyCenter_method_fromJson_D}</summary>
        /// <returns>${REST_SupplyCenter_method_fromJson_return}</returns>
        /// <param name="json">${REST_SupplyCenter_method_fromJson_param_jsonObject}</param>
        internal static SupplyCenter FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            SupplyCenter result = new SupplyCenter();
            result.MaxWeight = json["maxWeight"].GetNumberEx();
            result.NodeID = (int)json["nodeID"].GetNumberEx();
            result.ResourceValue = json["resourceValue"].GetNumberEx();
            result.Type = (SupplyCenterType)Enum.Parse(typeof(SupplyCenterType), json["type"].GetStringEx(), true);
            return result;
        }
    }
}
