using Newtonsoft.Json;
using System.Collections.Generic;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_ServerType_ServerColor_Tile}</para>
    /// 	<para>${WP_REST_ServerType_ServerColor_Description}</para>
    /// </summary>
    public class ServerColor
    {
        /// <summary>${WP_REST_ServerType_ServerColor_constructor_None_D}</summary>
        /// <overloads>${WP_REST_ServerType_ServerColor_constructor_overloads}</overloads>
        public ServerColor()
        {
            Blue = 0;
            Green = 0;
            Red = 255;
        }
        /// <summary>${WP_REST_ServerType_ServerColor_constructor_String_D}</summary>
        public ServerColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>${WP_REST_ServerType_ServerColor_attribute_red_D}</summary>
        [JsonProperty("red")]
        public int Red { get; set; }
        /// <summary>${WP_REST_ServerType_ServerColor_attribute_Green_D}</summary>
        [JsonProperty("green")]
        public int Green { get; set; }
        /// <summary>${WP_REST_ServerType_ServerColor_attribute_Blue_D}</summary>
        [JsonProperty("blue")]
        public int Blue { get; set; }

    }
}
