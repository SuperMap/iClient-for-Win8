using System.Runtime.Serialization;
using System.Windows;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_MarkerStyle_Title}</para>
    /// 	<para>${WP_core_MarkerStyle_Description}</para>
    /// </summary>
    [KnownType(typeof(PredefinedMarkerStyle)), KnownType(typeof(PictureMarkerStyle)), KnownType(typeof(TextStyle))]
    public class MarkerStyle : Style
    {
        /// <summary>${WP_core_MarkerStyle_field_offsetXProperty_D}</summary>
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register("OffsetX", typeof(double), typeof(MarkerStyle), new PropertyMetadata(0.0, new PropertyChangedCallback(MarkerStyle.OnOffsetXPropertyChanged)));
        /// <summary>${WP_core_MarkerStyle_field_offsetYProperty_D}</summary>
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register("OffsetY", typeof(double), typeof(MarkerStyle), new PropertyMetadata(0.0, new PropertyChangedCallback(MarkerStyle.OnOffsetYPropertyChanged)));

        private static void OnOffsetXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MarkerStyle).OnPropertyChanged("OffsetX");
        }

        private static void OnOffsetYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MarkerStyle).OnPropertyChanged("OffsetY");
        }

        /// <summary>${WP_core_MarkerStyle_attribute_offsetX_D}</summary>
        public virtual double OffsetX
        {
            get
            {
                return (double)base.GetValue(OffsetXProperty);
            }
            set
            {
                base.SetValue(OffsetXProperty, value);
            }
        }

        /// <summary>${WP_core_MarkerStyle_attribute_offsetY_D}</summary>
        public virtual double OffsetY
        {
            get
            {
                return (double)base.GetValue(OffsetYProperty);
            }
            set
            {
                base.SetValue(OffsetYProperty, value);
            }
        }
    }
}
