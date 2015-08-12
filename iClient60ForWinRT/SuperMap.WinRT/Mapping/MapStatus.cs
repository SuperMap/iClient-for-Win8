using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// ${mapping_MapStatus_Title} 
    /// </summary>
    public enum MapStatus
    {
        /// <summary>
        /// ${Mapping_MapStatus_enum_Still_D}  
        /// </summary>
        Still = 0,
        /// <summary>
        /// ${Mapping_MapStatus_enum_PanStarted_D}  
        /// </summary>
        PanStarted = 1,
        /// <summary>
        /// ${Mapping_MapStatus_enum_Panning_D}
        /// </summary>
        Panning = 2,
        /// <summary>
        /// ${Mapping_MapStatus_enum_PanCompleted_D}
        /// </summary>
        PanCompleted = 3,
        /// <summary>
        /// ${Mapping_MapStatus_enum_ZoomStarted_D}
        /// </summary>
        ZoomStarted = 4,
        /// <summary>
        /// ${Mapping_MapStatus_enum_Zooming_D}
        /// </summary>
        Zooming = 5,
        /// <summary>
        /// ${Mapping_MapStatus_enum_ZoomCompleted_D}
        /// </summary>
        ZoomCompleted = 6
    }
}
