using DAL.Vulcan.Mongo.Core.Analysis;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Helpers;

namespace DAL.Vulcan.Mongo.Core.Models
{

    public class AnalysisDetailModel
    {
        public List<ProductWinLossHistory> Quotes { get; set; } = new List<ProductWinLossHistory>();

        public int Count => Quotes.Count;
        public decimal TotalCost
        {
            get { return Quotes.Sum(x => x.TotalCost); }
        }

        public decimal TotalPrice
        {
            get { return Quotes.Sum(x => x.TotalPrice); }
        }

        public decimal TotalAdditionalServiceCost
        {
            get { return Quotes.Sum(x => x.AdditionalServiceCost); }
        }

        public decimal TotalAdditionalServicePrice
        {
            get { return Quotes.Sum(x => x.AdditionalServicePrice); }
        }

        public decimal TotalKerfCost
        {
            get { return Quotes.Sum(x => x.TotalKerfCost); }
        }

        public decimal MaxPricePerPound { get; set; }
        public decimal MinPricePerPound { get; set; }
        public decimal AvgPricePerPound { get; set; }

        public decimal MaxPricePerInch { get; set; }
        public decimal MinPricePerInch { get; set; }
        public decimal AvgPricePerInch { get; set; }

        public decimal MaxPricePerKilogram { get; set; }
        public decimal MinPricePerKilogram { get; set; }
        public decimal AvgPricePerKilogram { get; set; }

        public decimal MaxPricePerEach { get; set; }
        public decimal MinPricePerEach { get; set; }
        public decimal AvgPricePerEach { get; set; }

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