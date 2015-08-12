
using System.Windows.Controls;
namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_PredefineFillStyle_Title}</para>
    /// 	<para>${WP_core_PredefineFillStyle_Description}</para>
    /// </summary>
    public sealed class PredefinedFillStyle : FillStyle
    {
        /// <summary>${WP_core_PredefinedFillStyle_constructor_None_D}</summary>
        public PredefinedFillStyle()
        {
            ControlTemplate = ResourceData.Dictionary["PredefinedFillStyle"] as ControlTemplate;
        }
    }
}
