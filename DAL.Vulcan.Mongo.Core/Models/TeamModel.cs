using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class TeamModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string CreateByUserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public LocationRef Location { get; set; }

        public List<CompanyGroupRef> CompanyGroups { get; set; } = new List<CompanyGroupRef>();
        public List<CompanyRef> Companies { get; set; } = new List<CompanyRef>();

        public List<CompanyRef> Alliances { get; set; } = new List<CompanyRef>();
        public List<CompanyRef> NonAlliances { get; set; } = new List<CompanyRef>();
        public List<ProspectRef> Prospects { get; set; } = new List<ProspectRef>();

        public List<Note> Notes { get; set; } = new List<Note>();
        public List<UserRef> ActiveUsers { get; set; } = new List<UserRef>();

        public List<GoalRef> Goals { get; set; } = new List<GoalRef>();
        public List<ActionRef> Actions { get; set; } = new List<ActionRef>();
        public List<CrmUserRef> CrmUsers { get; set; } = new List<CrmUserRef>();

        public string DefaultCurrency { get; set; }

        public TeamModel()
        {
        }

        public TeamModel(Team team, string application, string userId)
        {
            Id = team.Id.ToString();
            CreateByUserId = team.CreatedByUserId;
            Application = application;
            Name = team.Name;
            UserId = userId;
            Location = team.Location;
            CompanyGroups = team.CompanyGroups;
            Companies = team.Companies;
            Notes = team.Notes;
            ActiveUsers = team.ActiveUsers;
            Goals = team.Goals;
            Actions = team.Actions;
            CrmUsers = team.CrmUsers.OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName).ToList();
            Alliances = team.Alliances;
            NonAlliances = team.NonAlliances;
            Prospects = team.Prospects;
            DefaultCurrency = team.DefaultCurrency;
        }

    }
}
