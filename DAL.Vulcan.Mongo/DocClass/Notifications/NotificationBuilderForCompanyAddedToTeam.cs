using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForCompanyAddedToTeam : NotificationBuilder
    {
        private readonly Team _team;
        private readonly Company _company;

        public NotificationBuilderForCompanyAddedToTeam(Team team, Company company)
        {
            _team = team;
            _company = company;
        }
        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Assigned,
                PrimaryObjectType = NotificationObjectType.Company,
                SecondaryObjectType = NotificationObjectType.Team,
                Label = $"Company: [{_company.ShortName}] added to the team",
                Team = _team.AsTeamRef(),
                Company = _company.AsCompanyRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}