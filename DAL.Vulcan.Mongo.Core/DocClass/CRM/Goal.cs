using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class Goal: BaseDocument
    {
        public string Label { get; set; }

        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public GuidList<Milestone> Milestones { get; set; } = new GuidList<Milestone>();

        public int RevenueGoal { get; set; } = 0;
        public int RevenueAchieved { get; set; } = 0;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompleteBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompletedOn { get; set; }

        public bool Lost { get; set; }
        public DateTime? LostOn { get; set; }
        public string LostReason { get; set; } = String.Empty;

        public ReferenceList<CrmUser,CrmUserRef> CrmUsers { get; set; } = new ReferenceList<CrmUser, CrmUserRef>();
        public ReferenceList<Action,ActionRef> Actions { get; } = new ReferenceList<Action, ActionRef>();
        public ReferenceList<Contact, ContactRef> Contacts { get; } = new ReferenceList<Contact, ContactRef>();
        public TeamRef Team { get; set; }


        public bool IsCreatedByManager
        {
            get
            {
                var creator = new RepositoryBase<CrmUser>().AsQueryable().SingleOrDefault(x => x.User.Id == CreatedByUserId);
                return creator?.UserType == CrmUserType.Manager;
            }
        }

        public bool IsCreatedByDirector
        {
            get
            {
                var creator = new RepositoryBase<CrmUser>().AsQueryable().SingleOrDefault(x => x.User.Id == CreatedByUserId);
                return creator?.UserType == CrmUserType.Director;
            }
        }

        public bool IsCreatedBySalesPerson
        {
            get
            {
                var creator = new RepositoryBase<CrmUser>().AsQueryable().SingleOrDefault(x => x.User.Id == CreatedByUserId);
                return creator?.UserType == CrmUserType.SalesPerson;
            }
        }


        public bool IsRevenueGoalAcheived
        {
            get
            {
                if (RevenueGoal == 0) return true;

                return RevenueAchieved >= RevenueGoal;
            }
        }

        public bool IsDateGoalAcheived
        {
            get
            {
                if (CompleteBy == null) return true;

                return (CompletedOn <= CompleteBy);
            }
        }

        public void CheckIfGoalCompleted()
        {
            
            if ((PendingActions.Count == 0) && (FailedActions.Count == 0))
            {
                CompletedOn = DateTime.Now;
            }
        }

        public List<Action> PendingActions
        {
            get
            {
                return Actions.Select(x => x.AsAction()).Where(x => x.IsCompleted == false).ToList();
            }
        }

        public List<Action> FailedActions
        {
            get
            {
                return Actions.Select(x => x.AsAction()).Where(x => x.Failure).ToList();
            }
        }

        public List<Action> CompletedActions
        {
            get
            {
                return Actions.Select(x=>x.AsAction()).Where(x => x.IsCompleted == true).ToList();
            }
        }

        public List<Action> NotStartedActions
        {
            get
            {
                return Actions.Select(x => x.AsAction()).Where(x => x.IsCompleted == false && x.IsStarted == false).ToList();
            }
        }

        public List<MilestoneStatus> GetMilestoneStatusList()
        {
            var result = new List<MilestoneStatus>();
            foreach (var milestone in Milestones)
            {
                result.Add(new MilestoneStatus(this,milestone));
            }
            return result;
        }

        public void AddAction(Action action)
        {
            if (Actions.All(x => x.Id != action.Id.ToString()))
            {
                Actions.AddReferenceObject(action.AsActionRef());
                foreach (var crmUser in action.CrmUsers)
                {
                    CrmUsers.AddReferenceObject(crmUser);
                }
                Save();
                CheckIfGoalCompleted();
            }
        }

        public void AssignActionToMilestone(Action action, Milestone milestone)
        {
            if (Milestones.All(x => x.Id != milestone.Id))
            {
                throw new Exception("Milestone does not exist in this goal");
            }

            if (Actions.Select(x => x.AsAction()).All(x => x.Id != action.Id))
            {
                throw new Exception("Task does not exist in this goal");
            }

            var updateAction = Actions.Select(x => x.AsAction()).Single(x => x.Id == action.Id);
            updateAction.MilestoneId = milestone.Id;
            updateAction.SaveToDatabase();
            
            Save();
        }

        public void AddActionAndAssignToMilestone(Action action, Milestone milestone)
        {
            AddAction(action);   
            AssignActionToMilestone(action,milestone);
        }

        public Milestone AddNewMilestone(string label)
        {
            if (Milestones.Any(x => x.Label == label))
            {
                throw new Exception("Milestone with the label already exists");
            }

            var milestone = new Milestone()
            {
                Label = label,
               
            };
            Milestones.Add(milestone);
            Save();

            return milestone;
        }

        public void Save()
        {
            SaveToDatabase();

        }

        public GoalRef AsGoalRef()
        {
            return new GoalRef(this);
        }

        public void UpdateGoalStatus()
        {
            CheckIfGoalCompleted();
            Save();
        }

        public void SetRevenueGoal(int revenueGoal)
        {
            RevenueGoal = revenueGoal;
            Save();
        }

        public void SetRevenueAchieved(int revenueAcheived)
        {
            RevenueAchieved = revenueAcheived;
            Save();
        }
        public void AddRevenueAchieved(int revenueAcheived)
        {
            RevenueAchieved += revenueAcheived;
            Save();
        }

        public void RecordLoss(string reason)
        {
            Lost = true;
            LostOn = DateTime.Now;
            LostReason = reason;
            CheckIfGoalCompleted();
            Save();
        }

        public void RemoveLoss()
        {
            Lost = false;
            LostOn = null;
            LostReason = String.Empty;
            CheckIfGoalCompleted();
            Save();
        }

    }
}
