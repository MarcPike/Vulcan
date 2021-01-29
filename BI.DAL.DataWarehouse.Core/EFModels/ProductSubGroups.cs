using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductSubGroups
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? Dimension1 { get; set; }
        public decimal? Dimension1Min { get; set; }
        public decimal? Dimension1Max { get; set; }
        public decimal? Dimension2 { get; set; }
        public decimal? Dimension2Min { get; set; }
        public decimal? Dimension2Max { get; set; }
        public decimal? Dimension3 { get; set; }
        public decimal? Dimension3Min { get; set; }
        public decimal? Dimension3Max { get; set; }
        public decimal? Dimension4 { get; set; }
        public decimal? Dimension4Min { get; set; }
        public decimal? Dimension4Max { get; set; }
        public decimal? Dimension5 { get; set; }
        public decimal? Dimension5Min { get; set; }
        public decimal? Dimension5Max { get; set; }
        public decimal? MinStock { get; set; }
        public int? MinStockUnitId { get; set; }
        public decimal? MaxStock { get; set; }
        public int? MaxStockUnitId { get; set; }
        public decimal? MinStockPeriod { get; set; }
        public int? MinStockPeriodUnitId { get; set; }
        public decimal? MinReorder { get; set; }
        public int? MinReorderUnitId { get; set; }
        public decimal? MaxReorder { get; set; }
        public int? MaxReorderUnitId { get; set; }
        public int? MainSupplierId { get; set; }
        public int? MainSupplierLead { get; set; }
        public int? AlternateSupplierId { get; set; }
        public int? AlternateSupplierLead { get; set; }
        public decimal? GuideCost { get; set; }
        public decimal? GuidePrice { get; set; }
        public decimal? MinimumPrice { get; set; }
        public int? CommodityId { get; set; }
        public string PurchasingNotes { get; set; }
        public decimal? StandardCost { get; set; }
        public string DefaultLocation { get; set; }
        public int? SourceSubGroupId { get; set; }
        public int? ProductionProductId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public bool? MainBuyingGroup { get; set; }
        public bool? NonTraceable { get; set; }
    }
}
