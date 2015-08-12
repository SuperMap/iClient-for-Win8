using SuperMap.WinRT.Core;
using System.Collections.Generic;

namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_EditFeaturesParameters_Title}</para>
    /// 	<para>${REST_EditFeaturesParameters_Description}</para>
    /// </summary>
    public class EditFeaturesParameters
    {
        /// <summary>${REST_EditFeaturesParameters_constructor_D}</summary>
        public EditFeaturesParameters()
        {
            EditType = EditType.ADD;
        }
        /// <summary>${REST_EditFeaturesParameters_attribute_Features_D}</summary>
        public FeatureCollection Features { get; set; }

        /// <summary>${REST_EditFeaturesParameters_attribute_EditType_D}</summary>
        public EditType EditType { get; set; }

        /// <summary>${REST_EditFeaturesParameters_attribute_IDs_D}</summary>
        public IList<int> IDs { get; set; }
    }
}
