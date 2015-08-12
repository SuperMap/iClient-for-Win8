using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// ${WP_REST_EngineType_title}
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EngineType
    {
        /// <summary>${WP_REST_EngineType_attribute_DB2_D}</summary>
        DB2,
        /// <summary>${WP_REST_EngineType_attribute_GOOGLEMAPS_D}</summary>
        GOOGLEMAPS,
        /// <summary>${WP_REST_EngineType_attribute_IMAGEPLUGINS_D}</summary>
        IMAGEPLUGINS,
        /// <summary>${WP_REST_EngineType_attribute_ISERVERREST_D}</summary>
        ISERVERREST,
        /// <summary>${WP_REST_EngineType_attribute_KINGBASE_D}</summary>
        KINGBASE,
        /// <summary>${WP_REST_EngineType_attribute_MAPWORLD_D}</summary>
        MAPWORLD,
        /// <summary>${WP_REST_EngineType_attribute_NetCDF_D}</summary>
        NetCDF,
        /// <summary>${WP_REST_EngineType_attribute_OGC_D}</summary>
        OGC,
        /// <summary>${WP_REST_EngineType_attribute_ORACLEPLUS_D}</summary>
        ORACLEPLUS,
        /// <summary>${WP_REST_EngineType_attribute_ORACLESPATIAL_D}</summary>
        ORACLESPATIAL,
        /// <summary>${WP_REST_EngineType_attribute_POSTGRESQL_D}</summary>
        POSTGRESQL,
        /// <summary>${WP_REST_EngineType_attribute_SDBPLUS_D}</summary>
        SDBPLUS,
        /// <summary>${WP_REST_EngineType_attribute_SQLPLUS_D}</summary>
        SQLPLUS,
        /// <summary>${WP_REST_EngineType_attribute_SUPERMAPCLOUD_D}</summary>
        SUPERMAPCLOUD,
        /// <summary>${WP_REST_EngineType_attribute_UDB_D}</summary>
        UDB
    }
}
