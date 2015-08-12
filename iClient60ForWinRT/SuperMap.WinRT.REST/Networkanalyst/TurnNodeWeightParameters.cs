using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.Networkanalyst
{
    /// <summary>
    /// <para>${iServerJava6R_TurnNodeWeightParameters_Title}</para>
    /// </summary>
    public class TurnNodeWeightParameters
    {
        /// <summary>
        /// ${iServerJava6R_TurnNodeWeightParameters_constructor_D}
        /// </summary>
        public TurnNodeWeightParameters()
        {
            WeightField = TurnNodeWeightFieldType.TurnCost;
        }
        /// <summary>${iServerJava6R_TurnNodeWeightParameters_attribute_NodeID_D}</summary>
        public int NodeID { get; set; }
        /// <summary>${iServerJava6R_TurnNodeWeightParameters_attribute_FromEdgeID_D}</summary>
        public int FromEdgeID { get; set; }
        /// <summary>${iServerJava6R_TurnNodeWeightParameters_attribute_ToEdgeID_D}</summary>
        public int ToEdgeID { get; set; }
        /// <summary>${iServerJava6R_TurnNodeWeightParameters_attribute_WeightField_D}</summary>
        public TurnNodeWeightFieldType WeightField { get; set; }
        /// <summary>${iServerJava6R_TurnNodeWeightParameters_attribute_Weight_D}</summary>
        public int Weight { get; set; }

    }
}
