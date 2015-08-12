using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_PrjParameter_Title}</para>
    /// 	<para>${REST_PrjParameter_Description}</para>
    /// </summary>
    public class PrjParameter
    {
        /// <summary>${REST_PrjParameter_constructor_D}</summary>
        public PrjParameter()
        { }
        /// <summary>${REST_PrjParameter_attribute_SecondPointLongitude_D}</summary>
        public double SecondPointLongitude { get; set; }
        /// <summary>${REST_PrjParameter_attribute_FirstPointLongitude_D}</summary>
        public double FirstPointLongitude { get; set; }
        /// <summary>${REST_PrjParameter_attribute_FalseNorthing_D}</summary>
        public double FalseNorthing { get; set; }
        /// <summary>${REST_PrjParameter_attribute_SecondStandardParallel_D}</summary>
        public double SecondStandardParallel { get; set; }
        /// <summary>${REST_PrjParameter_attribute_FirstStandardParallel_D}</summary>
        public double FirstStandardParallel { get; set; }
        /// <summary>${REST_PrjParameter_attribute_CentralMeridian_D}</summary>
        public double CentralMeridian { get; set; }
        /// <summary>${REST_PrjParameter_attribute_CentralParallel_D}</summary>
        public double CentralParallel { get; set; }
        /// <summary>${REST_PrjParameter_attribute_ScaleFactor_D}</summary>
        public double ScaleFactor { get; set; }
        /// <summary>${REST_PrjParameter_attribute_Azimuth_D}</summary>
        public double Azimuth { get; set; }
        /// <summary>${REST_PrjParameter_attribute_FalseEasting_D}</summary>
        public double FalseEasting { get; set; }

        internal static PrjParameter FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new PrjParameter
            {
                Azimuth = json["azimuth"].GetNumberEx(),
                CentralMeridian = json["centralMeridian"].GetNumberEx(),
                CentralParallel = json["centralParallel"].GetNumberEx(),
                FalseEasting = json["falseEasting"].GetNumberEx(),
                FalseNorthing = json["falseNorthing"].GetNumberEx(),
                FirstStandardParallel = json["firstStandardParallel"].GetNumberEx(),
                FirstPointLongitude = json["firstPointLongitude"].GetNumberEx(),
                ScaleFactor = json["scaleFactor"].GetNumberEx(),
                SecondPointLongitude = json["secondPointLongitude"].GetNumberEx(),
                SecondStandardParallel = json["secondStandardParallel"].GetNumberEx(),
            };
        }
    }
}
