using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${mapping_MapImage_Title}</para>
    /// </summary>
    public class MapImage
    {
        /// <summary>
        ///${Mapping_MapImage_attribute_Url_D}
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// ${Mapping_MapImage_attribute_Data_D}
        /// </summary>
        public byte[] Data { get; set; }
        
        /// <summary>
        /// ${Mapping_MapImage_attribute_MapImageType_D}
        /// </summary>
        public MapImageType MapImageType { get; set; }
    }

    public enum MapImageType
    {
        /// <summary>
        /// ${Mapping_MapImage_enum_MapImageType_Url}
        /// </summary>
        Url,
        /// <summary>
        /// ${Mapping_MapImage_enum_MapImageType_Data}
        /// </summary>
        Data
    }
}
