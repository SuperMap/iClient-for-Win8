namespace SuperMap.WinRT.Rendering
{
    /// <summary>
    /// 	<para>${mapping_RangeItem_Title}</para>
    /// 	<para>${mapping_RangeItem_Description}</para>
    /// </summary>
    public class RangeItem
    {
        /// <summary>${mapping_RangeItem_attribute_maximumValue_D}</summary>
        public double MaximumValue { get; set; }

        /// <summary>${mapping_RangeItem_attribute_minimumValue_D}</summary>
        public double MinimumValue { get; set; }

        /// <summary>${mapping_RangeItem_attribute_style_D}</summary>
        public SuperMap.WinRT.Core.Style Style { get; set; }

    }
}
