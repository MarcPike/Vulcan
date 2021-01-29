using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVStockItemsExt
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string TagNumber { get; set; }
        public string BranchCode { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? ProductId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int IsLive { get; set; }
        public string WarehouseType { get; set; }
        public string WarehouseTypeDescription { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string BusinessUnit { get; set; }
        public string BusinessUnitCountry { get; set; }
        public string StockStatus { get; set; }
        public string StockStatusDescription { get; set; }
        public string HeatNumber { get; set; }
        public string Mill { get; set; }
        public string PartNumber { get; set; }
        public string Abc { get; set; }
        public string LeadTime { get; set; }
        public string Specification1 { get; set; }
        public string CountryOfMaterialOrigin { get; set; }
        public string ParentTagNumber { get; set; }
        public string ParentWarehouse { get; set; }
        public string MasterWarehouse { get; set; }
        public string MasterTagNumber { get; set; }
        public string StockHoldCode { get; set; }
        public string StockHoldReason { get; set; }
        public string StockHoldUser { get; set; }
        public string RejectReason { get; set; }
        public string CreatedRefPrefix { get; set; }
        public int? CreatedRefNo { get; set; }
        public string SupplierNumber { get; set; }
        public string SupplierName { get; set; }
        public string Pocategory { get; set; }
        public string PocategoryDescription { get; set; }
        public string Potype { get; set; }
        public string PotypeDescription { get; set; }
        public string SupplierReference { get; set; }
        public string TransientType { get; set; }
        public string TransientReference { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal? LengthOnHand { get; set; }
        public decimal? LengthAllocated { get; set; }
        public decimal? LengthAvailable { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? OriginalTotalCostOnHand { get; set; }
        public decimal? TotalCostPerWeightUom { get; set; }
        public decimal? TotalValueOnHand { get; set; }
        public decimal? MaterialValueAllocated { get; set; }
        public decimal? TransportValueAllocated { get; set; }
        public decimal? ProductionValueAllocated { get; set; }
        public decimal? MiscellaneousValueAllocated { get; set; }
        public decimal? SurchargeValueAllocated { get; set; }
        public decimal? TotalValueAllocated { get; set; }
        public decimal? MaterialValueAvailable { get; set; }
        public decimal? TransportValueAvailable { get; set; }
        public decimal? ProductionValueAvailable { get; set; }
        public decimal? MiscellaneousValueAvailable { get; set; }
        public decimal? SurchargeValueAvailable { get; set; }
        public decimal? TotalValueAvailable { get; set; }
    }
}
