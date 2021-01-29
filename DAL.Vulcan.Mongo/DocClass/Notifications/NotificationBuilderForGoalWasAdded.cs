using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForGoalWasAdded : NotificationBuilder
    {
        private readonly Goal _goal;

        public NotificationBuilderForGoalWasAdded(Goal goal)
        {
            _goal = goal;
        }
        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Added,
                PrimaryObjectType = NotificationObjectType.Goal,
                Label = $"Goal: [{_goal.Label}] was added",
                Goal = _goal.AsGoalRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}