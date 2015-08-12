using SuperMap.WinRT.Utilities;
using SuperMap.WinRT.Core;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_MeasureResult_Tile}</para>
    /// 	<para>${REST_MeasureResult_Description}</para>
    /// </summary>
    public class MeasureResult
    {
        internal MeasureResult()
        { }

        /// <summary>${REST_MeasureResult_attribute_Area_D}</summary>
        public double Area { get; private set; }
        /// <summary>${REST_MeasureResult_attribute_Distance_D}</summary>
        public double Distance { get; private set; }
        /// <summary>${REST_MeasureResult_attribute_Unit_D}</summary>
        public Unit Unit { get; private set; }

        /// <summary>${REST_MeasureResult_method_fromJSON_D}</summary>
        /// <returns>${REST_MeasureResult_method_fromJSON_return}</returns>
        /// <param name="json">${REST_MeasureResult_method_fromJSON_param_json}</param>
        internal static MeasureResult FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new MeasureResult
            {
                Area = json["area"].GetNumberEx(),
                Distance =json["distance"].GetNumberEx(),
                Unit = (Unit)Enum.Parse(typeof(Unit),json["unit"].GetStringEx(), true)
            };
        }
    }
}
