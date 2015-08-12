

using System.Collections.Generic;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_FindLocationParameters_Title}</para>
    /// 	<para>${REST_FindLocationParameters_Description}</para>
    /// 	<para>
    /// 		<list type="table">
    /// 			<item>
    /// 				<term><img src="Location1.bmp"/></term>
    /// 				<description><img src="Location2.bmp"/></description>
    /// 			</item>
    /// 		</list>
    /// 	</para>
    /// </summary>
    public class FindLocationAnalystParameters
    {
        private int count = 1;
        /// <summary>${REST_FindLocationParameters_constructor_D}</summary>
        public FindLocationAnalystParameters()
        {
        }

        //放在url中
        /// <summary>${REST_FindLocationParameters_attribute_supplyCenters_D}</summary>
        public IList<SupplyCenter> SupplyCenters { get; set; }

        //放在？后面
        /// <summary>${REST_FindLocationParameters_attribute_isFromCenter_D}</summary>
        public bool IsFromCenter { get; set; }
        /// <summary>${REST_FindLocationParameters_attribute_ExpectedSupplyCenterCount_D}</summary>
        public int ExpectedSupplyCenterCount
        {
            get { return count; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                count = value;
            }
        }
       
        /// <summary>${REST_FindLocationParameters_attribute_WeightName_D}</summary>
        public string WeightName { get; set; }
        /// <summary>${REST_FindLocationParameters_attribute_TurnWeightField_D}</summary>
        public string TurnWeightField { get; set; }
        /// <summary>${REST_FindLocationParameters_attribute_returnEdgeFeature_D}</summary>
        public bool ReturnEdgeFeature { get; set; }
        /// <summary>${REST_FindLocationParameters_attribute_ReturnEdgeGeometry_D}</summary>
        public bool ReturnEdgeGeometry { get; set; }
        /// <summary>${REST_FindLocationParameters_attribute_returnNodeFeature_D}</summary>
        public bool ReturnNodeFeature { get; set; }
        ///// <summary>${REST_FindLocationParameters_attribute_MapParameter_D}</summary>
        //public NAResultMapParameter MapParameter { get; set; }

    }
}
