using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ModifyProductionCostValueModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPieces { get; set; }
        public decimal TotalPounds { get; set; }
        public decimal TotalInches { get; set; }
        public string PerTypeName { get; set; }
        public string TypeName { get; set; }
        public decimal InternalCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal MinimumCost { get; set; }
        public string Id { get; set; }

        public ModifyProductionCostValueModel()
        {
            
        }
    }
}
