using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Models
{
    public class TeamUserCompanyViewSelectionsModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public UserRef User { get; set; }
        public TeamRef Team { get; set; }
        public List<CompanySelection> Alliances { get; set; } = new List<CompanySelection>();
        public List<CompanySelection> NonAlliances { get; set; } = new List<CompanySelection>();
        public List<ProspectSelection> Prospects { get; set; } = new List<ProspectSelection>();
    
        public TeamUserCompanyViewSelectionsModel()
        {
            
        }
        public TeamUserCompanyViewSelectionsModel(string application, string userId, TeamUserCompanyViewSelections companyViewSelections)
        {
            Id = companyViewSelections.Id.ToString();
            Application = application;
            UserId = userId;
            User = companyViewSelections.User;
            Team = companyViewSelections.Team;
            Alliances = companyViewSelections.Alliances;
            NonAlliances = companyViewSelections.NonAlliances;
            Prospects = companyViewSelections.Prospects;
        }

    }
}
