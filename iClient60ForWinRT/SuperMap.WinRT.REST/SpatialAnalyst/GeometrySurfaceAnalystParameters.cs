using System.Collections.Generic;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_GeometryExtractParams_Title}</para>
    /// 	<para>${REST_GeometryExtractParams_Description}</para>
    /// </summary>
    /// <remarks>${REST_GeometrySurfaceAnalystParms_Remarks}</remarks>
    public class GeometrySurfaceAnalystParameters : SurfaceAnalystParameters
    {
        /// <summary>${REST_GeometryExtractParams_constructor_D}</summary>
        public GeometrySurfaceAnalystParameters()
        { }
        /// <summary>${REST_GeometryExtractParams_attribute_Points_D}</summary>
        public Point2DCollection Points
        {
            get;
            set;
        }
        /// <summary>${REST_GeometryExtractParams_attribute_ZValues_D}</summary>
        public IList<double> ZValues
        {
            get;
            set;
        }

        internal static System.Collections.Generic.Dictionary<string, string> ToDictionary(GeometrySurfaceAnalystParameters geometrySurfaceAnalystParams)
        {
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();

            if (geometrySurfaceAnalystParams.Points != null && geometrySurfaceAnalystParams.Points.Count > 0)
            {
                List<string> ps = new List<string>();
                foreach (Point2D p in geometrySurfaceAnalystParams.Points)
                {
                    ps.Add(JsonHelper.FromPoint2D(p));
                }
                dict.Add("points", "[" + string.Join(",", ps.ToArray()) + "]");
            }
            else
            {
                dict.Add("points", "null");
            }

            if (geometrySurfaceAnalystParams.ZValues != null && geometrySurfaceAnalystParams.ZValues.Count > 0)
            {
                List<double> list = new List<double>();
                for (int i = 0; i < geometrySurfaceAnalystParams.ZValues.Count; i++)
                {
                    list.Add(geometrySurfaceAnalystParams.ZValues[i]);
                }

                dict.Add("zValues", "[" + string.Join(",", list.ToArray()) + "]");
            }
            else
            {
                dict.Add("zValues", "[]");
            }

            if (geometrySurfaceAnalystParams.ParametersSetting != null)
            {
                dict.Add("extractParameter", SurfaceAnalystParametersSetting.ToJson(geometrySurfaceAnalystParams.ParametersSetting));
            }
            else
            {
                dict.Add("extractParameter", SurfaceAnalystParametersSetting.ToJson(new SurfaceAnalystParametersSetting()));
            }

            string resultSetting = string.Format("\"dataReturnMode\":\"RECORDSET_ONLY\",\"expectCount\":{0}", geometrySurfaceAnalystParams.MaxReturnRecordCount);
            resultSetting = "{" + resultSetting + "}";
            dict.Add("resultSetting", resultSetting);

            dict.Add("resolution", geometrySurfaceAnalystParams.Resolution.ToString());
            return dict;
        }
    }
}
