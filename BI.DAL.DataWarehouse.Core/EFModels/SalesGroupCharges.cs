using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesGroupCharges
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? SalesGroupId { get; set; }
        public int? CostGroupId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? SalesNominalId { get; set; }
        public bool? InternalDistribution { get; set; }
        public int? InternalCostDebitId { get; set; }
        public int? InternalCostCreditId { get; set; }
        public int? ExternalCostDebitId { get; set; }
        public int? ExternalCostCreditId { get; set; }
        public bool? DefaultCharge { get; set; }
        public bool? NormallyVisible { get; set; }
        public short? Sequence { get; set; }
        public bool? ExternalDistribution { get; set; }
        public int? StockCostCreditId { get; set; }
        public int? StockCostDebitId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
