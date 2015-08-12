using Newtonsoft.Json;
using System;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_Projection_Title}</para>
    /// 	<para>${WP_REST_Projection_Description}</para>
    /// </summary>
    public class Projection
    {
        /// <summary>${WP_REST_Projection_constructor_D}</summary>
        public Projection()
        { }
        /// <summary>${WP_REST_Projection_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_Projection_attribute_Type_D}</summary>
        [JsonProperty("type")]
        public ProjectionType Type { get; set; }


    }
}
