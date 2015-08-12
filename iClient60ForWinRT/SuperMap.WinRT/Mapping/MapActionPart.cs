using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Input;
using Windows.UI.Xaml.Media;
using Windows.System.Threading;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.Devices.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SuperMap.WinRT.Mapping
{
    //Map类太长，把涉及到键盘 鼠标 滚轮 等MapAction的部分移到这里来。
    public sealed partial class Map : Control, INotifyPropertyChanged
    {
        private bool isKeyDown;//键盘用
        private MapAction curAction;
        private MapAction oldAction;

        /// <summary>${mapping_Map_event_actionChanged_D}</summary>
        public event EventHandler<MapActionArgs> MapActionChanged;

        private void BuildMapAction()
        {
            Pan panAction = new Pan(this);
            curAction = panAction;//默认pan操作
            oldAction = panAction;
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
        {
            base.OnManipulationInertiaStarting(e);
            if (Action != null)
            {
                Action.OnManipulationInertiaStarting(e);
            }
        }

        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            base.OnManipulationStarted(e);
            
            if (Action != null)
            {
                Action.OnManipulationStarted(e);
            }
        }

        protected override void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
            base.OnManipulationStarting(e);
            if (Action != null)
            {
                Action.OnManipulationStarting(e);
            }
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);

            if (Action != null)
            {
                Action.OnManipulationDelta(e);
            }
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            if (Action != null)
            {
                Action.OnManipulationCompleted(e);
            }
        }

        protected override void OnRightTapped(RightTappedRoutedEventArgs e)
        {
            base.OnRightTapped(e);
            if (Action != null)
            {
                Action.OnRightTapped(e);
            }
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            if (this.Action != null)
            {
                Action.OnTapped(e);
            }
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            if (this.Action != null)
            {
                this.Action.OnKeyDown(e);
                
            }
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            base.OnKeyUp(e);
            if (this.Action != null)
            {
                this.Action.OnKeyUp(e);
            }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (this.Action != null)
            {
                Action.OnPointerPressed(e);
            }
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            if (this.Action != null)
            {
                Action.OnPointerMoved(e);
            }
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (this.Action != null)
            {
                Action.OnPointerReleased(e);
            }
        }

        protected override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
            if (this.Action != null)
            {
                Action.OnHolding(e);
            }
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (this.Action != null)
            {
                Action.OnPointerEntered(e);
            }
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            if (this.Action != null)
            {
                Action.OnPointerExited(e);
            }
        }

        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
            base.OnPointerCaptureLost(e);
            if (this.Action != null)
            {
                Action.OnPointerCaptureLost(e);
            }
        }

        protected override void OnPointerCanceled(PointerRoutedEventArgs e)
        {
            base.OnPointerCanceled(e);
            if (this.Action != null)
            {
                Action.OnPointerCanceled(e);
            }
        }

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            if (this.Action != null)
            {
                Action.OnPointerWheelChanged(e);
            }
        }

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);
            if (this.Action != null)
            {
                this.Action.OnDoubleTapped(e);
            }
        }

        private void OnMapActionChanged(MapActionArgs args)
        {
            if (this.MapActionChanged != null)
            {
                this.MapActionChanged(this, args);
            }
        }
        /// <summary>${mapping_Map_attribute_action_D}</summary>
        public MapAction Action
        {
            get { return this.curAction; }
            set
            {
                this.oldAction = this.curAction;
                if (this.oldAction != null)
                {
                    this.oldAction.Deactivate();
                }
                this.curAction = value;
                MapActionArgs args = new MapActionArgs(this.oldAction, this.curAction);
                this.OnMapActionChanged(args);
            }
        }

    }
}
