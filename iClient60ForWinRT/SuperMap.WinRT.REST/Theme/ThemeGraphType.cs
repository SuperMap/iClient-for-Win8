using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeGraphType_Title}</para>
    /// 	<para>${REST_ThemeGraphType_Description}</para>
    /// </summary>
    public enum ThemeGraphType
    {
        /// <summary>${REST_ThemeGraphType_attribute_Area_D}</summary>
        AREA,                   //面积图。 
        /// <summary>${REST_ThemeGraphType_attribute_Bar_D}</summary>
        BAR,                    //柱状图。 
        /// <summary>${REST_ThemeGraphType_attribute_Bar3D_D}</summary>
        BAR3D,                  //三维柱状图。 
        /// <summary>${REST_ThemeGraphType_attribute_Line_D}</summary>
        LINE,                   //折线图。
        /// <summary>${REST_ThemeGraphType_attribute_Pie_D}</summary>
        PIE,                    //饼图。 
        /// <summary>${REST_ThemeGraphType_attribute_Pie3D_D}</summary>
        PIE3D,                  //三维饼图。
        /// <summary>${REST_ThemeGraphType_attribute_Point_D}</summary>
        POINT,                  //点状图。 
        /// <summary>${REST_ThemeGraphType_attribute_Ring_D}</summary>
        RING,                   //环状图。
        /// <summary>${REST_ThemeGraphType_attribute_Rose_D}</summary>
        ROSE,                   //玫瑰图。 
        /// <summary>${REST_ThemeGraphType_attribute_Rose3D_D}</summary>
        ROSE3D,                 //三维玫瑰图。 
        /// <summary>${REST_ThemeGraphType_attribute_Strack_Bar_D}</summary>
        STACK_BAR,              //堆叠柱状图。 
        /// <summary>${REST_ThemeGraphType_attribute_Stack_Bar3D_D}</summary>
        STACK_BAR3D,            //三维堆叠柱状图。
        /// <summary>${REST_ThemeGraphType_attribute_Step_D}</summary>
        STEP,                   //阶梯图。 
    }
}
