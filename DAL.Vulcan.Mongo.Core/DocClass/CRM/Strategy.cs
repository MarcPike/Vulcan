using DAL.Vulcan.Mongo.Base.Core.DocClass;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class Strategy: BaseDocument
    {
        public string Label { get; set; }
        public List<GoalRef> Goals { get; set; }

        public StrategyRef AsStrategyRef()
        {
            return new StrategyRef(this);
        }
    }

}
