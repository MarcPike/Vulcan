using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;
using CrmUser = DAL.Vulcan.Mongo.Core.DocClass.CRM.CrmUser;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperTeam : HelperBase, IHelperTeam
    {
        private readonly IHelperUser _helperUser;
        private readonly HelperCompany _helperCompany;

        public HelperTeam(IHelperUser helperUser)
        {
            _helperUser = helperUser;
            _helperCompany = new HelperCompany();
        }

        public Team GetTeam(string teamId)
        {
            var rep = new RepositoryBase<Team>();
            var salesTeam = rep.AsQueryable().SingleOrDefault(x => x.Id == ObjectId.Parse(teamId));
            if (salesTeam == null) throw new Exception("Team does not exist");
            return salesTeam;
        }

        //public Team AddManagerToTeam(Manager manager, Team team)
        //{
        //    team.AddManager(manager.AsManagerRef());
        //    return team;
        //}

        public Team RemoveUserFromTeam(CrmUser crmUser, Team team, bool withOverride = false)
        {
            if (team.CrmUsers.All(x => x.Id != crmUser.User.Id)) return team;

            if (!withOverride)
            {
                if (team.CrmUsers.Select(x=>x.AsCrmUser()).Count(x => x.UserType == CrmUserType.Director && x.UserType == CrmUserType.Manager) == 1)
                    throw new Exception("There are no other Directors and no Managers for this team. Cannot remove only Director.");
            }

            RemoveTeamActivitesFromUser(crmUser, team);

            var removeUser = team.CrmUsers.SingleOrDefault(x => x.UserId == crmUser.User.Id);
            team.CrmUsers.Remove(removeUser);

            RemoveTeamFromUser(crmUser, team);

            team.SaveToDatabase();

            return team;
        }

        public void RemoveTeamFromUser(CrmUser crmUser, Team team)
        {
            var teamForCrmUser = crmUser.Teams.FirstOrDefault(x => x.Id == team.Id.ToString());
            if (teamForCrmUser != null)
            {
                crmUser.Teams.Remove(teamForCrmUser);
            }
            crmUser.ViewConfig.Team = crmUser.Teams.FirstOrDefault() ?? Team.Helper.Find(x => x.Name == "Houston Sales").FirstOrDefault().AsTeamRef();
            crmUser.SaveToDatabase();

        }

        public Team CreateNewTeam(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            return new Team()
            {
                Location = crmUser.User.AsUser().Location,
                CreatedByUserId = userId
            };
        }


        public Team RemoveCompanyGroupFromTeam(CompanyGroup companyGroup, Team team)
        {
            //var group = team.CompanyGroups.SingleOrDefault(x => x.Id == companyGroup.Id.ToString());
            //if (group != null) team.CompanyGroups.Remove(group);
            //team.SaveToDatabase();

            team.RemoveCompanyGroup(companyGroup.AsCompanyGroupRef());
            return team;
        }

        //public Team AddCompanyToTeam(Company company, Team team)
        //{
        //    var newCompany = team.Companies.SingleOrDefault(x => x.Id == company.Id.ToString());
        //    if (newCompany == null) team.Companies.Add(company.AsCompanyRef());
        //    team.SaveToDatabase();
        //    return team;
        //}

        public Team RemoveCompanyFromTeam(Company company, Team team)
        {

            var foundCompany = team.Companies.SingleOrDefault(x => x.Id == company.Id.ToString());
            if (foundCompany != null) team.Companies.Remove(foundCompany);

            if (company.IsAlliance)
            {
                var allianceFound = team.Alliances.SingleOrDefault(x => x.Id == company.Id.ToString());
                team.Alliances.Remove(allianceFound);
            }

            if (!company.IsAlliance)
            {
                var nonAllianceFound = team.NonAlliances.SingleOrDefault(x => x.Id == company.Id.ToString());
                team.NonAlliances.Remove(nonAllianceFound);
            }

            team.SaveToDatabase();
            return team;
        }

        //public Team AddSalesPersonToTeam(SalesPerson salesPerson, Team team)
        //{
        //    team.AddSalesPerson(salesPerson.AsSalesPersonRef());
        //    team.Save();
        //    return team;
        //}

        public List<TeamRef> GetTeamsForUser(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            return crmUser.Teams.ToList();
        }

        public Stack<TeamMessage> GetTeamMessages(TeamRef team)
        {
            return TeamChannel.GetTeamChannel(team).Messages;
        }

        //public Team AddNewTeam(Manager manager, Location location, string teamName)
        //{
        //    var team = new Team(teamName, location, manager);
        //    return team;
        //}

        public void RemoveTeam(Team team)
        {
            foreach (var crmUserRef in team.CrmUsers.Select(x=>x.AsCrmUser()))
            {
                RemoveUserFromTeam(crmUserRef, team, true);
            }

            foreach (var companyGroupRef in team.CompanyGroups.ToList())
            {
                RemoveCompanyGroupFromTeam(companyGroupRef.AsCompanyGroup(), team);
            }

            foreach (var companyRef in team.Companies.ToList())
            {
                RemoveCompanyFromTeam(companyRef.AsCompany(), team);
            }

            var repGoal = new RepositoryBase<Goal>();
            var repAction = new RepositoryBase<Action>();
            foreach (var goal in team.Goals.Select(x => x.AsGoal()).ToList())
            {
                foreach (var task in goal.Actions.Select(x => x.AsAction()).ToList())
                {
                    repAction.RemoveOne(task);
                }
                repGoal.RemoveOne(goal);
            }

            foreach (var notificationRef in team.Notifications.ToList())
            {
                team.Notifications.RemoveDocumentRef(notificationRef);
                team.SaveToDatabase();
            }

            var rep = new RepositoryBase<Team>();
            rep.RemoveOne(team);
        }

        public List<TeamRef> GetMyTeams(CrmUser crmUser)
        {
            RemoveDeadTeams(crmUser.Teams);
            for (int i = 0; i < crmUser.Teams.Count; i++)
            {
                var team = crmUser.Teams[i].AsTeam();
                crmUser.Teams[i] = team.AsTeamRef();
            }
            ResolveViewConfigTeams(crmUser.ViewConfig, crmUser.Teams);
            crmUser.SaveToDatabase();
            return crmUser.Teams.ToList();
        }

        public List<TeamRef> GetAllTeams()
        {
            var teams = new RepositoryBase<Team>().AsQueryable().OrderBy(x => x.Name).ToList();
            return teams.Select(x=> x.AsTeamRef()).ToList();
        }


        private void ResolveViewConfigTeams(ViewConfig viewConfig, ReferenceList<Team, TeamRef> teams)
        {
            if (teams.Count == 0)
            {
                viewConfig.Team = null;
                viewConfig.ViewType = ViewType.MyStuff;
            }
            else if (viewConfig.Team == null)
            {
                viewConfig.ViewType = ViewType.Team;
                viewConfig.Team = teams.First();
            }

        }

        private void RemoveDeadTeams(ReferenceList<Team, TeamRef> teams)
        {
            foreach (var teamRef in teams.ToList())
            {
                if (teamRef.AsTeam() == null)
                {
                    teams.Remove(teamRef);
                }
            }
        }

        private static void RemoveTeamActivitesFromUser(CrmUser crmUser, Team team)
        {
            foreach (var notification in team.Notifications.ToList())
            {
                crmUser.Notifications.RemoveDocumentRef(notification);
                crmUser.SaveToDatabase();
            }
        }

        public List<UserRef> GetAllUsers(TeamRef teamRef)
        {
            var team = teamRef.AsTeam();
            var result = team.CrmUsers.Select(x=>x.AsCrmUser().User).ToList();
            return result.Distinct().ToList();
        }

        public Team SaveTeam(TeamModel model)
        {

            var helperUser = new HelperUser();
            var user = helperUser.GetUser(model.UserId);


            var rep = new RepositoryBase<Team>();
            var team = rep.Find(model.Id) ?? new Team(model.Name, model.Location.AsLocation(), user, model.Application, ObjectId.Parse(model.Id));

            var previousTeamName = team.Name;
            var teamNameChanged = model.Name != team.Name;

            // Look for Added/Removed CompanyGroups
            var removedCompanyGroups = team.CompanyGroups.Where(x => model.CompanyGroups.All(d => d.Id != x.Id)).ToList();
            var addedCompanyGroups = model.CompanyGroups.Where(x => team.CompanyGroups.All(d => d.Id != x.Id)).ToList();

            // Look for Added/Removed Companies
            var removedCompanies = team.Companies.Where(x => model.Companies.All(d => d.Id != x.Id)).ToList();
            var addedCompanies = model.Companies.Where(x => team.Companies.All(d => d.Id != x.Id)).ToList();

            //// Look for Added/Removed Alliances
            //var removedAlliances = team.Alliances.Where(x => model.Alliances.All(d => d.Id != x.Id)).ToList();
            //var addedAlliances = model.Alliances.Where(x => team.Alliances.All(d => d.Id != x.Id)).ToList();

            //// Look for Added/Removed NonAlliances
            //var removedNonAlliances = team.NonAlliances.Where(x => model.NonAlliances.All(d => d.Id != x.Id)).ToList();
            //var addedNonAlliances = model.NonAlliances.Where(x => team.NonAlliances.All(d => d.Id != x.Id)).ToList();

            // Look for Added/Removed CrmUsers
            var removedUsers = team.CrmUsers.Where(x => model.CrmUsers.All(d => d.Id != x.Id)).ToList();
            var addedUsers = model.CrmUsers.Where(x => team.CrmUsers.All(d => d.Id != x.Id)).ToList();

            // Look for Added/Removed Prospects
            var removedProspects = team.Prospects.Where(x => model.Prospects.All(d => d.Id != x.Id)).ToList();
            var addedProspects = model.Prospects.Where(x => team.Prospects.All(d => d.Id != x.Id)).ToList();

            team.Name = model.Name;
            team.Location = model.Location;
            team.CompanyGroups.ResyncWithList(model.CompanyGroups);
            team.Companies.ResyncWithList(model.Companies);
            team.Prospects.ResyncWithList(model.Prospects);
            team.Notes.ResynchWithList(model.Notes);
            team.ActiveUsers.ResyncWithList(model.ActiveUsers);
            team.Goals.ResyncWithList(model.Goals);
            team.Actions.ResyncWithList(model.Actions);
            team.CrmUsers.ResyncWithList(model.CrmUsers);
            team.Alliances.ResyncWithList(model.Alliances);
            team.NonAlliances.ResyncWithList(model.NonAlliances);
            team.SaveToDatabase();

            var teamRef = team.AsTeamRef();

            if (teamNameChanged)
            {
                foreach (var crmUser in team.CrmUsers.Select(x=>x.AsCrmUser()))
                {
                    var existingTeamRef = crmUser.Teams.SingleOrDefault(x => x.Id == teamRef.Id);
                    if (existingTeamRef == null)
                    {
                        crmUser.Teams.Add(teamRef);
                        crmUser.SaveToDatabase();
                    } else if (existingTeamRef.Name != teamRef.Name)
                    {
                        var indexOf = crmUser.Teams.IndexOf(existingTeamRef);
                        crmUser.Teams[indexOf] = teamRef;
                        crmUser.SaveToDatabase();
                    }

                    if (crmUser.ViewConfig.Team.Id == teamRef.Id)
                    {
                        crmUser.ViewConfig.Team = teamRef;
                        crmUser.SaveToDatabase();
                    }
                }
            }

            

            RemoveTeamUserCompanyViewSelections(removedUsers, teamRef);
            AddTeamUserCompanyViewSelections(addedUsers, teamRef);

            ReSynchAlliancesAndNonAlliancesCompanies(
                team,
                addedCompanyGroups,
                removedCompanyGroups);

            PublishSignalRNotifications(
                team,
                addedUsers, 
                removedUsers, 
                addedCompanyGroups, 
                removedCompanyGroups, 
                addedCompanies, 
                removedCompanies,
                teamNameChanged,
                previousTeamName,
                addedProspects,
                removedProspects);

            //RsSynchTeamUserCompanyViewSelectionsAlliancesAndNonAlliances(addedNonAlliances, removedNonAlliances, addedAlliances, removedAlliances, addedProspects, removedProspects, team, user.AsUserRef());

            //RemoveTeamUserCompanyViewSelections(removedUsers, teamRef);

            //AddTeamUserCompanyViewSelections(addedUsers, teamRef);

            //RemoveFromViewConfigAllRemovedUsers(removedUsers, teamRef);

            //AddTeamToViewConfigForAllAddedUsers(addedUsers,teamRef);

            return team;

        }

        //public TeamPriceTierModel GetTeamPriceTier(Team team, string application, string userId)
        //{
        //    var teamPriceTier = TeamPriceTier.GetForTeam(team);
        //    return new TeamPriceTierModel(teamPriceTier, application, userId);
        //}

        //public TeamPriceTierModel TeamPriceTierAddProductCode(Team team, string productCode, string application, string userId)
        //{
        //    var teamPriceTier = TeamPriceTier.GetForTeam(team);
        //    teamPriceTier.AddProductCode(productCode);
        //    return new TeamPriceTierModel(teamPriceTier, application, userId);
        //}

        //public TeamPriceTierModel SaveTeamPriceTierModel(TeamPriceTierModel model)
        //{
        //    var team = model.Team.AsTeam();
        //    var teamPriceTier = TeamPriceTier.GetForTeam(team);

        //    teamPriceTier.ProductWeightTierPrices = model.ProductWeightTierPrices;
        //    teamPriceTier.SaveToDatabase();
            
        //    return new TeamPriceTierModel(teamPriceTier, model.Application, model.UserId);
        //}

        //private void AddTeamToViewConfigForAllAddedUsers(List<CrmUserRef> addedUsers, TeamRef teamRef)
        //{
        //    foreach (var crmUser in addedUsers.Select(x => x.AsCrmUser()))
        //    {
        //        crmUser.ViewConfig.Team = teamRef;
        //        crmUser.ViewConfig.ViewType = ViewType.Team;
        //        crmUser.SaveToDatabase();
        //    }
        //}

        //private void RemoveFromViewConfigAllRemovedUsers(List<CrmUserRef> removedUsers, TeamRef teamRef)
        //{
        //    foreach (var crmUser in removedUsers.Select(x=>x.AsCrmUser()))
        //    {
        //        crmUser.ViewConfig.Team = crmUser.Teams.FirstOrDefault(x => x.Id != teamRef.Id);
        //        if (crmUser.ViewConfig.Team == null) crmUser.ViewConfig.ViewType = ViewType.MyStuff;
        //        crmUser.SaveToDatabase();
        //    }
        //}

        //public TeamUserCompanyViewSelectionsModel AddNonAllianceCompanyForUser(string application, string userId, string companyId)
        //{
        //    var crmUser = _helperUser.GetCrmUser(application, userId);
        //    var teamRef = crmUser.ViewConfig.Team;

        //    var team = teamRef.AsTeam();
        //    var userSelections = new TeamUserCompanyViewSelections(crmUser.User, teamRef);

        //    return new TeamUserCompanyViewSelectionsModel(application, userId, userSelections);

        //    //var companyRef = _helperCompany.GetCompanyRef(companyId);
        //    ////userCompanySelectionModel.NonAlliances.Add();

        //    //var teamModel = new TeamModel(teamRef.AsTeam(),application,userId);


        //    //if (teamModel.NonAlliances.All(x => x.Id != companyId))
        //    //{
        //    //    teamModel.NonAlliances.Add(companyRef);
        //    //}
        //    //var team = SaveTeam(teamModel);

        //    //var userSelections = TeamUserCompanyViewSelections.GetTeamUserCompanyViewSelections(crmUser.User, teamRef);
        //    //return new TeamUserCompanyViewSelectionsModel(application,userId,userSelections); 
        //}

        //public TeamUserCompanyViewSelectionsModel RemoveNonAllianceCompanyForUser(string application, string userId, string companyId)
        //{
        //    var crmUser = _helperUser.GetCrmUser(application, userId);
        //    var teamRef = crmUser.ViewConfig.Team;

        //    var companyRef = _helperCompany.GetCompanyRef(companyId);
        //    //userCompanySelectionModel.NonAlliances.Add();

        //    var teamModel = new TeamModel(teamRef.AsTeam(), application, userId);

        //    var removeNonAlliance = teamModel.NonAlliances.SingleOrDefault(x => x.Id == companyId);

        //    if (removeNonAlliance != null) 
        //    {
        //        teamModel.NonAlliances.Remove(removeNonAlliance);
        //    }
        //    var team = SaveTeam(teamModel);

        //    var teamUserCompanyViewSelections = new TeamUserCompanyViewSelections(crmUser.User, teamRef);
        //    var teamUserCompanyViewSelectionsModel = new TeamUserCompanyViewSelectionsModel(application, userId, teamUserCompanyViewSelections);

        //    var nonAllianceFound =
        //        teamUserCompanyViewSelections.NonAlliances.SingleOrDefault(x => x.Company.Id == companyId);

        //    if (nonAllianceFound != null)
        //    {
        //        throw new Exception("Team Save: Logic failed to remove Non-Alliance company to users");
        //    }

        //    return _helperUser.SaveTeamUserCompanyViewSelectionsModel(teamUserCompanyViewSelectionsModel);
        //}

        private static void PublishSignalRNotifications(
            Team team, List<CrmUserRef> addedUsers, List<CrmUserRef> removedUsers,
            List<CompanyGroupRef> addedCompanyGroups, List<CompanyGroupRef> removedCompanyGroups, 
            List<CompanyRef> addedCompanies, List<CompanyRef> removedCompanies, bool teamNameChanged, string previousTeamName, List<ProspectRef> addedProspects, List<ProspectRef> removedProspects)
        {

            // SignalR director changes
            foreach (var crmUserRef in addedUsers)
            {
                NotificationRouter.UserAddedToTeam(crmUserRef, team);
                NotificationRouter.YourTeamSettingsHaveChanged(crmUserRef);
            }
            foreach (var crmUserRef in removedUsers)
            {
                NotificationRouter.UserRemovedFromTeam(crmUserRef, team);
                NotificationRouter.YourTeamSettingsHaveChanged(crmUserRef);
            }

            // SignalR CompanyGroup changes
            foreach (var addedCompanyGroup in addedCompanyGroups)
            {
                NotificationRouter.CompanyGroupAddedToTeam(team, addedCompanyGroup.AsCompanyGroup());
            }
            foreach (var removedCompanyGroup in removedCompanyGroups)
            {
                NotificationRouter.CompanyGroupRemovedFromTeam(team, removedCompanyGroup.AsCompanyGroup());
            }

            //// SignalR CompanyGroup changes
            //foreach (var addedCompany in addedCompanies)
            //{
            //    NotificationRouter.CompanyAddedToTeam(team, addedCompany.AsCompany());
            //}
            //foreach (var removedCompany in removedCompanies)
            //{
            //    NotificationRouter.CompanyRemovedFromTeam(team, removedCompany.AsCompany());
            //}

            if (teamNameChanged)
            {
                NotificationRouter.TeamNameChanged(team, previousTeamName);
            }

            foreach (var addedProspect in addedProspects)
            {
                NotificationRouter.ProspectAddedToTeam(team, addedProspect.AsProspect());
            }

            foreach (var removedProspect in removedProspects)
            {
                NotificationRouter.ProspectRemovedFromTeam(team, removedProspect.AsProspect());
            }
        }

        private void ReSynchAlliancesAndNonAlliancesCompanies(
            Team team,
            List<CompanyGroupRef> addedCompanyGroups, 
            List<CompanyGroupRef> removedCompanyGroups)
        {
            if ((addedCompanyGroups.Count + removedCompanyGroups.Count) == 0) return;

            team.RefreshTeamCompaniesList();
            team.RefreshAllianceNonAllianceLists();

            //team.RefreshAllianceNonAllianceLists();

            team.SaveToDatabase();

            //LookForNewCompaniesFoundInNewCompanyGroups(team, addedCompanyGroups, addedAlliances, addedNonAlliances);
            //LookForRemovedCompaniesFoundInRemovedCompanyGroups(team, removedCompanyGroups, removedAlliances, removedNonAlliances);
        }

        //private static void LookForRemovedCompaniesFoundInRemovedCompanyGroups(Team team, List<CompanyGroupRef> removedCompanyGroups,
        //    List<CompanyRef> removedAlliances, List<CompanyRef> removedNonAlliances)
        //{
        //    foreach (var removedCompanyGroup in removedCompanyGroups.Select(x => x.AsCompanyGroup()).ToList())
        //    {
        //        var companies = removedCompanyGroup.GetAllCompanies();
        //        foreach (var removedCompany in companies)
        //        {
        //            if (removedCompanyGroup.IsAlliance)
        //            {
        //                RemoveAlliance(team, removedAlliances, removedCompany);
        //            }
        //            else
        //            {
        //                RemoveNonAlliance(team, removedNonAlliances, removedCompany);
        //            }
        //        }
        //    }
        //}

        //private static void RemoveNonAlliance(Team team, List<CompanyRef> removedNonAlliances, CompanyRef removedCompany)
        //{
        //    var removeThis = team.NonAlliances.SingleOrDefault(x => x.Id != removedCompany.Id);
        //    {
        //        removedNonAlliances.Add(removedCompany);
        //        team.NonAlliances.Remove(removeThis);
        //        team.SaveToDatabase();
        //    }
        //}

        //private static void RemoveAlliance(Team team, List<CompanyRef> removedAlliances, CompanyRef removedCompany)
        //{
        //    var removeThis = team.Alliances.SingleOrDefault(x => x.Id != removedCompany.Id);
        //    {
        //        removedAlliances.Add(removedCompany);
        //        team.Alliances.Remove(removeThis);
        //        team.SaveToDatabase();
        //    }
        //}

        //private static void LookForNewCompaniesFoundInNewCompanyGroups(Team team, List<CompanyGroupRef> addedCompanyGroups, List<CompanyRef> addedAlliances,
        //    List<CompanyRef> addedNonAlliances)
        //{
        //    foreach (var newCompanyGroup in addedCompanyGroups.Select(x => x.AsCompanyGroup()).ToList())
        //    {
        //        var companies = newCompanyGroup.GetAllCompanies();
        //        foreach (var newCompany in companies)
        //        {
        //            if (newCompanyGroup.IsAlliance)
        //            {
        //                AddAllianceToTeam(team, addedAlliances, newCompany);
        //            }
        //            else
        //            {
        //                AddNonAllianceToTeam(team, addedNonAlliances, newCompany);
        //            }
        //        }
        //    }
        //}

        //private static void AddNonAllianceToTeam(Team team, List<CompanyRef> addedNonAlliances, CompanyRef newCompany)
        //{
        //    if (team.NonAlliances.All(x => x.Id != newCompany.Id))
        //    {
        //        addedNonAlliances.Add(newCompany);
        //        team.NonAlliances.Add(newCompany);
        //        team.SaveToDatabase();
        //    }
        //}

        //private static void AddAllianceToTeam(Team team, List<CompanyRef> addedAlliances, CompanyRef newCompany)
        //{
        //    if (team.Alliances.All(x => x.Id != newCompany.Id))
        //    {
        //        addedAlliances.Add(newCompany);
        //        team.Alliances.Add(newCompany);
        //        team.SaveToDatabase();
        //    }
        //}

        //private static void RsSynchTeamUserCompanyViewSelectionsAlliancesAndNonAlliances(List<CompanyRef> addedNonAlliances, List<CompanyRef> removedNonAlliances, List<CompanyRef> addedAlliances,
        //    List<CompanyRef> removedAlliances, List<ProspectRef> addedProspects, List<ProspectRef> removedProspects, Team team, UserRef userRef)
        //{
        //    if ((
        //            addedNonAlliances.Count +
        //            removedNonAlliances.Count +
        //            addedAlliances.Count +
        //            removedAlliances.Count +
        //            addedProspects.Count +
        //            removedProspects.Count) > 0)
        //    {
        //        TeamUserCompanyViewSelections.ReSynchTeam(team.AsTeamRef(), userRef);
        //    }
        //}

        private static void RemoveTeamUserCompanyViewSelections(List<CrmUserRef> removedUsers, TeamRef teamRef)
        {
            foreach (var crmUser in removedUsers.Select(x=>x.AsCrmUser()))
            {
                var removeTeam = crmUser.Teams.SingleOrDefault(x => x.Id == teamRef.Id);
                if (removeTeam != null)
                {
                    crmUser.Teams.Remove(removeTeam);
                }

                if (crmUser.Teams.Any())
                {
                    crmUser.ViewConfig.Team = teamRef;
                    crmUser.ViewConfig.ViewType = ViewType.Team;
                }
                else
                {
                    crmUser.ViewConfig.Team = null;
                    crmUser.ViewConfig.ViewType = ViewType.MyStuff;
                }
                crmUser.SaveToDatabase();
            }

        }

        private static void AddTeamUserCompanyViewSelections(List<CrmUserRef> addedUsers, TeamRef teamRef)
        {
            foreach (var crmUser in addedUsers.Select(x => x.AsCrmUser()))
            {

                crmUser.ViewConfig.Team = teamRef;
                crmUser.ViewConfig.ViewType = ViewType.Team;
                crmUser.Teams.Add(teamRef);
                crmUser.SaveToDatabase();
            }


        }
    }
}