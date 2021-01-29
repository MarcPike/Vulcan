using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVSalesChargesPivot
    {
        public string Coid { get; set; }
        public int ItemId { get; set; }
        public decimal? ChargeQuantity { get; set; }
        public string ChargeQuantityUom { get; set; }
        public decimal? BaseMaterialCharge { get; set; }
        public decimal? BaseTransportCharge { get; set; }
        public decimal? BaseProductionCharge { get; set; }
        public decimal? BaseMiscellaneousCharge { get; set; }
        public decimal? BaseSurchargeCharge { get; set; }
        public decimal? BaseTotalCharge { get; set; }
        public string ChargeUom { get; set; }
        public decimal BaseMaterialValue { get; set; }
        public decimal BaseTransportValue { get; set; }
        public decimal BaseProductionValue { get; set; }
        public decimal BaseMiscellaneousValue { get; set; }
        public decimal BaseSurchargeValue { get; set; }
        public decimal? BaseTotalValue { get; set; }
        public string DefaultWeightUom { get; set; }
        public string DefaultBaseCurrency { get; set; }
    }
}
