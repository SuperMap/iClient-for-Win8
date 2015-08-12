using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// ${WP_Mapping_MapImage_title}
    /// </summary>
    public class MapImage
    {
        /// <summary>
        /// ${WP_Mapping_MapImage_attribute_Url_D}
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ${WP_Mapping_MapImage_attribute_Data_D}
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// ${WP_Mapping_MapImage_attribute_MapImageType_D}
        /// </summary>
        public MapImageType MapImageType { get; set; }
    }
    /// <summary>
    /// ${WP_Mapping_enum_MapImageType_title}
    /// </summary>
    public enum MapImageType
    {
        Url,
        Data
    }
}
