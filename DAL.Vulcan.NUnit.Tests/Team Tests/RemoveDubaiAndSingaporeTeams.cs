using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class RemoveDubaiAndSingaporeTeams
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var removeTeams = Team.Helper.Find(x => x.Name.StartsWith("Dubai") || x.Name.StartsWith("Singapore"))
                .ToList();

            var renameTeams = removeTeams.Where(x => x.Name == "Singapore Baker" || x.Name == "Dubai Baker").ToList();

            foreach (var team in removeTeams.ToList())
            {
                if (renameTeams.Any(x => x.Id == team.Id))
                {
                    removeTeams.Remove(team);
                }
            }

            Console.WriteLine("Renaming these teams");
            foreach (var renameTeam in renameTeams)
            {
                if (renameTeam.Name.StartsWith("Dubai"))
                {
                    RenameTeam(renameTeam, "Dubai Sales");
                }

                if (renameTeam.Name.StartsWith("Singapore"))
                {
                    RenameTeam(renameTeam, "Singapore Sales");
                }

            }

            Console.WriteLine("");

            Console.WriteLine("Removing these teams");
            foreach (var removeTeam in removeTeams)
            {
                RemoveTeam(removeTeam);
            }



        }

        private void RemoveTeam(Team team)
        {
            Console.WriteLine($"Removing Team [{team.Name}]");
            var teamRef = team.AsTeamRef();
            foreach (var teamCrmUser in team.CrmUsers)
            {
                var crmUser = teamCrmUser.AsCrmUser();

                var removeTeamRef = crmUser.Teams.FirstOrDefault(x => x.Id == teamRef.Id);
                if (removeTeamRef != null)
                {
                    crmUser.Teams.Remove(removeTeamRef);
                }


                if (crmUser.ViewConfig.Team.Id == team.Id.ToString())
                {
                    if (crmUser.Teams.Any())
                    {
                        crmUser.ViewConfig.Team = crmUser.Teams.First();
                    }
                    else
                    {
                        crmUser.ViewConfig.Team = null;
                        crmUser.ViewConfig.ViewType = ViewType.MyStuff;
                    }
                }

                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);
            }
        }

        private void RenameTeam(Team team, string newName)
        {
            Console.WriteLine($"Renaming Team [{team.Name}] to [{newName}] ");

            team.Name = newName;
            Team.Helper.Upsert(team);
            var teamRef = team.AsTeamRef();
            foreach (var teamCrmUser in team.CrmUsers)
            {
                var crmUser = teamCrmUser.AsCrmUser();

                if (crmUser.ViewConfig.Team.Id == teamRef.Id)
                {
                    crmUser.ViewConfig.Team = teamRef;
                }

                var replaceTeamRef = crmUser.Teams.FirstOrDefault(x => x.Id == teamRef.Id);
                if (replaceTeamRef != null)
                {
                    var index = crmUser.Teams.IndexOf(replaceTeamRef);
                    crmUser.Teams[index] = teamRef;
                }

                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);

            }

        }
    }
}
