using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_Route_Title}</para>
    /// 	<para>${REST_Route_Description}</para>
    /// </summary>
    public class Route
    {
        internal Route()
        {
        }
        /// <summary>${REST_Route_attribute_Length_D}</summary>
        public double Length { get; private set; }
        /// <summary>${REST_Route_attribute_Line_D}</summary>
        public Geometry Line { get; private set; }
        /// <summary>${REST_Route_attribute_MaxM_D}</summary>
        public double MaxM { get; private set; }
        /// <summary>${REST_Route_attribute_MinM_D}</summary>
        public double MinM { get; private set; }
        /// <summary>${REST_Route_attribute_Points_D}</summary>
        public List<PointWithMeasure> Points { get; private set; }
        /// <summary>${REST_Route_attribute_Region_D}</summary>
        public Geometry Region { get; private set; }

        /// <summary>${REST_Route_method_fromJson_D}</summary>
        /// <returns>${REST_Route_method_fromJson_return}</returns>
        /// <param name="json">${REST_Route_method_fromJson_param_jsonObject}</param>
        internal static Route FromJson(JsonObject json)
        {
            if (json == null)
                return null;
            Route result = new Route();

            result.Length = json["length"].GetNumberEx();
            result.Line = ServerGeometry.FromJson(json["line"].GetObjectEx()).ToGeometry();
            result.MaxM = json["maxM"].GetNumberEx();
            result.MinM = json["minM"].GetNumberEx();
            if (json["points"].ValueType !=JsonValueType.Null)
            {
                result.Points = new List<PointWithMeasure>();
                for (int i = 0; i < json["points"].GetArray().Count; i++)
                {
                    result.Points.Add(PointWithMeasure.FromJson(json["points"].GetArray()[i].GetObjectEx()));
                }
            }
            result.Region = ServerGeometry.FromJson(json["region"].GetObjectEx()).ToGeometry();

            return result;
        }
    }
}
