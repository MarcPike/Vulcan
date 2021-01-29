using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using CrmUser = DAL.Vulcan.Mongo.DocClass.CRM.CrmUser;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperGoal
    {
        Milestone AddMileStone(Goal goal, string label);
        void AddAction(Goal goal, Action action);
        void AddActionAndAssignToMilestone(Goal goal, Action action, Milestone milestone);
        void AssignActionToMilestone(Goal goal, Action action, Milestone milestone);
        Goal GetGoal(string goalId);
        List<Goal> GetGoalsForUser(CrmUser crmUser);
        List<Goal> GetGoalsForTeam(Team team);
        Goal GetGoalForTeam(Team team, string goalId);
        List<MilestoneStatus> GetMilestoneStatusList(Goal goal);
        MilestoneStatus GetMilestoneStatus(Goal goal, Milestone milestone);

        GoalModel CreateNewGoal(string application, string userId);
        GoalModel SaveGoal(GoalModel model);
        void RemoveGoal(Goal goal);
        List<GoalModel> GetGoalsForUser(string application, string userId);
    }
}