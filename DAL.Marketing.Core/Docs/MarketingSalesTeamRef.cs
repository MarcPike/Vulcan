using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Marketing.Core.Docs
{
    public class MarketingSalesTeamRef : ReferenceObject<MarketingSalesTeam>
    {
        public string Name { get; set; }
        public MarketingSalesTeamRef()
        {

        }

        public MarketingSalesTeamRef(MarketingSalesTeam document) : base(document)
        {
        }

        public MarketingSalesTeam AsStrategicSalesTeam()
        {
            return ToBaseDocument();
        }
    }
}