using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DocumentFormat.OpenXml.Office2010.Excel;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class MergeTeams
    {
        private List<string> TeamsToCombine = new List<string>()
        {
            "Cumbernauld Sales Team",
            "Sheffield Sales Team"
        };

        private string NewTeamName = "UK Sales Team";
        private string NewTeamOffice = "Sheffield";
        //private List<string> OtherLocations = new List<string>()
        //{
        //    "Cumbernauld"
        //};

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }



        [Test]
        public void Execute()
        {
            
            //var otherLocationsList = new List<LocationRef>();
            //foreach (var otherLocationOffice in OtherLocations)
            //{
            //    var otherLocation = Location.Helper.
            //        Find(x => x.Office == otherLocationOffice).Single();
            //    otherLocationsList.Add(otherLocation.AsLocationRef());
            //}

            var createdBy = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();

            var teamList = new List<Team>();
            foreach (var teamName in TeamsToCombine)
            {
                var team = Team.Helper.Find(x => x.Name == teamName).Single();
                teamList.Add(team);
            }

            var location = Location.Helper.
                Find(x => x.Office == NewTeamOffice).Single();

            var newTeam = new Team()
            {
                Name = NewTeamName,
                Location = location.AsLocationRef(),
                CreatedByUserId = createdBy.UserId
                //OtherLocations = otherLocationsList
            };
            Team.Helper.Upsert(newTeam);

            foreach (var team in teamList)
            {
                GetCrmUsers(team);

                GetCompanies(team);

                GetCompanyGroups(team);

                GetAlliances(team);

                GetNonAlliances(team);

                GetProspects(team);

                Team.Helper.Upsert(newTeam);
                MoveQuotes(team);

                //Team.Helper.DeleteOne(team.Id);

            }
            UpdateCrmUsersViewConfig();

            Console.WriteLine(ObjectDumper.Dump(newTeam));

            void GetCrmUsers(Team team)
            {
                foreach (var teamCrmUser in team.CrmUsers)
                {
                    if (newTeam.CrmUsers.All(x => x.Id != teamCrmUser.Id))
                    {
                        newTeam.CrmUsers.Add(teamCrmUser);
                    }
                }
            }

            void GetCompanies(Team team)
            {
                foreach (var teamCompany in team.Companies)
                {
                    if (newTeam.Companies.All(x => x.Id != teamCompany.Id))
                    {
                        newTeam.Companies.Add(teamCompany);
                    }
                }
            }

            void GetCompanyGroups(Team team)
            {
                foreach (var teamCompanyGroup in team.CompanyGroups)
                {
                    if (newTeam.CompanyGroups.All(x => x.Id != teamCompanyGroup.Id))
                    {
                        newTeam.CompanyGroups.Add(teamCompanyGroup);
                    }
                }
            }

            void GetAlliances(Team team)
            {
                foreach (var teamAlliance in team.Alliances)
                {
                    if (newTeam.Alliances.All(x => x.Id != teamAlliance.Id))
                    {
                        newTeam.Alliances.Add(teamAlliance);
                    }
                }
            }

            void GetNonAlliances(Team team)
            {
                foreach (var teamNonAlliance in team.NonAlliances)
                {
                    if (newTeam.NonAlliances.All(x => x.Id != teamNonAlliance.Id))
                    {
                        newTeam.NonAlliances.Add(teamNonAlliance);
                    }
                }
            }

            void GetProspects(Team team)
            {
                foreach (var teamProspect in team.Prospects)
                {
                    if (newTeam.Prospects.All(x => x.Id != teamProspect.Id))
                    {
                        newTeam.Prospects.Add(teamProspect);
                    }
                }
            }

            void MoveQuotes(Team team)
            {
                var oldTeamRef = team.AsTeamRef();
                var newTeamRef = newTeam.AsTeamRef();
                var teamFilter = CrmQuote.Helper.FilterBuilder.Where(x => x.Team.Id == oldTeamRef.Id);
                var teamUpdate = CrmQuote.Helper.UpdateBuilder.
                                Set(x => x.Team, newTeamRef).
                                Set(x => x.ModifiedDateTime, DateTime.Now);
                CrmQuote.Helper.UpdateMany(teamFilter, teamUpdate);
            }
        }


        [Test]
        public void LookForInvalidQuotes()
        {
            var teamList = new List<Team>();
            foreach (var teamName in TeamsToCombine)
            {
                var team = Team.Helper.Find(x => x.Name == teamName).Single().AsTeamRef();
                var filter = CrmQuote.Helper.FilterBuilder.Where(x => x.Team.Id == team.Id);
                var quotes = CrmQuote.Helper.Find(filter).ToList();
                if (quotes.Any())
                {
                    Console.WriteLine($"{quotes.Count} quotes found for Team: {teamName}");
                }
            }
        }

        [Test]
        public void UpdateCrmUsersViewConfig()
        {
            var removedTeams = new List<Team>();
            foreach (var teamName in TeamsToCombine)
            {
                var team = Team.Helper.Find(x => x.Name == teamName).Single();
                removedTeams.Add(team);
            }

            var newTeam = Team.Helper.Find(x => x.Name == NewTeamName).First();

            var createdBy = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();
            newTeam.CreatedByUserId = createdBy.UserId;

            foreach (var newTeamCrmUser in newTeam.CrmUsers)
            {
                var crmUser = newTeamCrmUser.AsCrmUser();
                crmUser.ViewConfig.Team = newTeam.AsTeamRef();

                foreach (var removedTeam in removedTeams)
                {
                    var removedTeamRef = removedTeam.AsTeamRef();
                    var removeTeamRef = crmUser.Teams.FirstOrDefault(x => x.Id == removedTeamRef.Id);
                    if (removeTeamRef != null)
                    {
                        crmUser.Teams.Remove(removeTeamRef);
                    }
                }

                if (crmUser.Teams.All(x=>x.Id != newTeam.Id.ToString()))
                {
                    crmUser.Teams.Add(newTeam.AsTeamRef());
                }

                Mongo.DocClass.CRM.CrmUser.Helper.Upsert(crmUser);
            }


        }

        [Test]
        public void SetCreatedBy()
        {
            var newTeam = Team.Helper.Find(x => x.Name == NewTeamName).First();

            var createdBy = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();
            newTeam.CreatedByUserId = createdBy.UserId;
            Team.Helper.Upsert(newTeam);

        }
    }
}
