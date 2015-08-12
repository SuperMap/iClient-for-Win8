using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_PictureFillStyle_Title}</para>
    /// 	<para>${core_PictureFillStyle_Description}</para>
    /// </summary>
    public sealed class PictureFillStyle : FillStyle
    {
        /// <summary>${core_PictureFillStyle_constructor_None_D}</summary>
        public PictureFillStyle()
        {
        }
        /// <summary>${core_PictureFillStyle_attribute_source_D}</summary>
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

        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_PictureFillStyle_field_sourceProperty_D}</summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(PictureFillStyle), null);
    }
}
