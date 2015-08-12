using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_GraduatedMode_Title}</para>
    /// 	<para>${REST_GraduatedMode_Description}</para>
    /// </summary>
    public enum GraduatedMode
    {
        /// <summary>${REST_GraduatedMode_attribute_Constant_D}</summary>
        CONSTANT,
        //常量分级模式。 
        /// <summary>${REST_GraduatedMode_attribute_Logarithm_D}</summary>
        LOGARITHM,
        //对数分级模式。 
        /// <summary>${REST_GraduatedMode_attribute_Squareroot_D}</summary>
        SQUAREROOT,
        //平方根分级模式。 

    }
}
