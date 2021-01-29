using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForTeamCreated : NotificationBuilder
    {
        private readonly Team _team;

        public NotificationBuilderForTeamCreated(Team team)
        {
            _team = team;
        }
        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Created,
                PrimaryObjectType = NotificationObjectType.Team,
                Label = $"New Team: [{_team.Name}] was created",
                Team = _team.AsTeamRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}