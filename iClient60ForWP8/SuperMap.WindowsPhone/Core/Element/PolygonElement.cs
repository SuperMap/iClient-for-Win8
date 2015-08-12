using System.Windows.Media;
using System.Windows.Shapes;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary><para>${WP_mapping_PolygonBase_Title}</para>${mapping_PolygonBase_Description}</summary>
    public class PolygonElement : ShapeElement
    {
        /// <summary>
        ///     #${WP_pubilc_Constructors_Initializes} <see cref="PolygonElement">PolygonBase</see>
        ///     ${WP_pubilc_Constructors_instance}
        /// </summary>
        public PolygonElement()
            : base(new Polygon())
        {
        }

        /// <summary>${WP_mapping_PolygonBase_attribute_FillRule_D}</summary>
        public FillRule FillRule
        {
            get
            {
                return (FillRule)base.EncapsulatedShape.GetValue(Polygon.FillRuleProperty);
            }
            set
            {
                base.EncapsulatedShape.SetValue(Polygon.FillRuleProperty, value);
            }
        }
        /// <summary>${WP_mapping_PolygonBase_attribute_ScreenPoints_D}</summary>
        protected override PointCollection ScreenPoints
        {
            get
            {
                return ((Polygon)base.EncapsulatedShape).Points;
            }
            set
            {
                ((Polygon)base.EncapsulatedShape).Points = value;
            }
        }
    }
}
