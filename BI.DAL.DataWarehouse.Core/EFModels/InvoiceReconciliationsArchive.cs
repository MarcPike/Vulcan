using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class InvoiceReconciliationsArchive
    {
        public DateTime SnapshotDate { get; set; }
        public DateTime AccountingPeriod { get; set; }
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTimeOffset? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTimeOffset? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? CostItemId { get; set; }
        public int? StockTransactionItemId { get; set; }
        public int? PurchaseItemId { get; set; }
        public int? SupplierId { get; set; }
        public int? GoodsItemId { get; set; }
        public int? CostGroup { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Value { get; set; }
        public decimal? BaseCost { get; set; }
        public decimal? BaseValue { get; set; }
        public int? CostUnitId { get; set; }
        public decimal? CostQuantity { get; set; }
        public int? CostQuantityUnitId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? ProductId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public string SpecificationValue1 { get; set; }
        public string SpecificationValue2 { get; set; }
        public string SpecificationValue3 { get; set; }
        public string SpecificationValue4 { get; set; }
        public string SpecificationValue5 { get; set; }
        public int? DespatchId { get; set; }
        public string MatchedStatus { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int? PurchaseGroupId { get; set; }
        public decimal? ReconciledValue { get; set; }
        public string SpecificationValue6 { get; set; }
        public string SpecificationValue7 { get; set; }
        public string SpecificationValue8 { get; set; }
        public string SpecificationValue9 { get; set; }
        public string SpecificationValue10 { get; set; }
        public decimal? AdjustmentValue { get; set; }
        public string CostSource { get; set; }
        public int? Pieces { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public string CreationReference { get; set; }
        public string BillingReference { get; set; }
        public int? BranchId { get; set; }
        public int? VehicleRunId { get; set; }
        public int? StockTransactionId { get; set; }
        public decimal? UpdatedReconciledValue { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public string CostItemPurchaseReference { get; set; }
        public decimal? CostingWeight { get; set; }
    }
}
