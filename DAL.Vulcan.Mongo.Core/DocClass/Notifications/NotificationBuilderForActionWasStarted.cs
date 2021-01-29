using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForActionWasStarted : NotificationBuilder
    {
        private readonly Action _action;
        private NotificationObjectType _primaryObjectType;

        public NotificationBuilderForActionWasStarted(Action action, NotificationObjectType primaryObjectType)
        {
            _primaryObjectType = primaryObjectType;
            _action = action;
        }

        public override Notification GetNotification()
        {
            //var x = GetNotificationObjectTypeForAction(action);
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Started,
                PrimaryObjectType = _primaryObjectType,
                Label = $"{_action.ActionType}: [{_action.Label}] was started ({_action.PercentComplete}%)",
                Action = _action.AsActionRef(),
                Team = _action.Team
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}