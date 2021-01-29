using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Fetchers
{
    public class GoalFetchResult
    {
        public List<Goal> Completed;
        public List<Goal> Pending;
        public List<Goal> Failures;
        public List<Goal> Losses;

        public void ReduceFromAnotherGoalResult(GoalFetchResult other)
        {
            foreach (var goal in other.Completed)
            {
                var completed = Completed.SingleOrDefault(x => x.Id == goal.Id);
                if (completed != null)
                {
                    Completed.Remove(completed);
                }
            }

            foreach (var goal in other.Pending)
            {
                var pending = Pending.SingleOrDefault(x => x.Id == goal.Id);
                if (pending != null)
                {
                    Pending.Remove(pending);
                }
            }

            foreach (var goal in other.Failures)
            {
                var failure = Failures.SingleOrDefault(x => x.Id == goal.Id);
                if (failure != null)
                {
                    Failures.Remove(failure);
                }
            }

            foreach (var goal in other.Losses)
            {
                var loss = Losses.SingleOrDefault(x => x.Id == goal.Id);
                if (loss != null)
                {
                    Losses.Remove(loss);
                }
            }

        }
    }
}