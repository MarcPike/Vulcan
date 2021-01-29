using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForCompanyGroupRemovedFromTeam : NotificationBuilder
    {
        private readonly Team _team;
        private readonly CompanyGroup _companyGroup;

        public NotificationBuilderForCompanyGroupRemovedFromTeam(Team team, CompanyGroup companyGroup)
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
                Label = $"Company Group: [{_companyGroup.Name}] removed from the team",
                Team = _team.AsTeamRef(),
                CompanyGroup = _companyGroup.AsCompanyGroupRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}