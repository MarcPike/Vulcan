using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Base.Core.TimeKeeper
{
    public class TimeKeeper: IDisposable
    {
        public List<TimeWatch> Watches = new List<TimeWatch>();
        public TimeKeeper()
        {

        }

        public void StartAction(string actionDescription)
        {
            var timeWatch = new TimeWatch(actionDescription);
            timeWatch.Start();
            Watches.Add(timeWatch);
        }

        public TimeSpan StopAction(string actionDescription)
        {
            var watcher = GetTimeWatch(actionDescription);

            return watcher.Elapsed;
        }

        public TimeSpan GetTotalElapsed(string actionDescription)
        {
            var watcher = GetTimeWatch(actionDescription);

            if (watcher.State == TimeWatchState.Running)
            {
                return watcher.TotalElapsedTime + watcher.Elapsed;
            }
            else
            {
                return watcher.TotalElapsedTime;
            }

        }

        private TimeWatch GetTimeWatch(string actionDescription)
        {
            var foundWatcher = Watches.SingleOrDefault(x => x.ActionDescription == actionDescription);
            if (foundWatcher == null) throw new Exception($"No watcher found == [{actionDescription}]");

            return foundWatcher;
        }

        public void Dispose()
        {
            foreach (var timeWatch in Watches)
            {
                timeWatch.Dispose();
            }
        }
    }
}
