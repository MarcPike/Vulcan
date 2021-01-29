using System;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;
namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperReminder
    {
        void RemoveAllRemindersForAction(Action action);
        ScheduledEvent AddReminderForAction(string label, DateTime executeOn, CrmUser user, DocClass.CRM.Action action, int occurrences, TimeSpan repeatTimeSpan, bool remindAllTeamMembers);
    }
}