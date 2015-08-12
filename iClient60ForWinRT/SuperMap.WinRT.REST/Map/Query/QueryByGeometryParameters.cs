

using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_QueryByGeometryParameters_Tile}</para>
    /// 	<para>${REST_QueryByGeometryParameters_Description}</para>
    /// </summary>
    public class QueryByGeometryParameters:QueryParameters
    {
        /// <summary>${REST_QueryByGeometryParameters_constructor_None_D}</summary>
        public QueryByGeometryParameters()
        { }

        /// <summary>${REST_QueryByGeometryParameters_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }
        /// <summary>${REST_QueryByGeometryParameters_attribute_SpatialQueryMode_D}</summary>
        public SpatialQueryMode SpatialQueryMode { get; set; }

    }
}
