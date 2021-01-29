using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ExchangeRate
    {
        public int Id { get; set; }
        public DateTime AccountingPeriod { get; set; }
        public string ToCurrency { get; set; }
        public string FromCurrency { get; set; }
        public decimal? Mbrate { get; set; }
        public decimal? Merate { get; set; }
        public decimal? Avrate { get; set; }
        public decimal? ExRate { get; set; }
        public decimal? AverageExRate { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
