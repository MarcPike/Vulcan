using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForActionAddedToGoal : NotificationBuilder
    {
        private readonly Action _action;

        public NotificationBuilderForActionAddedToGoal(Action action)
        {
            _action = action;
        }

        public override Notification GetNotification()
        {
            var goal = _action.Goal.AsGoal();

            var notification = new Notification()
            {
                ActionType = NotificationActionType.Added,
                PrimaryObjectType = NotificationObjectType.Action,
                SecondaryObjectType = NotificationObjectType.Goal,
                Label = $"Task: [{_action.Label}] was added to goal: [{goal.Label}]",
                Action = _action.AsActionRef(),
                Goal = goal?.AsGoalRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}