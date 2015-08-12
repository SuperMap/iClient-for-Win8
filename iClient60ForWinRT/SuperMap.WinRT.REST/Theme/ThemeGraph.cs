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
    /// 	<para>${REST_ThemeGraph_Title}</para>
    /// 	<para>${REST_ThemeGraph_Description}</para>
    /// 	<para><img src="themeGraph_iServer6.bmp"/></para>
    /// </summary>
    public class ThemeGraph : Theme
    {
        /// <summary>${REST_ThemeGraph_constructor_D}</summary>
        public ThemeGraph()
        {

        }

        /// <summary>${REST_ThemeGraph_attribute_barWidth_D}</summary>
        public double BarWidth
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_graduatedMode_D}</summary>
        public GraduatedMode GraduatedMode
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_graphSizeFixed_D}</summary>
        public bool GraphSizeFixed
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_graphText_D}</summary>
        public ThemeGraphText GraphText
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_graphType_D}</summary>
        public ThemeGraphType GraphType
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_items_D}</summary>
        public IList<ThemeGraphItem> Items
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_flow_D}</summary>
        public ThemeFlow Flow
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_graphSize_D}</summary>
        public ThemeGraphSize GraphSize
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_offset_D}</summary>
        public ThemeOffset Offset
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_overlapAvoided_D}</summary>
        public bool OverlapAvoided
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_roseAngle_D}</summary>
        public double RoseAngle
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_startAngle_D}</summary>
        public double StartAngle
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_graphAxes_D}</summary>
        public ThemeGraphAxes GraphAxes
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_negativeDisplayed_D}</summary>
        public bool NegativeDisplayed
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeGraph_attribute_memoryKeys_D}</summary>
        public IList<int> MemoryKeys
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraph themeGraph)
        {
            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format("\"barWidth\":{0}", themeGraph.BarWidth.ToString()));

            if (themeGraph.Flow != null)
            {
                list.Add(ThemeFlow.ToJson(themeGraph.Flow));
            }
            else
            {
                list.Add(string.Format("\"leaderLineStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
                list.Add("\"flowEnabled\":true");
                list.Add("\"leaderLineDisplayed\":false");
            }

            list.Add(string.Format("\"graduatedMode\":\"{0}\"", themeGraph.GraduatedMode));

            if (themeGraph.GraphAxes != null)
            {
                list.Add(ThemeGraphAxes.ToJson(themeGraph.GraphAxes));
            }
            else
            {
                list.Add(string.Format("\"axesColor\":{0}", ServerColor.ToJson(new ServerColor())));
                list.Add("\"axesDisplayed\":false");
                list.Add("\"axesGridDisplayed\":false");
                list.Add(string.Format("\"axesTextStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
                list.Add("\"axesTextDisplayed\":false");
            }

            if (themeGraph.GraphSize != null)
            {
                list.Add(ThemeGraphSize.ToJson(themeGraph.GraphSize));
            }
            else
            {
                list.Add(string.Format("\"maxGraphSize\":0.0"));
                list.Add(string.Format("\"minGraphSize\":0.0"));
            }

            list.Add(string.Format("\"graphSizeFixed\":{0}", themeGraph.GraphSizeFixed.ToString().ToLower()));

            if (themeGraph.GraphText != null)
            {
                list.Add(ThemeGraphText.ToJson(themeGraph.GraphText));
            }
            else
            {
                list.Add(string.Format("\"graphTextStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
                list.Add("\"graphTextDisplayed\":false");
                list.Add("\"graphTextFormat\":\"CAPTION\"");
            }

            list.Add(string.Format("\"graphType\":\"{0}\"", themeGraph.GraphType));

            if (themeGraph.Items != null && themeGraph.Items.Count > 0)
            {
                List<string> itemList = new List<string>();
                foreach (var item in themeGraph.Items)
                {
                    itemList.Add(ThemeGraphItem.ToJson(item));

                }
                list.Add(string.Format("\"items\":[{0}]", string.Join(",", itemList.ToArray())));
            }
            else
            {
                list.Add("\"items\":null");
            }

            list.Add(string.Format("\"negativeDisplayed\":{0}", themeGraph.NegativeDisplayed.ToString().ToLower()));

            if (themeGraph.Offset != null)
            {
                list.Add(ThemeOffset.ToJson(themeGraph.Offset));
            }
            else
            {
                list.Add("\"offsetX\":\"\"");
                list.Add("\"offsetY\":\"\"");
                list.Add("\"offsetFixed\":false");
            }

            list.Add(string.Format("\"overlapAvoided\":{0}", themeGraph.OverlapAvoided.ToString().ToLower()));

            list.Add(string.Format("\"roseAngle\":{0}", themeGraph.RoseAngle.ToString()));
            list.Add(string.Format("\"startAngle\":{0}", themeGraph.StartAngle.ToString()));

            list.Add("\"memoryData\":null");
            list.Add("\"type\":\"GRAPH\"");

            if (themeGraph.MemoryKeys != null && themeGraph.MemoryKeys.Count > 0)
            {
                list.Add(string.Format("\"memoryKeys\":[{0}]", string.Join(",", themeGraph.MemoryKeys)));
            }
            else
            {
                list.Add("\"memoryKeys\":[]");
            }

            json += string.Join(",", list.ToArray());
            json += "}";

            return json;
        }

        internal static ThemeGraph FromJson(JsonObject json)
        {
            if (json == null) return null;

            ThemeGraph themeGraph = new ThemeGraph();
            themeGraph.BarWidth = json["barWidth"].GetNumberEx();
            themeGraph.Flow = ThemeFlow.FromJson(json);
            if (json["graduatedMode"].ValueType!= JsonValueType.Null)
            {
                themeGraph.GraduatedMode = (GraduatedMode)Enum.Parse(typeof(GraduatedMode), json["graduatedMode"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }
            themeGraph.GraphAxes = ThemeGraphAxes.FromJson(json);
            themeGraph.GraphSize = ThemeGraphSize.FromJson(json);
            themeGraph.GraphSizeFixed = json["graphSizeFixed"].GetBooleanEx();
            themeGraph.GraphText = ThemeGraphText.FromJson(json);
            if (json["graphType"].ValueType != JsonValueType.Null)
            {
                themeGraph.GraphType = (ThemeGraphType)Enum.Parse(typeof(ThemeGraphType), json["graphType"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }

            if (json["items"].ValueType != JsonValueType.Null)
            {
                List<ThemeGraphItem> graphItems = new List<ThemeGraphItem>();
                foreach (JsonObject item in json["items"].GetArray())
                {
                    graphItems.Add(ThemeGraphItem.FromJson(item));
                }
                themeGraph.Items = graphItems;
            }

            if (json["memoryKeys"].ValueType!= JsonValueType.Null)
            {
                List<int> memoryKeysLsit = new List<int>();
                foreach (JsonObject item in json["memoryKeys"].GetArray())
                {
                    memoryKeysLsit.Add((int)item.GetNumber());
                }
                themeGraph.MemoryKeys = memoryKeysLsit;
            }

            themeGraph.NegativeDisplayed = json["negativeDisplayed"].GetBooleanEx();
            themeGraph.Offset = ThemeOffset.FromJson(json);
            themeGraph.OverlapAvoided = json["overlapAvoided"].GetBooleanEx();
            themeGraph.RoseAngle = json["roseAngle"].GetNumberEx();
            themeGraph.StartAngle = json["startAngle"].GetNumberEx();
            return themeGraph;
        }
    }
}
