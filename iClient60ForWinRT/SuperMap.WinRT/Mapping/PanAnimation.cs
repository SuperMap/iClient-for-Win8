using System;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SuperMap.WinRT.Mapping
{
    internal class PanAnimation
    {
        private Map _map;
        private TimeSpan _panduration;
        private bool _isAnimating;
        private DateTime _start;

        private double _startX;
        private double _startY;
        private double _targetX;
        private double _targetY;
        private DateTime _lastRenderingTime;

        internal event EventHandler<PanAnimationEventArgs> PanAnimationCompleted;
        internal event EventHandler<PanAnimationEventArgs> Panning;
        internal event EventHandler PanStarted;

        public PanAnimation(Map element)
        {
            this._map = element;
        }

        internal void DeltaPan(int dX, int dY, TimeSpan duration)
        {
            Point offset = this._map.GetCurrentOffset();
            _startX = offset.X;
            _startY = offset.Y;
            _panduration = duration;
            if (!_isAnimating)
            {
                _targetX = _startX + dX;
                _targetY = _startY + dY;
                _isAnimating = true;
                _start = DateTime.Now;
                CompositionTarget.Rendering += new EventHandler<object>(CompositionTarget_Rendering);
                OnPanStarted();
            }
            else
            {
                _targetX += dX;
                _targetY += dY;
                _start = _lastRenderingTime;
            }
        }
        private void CompositionTarget_Rendering(object sender, object e)
        {
            if (_panduration.Ticks > 0)
            {
                _lastRenderingTime = DateTime.Now;
                double timeSpan = DateTime.Now.Ticks - this._start.Ticks;
                double normalizedTime = ((double)timeSpan) / ((double)this._panduration.Ticks);
                //double num = this._easingFunction.Ease(normalizedTime);

                if (normalizedTime >= 1.0 || (Math.Abs(_targetX - _startX) <= 1 && Math.Abs(_targetY - _startY) <= 1))
                {
                    this.StopLoop();
                    this.OnPanAnimationCompleted(new Point(_targetX, _targetY));
                }
                else
                {
                    double numX = (this._targetX - this._startX) * normalizedTime;
                    double numY = (this._targetY - this._startY) * normalizedTime;
                    Point offset = new Point(numX + this._startX, numY + this._startY);
                    this.OnPanning(offset);
                }
            }
            else
            {
                this.StopLoop();

                this.OnPanAnimationCompleted(new Point(_targetX, _targetY));
            }
        }

        internal void Cancel()
        {
            StopLoop();
            this._startX = 0;
            this._startY = 0;
            this._targetX = 0;
            this._targetY = 0;
        }

        private void StopLoop()
        {
            if (this._isAnimating)
            {
                CompositionTarget.Rendering -= new EventHandler<object>(CompositionTarget_Rendering);
                this._isAnimating = false;
            }
        }

        private void OnPanAnimationCompleted(Point offset)
        {
            if (this.PanAnimationCompleted != null)
            {
                this.PanAnimationCompleted(this, new PanAnimationEventArgs(offset));
            }
        }
        private void OnPanning(Point offset)
        {
            if (this.Panning != null)
            {
                this.Panning(this, new PanAnimationEventArgs(offset));
            }
        }
        private void OnPanStarted()
        {
            if (this.PanStarted != null)
            {
                this.PanStarted(this, new EventArgs());
            }
        }
    }

    internal class PanAnimationEventArgs : EventArgs
    {
        public Point Offset
        {
            get;
            private set;
        }

        public PanAnimationEventArgs(Point offset)
        {
            Offset = offset;
        }
    }
}
