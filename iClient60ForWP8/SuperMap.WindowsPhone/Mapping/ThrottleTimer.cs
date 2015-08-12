using System;
using System.Windows.Threading;

namespace SuperMap.WindowsPhone.Mapping
{
    internal class ThrottleTimer
    {
        private DispatcherTimer throttleTimer;

        internal ThrottleTimer(int milliseconds)
            : this(milliseconds, null)
        {
        }

        internal ThrottleTimer(int milliseconds, Action handler)
        {
            Action = handler;
            throttleTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds((double)milliseconds) };

            throttleTimer.Tick += delegate(object s, EventArgs e)
            {
                if (Action != null)
                {
                    Action.Invoke();
                }
                throttleTimer.Stop();
            };
        }

        public void Invoke()
        {
            throttleTimer.Stop();
            throttleTimer.Start();
        }

        internal void Cancel()
        {
            throttleTimer.Stop();
        }

        public Action Action { get; set; }
    }
}
