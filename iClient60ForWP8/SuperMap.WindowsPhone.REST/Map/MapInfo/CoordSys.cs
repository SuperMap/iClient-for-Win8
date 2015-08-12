using Newtonsoft.Json;
using SuperMap.WindowsPhone.Core;
using System;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_CoordSys_Title}</summary>
    public class CoordSys
    {
        /// <summary>${WP_REST_CoordSys_constructor_D}</summary>
        public CoordSys()
        { }
        /// <summary>${WP_REST_CoordSys_attribute_Unit_D}</summary>
        [JsonProperty("unit")]
        public Unit Unit { get; set; }
        /// <summary>${WP_REST_CoordSys_attribute_SpatialRefType_D}</summary>
        [JsonProperty("spatialRefType")]
        public SpatialRefType SpatialRefType { get; set; }
        /// <summary>${WP_REST_CoordSys_attribute_PrimeMeridian_D}</summary>
        [JsonProperty("primeMeridian")]
        public PrimeMeridian PrimeMeridian { get; set; }
        /// <summary>${WP_REST_CoordSys_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_CoordSys_attribute_Type_D}</summary>
        [JsonProperty("type")]
        public CoordSysType Type { get; set; }
        /// <summary>${WP_REST_CoordSys_attribute_Datum_D}</summary>
        [JsonProperty("datum")]
        public Datum Datum { get; set; }

    }
}
