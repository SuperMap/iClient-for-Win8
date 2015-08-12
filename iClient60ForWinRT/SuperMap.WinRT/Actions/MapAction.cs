using System.Windows.Input;
using SuperMap.WinRT.Mapping;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SuperMap.WinRT.Actions
{
    /// <summary>
    /// 	<para>${ui_action_MapAction_Title}。</para>
    /// 	<para>${ui_action_MapAction_Description}</para>
    /// </summary>
    public abstract class MapAction
    {
        /// <summary>${ui_action_MapAction_constructor_None_D}</summary>
        /// <overloads>${ui_action_MapAction_constructor_overloads}</overloads>
        public MapAction()
        { }

        /// <summary>${ui_action_MapAction_constructor_Map_D}</summary>
        /// <param name="map">${ui_action_MapAction_constructor_Map_param_map}</param>
        /// <param name="name">${ui_action_MapAction_constructor_Map_param_name}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public MapAction(Map map, string name)
        {
            Name = name;
            Map = map;
        }

        /// <summary>
        /// ${ui_action_MapAction_event_OnKeyDown_D}
        /// </summary>
        public virtual void OnKeyDown(KeyRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnKeyUp_D}
        /// </summary>
        public virtual void OnKeyUp(KeyRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerWheelChanged_D}
        /// </summary>
        public virtual void OnPointerWheelChanged(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnDoubleTapped_D}
        /// </summary>
        public virtual void OnDoubleTapped(DoubleTappedRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerPressed_D}
        /// </summary>
        public virtual void OnPointerPressed(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnTapped_D}
        /// </summary>
        public virtual void OnTapped(TappedRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerMoved_D}
        /// </summary>
        public virtual void OnPointerMoved(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerReleased_D}
        /// </summary>
        public virtual void OnPointerReleased(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerCaptureLost_D}
        /// </summary>
        public virtual void OnPointerCaptureLost(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerEntered_D}
        /// </summary>
        public virtual void OnPointerEntered(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerExited_D}
        /// </summary>
        public virtual void OnPointerExited(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnPointerCanceled_D}
        /// </summary>
        public virtual void OnPointerCanceled(PointerRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnHolding_D}
        /// </summary>
        public virtual void OnHolding(HoldingRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnManipulationCompleted_D}
        /// </summary>
        public virtual void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnManipulationDelta_D}
        /// </summary>
        public virtual void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnManipulationInertiaStarting_D}
        /// </summary>
        public virtual void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnManipulationStarted_D}
        /// </summary>
        public virtual void OnManipulationStarted(ManipulationStartedRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnManipulationStarting_D}
        /// </summary>
        public virtual void OnManipulationStarting(ManipulationStartingRoutedEventArgs e) { }
        /// <summary>
        /// ${ui_action_MapAction_event_OnRightTapped_D}
        /// </summary>
        public virtual void OnRightTapped(RightTappedRoutedEventArgs e) { }

        //重要,每个子类必需覆盖重写
        /// <summary>${ui_action_MapAction_method_deactivate_D}</summary>
        public abstract void Deactivate();

        internal virtual void Dispose()
        {

        }

        /// <summary>${ui_action_MapAction_attribute_name_D}</summary>
        public string Name { get; set; }
        /// <summary>${ui_action_MapAction_attribute_map_D}</summary>
        public Map Map { get; protected set; }
    }
}
