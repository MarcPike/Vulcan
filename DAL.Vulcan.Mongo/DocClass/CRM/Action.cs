using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class Action: BaseDocument
    {
        public ActionType ActionType { get; set; } = ActionType.Task;
        public bool IsTeamAction { get; set; } 
        public string Label { get; set; }
        public bool IsStarted => PercentComplete > 0;
        public bool IsCompleted => PercentComplete == 100;
        public int PercentComplete { get; set; } = 0;
        public GoalRef Goal { get; set; }

        public Guid MilestoneId { get; set; } = Guid.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DueDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Completed { get; set; }
        public bool Failure { get; set; }
        public string FailureReason { get; set; } = string.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? FailedOn { get; set; }

        public List<ActionHistory> ActionHistory { get; set; } = new List<ActionHistory>();

        public List<Address> Addresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; }

        public ReferenceList<CrmUser, CrmUserRef> CrmUsers { get; set; } = new ReferenceList<CrmUser, CrmUserRef>();
        public ReferenceList<Contact, ContactRef> Contacts { get; set; } = new ReferenceList<Contact, ContactRef>();

        public ReferenceList<Notification, NotificationRef> Notifications { get; set; } = new ReferenceList<Notification, NotificationRef>();

        public ReferenceList<Action, ActionRef> FollowupActions { get; set; } = new ReferenceList<Action, ActionRef>();

        public ReferenceList<Action,ActionRef> ScheduledActions { get; set; } = new ReferenceList<Action, ActionRef>();
        public ActionSchedule Schedule { get; set; } = new ActionSchedule();
        public bool IsScheduled => ScheduledActions.Count == 0;

        public List<ScheduledEventRef> Reminders { get; set; } = new List<ScheduledEventRef>();

        public List<CompanyRef> Companies { get; set; }

        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();

        public TeamRef Team { get; set; }

        public int ReminderMinutesPrior { get; set; } = 0;

        public TimeSpan? RepeatSpan { get; set; }
        public int RepeatCount { get; set; } = 0;
        public ObjectId? RepeatFromActionId { get; set; } 

        public bool IsCreatedByDirector
        {
            get
            {
                return new RepositoryBase<CrmUser>().AsQueryable().Any(x => x.User.Id == CreatedByUserId && x.UserType == CrmUserType.Director);
            }
        }

        public void ApplySchedule()
        {
            ActionSchedule.ApplySchedule(this);
        }

        public bool IsCreatedByManager
        {
            get
            {
                if (IsCreatedByDirector) return false;
                return new RepositoryBase<CrmUser>().AsQueryable().Any(x => x.User.Id == CreatedByUserId && x.UserType == CrmUserType.Manager);
            }
        }

        public bool IsCreatedBySalesPerson
        {
            get
            {
                if (IsCreatedByManager) return false;

                return new RepositoryBase<CrmUser>().AsQueryable().Any(x => x.User.Id == CreatedByUserId && x.UserType == CrmUserType.SalesPerson);
            }
        }

        public void Fail(string reason)
        {
            Failure = true;
            FailureReason = reason;
            FailedOn = DateTime.Now;
            SaveToDatabase();
        }

        public void AssignToGoal(GoalRef goalRef)
        {
            Goal = goalRef;
            SaveToDatabase();
            var goal = goalRef.AsGoal();
            goal.Actions.AddReferenceObject(this.AsActionRef());
            goal.SaveToDatabase();
        }


        public ActionRef AsActionRef()
        {
            return new ActionRef(this);
        }
    }

    

}