using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_TextStyle_Title}</para>
    /// 	<para>${core_TextStyle_Description}</para>
    /// </summary>
    public sealed class TextStyle : MarkerStyle
    {
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_TextStyle_field_fontFamilyProperty_D}</summary>
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(TextStyle), new PropertyMetadata(new FontFamily("Arial")));
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_TextStyle_field_fontSizeProperty_D}</summary>
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(TextStyle), new PropertyMetadata(10.0));
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_TextStyle_field_foreGroundProperty_D}</summary>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(TextStyle), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_TextStyle_field_textProperty_D}</summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextStyle), null);
        /// <summary>${core_TextStyle_constructor_None_D}</summary>
        public TextStyle()
        {
            ControlTemplate = ResourceData.Dictionary["TextStyle"] as ControlTemplate;
        }
        /// <summary>${core_TextStyle_attribute_fontFamily_D}</summary>
        public FontFamily FontFamily
        {
            get
            {
                return (FontFamily)base.GetValue(FontFamilyProperty);
            }
            set
            {
                base.SetValue(FontFamilyProperty, value);
            }
        }

        /// <summary>${core_TextStyle_attribute_fontSize_D}</summary>
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

        /// <summary>${core_TextStyle_attribute_foreGround_D}</summary>
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

        /// <summary>${core_TextStyle_attribute_text_D}</summary>
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

