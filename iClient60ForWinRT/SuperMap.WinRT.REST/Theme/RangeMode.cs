using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_RangeMode_Title}</para>
    /// </summary>
    public enum RangeMode
    {
        //自定义分段。
        /// <summary>${REST_RangeMode_Custominterval_D}</summary>
        CUSTOMINTERVAL,

        //等距离分段。
        /// <summary>${REST_RangeMode_Equalinterval_D}</summary>
        EQUALINTERVAL,

        //对数分段。
        /// <summary>${REST_RangeMode_Logarithm_D}</summary>
        LOGARITHM,

        //等计数分段。
        /// <summary>${REST_RangeMode_Quantile_D}</summary>
        QUANTILE,

        //平方根分段。
        /// <summary>${REST_RangeMode_Squareroot_D}</summary>
        SQUAREROOT,

        //标准差分段。 
        /// <summary>${REST_RangeMode_Stddeviation_D}</summary>
        STDDEVIATION
    }
}
