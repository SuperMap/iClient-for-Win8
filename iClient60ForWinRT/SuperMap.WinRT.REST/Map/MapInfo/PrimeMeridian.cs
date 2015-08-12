using SuperMap.WinRT.Utilities;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_PrimeMeridian_Title}</para>
    /// 	<para>${REST_PrimeMeridian_Description}</para>
    /// </summary>
    public class PrimeMeridian
    {
        /// <summary>${REST_PrimeMeridian_constructor_D}</summary>
        public PrimeMeridian()
        { }
        /// <summary>${REST_PrimeMeridian_attribute_Name_D}</summary>
        public string Name { get; set; }
        /// <summary>${REST_PrimeMeridian_attribute_LongitudeValue_D}</summary>
        public double LongitudeValue { get; set; }
        /// <summary>${REST_PrimeMeridian_attribute_Type_D}</summary>
        public PrimeMeridianType Type { get; set; }

        internal static PrimeMeridian FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new PrimeMeridian
            {
                Name = json["name"].GetStringEx(),
                LongitudeValue = json["longitudeValue"].GetNumberEx(),
                Type = (PrimeMeridianType)Enum.Parse(typeof(PrimeMeridianType),
                json["type"].GetStringEx(), true)
            };
        }
    }
}
