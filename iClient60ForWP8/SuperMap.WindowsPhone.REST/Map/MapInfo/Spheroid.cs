using Newtonsoft.Json;
using System;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_Spheroid_Title}</para>
    /// 	<para>${WP_REST_Spheroid_Description}</para>
    /// </summary>
    public class Spheroid
    {
        /// <summary>${WP_REST_Spheroid_constructor_D}</summary>
        public Spheroid()
        { }
        /// <summary>${WP_REST_Spheroid_attribute_Axis_D}</summary>
        [JsonProperty("axis")]
        public double Axis { get; set; }
        /// <summary>${WP_REST_Spheroid_attribute_Flatten_D}</summary>
        [JsonProperty("flatten")]
        public double Flatten { get; set; }
        /// <summary>${WP_REST_Spheroid_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_Spheroid_attribute_Type_D}</summary>
        [JsonProperty("type")]
        public SpheroidType Type { get; set; }

    }
}
