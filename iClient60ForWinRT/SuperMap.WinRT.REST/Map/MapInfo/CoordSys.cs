using SuperMap.WinRT.Utilities;
using System;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_CoordSys_Title}</summary>
    public class CoordSys
    {
        /// <summary>${REST_CoordSys_constructor_D}</summary>
        public CoordSys()
        { }
        /// <summary>${REST_CoordSys_attribute_Unit_D}</summary>
        public Unit Unit { get; set; }
        /// <summary>${REST_CoordSys_attribute_SpatialRefType_D}</summary>
        public SpatialRefType SpatialRefType { get; set; }
        /// <summary>${REST_CoordSys_attribute_PrimeMeridian_D}</summary>
        public PrimeMeridian PrimeMeridian { get; set; }
        /// <summary>${REST_CoordSys_attribute_Name_D}</summary>
        public string Name { get; set; }
        /// <summary>${REST_CoordSys_attribute_Type_D}</summary>
        public CoordSysType Type { get; set; }
        /// <summary>${REST_CoordSys_attribute_Datum_D}</summary>
        public Datum Datum { get; set; }


        internal static CoordSys FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }

            return new CoordSys
            {
                Unit = (Unit)Enum.Parse(typeof(Unit), json["unit"].GetStringEx(), true),
                SpatialRefType = (SpatialRefType)Enum.Parse(typeof(SpatialRefType), json["spatialRefType"].GetStringEx(), true),
                PrimeMeridian = PrimeMeridian.FromJson(json["primeMeridian"].GetObjectEx()),
                Name = json["name"].GetStringEx(),
                Type = (CoordSysType)Enum.Parse(typeof(CoordSysType), json["type"].GetStringEx(), true),
                Datum = Datum.FromJson(json["datum"].GetObjectEx()),
            };
        }
    }
}
