using System;
namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_ResolutionsEventArgs_Title}</para>
    /// 	<para>${WP_mapping_ResolutionsEventArgs_Description}</para>
    /// </summary>
    public sealed class ResolutionsEventArgs : EventArgs
    {
        /// <summary>${WP_mapping_ResolutionsEventArgs_constructor_D}</summary>
        /// <param name="newResolutions">${WP_mapping_ResolutionsEventArgs_constructor_param_newResolutions}</param>
        /// /// <param name="oldResulotions">${WP_mapping_ResolutionsEventArgs_constructor_param_oldResulotions}</param>
        public ResolutionsEventArgs(double[] newResolutions, double[] oldResulotions)
        {
            NewResolutions = newResolutions;
            OldResulotions = oldResulotions;
        }
        /// <summary>${WP_mapping_ResolutionsEventArgs_attribute_NewResolutions_D}</summary>
        public double[] NewResolutions { get; private set; }
        /// <summary>${WP_mapping_ResolutionsEventArgs_attribute_OldResulotions_D}</summary>
        public double[] OldResulotions { get; private set; }
    }
}
