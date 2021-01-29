using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Messages;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Helpers;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class Team: BaseDocument
    {
        public static MongoRawQueryHelper<Team> Helper = new MongoRawQueryHelper<Team>();
        public string Name { get; set; }
        public LocationRef Location { get; set; }

        public ReferenceList<CompanyGroup,CompanyGroupRef> CompanyGroups { get; set; } = new ReferenceList<CompanyGroup, CompanyGroupRef>();
        public ReferenceList<Company,CompanyRef> Companies { get; set; } = new ReferenceList<Company, CompanyRef>();
        public ReferenceList<Company, CompanyRef> Alliances { get; set; } = new ReferenceList<Company, CompanyRef>();
        public ReferenceList<Company, CompanyRef> NonAlliances { get; set; } = new ReferenceList<Company, CompanyRef>();
        public ReferenceList<Prospect, ProspectRef> Prospects { get; set; } = new ReferenceList<Prospect, ProspectRef>();

        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();
        public ReferenceList<Notification,NotificationRef> Notifications { get; set; } = new ReferenceList<Notification, NotificationRef>();
        public ReferenceList<LdapUser, UserRef> ActiveUsers { get; set; } = new ReferenceList<LdapUser, UserRef>();

        public ReferenceList<Goal, GoalRef> Goals { get; set; } = new ReferenceList<Goal, GoalRef>();
        public ReferenceList<Action,ActionRef> Actions { get; set; } = new ReferenceList<Action, ActionRef>();
        public ReferenceList<CrmUser, CrmUserRef> CrmUsers { get; set; } = new ReferenceList<CrmUser, CrmUserRef>();

        public List<MessageObject> Messages { get; set; } = new List<MessageObject>();

        public string DefaultCurrency
        {
            get
            {


                var location = Location.AsLocation();

                if (location == null) return "USD";

                if (location.Office == "Norway") return "NOK";

                var coid = location.GetCoid();
                if (coid == string.Empty) return string.Empty;

                // This has not been included in any builds before 9/16/2020, but I think it should
                // I noticed it today so if it ever comes up, I can uncomment these lines
                // Otherwise USD will be the default for SIN, MSA and DUB, which may be right???

                //if (coid == "SIN") return "SGD";
                //if (coid == "MSA") return "MYR";
                //if (coid == "DUB") return "AED";

                if (coid == "EUR") return "GBP";
                if (coid == "CAN") return "CAD";
                return "USD";

            }
        }

        public string Coid
        {
            get
            {
                return Location.AsLocation().GetCoid();
            }
        }

        //public List<LocationRef> OtherLocations { get; set; } = new List<LocationRef>();

        public void AddGoal(Goal goal)
        {
            Goals.Add(goal.AsGoalRef());
            this.Save();
            goal.Team = this.AsTeamRef();
            goal.Save();
        }

        public List<Action> GetAllOpenActions()
        {
            var result = new List<Action>();
            foreach (var user in CrmUsers.Select(x=>x.AsCrmUser()))
            {

                result.AddRange(user.Actions.Select(x=>x.AsAction()).Where(x=>x.IsCompleted == false).ToList());
            }
            return result.OrderByDescending(x=>x.DueDate).ToList();
        }

        public List<Action> GetAllCompletedActions()
        {
            var result = new List<Action>();
            foreach (var user in CrmUsers.Select(x=>x.AsCrmUser()).ToList())
            {
                result.AddRange(user.Actions.Select(x=>x.AsAction()).Where(x => x.IsCompleted == true).ToList());
            }
            return result.OrderByDescending(x => x.DueDate).ToList();
        }

        public Team()
        {
            
        }

        public void AddCompanyGroup(CompanyGroupRef groupRef)
        {
            RemoveCompanyGroup(groupRef);

            var group = groupRef.AsCompanyGroup();
            //var groupCompanies = group.GetAllCompanies();

            CompanyGroups.Add(groupRef);

            RefreshTeamCompaniesList();
            RefreshAllianceNonAllianceLists();

            SaveToDatabase();
        }

        public void RemoveCompanyGroup(CompanyGroupRef groupRef)
        {
            var group = groupRef.AsCompanyGroup();

            if (CompanyGroups.Any(x => x.Id == groupRef.Id))
            {
                var removeGroup = CompanyGroups.Single(x => x.Id == groupRef.Id);
                CompanyGroups.Remove(removeGroup);

                RefreshTeamCompaniesList();
                RefreshAllianceNonAllianceLists();
            }

            SaveToDatabase();

        }

        public Team(string name, Location location, LdapUser user, string application, ObjectId id)
        {
            HelperUser _helperUser = new HelperUser(new HelperPerson());
            var crmUser = _helperUser.GetCrmUser(application,user.Id.ToString());
            crmUser.SecurityCheckCanCreateTeam();

            Id = id;
            CreatedByUserId = user.Id.ToString();
            Name = name;
            Location = location.AsLocationRef();
            CrmUsers.AddReferenceObject(crmUser.AsCrmUserRef());
            Save();
            var teamRef = AsTeamRef();

            crmUser.AddToTeam(teamRef);
            location.Teams.AddReferenceObject(teamRef);
            location.SaveToDatabase();
        }

        public void RefreshTeamCompaniesList()
        {
            Companies.Clear();
            foreach (var companyGroupRef in CompanyGroups.ToList())
            {
                var companyGroup = companyGroupRef.AsCompanyGroup();
                var companies = companyGroup.GetAllCompanies();
                foreach (var companyRef in companies)
                {

                    if (Companies.All(x => x.Id != companyRef.Id))
                    {
                        if (companyRef.Coid == null)
                        {
                            var company = companyRef.AsCompany();
                            if (company == null) continue;
                            companyRef.Coid = company.Location.GetCoid();
                        }

                        Companies.Add(companyRef);
                    }
                }
            }
            SaveToDatabase();
        }

        public void RefreshAllianceNonAllianceLists()
        {
            Alliances.Clear();
            NonAlliances.Clear();
            foreach (var companyRef in Companies.ToList())
            {
                var company = companyRef.AsCompany();
                if (company == null)
                {
                    Companies.Remove(companyRef);
                    var alliance = Alliances.SingleOrDefault(x => x.Id == companyRef.Id);
                    var nonAlliance = NonAlliances.SingleOrDefault(x => x.Id == companyRef.Id);
                    if (alliance != null) Alliances.Remove(alliance);
                    if (nonAlliance != null) NonAlliances.Remove(nonAlliance);
                    continue;
                }

                if ((company.IsAlliance)  && Alliances.All(x=>x.Id != company.Id.ToString()))
                {
                    Alliances.Add(company.AsCompanyRef());
                }
                if ((!company.IsAlliance) && NonAlliances.All(x => x.Id != company.Id.ToString()))
                {
                    NonAlliances.Add(company.AsCompanyRef());
                }
            }
            SaveToDatabase();
        }

        public void RefreshCompanyNames()
        {
            var modified = false;
            var coid = Location.AsLocation().GetCoid();
            var companyQueryForIMetal = new QueryCompany(coid);
            var removeCompanies = new List<CompanyRef>();
            foreach (var companyRef in Companies.ToList())
            {

                var companySearchResult = companyQueryForIMetal.GetForId(companyRef.SqlId);

                if (companySearchResult == null)
                {
                    removeCompanies.Add(companyRef);
                    continue;
                }

                if ((companySearchResult.Name != companyRef.Name) && (companySearchResult.CompanyStatusCode != "N"))
                {
                    companyRef.Name = companySearchResult.Name;
                    modified = true;
                }
            }

            if (removeCompanies.Any())
            {
                modified = true;
                foreach (var removeCompany in removeCompanies)
                {
                    var foundCompany = Companies.First(x => x.Id == removeCompany.Id);
                    Companies.Remove(foundCompany);
                }
            }

            if (modified)
            {
                Helper.Upsert(this);
            }

        }

        public Team Save()
        {
            SaveToDatabase();
            return this;
        }

        public TeamRef AsTeamRef()
        {
            return new TeamRef(this);
        }
    }
}