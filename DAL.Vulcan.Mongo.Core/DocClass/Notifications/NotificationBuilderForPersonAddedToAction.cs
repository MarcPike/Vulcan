using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForPersonAddedToAction : NotificationBuilder
    {
        private readonly CrmUser _crmUser;
        private readonly Action _action;
        private readonly NotificationObjectType _primaryObjectType;

        public NotificationBuilderForPersonAddedToAction(Action action, CrmUser crmUser, NotificationObjectType primaryObjectType)
        {
            _action = action;
            _crmUser = crmUser;
            _primaryObjectType = primaryObjectType;
        }
        public override Notification GetNotification()
        {
            var crmUserRef = _crmUser.AsCrmUserRef();

            var notification = new Notification()
            {
                ActionType = NotificationActionType.Assigned,
                PrimaryObjectType = _primaryObjectType,
                SecondaryObjectType = NotificationObjectType.SalesPerson,
                Label = $"You have been assigned a new Task: [{_action.Label}]",
                Action = _action.AsActionRef(),
                CrmUser = crmUserRef,
                Team = _action.Team
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
        public Notification GetNotificationForOtherUsers()
        {
            var crmUserRef = _crmUser.AsCrmUserRef();

            var notification = new Notification()
            {
                ActionType = NotificationActionType.Added,
                PrimaryObjectType = _primaryObjectType,
                SecondaryObjectType = NotificationObjectType.SalesPerson,
                Label = $"{crmUserRef.FullName} has been added to this {_action.Label}",
                Action = _action.AsActionRef(),
                CrmUser = crmUserRef
            };
            notification = Repository.Upsert(notification);
            return notification;
        }

    }
}