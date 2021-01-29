using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductionCostingRules
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public string ProductionType { get; set; }
        public int? StockStatusId { get; set; }
        public string Description { get; set; }
        public string MaterialCostingType { get; set; }
        public string MaterialStandardCostProduct { get; set; }
        public string MaterialStandardCostSource { get; set; }
        public string MaterialMarkupType { get; set; }
        public decimal? MaterialMarkupPercentage { get; set; }
        public string MaterialRollupType { get; set; }
        public string ProductionCostingType { get; set; }
        public string ProductionStandardCostSource { get; set; }
        public string ProductionRollupType { get; set; }
        public string ConsumableCostingType { get; set; }
        public string ConsumableRollupType { get; set; }
        public string TransportCostingType { get; set; }
        public string TransportRollupType { get; set; }
        public string MiscellaneousCostingType { get; set; }
        public string MiscellaneousRollupType { get; set; }
        public string SurchargeCostingType { get; set; }
        public string SurchargeRollupType { get; set; }
        public string OutworkCostingType { get; set; }
        public string OutworkRollupType { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public string TransportStepCostingType { get; set; }
        public string TransportStepRollupType { get; set; }
    }
}
