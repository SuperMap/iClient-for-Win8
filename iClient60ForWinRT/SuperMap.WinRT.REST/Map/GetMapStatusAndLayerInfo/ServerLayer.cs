using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using Windows.UI;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ServerLayer_Title}</para>
    /// 	<para>${REST_ServerLayer_Description}</para>
    /// </summary>
    public class ServerLayer
    {
        /// <summary>${REST_ServerLayer_constructor_D}</summary>
        public ServerLayer()
        { }

        //Property
        /// <summary>${REST_ServerLayer_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds
        {
            get;
            internal set;
        }

        //对应服务端的Layer属性
        /// <summary>${REST_ServerLayer_attribute_caption_D}</summary>
        public string Caption { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_Description_D}</summary>
        public string Description { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_Name_D}</summary>
        public string Name { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_IsQueryable_D}</summary>
        public bool IsQueryable { get; internal set; }

        //图层的子图层先不控制
        //public System.Collections.Generic.List<LayerInfo> SubLayers { get; internal set; }

        //这里默认是UGC了，不开放给用户啦
        //private string LayerType = "UGC";
        /// <summary>${REST_ServerLayer_attribute_IsVisible_D}</summary>
        public bool IsVisible { get; internal set; }

        //对应服务端UGCMapLayer属性
        /// <summary>${REST_ServerLayer_attribute_IsCompleteLineSymbolDisplayed_D}</summary>
        public bool IsCompleteLineSymbolDisplayed { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_MaxScale_D}</summary>
        public double MaxScale { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_minScale_D}</summary>
        public double MinScale { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_MinVisibleGeometrySize_D}</summary>
        public double MinVisibleGeometrySize { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_OpaqueRate_D}</summary>
        public int OpaqueRate { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_IsSymbolScalable_D}</summary>
        public bool IsSymbolScalable { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_SymbolScale_D}</summary>
        public double SymbolScale { get; internal set; }

        //对应服务端UGCLayer
        /// <summary>${REST_ServerLayer_attribute_DatasetInfo_D}</summary>
        public DatasetInfo DatasetInfo { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_DisplayFilter_D}</summary>
        public string DisplayFilter { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_JoinItems_D}</summary>
        public System.Collections.Generic.List<JoinItem> JoinItems { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_RepresentationField_D}</summary>
        public string RepresentationField { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_UGCLayerType_D}</summary>
        public SuperMapLayerType UGCLayerType { get; internal set; }
        /// <summary>${REST_ServerLayer_attribute_UGCLayer_D}</summary>
        public UGCLayer UGCLayer { get; internal set; }
        /// <summary>${REST_ServerLayer_method_FromJson_D}</summary>
        /// <returns>${REST_ServerLayer_method_FromJson_return}</returns>
        /// <param name="json">${REST_ServerLayer_method_FromJson_param_jsonObject}</param>
        internal static ServerLayer FromJson(JsonObject json)
        {
            var serverLayer = new ServerLayer();
            if (json["bounds"].ValueType != JsonValueType.Null)
            {
                serverLayer.Bounds = JsonHelper.ToRectangle2D(json["bounds"].GetObjectEx());
            }
            else
            {
                //null
            }
            serverLayer.Caption = json["caption"].GetStringEx();
            serverLayer.Description = json["description"].GetStringEx();
            serverLayer.Name = json["name"].GetStringEx();
            serverLayer.IsQueryable = json["queryable"].GetBooleanEx();
            serverLayer.IsVisible = json["visible"].GetBooleanEx();
            serverLayer.IsCompleteLineSymbolDisplayed = json["completeLineSymbolDisplayed"].GetBooleanEx();
            serverLayer.MaxScale = json["maxScale"].GetNumberEx();
            serverLayer.MinScale = json["minScale"].GetNumberEx();
            serverLayer.MinVisibleGeometrySize = json["minVisibleGeometrySize"].GetNumberEx();
            serverLayer.OpaqueRate = (int)json["opaqueRate"].GetNumberEx();
            serverLayer.IsSymbolScalable = json["symbolScalable"].GetBooleanEx();
            serverLayer.SymbolScale = json["symbolScale"].GetNumberEx();
            serverLayer.DatasetInfo = DatasetInfo.FromJson(json["datasetInfo"].GetObjectEx());
            serverLayer.DisplayFilter = json["displayFilter"].GetStringEx();

            if (json["joinItems"].ValueType != JsonValueType.Null)
            {
                List<JoinItem> joinItems = new List<JoinItem>();
                foreach (JsonValue item in json["joinItems"].GetArray())
                {
                    joinItems.Add(JoinItem.FromJson(item.GetObjectEx()));
                }
                serverLayer.JoinItems = joinItems;
            }
            serverLayer.RepresentationField = json["representationField"].GetStringEx();

            if (json["ugcLayerType"].GetStringEx() == SuperMapLayerType.GRID.ToString())
            {
                UGCGridLayer ugcGridLayer = new UGCGridLayer();

                List<Color> colors = new List<Color>();
                foreach (JsonValue colorItem in json["colors"].GetArray())
                {
                    colors.Add(ServerColor.FromJson(colorItem.GetObjectEx()).ToColor());
                }
                ugcGridLayer.Colors = colors;

                if (json["dashStyle"].ValueType != JsonValueType.Null)
                {
                    ugcGridLayer.DashStyle = ServerStyle.FromJson(json["dashStyle"].GetObjectEx());
                }
                if (json["gridType"].ValueType != JsonValueType.Null)
                {
                    ugcGridLayer.GridType = (GridType)Enum.Parse(typeof(GridType), json["gridType"].GetStringEx(), true);
                }
                else
                {
                }

                ugcGridLayer.HorizontalSpacing = json["horizontalSpacing"].GetNumberEx();
                ugcGridLayer.SizeFixed = json["sizeFixed"].GetBooleanEx();

                if (json["solidStyle"].ValueType != JsonValueType.Null)
                {
                    ugcGridLayer.SolidStyle = ServerStyle.FromJson(json["solidStyle"].GetObjectEx());
                }

                if (json["specialColor"].ValueType != JsonValueType.Null)
                {
                    ugcGridLayer.SpecialColor = ServerColor.FromJson(json["specialColor"].GetObjectEx()).ToColor();
                }
                ugcGridLayer.SpecialValue = json["specialValue"].GetNumberEx();
                ugcGridLayer.VerticalSpacing = json["verticalSpacing"].GetNumberEx();
                serverLayer.UGCLayer = ugcGridLayer;
            }

            else if (json["ugcLayerType"].GetStringEx() == SuperMapLayerType.IMAGE.ToString())
            {
                UGCImageLayer ugcImageLayer = new UGCImageLayer();
                ugcImageLayer.Brightness = (int)json["brightness"].GetNumberEx();
                if (json["colorSpaceType"].ValueType != JsonValueType.Null)
                {
                    ugcImageLayer.ColorSpaceType = (ColorSpaceType)Enum.Parse(typeof(ColorSpaceType), json["colorSpaceType"].GetStringEx(), true);
                }
                else
                {
                }
                ugcImageLayer.Contrast = (int)json["contrast"].GetNumberEx();

                List<int> bandIndexes = new List<int>();
                if (json["displayBandIndexes"].ValueType != JsonValueType.Null && (json["displayBandIndexes"].GetArray()).Count > 0)
                {
                    foreach (JsonObject item in json["displayBandIndexes"].GetArray())
                    {
                        bandIndexes.Add((int)item.GetNumber());
                    }

                    ugcImageLayer.DisplayBandIndexes = bandIndexes;
                }

                ugcImageLayer.Transparent = json["transparent"].GetBooleanEx();
                ugcImageLayer.TransparentColor = ServerColor.FromJson(json["transparentColor"].GetObjectEx()).ToColor();
                serverLayer.UGCLayer = ugcImageLayer;
            }

            else if (json["ugcLayerType"].GetStringEx() == SuperMapLayerType.THEME.ToString())
            {
                UGCThemeLayer ugcThemeLayer = new UGCThemeLayer();
                if (json["theme"].ValueType != JsonValueType.Null)
                {

                    if (json["theme"].GetObjectEx()["type"].GetStringEx() == "UNIQUE")
                    {
                        ugcThemeLayer.Theme = ThemeUnique.FromJson(json["theme"].GetObjectEx());
                    }

                    else if (json["theme"].GetObjectEx()["type"].GetStringEx() == "RANGE")
                    {
                        ugcThemeLayer.Theme = ThemeRange.FromJson(json["theme"].GetObjectEx());
                    }

                    else if (json["theme"].GetObjectEx()["type"].GetStringEx() == "LABEL")
                    {
                        ugcThemeLayer.Theme = ThemeLabel.FromJson(json["theme"].GetObjectEx());
                    }

                    else if (json["theme"].GetObjectEx()["type"].GetStringEx() == "GRAPH")
                    {
                        ugcThemeLayer.Theme = ThemeGraph.FromJson(json["theme"].GetObjectEx());
                    }

                    else if (json["theme"].GetObjectEx()["type"].GetStringEx() == "DOTDENSITY")
                    {
                        ugcThemeLayer.Theme = ThemeDotDensity.FromJson(json["theme"].GetObjectEx());
                    }

                    else if (json["theme"].GetObjectEx()["type"].GetStringEx() == "GRADUATEDSYMBOL")
                    {
                        ugcThemeLayer.Theme = ThemeGraduatedSymbol.FromJson(json["theme"].GetObjectEx());
                    }
                    else
                    {
                        //以后有需求再添加，现在就写到这里，共六个专题图。
                    }
                }
                if (json["theme"].GetObjectEx()["type"].ValueType != JsonValueType.Null)
                {
                    ugcThemeLayer.ThemeType = (ThemeType)Enum.Parse(typeof(ThemeType), json["theme"].GetObjectEx()["type"].GetStringEx(), true);
                }
                serverLayer.UGCLayer = ugcThemeLayer;
                //ugcThemeLayer.Theme
            }

            else if (json["ugcLayerType"].GetStringEx() == SuperMapLayerType.VECTOR.ToString() && json.ContainsKey("style"))
            {
                serverLayer.UGCLayer = UGCVectorLayer.FromJson(json["style"].GetObjectEx());
            }
            else
            {
                serverLayer.UGCLayer = new UGCLayer();
            }
            if (json["ugcLayerType"].ValueType != JsonValueType.Null)
            {
                serverLayer.UGCLayerType = (SuperMapLayerType)Enum.Parse(typeof(SuperMapLayerType), json["ugcLayerType"].GetStringEx(), true);
            }
            else
            {
                //不做处理
            }

            //这里不判断WMS和WFS图层。
            //else if (json["ugcLayerType"] == SuperMapLayerType.WMS.ToString())
            //{

            //}
            //根据图层类型增加相应属性。
            return serverLayer;
        }
    }
}
