using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CostOverrideModel
    {
        public List<string> CostOverrideTypeList = Enum.GetNames(typeof(OverrideType)).ToList();

        public string OverrideType { get; set; } 

        public decimal Value { get; set; }

        public CostOverrideModel()
        {
        }

        public CostOverride AsCostOverride()
        {
            var costOverrideType = (OverrideType)Enum.Parse(typeof(OverrideType), OverrideType);
            return new CostOverride()
            {
                OverrideType = costOverrideType,
                Value = this.Value
            };
        }

        public CostOverrideModel(CostOverride costOverride)
        {
            OverrideType = costOverride.OverrideType.ToString();
            Value = costOverride.Value;
        }
    }
}
