using System.Collections.Generic;
using DAL.Marketing.Core.Docs;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Marketing.Core.Models
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


        public MarketingSalesTeamModel(string application, string userId, MarketingSalesTeam team)
        {
            Id = team.Id.ToString();
            Name = team.Name;
            SalesPersons = team.SalesPersons;

            Application = application;
            UserId = userId;
        }

    }
}