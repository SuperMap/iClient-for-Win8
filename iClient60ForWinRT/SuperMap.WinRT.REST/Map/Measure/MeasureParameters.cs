
using System.Collections.Generic;
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_MeasureParameters_Tile}</para>
    /// 	<para>${REST_MeasureParameters_Description}</para>
    /// </summary>
    public class MeasureParameters
    {
        /// <summary>${REST_MeasureParameters_constructor_None_D}</summary>
        public MeasureParameters()
        {
            Unit = Unit.Meter;
        }

        /// <summary>${REST_MeasureParameters_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }
        /// <summary>${REST_MeasureParameters_attribute_Unit_D}</summary>
        public Unit Unit { get; set; }
    }
}
