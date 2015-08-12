using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Utilities;
using Windows.UI.Input;
using Windows.Devices.Input;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml.Input;
using SuperMap.WinRT.Core;
using System;
using Windows.System;

namespace SuperMap.WinRT.Actions
{
    //在判断是否为多点缩放是否完成时，只能通过计数的方式来判断，当点数从多个变成一个时，认为多点缩放完成。
    //不能依赖OnManipulationCompleted的理由是：
    //1.正常情况下OnManipulationCompleted只能在所有手指离开后才触发；
    //2.当触摸点数超过设备支持的最大上限时，OnManipulationCompleted触发不可控。例如开发用的显示器只支持两点触摸。
    /// <summary>
    /// 	<para>${ui_action_Pan_Title}。</para>
    /// 	<para>${ui_action_Pan_Description}</para>
    /// </summary>
    public class Pan : MapAction
    {
        static bool _isMultiple;
        static bool _isPanning;

        static ObservableCollection<uint> _devices;
        private DateTime _mouseWheel;

        /// <summary>${ui_action_Pan_constructor_Map_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// Pan panTo = new Pan(MyMap)
        /// </code>
        /// </example>
        /// <param name="map">${ui_action_Pan_constructor_Map_param_map}</param>
        /// <param name="cursor">${ui_action_MapAction_constructor_Map_param_cursor}</param>
        public Pan(Map map)
            : base(map, "Pan")
        {
            if (_devices == null)
            {
                _devices = new ObservableCollection<uint>();
                _devices.CollectionChanged += _devices_CollectionChanged;
            }
        }

        /// <summary>
        /// 计算出接触点的变化，由于状态的切换会有重复，因此需要做多个if判断
        /// 在此只能判断是否需要结束，无法判断开始。因为可能出现接触点没有移动的情况，在这种情况下，是不能开始的。
        /// 比如单击，双击，有接触点变化，但是没有平移，因此不能作为开始平移和结束平移的依据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _devices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int nowCount = _devices.Count;
            int oldCount = nowCount;
            if (nowCount > 1)
            {
                _isPanning = false;
                _isMultiple = true;
            }
            else if (nowCount == 1)
            {
                _isPanning = true;
                _isMultiple = false;
            }
            else
            {
                _isMultiple = false;
                _isPanning = false;
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    oldCount -= e.NewItems.Count;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    oldCount += e.OldItems.Count;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    oldCount += e.OldItems.Count;
                    break;
            }
            if (nowCount <= 1 && oldCount > 1)
            {
                EndManipulation();
            }
            if (oldCount == 1 && nowCount != 1)
            {
                EndPan();
            }
        }

        private void StartedPan()
        {
            Map.ManipulationPan(0, 0, MapStatus.PanStarted);
        }

        /// <summary>
        /// 只有在Panning的状态下，才能认为会结束。因为单击和双击也会触发这个方法。
        /// </summary>
        private void EndPan()
        {
            if (Map.MapStatus == MapStatus.Panning)
            {
                Map.ManipulationPan(0, 0, MapStatus.PanCompleted);
            }
        }

        private void StartManipulation()
        {
            Map.ManipulationZoom(Map.Resolution, Map.Center, MapStatus.ZoomStarted);
        }

        /// <summary>
        /// 只有在Zooming的状态下，才能认为会结束。
        /// </summary>
        private void EndManipulation()
        {
            if (Map.MapStatus == MapStatus.Zooming)
            {
                Map.ManipulationZoom(Map.Resolution, Map.Center, MapStatus.ZoomCompleted);
            }
        }

