using System;
namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${mapping_ScalesEventArgs_Title}</para>
    /// 	<para>${mapping_ScalesEventArgs_Description}</para>
    /// </summary>
    public sealed class ScalesEventArgs : EventArgs
    {
        /// <summary>${mapping_ScalesEventArgs_constructor_D}</summary>
        /// <param name="oldScales">${mapping_ScalesEventArgs_constructor_param_oldScales}</param>
        /// /// <param name="newScales">${mapping_ScalesEventArgs_constructor_param_newScales}</param>
        public ScalesEventArgs(double[] oldScales, double[] newScales)
        {
            OldScales = oldScales;
            NewScales = newScales;
        }
        /// <summary>${mapping_ScalesEventArgs_attribute_oldScales_D}</summary>
        public double[] OldScales { get; private set; }
        /// <summary>${mapping_ScalesEventArgs_attribute_newScales_D}</summary>
        public double[] NewScales { get; private set; }
    }
}
