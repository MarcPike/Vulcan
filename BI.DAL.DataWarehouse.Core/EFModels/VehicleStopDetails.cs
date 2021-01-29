using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VehicleStopDetails
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public DateTime Cdate { get; set; }
        public DateTime Mdate { get; set; }
        public int CuserId { get; set; }
        public int MuserId { get; set; }
        public string Status { get; set; }
        public int? VehicleStopId { get; set; }
        public int? StopType { get; set; }
        public string ItemType { get; set; }
        public int? SalesItemId { get; set; }
        public int? PurchaseItemId { get; set; }
        public int? DespatchItemId { get; set; }
        public int? ProductId { get; set; }
        public int? StockItemId { get; set; }
        public string Description { get; set; }
        public int? Pieces { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Dimension1 { get; set; }
        public decimal? Dimension2 { get; set; }
        public decimal? Dimension3 { get; set; }
        public decimal? Dimension4 { get; set; }
        public decimal? Dimension5 { get; set; }
        public decimal? PackWidth { get; set; }
        public decimal? PackHeight { get; set; }
        public decimal? PackLength { get; set; }
        public int? PackDimensionUnitsId { get; set; }
        public string Notes { get; set; }
        public int? GoodsInwardsItemId { get; set; }
        public int? StatusEnum { get; set; }
        public int? ParentId { get; set; }
        public DateTime? PickingTicketDate { get; set; }
        public bool? PickingConfirmed { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
        public int? ProcessGroupId { get; set; }
        public DateTime? PrintedOnPickingTicketTimestamp { get; set; }
        public DateTime? MiscellaneousNotePrinted { get; set; }
    }
}
