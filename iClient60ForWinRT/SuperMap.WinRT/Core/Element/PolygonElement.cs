using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SuperMap.WinRT.Core
{
    /// <summary><para>${mapping_PolygonBase_Title}</para>${mapping_PolygonBase_Description}</summary>
    public class PolygonElement : ShapeElement
    {
        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="PolygonElement">PolygonBase</see>
        ///     ${pubilc_Constructors_instance}
        /// </summary>
        public PolygonElement()
            : base(new Polygon())
        {
        }

        /// <summary>${mapping_PolygonBase_attribute_FillRule_D}</summary>
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
        /// <summary>${mapping_PolygonBase_attribute_ScreenPoints_D}</summary>
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
