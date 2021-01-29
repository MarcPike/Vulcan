using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace Vulcan.Schedule.Service
{
    public class ScheduleService
    {
        private const string QueueName = "ScheduledEvents";
        private QueueSchedule _schedule;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public async void Start()
        {
            var rep = new RepositoryBase<QueueSchedule>();
            _schedule = rep.AsQueryable().SingleOrDefault(x => x.Name == QueueName);

            if (_schedule == null)
            {
                _schedule = new QueueSchedule()
                {
                    Name = QueueName,
                    CreateDateTime = DateTime.Now,
                    CreatedByUserId = "Admin",
                    Events = new List<ScheduledEvent>()
                };
                _schedule.SaveToDatabase();
            }
            _schedule.RestartAllJobs();
            if (_schedule == null)
            {
                _schedule = new QueueSchedule() { Name = QueueName };
                _schedule.Save();
            }

            await Activate();
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        public async Task Activate()
        {
            await Task.Run(async () =>
                {

                    while (true)
                    {
                        if (_cancellationToken.IsCancellationRequested) break;

                        var eventFinder = new EventFinder();

                        ScheduledEvent getEvent = await eventFinder.GetNextEventAsync(_schedule.Id, _cancellationToken);

                        await getEvent.Execute();
                    }

                }
            );

        }
    }
}
