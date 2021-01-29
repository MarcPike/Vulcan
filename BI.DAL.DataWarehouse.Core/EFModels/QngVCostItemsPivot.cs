using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVCostItemsPivot
    {
        public string Coid { get; set; }
        public string DefaultBaseCurrency { get; set; }
        public int? ItemId { get; set; }
        public string ItemType { get; set; }
        public string PobranchCode { get; set; }
        public int? Ponumber { get; set; }
        public int? PoitemNumber { get; set; }
        public string SobranchCode { get; set; }
        public int? Sonumber { get; set; }
        public int? SoitemNumber { get; set; }
        public string GibranchCode { get; set; }
        public int? Ginumber { get; set; }
        public int? GiitemNumber { get; set; }
        public decimal? CostQuantity { get; set; }
        public string CostQuantityUom { get; set; }
        public decimal BaseMaterialCost { get; set; }
        public decimal BaseTransportCost { get; set; }
        public decimal BaseProductionCost { get; set; }
        public decimal BaseMiscellaneousCost { get; set; }
        public decimal BaseSurchargeCost { get; set; }
        public decimal? BaseTotalCost { get; set; }
        public string BaseCostUom { get; set; }
        public decimal OrderedMaterialValue { get; set; }
        public decimal OrderedTransportValue { get; set; }
        public decimal OrderedProductionValue { get; set; }
        public decimal OrderedMiscellaneousValue { get; set; }
        public decimal OrderedSurchargeValue { get; set; }
        public decimal? OrderedTotalValue { get; set; }
        public decimal BalanceMaterialValue { get; set; }
        public decimal BalanceTransportValue { get; set; }
        public decimal BalanceProductionValue { get; set; }
        public decimal BalanceMiscellaneousValue { get; set; }
        public decimal BalanceSurchargeValue { get; set; }
        public decimal? BalanceTotalValue { get; set; }
    }
}
