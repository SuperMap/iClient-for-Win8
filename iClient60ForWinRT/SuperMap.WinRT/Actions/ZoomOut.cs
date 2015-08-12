using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;

namespace SuperMap.WinRT.Actions
{
    /// <summary>
    /// 	<para>${ui_action_ZoomOut_Title}。</para>
    /// 	<para>${ui_action_ZoomOut_Description_sl}</para>
    /// </summary>
    /// <seealso cref="DrawRectangle">DrawRectangle Class</seealso>
    public class ZoomOut : DrawRectangle
    {
        /// <summary>${ui_action_ZoomOut_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// ZoomOut zoomMap = new ZoomOut(MyMap)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_ZoomOut_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public ZoomOut(Map map)
            : base(map)
        { }
        
        /// <summary>${ui_action_ZoomOut_event_OnPointerReleased_D}</summary>
        public override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Rectangle == null)
            {
                return;
            }
            Rectangle2D zoomBounds = (Rectangle2D)Rectangle.GetValue(ElementsLayer.BBoxProperty);
            if (Rectangle2D.IsNullOrEmpty(zoomBounds) || zoomBounds.Width == 0.0 || zoomBounds.Height == 0.0)
            {
                return;
            }
            Point2D center = zoomBounds.Center;
            Rectangle2D currentBounds = Map.ViewBounds;

            double whRatioCurrent = currentBounds.Width / currentBounds.Height;
            double whRatioZoomBox = zoomBounds.Width / zoomBounds.Height;
            Rectangle2D newBounds = Rectangle2D.Empty;
            if (whRatioZoomBox > whRatioCurrent)  // use width
            {
                double multiplier = currentBounds.Width / zoomBounds.Width;
                double newWidth = currentBounds.Width * multiplier;
                newBounds = new Rectangle2D(new Point2D(center.X - (newWidth / 2), center.Y - (newWidth / 2)),
                                               new Point2D(center.X + (newWidth / 2), center.Y + (newWidth / 2)));
            }
            else// use height
            {
                double multiplier = currentBounds.Height / zoomBounds.Height;
                double newHeight = currentBounds.Height * multiplier;
                newBounds = new Rectangle2D(new Point2D(center.X - (newHeight / 2), center.Y - (newHeight / 2)),
                                               new Point2D(center.X + (newHeight / 2), center.Y + (newHeight / 2)));
            }

            if (!Rectangle2D.IsNullOrEmpty(newBounds))
            {
                Map.ZoomTo(newBounds);
            }
            base.OnPointerReleased(e);
        }
    }
}
