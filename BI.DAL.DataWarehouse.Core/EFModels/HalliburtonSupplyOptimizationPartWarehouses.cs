using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class HalliburtonSupplyOptimizationPartWarehouses
    {
        public string Coid { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
