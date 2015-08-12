using System;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.System;

namespace SuperMap.WinRT.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawPoint_Title}。</para>
    /// 	<para>${ui_action_DrawPoint_Description}</para>
    /// </summary>
    public class DrawPoint : Pan
    {
        /// <summary>${ui_action_DrawPoint_event_drawCompleted_D}</summary>
        public event EventHandler<DrawEventArgs> DrawCompleted;

        /// <summary>${ui_action_DrawPoint_constructor_Map_D}</summary>
        /// <example>
        /// <code>
        /// DrawPoint draw = new DrawPoint(MyMap);
        /// MyMap.Action=draw;
        /// draw.DrawCompleted += drawPoint_DrawCompleted;
        /// void drawPoint_DrawCompleted(object sender, DrawEventArgs e)
        /// {
        ///     //TODO
        /// }
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawPoint_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public DrawPoint(Map map)
            : base(map)
        {
            this.Name = "DrawPoint";
        }
        
        /// <summary>
        /// ${ui_action_DrawPoint_event_OnTapped_D}
        /// </summary>
        public override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            Point2D center = Map.ScreenToMap(e.GetPosition(Map));
            DrawEventArgs args = new DrawEventArgs
            {
                DrawName = Name,
                Element = new Pushpin { Location = center },
                Geometry = new GeoPoint(center.X, center.Y)
            };
            OnDrawComplete(args);

        }

        private void OnDrawComplete(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this , args);
            }
        }

    }
}
