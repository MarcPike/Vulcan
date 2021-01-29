using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CostItems
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? CostGroupId { get; set; }
        public decimal? Cost { get; set; }
        public int? CostUnitId { get; set; }
        public decimal? CostQuantity { get; set; }
        public int? CostQuantityUnitId { get; set; }
        public decimal? Value { get; set; }
        public int? PoBranchId { get; set; }
        public int? PoNumber { get; set; }
        public int? PoItem { get; set; }
        public int? SupplierId { get; set; }
        public string BillingReference { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string ItemType { get; set; }
        public int? ItemId { get; set; }
        public bool? InternalCost { get; set; }
        public bool? ActualCost { get; set; }
        public int? WorkCentreId { get; set; }
        public bool? SystemCost { get; set; }
        public int? QuantityTypeCode { get; set; }
        public decimal? BaseCost { get; set; }
        public decimal? BaseValue { get; set; }
        public int? ExchangeRateType { get; set; }
        public string Description { get; set; }
        public int? PurchaseGroupId { get; set; }
        public int? CostPurchaseItemId { get; set; }
        public string CostFixStatus { get; set; }
        public string Visibility { get; set; }
        public bool? DutyItem { get; set; }
        public decimal? PurchaseItemLumpSumCost { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? FabricationChargeCodePriceId { get; set; }
        public string FabricationSource { get; set; }
        public bool FromHeaderCost { get; set; }
    }
}
