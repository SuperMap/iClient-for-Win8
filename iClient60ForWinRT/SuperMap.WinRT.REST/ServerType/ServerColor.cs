using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ServerType_ServerColor_Tile}</para>
    /// 	<para>${REST_ServerType_ServerColor_Description}</para>
    /// </summary>
    public class ServerColor
    {
        /// <summary>${REST_ServerType_ServerColor_constructor_None_D}</summary>
        /// <overloads>${REST_ServerType_ServerColor_constructor_overloads}</overloads>
        public ServerColor()
        {
            Blue = 0;
            Green = 0;
            Red = 255;
        }
        /// <summary>${REST_ServerType_ServerColor_constructor_String_D}</summary>
        public ServerColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>${REST_ServerType_ServerColor_attribute_red_D}</summary>
        public int Red { get; set; }
        /// <summary>${REST_ServerType_ServerColor_attribute_Green_D}</summary>
        public int Green { get; set; }
        /// <summary>${REST_ServerType_ServerColor_attribute_Blue_D}</summary>
        public int Blue { get; set; }

        internal static string ToJson(ServerColor result)
        {
            if (result == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format("\"red\":{0}", result.Red));
            list.Add(string.Format("\"green\":{0}", result.Green));
            list.Add(string.Format("\"blue\":{0}", result.Blue));
            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ServerColor FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new ServerColor()
            {
                Red = (int)json["red"].GetNumberEx(),
                Green = (int)json["green"].GetNumberEx(),
                Blue = (int)json["blue"].GetNumberEx()
            };
        }
    }
}
