using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// ${mapping_PolylineBase_Title}<br/>
    /// ${mapping_PolylineBase_Description}
    /// </summary>
    public class PolylineElement : ShapeElement
    {
        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="PolylineElement">PolylineBase</see>
        ///     ${pubilc_Constructors_instance}
        /// </summary>
        public PolylineElement()
            : base(new Polyline())
        {
        }

        /// <summary>${mapping_PolygonBase_attribute_FillRule_D}</summary>
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
        /// <summary>${mapping_PolygonBase_attribute_ScreenPoints_D}</summary>
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
