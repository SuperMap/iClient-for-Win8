using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;

namespace SuperMap.WinRT.Actions
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
        /// ZoomIn zoomInMap = new ZoomIn(MyMap)
        /// </code>
        /// </example>
        /// <overloads>${ui_action_ZoomIn_constructor_overloads}</overloads>
        /// <param name="map">${ui_action_ZoomIn_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public ZoomIn(Map map)
            : base(map)
        {
        }

        /// <summary>${ui_action_ZoomIn_event_OnPointerReleased_D}</summary>
        public override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Rectangle == null)
            {
                return;
            }
            Rectangle2D zoomBounds = (Rectangle2D)Rectangle.GetValue(ElementsLayer.BBoxProperty);
            Map.ZoomTo(zoomBounds);
            base.OnPointerReleased(e);
        }
    }
}
