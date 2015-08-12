﻿

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_CoordSysType_Title}</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CoordSysType
    {
        /// <summary>${WP_REST_CoordSysType_attribute_CoordSysType_D}</summary>
        GCS_ADINDAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AFGOOYE_D}</summary>
        GCS_AFGOOYE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AGADEZ_D}</summary>
        GCS_AGADEZ,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AGD_1966_D}</summary>
        GCS_AGD_1966,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AGD_1984_D}</summary>
        GCS_AGD_1984,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AIN_EL_ABD_1970_D}</summary>
        GCS_AIN_EL_ABD_1970,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AIRY_1830_D}</summary>
        GCS_AIRY_1830,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AIRY_MOD_D}</summary>
        GCS_AIRY_MOD,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ALASKAN_ISLANDS_D}</summary>
        GCS_ALASKAN_ISLANDS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AMERSFOORT_D}</summary>
        GCS_AMERSFOORT,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ANNA_1_1965_D}</summary>
        GCS_ANNA_1_1965,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ANTIGUA_ISLAND_1943_D}</summary>
        GCS_ANTIGUA_ISLAND_1943,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ARATU_D}</summary>
        GCS_ARATU,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ARC_1950_D}</summary>
        GCS_ARC_1950,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ARC_1960_D}</summary>
        GCS_ARC_1960,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ASCENSION_ISLAND_1958_D}</summary>
        GCS_ASCENSION_ISLAND_1958,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ASTRO_1952_D}</summary>
        GCS_ASTRO_1952,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ATF_PARIS_D}</summary>
        GCS_ATF_PARIS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ATS_1977_D}</summary>
        GCS_ATS_1977,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AUSTRALIAN_D}</summary>
        GCS_AUSTRALIAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_AYABELLE_D}</summary>
        GCS_AYABELLE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BARBADOS_D}</summary>
        GCS_BARBADOS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BATAVIA_D}</summary>
        GCS_BATAVIA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BATAVIA_JAKARTA_D}</summary>
        GCS_BATAVIA_JAKARTA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BEACON_E_1945_D}</summary>
        GCS_BEACON_E_1945,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BEDUARAM_D}</summary>
        GCS_BEDUARAM,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BEIJING_1954_D}</summary>
        GCS_BEIJING_1954,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BELGE_1950_D}</summary>
        GCS_BELGE_1950,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BELGE_1950_BRUSSELS_D}</summary>
        GCS_BELGE_1950_BRUSSELS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BELGE_1972_D}</summary>
        GCS_BELGE_1972,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BELLEVUE_D}</summary>
        GCS_BELLEVUE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BERMUDA_1957_D}</summary>
        GCS_BERMUDA_1957,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BERN_1898_D}</summary>
        GCS_BERN_1898,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BERN_1898_BERN_D}</summary>
        GCS_BERN_1898_BERN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BERN_1938_D}</summary>
        GCS_BERN_1938,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BESSEL_1841_D}</summary>
        GCS_BESSEL_1841,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BESSEL_MOD_D}</summary>
        GCS_BESSEL_MOD,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BESSEL_NAMIBIA_D}</summary>
        GCS_BESSEL_NAMIBIA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BISSAU_D}</summary>
        GCS_BISSAU,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BOGOTA_D}</summary>
        GCS_BOGOTA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BOGOTA_BOGOTA_D}</summary>
        GCS_BOGOTA_BOGOTA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_BUKIT_RIMPAH_D}</summary>
        GCS_BUKIT_RIMPAH,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CACANAVERAL_D}</summary>
        GCS_CACANAVERAL,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CAMACUPA_D}</summary>
        GCS_CAMACUPA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CAMP_AREA_D}</summary>
        GCS_CAMP_AREA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CAMPO_INCHAUSPE_D}</summary>
        GCS_CAMPO_INCHAUSPE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CANTON_1966_D}</summary>
        GCS_CANTON_1966,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CAPE_D}</summary>
        GCS_CAPE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CARTHAGE_D}</summary>
        GCS_CARTHAGE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CARTHAGE_DEGREE_D}</summary>
        GCS_CARTHAGE_DEGREE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CHATHAM_ISLAND_1971_D}</summary>
        GCS_CHATHAM_ISLAND_1971,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CHINA_2000_D}</summary>
        GCS_CHINA_2000,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CHUA_D}</summary>
        GCS_CHUA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1858_D}</summary>
        GCS_CLARKE_1858,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1866_D}</summary>
        GCS_CLARKE_1866,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1866_MICH_D}</summary>
        GCS_CLARKE_1866_MICH,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1880_D}</summary>
        GCS_CLARKE_1880,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1880_ARC_D}</summary>
        GCS_CLARKE_1880_ARC,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1880_BENOIT_D}</summary>
        GCS_CLARKE_1880_BENOIT,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1880_IGN_D}</summary>
        GCS_CLARKE_1880_IGN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1880_RGS_D}</summary>
        GCS_CLARKE_1880_RGS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CLARKE_1880_SGA_D}</summary>
        GCS_CLARKE_1880_SGA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CONAKRY_1905_D}</summary>
        GCS_CONAKRY_1905,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_CORREGO_ALEGRE_D}</summary>
        GCS_CORREGO_ALEGRE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_COTE_D_IVOIRE_D}</summary>
        GCS_COTE_D_IVOIRE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DABOLA_D}</summary>
        GCS_DABOLA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DATUM_73_D}</summary>
        GCS_DATUM_73,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DEALUL_PISCULUI_1933_D}</summary>
        GCS_DEALUL_PISCULUI_1933,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DEALUL_PISCULUI_1970_D}</summary>
        GCS_DEALUL_PISCULUI_1970,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DECEPTION_ISLAND_D}</summary>
        GCS_DECEPTION_ISLAND,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GCS_DEIR_EZ_ZOR_D}</summary>
        GCS_DEIR_EZ_ZOR,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DHDNB_D}</summary>
        GCS_DHDNB,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DOS_1968_D}</summary>
        GCS_DOS_1968,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DOS_71_4}</summary>
        GCS_DOS_71_4,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_DOUALA_D}</summary>
        GCS_DOUALA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EASTER_ISLAND_1967_D}</summary>
        GCS_EASTER_ISLAND_1967,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ED_1950_D}</summary>
        GCS_ED_1950,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ED_1987_D}</summary>
        GCS_ED_1987,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EGYPT_1907_D}</summary>
        GCS_EGYPT_1907,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ETRS_1989_D}</summary>
        GCS_ETRS_1989,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EUROPEAN_1979_D}</summary>
        GCS_EUROPEAN_1979,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_1830_D}</summary>
        GCS_EVEREST_1830,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_BANGLADESH_D}</summary>
        GCS_EVEREST_BANGLADESH,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_DEF_1967_D}</summary>
        GCS_EVEREST_DEF_1967,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_DEF_1975_D}</summary>
        GCS_EVEREST_DEF_1975,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_INDIA_NEPAL_D}</summary>
        GCS_EVEREST_INDIA_NEPAL,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_MOD_D}</summary>
        GCS_EVEREST_MOD,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_EVEREST_MOD_1969_D}</summary>
        GCS_EVEREST_MOD_1969,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_FAHUD_D}</summary>
        GCS_FAHUD,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_FISCHER_1960_D}</summary>
        GCS_FISCHER_1960,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_FISCHER_1968_D}</summary>
        GCS_FISCHER_1968,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_FISCHER_MOD_D}</summary>
        GCS_FISCHER_MOD,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_FORT_THOMAS_1955_D}</summary>
        GCS_FORT_THOMAS_1955,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GAN_1970_D}</summary>
        GCS_GAN_1970,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GANDAJIKA_1970_D}</summary>
        GCS_GANDAJIKA_1970,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GAROUA_D}</summary>
        GCS_GAROUA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GDA_1994_D}</summary>
        GCS_GDA_1994,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GEM_10C_D}</summary>
        GCS_GEM_10C,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GGRS_1987_D}</summary>
        GCS_GGRS_1987,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GRACIOSA_1948_D}</summary>
        GCS_GRACIOSA_1948,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GREEK_D}</summary>
        GCS_GREEK,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GREEK_ATHENS_D}</summary>
        GCS_GREEK_ATHENS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GRS_1967_D}</summary>
        GCS_GRS_1967,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GRS_1980_D}</summary>
        GCS_GRS_1980,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GUAM_1963_D}</summary>
        GCS_GUAM_1963,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GUNUNG_SEGARA_D}</summary>
        GCS_GUNUNG_SEGARA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GUX_1_D}</summary>
        GCS_GUX_1,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_GUYANE_FRANCAISE_D}</summary>
        GCS_GUYANE_FRANCAISE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HELMERT_1906_D}</summary>
        GCS_HELMERT_1906,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HERAT_NORTH_D}</summary>
        GCS_HERAT_NORTH,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HITO_XVIII_1963_D}</summary>
        GCS_HITO_XVIII_1963,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HJORSEY_1955_D}</summary>
        GCS_HJORSEY_1955,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HONG_KONG_1963_D}</summary>
        GCS_HONG_KONG_1963,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HOUGH_1960_D}</summary>
        GCS_HOUGH_1960,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HU_TZU_SHAN_D}</summary>
        GCS_HU_TZU_SHAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_HUNGARIAN_1972_D}</summary>
        GCS_HUNGARIAN_1972,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INDIAN_1954_D}</summary>
        GCS_INDIAN_1954,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INDIAN_1960_D}</summary>
        GCS_INDIAN_1960,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INDIAN_1975_D}</summary>
        GCS_INDIAN_1975,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INDONESIAN_D}</summary>
        GCS_INDONESIAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INDONESIAN_1974_D}</summary>
        GCS_INDONESIAN_1974,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INTERNATIONAL_1924_D}</summary>
        GCS_INTERNATIONAL_1924,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_INTERNATIONAL_1967_D}</summary>
        GCS_INTERNATIONAL_1967,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ISTS_061_1968_D}</summary>
        GCS_ISTS_061_1968,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ISTS_073_1969_D}</summary>
        GCS_ISTS_073_1969,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_JAMAICA_1875_D}</summary>
        GCS_JAMAICA_1875,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_JAMAICA_1969_D}</summary>
        GCS_JAMAICA_1969,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_JAPAN_2000_D}</summary>
        GCS_JAPAN_2000,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_JOHNSTON_ISLAND_1961_D}</summary>
        GCS_JOHNSTON_ISLAND_1961,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KALIANPUR_D}</summary>
        GCS_KALIANPUR,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KANDAWALA_D}</summary>
        GCS_KANDAWALA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KERGUELEN_ISLAND_1949_D}</summary>
        GCS_KERGUELEN_ISLAND_1949,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KERTAU_D}</summary>
        GCS_KERTAU,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KKJ_D}</summary>
        GCS_KKJ,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KOC_D}</summary>
        GCS_KOC_,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KRASOVSKY_1940_D}</summary>
        GCS_KRASOVSKY_1940,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KUDAMS_D}</summary>
        GCS_KUDAMS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_KUSAIE_1951_D}</summary>
        GCS_KUSAIE_1951,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LA_CANOA_D}</summary>
        GCS_LA_CANOA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LAKE_D}</summary>
        GCS_LAKE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LC5_1961_D}</summary>
        GCS_LC5_1961,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LEIGON_D}</summary>
        GCS_LEIGON,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LIBERIA_1964_D}</summary>
        GCS_LIBERIA_1964,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LISBON_D}</summary>
        GCS_LISBON,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LISBON_LISBO_D}</summary>
        GCS_LISBON_LISBO,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LOMA_QUINTANA_D}</summary>
        GCS_LOMA_QUINTANA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LOME_D}</summary>
        GCS_LOME,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_LUZON_1911_D}</summary>
        GCS_LUZON_1911,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MAHE_1971_D}</summary>
        GCS_MAHE_1971,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MAKASSAR_D}</summary>
        GCS_MAKASSAR,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MAKASSAR_JAKARTA_D}</summary>
        GCS_MAKASSAR_JAKARTA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MALONGO_1987_D}</summary>
        GCS_MALONGO_1987,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MANOCA_D}</summary>
        GCS_MANOCA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MASSAWA_D}</summary>
        GCS_MASSAWA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MERCHICH_D}</summary>
        GCS_MERCHICH,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MGI_D}</summary>
        GCS_MGI_,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MGI_FERRO_D}</summary>
        GCS_MGI_FERRO,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MHAST_D}</summary>
        GCS_MHAST,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MIDWAY_1961_D}</summary>
        GCS_MIDWAY_1961,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MINNA_D}</summary>
        GCS_MINNA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MONTE_MARIO_D}</summary>
        GCS_MONTE_MARIO,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MONTE_MARIO_ROME_D}</summary>
        GCS_MONTE_MARIO_ROME,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MONTSERRAT_ISLAND_1958_D}</summary>
        GCS_MONTSERRAT_ISLAND_1958,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_MPORALOKO_D}</summary>
        GCS_MPORALOKO,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NAD_1927_D}</summary>
        GCS_NAD_1927,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NAD_1983_D}</summary>
        GCS_NAD_1983,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NAD_MICH_D}</summary>
        GCS_NAD_MICH,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NAHRWAN_1967_D}</summary>
        GCS_NAHRWAN_1967,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NAPARIMA_1972_D}</summary>
        GCS_NAPARIMA_1972,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NDG_PARIS_D}</summary>
        GCS_NDG_PARIS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NGN_D}</summary>
        GCS_NGN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NGO_1948_D}</summary>
        GCS_NGO_1948_,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NORD_SAHARA_1959_D}</summary>
        GCS_NORD_SAHARA_1959,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NSWC_9Z_2_D}</summary>
        GCS_NSWC_9Z_2_,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NTF_D}</summary>
        GCS_NTF_,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NTF_PARIS_D}</summary>
        GCS_NTF_PARIS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NWL_9D_D}</summary>
        GCS_NWL_9D,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_NZGD_1949_D}</summary>
        GCS_NZGD_1949,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OBSERV_METEOR_1939_D}</summary>
        GCS_OBSERV_METEOR_1939,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OLD_HAWAIIAN_D}</summary>
        GCS_OLD_HAWAIIAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OMAN_D}</summary>
        GCS_OMAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OS_SN_1980_D}</summary>
        GCS_OS_SN_1980,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OSGB_1936_D}</summary>
        GCS_OSGB_1936,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OSGB_1970_SN_D}</summary>
        GCS_OSGB_1970_SN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OSU_86F_D}</summary>
        GCS_OSU_86F,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_OSU_91A_D}</summary>
        GCS_OSU_91A,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PADANG_1884_D}</summary>
        GCS_PADANG_1884,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PADANG_1884_JAKARTA_D}</summary>
        GCS_PADANG_1884_JAKARTA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PALESTINE_1923_D}</summary>
        GCS_PALESTINE_1923,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PICO_DE_LAS_NIEVES_D}</summary>
        GCS_PICO_DE_LAS_NIEVES,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PITCAIRN_1967_D}</summary>
        GCS_PITCAIRN_1967,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PLESSIS_1817_D}</summary>
        GCS_PLESSIS_1817,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_POINT58_D}</summary>
        GCS_POINT58,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_POINTE_NOIRE_D}</summary>
        GCS_POINTE_NOIRE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PORTO_SANTO_1936_D}</summary>
        GCS_PORTO_SANTO_1936,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PSAD_1956_D}</summary>
        GCS_PSAD_1956,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PUERTO_RICO_D}</summary>
        GCS_PUERTO_RICO,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PULKOVO_1942_D}</summary>
        GCS_PULKOVO_1942,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_PULKOVO_1995_D}</summary>
        GCS_PULKOVO_1995,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_QATAR_D}</summary>
        GCS_QATAR,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_QATAR_1948_D}</summary>
        GCS_QATAR_1948,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_QORNOQ_D}</summary>
        GCS_QORNOQ,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_REUNION_D}</summary>
        GCS_REUNION,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_RT38_D}</summary>
        GCS_RT38,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_RT38_STOCKHOLM_D}</summary>
        GCS_RT38_STOCKHOLM,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_S_ASIA_SINGAPORE_D}</summary>
        GCS_S_ASIA_SINGAPORE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_S_JTSK_D}</summary>
        GCS_S_JTSK,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_S42_HUNGARY_D}</summary>
        GCS_S42_HUNGARY,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SAD_1969_D}</summary>
        GCS_SAD_1969,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SAMOA_1962_D}</summary>
        GCS_SAMOA_1962,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SANTO_DOS_1965_D}</summary>
        GCS_SANTO_DOS_1965,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SAO_BRAZ_D}</summary>
        GCS_SAO_BRAZ,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SAPPER_HILL_1943_D}</summary>
        GCS_SAPPER_HILL_1943,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SCHWARZECK_D}</summary>
        GCS_SCHWARZECK,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SEGORA_D}</summary>
        GCS_SEGORA,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SELVAGEM_GRANDE_1938_D}</summary>
        GCS_SELVAGEM_GRANDE_1938,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SERINDUNG_D}</summary>
        GCS_SERINDUNG,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SPHERE_D}</summary>
        GCS_SPHERE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SPHERE_AI_D}</summary>
        GCS_SPHERE_AI,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_STRUVE_1860_D}</summary>
        GCS_STRUVE_1860,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_SUDAN_D}</summary>
        GCS_SUDAN,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TANANARIVE_1925_D}</summary>
        GCS_TANANARIVE_1925,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TANANARIVE_1925_PARIS_D}</summary>
        GCS_TANANARIVE_1925_PARIS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TERN_ISLAND_1961_D}</summary>
        GCS_TERN_ISLAND_1961,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TIMBALAI_1948_D}</summary>
        GCS_TIMBALAI_1948,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TM65_D}</summary>
        GCS_TM65,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TM75_D}</summary>
        GCS_TM75,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TOKYO_D}</summary>
        GCS_TOKYO,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TRINIDAD_1903_D}</summary>
        GCS_TRINIDAD_1903,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TRISTAN_1968_D}</summary>
        GCS_TRISTAN_1968,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_TRUCIAL_COAST_1948_D}</summary>
        GCS_TRUCIAL_COAST_1948,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_USER_DEFINE_D}</summary>
        GCS_USER_DEFINE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_VITI_LEVU_1916_D}</summary>
        GCS_VITI_LEVU_1916,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_VOIROL_1875_D}</summary>
        GCS_VOIROL_1875,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_VOIROL_1875_PARIS_D}</summary>
        GCS_VOIROL_1875_PARIS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_VOIROL_UNIFIE_1960_D}</summary>
        GCS_VOIROL_UNIFIE_1960,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_VOIROL_UNIFIE_1960_PARIS_D}</summary>
        GCS_VOIROL_UNIFIE_1960_PARIS,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WAKE_ENIWETOK_1960_D}</summary>
        GCS_WAKE_ENIWETOK_1960,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WAKE_ISLAND_1952_D}</summary>
        GCS_WAKE_ISLAND_1952,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WALBECK_D}</summary>
        GCS_WALBECK,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WAR_OFFICE_D}</summary>
        GCS_WAR_OFFICE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WGS_1966_D}</summary>
        GCS_WGS_1966,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WGS_1972_D}</summary>
        GCS_WGS_1972,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WGS_1972_BE_D}</summary>
        GCS_WGS_1972_BE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_WGS_1984_D}</summary>
        GCS_WGS_1984,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_XIAN_1980_D}</summary>
        GCS_XIAN_1980,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_YACARE_D}</summary>
        GCS_YACARE,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_YOFF_D}</summary>
        GCS_YOFF,
        /// <summary>${WP_REST_CoordSysType_attribute_GCS_ZANDERIJ_D}</summary>
        GCS_ZANDERIJ
    }
}
