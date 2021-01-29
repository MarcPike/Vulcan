using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesItemsDrillingCharges
    {
        public int SalesItemsDrillingChargesId { get; set; }
        public string Coid { get; set; }
        public int SalesBranchId { get; set; }
        public int SalesItemId { get; set; }
        public decimal? DrillSize { get; set; }
        public decimal? PricePer { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
