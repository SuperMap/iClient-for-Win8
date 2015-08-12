using System.Collections.Generic;
using System.Globalization;
using SuperMap.Web.Core;

namespace SuperMap.Web.OGC
{
    /// <summary>${mapping_Spatial_Tile}</summary>
    public abstract class Spatial : Filter
    {
        /// <summary>${mapping_Spatial_constructor_D}</summary>
        protected Spatial()
        { }

        /// <summary>${mapping_Spatial_attribute_Type_D}</summary>
        public SpatialType Type { get; set; }
        /// <summary>${mapping_Spatial_attribute_PropertyName_D}</summary>
        public string PropertyName { get; set; }

        ////The distance to use in a DWithin spatial filter.
        ///// <summary>${mapping_Spatial_attribute_Distance_D}</summary>
        //public double Distance { get; set; }

        internal string GetCoorStr(Point2D p)
        {
            string str = string.Empty;
            str = p.ToString(CultureInfo.InvariantCulture);
            return str;
        }

        internal string GetCoorStr(Point2D p1, Point2D p2)
        {
            string str = string.Empty;
            if (!p1.IsEmpty && !p2.IsEmpty)
            {
                str = p1.ToString(CultureInfo.InvariantCulture) + " " + p2.ToString(CultureInfo.InvariantCulture);
            }

            return str;
        }

        internal string GetCoorStr(Point2DCollection ps)
        {
            string str = string.Empty;
            List<string> strCol = new List<string>();
            if (ps != null)
            {
                foreach (Point2D item in ps)
                {
                    strCol.Add(item.ToString(CultureInfo.InvariantCulture));
                }

                str = string.Join(" ", strCol);
            }

            return str;
        }
    }

    /// <summary>${mapping_SpatialType_Tile}</summary>
    public enum SpatialType
    {
        /// <summary>${mapping_SpatialType_attribute_BBOX_D}</summary>
        BBOX,           //SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Intersects_D}</summary>
        Intersects,     //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Within_D}</summary>
        Within,         //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Contains_D}</summary>
        Contains,       //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Equals_D}</summary>
        Equals,         //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Disjoint_D}</summary>
        Disjoint,       //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Touches_D}</summary>
        Touches,        //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Overlaps_D}</summary>
        Overlaps,       //SpatialGeometry,SpatialRect
        /// <summary>${mapping_SpatialType_attribute_Crosses_D}</summary>
        Crosses,        //SpatialGeometry,SpatialRect

        //目前iServer6R服务端不支持,geoServer支持。
        /// <summary>${mapping_SpatialType_attribute_DWithin_D}</summary>
        DWithin,//ogc:DistanceBufferType
        /// <summary>${mapping_SpatialType_attribute_Beyond_D}</summary>
        Beyond,//ogc:DistanceBufferType
    }
}
