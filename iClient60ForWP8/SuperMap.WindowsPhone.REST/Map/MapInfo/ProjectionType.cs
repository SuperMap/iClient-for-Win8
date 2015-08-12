

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_ProjectionType_Title}</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectionType
    {
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_NONPROJECTION_D}</summary>
        PRJ_NONPROJECTION,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_PRJ_PLATE_CARREE_D}</summary>
        PRJ_PLATE_CARREE,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_EQUIDISTANT_CYLINDRICAL_D}</summary>
        PRJ_EQUIDISTANT_CYLINDRICAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_MILLER_CYLINDRICAL_D}</summary>
        PRJ_MILLER_CYLINDRICAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_MERCATOR_D}</summary>
        PRJ_MERCATOR,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_GAUSS_KRUGER_D}</summary>
        PRJ_GAUSS_KRUGER,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_TRANSVERSE_MERCATOR_D}</summary>
        PRJ_TRANSVERSE_MERCATOR,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ALBERS_D}</summary>
        PRJ_ALBERS,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_SINUSOIDAL_D}</summary>
        PRJ_SINUSOIDAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_MOLLWEIDE_D}</summary>
        PRJ_MOLLWEIDE,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ECKERT_VI_D}</summary>
        PRJ_ECKERT_VI,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_PRJ_ECKERT_V_D}</summary>
        PRJ_ECKERT_V,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ECKERT_IV_D}</summary>
        PRJ_ECKERT_IV,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ECKERT_III_D}</summary>
        PRJ_ECKERT_III,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ECKERT_II_D}</summary>
        PRJ_ECKERT_II,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ECKERT_I_D}</summary>
        PRJ_ECKERT_I,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_PRJ_GALL_STEREOGRAPHIC_D}</summary>
        PRJ_GALL_STEREOGRAPHIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_BEHRMANN_D}</summary>
        PRJ_BEHRMANN,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_WINKEL_I_D}</summary>
        PRJ_WINKEL_I,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_WINKEL_II_D}</summary>
        PRJ_WINKEL_II,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_LAMBERT_CONFORMAL_CONIC_D}</summary>
        PRJ_LAMBERT_CONFORMAL_CONIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_POLYCONIC_D}</summary>
        PRJ_POLYCONIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_QUARTIC_AUTHALIC_D}</summary>
        PRJ_QUARTIC_AUTHALIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_LOXIMUTHAL_D}</summary>
        PRJ_LOXIMUTHAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_BONNE_D}</summary>
        PRJ_BONNE,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_HOTINE_D}</summary>
        PRJ_HOTINE,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_STEREOGRAPHIC_D}</summary>
        PRJ_STEREOGRAPHIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_EQUIDISTANT_CONIC_D}</summary>
        PRJ_EQUIDISTANT_CONIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_CASSINI_D}</summary>
        PRJ_CASSINI,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_VAN_DER_GRINTEN_I_D}</summary>
        PRJ_VAN_DER_GRINTEN_I,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_ROBINSON_D}</summary>
        PRJ_ROBINSON,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_TWO_POINT_EQUIDISTANT_D}</summary>
        PRJ_TWO_POINT_EQUIDISTANT,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_EQUIDISTANT_AZIMUTHAL_D}</summary>
        PRJ_EQUIDISTANT_AZIMUTHAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_LAMBERT_AZIMUTHAL_EQUAL_AREA_D}</summary>
        PRJ_LAMBERT_AZIMUTHAL_EQUAL_AREA,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_CONFORMAL_AZIMUTHAL_D}</summary>
        PRJ_CONFORMAL_AZIMUTHAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_GNOMONIC_D}</summary>
        PRJ_ORTHO_GRAPHIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_CHINA_AZIMUTHAL_D}</summary>
        PRJ_GNOMONIC,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_SANSON_D}</summary>
        PRJ_CHINA_AZIMUTHAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_SANSON_D}</summary>
        PRJ_SANSON,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_EQUALAREA_CYLINDRICAL_D}</summary>
        PRJ_EQUALAREA_CYLINDRICAL,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_HOTINE_AZIMUTH_NATORIGIN_D}</summary>
        PRJ_HOTINE_AZIMUTH_NATORIGIN,
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_SPHERE_MERCATOR_D}</summary>
        PRJ_SPHERE_MERCATOR, //SPHERE MERCATOR投影
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_OBLIQUE_STEREOGRAPHIC_D}</summary>
        PRJ_OBLIQUE_STEREOGRAPHIC,  //Oblique stereographic投影,支持 欧洲地理坐标系 所需，解决bug:ISVJ-98
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_OBLIQUE_MERCATOR_D}</summary>
        PRJ_OBLIQUE_MERCATOR,//斜轴墨卡托投影
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_HOTINE_OBLIQUE_MERCATOR_D}</summary>
        PRJ_HOTINE_OBLIQUE_MERCATOR, //Hotine斜轴墨卡托投影
        /// <summary>${WP_REST_ProjectionType_attribute_PRJ_BONNE_SOUTH_ORIENTATED_D}</summary>
        PRJ_BONNE_SOUTH_ORIENTATED,//南半球彭纳投影 
    }
}
