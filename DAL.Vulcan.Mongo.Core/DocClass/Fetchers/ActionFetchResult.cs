using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;
namespace DAL.Vulcan.Mongo.Core.DocClass.Fetchers
{
    public class ActionFetchResult
    {
        public List<Action> Completed;
        public List<Action> Pending;
        public List<Action> Failures;

        public void ReduceFromAnotherTaskResult(ActionFetchResult other)
        {
            foreach (var task in other.Completed)
            {
                var completed = Completed.SingleOrDefault(x => x.Id == task.Id);
                if (completed != null)
                {
                    Completed.Remove(completed);
                }
            }

            foreach (var task in other.Pending)
            {
                var pending = Pending.SingleOrDefault(x => x.Id == task.Id);
                if (pending != null)
                {
                    Pending.Remove(pending);
                }
            }

            foreach (var task in other.Failures)
            {
                var failure = Failures.SingleOrDefault(x => x.Id == task.Id);
                if (failure != null)
                {
                    Failures.Remove(failure);
                }
            }

        }

    }
}