using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class HalliburtonSupplyOptimizationPartMappingExternalToiMetalImport
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public string CustomerPartNumber { get; set; }
        public int? ProductIdiMetalInternal { get; set; }
        public string ProductCode { get; set; }
        public string Plant { get; set; }
        public string IsConsignment { get; set; }
        public string ProductSpec { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCondition { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }
}
