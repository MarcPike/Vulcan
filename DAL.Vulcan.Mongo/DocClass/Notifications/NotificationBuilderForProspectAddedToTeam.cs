using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForProspectAddedToTeam : NotificationBuilder
    {
        private readonly Prospect _prospect;
        private readonly Team _team;

        public NotificationBuilderForProspectAddedToTeam(Team team, Prospect prospect)
        {
            _team = team;
            _prospect = prospect;
        }

        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Added,
                PrimaryObjectType = NotificationObjectType.Prospect,
                SecondaryObjectType = NotificationObjectType.Team,
                Label = $"Prospect: [{_prospect.Name}] added to the team",
                Team = _team.AsTeamRef(),
                Prospect = _prospect.AsProspectRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}