using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Logger;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace DAL.Vulcan.Mongo.Core.DocClass.QueueSchedule
{
    public class ScheduledEvent: BaseDocument
    {
        private VulcanLogger _logger = new VulcanLogger();
        public ObjectId QueueId { get; set; }

        public ScheduledEventType EventType { get; set; }
        public QueueSchedule GetParent()
        {
            var rep = new RepositoryBase<QueueSchedule>();
            return rep.AsQueryable().SingleOrDefault(x => x.Id == QueueId);
        }
        public string Label { get; set; }

        public NotificationRef Notification { get; set; } 

        public TimeSpan? ReoccureSpan { get; set; }
        public int OccuranceLimit { get; set; }
        public int OccuranceCount { get; set; } = 1;

        public ScheduledEventWorkStatus Status { get; set; } = ScheduledEventWorkStatus.Pending;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExecuteOn { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime AddedOn { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? StartedOn { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompletedOn { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? FailedOn { get; set; }

        public List<CrmUserRef> CrmUsers { get; set; } = new List<CrmUserRef>();

        public string ErrorMessage { get; set; } = String.Empty;

        public TimeSpan Duration
        {
            get
            {
                if ((StartedOn == null) || (CompletedOn == null)) return new TimeSpan(0);

                return (CompletedOn ?? DateTime.Now) - (StartedOn ?? DateTime.Now);
            }
        }

        public ScheduledEvent()
        {
            
        }

        public virtual async Task<bool> OnExecute()
        {
            Console.WriteLine($"{Label} (Executing Occurrence# {OccuranceCount} of {OccuranceLimit}) @ {DateTime.Now}");
            return await GetParent().ExecuteEvent(this);
        }

        public async Task<bool> Execute()
        {
            Starting();

            var success = false;

            try
            {
                success = await OnExecute();
            }
            catch (Exception ex)
            {
                QueueError.CreateEmailExceptionLog(ex);
                //_logger.Log(className: this.GetType().Name, methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                ErrorMessage = ex.Message;
                success = false;
            }
            if (!success)
            {
                Failed();
            }
            else
            {
                Completed();
            }
            return success;
        }

        private void Completed()
        {
            CompletedOn = DateTime.Now;
            Status = ScheduledEventWorkStatus.Completed;
            SaveToDatabase();

            GetParent().SaveScheduledEvent(this);
            AddNewReoccurringTaskIfRequired();
            Console.WriteLine($"{EventType} {Label} (Completed Occurrence# {OccuranceCount} of {OccuranceLimit}) @ {DateTime.Now}");
        }


        private void Failed()
        {
            FailedOn = DateTime.Now;
            Status = ScheduledEventWorkStatus.Failed;
            SaveToDatabase();

            GetParent().SaveScheduledEvent(this);
            Console.WriteLine($"{EventType} {Label} (Failed Occurrence# {OccuranceCount} of {OccuranceLimit}) @ {DateTime.Now}");
        }

        private void Starting()
        {

            StartedOn = DateTime.Now;
            Status = ScheduledEventWorkStatus.Active;
            SaveToDatabase();

            GetParent().SaveScheduledEvent(this);
            Console.WriteLine($"{EventType} {Label} (Starting Occurrence# {OccuranceCount} of {OccuranceLimit}) @ {DateTime.Now}");
        }

        private void AddNewReoccurringTaskIfRequired()
        {
            if (ReoccureSpan != null)
            {
                if (OccuranceCount >= OccuranceLimit) return;

                var newEvent = new ScheduledEvent()
                {
                    QueueId = QueueId,
                    Notification = Notification,
                    Label = Label,
                    ExecuteOn = DateTime.Now.Add(ReoccureSpan ?? new TimeSpan(0, 0, 0)),
                    ReoccureSpan = ReoccureSpan,
                    OccuranceCount = OccuranceCount + 1,
                    OccuranceLimit = OccuranceLimit,
                    CrmUsers = CrmUsers,
                    EventType = EventType
                };
                foreach (var tag in Tags)
                {
                    newEvent.SetTagValue(tag.Key, tag.Value);
                }
                Console.WriteLine($"Adding next occurance for {newEvent.Label} #{newEvent.OccuranceCount}");
                GetParent().AddEvent(newEvent);
                GetParent().Save();
            }
        }

        public async Task<bool> OnExecute(ScheduledEvent scheduledEvent)
        {
            var success = false;
            await Task.Run(async () =>
            {
                try
                {
                    var notificationRef = scheduledEvent.Notification;
                    var notification = notificationRef.AsNotification();

                    var phoneCallId = scheduledEvent.GetTagValue("Id", ObjectId.Empty).ToString();
                    foreach (var crmUser in scheduledEvent.CrmUsers.Select(x=>x.AsCrmUser()))
                    {
                        crmUser.Notifications.Add(notificationRef);
                        crmUser.SaveToDatabase();
                        var sendToSignalR = await PublishSignalR.PublishSignalREvents.SendNewReminderToUser(crmUser.AsCrmUserRef(), notificationRef, notification.SecondaryObjectType.ToString());
                    }

                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                    scheduledEvent.FailedOn = DateTime.Now;
                    scheduledEvent.ErrorMessage = ex.Message;
                    scheduledEvent.SaveToDatabase();
                    Console.WriteLine(ex.Message);
                }

            });
            return success;
        }

        public ScheduledEventRef AsScheduledEventRef()
        {
            return new ScheduledEventRef(this);
        }
    }
}