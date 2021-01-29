using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForUserRemovedFromTeam : NotificationBuilder
    {
        private readonly CrmUserRef _crmUserRef;
        private readonly Team _team;

        public NotificationBuilderForUserRemovedFromTeam(CrmUserRef crmUserRef, Team team)
        {
            _crmUserRef = crmUserRef;
            _team = team;
        }

        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Removed,
                PrimaryObjectType = NotificationObjectType.SalesPerson,
                SecondaryObjectType = NotificationObjectType.Team,
                Label = $"SalesPerson: [{_crmUserRef.FullName}] was removed from Team",
                CrmUser = _crmUserRef,
                Team = _team.AsTeamRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}