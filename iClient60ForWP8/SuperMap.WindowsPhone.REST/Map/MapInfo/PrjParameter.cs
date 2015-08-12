
using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_PrjParameter_Title}</para>
    /// 	<para>${WP_REST_PrjParameter_Description}</para>
    /// </summary>
    public class PrjParameter
    {
        /// <summary>${WP_REST_PrjParameter_constructor_D}</summary>
        public PrjParameter()
        { }
        /// <summary>${WP_REST_PrjParameter_attribute_SecondPointLongitude_D}</summary>
        [JsonProperty("secondPointLongitude")]
        public double SecondPointLongitude { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_FirstPointLongitude_D}</summary>
        [JsonProperty("firstPointLongitude")]
        public double FirstPointLongitude { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_FalseNorthing_D}</summary>
        [JsonProperty("falseNorthing")]
        public double FalseNorthing { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_SecondStandardParallel_D}</summary>
        [JsonProperty("secondStandardParallel")]
        public double SecondStandardParallel { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_FirstStandardParallel_D}</summary>
        [JsonProperty("firstStandardParallel")]
        public double FirstStandardParallel { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_CentralMeridian_D}</summary>
        [JsonProperty("centralMeridian")]
        public double CentralMeridian { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_CentralParallel_D}</summary>
        [JsonProperty("centralParallel")]
        public double CentralParallel { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_ScaleFactor_D}</summary>
        [JsonProperty("scaleFactor")]
        public double ScaleFactor { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_Azimuth_D}</summary>
        [JsonProperty("azimuth")]
        public double Azimuth { get; set; }
        /// <summary>${WP_REST_PrjParameter_attribute_FalseEasting_D}</summary>
        [JsonProperty("falseEasting")]
        public double FalseEasting { get; set; }

    }
}
