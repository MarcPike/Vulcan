using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Cache;

namespace Vulcan.IMetal.Helpers
{
    public interface IHelperCurrencyForIMetal
    {
        List<string> GetSupportedDisplayCurrencyCodes();

        decimal ConvertValueBackToBaseCurrency(decimal value, decimal exchangeRateToDisplayCurrency);

        decimal GetExchangeRateForCurrencyFromCoid(string displayCurrency, string coid);
        string GetDefaultCurrencyForCoid(string coid);
        (decimal USD, decimal GBP, decimal CAD, decimal AED, decimal MYR, decimal SGD, decimal EUR) GetExchangeRatesFromCoid(string coid);

        decimal ConvertValueFromCurrencyToCurrency(decimal value, string fromCurrency, string toCurrency);
    }
}