using System.Windows.Media;

namespace SuperMap.WindowsPhone.Actions
{
    //Silverlight Shape的10个成员 + Opacity 
    /// <summary>
    /// 	<para>${ui_action_IDrawStyle_Title}。</para>
    /// 	<para>${ui_action_IDrawStyle_Description}</para>
    /// </summary>
    public interface IDrawStyle
    {
        /// <summary>${ui_action_IDrawStyle_attribute_fill_D}</summary>
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_fill_example_D}">
        /// Fill = new SolidColorBrush(Colors.Orange)
        /// </code>
        /// </example>
        Brush Fill { get; set; }
        /// <summary>${ui_action_IDrawStyle_attribute_stroke_D}</summary>
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_stroke_example_D}">
        /// Stroke = new SolidColorBrush(Colors.Green)
        /// </code>
        /// </example>
        Brush Stroke { get; set; }
        /// <summary>${ui_action_IDrawStyle_attribute_strokeThickness_D}</summary>
        double StrokeThickness { get; set; }

        /// <summary>${ui_action_IDrawStyle_attribute_strokeMiterLimit_D}</summary>
        double StrokeMiterLimit { get; set; }
        /// <summary>${ui_action_IDrawStyle_attribute_strokeDashOffset_D}</summary>
        double StrokeDashOffset { get; set; }
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_strokeDashArray_example_D}">
        /// StrokeDashArray=new DoubleCollection(){1,2}
        /// </code>
        /// </example>
        /// <summary>${ui_action_IDrawStyle_attribute_strokeDashArray_D}</summary>
        DoubleCollection StrokeDashArray { get; set; }

        /// <summary>${ui_action_IDrawStyle_attribute_strokeDashCap_D}</summary>
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_strokeDashCap_example_D}">
        /// StrokeDashCap = PenLineCap.Flat
        /// </code>
        /// </example>
        PenLineCap StrokeDashCap { get; set; }
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_strokeDashCap_example_D}">
        /// StrokeEndLineCap = PenLineCap.Square
        /// </code>
        /// </example>
        /// <summary>${ui_action_IDrawStyle_attribute_strokeEndLineCap_D}</summary>
        PenLineCap StrokeEndLineCap { get; set; }
        /// <summary>${ui_action_IDrawStyle_attribute_strokeStartLineCap_D}</summary>
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_strokeStartLineCap_example_D}">
        /// StrokeStartLineCap = PenLineCap.Round
        /// </code>
        /// </example>
        PenLineCap StrokeStartLineCap { get; set; }

        /// <summary>${ui_action_IDrawStyle_attribute_StrokeLineJoin_D}</summary>
        /// <example>
        /// 	<code lang="CS" description="${ui_action_IDrawStyle_attribute_strokeStartLineCap_example_D}">
        /// StrokeLineJoin = PenLineJoin.Miter
        /// </code>
        /// </example>
        PenLineJoin StrokeLineJoin { get; set; }

        //FillRule FillRule { get; set; } //放到线和面中，和SL的组织方式一致，矩形就没这个
        /// <example>
        /// 	<code lang="CS">
        /// 	</code>
        /// </example>
        /// <summary>${ui_action_IDrawStyle_attribute_Opacity_D}</summary>
        double Opacity { get; set; }
    }
}
