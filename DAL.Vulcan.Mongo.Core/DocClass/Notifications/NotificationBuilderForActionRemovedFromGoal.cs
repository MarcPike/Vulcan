using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForActionRemovedFromGoal : NotificationBuilder
    {
        private readonly Action _action;
        private readonly Goal _goal;

        public NotificationBuilderForActionRemovedFromGoal(Action action, Goal goal)
        {
            _action = action;
            _goal = goal;
        }

        public override Notification GetNotification()
        {

            _action.RemoveLink(_goal);
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Removed,
                PrimaryObjectType = NotificationObjectType.Action,
                SecondaryObjectType = NotificationObjectType.Goal,
                Label = $"Task: [{_action.Label}] was removed from goal: [{_goal.Label}]",
                Action = _action.AsActionRef(),
                Goal = _goal?.AsGoalRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}