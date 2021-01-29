using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForTeamNameChanged : NotificationBuilder
    {
        private readonly Team _team;
        private readonly string _previousName;

        public NotificationBuilderForTeamNameChanged(Team team, string previousName)
        {
            _team = team;
            _previousName = previousName;
        }

        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Renamed,
                PrimaryObjectType = NotificationObjectType.Team,
                SecondaryObjectType = NotificationObjectType.None,
                Label = $"Team [{_previousName}] was renamed to : [{_team.Name}]",
                Team = _team.AsTeamRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}