using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_PointWithMeasure_Title}</para>
    /// 	<para>${REST_PointWithMeasure_Description}</para>
    /// </summary>
    public class PointWithMeasure
    {
        internal PointWithMeasure()
        {
        }

        /// <summary>${REST_Route_attribute_Measure_D}</summary>
        public double Measure { get; private set; }
        /// <summary>${REST_Route_attribute_X_D}</summary>
        public double X { get; private set; }
        /// <summary>${REST_Route_attribute_Y_D}</summary>
        public double Y { get; private set; }

        /// <summary>${REST_PointWithMeasure_method_fromJson_D}</summary>
        /// <returns>${REST_PointWithMeasure_method_fromJson_return}</returns>
        /// <param name="json">${REST_PointWithMeasure_method_fromJson_param_jsonObject}</param>
        internal static PointWithMeasure FromJson(JsonObject json)
        {
            if (json == null)
                return null;
            return new PointWithMeasure { Measure = json["measure"].GetNumberEx(),X=json["x"].GetNumberEx(),Y=json["y"].GetNumberEx() };
        }
    }
}
