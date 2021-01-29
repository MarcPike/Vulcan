using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VPentagonShippingItems
    {
        public string Coid { get; set; }
        public string DespatchBranchCode { get; set; }
        public string DespatchBranchName { get; set; }
        public int? DespatchNumber { get; set; }
        public int? DespatchItemNumber { get; set; }
        public string DespatchDescription { get; set; }
        public string SalesOrderDescription { get; set; }
        public string DespatchStatusCode { get; set; }
        public DateTime? DateRaised { get; set; }
        public DateTime? DateDespatched { get; set; }
        public DateTime? DateEta { get; set; }
        public DateTime? DatePrinted { get; set; }
        public int? SalesItemId { get; set; }
        public string SoitemReference { get; set; }
        public int? DeliveredPieces { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? DeliveredWeight { get; set; }
        public string ProductCode { get; set; }
        public decimal? OrderedQuantity { get; set; }
        public string OrderedQuantityUom { get; set; }
        public decimal? ChargeQuantity { get; set; }
        public decimal? BaseUnitCharge { get; set; }
        public string ChargeUom { get; set; }
        public decimal? BaseNetValue { get; set; }
        public decimal? BaseTransportValue { get; set; }
        public string DefaultBaseCurrency { get; set; }
    }
}
