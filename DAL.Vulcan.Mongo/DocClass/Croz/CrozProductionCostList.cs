using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.DocClass.Croz
{
    public class CrozProductionCostList : BaseDocument
    {
        public static MongoRawQueryHelper<CrozProductionCostList> Helper = new MongoRawQueryHelper<CrozProductionCostList>();
        public string Region { get; set; } = string.Empty;
        public List<CrozProductionCost> ProductionCosts { get; set; } = new List<CrozProductionCost>();
    }
}
