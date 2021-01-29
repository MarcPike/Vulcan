using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class TeamUserCompanyViewSelections : BaseDocument
    {
        public UserRef User { get; set; }
        public TeamRef Team { get; set; }
        public List<CompanySelection> Alliances { get; set; } = new List<CompanySelection>();
        public List<CompanySelection> NonAlliances { get; set; } = new List<CompanySelection>();
        public List<ProspectSelection> Prospects { get; set; } = new List<ProspectSelection>();
        public TeamUserCompanyViewSelections()
        {
        }

        public static void ReSynchTeam(TeamRef teamRef, UserRef userRef)
        {
            var team = teamRef.AsTeam();
            var alliances = team.Alliances.Select(x => new CompanySelection() {Company = x, Selected = true}).ToList();
            var nonAlliances = team.NonAlliances.Select(x => new CompanySelection() { Company = x, Selected = true }).ToList();
            var prospects = team.Prospects.Select(x => new ProspectSelection() { Prospect = x, Selected = false }).ToList();
            var teamUserViews = new RepositoryBase<TeamUserCompanyViewSelections>().AsQueryable().Where(x=>x.Team.Id == teamRef.Id).ToList();

            // Search for missing users
            foreach (var crmUserRef in team.CrmUsers.ToList())
            {
                if (teamUserViews.All(x => x.User.Id != crmUserRef.UserId))
                {
                    var newTeamUserCompanyViewSelections =
                        new TeamUserCompanyViewSelections(crmUserRef.AsCrmUser().User, teamRef);
                    newTeamUserCompanyViewSelections.SaveToDatabase();
                    teamUserViews.Add(newTeamUserCompanyViewSelections);
                }
            }
            foreach (var userView in teamUserViews)
            {
                // Add new Alliances
                foreach (var newAlliance in alliances)
                {
                    if (userView.Alliances.Any(x => x.Company.Id == newAlliance.Company.Id)) continue;
                    userView.Alliances.Add(newAlliance);
                    userView.SaveToDatabase();
                }

                // Remove invalid Alliances
                foreach (var removeAlliance in userView.Alliances)
                {
                    if (alliances.All(x => x.Company.Id != removeAlliance.Company.Id))
                    {
                        userView.Alliances.Remove(removeAlliance);
                        userView.SaveToDatabase();
                    }
                }

                // Add new NonAlliances
                foreach (var newNonAlliance in nonAlliances)
                {
                    if (userView.NonAlliances.Any(x => x.Company.Id == newNonAlliance.Company.Id)) continue;
                    userView.NonAlliances.Add(newNonAlliance);
                    userView.SaveToDatabase();
                }

                // Remove invalid NonAlliances
                foreach (var removeNonAlliance in userView.NonAlliances)
                {
                    if (nonAlliances.All(x => x.Company.Id != removeNonAlliance.Company.Id))
                    {
                        userView.NonAlliances.Remove(removeNonAlliance);
                        userView.SaveToDatabase();
                    }
                }

                // Add new Prospects
                foreach (var newProspect in prospects)
                {
                    if (userView.Prospects.Any(x => x.Prospect.Id == newProspect.Prospect.Id)) continue;

                    userView.Prospects.Add(newProspect);
                    userView.SaveToDatabase();
                }

                // Remove invalid Prospects
                foreach (var removeProspect in userView.Prospects)
                {
                    if (prospects.All(x => x.Prospect.Id != removeProspect.Prospect.Id))
                    {
                        userView.Prospects.Remove(removeProspect);
                        userView.SaveToDatabase();
                    }
                }

            }
        }

        public static TeamUserCompanyViewSelections GetTeamUserCompanyViewSelections(UserRef userRef, TeamRef teamRef)
        {
            return new RepositoryBase<TeamUserCompanyViewSelections>().AsQueryable()
                       .SingleOrDefault(x => x.User.Id == userRef.Id && x.Team.Id == teamRef.Id) ??
                   new TeamUserCompanyViewSelections(userRef, teamRef);
        }

        public static void RemoveTeamUserCompanyViewSelectionsFor(UserRef userRef, TeamRef teamRef)
        {
            var rep = new RepositoryBase<TeamUserCompanyViewSelections>();
            var removeThis = rep.AsQueryable().
                SingleOrDefault(x => x.User.Id == userRef.Id && x.Team.Id == teamRef.Id);
            if (removeThis != null) rep.RemoveOne(removeThis);
        }

        public TeamUserCompanyViewSelections(UserRef userRef, TeamRef teamRef)
        {
            var team = teamRef.AsTeam();
            User = userRef;
            Team = teamRef;
            foreach (var companyRef in team.Alliances)
            {
                Alliances.Add(new CompanySelection()
                {
                    Company = companyRef,
                    Selected = true
                });
            }

            foreach (var companyRef in team.NonAlliances)
            {
                NonAlliances.Add(new CompanySelection()
                {
                    Company = companyRef,
                    Selected = true
                });
            }
            foreach (var teamProspect in team.Prospects)
            {
                Prospects.Add(new ProspectSelection()
                {
                    Prospect = teamProspect,
                    Selected = false
                });
            }
            
        }
    }


}