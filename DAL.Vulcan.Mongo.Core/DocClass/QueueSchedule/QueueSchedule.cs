using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.QueueSchedule
{
    public class QueueSchedule: BaseDocument
    {
        public static MongoRawQueryHelper<QueueSchedule> Helper = new MongoRawQueryHelper<QueueSchedule>();

        public string Name { get; set; }
        public List<ScheduledEvent> Events { get; set; } = new List<ScheduledEvent>();

        public ScheduledEvent AddEvent(ScheduledEvent scheduledEvent)
        {
            scheduledEvent.QueueId = this.Id;
            Events.Add(scheduledEvent);
            Save();
            return scheduledEvent;
        }

        public QueueSchedule FindByName(string name)
        {
            var rep = new RepositoryBase<QueueSchedule>();
            return rep.AsQueryable().SingleOrDefault(x => x.Name == name);
        }

        public QueueSchedule Save()
        {
            SaveToDatabase();
            return this;
        }

        public async Task<bool> ExecuteEvent(ScheduledEvent scheduledEvent)
        {
            return await scheduledEvent.OnExecute(scheduledEvent);
        }
        public void RestartAllJobs()
        {
            var events = Events.Where(x => x.Status == ScheduledEventWorkStatus.Active).ToList();
            foreach (var scheduledEvent in events)
            {
                scheduledEvent.Status = ScheduledEventWorkStatus.Pending;
            }
            Save();
        }

        internal void SaveScheduledEvent(ScheduledEvent scheduledEvent)
        {
            var foundEvent = Events.SingleOrDefault(x => x.Id == scheduledEvent.Id);
            var indexOf = Events.IndexOf(foundEvent);
            Events[indexOf] = scheduledEvent;
            this.SaveToDatabase();
        }
    }

}
