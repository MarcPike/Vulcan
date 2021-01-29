using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Marketing.Docs
{
    public class MarketingSalesTeam: BaseDocument
    {
        public string Name { get; set; } = string.Empty;
        public List<CrmUserRef> SalesPersons { get; set; } = new List<CrmUserRef>();

        public MarketingSalesTeamRef AsMarketingSalesTeamRef()
        {
            return new MarketingSalesTeamRef(this);
        }
    }
}