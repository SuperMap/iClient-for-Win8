using System.Runtime.Serialization;
using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_LineStyle_Title}</para>
    /// 	<para>${core_LineStyle_Description}</para>
    /// </summary>
    [KnownType(typeof(PredefinedLineStyle))]
    public class LineStyle : Style
    {
        /// <summary>${core_LineStyle_constructor_None_D}</summary>
        public LineStyle()
        {
            ControlTemplate = ResourceData.Dictionary["LineStyle"] as ControlTemplate;
        }
        /// <summary>${core_LineStyle_attribute_stroke_D}</summary>
        /// <remarks>${core_Style_Public_attribute_Remarks}</remarks>
        public Brush Stroke
        {
            get
            {
                return (base.GetValue(StrokeProperty) as Brush);
            }
            set
            {
                base.SetValue(StrokeProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeProperty_D}</summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(LineStyle), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        /// <summary>${core_LineStyle_attribute_strokeThickness_D}</summary>
        public double StrokeThickness
        {
            get
            {
                return (double)base.GetValue(StrokeThicknessProperty);
            }
            set
            {
                base.SetValue(StrokeThicknessProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeThicknessProperty_D}</summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(LineStyle), new PropertyMetadata(1.0));
        /// <summary>${core_FillStyle_attribute_strokeDashArray_D}</summary>
        /// <remarks>${core_FillStyle_attribute_strokeDashArray_Remarks}</remarks>
        public DoubleCollection StrokeDashArray
        {
            get
            {
                return (base.GetValue(StrokeDashArrayProperty) as DoubleCollection);
            }
            set
            {
                base.SetValue(StrokeDashArrayProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeDashArrayProperty_D}</summary>
        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(LineStyle), null);

        /// <summary>${core_FillStyle_attribute_strokeDashCap_D}</summary>
        /// <remarks>${core_FillStyle_attribute_strokeDashCap_Remarks}</remarks>
        public PenLineCap StrokeDashCap
        {
            get
            {
                return (PenLineCap)base.GetValue(StrokeDashCapProperty);
            }
            set
            {
                base.SetValue(StrokeDashCapProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeDashCapProperty_D}</summary>
        public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(LineStyle), new PropertyMetadata(PenLineCap.Flat));
        /// <summary>${core_FillStyle_attribute_strokeDashOffset_D}</summary>
        /// <remarks>${core_FillStyle_attribute_strokeDashOffset_Remarks}</remarks>
        public double StrokeDashOffset
        {
            get
            {
                return (double)base.GetValue(StrokeDashOffsetProperty);
            }
            set
            {
                base.SetValue(StrokeDashOffsetProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeDashOffsetProperty_D}</summary>
        public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(LineStyle), null);
        /// <summary>${core_FillStyle_attribute_strokeEndLineCap_D}</summary>
        /// <remarks>${core_FillStyle_attribute_strokeEndLineCap_Remarks}</remarks>
        public PenLineCap StrokeEndLineCap
        {
            get
            {
                return (PenLineCap)base.GetValue(StrokeEndLineCapProperty);
            }
            set
            {
                base.SetValue(StrokeEndLineCapProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeEndLineCapProperty_D}</summary>
        public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(LineStyle), new PropertyMetadata(PenLineCap.Flat));
        /// <summary>${core_FillStyle_attribute_strokeLineJoin_D}</summary>
        /// <remarks>${core_FillStyle_attribute_strokeLineJoin_Remarks}</remarks>
        public PenLineJoin StrokeLineJoin
        {
            get
            {
                return (PenLineJoin)base.GetValue(StrokeLineJoinProperty);
            }
            set
            {
                base.SetValue(StrokeLineJoinProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeLineJoinProperty_D}</summary>
        public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(LineStyle), new PropertyMetadata(PenLineJoin.Bevel));
        /// <summary>${core_FillStyle_attribute_strokeStartLineCap_D}</summary>
        /// <remarks>${core_FillStyle_attribute_strokeStartLineCap_Remarks}</remarks>
        public PenLineCap StrokeStartLineCap
        {
            get
            {
                return (PenLineCap)base.GetValue(StrokeStartLineCapProperty);
            }
            set
            {
                base.SetValue(StrokeStartLineCapProperty, value);
            }
        }
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        /// <summary>${core_Style_Public_field_strokeStartLineCap_D}</summary>
        public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(LineStyle), new PropertyMetadata(PenLineCap.Flat));
    }
}
