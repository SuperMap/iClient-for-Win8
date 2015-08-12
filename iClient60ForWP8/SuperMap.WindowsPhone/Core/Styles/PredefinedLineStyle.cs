using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_PredefineLineStyle_Title}</para>
    /// 	<para>${WP_core_PredefineLineStyle_Description}</para>
    /// </summary>
    public sealed class PredefinedLineStyle : LineStyle
    {
        /// <summary>${WP_core_PredefinedLineStyle_enum_LineSymbol_D}</summary>
        public enum LineSymbol
        {
            /// <summary><img src="dash.png"/></summary>
            Dash,
            /// <summary><img src="dashdot.png"/></summary>
            DashDot,
            /// <summary><img src="dashdotdot.png"/></summary>
            DashDotDot,
            /// <summary><img src="dot.png"/></summary>
            Dot,
            /// <summary><img src="solid.png"/></summary>
            Solid
        }

        /// <summary>${WP_core_PredefinedLineStyle_constructor_None_D}</summary>
        public PredefinedLineStyle()
        {
            ControlTemplate = ResourceData.Dictionary["PredefinedLineStyle_Solid"] as ControlTemplate;
        }

        /// <summary>${WP_core_PredefinedLineStyle_attribute_symbol_D}</summary>
        public LineSymbol Symbol
        {
            get { return (LineSymbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_PredefinedLineStyle_field_symbolProperty_D}</summary>
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof(LineSymbol), typeof(PredefinedLineStyle), new PropertyMetadata(LineSymbol.Solid, new PropertyChangedCallback(onSymbolChanged)));

        private static void onSymbolChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as PredefinedLineStyle).ControlTemplate = ResourceData.Dictionary[string.Format(CultureInfo.InvariantCulture, "PredefinedLineStyle_{0}", e.NewValue)] as ControlTemplate;
        }
    }
}

