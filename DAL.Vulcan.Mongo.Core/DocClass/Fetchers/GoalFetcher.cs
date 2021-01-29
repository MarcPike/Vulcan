using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.Fetchers
{
    public static class GoalFetcher
    {
        public static GoalFetchResult GetUserGoals(string userId)
        {
            var rep = new RepositoryBase<Goal>();
            var goals = rep.AsQueryable().ToList();
            var result = new GoalFetchResult();

            foreach (var goal in goals)
            {
                foreach (var action in goal.Actions.Select(x=>x.AsAction()))
                {
                    if (action.CrmUsers.Any(x => x.Id == userId))
                    {
                        AssignToResult(goal, result);
                    }
                }
            }

            return result;
        }

        private static void AssignToResult(Goal goal, GoalFetchResult result)
        {
            if ((goal.PendingActions.Count > 0) && (goal.FailedActions.Count == 0))
            {
                result.Pending.Add(goal);
            }
            if ((goal.PendingActions.Count == 0) && (goal.FailedActions.Count == 0))
            {
                result.Completed.Add(goal);
            }
            if (goal.FailedActions.Count > 0)
            {
                result.Failures.Add(goal);
            }
        }
    }
}