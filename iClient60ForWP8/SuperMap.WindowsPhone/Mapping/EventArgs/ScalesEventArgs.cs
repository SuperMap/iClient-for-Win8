using System;
namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_ScalesEventArgs_Title}</para>
    /// 	<para>${WP_mapping_ScalesEventArgs_Description}</para>
    /// </summary>
    public sealed class ScalesEventArgs : EventArgs
    {
        /// <summary>${WP_mapping_ScalesEventArgs_constructor_D}</summary>
        /// <param name="oldScales">${WP_mapping_ScalesEventArgs_constructor_param_oldScales}</param>
        /// /// <param name="newScales">${WP_mapping_ScalesEventArgs_constructor_param_newScales}</param>
        public ScalesEventArgs(double[] oldScales, double[] newScales)
        {
            OldScales = oldScales;
            NewScales = newScales;
        }
        /// <summary>${WP_mapping_ScalesEventArgs_attribute_oldScales_D}</summary>
        public double[] OldScales { get; private set; }
        /// <summary>${WP_mapping_ScalesEventArgs_attribute_newScales_D}</summary>
        public double[] NewScales { get; private set; }
    }
}
