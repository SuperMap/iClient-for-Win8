
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_SpheroidType_Title}</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SpheroidType
    {
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_AIRY_1830_D}</summary>
        SPHEROID_AIRY_1830,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_AIRY_MOD_D}</summary>
        SPHEROID_AIRY_MOD,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_ATS_1977_D}</summary>
        SPHEROID_ATS_1977,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_AUSTRALIAN_D}</summary>
        SPHEROID_AUSTRALIAN,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_BESSEL_1841_D}</summary>
        SPHEROID_BESSEL_1841,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_BESSEL_MOD_D}</summary>
        SPHEROID_BESSEL_MOD,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_BESSEL_NAMIBIA_D}</summary>
        SPHEROID_BESSEL_NAMIBIA,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CHINA_2000_D}</summary>
        SPHEROID_CHINA_2000,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1858_D}</summary>
        SPHEROID_CLARKE_1858,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1866_D}</summary>
        SPHEROID_CLARKE_1866,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1866_MICH_D}</summary>
        SPHEROID_CLARKE_1866_MICH,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1880_D}</summary>
        SPHEROID_CLARKE_1880,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1880_ARC_D}</summary>
        SPHEROID_CLARKE_1880_ARC,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1880_BENOIT_D}</summary>
        SPHEROID_CLARKE_1880_BENOIT,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1880_IGN_D}</summary>
        SPHEROID_CLARKE_1880_IGN,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1880_RGS_D}</summary>
        SPHEROID_CLARKE_1880_RGS,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_CLARKE_1880_SGA_D}</summary>
        SPHEROID_CLARKE_1880_SGA,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_EVEREST_1830_D}</summary>
        SPHEROID_EVEREST_1830,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_EVEREST_DEF_1967_D}</summary>
        SPHEROID_EVEREST_DEF_1967,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_EVEREST_DEF_197_D}</summary>
        SPHEROID_EVEREST_DEF_197,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_EVEREST_MOD_D}</summary>
        SPHEROID_EVEREST_MOD,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_EVEREST_MOD_1969_D}</summary>
        SPHEROID_EVEREST_MOD_1969,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_FISCHER_1960_D}</summary>
        SPHEROID_FISCHER_1960,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_FISCHER_1968_D}</summary>
        SPHEROID_FISCHER_1968,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_FISCHER_MOD_D}</summary>
        SPHEROID_FISCHER_MOD,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_GEM_10C_D}</summary>
        SPHEROID_GEM_10C,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_GRS_1967_D}</summary>
        SPHEROID_GRS_1967,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_GRS_1980_D}</summary>
        SPHEROID_GRS_1980,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_HELMERT_1906_D}</summary>
        SPHEROID_HELMERT_1906,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_HOUGH_1960_D}</summary>
        SPHEROID_HOUGH_1960,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_INDONESIAN_D}</summary>
        SPHEROID_INDONESIAN,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_INTERNATIONAL_1924_D}</summary>
        SPHEROID_INTERNATIONAL_1924,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_INTERNATIONAL_1967_D}</summary>
        SPHEROID_INTERNATIONAL_1967,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_INTERNATIONAL_1975_D}</summary>
        SPHEROID_INTERNATIONAL_1975,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_KRASOVSKY_1940_D}</summary>
        SPHEROID_KRASOVSKY_1940,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_NWL_10D_D}</summary>
        SPHEROID_NWL_10D,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_NWL_9D_D}</summary>
        SPHEROID_NWL_9D,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_OSU_86F_D}</summary>
        SPHEROID_OSU_86F,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_OSU_91A_D}</summary>
        SPHEROID_OSU_91A,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_PLESSIS_1817_D}</summary>
        SPHEROID_PLESSIS_1817,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_SPHERE_D}</summary>
        SPHEROID_SPHERE,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_SPHERE_AI_D}</summary>
        SPHEROID_SPHERE_AI,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_STRUVE_1860_D}</summary>
        SPHEROID_STRUVE_1860,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_USER_DEFINED_D}</summary>
        SPHEROID_USER_DEFINED,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_WALBECK_D}</summary>
        SPHEROID_WALBECK,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_WAR_OFFICE_D}</summary>
        SPHEROID_WAR_OFFICE,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_WGS_1966_D}</summary>
        SPHEROID_WGS_1966,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_WGS_1972_D}</summary>
        SPHEROID_WGS_1972,
        /// <summary>${WP_REST_SpheroidType_attribute_SPHEROID_WGS_1984_D}</summary>
        SPHEROID_WGS_1984
    }
}
