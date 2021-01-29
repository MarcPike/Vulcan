using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CustomerPartDemand
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public DateTime? MonthOf { get; set; }
        public string Plant { get; set; }
        public int? CustomerIdiMetalInternal { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerPartNumberWithDescription { get; set; }
        public string CustomerPartNumber { get; set; }
        public int? ProductIdiMetalInternal { get; set; }
        public string ProductCode { get; set; }
        public decimal CustomerStockOnSiteBase { get; set; }
        public string CustomerStockOnSiteUom { get; set; }
        public decimal? CustomerStockOnSiteInches { get; set; }
        public decimal CustomerIncomingBase { get; set; }
        public string CustomerIncomingUom { get; set; }
        public decimal? CustomerIncomingInches { get; set; }
        public decimal DemandBase { get; set; }
        public string DemandUom { get; set; }
        public decimal? DemandInches { get; set; }
        public decimal SafetyStockBase { get; set; }
        public string SafetyStockUom { get; set; }
        public decimal? SafetyStockInches { get; set; }
        public decimal? CustomerDemandBalanceInches { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
    }
}
