using System.Collections.Generic;
using System.Linq;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class MilestoneStatus
    {
        public List<Action> TasksNotStarted { get; set; } = new List<Action>();
        public List<Action> TasksStarted { get; set; } = new List<Action>();
        public List<Action> TasksCompleted { get; set; } = new List<Action>();
        public int TaskNotStartedCount => TasksNotStarted.Count;
        public int TaskStartedCount => TasksStarted.Count;
        public int TaskCompletedCount => TasksCompleted.Count;
        public int TotalCount => TaskNotStartedCount + TaskStartedCount + TaskCompletedCount;

        public MilestoneStatus(Goal goal, Milestone milestone)
        {
            var actions = goal.Actions.Select(x=>x.AsAction()).Where(x => x.MilestoneId == milestone.Id).ToList();
            TasksNotStarted.AddRange(actions.Where(x => x.IsStarted == false && x.IsCompleted == false));
            TasksStarted.AddRange(actions.Where(x => x.IsStarted && x.IsCompleted == false));
            TasksStarted.AddRange(actions.Where(x => x.IsCompleted));
        }
    }
}