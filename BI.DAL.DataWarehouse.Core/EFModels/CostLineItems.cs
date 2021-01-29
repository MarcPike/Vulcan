using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CostLineItems
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string ViewId { get; set; }
        public int? RowNumber { get; set; }
        public int? ItemId { get; set; }
        public string DefaultWeightUom { get; set; }
        public string DefaultBaseCurrency { get; set; }
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
        public string CostGroupCode { get; set; }
        public string CostGroupDescription { get; set; }
        public string CostUnitCode { get; set; }
        public string CostUnitDescription { get; set; }
        public decimal? CostUnitScale { get; set; }
        public string CostUnitQuantityCode { get; set; }
        public string CostQuantityUnitCode { get; set; }
        public string CostQuantityUnitDescription { get; set; }
        public decimal? CostQuantity { get; set; }
        public decimal? BaseCost { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? ExchangeRateType { get; set; }
        public string Description { get; set; }
        public string Visibility { get; set; }
        public bool? SystemCost { get; set; }
        public decimal? ItemScale { get; set; }
        public decimal? CostLineQuantity2 { get; set; }
        public string CostLineQuantity2Uom { get; set; }
        public decimal? OrderedBaseValue2 { get; set; }
        public decimal? BalanceBaseValue2 { get; set; }
        public decimal? CostQuantity2 { get; set; }
        public string CostQuantity2Uom { get; set; }
        public decimal? OrderedBaseUnitCost2 { get; set; }
        public decimal? BalanceBaseUnitCost2 { get; set; }
        public string UnitCost2Uom { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
