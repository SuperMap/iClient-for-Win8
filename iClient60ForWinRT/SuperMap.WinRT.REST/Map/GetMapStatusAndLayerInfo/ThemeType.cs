using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_ThemeType_Title}</summary>
    public enum ThemeType
    {
        /// <summary>${REST_ThemeType_attribute_DOTDENSITY_D}</summary>
        DOTDENSITY,//           点密度专题图。 
        /// <summary>${REST_ThemeType_attribute_GRADUATEDSYMBOL_D}</summary>
        GRADUATEDSYMBOL,//           等级符号专题图。 
        /// <summary>${REST_ThemeType_attribute_GRAPH_D}</summary>
        GRAPH,//           统计专题图。
        /// <summary>${REST_ThemeType_attribute_LABEL_D}</summary>
        LABEL,//           标签专题图。 
        /// <summary>${REST_ThemeType_attribute_RANGE_D}</summary>
        RANGE,//           分段专题图。
        /// <summary>${REST_ThemeType_attribute_UNIQUE_D}</summary>
        UNIQUE,//           単值专题图。 

        //这两个不要
        //GRIDRANGE,//           栅格分段专题图。 
        //GRIDUNIQUE,//           栅格单值专题图。 
    }
}
