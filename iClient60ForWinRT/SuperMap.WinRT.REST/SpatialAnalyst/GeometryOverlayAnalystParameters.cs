
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryOverlayAnalystParams_Title}</para>
    /// 	<para>${REST_GeometryOverlayAnalystParams_Description}</para>
    /// </summary>
    public class GeometryOverlayAnalystParameters : OverlayAnalystParameters
    {
        /// <summary>${REST_GeometryOverlayAnalystParams_constructor_D}</summary>
        public GeometryOverlayAnalystParameters()
        { }
        /// <summary>${REST_GeometryOverlayAnalystParams_attribute_SourceGeometry_D}</summary>
        public Geometry SourceGeometry
        {
            get;
            set;
        }
        /// <summary>${REST_GeometryOverlayAnalystParams_attribute_OperateGeometry_D}</summary>
        public Geometry OperateGeometry
        {
            get;
            set;
        }

        internal static System.Collections.Generic.Dictionary<string, string> ToDictionary(GeometryOverlayAnalystParameters geometryOverlayParams)
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>();
            if (geometryOverlayParams.SourceGeometry != null)
            {
                dict.Add("sourceGeometry", ServerGeometry.ToJson(geometryOverlayParams.SourceGeometry.ToServerGeometry()));
            }
            else
            {
                dict.Add("sourceGeometry", ServerGeometry.ToJson(new ServerGeometry()));
            }

            if (geometryOverlayParams.OperateGeometry != null)
            {
                dict.Add("operateGeometry", ServerGeometry.ToJson(geometryOverlayParams.OperateGeometry.ToServerGeometry()));
            }
            else
            {
                dict.Add("operateGeometry", ServerGeometry.ToJson(new ServerGeometry()));
            }

            dict.Add("operation", "\"" + geometryOverlayParams.Operation.ToString() + "\"");
            return dict;
        }
    }
}
