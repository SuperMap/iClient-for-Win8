using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_TextStyle_Title}</para>
    /// 	<para>${WP_core_TextStyle_Description}</para>
    /// </summary>
    public sealed class TextStyle : MarkerStyle
    {
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_TextStyle_field_fontFamilyProperty_D}</summary>
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(System.Windows.Media.FontFamily), typeof(TextStyle), new PropertyMetadata(new System.Windows.Media.FontFamily("Arial")));
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_TextStyle_field_fontSizeProperty_D}</summary>
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(TextStyle), new PropertyMetadata(10.0));
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_TextStyle_field_foreGroundProperty_D}</summary>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(TextStyle), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_TextStyle_field_textProperty_D}</summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextStyle), null);
        /// <summary>${WP_core_TextStyle_constructor_None_D}</summary>
        public TextStyle()
        {
            ControlTemplate = ResourceData.Dictionary["TextStyle"] as ControlTemplate;
        }
        /// <summary>${WP_core_TextStyle_attribute_fontFamily_D}</summary>
        public FontFamily FontFamily
        {
            get
            {
                return (System.Windows.Media.FontFamily)base.GetValue(FontFamilyProperty);
            }
            set
            {
                base.SetValue(FontFamilyProperty, value);
            }
        }

        /// <summary>${WP_core_TextStyle_attribute_fontSize_D}</summary>
        public double FontSize
        {
            get
            {
                return (double)base.GetValue(FontSizeProperty);
            }
            set
            {
                base.SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>${WP_core_TextStyle_attribute_foreGround_D}</summary>
        public Brush Foreground
        {
            get
            {
                return (Brush)base.GetValue(ForegroundProperty);
            }
            set
            {
                base.SetValue(ForegroundProperty, value);
            }
        }

        /// <summary>${WP_core_TextStyle_attribute_text_D}</summary>
        public string Text
        {
            get
            {
                return (string)base.GetValue(TextProperty);
            }
            set
            {
                base.SetValue(TextProperty, value);
            }
        }
    }
}

