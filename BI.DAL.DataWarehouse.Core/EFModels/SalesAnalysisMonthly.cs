using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesAnalysisMonthly
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? Month { get; set; }
        public int? BranchId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public int? DeliveryBranchId { get; set; }
        public int? SalesGroupId { get; set; }
        public int? PartSpecificationId { get; set; }
        public int? Pieces { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal? MaterialValue { get; set; }
        public decimal? TransportValue { get; set; }
        public decimal? ProductionValue { get; set; }
        public decimal? MiscellaneousValue { get; set; }
        public decimal? SurchargeValue { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? TransportCost { get; set; }
        public decimal? ProductionCost { get; set; }
        public decimal? MiscellaneousCost { get; set; }
        public decimal? SurchargeCost { get; set; }
        public decimal? VatValue1 { get; set; }
        public decimal? VatValue2 { get; set; }
        public decimal? VatValue3 { get; set; }
        public decimal? VatValue4 { get; set; }
        public decimal? BaseMaterialValue { get; set; }
        public decimal? BaseTransportValue { get; set; }
        public decimal? BaseProductionValue { get; set; }
        public decimal? BaseMiscellaneousValue { get; set; }
        public decimal? BaseSurchargeValue { get; set; }
        public decimal? BaseVatValue1 { get; set; }
        public decimal? BaseVatValue2 { get; set; }
        public decimal? BaseVatValue3 { get; set; }
        public decimal? BaseVatValue4 { get; set; }
        public decimal? StockMaterialCost { get; set; }
        public decimal? StockTransportCost { get; set; }
        public decimal? StockProductionCost { get; set; }
        public decimal? StockMiscellaneousCost { get; set; }
        public decimal? StockSurchargeCost { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
