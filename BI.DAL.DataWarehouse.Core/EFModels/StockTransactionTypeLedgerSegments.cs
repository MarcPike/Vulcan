using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockTransactionTypeLedgerSegments
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string LedgerTransactionType { get; set; }
        public string InternalMaterialFormula { get; set; }
        public string InternalTransportFormula { get; set; }
        public string InternalProductionFormula { get; set; }
        public string InternalMiscellaneousFormula { get; set; }
        public string InternalSurchargeFormula { get; set; }
        public string ExternalMaterialFormula { get; set; }
        public string ExternalTransportFormula { get; set; }
        public string ExternalProductionFormula { get; set; }
        public string ExternalMiscellaneousFormula { get; set; }
        public string ExternalSurchargeFormula { get; set; }
        public string StockControlFormula { get; set; }
        public int? AdjustmentReasonId { get; set; }
        public string CompositeKey { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
