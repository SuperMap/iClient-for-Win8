//using System;
//using System.Collections.Generic;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Threading;

//namespace SuperMap.WindowsPhone.Utilities
//{
//    /// <summary>
//    /// 	<para>${utility_DoubleClickHelper_Title}</para>
//    /// 	<para>${utility_DoubleClickHelper_Description}</para>
//    /// </summary>
//    public static class DoubleClickHelper
//    {
//        private const int doubleClickInterval = 222;
//        private static readonly DependencyProperty DoubleClickTimerProperty = DependencyProperty.RegisterAttached("DoubleClickTimer", typeof(DispatcherTimer), typeof(UIElement), null);
//        private static readonly DependencyProperty DoubleClickHandlersProperty = DependencyProperty.RegisterAttached("DoubleClickHandlers", typeof(List<MouseButtonEventHandler>), typeof(UIElement), null);
//        private static readonly DependencyProperty DoubleClickPositionProperty = DependencyProperty.RegisterAttached("DoubleClickPosition", typeof(Point), typeof(UIElement), null);
//        /// <summary>${utility_DoubleClickHelper_method_AddDoubleClick_D}</summary>
//        /// <param name="element">The Element to listen for double clicks on.</param>
//        /// <param name="handler">The handler.</param>
//        public static void AddDoubleClick(this UIElement element, MouseButtonEventHandler handler)
//        {
//            if (element != null)
//            {
//                element.MouseLeftButtonDown += element_MouseLeftButtonDown;
//                List<MouseButtonEventHandler> handlers;
//                handlers = element.GetValue(DoubleClickHandlersProperty) as List<MouseButtonEventHandler>;
//                if (handlers == null)
//                {
//                    handlers = new List<MouseButtonEventHandler>();
//                    element.SetValue(DoubleClickHandlersProperty, handlers);
//                }
//                handlers.Add(handler);
//            }
//        }
//        /// <summary>${utility_DoubleClickHelper_method_RemoveDoubleClick_D}</summary>
//        /// <param name="element">The element.</param>
//        /// <param name="handler">The handler.</param>
//        public static void RemoveDoubleClick(this UIElement element, MouseButtonEventHandler handler)
//        {
//            if (element != null)
//            {
//                element.MouseLeftButtonDown -= element_MouseLeftButtonDown;
//                List<MouseButtonEventHandler> handlers = element.GetValue(DoubleClickHandlersProperty) as List<MouseButtonEventHandler>;
//                if (handlers != null)
//                {
//                    handlers.Remove(handler);
//                    if (handlers.Count == 0)
//                        element.ClearValue(DoubleClickHandlersProperty);
//                }
//            }
//        }
//        private static void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//        {
//            UIElement element = sender as UIElement;
//            Point position = e.GetPosition(element);
//            DispatcherTimer timer = element.GetValue(DoubleClickTimerProperty) as DispatcherTimer;
//            if (timer != null)
//            {
//                timer.Stop();
//                Point oldPosition = (Point)element.GetValue(DoubleClickPositionProperty);
//                element.ClearValue(DoubleClickTimerProperty);
//                element.ClearValue(DoubleClickPositionProperty);
//                if (Math.Abs(oldPosition.X - position.X) < 1 && Math.Abs(oldPosition.Y - position.Y) < 1) //mouse didn't move => Valid double click
//                {
//                    List<MouseButtonEventHandler> handlers = element.GetValue(DoubleClickHandlersProperty) as List<MouseButtonEventHandler>;
//                    if (handlers != null)
//                    {
//                        foreach (MouseButtonEventHandler handler in handlers)
//                        {
//                            handler(sender, e);
//                        }
//                    }
//                    return;
//                }
//            }
//            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(doubleClickInterval) };
//            timer.Tick += new EventHandler((s, args) =>
//            { 
//                (s as DispatcherTimer).Stop();
//                element.ClearValue(DoubleClickTimerProperty); 
//                element.ClearValue(DoubleClickPositionProperty);
//            });
//            element.SetValue(DoubleClickTimerProperty, timer);
//            element.SetValue(DoubleClickPositionProperty, position);
//            timer.Start();
//        }
//    }
//}
