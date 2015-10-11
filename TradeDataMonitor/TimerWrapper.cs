using System;
using System.Threading;

namespace TradeDataMonitoring
{
    /// <summary>
    /// Wrapper around System.Threading.Timer
    /// </summary>
    public class TimerWrapper : ITimer
    {
        private Timer _timer;

        /// <summary>
        /// Initializes a new instance of the Timer class, using a 32-bit signed integer to specify the time interval.
        /// </summary>
        /// <param name="callback">A <see cref="T:System.Threading.TimerCallback"/> delegate representing a method to be executed. </param><param name="state">An object containing information to be used by the callback method, or null. </param><param name="dueTime">The amount of time to delay before <paramref name="callback"/> is invoked, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to prevent the timer from starting. Specify zero (0) to start the timer immediately. </param><param name="period">The time interval between invocations of <paramref name="callback"/>, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to disable periodic signaling. </param><exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime"/> or <paramref name="period"/> parameter is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite"/>. </exception><exception cref="T:System.ArgumentNullException">The <paramref name="callback"/> parameter is null. </exception>
        public void Init(TimerCallback callback, object state, int dueTime, int period)
        {
            _timer = new Timer(callback, state, dueTime, period);
        }

        /// <summary>
        /// Changes the start time and the interval between method invocations for a timer, using 32-bit signed integers to measure time intervals.
        /// </summary>
        /// 
        /// <returns>
        /// true if the timer was successfully updated; otherwise, false.
        /// </returns>
        /// <param name="dueTime">The amount of time to delay before the invoking the callback method specified when the <see cref="T:System.Threading.Timer"/> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to prevent the timer from restarting. Specify zero (0) to restart the timer immediately. </param><param name="period">The time interval between invocations of the callback method specified when the <see cref="T:System.Threading.Timer"/> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to disable periodic signaling. </param><exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Timer"/> has already been disposed. </exception><exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime"/> or <paramref name="period"/> parameter is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite"/>. </exception><filterpriority>2</filterpriority>
        public bool Change(int dueTime, int period)
        {
            if (_timer == null)
            {
                throw new InvalidOperationException("Must call Init first!");
            }
            return _timer.Change(dueTime, period);
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}