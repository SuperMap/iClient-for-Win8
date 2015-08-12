using SuperMap.WinRT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Mapping
{
    internal class ZoomAnimation
    {
        double _startResolution;
        double _targetResolution;
        Point2D _startCenter;
        Point2D _targetCenter;
        bool _isZooming;
        Map _map;
        TimeSpan _zoomDuration;
        DateTime _startTime;

        public event EventHandler ZoomStarted;
        public event EventHandler<ZoomAnimationEventArgs> Zooming;
        public event EventHandler<ZoomAnimationEventArgs> ZoomCompleted;

        public ZoomAnimation(Map map)
        {
            _map = map;
        }

        public void DeltaZoom(double targetResolution, Point2D targetCenter, TimeSpan zoomDuration)
        {
            _targetResolution = targetResolution;
            _targetCenter = targetCenter;
            _startTime = DateTime.Now;
            _startCenter = _map.Center;
            _startResolution = _map.Resolution;
            if (Point2D.IsNullOrEmpty(_startCenter))
            {
                _zoomDuration = new TimeSpan(0);
            }
            else
            {
                _zoomDuration = zoomDuration;
            }
            if (!_isZooming)
            {
                _isZooming = true;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                OnZoomStarted();
            }
        }

        public void Cancel()
        {
            if (_isZooming)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                _isZooming = false;
            }
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            if (_zoomDuration.Ticks > 0)
            {
                double progress = (DateTime.Now.Ticks - _startTime.Ticks) * 1.0 / _zoomDuration.Ticks;
                double targetResolution = 0;
                Point2D targetCenter = Point2D.Empty;
                if (progress >= 1)
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                    _isZooming = false;
                    OnZoomCompleted(_targetResolution, _targetCenter);
                }
                else
                {
                    targetResolution = (_targetResolution - _startResolution) * progress + _startResolution;
                    targetCenter.X = (_targetCenter.X - _startCenter.X) * progress + _startCenter.X;
                    targetCenter.Y = (_targetCenter.Y - _startCenter.Y) * progress + _startCenter.Y;
                    OnZooming(targetResolution, targetCenter);
                }
            }
            else
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                _isZooming = false;
                OnZoomCompleted(_targetResolution, _targetCenter);
            }

        }

        private void OnZoomStarted()
        {
            if (ZoomStarted != null)
            {
                ZoomStarted(this, new EventArgs());
            }
        }

        private void OnZooming(double nowResolution, Point2D nowCenter)
        {
            if (Zooming != null)
            {
                Zooming(this, new ZoomAnimationEventArgs(nowResolution, nowCenter));
            }
        }

        private void OnZoomCompleted(double nowResolution, Point2D nowCenter)
        {
            if (ZoomCompleted != null)
            {
                ZoomCompleted(this, new ZoomAnimationEventArgs(nowResolution, nowCenter));
            }
        }
    }

    internal class ZoomAnimationEventArgs : EventArgs
    {
        public double NowResolution
        {
            get;
            private set;
        }

        public Point2D NowCenter
        {
            get;
            private set;
        }

        public ZoomAnimationEventArgs(double nowResolution, Point2D nowCenter)
        {
            NowResolution = nowResolution;
            NowCenter = nowCenter;
        }
    }
}
