using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public enum ActionScheduleType
    {
        UseDayOfWeek,
        UseDayOfMonth,
        UseDaysFrom
    }
    public class ActionSchedule
    {
        public ActionScheduleType ScheduleType { get; set; } = ActionScheduleType.UseDaysFrom;
        public int DaysFrom { get; set; } = 0;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

        public static void ApplySchedule(Action action)
        {
            var rep = new RepositoryBase<Action>();
            var schedule = action.Schedule;
            var result = new List<DateTime>();
            var startDate = action.DueDate.Date;
            var startTime = action.DueDate - startDate;

            schedule.Validate(startDate);
            if (action.ScheduledActions.Count > 0)
            {
                schedule.RemoveAllFutureActions(rep, action.ScheduledActions.Select(x => x.AsAction()).ToList());
            }

            var followupActionsNotCompleted =
                action.FollowupActions.Select(x => x.AsAction()).Where(x => x.IsCompleted == false);

            var followupActionList = new ReferenceList<Action,ActionRef>();
            followupActionList.AddListOfReferenceObjects(followupActionsNotCompleted.Select(x=>x.AsActionRef()).ToList());

            var scheduledActions = new List<Action>();

            if (schedule.ScheduleType == ActionScheduleType.UseDayOfMonth)
            {
                while (startDate < schedule.EndDate)
                {
                    startDate = startDate.AddMonths(1);

                    if (startDate > schedule.EndDate) break;

                    var newAction = CreateNewAction(action, startDate, startTime, followupActionList, schedule, rep);
                    scheduledActions.Add(newAction);
                }
            }

            if (schedule.ScheduleType == ActionScheduleType.UseDayOfWeek)
            {
                while (startDate < schedule.EndDate)
                {
                    startDate = startDate.AddDays(7);

                    if (startDate > schedule.EndDate) break;
                    
                    var newAction = CreateNewAction(action, startDate, startTime, followupActionList, schedule, rep);
                    scheduledActions.Add(newAction);
                }
            }

            if (schedule.ScheduleType == ActionScheduleType.UseDaysFrom)
            {
                while (startDate < schedule.EndDate)
                {
                    startDate = startDate.AddDays(schedule.DaysFrom);

                    if (startDate > schedule.EndDate) break;

                    var newAction = CreateNewAction(action, startDate, startTime, followupActionList, schedule, rep);
                    scheduledActions.Add(newAction);
                }
            }

            PublishNewScheduledActionsToAllCrmUsers(action, scheduledActions);

            BuildScheduledActionsReferences(action, scheduledActions, rep);
        }

        private static void PublishNewScheduledActionsToAllCrmUsers(Action action, List<Action> scheduledActions)
        {
            foreach (var crmUser in action.CrmUsers.Select(x => x.AsCrmUser()))
            {
                crmUser.Actions.AddRange(scheduledActions.Select(x => x.AsActionRef()));
                crmUser.SaveToDatabase();
            }
        }

        private static void BuildScheduledActionsReferences(Action action, List<Action> scheduledActions, RepositoryBase<Action> rep)
        {

            var futureActions = new List<Action>();
            for (int i = scheduledActions.Count - 1; i >= 0; i--)
            {
                if (futureActions.Count > 0)
                {
                    scheduledActions[i].ScheduledActions = new ReferenceList<Action, ActionRef>();
                    scheduledActions[i].ScheduledActions
                        .AddListOfReferenceObjects(futureActions.Select(x => x.AsActionRef()).ToList());
                    rep.Upsert(scheduledActions[i]);
                }
                futureActions.Add(scheduledActions[i]);
            }
            action.ScheduledActions.AddListOfReferenceObjects(futureActions.Where(x=>x.Id != action.Id).Select(x => x.AsActionRef()).ToList());
            rep.Upsert(action);
        }

        private static Action CreateNewAction(Action action, DateTime startDate, TimeSpan startTime,
            ReferenceList<Action, ActionRef> followupActionList, ActionSchedule schedule, RepositoryBase<Action> rep)
        {
            var newAction = new Action()
            {
                CrmUsers = action.CrmUsers,
                ActionHistory = new List<ActionHistory>(),
                ActionType = action.ActionType,
                Addresses = action.Addresses,
                Completed = null,
                Contacts = action.Contacts,
                CreateDateTime = action.CreateDateTime,
                CreatedByUserId = action.CreatedByUserId,
                DueDate = startDate + startTime,
                EmailAddresses = action.EmailAddresses,
                FailedOn = null,
                Failure = false,
                FailureReason = String.Empty,
                FollowupActions = followupActionList,
                Goal = action.Goal,
                Label = action.Label,
                Links = action.Links,
                MilestoneId = action.MilestoneId,
                ModifiedByUserId = action.ModifiedByUserId,
                ModifiedDateTime = action.ModifiedDateTime,
                Notes = action.Notes,
                Notifications = action.Notifications,
                PercentComplete = 0,
                PhoneNumbers = action.PhoneNumbers,
                ReminderMinutesPrior = action.ReminderMinutesPrior,
                RepeatCount = action.RepeatCount,
                RepeatFromActionId = action.RepeatFromActionId,
                RepeatSpan = action.RepeatSpan,
                Schedule = schedule,
                ScheduledActions = new ReferenceList<Action, ActionRef>(),
                SearchTags = action.SearchTags,
                Tags = action.Tags,
                Team = action.Team,
                ExtraElements = action.ExtraElements,
                Version = action.Version
            };
            rep.Upsert(newAction);
            return newAction;
        }

        public void RemoveAllFutureActions(RepositoryBase<Action> rep, List<Action> futureActions)
        {
            foreach (var removeAction in futureActions)
            {
                if (removeAction == null) continue;
                foreach (var crmUser in removeAction.CrmUsers.Select(x=>x.AsCrmUser()))
                {
                    crmUser.Actions.RemoveDocumentRef(removeAction.AsActionRef());
                    crmUser.SaveToDatabase();
                }
                rep.RemoveOne(removeAction);
            }

        }

        public void Validate(DateTime startDate)
        {
            if (ScheduleType == ActionScheduleType.UseDaysFrom && DaysFrom <= 0)
            {
                throw new Exception("Action schedule has insufficient data");
            }

            if (startDate > EndDate)
            {
                throw new Exception("DueDate is greater than Schedule.EndDate");
            }
        }
    }
}
