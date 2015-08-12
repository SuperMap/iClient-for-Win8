using Newtonsoft.Json;
using System;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_Datum_Title}</para>
    /// 	<para>${WP_REST_Datum_Description}</para>
    /// </summary>
    public class Datum
    {
        /// <summary>${WP_REST_Datum_constructor_D}</summary>
        public Datum()
        { }
        /// <summary>${WP_REST_Datum_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_Datum_attribute_Type_D}</summary>
        [JsonProperty("type")]
        public DatumType Type { get; set; }
        /// <summary>${WP_REST_Datum_attribute_Spheroid_D}</summary>
        [JsonProperty("spheroid")]
        public Spheroid Spheroid { get; set; }

    }
}
