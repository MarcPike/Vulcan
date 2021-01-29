using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForActionWasModified : NotificationBuilder
    {
        private readonly Action _action;
        private NotificationObjectType _primaryObjectType;

        public NotificationBuilderForActionWasModified(Action action, NotificationObjectType primaryObjectType)
        {
            _primaryObjectType = primaryObjectType;
            _action = action;
        }

        public override Notification GetNotification()
        {
            //var x = GetNotificationObjectTypeForAction(action);
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Modified,
                PrimaryObjectType = _primaryObjectType,
                Label = $"{_action.ActionType}: [{_action.Label}] progress has changed ({_action.PercentComplete}%)",
                Action = _action.AsActionRef(),
                Team = _action.Team
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}