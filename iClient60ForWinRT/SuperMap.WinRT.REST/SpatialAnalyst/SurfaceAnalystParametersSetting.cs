using System.Collections.Generic;
using System.Text;
using SuperMap.WinRT.Core;

namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_ExtractParamsSetting_Title}</para>
    /// 	<para>${REST_ExtractParamsSetting_Description}</para>
    /// </summary>
    public class SurfaceAnalystParametersSetting
    {
        /// <summary>${REST_ExtractParamsSetting_constructor_D}</summary>
        public SurfaceAnalystParametersSetting()
        { }
        /// <summary>${REST_ExtractParamsSetting_attribute_ClipRegion_D}</summary>
        public GeoRegion ClipRegion
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_DatumValue_D}</summary>
        public double DatumValue  //基准值
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_ExpectedZValues_D}</summary>
        public IList<double> ExpectedZValues
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_Interval_D}</summary>
        public double Interval
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_ResampleTolerance_D}</summary>
        public double ResampleTolerance
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_SmoothMethod_D}</summary>
        public SmoothMethod SmoothMethod
        {
            get;
            set;
        }
        /// <summary>${REST_ExtractParamsSetting_attribute_Smoothness_D}</summary>
        public int Smoothness
        {
            get;
            set;
        }

        internal static string ToJson(SurfaceAnalystParametersSetting surfaceAnalystSetting)
        {
            System.Text.StringBuilder json = new StringBuilder("{");
            if (surfaceAnalystSetting.ClipRegion != null)
            {
                json.AppendFormat("\"clipRegion\":{0},", ServerGeometry.ToJson(surfaceAnalystSetting.ClipRegion.ToServerGeometry()));
            }
            else
            {
                json.AppendFormat("\"clipRegion\":null,");
            }

            json.AppendFormat("\"datumValue\":{0},", surfaceAnalystSetting.DatumValue);

            if (surfaceAnalystSetting.ExpectedZValues != null && surfaceAnalystSetting.ExpectedZValues.Count > 0)
            {
                System.Collections.Generic.List<double> list = new List<double>();
                for (int i = 0; i < surfaceAnalystSetting.ExpectedZValues.Count; i++)
                {
                    list.Add(surfaceAnalystSetting.ExpectedZValues[i]);
                }

                json.AppendFormat("\"expectedZValues\":[{0}],", string.Join(",", list.ToArray()));
            }
            
            json.AppendFormat("\"interval\":{0},", surfaceAnalystSetting.Interval);

            json.AppendFormat("\"resampleTolerance\":{0},", surfaceAnalystSetting.ResampleTolerance);

            json.AppendFormat("\"smoothMethod\":\"{0}\",", surfaceAnalystSetting.SmoothMethod);

            json.AppendFormat("\"smoothness\":{0},", surfaceAnalystSetting.Smoothness);

            json.Append("}");
            return json.ToString();
        }
    }
}
