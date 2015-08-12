using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Utilities;
using SuperMap.WinRT.Mapping;
using Windows.UI.Input;

namespace SuperMap.WinRT.Actions
{
    internal class OvPan : MapAction
    {
        private bool isMouseCaptured;
        private double oldMouseX;
        private double oldMouseY;

        private Map mapBig;

        public OvPan(Map map, Map mapBig)
            : base(map, "OvPan")
        {
            this.mapBig = mapBig;
        }

        public override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint(Map);
            oldMouseX = point.Position.X;
            oldMouseY = point.Position.Y;
            isMouseCaptured = true;
            Map.CapturePointer(e.Pointer);
            e.Handled = true;
            base.OnPointerPressed(e);
        }

        public override void OnPointerMoved(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (isMouseCaptured)
            {
                PointerPoint point = e.GetCurrentPoint(Map);
                double newMouseX = point.Position.X;
                double newMouseY = point.Position.Y;
                double deltaX;
                double deltaY;
                deltaX = newMouseX - oldMouseX;
                deltaY = newMouseY - oldMouseY;

                Map.PanByPixel(-deltaX, -deltaY);

                oldMouseY = point.Position.Y;
                oldMouseX = point.Position.X;
            }
            base.OnPointerMoved(e);
        }

        public override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isMouseCaptured = false;
            Map.ReleasePointerCaptures();
            oldMouseY = -1;
            oldMouseX = -1;

            mapBig.PanTo(Map.ViewBounds.Center);

            e.Handled = true;
            base.OnPointerReleased(e);
        }

        public override void Deactivate()
        {

        }

    }
}