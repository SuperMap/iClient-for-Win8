using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_FillStyle_Title}</para>
    /// 	<para>${WP_core_FillStyle_Description}</para>
    /// </summary>
    [KnownType(typeof(PredefinedFillStyle)), KnownType(typeof(PictureFillStyle))]
    public class FillStyle : Style
    {
        /// <summary>${WP_core_FillStyle_constructor_None_D}</summary>
        public FillStyle()
        {
            ControlTemplate = ResourceData.Dictionary["FillStyle"] as ControlTemplate;
        }

        /// <summary>${WP_core_FillStyle_attribute_fill_D}</summary>
        /// <remarks>${WP_core_Style_Public_attribute_Remarks}</remarks>
        public Brush Fill
        {
            get
            {
                return (base.GetValue(FillProperty) as Brush);
            }
            set
            {
                base.SetValue(FillProperty, value);
            }
        }
        /// <summary>${WP_core_Style_Public_field_fillProperty_D}</summary>
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(FillStyle), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        /// <summary>${WP_core_FillStyle_attribute_stroke_D}</summary>
        /// <remarks>${WP_core_Style_Public_attribute_Remarks}</remarks>
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
        /// <summary>${WP_core_Style_Public_field_strokeProperty_D}</summary>
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(FillStyle), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>${WP_core_FillStyle_attribute_strokeThickness_D}</summary>
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
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_Style_Public_field_strokeThicknessProperty_D}</summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(FillStyle), new PropertyMetadata(1.0));

        /// <summary>${WP_core_FillStyle_attribute_strokeDashArray_D}</summary>
        /// <remarks>${WP_core_FillStyle_attribute_strokeDashArray_Remarks}</remarks>
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
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_Style_Public_field_strokeDashArrayProperty_D}</summary>
        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(FillStyle), null);

        /// <summary>${WP_core_FillStyle_attribute_strokeDashCap_D}</summary>
        /// <remarks>${WP_core_FillStyle_attribute_strokeDashCap_Remarks}</remarks>
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
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_Style_Public_field_strokeDashCapProperty_D}</summary>
        public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(FillStyle), new PropertyMetadata(PenLineCap.Flat));

        /// <summary>${WP_core_FillStyle_attribute_strokeDashOffset_D}</summary>
        /// <remarks>${WP_core_FillStyle_attribute_strokeDashOffset_Remarks}</remarks>
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
        public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(FillStyle), null);

        /// <summary>${WP_core_FillStyle_attribute_strokeEndLineCap_D}</summary>
        /// <remarks>${WP_core_FillStyle_attribute_strokeEndLineCap_Remarks}</remarks>
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
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_Style_Public_field_strokeEndLineCapProperty_D}</summary>
        public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(FillStyle), new PropertyMetadata(PenLineCap.Flat));

        /// <summary>${WP_core_FillStyle_attribute_strokeLineJoin_D}</summary>
        /// <remarks>${WP_core_FillStyle_attribute_strokeLineJoin_Remarks}</remarks>
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
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_Style_Public_field_strokeLineJoinProperty_D}</summary>
        public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(FillStyle), new PropertyMetadata(PenLineJoin.Bevel));

        /// <summary>${WP_core_FillStyle_attribute_strokeStartLineCap_D}</summary>
        /// <remarks>${WP_core_FillStyle_attribute_strokeStartLineCap_Remarks}</remarks>
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
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        /// <summary>${WP_core_Style_Public_field_strokeStartLineCap_D}</summary>
        public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(FillStyle), new PropertyMetadata(PenLineCap.Flat));
    }
}
