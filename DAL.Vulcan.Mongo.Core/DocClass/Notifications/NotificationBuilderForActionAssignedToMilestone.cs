using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForActionAssignedToMilestone : NotificationBuilder
    {
        private readonly Milestone _milestone;
        private readonly Action _action;
        private readonly Goal _goal;

        public NotificationBuilderForActionAssignedToMilestone(Goal goal, Action action, Milestone milestone)
        {
            _goal = goal;
            _action = action;
            _milestone = milestone;
        }

        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Assigned,
                PrimaryObjectType = NotificationObjectType.Action,
                SecondaryObjectType = NotificationObjectType.Milestone,
                Label = $"Task: [{_action.Label}] was assigned to Milestone: [{_milestone.Label}]",
                Action = _action.AsActionRef(),
                Goal = _goal.AsGoalRef()
            };
            notification = Repository.Upsert(notification);
            return notification;

        }
    }
}