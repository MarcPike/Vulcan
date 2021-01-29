using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForActionFailed : NotificationBuilder
    {
        private readonly Action _action;
        private NotificationObjectType _primaryObjectType;

        public NotificationBuilderForActionFailed(Action action, NotificationObjectType primaryObjectType)
        {
            _primaryObjectType = primaryObjectType;
            _action = action;
        }

        public override Notification GetNotification()
        {
            //var x = GetNotificationObjectTypeForAction(action);
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Completed,
                PrimaryObjectType = _primaryObjectType,
                Label = $"{_action.ActionType}: [{_action.Label}] failed",
                Action = _action.AsActionRef(),
                Team = _action.Team
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}