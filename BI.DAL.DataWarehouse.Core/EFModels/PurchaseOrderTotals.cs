using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class PurchaseOrderTotals
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? OrderedPieces { get; set; }
        public decimal? OrderedQuantity { get; set; }
        public decimal? OrderedWeight { get; set; }
        public int? DeliveredPieces { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? DeliveredWeight { get; set; }
        public int? BalancePieces { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public decimal? BalanceWeight { get; set; }
        public decimal? MaterialValue { get; set; }
        public decimal? TransportValue { get; set; }
        public decimal? ProductionValue { get; set; }
        public decimal? MiscellaneousValue { get; set; }
        public decimal? SurchargeValue { get; set; }
        public int? TransportPieces { get; set; }
        public decimal? TransportQuantity { get; set; }
        public decimal? TransportWeight { get; set; }
        public int? TransientPieces { get; set; }
        public decimal? TransientQuantity { get; set; }
        public decimal? TransientWeight { get; set; }
        public decimal? BaseMaterialValue { get; set; }
        public decimal? BaseTransportValue { get; set; }
        public decimal? BaseProductionValue { get; set; }
        public decimal? BaseMiscellaneousValue { get; set; }
        public decimal? BaseSurchargeValue { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
