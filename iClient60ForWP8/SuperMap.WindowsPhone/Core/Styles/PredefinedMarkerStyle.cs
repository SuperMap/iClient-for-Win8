using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_PredefinedMarkerStyle_Title}</para>
    /// 	<para>${WP_core_PredefinedMarkerStyle_Description}</para>
    /// </summary>
    public sealed class PredefinedMarkerStyle : MarkerStyle
    {
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_PredefinedMarkerStyle_field_colorProperty_D}</summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register
            ("Color", typeof(Brush), typeof(PredefinedMarkerStyle), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(OnColorPropertyChanged)));
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_PredefinedMarkerStyle_field_sizeProperty_D}</summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register
            ("Size", typeof(double), typeof(PredefinedMarkerStyle), new PropertyMetadata(10.0, new PropertyChangedCallback(OnSizePropertyChanged)));
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_PredefinedMarkerStyle_field_symbolProperty_D}</summary>
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register
            ("Symbol", typeof(MarkerSymbol), typeof(PredefinedMarkerStyle), new PropertyMetadata(MarkerSymbol.Circle, new PropertyChangedCallback(OnSymbolChanged)));

        /// <summary>${WP_core_PredefinedMarkerStyle_constructor_None_D}</summary>
        public PredefinedMarkerStyle()
        {
            ControlTemplate = ResourceData.Dictionary["PredefinedMarkerStyle_Circle"] as ControlTemplate;
        }

        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void OnSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PredefinedMarkerStyle style = d as PredefinedMarkerStyle;
            style.OnPropertyChanged("Size");
            style.OnPropertyChanged("OffsetX");
            style.OnPropertyChanged("OffsetY");
        }

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PredefinedMarkerStyle).ControlTemplate = ResourceData.Dictionary[string.Format(CultureInfo.InvariantCulture, "PredefinedMarkerStyle_{0}", e.NewValue)] as ControlTemplate;
        }

        /// <summary>${WP_core_PredefinedMarkerStyle_attribute_color_D}</summary>
        public Brush Color
        {
            get
            {
                return (Brush)base.GetValue(ColorProperty);
            }
            set
            {
                base.SetValue(ColorProperty, value);
            }
        }

        /// <summary>${WP_core_PredefinedMarkerStyle_attribute_offsetX_D}</summary>
        public override double OffsetX
        {
            get
            {
                return (this.Size * 0.5);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>${WP_core_PredefinedMarkerStyle_attribute_offsetY_D}</summary>
        public override double OffsetY
        {
            get
            {
                return (this.Size * 0.5);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>${WP_core_PredefinedMarkerStyle_attribute_size_D}</summary>
        public double Size
        {
            get
            {
                return (double)base.GetValue(SizeProperty);
            }
            set
            {
                value = value <= 0 ? 10 : value;
                base.SetValue(SizeProperty, value);
            }
        }

        /// <summary>${WP_core_PredefinedMarkerStyle_attribute_symbol_D}</summary>
        public MarkerSymbol Symbol
        {
            get
            {
                return (MarkerSymbol)base.GetValue(SymbolProperty);
            }
            set
            {
                base.SetValue(SymbolProperty, value);
            }
        }

        /// <summary>${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_D}</summary>
        public enum MarkerSymbol
        {
            /// <summary>
            /// 	<para>${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_circle}:<img src="circle.png"/></para>
            /// </summary>
            Circle,
            /// <summary>
            /// ${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_square}:<img src="square.png"/>
            /// </summary>
            Square,
            /// <summary>
            /// 	<para>${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_cross}:<img src="cross.png"/></para>
            /// </summary>
            Cross,
            /// <summary>
            /// ${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_diamond}:<img src="diamond.png"/>
            /// </summary>
            Diamond,
            /// <summary>
            /// ${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_triangle}:<img src="triangle.png"/>
            /// </summary>
            Triangle,
            /// <summary>
            /// ${WP_core_PredefinedMarkerStyle_enum_MarkerSymbol_star}:<img src="star.png"/>
            /// </summary>
            Star
        }
    }
}
