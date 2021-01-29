using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVExchangeRate
    {
        public DateTime AccountingPeriod { get; set; }
        public string ToCurrency { get; set; }
        public string FromCurrency { get; set; }
        public decimal? Mbrate { get; set; }
        public decimal? Merate { get; set; }
        public decimal? Avrate { get; set; }
    }
}
