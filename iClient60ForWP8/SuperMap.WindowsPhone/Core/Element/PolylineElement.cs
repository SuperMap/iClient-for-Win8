using System.Windows.Media;
using System.Windows.Shapes;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// ${WP_mapping_PolylineBase_Title}<br/>
    /// ${WP_mapping_PolylineBase_Description}
    /// </summary>
    public class PolylineElement : ShapeElement
    {
        /// <summary>
        ///     #${WP_pubilc_Constructors_Initializes} <see cref="PolylineElement">PolylineBase</see>
        ///     ${WP_pubilc_Constructors_instance}
        /// </summary>
        public PolylineElement()
            : base(new Polyline())
        {
        }

        /// <summary>${WP_mapping_PolygonBase_attribute_FillRule_D}</summary>
        public FillRule FillRule
        {
            get
            {
                return (FillRule)base.EncapsulatedShape.GetValue(Polyline.FillRuleProperty);
            }
            set
            {
                base.EncapsulatedShape.SetValue(Polyline.FillRuleProperty, value);
            }
        }
        /// <summary>${WP_mapping_PolygonBase_attribute_ScreenPoints_D}</summary>
        protected override PointCollection ScreenPoints
        {
            get
            {
                return ((Polyline)base.EncapsulatedShape).Points;
            }
            set
            {
                ((Polyline)base.EncapsulatedShape).Points = value;
            }
        }
    }
}
