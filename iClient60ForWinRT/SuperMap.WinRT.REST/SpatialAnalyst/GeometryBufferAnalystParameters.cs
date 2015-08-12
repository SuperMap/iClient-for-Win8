
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryBufferAnalystParams_Title}</para>
    /// 	<para>${REST_GeometryBufferAnalystParams_Description}</para>
    /// </summary>
    public class GeometryBufferAnalystParameters : BufferAnalystParameters
    {
        /// <summary>${REST_GeometryBufferAnalystParams_constructor_D}</summary>
        public GeometryBufferAnalystParameters()
        {
        }
        /// <summary>${REST_GeometryBufferAnalystParams_attribute_sourceGeometry_D}</summary>
        public Geometry SourceGeometry
        {
            get;
            set;
        }

        internal static System.Collections.Generic.Dictionary<string, string> ToDictionary(GeometryBufferAnalystParameters geometryBufferParams)
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>();
            if (geometryBufferParams.BufferSetting != null)
            {
                dict.Add("analystParameter", BufferSetting.ToJson(geometryBufferParams.BufferSetting));
            }
            else
            {
                dict.Add("analystParameter", BufferSetting.ToJson(new BufferSetting()));
            }

            if (geometryBufferParams.SourceGeometry != null)
            {
                dict.Add("sourceGeometry", ServerGeometry.ToJson(geometryBufferParams.SourceGeometry.ToServerGeometry()));
            }
            else
            {
                dict.Add("sourceGeometry", ServerGeometry.ToJson(new ServerGeometry()));
            }
            return dict;
        }
    }
}
