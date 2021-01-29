using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class PurchaseOrderItems
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? PurchaseHeaderId { get; set; }
        public int? ItemNumber { get; set; }
        public int? ProductId { get; set; }
        public int? DimensionsId { get; set; }
        public string ExternalDescription { get; set; }
        public string InternalDescription { get; set; }
        public int? OrderedPieces { get; set; }
        public decimal? OrderedQuantity { get; set; }
        public decimal? OrderedWeight { get; set; }
        public int? AllocatedPieces { get; set; }
        public decimal? AllocatedQuantity { get; set; }
        public decimal? AllocatedWeight { get; set; }
        public int? DeliveredPieces { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? DeliveredWeight { get; set; }
        public int? BalancePieces { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public decimal? BalanceWeight { get; set; }
        public int? WeightUnitsId { get; set; }
        public int? BoughtForCustomerId { get; set; }
        public int? PartSpecificationId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? OriginalDueDate { get; set; }
        public string ManualDate { get; set; }
        public int? RollingWeek { get; set; }
        public string RollingReference { get; set; }
        public DateTime? ReadyDate { get; set; }
        public DateTime? DespatchDate { get; set; }
        public bool? Visible { get; set; }
        public decimal? VisibleCost { get; set; }
        public int? VisibleUnitId { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? LocalCost { get; set; }
        public decimal? OriginalQuantity { get; set; }
        public int? OriginalQuantityUnitId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? TransportPieces { get; set; }
        public decimal? TransportQuantity { get; set; }
        public decimal? TransportWeight { get; set; }
        public string SpecificationValue1 { get; set; }
        public string SpecificationValue2 { get; set; }
        public string SpecificationValue3 { get; set; }
        public string SpecificationValue4 { get; set; }
        public string SpecificationValue5 { get; set; }
        public int? PurchaseOrderTotalsId { get; set; }
        public bool? ShowCosts { get; set; }
        public int? TransientPieces { get; set; }
        public decimal? TransientQuantity { get; set; }
        public decimal? TransientWeight { get; set; }
        public string SpecificationValue6 { get; set; }
        public string SpecificationValue7 { get; set; }
        public string SpecificationValue8 { get; set; }
        public string SpecificationValue9 { get; set; }
        public string SpecificationValue10 { get; set; }
        public int? PurchaseGroupId { get; set; }
        public string GoodsInwardsNotes { get; set; }
        public int? EntryPieces { get; set; }
        public decimal? EntryQuantity { get; set; }
        public decimal? EntryWeight { get; set; }
        public int? PartCompanyId { get; set; }
        public string PartNumber { get; set; }
        public int? WorkingSpecificationId { get; set; }
        public DateTime? LastReceiptDate { get; set; }
        public string EnquiryStatus { get; set; }
        public int? BtbPurchaseHeaderId { get; set; }
        public int? ProductLevelAllocationId { get; set; }
        public int? TransferFromBranchId { get; set; }
        public int? BtbSupplierId { get; set; }
        public int? BtbBranchId { get; set; }
        public string RequestStatus { get; set; }
        public string RequestType { get; set; }
        public string QuoteReference { get; set; }
        public int? ImportBatchNumber { get; set; }
        public int? ImportNumber { get; set; }
        public int? ImportItem { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? ProductSubGroupId { get; set; }
        public int? DeletionReasonId { get; set; }
        public string DeletionReasonDescription { get; set; }
    }
}
