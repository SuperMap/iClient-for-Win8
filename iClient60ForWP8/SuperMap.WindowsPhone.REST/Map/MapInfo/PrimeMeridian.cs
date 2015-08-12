using Newtonsoft.Json;
using System;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_PrimeMeridian_Title}</para>
    /// 	<para>${WP_REST_PrimeMeridian_Description}</para>
    /// </summary>
    public class PrimeMeridian
    {
        /// <summary>${WP_REST_PrimeMeridian_constructor_D}</summary>
        public PrimeMeridian()
        { }
        /// <summary>${WP_REST_PrimeMeridian_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_PrimeMeridian_attribute_LongitudeValue_D}</summary>
        [JsonProperty("longitudeValue")]
        public double LongitudeValue { get; set; }
        /// <summary>${WP_REST_PrimeMeridian_attribute_Type_D}</summary>
        [JsonProperty("type")]
        public PrimeMeridianType Type { get; set; }

    }
}
