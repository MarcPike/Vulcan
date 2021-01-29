using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.CRM
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
