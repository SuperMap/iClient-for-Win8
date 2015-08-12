using System;
using SuperMap.WinRT.Core;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>${mapping_ViewBoundsEventArgs_Title}</summary>
    /// <remarks>${mapping_ViewBoundsEventArgs_Exception}</remarks>
    public sealed class ViewBoundsEventArgs:EventArgs
    {
        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="ViewBoundsEventArgs">ViewBoundsEventArgs</see> ${pubilc_Constructors_instance}
        /// </summary>
        /// <param name="oldViewBounds">${mapping_ViewBoundsEventArgs_constructor_param_oldViewBounds}</param>
        /// <param name="newViewBounds">${mapping_ViewBoundsEventArgs_constructor_param_newViewBounds}</param>
        public ViewBoundsEventArgs(Rectangle2D oldViewBounds, Rectangle2D newViewBounds)
        {
            OldViewBounds = oldViewBounds;
            NewViewBounds = newViewBounds;
        }

        /// <summary>${mapping_ViewBoundsEventArgs_attribute_newViewBounds_D}</summary>
        public Rectangle2D NewViewBounds { get; private set; }
        /// <summary>${mapping_ViewBoundsEventArgs_attribute_oldViewBounds_D}</summary>
        public Rectangle2D OldViewBounds { get; private set; }
    }
}
