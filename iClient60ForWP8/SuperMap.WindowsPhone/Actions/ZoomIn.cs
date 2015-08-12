using System.Windows.Input;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_ZoomIn_Title}。</para>
    /// 	<para>${ui_action_ZoomIn_Description_sl}</para>
    /// </summary>
    /// <seealso cref="DrawRectangle">DrawRectangle Class</seealso>
    public class ZoomIn : DrawRectangle
    {
        /// <summary>${ui_action_ZoomIn_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// ZoomIn zoomInMap = new ZoomIn(MyMap,Cursors.Arrow)
        /// </code>
        /// </example>
        /// <overloads>${ui_action_ZoomIn_constructor_overloads}</overloads>
        /// <param name="map">${ui_action_ZoomIn_constructor_Map_param_map}</param>
        public ZoomIn(Map map)
            : base(map)
        {
        }

        /// <summary>${ui_action_MapAction_event_onMouseDown_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseDown_param_e}</param>
        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>${ui_action_MapAction_event_onMouseMove_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseMove_param_e}</param>
        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <summary>${ui_action_MapAction_event_onMouseUp_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseUp_param_e}</param>
        public override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (Rectangle == null)
            {
                return;
            }
            Rectangle2D zoomBounds = (Rectangle2D)Rectangle.GetValue(ElementsLayer.BBoxProperty);
            Map.ZoomTo(zoomBounds);
            base.OnMouseLeftButtonUp(e);
        }

    }
}
