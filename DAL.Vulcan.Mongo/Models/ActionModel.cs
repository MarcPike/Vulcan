using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Helpers;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Models
{
    public class ActionModel : BaseModel
    {
        public bool IsTeamAction { get; set; } 

        public string Id { get; set; }
        public string ActionType { get; set; }

        public string Label { get; set; }
        public bool IsStarted => PercentComplete > 0;
        public bool IsCompleted => PercentComplete == 100;
        public int PercentComplete { get; set; } = 0;

        public GoalRef Goal { get; set; }
        public Guid MilestoneId { get; set; } = Guid.Empty;

        public bool Failure { get; set; }
        public string FailureReason { get; set; } = string.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? FailedOn { get; set; }

        public List<ActionHistory> ActionHistory { get; set; } = new List<ActionHistory>();

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Completed { get; set; }

        public List<Address> Addresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; }

        public ReferenceList<DocClass.CRM.CrmUser, CrmUserRef> CrmUsers { get; set; } = new ReferenceList<DocClass.CRM.CrmUser, CrmUserRef>();
        public ReferenceList<Contact, ContactRef> Contacts { get; set; } = new ReferenceList<Contact, ContactRef>();

        public ReferenceList<Action, ActionRef> FollowupActions { get; set; } = new ReferenceList<Action, ActionRef>();

        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();

        public TeamRef Team { get; set; }

        public List<string> SearchTags { get; set; } = new List<string>();

        public List<GridFSFileInfo> FileAttachments { get; set; } = new List<GridFSFileInfo>();

        public int ReminderMinutesPrior { get; set; } = 0;

        public List<ScheduledEventRef> Reminders { get; set; } = new List<ScheduledEventRef>();

        public ActionModel() : base()
        {
        }

        private ActionModel(string application, string userId, Action action) : base(application, userId)
        {
            Id = action.Id.ToString();
            ActionType = action.ActionType.ToString();

            EmailAddresses = action.EmailAddresses ?? new List<EmailAddress>();
            Addresses = action.Addresses ?? new List<Address>();
            PhoneNumbers = action.PhoneNumbers ?? new List<PhoneNumber>();

            Contacts = action.Contacts ?? new ReferenceList<Contact, ContactRef>();
            CrmUsers = action.CrmUsers ?? new ReferenceList<CrmUser, CrmUserRef>();

            DueDate = action.DueDate;
            Completed = action.Completed;
            PercentComplete = action.PercentComplete;
            ReminderMinutesPrior = action.ReminderMinutesPrior;
            Label = action.Label;

            FollowupActions = action.FollowupActions ?? new ReferenceList<Action, ActionRef>();

            Team = action.Team;

            ActionHistory = action.ActionHistory;

            SearchTags = action.SearchTags;
            Goal = action.Goal;
            MilestoneId = action.MilestoneId;

            Notes = action.Notes;

            Reminders = action.Reminders ?? new List<ScheduledEventRef>();

            IsTeamAction = action.IsTeamAction;

            FileAttachments = Base.FileAttachment.FileAttachmentsVulcan.GetAllAttachmentsForDocument(action) ?? new List<GridFSFileInfo>();
        }
        public static ActionModel GetActionModel(string application, string userId, string actionId)
        {
            var action = new RepositoryBase<Action>().Find(actionId);
            if (action == null) throw new Exception("Action not found");

            var result = new ActionModel(application, userId, action)
            {
                Application = application,
                UserId = userId
            };
            return result;
        }

        public static ActionModel CreateNewActionModel(string application, string userId)
        {
            var action = new Action()
            {
                CreatedByUserId = userId,
                CreateDateTime = DateTime.Now
            };
            var _helperUser = new HelperUser(new HelperPerson());
            var crmUser = _helperUser.GetCrmUser(application, userId);
            action.CrmUsers.Add(crmUser.AsCrmUserRef());
           
            var team = crmUser.ViewConfig.Team;

            var result = new ActionModel(application, userId, action)
            {
                Application = application,
                UserId = userId,
                Team = team,
                DueDate = DateTime.Now
            };
            return result;
        }

    }
}
