using System.Windows.Input;
using SuperMap.WindowsPhone.Mapping;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_MapAction_Title}。</para>
    /// 	<para>${ui_action_MapAction_Description}</para>
    /// </summary>
    public abstract class MapAction
    {
        /// <summary>${ui_action_MapAction_constructor_None_D}</summary>
        /// <overloads>${ui_action_MapAction_constructor_overloads}</overloads>
        protected MapAction()
        { }

        /// <summary>${ui_action_MapAction_constructor_Map_D}</summary>
        /// <param name="map">${ui_action_MapAction_constructor_Map_param_map}</param>
        /// <param name="name">${ui_action_MapAction_constructor_Map_param_name}</param>
        protected MapAction(Map map, string name)
        {
            Name = name;
            Map = map;
            Map.Focus();
        }

        /// <summary>
        /// 在 DoubleTap 事件发生之前调用。
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnDoubleTap(GestureEventArgs e) { }

        /// <summary>
        /// 在 Tap 事件发生之前调用。
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTap(GestureEventArgs e) { }
        
        /// <summary>
        /// 在 Hold 事件发生之前调用。
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnHold(GestureEventArgs e) { }

        public virtual void OnManipulationStarted(ManipulationStartedEventArgs e) { }

        public virtual void OnManipulationDelta(ManipulationDeltaEventArgs e) { }

        public virtual void OnManipulationCompleted(ManipulationCompletedEventArgs e) { }

        /// <summary>${ui_action_MapAction_event_onMouseDown_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseDown_param_e}</param>
        public virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //Map.Focus();//需要重新获取焦点，否则键盘事件不响应
        }

        /// <summary>${ui_action_MapAction_event_onMouseMove_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseMove_param_e}</param>
        public virtual void OnMouseMove(MouseEventArgs e) { }

        /// <summary>${ui_action_MapAction_event_onMouseEnter_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseEnter_param_e}</param>
        public virtual void OnMouseEnter(MouseEventArgs e) { }

        /// <summary>${ui_action_MapAction_event_onMouseLeave_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseLeave_param_e}</param>
        public virtual void OnMouseLeave(MouseEventArgs e) { }

        /// <summary>${ui_action_MapAction_event_onMouseUp_D}</summary>
        /// <param name="e">${ui_action_MapAction_event_onMouseUp_param_e}</param>
        public virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e) { }

        //重要,每个子类必需覆盖重写
        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public abstract void Deactivate();

        /// <summary>${ui_action_MapAction_attribute_name_D}</summary>
        public string Name { get; set; }
        /// <summary>${ui_action_MapAction_attribute_map_D}</summary>
        public Map Map { get; protected set; }
    }
}
