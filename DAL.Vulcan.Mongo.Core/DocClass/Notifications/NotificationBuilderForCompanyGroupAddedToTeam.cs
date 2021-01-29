using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Notifications
{
    public class NotificationBuilderForCompanyGroupAddedToTeam : NotificationBuilder
    {
        private readonly Team _team;
        private readonly CompanyGroup _companyGroup;

        public NotificationBuilderForCompanyGroupAddedToTeam(Team team, CompanyGroup companyGroup)
        {
            _team = team;
            _companyGroup = companyGroup;
        }
        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Assigned,
                PrimaryObjectType = NotificationObjectType.CompanyGroup,
                SecondaryObjectType = NotificationObjectType.Team,
                Label = $"Company Group: [{_companyGroup.Name}] added to the team",
                Team = _team.AsTeamRef(),
                CompanyGroup = _companyGroup.AsCompanyGroupRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}