using System.Collections.Generic;

namespace SuperMap.Web.OGC
{
    /// <summary>${mapping_LayerType_Tile}</summary>
    public class WFSFeatureDescription
    {
        /// <summary>${mapping_LayerType_constructor_D}</summary>
        public WFSFeatureDescription()
        {
            //IDs = new List<string>();
            Properties = new List<string>();
        }

        //iServer默认的是：the_geom
        private string spatialProperty = "the_geom";
        /// <summary>${mapping_LayerType_attribute_TypeName_D}</summary>
        public string TypeName { get; set; }
        ///// <summary>${mapping_LayerType_attribute_IDs_D}</summary>
        //public List<string> IDs { get; internal set; }
        /// <summary>${mapping_LayerType_attribute_Properties_D}</summary>
        public List<string> Properties { get; set; }
        /// <summary>${mapping_LayerType_attribute_SpacialProperty_D}</summary>        
        public string SpatialProperty
        {
            get { return spatialProperty; }
            set { spatialProperty = value; }
        }
    }
}
