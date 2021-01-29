using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    public static class LoadMissingCompaniesForAllGroups
    {
        public static (List<CompanyRef> AllianceCompaniesAdded, List<CompanyRef> NonAllianceCompaniesAdded, List<TeamRef> TeamsUpdated) Execute()
        {
            var repTeam = new RepositoryBase<Team>();
            var repCompanyGroup = new RepositoryBase<CompanyGroup>();
            var allianceCompaniesAdded = new List<CompanyRef>();
            var nonAllianceCompaniesAdded = new List<CompanyRef>();
            var teamsUpdated = new List<TeamRef>();

            foreach (var companyGroup in repCompanyGroup.AsQueryable().Where(x => x.IsAlliance).ToList())
            {
                var newCompanies = companyGroup.RefreshAllianceCompanyList(repCompanyGroup);
                if (newCompanies.Any())
                {
                    var teamsUsingThisGroup = repTeam.AsQueryable()
                        .Where(x => x.CompanyGroups.Any(g => g.Id == companyGroup.Id.ToString()));

                    foreach (var team in teamsUsingThisGroup)
                    {
                        team.Companies.AddListOfReferenceObjects(newCompanies);
                        team.SaveToDatabase();
                        teamsUpdated.Add(team.AsTeamRef());

                    }
                }
                allianceCompaniesAdded.AddRange(newCompanies);
                
            }
            foreach (var companyGroup in repCompanyGroup.AsQueryable().Where(x => x.IsAlliance == false).ToList())
            {

                var newCompanies = companyGroup.RefreshNonAllianceCompanyList(repCompanyGroup);
                if (newCompanies.Any())
                {
                    var teamsUsingThisGroup = repTeam.AsQueryable()
                        .Where(x => x.CompanyGroups.Any(g => g.Id == companyGroup.Id.ToString()));

                    foreach (var team in teamsUsingThisGroup)
                    {
                        team.Companies.AddListOfReferenceObjects(newCompanies);
                        team.SaveToDatabase();
                        teamsUpdated.Add(team.AsTeamRef());
                    }
                }
                nonAllianceCompaniesAdded.AddRange(newCompanies);
            }

            return (allianceCompaniesAdded, nonAllianceCompaniesAdded, teamsUpdated);
        }

    }
}
