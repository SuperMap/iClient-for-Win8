
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.Networkanalyst
{

    /// <summary>
    /// <para>${iServerJava6R_EdgeWeightParameters_Title}</para>
    /// </summary>
    public class EdgeWeightParameters
    {
        /// <summary>
        /// ${iServerJava6R_EdgeWeightParameters_constructor_D}
        /// </summary>
        public EdgeWeightParameters()
        {
            WeightField = WeightFieldType.TIME;
        }
        /// <summary>${iServerJava6R_EdgeWeightParameters_attribute_EdgeID_D}</summary>
        public int EdgeID { get; set; }
        /// <summary>${iServerJava6R_EdgeWeightParameters_attribute_FromNodeID_D}</summary>
        public int FromNodeID { get; set; }
        /// <summary>${iServerJava6R_EdgeWeightParameters_attribute_ToNodeID_D}</summary>
        public int ToNodeID { get; set; }
        /// <summary>${iServerJava6R_EdgeWeightParameters_attribute_WeightField_D}</summary>
        public WeightFieldType WeightField { get; set; }
        /// <summary>${iServerJava6R_EdgeWeightParameters_attribute_Weight_D}</summary>
        public int Weight { get; set; }
    }

}
