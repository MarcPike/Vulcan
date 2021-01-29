using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Analysis;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.Models
{

    public class AnalysisDetailModel
    {
        public List<ProductWinLossHistory> Quotes { get; set; } = new List<ProductWinLossHistory>();

        public int Count => Quotes.Count;
        public decimal TotalCost => Quotes.Sum(x=>x.TotalCost);
        public decimal TotalPrice => Quotes.Sum(x => x.TotalPrice);
        public decimal TotalAdditionalServiceCost => Quotes.Sum(x => x.AdditionalServiceCost);
        public decimal TotalAdditionalServicePrice => Quotes.Sum(x => x.AdditionalServicePrice);
        public decimal TotalKerfCost => Quotes.Sum(x => x.TotalKerfCost);

        public decimal MaxPricePerPound;
        public decimal MinPricePerPound;
        public decimal AvgPricePerPound;

        public decimal MaxPricePerInch;
        public decimal MinPricePerInch;
        public decimal AvgPricePerInch;

        public decimal MaxPricePerKilogram;
        public decimal MinPricePerKilogram;
        public decimal AvgPricePerKilogram;

        public decimal MaxPricePerEach;
        public decimal MinPricePerEach;
        public decimal AvgPricePerEach;

        public AnalysisDetailModel(List<ProductWinLossHistory> quoteHistory, string displayCurrency)
        {
            var helperCurrency = new HelperCurrencyForIMetal();

            foreach (var hist in quoteHistory)
            {
                hist.PricePerKilogram =
                    helperCurrency.ConvertValueFromCurrencyToCurrency(hist.PricePerKilogram, hist.DisplayCurrency,
                        displayCurrency);
                hist.PricePerInch =
                    helperCurrency.ConvertValueFromCurrencyToCurrency(hist.PricePerInch, hist.DisplayCurrency,
                        displayCurrency);
                hist.PricePerEach =
                    helperCurrency.ConvertValueFromCurrencyToCurrency(hist.PricePerEach, hist.DisplayCurrency,
                        displayCurrency);
                hist.PricePerPound =
                    helperCurrency.ConvertValueFromCurrencyToCurrency(hist.PricePerPound, hist.DisplayCurrency,
                        displayCurrency);
            }

            Quotes = quoteHistory;

            Calculate();
        }

        private void Calculate()
        {
            if (!Quotes.Any()) return;

            MaxPricePerPound = Quotes.Max(x => x.PricePerPound);
            MinPricePerPound = Quotes.Min(x => x.PricePerPound);
            AvgPricePerPound = Quotes.Average(x => x.PricePerPound);

            MaxPricePerInch = Quotes.Max(x => x.PricePerInch);
            MinPricePerInch = Quotes.Min(x => x.PricePerInch);
            AvgPricePerInch = Quotes.Average(x => x.PricePerInch);

            MaxPricePerKilogram = Quotes.Max(x => x.PricePerKilogram);
            MinPricePerKilogram = Quotes.Min(x => x.PricePerKilogram);
            AvgPricePerKilogram = Quotes.Average(x => x.PricePerKilogram);

            MaxPricePerEach = Quotes.Max(x => x.PricePerEach);
            MinPricePerEach = Quotes.Min(x => x.PricePerEach);
            AvgPricePerEach = Quotes.Average(x => x.PricePerEach);

        }
    }
}