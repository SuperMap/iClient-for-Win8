using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawPoint_Title}。</para>
    /// 	<para>${ui_action_DrawPoint_Description}</para>
    /// </summary>
    public class DrawPoint : MapAction
    {
        /// <summary>${ui_action_DrawPoint_event_drawCompleted_D}</summary>
        public event EventHandler<DrawEventArgs> DrawCompleted;

        /// <summary>${ui_action_DrawPoint_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// DrawPoint draw = new DrawPoint(MyMap,Cursors.Stylus)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_DrawPoint_constructor_Map_param_map}</param>
        public DrawPoint(Map map )
            : base(map , "DrawPoint")
        {
            
        }
        
        public override void OnTap(GestureEventArgs e)
        {
            Point2D point = Map.ScreenToMap(e.GetPosition(Map));
            DrawEventArgs args = new DrawEventArgs
            {
                DrawName = Name,
                Element = new Pushpin { Location = point },
                Geometry = new GeoPoint(point.X, point.Y)
            };
            OnDrawComplete(args);
            base.OnTap(e);
        }

        private void OnDrawComplete(DrawEventArgs args)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted(this , args);
            }
        }


        public override void Deactivate()
        {
            
        }
    }
}
