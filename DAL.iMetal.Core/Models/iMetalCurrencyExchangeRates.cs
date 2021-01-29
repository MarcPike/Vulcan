using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.iMetal.Core.Models
{
    public class iMetalCurrencyExchangeRates
    {
        public int Id{ get; set; }

        public string Status{ get; set; }

        public string Code{ get; set; }

        public string Name{ get; set; }

        public string Symbol{ get; set; }

        public decimal? ExchangeRate{ get; set; }

    }
}
