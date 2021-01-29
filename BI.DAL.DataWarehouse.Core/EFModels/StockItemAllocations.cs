using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockItemAllocations
    {
        public string ViewId { get; set; }
        public string Coid { get; set; }
        public string DefaultWeightUom { get; set; }
        public string DefaultBaseCurrency { get; set; }
        public int StockAllocationId { get; set; }
        public int StockItemId { get; set; }
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AllocationType { get; set; }
        public string ReferenceNumber { get; set; }
        public string AllocatedWarehouseCode { get; set; }
        public string AllocatedWarehouseName { get; set; }
        public string StockItem { get; set; }
        public string ProductCode { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductCondition { get; set; }
        public decimal? TagLength { get; set; }
        public string StockLocation { get; set; }
        public string Mill { get; set; }
        public string HeatNumber { get; set; }
        public decimal? StockTotalCostPerWeightUom { get; set; }
        public decimal? StockTotalValueOnHand { get; set; }
        public decimal? TotalAllocatedValue { get; set; }
        public DateTime? StockOriginatedDate { get; set; }
        public int? AllocatedPieces { get; set; }
        public decimal? AllocatedQuantity { get; set; }
        public decimal? AllocatedWeight { get; set; }
        public string ProductDescription { get; set; }
        public string SizeDescription { get; set; }
        public string Comments { get; set; }
        public int? SalesHeaderId { get; set; }
        public int SalesItemId { get; set; }
        public string SalesItemReferenceNumber { get; set; }
        public string SalesItemTypeCode { get; set; }
        public string SalesItemBranchCode { get; set; }
        public int? SalesOrderNumber { get; set; }
        public int? SalesItemNumber { get; set; }
        public string SalesGroupCode { get; set; }
        public string SalesGroupDescription { get; set; }
        public string SalesGroupTypeCode { get; set; }
        public string SalesGroupTypeDescription { get; set; }
        public string SalesRep { get; set; }
        public string Salesperson { get; set; }
        public DateTime? SalesItemDueDate { get; set; }
        public decimal? SalesItemTotalBaseValue { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string FinalProductCode { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
    }
}
