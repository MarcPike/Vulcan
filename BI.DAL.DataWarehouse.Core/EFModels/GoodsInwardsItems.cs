using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class GoodsInwardsItems
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? HeaderId { get; set; }
        public int? Item { get; set; }
        public int? PoBranchId { get; set; }
        public int? PoNumber { get; set; }
        public int? PoItem { get; set; }
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }
        public bool? Costed { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? TransportCost { get; set; }
        public decimal? ProductionCost { get; set; }
        public decimal? MiscellaneousCost { get; set; }
        public decimal? SurchargeCost { get; set; }
        public decimal? MaterialValue { get; set; }
        public decimal? TransportValue { get; set; }
        public decimal? ProductionValue { get; set; }
        public decimal? MiscellaneousValue { get; set; }
        public decimal? SurchargeValue { get; set; }
        public int? PurchaseItemId { get; set; }
        public decimal? MaterialStockCost { get; set; }
        public decimal? TransportStockCost { get; set; }
        public decimal? ProductionStockCost { get; set; }
        public decimal? MiscellaneousStockCost { get; set; }
        public decimal? SurchargeStockCost { get; set; }
        public int? TransportPieces { get; set; }
        public decimal? TransportQuantity { get; set; }
        public decimal? TransportWeight { get; set; }
        public int? TransientPieces { get; set; }
        public decimal? TransientQuantity { get; set; }
        public decimal? TransientWeight { get; set; }
        public bool? ServiceItem { get; set; }
        public int? ServicePieces { get; set; }
        public decimal? ServiceQuantity { get; set; }
        public decimal? ServiceWeight { get; set; }
        public bool? ForceItemCompletion { get; set; }
        public int? InvoiceItemId { get; set; }
        public int? SourceCreditStockItemId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
