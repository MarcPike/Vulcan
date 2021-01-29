using System;
using NUnit.Framework;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Tests.GeneralInfoTests
{
    [TestFixture()]
    public class ExchangeRateTests
    {
        [Test]
        public void GetAllRatesForINC()
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            var exchangeRates = helperCurrency.GetExchangeRatesFromCoid("INC");

            ShowValues(exchangeRates);

        }

        [Test]
        public void GetCurrencyCodes()
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            foreach (var currencyCode in helperCurrency.GetCurrencyCodes("INC"))
            {
                Console.WriteLine(ObjectDumper.Dump(currencyCode));
            }
        }

        [Test]
        public void ConvertGBP()
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            var tenGBP = (decimal) 10;

            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP,"GBP","USD")} USD");
            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "GBP", "CAD")} CAD");
            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "GBP", "EUR")} EUR");
            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "GBP", "SGD")} SGD");
            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "GBP", "MYR")} MYR");
            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "GBP", "AED")} AED");
            Console.WriteLine($"10 GBP is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "GBP", "NOK")} NOK");
        }

        [Test]
        public void ConvertUSD()
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            var tenGBP = (decimal)10;

            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "USD")} USD");
            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "CAD")} CAD");
            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "EUR")} EUR");
            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "SGD")} SGD");
            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "MYR")} MYR");
            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "AED")} AED");
            Console.WriteLine($"10 USD is {helperCurrency.ConvertValueFromCurrencyToCurrency(tenGBP, "USD", "NOK")} NOK");
        }

        [Test]
        public void ConvertSGDtoMYR()
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            var tenSGD = (decimal) 10;

            var tenMYR = helperCurrency.ConvertValueFromCurrencyToCurrency(tenSGD, "SGD", "MYR");

            Console.WriteLine($"10 SGD is {tenMYR}");

        }

        private void ShowValues((decimal USD, decimal GBP, decimal CAD, decimal AED, decimal MYR, decimal SGD, decimal EUR, decimal NOK) exchangeRates)
        {
            Console.WriteLine($"USD: {exchangeRates.USD}");
            Console.WriteLine($"GBP: {exchangeRates.GBP}");
            Console.WriteLine($"CAD: {exchangeRates.CAD}");
            Console.WriteLine($"AED: {exchangeRates.AED}");
            Console.WriteLine($"MYR: {exchangeRates.MYR}");
            Console.WriteLine($"SGD: {exchangeRates.SGD}");
            Console.WriteLine($"EUR: {exchangeRates.EUR}");
            Console.WriteLine($"NOK: {exchangeRates.NOK}");
        }
    }
}
