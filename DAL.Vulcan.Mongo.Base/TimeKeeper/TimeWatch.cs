using System;
using System.Diagnostics;

namespace DAL.Vulcan.Mongo.Base.TimeKeeper
{
    public class TimeWatch: IDisposable
    {
        private Stopwatch _stopwatch = new Stopwatch();

        public TimeWatchState State { get; set; } = TimeWatchState.Created;
        public string ActionDescription { get; set; }
        private TimeSpan _totalElapsedTime { get; set; } 
        public TimeSpan TotalElapsedTime => _totalElapsedTime;
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public TimeWatch(string actionDescription)
        {
            ActionDescription = actionDescription;
        }

        public void Start()
        {
            _stopwatch.Start();
            State = TimeWatchState.Running;
            _totalElapsedTime.Add(_stopwatch.Elapsed);
        }

        public void Stop()
        {
            _stopwatch.Stop();
            State = TimeWatchState.Stopped;
            _totalElapsedTime += _stopwatch.Elapsed;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
        }
    }
}