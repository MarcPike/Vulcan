using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.DocClass.QueueSchedule
{
    public class EventFinder
    {
        public async Task<ScheduledEvent> GetNextEventAsync(ObjectId queueId, CancellationTokenSource cancellationToken)
        {

            ScheduledEvent result = null;
            await Task.Run(() =>
            {
                while (result == null)
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    
                    var now = DateTime.Now;
                    var rep = new RepositoryBase<QueueSchedule>();
                    var queue = rep.AsQueryable().Single(x => x.Id == queueId);

                    result = queue.Events.Where(x => x.Status == ScheduledEventWorkStatus.Pending &&
                                               x.ExecuteOn <= now)
                        .OrderByDescending(x => x.AddedOn)
                        .FirstOrDefault();

                    if (result == null)
                    {
                        Thread.Sleep(1000);
                        if (cancellationToken.IsCancellationRequested) break;

                    }
                }
            });
            if (cancellationToken.IsCancellationRequested) return null;

            return result;
        }

    }
}
