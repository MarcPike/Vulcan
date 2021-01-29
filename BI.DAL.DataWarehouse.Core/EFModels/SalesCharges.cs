using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesCharges
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? SalesChargeTypeId { get; set; }
        public int ItemId { get; set; }
        public int? SalesGroupChargeId { get; set; }
        public string Description { get; set; }
        public decimal? Charge { get; set; }
        public int? ChargeUnitId { get; set; }
        public decimal? ChargeQuantity { get; set; }
        public int? ChargeQuantityUnitId { get; set; }
        public decimal? Value { get; set; }
        public decimal? ExchangeRate { get; set; }
        public bool? ShowCustomer { get; set; }
        public bool? SystemCharge { get; set; }
        public string CostGroupCode { get; set; }
        public decimal? BaseCharge { get; set; }
        public decimal? BaseValue { get; set; }
        public string ChargeFixStatus { get; set; }
        public string ChargeVisibility { get; set; }
        public int? ChargeItem { get; set; }
        public int? DiscountItem { get; set; }
        public bool? ConfirmAtInvoicing { get; set; }
        public bool? CreatedFromPart { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? FabricationChargeCodePriceId { get; set; }
        public string PricingNotes { get; set; }
        public string FabricationSource { get; set; }
    }
}
