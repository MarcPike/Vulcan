using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class AddNorwaySalesDueToBug
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void AddNorwaySalesTeamToProduction()
        {
            var teamModel = new TeamModel();
            var helperUser = new HelperUser(new HelperPerson());

            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();

            teamModel.Name = "Norway Sales Team";
            teamModel.Location = Location.Helper.Find(x => x.Office == "Norway").First().AsLocationRef();
            teamModel.CreateByUserId = crmUser.UserId;
            teamModel.Application = "vulcancrm";
            teamModel.UserId = crmUser.UserId;
            teamModel.Id = new Team().Id.ToString();


            var helperTeam = new HelperTeam(helperUser);

            helperTeam.SaveTeam(teamModel);


        }

        [Test]
        public void GetNewTeamModel()
        {
            var team = Team.Helper.Find(x => x.Name == "Norway Sales Team").First();

            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();
            //team.CrmUsers.Add(crmUser.AsCrmUserRef());
            //Team.Helper.Upsert(team);

            crmUser.Teams.Add(team.AsTeamRef());
            Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);

            var teamModel = new TeamModel(team, "vulcancrm", crmUser.UserId);
        }

        [Test]
        public void GetAllTeams()
        {
            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();

            var helperUser = new HelperUser(new HelperPerson());
            var helperTeam = new HelperTeam(helperUser);

            var allTeams = helperTeam.GetAllTeams();
            var myTeams = helperTeam.GetMyTeams(crmUser);


        }

    }

}
