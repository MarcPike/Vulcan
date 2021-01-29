using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForProspectRemovedFromTeam : NotificationBuilder
    {
        private readonly Prospect _prospect;
        private readonly Team _team;

        public NotificationBuilderForProspectRemovedFromTeam(Team team, Prospect prospect)
        {
            _team = team;
            _prospect = prospect;
        }

        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Removed,
                PrimaryObjectType = NotificationObjectType.Prospect,
                SecondaryObjectType = NotificationObjectType.Team,
                Label = $"Prospect: [{_prospect.Name}] removed from the team",
                Team = _team.AsTeamRef(),
                Prospect = _prospect.AsProspectRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}