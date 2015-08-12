

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_FillGradientMode_Tile}</para>
    /// 	<para>${WP_REST_ServerType_FillGradientMode_Description}</para>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FillGradientMode
    {
        /// <summary>${WP_REST_ServerType_FillGradientMode_attribute_NONE_D}</summary>
        NONE,
        /// <summary>
        /// <para>${WP_REST_ServerType_FillGradientMode_attribute_LINEAR_D}</para>
        /// <para><img src="fillGradientModeLinear.bmp"/></para>
        /// </summary>
        LINEAR,
        /// <summary>
        /// <para>${WP_REST_ServerType_FillGradientMode_attribute_RADIAL_D}</para>
        /// <para><img src="fillGradientModeRadial.bmp"/></para>
        /// </summary>
        RADIAL,
        /// <summary>
        /// <para>${WP_REST_ServerType_FillGradientMode_attribute_CONICAL_D}</para>
        /// <para><img src="fillGradientModeConical.bmp"/></para>
        /// </summary>
        CONICAL,
        /// <summary>
        /// <para>${WP_REST_ServerType_FillGradientMode_attribute_SQUARE_D}</para>
        /// <para><img src="fillGradientModeSquare.bmp"/></para>
        /// </summary>
        SQUARE,

    }
}
