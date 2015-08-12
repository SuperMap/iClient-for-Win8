using Newtonsoft.Json;
using SuperMap.WindowsPhone.Core;
using System;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_PrjCoordSys_Title}</para>
    /// 	<para>${WP_REST_PrjCoordSys_Description}</para>
    /// </summary>
    public class PrjCoordSys
    {
        /// <summary>${WP_REST_PrjCoordSys_constructor_D}</summary>
        public PrjCoordSys()
        { }
        /// <summary>${WP_REST_PrjCoordSys_attribute_CoordUnit_D}</summary>
        [JsonProperty("coordUnit")]
        public Unit CoordUnit { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_Name_D}</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_Projection_D}</summary>
        [JsonProperty("projection")]
        public Projection Projection { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_CoordSystem_D}</summary>
        [JsonProperty("coordSystem")]
        public CoordSys CoordSystem { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_DistanceUnit_D}</summary>
        [JsonProperty("distanceUnit")]
        public Unit DistanceUnit { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_ProjectionParam_D}</summary>
        [JsonProperty("projectionParam")]
        public PrjParameter ProjectionParam { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_epsgCode_D}</summary>
        [JsonProperty("epsgCode")]
        public int EpsgCode { get; set; }
        /// <summary>${WP_REST_PrjCoordSys_attribute_Type_D}</summary>
        [JsonProperty("type")]
        public PrjCoordSysType Type { get; set; }

    }
}
