using System;
using SuperMap.WinRT.Core;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_PrjCoordSys_Title}</para>
    /// 	<para>${REST_PrjCoordSys_Description}</para>
    /// </summary>
    public class PrjCoordSys
    {
        /// <summary>${REST_PrjCoordSys_constructor_D}</summary>
        public PrjCoordSys()
        { }
        /// <summary>${REST_PrjCoordSys_attribute_CoordUnit_D}</summary>
        public Unit CoordUnit { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_Name_D}</summary>
        public string Name { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_Projection_D}</summary>
        public Projection Projection { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_CoordSystem_D}</summary>
        public CoordSys CoordSystem { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_DistanceUnit_D}</summary>
        public Unit DistanceUnit { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_ProjectionParam_D}</summary>
        public PrjParameter ProjectionParam { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_epsgCode_D}</summary>
        public int EpsgCode { get; set; }
        /// <summary>${REST_PrjCoordSys_attribute_Type_D}</summary>
        public PrjCoordSysType Type { get; set; }

        internal static PrjCoordSys FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            PrjCoordSys coorSys = new PrjCoordSys();
            coorSys.CoordUnit = (Unit)Enum.Parse(typeof(Unit), json["coordUnit"].GetStringEx(), true);
            coorSys.Name = json["name"].GetStringEx();
            coorSys.Projection = Projection.FromJson(json["projection"].GetObjectEx());
            coorSys.CoordSystem = CoordSys.FromJson(json["coordSystem"].GetObjectEx());
            coorSys.DistanceUnit = (Unit)Enum.Parse(typeof(Unit), json["distanceUnit"].GetStringEx(), true);
            coorSys.ProjectionParam = PrjParameter.FromJson(json["projectionParam"].GetObjectEx());
            //Type = json["type"].GetStringEx(),
            if (json["type"].ValueType != JsonValueType.Null)
            {
                coorSys.Type = (PrjCoordSysType)Enum.Parse(typeof(PrjCoordSysType), json["type"].GetStringEx(), true);
            }
            coorSys.EpsgCode = (int)json["epsgCode"].GetNumberEx();

            return coorSys;
        }
    }
}
