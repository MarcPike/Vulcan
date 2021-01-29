using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Models;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.iMetal.Core.Helpers
{
    public class HelperCurrencyForIMetal : IHelperCurrencyForIMetal
    {
        public static List<ExchangeRatesFromCoid> ExchangeRatesFromCoidList = new List<ExchangeRatesFromCoid>();

        public List<string> GetSupportedDisplayCurrencyCodes()
        {
            return new List<string>()
            {
                "GBP",
                "CAD",
                "USD",
                "AED",
                "MYR",
                "SGD",
                "EUR",
                "NOK"
            };
        }

        public string GetFormattedHtmlForCurrencyCode(string value, string currencyCode)
        {
            var currencyHexCodes = new Dictionary<string,string>()
            {
                { "USD", "&#x24"},
                { "GBP", "&#163"},
                { "CAD", "CA&#36"},
                { "AED", "Dhs"},
                { "MYR", "&#82;&#77"},
                { "SGD", "S&#36"},
                { "EUR", "&#8364"},
                { "NOK", "kr"}
            };

            var hexCode = currencyHexCodes.SingleOrDefault(x => x.Key == currencyCode).Value ?? string.Empty;

            return $"{hexCode} {value}";
        }

        public string GetDefaultCurrencyForCoid(string coid)
        {
            if (coid == string.Empty) return string.Empty;

            if (coid == "EUR") return "GBP";
            if (coid == "CAN") return "CAD";
            if (coid == "NOR") return "NOK";
            //if (coid == "DUB") return "AED";
            //if (coid == "MSA") return "MYR";
            //if (coid == "SIN") return "SGD";
            return "USD";
        }

        public (string Currency, string Symbol) GetSymbolForCurrency(string currency)
        {
            var currencyHexCodes = new Dictionary<string, string>()
            {
                { "USD", "USD &#x24;"},
                { "GBP", "GBP &#xa3;"},
                { "CAD", "CAD &#36;"},
                { "AED", "AED"},
                { "MYR", "MYR &#82;&#77;"},
                { "SGD", "SGD S&#36;"},
                { "EUR", "EUR &#8364;"},
                { "NOK", "NOK kr"}
            };

            var symbol = currencyHexCodes.SingleOrDefault(x => x.Key == currency).Value ?? string.Empty;


            return (currency, symbol);
        }

        public (decimal USD, decimal GBP, decimal CAD, decimal AED, decimal MYR, decimal SGD, decimal EUR, decimal NOK) GetExchangeRatesFromCoid(string coid)
        {
            var exchangeRateList = ExchangeRatesFromCoidList.FirstOrDefault(x => x.FromCoid == coid);

            if (exchangeRateList == null)
            {
                var conn = ConnectionFactory.GetConnection(coid);

                var sqlQueryDynamic = new SqlQueryDynamic(coid, conn);

                var currencyCodes = sqlQueryDynamic.ExecuteQueryAsync(
                    "select symbol, exchange_rate from public.currency_codes").Result.ToList();


                var gbp = currencyCodes.FirstOrDefault(x => x.symbol == "GBP")?.exchange_rate ?? (decimal)1;
                var cad = currencyCodes.FirstOrDefault(x => x.symbol == "CAD")?.exchange_rate ?? (decimal)1;
                var usd = currencyCodes.FirstOrDefault(x => x.symbol == "USD")?.exchange_rate ?? (decimal)1;
                var aed = currencyCodes.FirstOrDefault(x => x.symbol == "AED")?.exchange_rate ?? (decimal)1;
                var myr = currencyCodes.FirstOrDefault(x => x.symbol == "MYR")?.exchange_rate ?? (decimal)1;
                var sgd = currencyCodes.FirstOrDefault(x => x.symbol == "SGD")?.exchange_rate ?? (decimal)1;
                var eur = currencyCodes.FirstOrDefault(x => x.symbol == "EUR")?.exchange_rate ?? (decimal)1;
                var nok = currencyCodes.FirstOrDefault(x => x.symbol == "NOK")?.exchange_rate ?? (decimal)1;

                exchangeRateList = new ExchangeRatesFromCoid()
                {
                    FromCoid = coid,
                    GBP = gbp,
                    CAD = cad,
                    USD = usd,
                    AED = aed,
                    MYR = myr,
                    SGD = sgd,
                    EUR = eur,
                    NOK = nok
                };

                ExchangeRatesFromCoidList.Add(exchangeRateList);

            }
            return (
                exchangeRateList.USD, 
                exchangeRateList.GBP, 
                exchangeRateList.CAD, 
                exchangeRateList.AED, 
                exchangeRateList.MYR, 
                exchangeRateList.SGD, 
                exchangeRateList.EUR, 
                exchangeRateList.NOK);
           
        }

        public decimal GetExchangeRateForCurrencyFromCoid(string displayCurrency, string coid)
        {
            var factor = (decimal)1;
            var currency = GetDefaultCurrencyForCoid(coid);
            if (currency == displayCurrency) return factor;




            var exchangeRates = GetExchangeRatesFromCoid(coid);

            if (displayCurrency == "USD") factor = exchangeRates.USD;
            if (displayCurrency == "CAD") factor = exchangeRates.CAD;
            if (displayCurrency == "GBP") factor = exchangeRates.GBP;
            if (displayCurrency == "AED") factor = exchangeRates.AED;
            if (displayCurrency == "MYR") factor = exchangeRates.MYR;
            if (displayCurrency == "SGD") factor = exchangeRates.SGD;
            if (displayCurrency == "EUR") factor = exchangeRates.EUR;
            if (displayCurrency == "NOK") factor = exchangeRates.NOK;

            return factor;
        }

        public string GetCoidForCurrency(string currency)
        {
            if (currency == "GBP") return "EUR";
            if (currency == "CAD") return "CAN";

            return "INC";

        }

        public decimal ConvertValueBackToBaseCurrency(decimal value, decimal exchangeRateToDisplayCurrency)
        {
            if (value == 0) return 0;
            if (exchangeRateToDisplayCurrency == 1) return value;

            return value + (value * -exchangeRateToDisplayCurrency);

        }

        public decimal ConvertValueFromCurrencyToCurrency(decimal value, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency) return value;

            var exchangeRates = GetExchangeRatesFromCoid("INC");

            decimal valueInUSD = value;
            if (fromCurrency == "USD")
                valueInUSD = value;
            if (fromCurrency == "GBP")
                valueInUSD = (1 / exchangeRates.GBP) * value;
            if (fromCurrency == "CAD")
                valueInUSD = (1 / exchangeRates.CAD) * value;
            if (fromCurrency == "AED")
                valueInUSD = (1 / exchangeRates.AED) * value;
            if (fromCurrency == "MYR")
                valueInUSD = (1 / exchangeRates.MYR) * value;
            if (fromCurrency == "SGD")
                valueInUSD = (1 / exchangeRates.SGD) * value;
            if (fromCurrency == "EUR")
                valueInUSD = (1 / exchangeRates.EUR) * value;
            if (fromCurrency == "NOK")
                valueInUSD = (1 / exchangeRates.NOK) * value;

            if (toCurrency == "USD")
                return valueInUSD;
            if (toCurrency == "GBP")
                return valueInUSD * exchangeRates.GBP;
            if (toCurrency == "CAD")
                return valueInUSD * exchangeRates.CAD;
            if (toCurrency == "AED")
                return valueInUSD * exchangeRates.AED;
            if (toCurrency == "MYR")
                return valueInUSD * exchangeRates.MYR;
            if (toCurrency == "SGD")
                return valueInUSD * exchangeRates.SGD;
            if (toCurrency == "EUR")
                return valueInUSD * exchangeRates.EUR;
            if (toCurrency == "NOK")
                return valueInUSD * exchangeRates.NOK;

            return 0;

        }

    }
}
