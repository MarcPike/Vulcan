using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class InboundAllocations1
    {
        public string Id { get; set; }
        public string Coid { get; set; }
        public string DefaultWeightUom { get; set; }
        public string DefaultBaseCurrency { get; set; }
        public int InboundAllocationId { get; set; }
        public string InboundAllocationType { get; set; }
        public int? InboundHeaderId { get; set; }
        public int? InboundItemId { get; set; }
        public string InboundOrderBranchCode { get; set; }
        public string InboundOrderBranchName { get; set; }
        public int? InboundOrderNumber { get; set; }
        public int? InboundOrderItemNumber { get; set; }
        public string InboundItemReferenceNumber { get; set; }
        public string VendorNumber { get; set; }
        public string VendorName { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public DateTime? InboundOrderItemDueDate { get; set; }
        public DateTime? InboundOrderItemOriginalDueDate { get; set; }
        public string InboundItemStatusCode { get; set; }
        public string InboundItemStatusDescription { get; set; }
        public string InboundProductCode { get; set; }
        public string InboundProductDescription { get; set; }
        public string InboundOrderWarehouseCode { get; set; }
        public string InboundOrderWarehouseName { get; set; }
        public int? AllocatedPieces { get; set; }
        public decimal? AllocatedQuantity { get; set; }
        public decimal? AllocatedWeight { get; set; }
        public int? ReservedByHeaderId { get; set; }
        public int? ReservedByItemId { get; set; }
        public string ReservedByDiscrimnitor { get; set; }
        public string ReservedByOrderBranchCode { get; set; }
        public string ReservedByOrderBranchName { get; set; }
        public int? ReservedByOrderNumber { get; set; }
        public int? ReservedByItemNumber { get; set; }
        public string ReservedByItemReferenceNumber { get; set; }
        public string ReservedByOrderType { get; set; }
        public string ReservedByOrderTypeDescription { get; set; }
        public string ReservedByItemStatusCode { get; set; }
        public string ReservedByItemStatusDescription { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string ReservedByWarehouseCode { get; set; }
        public string ReservedByWarehouseName { get; set; }
        public string ReservedByProductCode { get; set; }
        public string ReservedByProductDescription { get; set; }
        public string ReservedByProductCategory { get; set; }
        public string ReservedByProductSize { get; set; }
        public string ReservedByProductSizeDescription { get; set; }
        public string ReservedByProductCondition { get; set; }
        public string ReservedByProductGrade { get; set; }
        public decimal? ReservedByProductLength { get; set; }
        public string ReservedByItemPartNumber { get; set; }
        public DateTime? ReservedByDueDate { get; set; }
        public string ReservedByReference { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
