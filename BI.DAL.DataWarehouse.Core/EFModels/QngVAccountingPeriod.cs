using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVAccountingPeriod
    {
        public DateTime? PeriodBeginDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public DateTime? AccountingPeriod { get; set; }
        public string AccountingPeriodName { get; set; }
        public int? AccountingYear { get; set; }
        public int? AccountingMonth { get; set; }
        public int? FiscalPeriod { get; set; }
        public int? FiscalYear { get; set; }
        public int? FiscalMonth { get; set; }
        public int? IMetalPeriod { get; set; }
        public int? StelplanPeriod { get; set; }
        public int? SageYear { get; set; }
        public int? SageMonth { get; set; }
    }
}
