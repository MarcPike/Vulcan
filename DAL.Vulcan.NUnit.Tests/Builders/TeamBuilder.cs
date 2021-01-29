using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.NUnit.Tests.Builders
{
    public static class TeamBuilder
    {
        public const string TeamName = "Team Test";
        private static readonly RepositoryBase<Team> Repository = new RepositoryBase<Team>();
        private static readonly HelperTeam HelperTeam = new HelperTeam(new HelperUser(new HelperPerson()));

        public static TeamModel GetTeamTest(string application, string networkId)
        {
            var crmUser = CrmUserBuilder.GetCrmUser(networkId);
            var teamFound = Repository.AsQueryable().SingleOrDefault(x => x.Name == TeamName);
            if (teamFound == null)
            {
                teamFound = HelperTeam.CreateNewTeam(crmUser.Application, crmUser.User.Id);
                teamFound.Name = "Team Test";
                teamFound.SaveToDatabase();
            }
            return new TeamModel(teamFound,application,crmUser.User.Id);
        }

        public static TeamModel SaveTeamTest(TeamModel model)
        {
            var team = HelperTeam.SaveTeam(model);
            return new TeamModel(team,model.Application,model.UserId);
        }

        public static void RemoveTeamTest(TeamModel model)
        {
            var team = Repository.AsQueryable().SingleOrDefault(x => x.Name == TeamName);
            if (team == null) return;
            HelperTeam.RemoveTeam(team);
        }

    }
}
