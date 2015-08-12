using SuperMap.WinRT.Utilities;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Spheroid_Title}</para>
    /// 	<para>${REST_Spheroid_Description}</para>
    /// </summary>
    public class Spheroid
    {
        /// <summary>${REST_Spheroid_constructor_D}</summary>
        public Spheroid()
        { }
        /// <summary>${REST_Spheroid_attribute_Axis_D}</summary>
        public double Axis { get; set; }
        /// <summary>${REST_Spheroid_attribute_Flatten_D}</summary>
        public double Flatten { get; set; }
        /// <summary>${REST_Spheroid_attribute_Name_D}</summary>
        public string Name { get; set; }
        /// <summary>${REST_Spheroid_attribute_Type_D}</summary>
        public SpheroidType Type { get; set; }

        internal static Spheroid FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new Spheroid
            {
                Axis = json["axis"].GetNumberEx(),
                Flatten = json["flatten"].GetNumberEx(),
                Name = json["name"].GetStringEx(),
                Type = (SpheroidType)Enum.Parse(typeof(SpheroidType),
                json["type"].GetStringEx(), true)
            };
        }
    }
}
