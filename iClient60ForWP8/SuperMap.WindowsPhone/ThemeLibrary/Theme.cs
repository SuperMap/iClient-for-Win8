using System.Windows.Media;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Theme
{
    /// <summary>
    /// 	<para>${WP_core_Theme_Title}</para>
    /// 	<para>${WP_core_Theme_Description}</para>
    /// </summary>
    public class Theme
    {
        /// <summary>${WP_core_Theme_constructor_None_D}</summary>
        public Theme( )
        {
        }
        /// <summary>${WP_core_Theme_attribute_Fill_D}</summary>
        public Brush Fill { get; set; }
        /// <summary>${WP_core_Theme_attribute_Stroke_D}</summary>
        public Brush Stroke { get; set; }
        /// <summary>${WP_core_Theme_attribute_StrokeThickness_D}</summary>
        public double StrokeThickness { get; set; }
        /// <summary>${WP_core_Theme_attribute_Opacity_D}</summary>
        public double Opacity { get; set; }
        /// <summary>${WP_core_Theme_attribute_Size_D}</summary>
        public double Size { get; set; }
        /// <summary>${WP_core_Theme_attribute_Color_D}</summary>
        public Brush Color { get; set; }
        /// <summary>${WP_core_Theme_attribute_HoverVertexStyle_D}</summary>
        public PredefinedMarkerStyle HoverVertexStyle { get; set; }
        /// <summary>${WP_core_Theme_attribute_SnapStyle_D}</summary>
        public PredefinedMarkerStyle SnapStyle { get; set; }
        /// <summary>${WP_core_Theme_attribute_HoverCenterStyle_D}</summary>
        public PredefinedMarkerStyle HoverCenterStyle { get; set; }
    }

}
