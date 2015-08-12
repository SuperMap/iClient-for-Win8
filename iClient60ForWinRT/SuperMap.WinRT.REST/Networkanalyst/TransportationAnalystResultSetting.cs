

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransportationAnalystResultSetting_Title}</para>
    /// 	<para>${REST_TransportationAnalystResultSetting_Description}</para>
    /// </summary>
    public class TransportationAnalystResultSetting
    {
        /// <summary>${REST_TransportationAnalystResultSetting_constructor_D}</summary>
        public TransportationAnalystResultSetting()
        {
        }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnEdgeFeatures_D}</summary>
        public bool ReturnEdgeFeatures { get; set; }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnEdgeGeometry_D}</summary>
        public bool ReturnEdgeGeometry { get; set; }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnEdgeIDs_D}</summary>
        public bool ReturnEdgeIDs { get; set; }

        ///// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnImage_D}</summary>
        //public bool ReturnImage { get; set; }
        
        
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnNodeFeatures_D}</summary>
        public bool ReturnNodeFeatures { get; set; }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnNodeGeometry_D}</summary>
        public bool ReturnNodeGeometry { get; set; }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnNodeIDs_D}</summary>
        public bool ReturnNodeIDs { get; set; }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_ReturnPathGuides_D}</summary>
        public bool ReturnPathGuides { get; set; }
        /// <summary>${REST_TransportationAnalystResultSetting_attribute_returnRoutes_D}</summary>
        public bool ReturnRoutes { get; set; }

        internal static string ToJson(TransportationAnalystResultSetting param)
        {
            if (param != null)
            {
                string json = "{";
                List<string> list = new List<string>();

                list.Add(string.Format("\"returnEdgeFeatures\":{0}", param.ReturnEdgeFeatures.ToString().ToLower()));
                list.Add(string.Format("\"returnEdgeGeometry\":{0}", param.ReturnEdgeGeometry.ToString().ToLower()));
                list.Add(string.Format("\"returnEdgeIDs\":{0}", param.ReturnEdgeIDs.ToString().ToLower()));
                //list.Add(string.Format("\"returnImage\":{0}", param.ReturnImage.ToString().ToLower()));
                list.Add(string.Format("\"returnNodeFeatures\":{0}", param.ReturnNodeFeatures.ToString().ToLower()));
                list.Add(string.Format("\"returnNodeGeometry\":{0}", param.ReturnNodeGeometry.ToString().ToLower()));
                list.Add(string.Format("\"returnNodeIDs\":{0}", param.ReturnNodeIDs.ToString().ToLower()));
                list.Add(string.Format("\"returnPathGuides\":{0}", param.ReturnPathGuides.ToString().ToLower()));
                list.Add(string.Format("\"returnRoutes\":{0}", param.ReturnRoutes.ToString().ToLower()));

                json += string.Join(",", list.ToArray());
                json += "}";
                return json;
            }
            return null;
        }
    }
}
