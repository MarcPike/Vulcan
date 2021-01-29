using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Importers
{
    public static class UpdateTeamCompaniesThatWereModifiedDuringImport
    {
        public static void Execute(List<Company> companies)
        {
            foreach (var company in companies)
            {
                var companyId = company.Id.ToString();

                foreach (var team in Team.Helper.Find(x => x.Companies.Any(c => c.Id == companyId)).ToList())
                {
                    var teamComp = team.Companies.First(x => x.Id == companyId);
                    teamComp.Code = company.Code;
                    teamComp.Name = company.Name;
                    teamComp.ShortName = company.ShortName;
                    teamComp.IsActive = company.IsActive;
                    var indexOf = team.Companies.IndexOf(teamComp);
                    team.Companies[indexOf] = teamComp;

                    var teamAlliance = team.Alliances.FirstOrDefault(x => x.Id == companyId);
                    if (teamAlliance != null)
                    {
                        teamAlliance.Code = company.Code;
                        teamAlliance.Name = company.Name;
                        teamAlliance.ShortName = company.ShortName;
                        teamAlliance.IsActive = company.IsActive;
                        indexOf = team.Alliances.IndexOf(teamAlliance);
                        team.Alliances[indexOf] = teamAlliance;
                    } 
                        

                    var teamNonAlliance = team.NonAlliances.FirstOrDefault(x => x.Id == companyId);
                    if (teamNonAlliance != null)
                    {
                        teamNonAlliance.Code = company.Code;
                        teamNonAlliance.Name = company.Name;
                        teamNonAlliance.ShortName = company.ShortName;
                        teamNonAlliance.IsActive = company.IsActive;
                        indexOf = team.NonAlliances.IndexOf(teamNonAlliance);
                        team.NonAlliances[indexOf] = teamNonAlliance;
                    }
                    Team.Helper.Upsert(team);

                }


            }
        }
    }
}