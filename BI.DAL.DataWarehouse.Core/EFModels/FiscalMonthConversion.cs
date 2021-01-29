using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class FiscalMonthConversion
    {
        public int Id { get; set; }
        public DateTime CalendarMonth { get; set; }
        public DateTime? PeriodBeginDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public int? StelplanPeriod { get; set; }
        public int? SagePeriod { get; set; }
        public int? SageYear { get; set; }
        public int? SageMonth { get; set; }
        public int? FiscalYear { get; set; }
        public int? FiscalMonth { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
