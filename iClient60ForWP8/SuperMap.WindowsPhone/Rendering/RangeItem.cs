namespace SuperMap.WindowsPhone.Rendering
{
    /// <summary>
    /// 	<para>${WP_mapping_RangeItem_Title}</para>
    /// 	<para>${WP_mapping_RangeItem_Description}</para>
    /// </summary>
    public class RangeItem
    {
        /// <summary>${WP_mapping_RangeItem_attribute_maximumValue_D}</summary>
        public double MaximumValue { get; set; }

        /// <summary>${WP_mapping_RangeItem_attribute_minimumValue_D}</summary>
        public double MinimumValue { get; set; }

        /// <summary>${WP_mapping_RangeItem_attribute_style_D}</summary>
        public SuperMap.WindowsPhone.Core.Style Style { get; set; }

    }
}
