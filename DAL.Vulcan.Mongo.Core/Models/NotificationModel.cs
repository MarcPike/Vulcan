using System;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class NotificationModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string ActionType { get; set; }
        public string PrimaryObjectType { get; set; }
        public string SecondaryObjectType { get; set; }
        public string Label { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NotificationDate { get; set; }

        public StrategyRef Strategy { get; set; }
        public GoalRef Goal { get; set; }
        public ActionRef Action { get; set; }
        public ContactMeetingInviteRef ContactMeetingInvite { get; set; }
        public TeamRef Team { get; set; }

        public CrmUserRef CrmUser { get; set; }

        public CompanyGroupRef CompanyGroup { get; set; }
        public CompanyRef Company { get; set; }

        public NotificationModel(string application, string userId, Notification notification)
        {
            Application = application;
            UserId = userId;
            Id = notification.Id.ToString();
            Label = notification.Label;
            ActionType = notification.ActionType.ToString();
            PrimaryObjectType = notification.PrimaryObjectType.ToString();
            SecondaryObjectType = notification.SecondaryObjectType.ToString();

            Goal = notification.Goal;
            Action = notification.Action;

            CrmUser = notification.CrmUser;

            NotificationDate = notification.NotificationDate;
            Strategy = notification.Strategy;
            ContactMeetingInvite = notification.ContactMeetingInvite;
            Team = notification.Team;
            CompanyGroup = notification.CompanyGroup;
            Company = notification.Company;
        }

    }
}