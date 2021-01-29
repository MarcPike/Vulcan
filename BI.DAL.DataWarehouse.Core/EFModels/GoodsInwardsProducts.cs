using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class GoodsInwardsProducts
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? ItemId { get; set; }
        public int? StockItemId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public int? PhysicalPieces { get; set; }
        public decimal? PhysicalWeight { get; set; }
        public decimal? PhysicalQuantity { get; set; }
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
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
