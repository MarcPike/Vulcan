using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForGoalLost : NotificationBuilder
    {
        private readonly Goal _goal;

        public NotificationBuilderForGoalLost(Goal goal)
        {
            _goal = goal;
        }

        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Lost,
                PrimaryObjectType = NotificationObjectType.Goal,
                Label = $"Goal: [{_goal.Label}] was Lost due to [{_goal.LostReason}]",
                Goal = _goal.AsGoalRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}