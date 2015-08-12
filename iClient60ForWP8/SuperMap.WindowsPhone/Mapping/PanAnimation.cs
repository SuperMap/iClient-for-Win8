using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SuperMap.WindowsPhone.Mapping
{
    internal class PanAnimation
    {
        private FrameworkElement element;
        private TimeSpan panduration;
        private bool isAnimating;
        private DateTime? start;
        private EasingFunctionBase easingFunction;

        private double startX;
        private double startY;
        private double targetX;
        private double targetY;

        internal event EventHandler PanAnimationCompleted;
        internal event EventHandler Panning;

        public PanAnimation(FrameworkElement element)
        {
            this.element = element;
            this.element.RenderTransform = new TranslateTransform();
            this.easingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut };
        }

        internal void DeltaPan(int dX, int dY, TimeSpan duration,bool add)
        {
            if (duration.Ticks > 0L)
            {
                this.panduration = duration;
                if ((dX != 0) || (dY != 0))
                {
                    this.StartLoop();
                    this.start = new DateTime?(DateTime.Now.AddMilliseconds(-33.0));
                    TranslateTransform renderTransform = this.element.RenderTransform as TranslateTransform;
                    this.startX = renderTransform.X;
                    this.startY = renderTransform.Y;
                    if (add)
                    {
                        this.targetX += dX;
                        this.targetY += dY;
                    }
                    else
                    {
                        this.targetX = renderTransform.X + dX;
                        this.targetY = renderTransform.Y + dY;
                    }
                }
            }
            else
            {
                TranslateTransform transform = this.element.RenderTransform as TranslateTransform;
                this.targetX = transform.X += dX;
                this.targetY = transform.Y += dY;
                this.OnPanning();
            }
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double normalizedTime = ((double)(DateTime.Now.Ticks - this.start.Value.Ticks)) / ((double)this.panduration.Ticks);
            TranslateTransform renderTransform = this.element.RenderTransform as TranslateTransform;
            double num = this.easingFunction.Ease(normalizedTime);
            double numX = (this.targetX - this.startX) * num;
            double numY = (this.targetY - this.startY) * num;
            renderTransform.X = numX + this.startX;
            renderTransform.Y = numY + this.startY;
            if ((normalizedTime >= 1.0) || ((Math.Abs((double)(renderTransform.X - this.targetX)) < 0.1) && (Math.Abs((double)(renderTransform.Y - this.targetY)) < 0.1)))
            {
                this.StopLoop();
                renderTransform.X = this.targetX;
                renderTransform.Y = this.targetY;
                this.OnPanAnimationCompleted();
            }
            else
            {
                this.OnPanning();
            }
        }

        internal Point GetCurrentOffset()
        {
            TranslateTransform renderTransform = this.element.RenderTransform as TranslateTransform;
            return new Point(-renderTransform.X, -renderTransform.Y);
        }

        internal void ResetTranslate()
        {
            this.StopLoop();
            TranslateTransform renderTransform = this.element.RenderTransform as TranslateTransform;
            renderTransform.X = 0.0;
            renderTransform.Y = 0.0;
            this.targetX = 0.0;
            this.targetY = 0.0;
        }

        internal void Stop(bool stopTimer)
        {
            if (this.isAnimating)
            {
                this.StopLoop();
                if (!stopTimer)
                {
                    this.OnPanAnimationCompleted();
                }
            }
        }
        private void StartLoop()
        {
            if (!this.isAnimating)
            {
                this.isAnimating = true;
                CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            }
        }
        private void StopLoop()
        {
            if (this.isAnimating)
            {
                CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);
                this.isAnimating = false;
            }
        }
        private void OnPanAnimationCompleted()
        {
            if (this.PanAnimationCompleted != null)
            {
                this.PanAnimationCompleted(this, new EventArgs());
            }
        }
        private void OnPanning()
        {
            if (this.Panning != null)
            {
                this.Panning(this, new EventArgs());
            }
        }
    }
}
