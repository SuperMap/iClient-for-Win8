using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeGraphItem_Title}</para>
    /// 	<para>${REST_ThemeGraphItem_Description}</para>
    /// </summary>
    public class ThemeGraphItem
    {
        /// <summary>${REST_ThemeGraphItem_constructor_D}</summary>
        public ThemeGraphItem() { }

        /// <summary>${REST_ThemeGraphItem_attribute_caption_D}</summary>
        public String Caption
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphItem_attribute_graphExpression_D}</summary>   
        public String GraphExpression
        {
            get;
            set;
        }
        ///// <summary>${REST_ThemeGraphItem_attribute_rangeSetting_D}</summary>      
        //public ThemeRange RangeSetting 
        //{ 
        //    get; 
        //    set; 
        //}
        /// <summary>${REST_ThemeGraphItem_attribute_uniformStyle_D}</summary> 
        public ServerStyle UniformStyle
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraphItem_attribute_memoryDoubleValues_D}</summary> 
        public IList<double> MemoryDoubleValues
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraphItem graphItem)
        {
            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(graphItem.Caption))
            {
                list.Add(string.Format("\"caption\":\"{0}\"", graphItem.Caption));
            }
            else
            {
                list.Add("\"caption\":null");
            }

            if (!string.IsNullOrEmpty(graphItem.GraphExpression))
            {
                list.Add(string.Format("\"graphExpression\":\"{0}\"", graphItem.GraphExpression));
            }
            else
            {
                list.Add("\"graphExpression\":null");
            }

            //if (graphItem.RangeSetting != null)
            //{
            //    list.Add(string.Format("\"rangeSetting\":{0}", ThemeRange.ToJson(graphItem.RangeSetting)));
            //}
            //else
            //{
            //    list.Add("\"rangeSetting\":null");
            //}

            if (graphItem.UniformStyle != null)
            {
                list.Add(string.Format("\"uniformStyle\":{0}", ServerStyle.ToJson(graphItem.UniformStyle)));
            }
            else
            {
                list.Add(string.Format("\"uniformStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (graphItem.MemoryDoubleValues != null && graphItem.MemoryDoubleValues.Count > 0)
            {
                list.Add(string.Format("\"memoryDoubleValues\":[{0}]", string.Join(",", graphItem.MemoryDoubleValues)));
            }
            else
            {
                list.Add("\"memoryDoubleValues\":null");
            }
            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ThemeGraphItem FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeGraphItem graphItem = new ThemeGraphItem();
            graphItem.Caption = json["caption"].GetStringEx();
            graphItem.GraphExpression = json["graphExpression"].GetStringEx();
            if (json["memoryDoubleValues"] != null)
            {
                List<double> memoryValues = new List<double>();
                foreach (JsonObject item in json["memoryDoubleValues"].GetArray())
                {
                    memoryValues.Add(item.GetNumber());
                }

                graphItem.MemoryDoubleValues = memoryValues;
            }

            graphItem.UniformStyle = ServerStyle.FromJson(json["uniformStyle"].GetObjectEx());
            return graphItem;
        }
    }
}
