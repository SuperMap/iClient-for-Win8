

using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_QueryByDistanceParameters_Tile}</para>
    /// 	<para>${REST_QueryByDistanceParameters_Description}</para>
    /// </summary>
    public class QueryByDistanceParameters : QueryParameters
    {
        /// <summary>${REST_QueryByDistanceParameters_constructor_None_D}</summary>
        public QueryByDistanceParameters()
        { }
        /// <summary>${REST_QueryByDistanceParameters_attribute_Distance_D}</summary>
        public double Distance { get; set; }

        /// <summary>${REST_QueryByDistanceParameters_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }

        /// <summary>${REST_QueryByDistanceParameters_attribute_IsNearest_D}</summary>
        public bool IsNearest { get; set; }
    }
}
