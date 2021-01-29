using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.TeamSettings;
using CrmUser = DAL.Vulcan.Mongo.Core.DocClass.CRM.CrmUser;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperTeam
    {
        Team GetTeam(string teamId);

        //Team AddManagerToTeam(Manager manager, Team team);
        Team RemoveUserFromTeam(CrmUser crmUser, Team team, bool forceOverride=false);

        //Team AddCompanyGroupToTeam(CompanyGroup companyGroup, Team team);
        Team RemoveCompanyGroupFromTeam(CompanyGroup companyGroup, Team team);

        //Team AddCompanyToTeam(Company company, Team team);
        Team RemoveCompanyFromTeam(Company company, Team team);

        Team CreateNewTeam(string application, string userId);

        List<TeamRef> GetTeamsForUser(string application, string userId);

        Stack<TeamMessage> GetTeamMessages(TeamRef team);
        //Team AddNewTeam(Manager manager, Location location, string teamName);
        void RemoveTeam(Team team);

        List<TeamRef> GetMyTeams(CrmUser crmUser);
        List<TeamRef> GetAllTeams();

        List<UserRef> GetAllUsers(TeamRef teamRef);

        Team SaveTeam(TeamModel model);

        //TeamPriceTierModel GetTeamPriceTier(Team team, string application, string userId);

        //TeamPriceTierModel TeamPriceTierAddProductCode(Team team, string productCode, string application,
        //    string userId);

        //TeamPriceTierModel SaveTeamPriceTierModel(TeamPriceTierModel model);

        //TeamUserCompanyViewSelectionsModel AddNonAllianceCompanyForUser(string application, string userId, string companyId);

        //TeamUserCompanyViewSelectionsModel RemoveNonAllianceCompanyForUser(string application, string userId,
        //    string companyId);
    }
}