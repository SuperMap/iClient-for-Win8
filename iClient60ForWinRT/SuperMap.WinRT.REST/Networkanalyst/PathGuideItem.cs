

using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_PathGuideItem_Title}</para>
    /// 	<para>${REST_PathGuideItem_Description}</para>
    /// </summary>
    public class PathGuideItem
    {
        internal PathGuideItem()
        {
        }

        /// <summary>${REST_PathGuideItem_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_DirectionType_D}</summary>
        public DirectionType DirectionType { get; private set; }
        /// <summary><para>${REST_PathGuideItem_attribute_Distance_D}</para>
        /// <para><img src="tolerance.png"/></para>
        /// </summary>
        public double Distance { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_ID_D}</summary>
        public int ID { get; private set; }
        /// <summary><para>${REST_PathGuideItem_attribute_Index_D}</para>
        ///         <para><img src="PathGuideItemIndex.png"/></para>
        /// </summary>
        public int Index { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_IsEdge_D}</summary>
        public bool IsEdge { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_isStop_D}</summary>
        public bool IsStop { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_Length_D}</summary>
        public double Length { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_Name_D}</summary>
        public string Name { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_SideType_D}</summary>
        public SideType SideType { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_TurnAngle_D}</summary>
        public double TurnAngle { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_TurnType_D}</summary>
        public TurnType TurnType { get; private set; }
        /// <summary>${REST_PathGuideItem_attribute_Weight_D}</summary>
        public double Weight { get; private set; }

        /// <summary>${REST_PathGuideItem_method_fromJson_D}</summary>
        /// <returns>${REST_PathGuideItem_method_fromJson_return}</returns>
        /// <param name="json">${REST_PathGuideItem_method_fromJson_param_jsonObject}</param>
        internal static PathGuideItem FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            PathGuideItem item = new PathGuideItem();

            if (json.ContainsKey("bounds"))
            {
                item.Bounds = JsonHelper.ToRectangle2D(json["bounds"].GetObjectEx());
            }

            if (json.ContainsKey("directionType"))
            {
                item.DirectionType = (DirectionType)Enum.Parse(typeof(DirectionType), json["directionType"].GetStringEx(), true);
            }

            if (json.ContainsKey("distance"))
            {
                item.Distance = json["distance"].GetNumberEx();
            }

            if (json.ContainsKey("id"))
            {
                item.ID = (int)json["id"].GetNumberEx();
            }

            if (json.ContainsKey("index"))
            {
                item.Index = (int)json["index"].GetNumberEx();
            }

            if (json.ContainsKey("isEdge"))
            {
                item.IsEdge = json["isEdge"].GetBooleanEx();
            }


            if (json.ContainsKey("isStop"))
            {
                item.IsStop = json["isStop"].GetBooleanEx();
            }

            if (json.ContainsKey("length"))
            {
                item.Length = json["length"].GetNumberEx();
            }

            if (json.ContainsKey("name"))
            {
                item.Name = json["name"].GetStringEx();
            }

            if (json.ContainsKey("sideType"))
            {
                item.SideType = (SideType)Enum.Parse(typeof(SideType), json["sideType"].GetStringEx(), true);
            }
            try
            {
                if (json.ContainsKey("turnAngle"))
                {
                    item.TurnAngle = json["turnAngle"].GetNumberEx();
                }
            }
            finally
            {
                item.TurnAngle = 0.0;
            }

            if (json.ContainsKey("turnType"))
            {
                item.TurnType = (TurnType)Enum.Parse(typeof(TurnType), json["turnType"].GetStringEx(), true);
            }

            if (json.ContainsKey("weight"))
            {
                item.Weight = json["weight"].GetNumberEx();
            }
            return item;
        }
    }
}
