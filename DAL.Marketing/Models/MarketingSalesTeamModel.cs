using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Marketing.Models
{
    public class MarketingSalesTeamModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; } 
        public List<CrmUserRef> SalesPersons { get; set; } = new List<CrmUserRef>();

        public MarketingSalesTeamModel()
        {
        }


        public MarketingSalesTeamModel(string application, string userId, Marketing.Docs.MarketingSalesTeam team)
        {
            Id = team.Id.ToString();
            Name = team.Name;
            SalesPersons = team.SalesPersons;

            Application = application;
            UserId = userId;
        }

    }
}