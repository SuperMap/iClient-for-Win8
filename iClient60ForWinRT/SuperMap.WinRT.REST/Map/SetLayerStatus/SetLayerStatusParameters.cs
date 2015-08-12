using System.Collections.Generic;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${iServer6_SetLayerStatusParameter_Title}</para>
    /// 	<para>${iServer6_SetLayerStatusParameter_Description}</para>
    /// </summary>
    public class SetLayerStatusParameters
    {
        /// <summary>${iServer6_SetLayerStatusParameter_constructor_D}</summary>
        public SetLayerStatusParameters()
        {
            HoldTime = 10;
        }
        /// <summary>${iServer6_SetLayerStatusParameter_attribute_layerStatusList_D}</summary>
        public IList<LayerStatus> LayerStatusList { get; set; }
        /// <summary>${iServer6_SetLayerStatusParameter_attribute_HoldTime_D}</summary>
        public int HoldTime { get; set; }
        /// <summary>${iServer6_SetLayerStatusParameter_attribute_ResourceID_D}</summary>
        public string ResourceID { get; set; }

        internal static string ToJson(SetLayerStatusParameters parameters)
        {
            string json = "{";
            var list = new List<string>();
            foreach (var item in parameters.LayerStatusList)
            {
                list.Add(LayerStatus.ToJson(item));
            }

            json += string.Format("\"layers\":[{0}]", string.Join(",", list.ToArray()));
            json += "}";

            return json;
        }
    }
}
