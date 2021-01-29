using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.Croz
{
    public class CrozProductionCostList : BaseDocument
    {
        public static MongoRawQueryHelper<CrozProductionCostList> Helper = new MongoRawQueryHelper<CrozProductionCostList>();
        public string Region { get; set; } = string.Empty;
        public List<CrozProductionCost> ProductionCosts { get; set; } = new List<CrozProductionCost>();
    }
}
