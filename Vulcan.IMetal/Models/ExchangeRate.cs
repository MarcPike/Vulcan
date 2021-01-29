using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.IMetal.Models
{
    public class ExchangeRate
    {
        public string SourceCoid { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime AsOfDate { get; set; }

        public static List<ExchangeRate> GetRateList()
        {
            
            var result = new List<ExchangeRate>();

            GetRatesForInc(result);
            GetRatesForEur(result);
            GetRatesForCan(result);
            GetRatesForSin(result);
            GetRatesForMsa(result);
            GetRatesForDub(result);

            return result;
        }

        private static void GetRatesForDub(List<ExchangeRate> result)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("INC");

            //var exchangeRates = StockItemsAdvancedQuery.GetExchangeRatesFromCoid("INC");

            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "USD",
                    Rate = exchangeRates.USD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "CAD",
                    Rate = exchangeRates.CAD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "GBP",
                    Rate = exchangeRates.GBP,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "AED",
                    Rate = exchangeRates.AED,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "MYR",
                    Rate = exchangeRates.MYR,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "SGD",
                    Rate = exchangeRates.SGD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "DUB",
                    FromCurrency = "USD",
                    ToCurrency = "EUR",
                    Rate = exchangeRates.EUR,
                    AsOfDate = DateTime.Now
                });
        }

        private static void GetRatesForMsa(List<ExchangeRate> result)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("INC");

            //var exchangeRates = StockItemsAdvancedQuery.GetExchangeRatesFromCoid("INC");

            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "USD",
                    Rate = exchangeRates.USD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "CAD",
                    Rate = exchangeRates.CAD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "GBP",
                    Rate = exchangeRates.GBP,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "AED",
                    Rate = exchangeRates.AED,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "MYR",
                    Rate = exchangeRates.MYR,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "SGD",
                    Rate = exchangeRates.SGD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "MSA",
                    FromCurrency = "USD",
                    ToCurrency = "EUR",
                    Rate = exchangeRates.EUR,
                    AsOfDate = DateTime.Now
                });
        }

        private static void GetRatesForSin(List<ExchangeRate> result)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("INC");

            //var exchangeRates = StockItemsAdvancedQuery.GetExchangeRatesFromCoid("INC");

            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "USD",
                    Rate = exchangeRates.USD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "CAD",
                    Rate = exchangeRates.CAD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "GBP",
                    Rate = exchangeRates.GBP,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "AED",
                    Rate = exchangeRates.AED,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "MYR",
                    Rate = exchangeRates.MYR,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "SGD",
                    Rate = exchangeRates.SGD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "SIN",
                    FromCurrency = "USD",
                    ToCurrency = "EUR",
                    Rate = exchangeRates.EUR,
                    AsOfDate = DateTime.Now
                });
        }

        private static void GetRatesForCan(List<ExchangeRate> result)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("CAN");

            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "CAD",
                    Rate = exchangeRates.CAD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "GBP",
                    Rate = exchangeRates.GBP,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "USD",
                    Rate = exchangeRates.USD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "AED",
                    Rate = exchangeRates.AED,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "MYR",
                    Rate = exchangeRates.MYR,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "SGD",
                    Rate = exchangeRates.SGD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "CAN",
                    FromCurrency = "CAD",
                    ToCurrency = "EUR",
                    Rate = exchangeRates.EUR,
                    AsOfDate = DateTime.Now
                });

        }

        private static void GetRatesForEur(List<ExchangeRate> result)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("EUR");

            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "GBP",
                    Rate = exchangeRates.GBP,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "CAD",
                    Rate = exchangeRates.CAD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "USD",
                    Rate = exchangeRates.USD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "AED",
                    Rate = exchangeRates.AED,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "MYR",
                    Rate = exchangeRates.MYR,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "SGD",
                    Rate = exchangeRates.SGD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "EUR",
                    FromCurrency = "GBP",
                    ToCurrency = "EUR",
                    Rate = exchangeRates.EUR,
                    AsOfDate = DateTime.Now
                });


        }

        private static void GetRatesForInc(List<ExchangeRate> result)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("INC");

            //var exchangeRates = StockItemsAdvancedQuery.GetExchangeRatesFromCoid("INC");

            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "USD",
                    Rate = exchangeRates.USD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "CAD",
                    Rate = exchangeRates.CAD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "GBP",
                    Rate = exchangeRates.GBP,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "AED",
                    Rate = exchangeRates.AED,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "MYR",
                    Rate = exchangeRates.MYR,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "SGD",
                    Rate = exchangeRates.SGD,
                    AsOfDate = DateTime.Now
                });
            result.Add(
                new ExchangeRate()
                {
                    SourceCoid = "INC",
                    FromCurrency = "USD",
                    ToCurrency = "EUR",
                    Rate = exchangeRates.EUR,
                    AsOfDate = DateTime.Now
                });
        }
    }
}
