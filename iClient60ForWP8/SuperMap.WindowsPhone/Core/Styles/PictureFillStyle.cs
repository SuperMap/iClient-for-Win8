using System.Windows;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_PictureFillStyle_Title}</para>
    /// 	<para>${WP_core_PictureFillStyle_Description}</para>
    /// </summary>
    public sealed class PictureFillStyle : FillStyle
    {
        /// <summary>${WP_core_PictureFillStyle_constructor_None_D}</summary>
        public PictureFillStyle()
        {
        }
        /// <summary>${WP_core_PictureFillStyle_attribute_source_D}</summary>
        public ImageSource Source
        {
            get
            {
                ImageBrush image = base.Fill as ImageBrush;
                if (image != null)
                {
                    return image.ImageSource;
                }
                return null;
            }
            set
            {
                ImageBrush image = new ImageBrush();
                image.ImageSource = value;
                base.Fill = image;
            }
        }

        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_PictureFillStyle_field_sourceProperty_D}</summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(PictureFillStyle), null);
    }
}
