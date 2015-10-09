using System.Threading;

namespace TradeDataMonitoring
{
    public interface ITimer
    {
        /// <summary>
        /// Changes the start time and the interval between method invocations for a timer, using 32-bit signed integers to measure time intervals.
        /// </summary>  name="period">The time interval between invocations of the callback method in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to disable periodic signaling. </param> 
        bool Change(int dueTime, int period);

        void Init(TimerCallback callback, object state, int dueTime, int period);
    }
}