using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_PictureMarkerStyle_Title}</para>
    /// 	<para>${core_PictureMarkerStyle_Description}</para>
    /// </summary>
    public sealed class PictureMarkerStyle : MarkerStyle
    {
        /// <summary>${core_PictureMarkerStyle_constructor_None_D}</summary>
        public PictureMarkerStyle()
        {
            ControlTemplate = ResourceData.Dictionary["PictureMarkerStyle"] as ControlTemplate;
        }

        /// <summary>${core_PictureFillStyle_attribute_source_D}</summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_PictureFillStyle_field_sourceProperty_D}</summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(PictureMarkerStyle), null);

        /// <summary>${core_PictureMarkerStyle_attribute_height_D}</summary>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_PictureMarkerStyle_field_heightProperty_D}</summary>
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(PictureMarkerStyle), new PropertyMetadata(10.0));

        /// <summary>${core_PictureMarkerStyle_attribute_width_D}</summary>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_PictureMarkerStyle_field_widthProperty_D}</summary>
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(PictureMarkerStyle), new PropertyMetadata(10.0));
    }
}
