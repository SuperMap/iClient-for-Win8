using System;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Datum_Title}</para>
    /// 	<para>${REST_Datum_Description}</para>
    /// </summary>
    public class Datum
    {
        /// <summary>${REST_Datum_constructor_D}</summary>
        public Datum()
        { }
        /// <summary>${REST_Datum_attribute_Name_D}</summary>
        public string Name { get; set; }
        /// <summary>${REST_Datum_attribute_Type_D}</summary>
        public DatumType Type { get; set; }
        /// <summary>${REST_Datum_attribute_Spheroid_D}</summary>
        public Spheroid Spheroid { get; set; }

        internal static Datum FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new Datum
            {
                Name = json["name"].GetStringEx(),
                Type = (DatumType)Enum.Parse(typeof(DatumType),
                json["type"].GetStringEx(), true),
                Spheroid = Spheroid.FromJson(json["spheroid"].GetObjectEx())
            };
        }

    }
}
