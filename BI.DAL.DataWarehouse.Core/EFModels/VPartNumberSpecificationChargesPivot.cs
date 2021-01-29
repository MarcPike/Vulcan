using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VPartNumberSpecificationChargesPivot
    {
        public string Coid { get; set; }
        public int? PartNumberSpecificationId { get; set; }
        public int? ChargeUnitId { get; set; }
        public decimal MaterialCharge { get; set; }
        public decimal TransportCharge { get; set; }
        public decimal ProductionCharge { get; set; }
        public decimal MiscellaneousCharge { get; set; }
        public decimal SurchargeCharge { get; set; }
        public decimal? TotalCharge { get; set; }
    }
}
