using SuperMap.WinRT.Utilities;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Projection_Title}</para>
    /// 	<para>${REST_Projection_Description}</para>
    /// </summary>
    public class Projection
    {
        /// <summary>${REST_Projection_constructor_D}</summary>
        public Projection()
        { }
        /// <summary>${REST_Projection_attribute_Name_D}</summary>
        public string Name { get; set; }
        /// <summary>${REST_Projection_attribute_Type_D}</summary>
        public ProjectionType Type { get; set; }


        internal static Projection FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new Projection
            {
                Name = json["name"].GetStringEx(),
                Type = (ProjectionType)Enum.Parse(typeof(ProjectionType),
                json["type"].GetStringEx(), true)
            };
        }
    }
}
