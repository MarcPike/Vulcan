using System;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperUserViewConfig : HelperBase, IHelperUserViewConfig
    {
        private readonly IHelperUser _helperUser;
        private readonly IHelperTeam _helperTeam;


        public HelperUserViewConfig(
            IHelperUser helperUser,
            IHelperTeam helperTeam)
        {
            _helperUser = helperUser;
            _helperTeam = helperTeam;
        }

        public ViewConfig GetViewConfig(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            return crmUser.ViewConfig;

        }

        public string GetDefaultRoleName(string application, string userId)
        {
            var roleName = string.Empty;
            var user = _helperUser.GetUser(userId);
            var roles = user.GetApplicationRoles(application);
            if (roles.Any(x => x == "Manager"))
            {
                roleName = "Manager";
            }
            else if (roles.Any(x => x == "SalesPerson"))
            {
                roleName = "SalesPerson";
            }
            return roleName;
        }

        public ViewConfig SetViewConfig(string application, string userId, string salesTeamId = null)
        {
            var user = _helperUser.GetUser(userId);

            RemoveUserFromAllTeamActiveUsers(application, userId);

            var result = GetViewConfig(application, userId);
            if (salesTeamId == null)
            {
                result.ViewType = ViewType.MyStuff;
                result.Team = null;
            }
            else
            {
                var salesTeam = _helperTeam.GetTeam(salesTeamId);
                result.ViewType = ViewType.Team;
                result.Team = salesTeam.AsTeamRef();
                AddUserToTeamActiveUsers(user,salesTeam);
            }

            return result;

        }

        private void AddUserToTeamActiveUsers(LdapUser user, Team team)
        {
            team.ActiveUsers.Add(user.AsUserRef());
            team.Save();
        }
        

        private void RemoveUserFromAllTeamActiveUsers(string application, string userId)
        {
            var allTeams = _helperTeam.GetTeamsForUser(application, userId);
            foreach (var salesTeam in allTeams.Select(x => x.AsTeam()).ToList())
            {
                var activeUser = (salesTeam.ActiveUsers.SingleOrDefault(x => x.Id == userId));
                if (activeUser != null)
                {
                    salesTeam.ActiveUsers.Remove(activeUser);
                    salesTeam.Save();
                }
            }
        }
    }
}
