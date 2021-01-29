using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class TargetPercentage : ValueCollection<decimal>
    {
        public TargetPercentage()
        {
            Name = "TargetPercentage";
        }
    }
}
