using System;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>${WP_mapping_ViewBoundsEventArgs_Title}</summary>
    /// <remarks>${WP_mapping_ViewBoundsEventArgs_Exception}</remarks>
    public sealed class ViewBoundsEventArgs:EventArgs
    {
        /// <summary>
        ///     ${WP_pubilc_Constructors_Initializes} <see cref="ViewBoundsEventArgs">ViewBoundsEventArgs</see> ${WP_pubilc_Constructors_instance}
        /// </summary>
        /// <param name="oldViewBounds">${WP_mapping_ViewBoundsEventArgs_constructor_param_oldViewBounds}</param>
        /// <param name="newViewBounds">${WP_mapping_ViewBoundsEventArgs_constructor_param_newViewBounds}</param>
        public ViewBoundsEventArgs(Rectangle2D oldViewBounds, Rectangle2D newViewBounds)
        {
            OldViewBounds = oldViewBounds;
            NewViewBounds = newViewBounds;
        }

        /// <summary>${WP_mapping_ViewBoundsEventArgs_attribute_newViewBounds_D}</summary>
        public Rectangle2D NewViewBounds { get; private set; }
        /// <summary>${WP_mapping_ViewBoundsEventArgs_attribute_oldViewBounds_D}</summary>
        public Rectangle2D OldViewBounds { get; private set; }
    }
}
