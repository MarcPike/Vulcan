using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;
using CrmUser = DAL.Vulcan.Mongo.Core.DocClass.CRM.CrmUser;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperGoal : HelperBase, IHelperGoal
    {
        private readonly IHelperNotifications _helperNotifications;
        private readonly IHelperAction _helperAction;
        private readonly IHelperUser _helperUser;

        public HelperGoal(
            IHelperUser helperUser,
            IHelperNotifications helperNotifications,
            IHelperAction helperTask)
        {
            _helperUser = helperUser;
            _helperNotifications = helperNotifications;
            _helperAction = helperTask;
        }

        public Strategy GetStrategy(string strategyId)
        {
            var strategy = new RepositoryBase<Strategy>().Find(strategyId);
            if (strategy == null) throw new Exception("Strategy not found");
            return strategy;
        }

        public Strategy GetNewStrategy()
        {
            var strategy = new Strategy();
            return strategy;
        }


        public void AddGoalToStrategy(string goalId, string strategyId)
        {
            var goal = GetGoal(goalId);
            if (goal == null) throw new Exception("Goal not found");

            var strategy = GetStrategy(strategyId);
            if (strategy == null) throw new Exception("Strategy not found");

            strategy.Goals.Add(goal.AsGoalRef());
            strategy.SaveToDatabase();
        }

        public void RemoveGoalFromStrategy(string goalId, string strategyId)
        {
            var goal = GetGoal(goalId);
            if (goal == null) throw new Exception("Goal not found");

            var strategy = GetStrategy(strategyId);
            if (strategy == null) throw new Exception("Strategy not found");

            var goalFound = strategy.Goals.FirstOrDefault(x => x.Id == goalId);
            if (goalFound != null)
            {
                strategy.Goals.Remove(goalFound);
                strategy.SaveToDatabase();
            }
        }

        public Goal GetGoal(string goalId)
        {
            var rep = new RepositoryBase<Goal>();
            return rep.Find(goalId);
        }

        public Milestone AddMileStone(Goal goal, string label)
        {
            return goal.AddNewMilestone(label);
        }

        public void AddAction(Goal goal, Action action)
        {
            goal.AddAction(action);
        }

        public void AssignActionToMilestone(Goal goal, Action action, Milestone milestone)
        {
            goal.AssignActionToMilestone(action, milestone);
        }

        public void AddActionAndAssignToMilestone(Goal goal, Action action, Milestone milestone)
        {
            goal.AddActionAndAssignToMilestone(action, milestone);
        }

        public List<Goal> GetGoalsForUser(CrmUser crmUser)
        {
            return new LinkResolver<Goal>(crmUser).GetAllLinkedDocuments();
        }

        public List<Goal> GetGoalsForTeam(Team team)
        {
            return new LinkResolver<Goal>(team).GetAllLinkedDocuments();
        }

        public Goal GetGoalForTeam(Team team, string goalId)
        {
            var goals = new LinkResolver<Goal>(team).GetAllLinkedDocuments();
            var id = ObjectId.Parse(goalId);
            return goals.SingleOrDefault(x => x.Id == id);
        }

        public List<MilestoneStatus> GetMilestoneStatusList(Goal goal)
        {
            return goal.GetMilestoneStatusList();
        }

        public MilestoneStatus GetMilestoneStatus(Goal goal, Milestone milestone)
        {
            if (goal.Milestones.All(x => x.Id != milestone.Id))
            {
                throw new Exception("Milestone does not exist.");
            }
            return new MilestoneStatus(goal, milestone);
        }

        public GoalModel CreateNewGoal(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            if ((crmUser.UserType != CrmUserType.Director) && (crmUser.UserType != CrmUserType.Manager))
            {
                throw new Exception("You must be a Director or a Manager to create a Goal");
            }

            var goal = new Goal
            {
                CreatedByUserId = userId
            };

            goal.CrmUsers.Add(crmUser.AsCrmUserRef());
            goal.Team = crmUser.ViewConfig.Team;
            var model = new GoalModel(goal, application, userId);
            return model;
        }

        public GoalModel SaveGoal(GoalModel model)
        {
            var actions = new List<System.Action>();
            var crmUser = _helperUser.GetCrmUser(model.Application,model.UserId);

            var rep = new RepositoryBase<Goal>();
            var goal = rep.Find(model.Id);

            if (goal == null)
            {
                goal = new Goal()
                {
                    Id = ObjectId.Parse(model.Id),
                    CreatedByUserId = model.UserId,
                };

                actions.Add(() => _helperNotifications.GoalWasAdded(goal));

                if (model.CrmUsers.All(x => x.Id != crmUser.User.Id))
                    model.CrmUsers.Add(crmUser.AsCrmUserRef());
            };

            goal.Label = model.Label;
            goal.Notes = model.Notes;
            goal.Milestones = model.Milestones;
            goal.RevenueGoal = model.RevenueGoal;
            goal.RevenueAchieved = model.RevenueAchieved;
            goal.CompleteBy = model.CompleteBy;
            goal.CompletedOn = model.CompletedOn;
            goal.Lost = model.Lost;
            goal.LostOn = model.LostOn;
            goal.LostReason = model.LostReason;

            goal.CrmUsers.AddListOfReferenceObjects(model.CrmUsers);
            goal.Actions.AddListOfReferenceObjects(model.Actions);

            goal.Team = model.Team;
            goal.SaveToDatabase();            // Run all our collected Actions

            foreach (var activityAction in actions)
            {
                activityAction.Invoke();
            }

            return new GoalModel(goal, model.Application, model.UserId);
        }

        public void RemoveGoal(Goal goal)
        {
            foreach (var action in goal.Actions.Select(x=> x.AsAction()).ToList())
            {
                _helperAction.RemoveAction(action);
            }
            _helperNotifications.RemoveNotificationsLinkedToGoal(goal);
            new RepositoryBase<Goal>().RemoveOne(goal);
        }

        public List<GoalModel> GetGoalsForUser(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var goals = new List<GoalModel>();
            foreach (var goal in crmUser.Goals.Select(x => x.AsGoal()).ToList())
            {
                if (crmUser.ViewConfig.Team.Id == goal.Team.Id)
                {
                    goals.Add(new GoalModel(goal, application, userId));
                }
            }
            return goals;
        }

        private void SynchronizeWithModelValues(GoalModel model, Goal goal)
        {
            goal.Label = model.Label;
            goal.Notes = model.Notes;
            goal.Milestones = model.Milestones;
            goal.RevenueGoal = model.RevenueGoal;
            goal.RevenueAchieved = model.RevenueAchieved;
            goal.CompleteBy = model.CompleteBy;
            goal.CompletedOn = model.CompletedOn;
            goal.Lost = model.Lost;
            goal.LostOn = model.LostOn;
            goal.LostReason = model.LostReason;
            goal.CreatedByUserId = model.CreatedByUserId;

            goal.Team = model.Team;

            if (goal.Team != null)
            {
                var team = goal.Team.AsTeam();
                team.AddGoal(goal);
                team.SaveToDatabase();
            }
        }


        private void SynchronizeGoalUsersWithModel(GoalModel model, Goal goal)
        {
            foreach (var crmUserRef in model.CrmUsers)
            {
                goal.CrmUsers.AddReferenceObject(crmUserRef);
            }

            goal.Save();
        }

    }
}