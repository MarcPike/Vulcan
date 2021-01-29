using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockTransactionItems
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public DateTime Cdate { get; set; }
        public int CuserId { get; set; }
        public DateTime Mdate { get; set; }
        public int MuserId { get; set; }
        public string Status { get; set; }
        public int? TransactionId { get; set; }
        public int? Item { get; set; }
        public int? Sequence { get; set; }
        public int? BranchId { get; set; }
        public int? WarehouseId { get; set; }
        public int? ProductId { get; set; }
        public int? StockItemId { get; set; }
        public int? ParentStockItemId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public string Description { get; set; }
        public int? Pieces { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Quantity { get; set; }
        public int? BalancePieces { get; set; }
        public decimal? BalanceWeight { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public decimal? MaterialCost { get; set; }
        public int? MaterialUnitsId { get; set; }
        public decimal? MaterialValue { get; set; }
        public decimal? TransportCost { get; set; }
        public int? TransportUnitsId { get; set; }
        public decimal? TransportValue { get; set; }
        public decimal? ProductionCost { get; set; }
        public int? ProductionUnitsId { get; set; }
        public decimal? ProductionValue { get; set; }
        public decimal? MiscellaneousCost { get; set; }
        public int? MiscellaneousUnitsId { get; set; }
        public decimal? MiscellaneousValue { get; set; }
        public decimal? SurchargeCost { get; set; }
        public int? SurchargeUnitsId { get; set; }
        public decimal? SurchargeValue { get; set; }
        public decimal? BalanceMaterialCost { get; set; }
        public decimal? BalanceTransportCost { get; set; }
        public decimal? BalanceProductionCost { get; set; }
        public decimal? BalanceMiscellaneousCost { get; set; }
        public decimal? BalanceMaterialVal { get; set; }
        public decimal? BalanceTransportVal { get; set; }
        public decimal? BalanceProductionVal { get; set; }
        public decimal? BalanceMiscellaneousVal { get; set; }
        public decimal? BalanceSurchargeValue { get; set; }
        public decimal? BalanceSurchargeCost { get; set; }
        public int? CompanyId { get; set; }
        public int? OrderBranchId { get; set; }
        public int? OrderNumber { get; set; }
        public int? OrderItem { get; set; }
        public int? DespatchBranchId { get; set; }
        public int? DespatchNumber { get; set; }
        public int? DespatchItem { get; set; }
        public int? InvoiceBranchId { get; set; }
        public int? InvoiceNumber { get; set; }
        public int? InvoiceItem { get; set; }
        public string OtherOrderReference { get; set; }
        public DateTime? DespatchedDate { get; set; }
        public bool? CustomersOwn { get; set; }
        public int? BoughtForCustomerId { get; set; }
        public int? SalesType { get; set; }
        public decimal? PackingWeight { get; set; }
        public decimal? InvoiceWeight { get; set; }
        public int? ProductionFactor { get; set; }
        public decimal? ScrapPercentage { get; set; }
        public int? EstimatedConsumptionPieces { get; set; }
        public decimal? EstimatedConsumptionQuantity { get; set; }
        public decimal? EstimatedConsumptionWeight { get; set; }
        public int? BatchNumber { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
        public int? OutsideProcessorId { get; set; }
    }
}
