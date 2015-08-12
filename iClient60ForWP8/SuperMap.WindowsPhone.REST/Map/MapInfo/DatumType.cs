

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_DatumType_Title}</para>
    /// 	<para>${WP_REST_DatumType_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatumType
    {
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ADINDAN_D}</summary>
        DATUM_ADINDAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AFGOOYE_D}</summary>
        DATUM_AFGOOYE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AGADEZ_D}</summary>
        DATUM_AGADEZ,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AGD_1966_D}</summary>
        DATUM_AGD_1966,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AGD_1984_D}</summary>
        DATUM_AGD_1984,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AIN_EL_ABD_1970_D}</summary>
        DATUM_AIN_EL_ABD_1970,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AIRY_1830_D}</summary>
        DATUM_AIRY_1830,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AIRY_MOD_D}</summary>
        DATUM_AIRY_MOD,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ALASKAN_ISLANDS_D}</summary>
        DATUM_ALASKAN_ISLANDS,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AMERSFOORT_D}</summary>
        DATUM_AMERSFOORT,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ANNA_1_1965_D}</summary>
        DATUM_ANNA_1_1965,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ANTIGUA_ISLAND_1943_D}</summary>
        DATUM_ANTIGUA_ISLAND_1943,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ARATU_D}</summary>
        DATUM_ARATU,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ARC_1950_D}</summary>
        DATUM_ARC_1950,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ARC_1960_D}</summary>
        DATUM_ARC_1960,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ASCENSION_ISLAND_1958_D}</summary>
        DATUM_ASCENSION_ISLAND_1958,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ASTRO_1952_D}</summary>
        DATUM_ASTRO_1952,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ATF_D}</summary>
        DATUM_ATF,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ATS_1977_D}</summary>
        DATUM_ATS_1977,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AUSTRALIAN_D}</summary>
        DATUM_AUSTRALIAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_AYABELLE_D}</summary>
        DATUM_AYABELLE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BARBADOS_D}</summary>
        DATUM_BARBADOS,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BATAVIA_D}</summary>
        DATUM_BATAVIA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BEACON_E_1945_D}</summary>
        DATUM_BEACON_E_1945,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BEDUARAM_D}</summary>
        DATUM_BEDUARAM,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BEIJING_1954_D}</summary>
        DATUM_BEIJING_1954,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BELGE_1950_D}</summary>
        DATUM_BELGE_1950,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BELGE_1972_D}</summary>
        DATUM_BELGE_1972,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BELLEVUE_D}</summary>
        DATUM_BELLEVUE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BERMUDA_1957_D}</summary>
        DATUM_BERMUDA_1957,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BERN_1898_D}</summary>
        DATUM_BERN_1898,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BERN_1938_D}</summary>
        DATUM_BERN_1938,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BESSEL_1841_D}</summary>
        DATUM_BESSEL_1841,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BESSEL_MOD_D}</summary>
        DATUM_BESSEL_MOD,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BESSEL_NAMIBIA_D}</summary>
        DATUM_BESSEL_NAMIBIA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BISSAU_D}</summary>
        DATUM_BISSAU,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BOGOTA_D}</summary>
        DATUM_BOGOTA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_BUKIT_RIMPAH_D}</summary>
        DATUM_BUKIT_RIMPAH,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CACANAVERAL_D}</summary>
        DATUM_CACANAVERAL,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CAMACUPA_D}</summary>
        DATUM_CAMACUPA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CAMP_AREA_D}</summary>
        DATUM_CAMP_AREA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CAMPO_INCHAUSPE_D}</summary>
        DATUM_CAMPO_INCHAUSPE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CANTON_1966_D}</summary>
        DATUM_CANTON_1966,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CAPE_D}</summary>
        DATUM_CAPE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CARTHAGE_D}</summary>
        DATUM_CARTHAGE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CHATHAM_ISLAND_1971_D}</summary>
        DATUM_CHATHAM_ISLAND_1971,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CHINA_2000_D}</summary>
        DATUM_CHINA_2000,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CHUA_D}</summary>
        DATUM_CHUA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1858_D}</summary>
        DATUM_CLARKE_1858,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1866_D}</summary>
        DATUM_CLARKE_1866,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1866_MICH_D}</summary>
        DATUM_CLARKE_1866_MICH,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1880_D}</summary>
        DATUM_CLARKE_1880,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1880_ARC_D}</summary>
        DATUM_CLARKE_1880_ARC,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1880_BENOIT_D}</summary>
        DATUM_CLARKE_1880_BENOIT,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1880_IGN_D}</summary>
        DATUM_CLARKE_1880_IGN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1880_RGS_D}</summary>
        DATUM_CLARKE_1880_RGS,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CLARKE_1880_SGA_D}</summary>
        DATUM_CLARKE_1880_SGA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CONAKRY_1905_D}</summary>
        DATUM_CONAKRY_1905,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_CORREGO_ALEGRE_D}</summary>
        DATUM_CORREGO_ALEGRE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_COTE_D_IVOIRE_D}</summary>
        DATUM_COTE_D_IVOIRE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DABOLA_D}</summary>
        DATUM_DABOLA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DATUM_73_D}</summary>
        DATUM_DATUM_73,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DEALUL_PISCULUI_1933_D}</summary>
        DATUM_DEALUL_PISCULUI_1933,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DEALUL_PISCULUI_1970_D}</summary>
        DATUM_DEALUL_PISCULUI_1970,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DECEPTION_ISLAND_D}</summary>
        DATUM_DECEPTION_ISLAND,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DEIR_EZ_ZOR_D}</summary>
        DATUM_DEIR_EZ_ZOR,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DHDN_D}</summary>
        DATUM_DHDN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DOS_71_4_D}</summary>
        DATUM_DOS_71_4,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_DOUALA_D}</summary>
        DATUM_DOUALA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EASTER_ISLAND_1967_D}</summary>
        DATUM_EASTER_ISLAND_1967,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ED_1950_D}</summary>
        DATUM_ED_1950,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ED_1987_D}</summary>
        DATUM_ED_1987,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EGYPT_1907_D}</summary>
        DATUM_EGYPT_1907,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ETRS_1989_D}</summary>
        DATUM_ETRS_1989,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EUROPEAN_1979_D}</summary>
        DATUM_EUROPEAN_1979,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_1830_D}</summary>
        DATUM_EVEREST_1830,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_BANGLADESH_D}</summary>
        DATUM_EVEREST_BANGLADESH,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_DEF_1967_D}</summary>
        DATUM_EVEREST_DEF_1967,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_DEF_1975_D}</summary>
        DATUM_EVEREST_DEF_1975,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_INDIA_NEPAL_D}</summary>
        DATUM_EVEREST_INDIA_NEPAL,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_MOD_D}</summary>
        DATUM_EVEREST_MOD,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_EVEREST_MOD_1969_D}</summary>
        DATUM_EVEREST_MOD_1969,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_FAHUD_D}</summary>
        DATUM_FAHUD,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_FISCHER_1960_D}</summary>
        DATUM_FISCHER_1960,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_FISCHER_1968_D}</summary>
        DATUM_FISCHER_1968,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_FISCHER_MOD_D}</summary>
        DATUM_FISCHER_MOD,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_FORT_THOMAS_1955_D}</summary>
        DATUM_FORT_THOMAS_1955,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GAN_1970_D}</summary>
        DATUM_GAN_1970,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GANDAJIKA_1970_D}</summary>
        DATUM_GANDAJIKA_1970,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GAROUA_D}</summary>
        DATUM_GAROUA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GDA_1994_D}</summary>
        DATUM_GDA_1994,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GEM_10C_D}</summary>
        DATUM_GEM_10C,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GGRS_1987_D}</summary>
        DATUM_GGRS_1987,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GRACIOSA_1948_D}</summary>
        DATUM_GRACIOSA_1948,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GREEK_D}</summary>
        DATUM_GREEK,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GRS_1967_D}</summary>
        DATUM_GRS_1967,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GRS_1980_D}</summary>
        DATUM_GRS_1980,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GUAM_1963_D}</summary>
        DATUM_GUAM_1963,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GUNUNG_SEGARA_D}</summary>
        DATUM_GUNUNG_SEGARA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GUX_1_D}</summary>
        DATUM_GUX_1,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_GUYANE_FRANCAISE_D}</summary>
        DATUM_GUYANE_FRANCAISE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HELMERT_1906_D}</summary>
        DATUM_HELMERT_1906,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HERAT_NORTH_D}</summary>
        DATUM_HERAT_NORTH,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HITO_XVIII_1963_D}</summary>
        DATUM_HITO_XVIII_1963,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HJORSEY_1955_D}</summary>
        DATUM_HJORSEY_1955,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HONG_KONG_1963_D}</summary>
        DATUM_HONG_KONG_1963,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HOUGH_1960_D}</summary>
        DATUM_HOUGH_1960,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HU_TZU_SHAN_D}</summary>
        DATUM_HU_TZU_SHAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_HUNGARIAN_1972_D}</summary>
        DATUM_HUNGARIAN_1972,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INDIAN_1954_D}</summary>
        DATUM_INDIAN_1954,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INDIAN_1960_D}</summary>
        DATUM_INDIAN_1960,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INDIAN_1975_D}</summary>
        DATUM_INDIAN_1975,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INDONESIAN_D}</summary>
        DATUM_INDONESIAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INDONESIAN_1974_D}</summary>
        DATUM_INDONESIAN_1974,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INTERNATIONAL_1924_D}</summary>
        DATUM_INTERNATIONAL_1924,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_INTERNATIONAL_1967_D}</summary>
        DATUM_INTERNATIONAL_1967,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ISTS_061_1968_D}</summary>
        DATUM_ISTS_061_1968,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ISTS_073_1969_D}</summary>
        DATUM_ISTS_073_1969,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_JAMAICA_1875_D}</summary>
        DATUM_JAMAICA_1875,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_JAMAICA_1969_D}</summary>
        DATUM_JAMAICA_1969,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_JAPAN_2000_D}</summary>
        DATUM_JAPAN_2000,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_JOHNSTON_ISLAND_1961_D}</summary>
        DATUM_JOHNSTON_ISLAND_1961,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KALIANPUR_D}</summary>
        DATUM_KALIANPUR,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KANDAWALA_D}</summary>
        DATUM_KANDAWALA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KERGUELEN_ISLAND_1949_D}</summary>
        DATUM_KERGUELEN_ISLAND_1949,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KERTAU_D}</summary>
        DATUM_KERTAU,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KKJ_D}</summary>
        DATUM_KKJ,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KOC_D}</summary>
        DATUM_KOC,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KRASOVSKY_1940_D}</summary>
        DATUM_KRASOVSKY_1940,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KUDAMS_D}</summary>
        DATUM_KUDAMS,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_KUSAIE_1951_D}</summary>
        DATUM_KUSAIE_1951,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LA_CANOA_D}</summary>
        DATUM_LA_CANOA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LAKE_D}</summary>
        DATUM_LAKE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LC5_1961_D}</summary>
        DATUM_LC5_1961,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LEIGON_D}</summary>
        DATUM_LEIGON,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LIBERIA_1964_D}</summary>
        DATUM_LIBERIA_1964,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LISBON_D}</summary>
        DATUM_LISBON,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LOMA_QUINTANA_D}</summary>
        DATUM_LOMA_QUINTANA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LOME_D}</summary>
        DATUM_LOME,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_LUZON_1911_D}</summary>
        DATUM_LUZON_1911,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MAHE_1971_D}</summary>
        DATUM_MAHE_1971,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MAKASSAR_D}</summary>
        DATUM_MAKASSAR,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MALONGO_1987_D}</summary>
        DATUM_MALONGO_1987,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MANOCA_D}</summary>
        DATUM_MANOCA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MASSAWA_D}</summary>
        DATUM_MASSAWA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MERCHICH_D}</summary>
        DATUM_MERCHICH,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MGI_D}</summary>
        DATUM_MGI,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MHAST_D}</summary>
        DATUM_MHAST,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MIDWAY_1961_D}</summary>
        DATUM_MIDWAY_1961,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MINNA_D}</summary>
        DATUM_MINNA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MONTE_MARIO_D}</summary>
        DATUM_MONTE_MARIO,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MONTSERRAT_ISLAND_1958_D}</summary>
        DATUM_MONTSERRAT_ISLAND_1958,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_MPORALOKO_D}</summary>
        DATUM_MPORALOKO,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NAD_1927_D}</summary>
        DATUM_NAD_1927,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NAD_1983_D}</summary>
        DATUM_NAD_1983,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NAD_MICH_D}</summary>
        DATUM_NAD_MICH,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NAHRWAN_1967_D}</summary>
        DATUM_NAHRWAN_1967,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NAPARIMA_1972_D}</summary>
        DATUM_NAPARIMA_1972,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NDG_D}</summary>
        DATUM_NDG,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NGN_D}</summary>
        DATUM_NGN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NGO_1948_D}</summary>
        DATUM_NGO_1948,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NORD_SAHARA_1959_D}</summary>
        DATUM_NORD_SAHARA_1959,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NSWC_9Z_2_D}</summary>
        DATUM_NSWC_9Z_2,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NTF_D}</summary>
        DATUM_NTF,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NWL_9D_D}</summary>
        DATUM_NWL_9D,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_NZGD_1949_D}</summary>
        DATUM_NZGD_1949,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OBSERV_METEOR_1939_D}</summary>
        DATUM_OBSERV_METEOR_1939,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OLD_HAWAIIAN_D}</summary>
        DATUM_OLD_HAWAIIAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OMAN_D}</summary>
        DATUM_OMAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OS_SN_1980_D}</summary>
        DATUM_OS_SN_1980,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OSGB_1936_D}</summary>
        DATUM_OSGB_1936,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OSGB_1970_SN_D}</summary>
        DATUM_OSGB_1970_SN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OSU_86F_D}</summary>
        DATUM_OSU_86F,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_OSU_91A_D}</summary>
        DATUM_OSU_91A,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PADANG_1884_D}</summary>
        DATUM_PADANG_1884,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PALESTINE_1923_D}</summary>
        DATUM_PALESTINE_1923,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PICO_DE_LAS_NIEVES_D}</summary>
        DATUM_PICO_DE_LAS_NIEVES,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PITCAIRN_1967_D}</summary>
        DATUM_PITCAIRN_1967,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PLESSIS_1817_D}</summary>
        DATUM_PLESSIS_1817,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_POINT58_D}</summary>
        DATUM_POINT58,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_POINTE_NOIRE_D}</summary>
        DATUM_POINTE_NOIRE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PORTO_SANTO_1936_D}</summary>
        DATUM_PORTO_SANTO_1936,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PSAD_1956_D}</summary>
        DATUM_PSAD_1956,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PUERTO_RICO_D}</summary>
        DATUM_PUERTO_RICO,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PULKOVO_1942_D}</summary>
        DATUM_PULKOVO_1942,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_PULKOVO_1995_D}</summary>
        DATUM_PULKOVO_1995,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_QATAR_D}</summary>
        DATUM_QATAR,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_QATAR_1948_D}</summary>
        DATUM_QATAR_1948,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_QORNOQ_D}</summary>
        DATUM_QORNOQ,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_REUNION_D}</summary>
        DATUM_REUNION,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_S_ASIA_SINGAPORE_D}</summary>
        DATUM_S_ASIA_SINGAPORE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_S_JTSK_D}</summary>
        DATUM_S_JTSK,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_S42_HUNGARY_D}</summary>
        DATUM_S42_HUNGARY,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SAD_1969_D}</summary>
        DATUM_SAD_1969,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SAMOA_1962_D}</summary>
        DATUM_SAMOA_1962,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SANTO_DOS_1965_D}</summary>
        DATUM_SANTO_DOS_1965,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SAO_BRAZ_D}</summary>
        DATUM_SAO_BRAZ,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SAPPER_HILL_1943_D}</summary>
        DATUM_SAPPER_HILL_1943,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SCHWARZECK_D}</summary>
        DATUM_SCHWARZECK,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SEGORA_D}</summary>
        DATUM_SEGORA,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SELVAGEM_GRANDE_1938_D}</summary>
        DATUM_SELVAGEM_GRANDE_1938,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SERINDUNG_D}</summary>
        DATUM_SERINDUNG,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SPHERE_D}</summary>
        DATUM_SPHERE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SPHERE_AI_D}</summary>
        DATUM_SPHERE_AI,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_STOCKHOLM_1938_D}</summary>
        DATUM_STOCKHOLM_1938,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_STRUVE_1860_D}</summary>
        DATUM_STRUVE_1860,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_SUDAN_D}</summary>
        DATUM_SUDAN,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TANANARIVE_1925_D}</summary>
        DATUM_TANANARIVE_1925,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TERN_ISLAND_1961_D}</summary>
        DATUM_TERN_ISLAND_1961,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TIMBALAI_1948_D}</summary>
        DATUM_TIMBALAI_1948,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TM65_D}</summary>
        DATUM_TM65,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TM75_D}</summary>
        DATUM_TM75,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TOKYO_D}</summary>
        DATUM_TOKYO,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TRINIDAD_1903_D}</summary>
        DATUM_TRINIDAD_1903,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TRISTAN_1968_D}</summary>
        DATUM_TRISTAN_1968,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_TRUCIAL_COAST_1948_D}</summary>
        DATUM_TRUCIAL_COAST_1948,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_USER_DEFINED_D}</summary>
        DATUM_USER_DEFINED,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_VITI_LEVU_1916_D}</summary>
        DATUM_VITI_LEVU_1916,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_VOIROL_1875_D}</summary>
        DATUM_VOIROL_1875,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_VOIROL_UNIFIE_1960_D}</summary>
        DATUM_VOIROL_UNIFIE_1960,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WAKE_ENIWETOK_1960_D}</summary>
        DATUM_WAKE_ENIWETOK_1960,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WAKE_ISLAND_1952_D}</summary>
        DATUM_WAKE_ISLAND_1952,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WALBECK_D}</summary>
        DATUM_WALBECK,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WAR_OFFICE_D}</summary>
        DATUM_WAR_OFFICE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WGS_1966_D}</summary>
        DATUM_WGS_1966,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WGS_1972_BE_D}</summary>
        DATUM_WGS_1972,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WGS_1972_BE_D}</summary>
        DATUM_WGS_1972_BE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_WGS_1984_D}</summary>
        DATUM_WGS_1984,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_XIAN_1980_D}</summary>
        DATUM_XIAN_1980,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_YACARE_D}</summary>
        DATUM_YACARE,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_YOFF_D}</summary>
        DATUM_YOFF,
        /// <summary>${WP_REST_DatumType_attribute_DATUM_ZANDERIJ_D}</summary>
        DATUM_ZANDERIJ
    }
}
