using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Models
{
    public class GoalModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Application { get; set; }
        public string Label { get; set; }


        public GuidList<Note> Notes { get; set; } 
        public GuidList<Milestone> Milestones { get; set; } 

        public int RevenueGoal { get; set; } = 0;
        public int RevenueAcheived { get; set; } = 0;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompleteBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompletedOn { get; set; }

        public bool Lost { get; set; }
        public DateTime? LostOn { get; set; }
        public string LostReason { get; set; }

        public List<CrmUserRef> CrmUsers { get; set; }
        public List<ActionRef> Actions { get; set; }


        public TeamRef Team { get; set; }

        public List<string> AudienceChoices { get; set; }
        public string CreatedByUserId { get; set; }

        public bool IsRevenueGoalAcheived { get; set; }

        public bool IsDateGoalAcheived { get; set; }


        public List<Action> PendingActions { get; set; }

        public List<Action> FailedActions { get; set; }

        public List<Action> CompletedActions { get; set; }

        public List<Action> NotStartedActions { get; set; }

        public GoalModel(Goal goal, string application, string userId)
        {
            Id = goal.Id.ToString();
            Label = goal.Label;
            Notes = goal.Notes;
            Milestones = goal.Milestones;
            RevenueGoal = goal.RevenueGoal;
            RevenueAcheived = goal.RevenueAcheived;
            CompleteBy = goal.CompleteBy;
            CompletedOn = goal.CompletedOn;
            Lost = goal.Lost;
            LostOn = goal.LostOn;
            LostReason = goal.LostReason;

            CrmUsers = goal.CrmUsers;
            Actions = goal.Actions;

            Application = application;
            UserId = userId;
            Team = goal.Team;
            CreatedByUserId = goal.CreatedByUserId;
            IsDateGoalAcheived = goal.IsDateGoalAcheived;
            IsRevenueGoalAcheived = goal.IsRevenueGoalAcheived;
            PendingActions = goal.PendingActions;
            FailedActions = goal.FailedActions;
            CompletedActions = goal.CompletedActions;
            NotStartedActions = goal.NotStartedActions;
        }

    }
}
