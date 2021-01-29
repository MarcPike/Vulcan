using System;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using DAL.Vulcan.Mongo.Core.DocClass.QueueSchedule;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;
namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperReminder
    {
        void RemoveAllRemindersForAction(Action action);
        ScheduledEvent AddReminderForAction(string label, DateTime executeOn, CrmUser user, DocClass.CRM.Action action, int occurrences, TimeSpan repeatTimeSpan, bool remindAllTeamMembers);
    }
}