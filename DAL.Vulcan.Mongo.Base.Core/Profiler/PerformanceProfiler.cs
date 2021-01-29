using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DAL.Vulcan.Mongo.Base.Core.Profiler
{
    public class PerformanceProfiler
    {
        private Stopwatch _watch = new Stopwatch();
        private List<ActivityProcess> _processes { get; set; } = new List<ActivityProcess>();

        public void Clear()
        {
            _processes.Clear();
        }

        public void Start(string activityName)
        {
            _processes.Add(new ActivityProcess(activityName));
        }

        public void Stop(string activityName)
        {
            var activity = _processes.LastOrDefault(x => x.Name == activityName && x.IsActive);
            activity?.Stop();
        }

        public List<ActivityReport> GetReport()
        {
            var result = new List<ActivityReport>();

            foreach (var activityProcess in _processes)
            {
                if (activityProcess.IsActive) activityProcess.Stop();

                var reportItem = result.FirstOrDefault(x => x.ActivityName == activityProcess.Name);
                if (reportItem == null)
                {
                    reportItem = new ActivityReport() {ActivityName = activityProcess.Name};
                    result.Add(reportItem);
                }
                reportItem.TotalDuration += activityProcess.Duration;

            }

            return result;
        }
        
    }

    public class ActivityReport
    {
        public string ActivityName { get; set; }
        public TimeSpan TotalDuration { get; set; }

    }

    public class ActivityProcess
    {
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
        private Stopwatch _stopwatch { get; set; }

        public TimeSpan Duration => _stopwatch.Elapsed;

        public ActivityProcess(string activityName)
        {
            Name = activityName;
            IsActive = true;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            IsActive = false;
        }

    }
}