        /// <summary>
        /// ${ui_action_Pan_event_OnDoubleTapped_D}
        /// </summary>
        public override void OnDoubleTapped(Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);
            Point curMousePos = e.GetPosition(Map);
            Map.ZoomToResolution(Map.GetNextResolution(!(KeyboardHelper.ShiftIsDown())), Map.ScreenToMap(curMousePos));

        }

        public override void OnPointerCanceled(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerCanceled(e);
            uint deviceId = e.Pointer.PointerId;
            if (_devices.Contains(deviceId))
            {
                _devices.Remove(deviceId);
            }
        }

        public override void OnPointerCaptureLost(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerCaptureLost(e);
            uint deviceId = e.Pointer.PointerId;
            if (_devices.Contains(deviceId))
            {
                _devices.Remove(deviceId);
            }
        }

        public override void OnPointerEntered(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            Map.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            PointerPoint point = e.GetCurrentPoint(Map);
            if (point.IsInContact && ((point.PointerDevice.PointerDeviceType == PointerDeviceType.Mouse && point.Properties.IsLeftButtonPressed)
                || point.PointerDevice.PointerDeviceType == PointerDeviceType.Pen || point.PointerDevice.PointerDeviceType == PointerDeviceType.Touch))
            {
                uint deviceId = point.PointerId;
                if (!_devices.Contains(deviceId))
                {
                    _devices.Add(deviceId);
                }
            }
        }

        public override void OnPointerExited(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            uint deviceId = e.Pointer.PointerId;
            if (_devices.Contains(deviceId))
            {
                _devices.Remove(deviceId);
            }
        }

        public override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            Map.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            uint deviceId = e.Pointer.PointerId;
            if (!_devices.Contains(deviceId))
            {
                _devices.Add(deviceId);
            }
        }

        public override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            e.Handled = true;
            uint deviceId = e.Pointer.PointerId;
            if (_devices.Contains(deviceId))
            {
                _devices.Remove(deviceId);
            }
        }

        /// <summary>
        /// ${ui_action_Pan_event_OnPointerWheelChanged_D}
        /// </summary>
        public override void OnPointerWheelChanged(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            PointerPoint curWheelPos = e.GetCurrentPoint(Map);

            DateTime time = DateTime.Now;
            if ((time - _mouseWheel) < new TimeSpan(0, 0, 0, 0, 300))
            {
                return;
            }
            _mouseWheel = time;
            Point2D pnt = Map.ScreenToMap(curWheelPos.Position);
            double newResolution = Map.GetNextResolution(curWheelPos.Properties.MouseWheelDelta > 0);
            Rectangle2D newBounds = new Rectangle2D(pnt.X - curWheelPos.Position.X * newResolution, pnt.Y - (Map.ActualHeight - curWheelPos.Position.Y) * newResolution, pnt.X + (Map.ActualWidth - curWheelPos.Position.X) * newResolution, pnt.Y + curWheelPos.Position.Y * newResolution);
            Map.ZoomTo(newBounds);
        }

        /// <summary>
        /// ${ui_action_Pan_event_OnManipulationDelta_D}
        /// </summary>
        public override void OnManipulationDelta(Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (_isPanning)
            {
                if (Map.MapStatus != MapStatus.Panning)
                {
                    StartedPan();
                }
                Map.ManipulationPan((int)e.Delta.Translation.X, (int)e.Delta.Translation.Y, MapStatus.Panning);
            }
            else if (_isMultiple)
            {
                if (Map.MapStatus != MapStatus.Zooming)
                {
                    StartManipulation();
                }
                double scale = e.Delta.Scale;
                double newResolution = Map.Resolution / scale;

                if (Map.Resolutions != null && Map.Resolutions.Length > 0)
                {
                    if (newResolution >= Map.MaxResolution)
                    {
                        newResolution = Map.MaxResolution;
                        scale = Map.MaxResolution / newResolution;
                    }
                    else if (newResolution <= Map.MinResolution)
                    {
                        newResolution = Map.MinResolution;
                        scale = Map.MinResolution / newResolution;
                    }
                }
                Point2D pinchCenter = Map.ScreenToMap(e.Position);
                double offsetX = e.Delta.Translation.X;
                double offsetY = e.Delta.Translation.Y;
                if (Math.Abs(offsetX) < 1 && Math.Abs(offsetY) < 1)
                {
                    offsetX = 0;
                    offsetY = 0;
                }
                Point2D futureCenter = new Point2D(pinchCenter.X - (e.Position.X + offsetX - Map.ActualWidth / 2) * newResolution, pinchCenter.Y + (e.Position.Y + offsetY - Map.ActualHeight / 2) * newResolution);
                Map.ManipulationZoom(newResolution, futureCenter, MapStatus.Zooming);
            }
        }

        /// <summary>
        /// ${ui_action_Pan_event_OnKeyDown_D}
        /// </summary>
        public override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);
            if ((((e.Key == VirtualKey.Down) || (e.Key == VirtualKey.Up)) || ((e.Key == VirtualKey.Left) || (e.Key == VirtualKey.Right))) || ((e.Key == VirtualKey.Subtract) || (e.Key == VirtualKey.Add)))
            {
                double delta1 = Map.PanFactor * Map.ViewBounds.Width;
                double delta2 = Map.PanFactor * Map.ViewBounds.Height;
                switch (e.Key)
                {
                    case VirtualKey.Left:
                        Map.Pan(-delta1, 0);
                        break;
                    case VirtualKey.Right:
                        Map.Pan(delta1, 0);
                        break;
                    case VirtualKey.Up:
                        Map.Pan(0, delta2);
                        break;
                    case VirtualKey.Down:
                        Map.Pan(0, -delta2);
                        break;
                    case VirtualKey.Add:
                        Map.ZoomIn();
                        break;
                    case VirtualKey.Subtract:
                        Map.ZoomOut();
                        break;
                }
            }
        }

        /// <summary>${ui_action_Pan_method_deactivate_D}</summary>
        public override void Deactivate()
        {

        }

        internal override void Dispose()
        {
            base.Dispose();
            _devices.CollectionChanged -= _devices_CollectionChanged;
            _devices = null;
        }
    }
}