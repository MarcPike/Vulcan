using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Cache;
using Vulcan.IMetal.Context;

namespace Vulcan.IMetal.Helpers
{
    public class HelperCurrencyForIMetal : IHelperCurrencyForIMetal
    {
        public List<string> GetSupportedDisplayCurrencyCodes()
        {
            /*
             *             var gbp = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "GBP")?.ExchangeRate ?? (decimal)1;
            var cad = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "CAD")?.ExchangeRate ?? (decimal)1;
            var usd = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "USD")?.ExchangeRate ?? (decimal)1;
            var aed = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "AED")?.ExchangeRate ?? (decimal)1;
            var myr = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "MYR")?.ExchangeRate ?? (decimal)1;
            var sgd = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "SGD")?.ExchangeRate ?? (decimal)1;
            var eur = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "EUR")?.ExchangeRate ?? (decimal)1;

             */
            return new List<string>()
            {
                "GBP",
                "CAD",
                "USD",
                "AED",
                "MYR",
                "SGD",
                "EUR"
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
                { "EUR", "&#8364"}
            };

            var hexCode = currencyHexCodes.SingleOrDefault(x => x.Key == currencyCode).Value ?? string.Empty;

            return $"{hexCode} {value}";
        }

        public string GetDefaultCurrencyForCoid(string coid)
        {
            if (coid == string.Empty) return string.Empty;

            if (coid == "EUR") return "GBP";
            if (coid == "CAN") return "CAD";
            return "USD";
        }

        public (string Currency, string Symbol) GetSymbolForCurrency(string currency)
        {
            var currencyHexCodes = new Dictionary<string, string>()
            {
                { "USD", "&#x24;"},
                { "GBP", "&#163;"},
                { "CAD", "CA&#36;"},
                { "AED", "Dhs"},
                { "MYR", "&#82;&#77;"},
                { "SGD", "S&#36;"},
                { "EUR", "&#8364;"}
            };

            var symbol = currencyHexCodes.SingleOrDefault(x => x.Key == currency).Value ?? string.Empty;


            return (currency, symbol);
        }


        public (decimal USD, decimal GBP, decimal CAD, decimal AED, decimal MYR, decimal SGD, decimal EUR) GetExchangeRatesFromCoid(string coid)
        {
            var context = ContextFactory.GetStockItemsContextForCoid(coid);

            var gbp = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "GBP")?.ExchangeRate ?? (decimal)1;
            var cad = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "CAD")?.ExchangeRate ?? (decimal)1;
            var usd = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "USD")?.ExchangeRate ?? (decimal)1;
            var aed = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "AED")?.ExchangeRate ?? (decimal)1;
            var myr = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "MYR")?.ExchangeRate ?? (decimal)1;
            var sgd = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "SGD")?.ExchangeRate ?? (decimal)1;
            var eur = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "EUR")?.ExchangeRate ?? (decimal)1;

            return (usd, gbp, cad, aed, myr, sgd, eur);
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

            return 0;

        }
    }
}
