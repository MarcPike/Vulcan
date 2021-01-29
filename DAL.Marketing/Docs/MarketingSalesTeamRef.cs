using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Marketing.Docs
{
    public class MarketingSalesTeamRef : ReferenceObject<Marketing.Docs.MarketingSalesTeam>
    {
        public string Name { get; set; }
        public MarketingSalesTeamRef()
        {

        }

        public MarketingSalesTeamRef(Marketing.Docs.MarketingSalesTeam document) : base(document)
        {
        }

        public Marketing.Docs.MarketingSalesTeam AsStrategicSalesTeam()
        {
            return ToBaseDocument();
        }
    }
}