using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_Pan_Title}。</para>
    /// 	<para>${ui_action_Pan_Description}</para>
    /// </summary>
    public class Pan : MapAction
    {
        private bool isMouseCaptured;
        private double oldMouseX;
        private double oldMouseY;
        
        /// <summary>${ui_action_Pan_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// Pan panTo = new Pan(MyMap,Cursors.Hand)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_Pan_constructor_Map_param_map}</param>
        public Pan(Map map)
            : base(map, "Pan")
        {
        }

        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public override void Deactivate()
        {
        }
    }
}