using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using MongoDB.Bson;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperReminder : HelperBase, IHelperReminder
    {
        private const string TestQueueName = "ScheduledEvents";
        private readonly QueueSchedule _schedule;

        public HelperReminder()
        {
            var rep = new RepositoryBase<QueueSchedule>();
            _schedule = rep.AsQueryable().SingleOrDefault(x => x.Name == TestQueueName);

        }

        public void RemoveAllRemindersForAction(Action action)
        {
            
            foreach (var reminder in action.Reminders)
            {
                var actualEvent = _schedule.Events.SingleOrDefault(x => x.Id == ObjectId.Parse(reminder.Id));
                if (actualEvent != null)
                {
                    _schedule.Events.Remove(actualEvent);
                }
            }
            _schedule.Save();
            action.Reminders = new List<ScheduledEventRef>();
        }

        public ScheduledEvent AddReminderForAction(string label, DateTime executeOn, CrmUser user, Action action, int occurrences, TimeSpan repeatTimeSpan, bool remindAllTeamMembers)
        {
            NotificationObjectType secondaryObjectType;
            switch (action.ActionType)
            {
                case ActionType.Email:
                {
                    secondaryObjectType = NotificationObjectType.Email;
                    break;
                }
                case ActionType.Event:
                {
                    secondaryObjectType = NotificationObjectType.Event;
                    break;
                }
                case ActionType.Fax:
                {
                    secondaryObjectType = NotificationObjectType.Fax;
                    break;
                }
                case ActionType.Meeting:
                {
                    secondaryObjectType = NotificationObjectType.Meeting;
                    break;
                }
                case ActionType.Phone:
                {
                    secondaryObjectType = NotificationObjectType.Phonecall;
                    break;
                }
                default:
                {
                    secondaryObjectType = NotificationObjectType.Task;
                    break;
                }
            }

            var notification = new Notification
            {
                Label = label,
                ActionType = NotificationActionType.Schedule,
                PrimaryObjectType = NotificationObjectType.Reminder,
                SecondaryObjectType = secondaryObjectType,
                Action = action.AsActionRef()
            };
            notification.SaveToDatabase();

            var scheduledEvent = new ScheduledEvent
            {
                Notification = notification.AsNotificationRef(),
                QueueId = _schedule.Id,
                Label = label,
                ExecuteOn = executeOn,
                EventType = ScheduledEventType.Action,
                ReoccureSpan = repeatTimeSpan,
                OccuranceLimit = occurrences
            };

            if (remindAllTeamMembers)
            {
                scheduledEvent.CrmUsers.AddRange(user.ViewConfig.Team.AsTeam().CrmUsers);
            }
            else
            {
                scheduledEvent.CrmUsers.Add(user.AsCrmUserRef());
            }
            scheduledEvent.SaveToDatabase();

            _schedule.AddEvent(scheduledEvent);

            notification.CreateLink(scheduledEvent);

            action.Reminders.Add(scheduledEvent.AsScheduledEventRef());
            return scheduledEvent;
        }
    }
}
