using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForUserAddedToTeam : NotificationBuilder
    {
        private readonly CrmUserRef _crmUserRef;
        private readonly Team _team;

        public NotificationBuilderForUserAddedToTeam(CrmUserRef crmUserRef, Team team)
        {
            _crmUserRef = crmUserRef;
            _team = team;
        }
        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Added,
                PrimaryObjectType = NotificationObjectType.SalesPerson,
                SecondaryObjectType = NotificationObjectType.Team,
                Label = $"SalesPerson: [{_crmUserRef.FullName}] was added to Team",
                CrmUser = _crmUserRef,
                Team = _team.AsTeamRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}