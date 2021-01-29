using DAL.iMetal.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class BaseCost
    {
        public decimal CostPerInch { get; set; } = 0;
        public decimal CostPerPound { get; set; } = 0;
        public decimal CostPerKg { get; set; } = 0;
        public decimal CostPerPiece { get; set; } = 0;

        public RequiredQuantity RequiredQuantity { get; set; }

        public int TotalPieces => RequiredQuantity.Pieces;
        public decimal TotalInches => RequiredQuantity.TotalInches();
        public decimal TotalFeet => RequiredQuantity.TotalFeet();
        public decimal TotalPounds => RequiredQuantity.TotalPounds();
        public decimal TotalKilograms => RequiredQuantity.TotalKilograms();

        public decimal TotalCost => (TotalPounds * CostPerPound * ExchangeRate).RoundAndNormalize(2);
        public decimal TheoWeight { get; set; }

        public string TagNumber { get; set; } = string.Empty;

        public bool IsCostMadeup { get; set; } = false;

        public decimal KurfInchesPerCut { get; set; } = (decimal) 0.25;

        public decimal ExchangeRate { get; set; } = (decimal) 1;

        public BaseCost()
        {
        }

        public static BaseCost CreateNewBaseCostFinishedFromStarting(BaseCost baseCostStart, ProductMaster finishedProduct)
        {
            var baseCostFinish = new BaseCost()
            {
                TheoWeight = finishedProduct.TheoWeight,
                CostPerPound = baseCostStart.CostPerPound,
                CostPerInch = baseCostStart.CostPerInch,
                CostPerPiece = baseCostStart.CostPerPiece,
                CostPerKg = baseCostStart.CostPerKg,
                RequiredQuantity = baseCostStart.RequiredQuantity,
                KurfInchesPerCut = baseCostStart.KurfInchesPerCut,
                ExchangeRate = baseCostStart.ExchangeRate
            };

            return baseCostFinish;
        }

        public RequiredQuantity UpdateAllValues(string coid, decimal costPerPound, decimal theoWeight, OrderQuantity quantity)
        {
            var requiredQuantity = quantity.GetRequiredQuantity(coid, theoWeight);
            var totalInches = requiredQuantity.TotalInches();
            var totalPounds = requiredQuantity.TotalPounds();
            var costPerPiece = (costPerPound * totalPounds) * ExchangeRate;
            var costPerInch = (costPerPiece / totalInches);

            RequiredQuantity = requiredQuantity;
            TheoWeight = theoWeight;
            CostPerKg = costPerPound * (decimal)2.20462;
            CostPerPound = costPerPound;
            CostPerPiece = costPerPiece;
            CostPerInch = costPerInch;

            return requiredQuantity;
        }

        public static BaseCost FromPurchaseOrderItem(string coid, decimal costPerPound,
            decimal theoWeight, OrderQuantity quantity, string displayCurrency)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRate = helperCurrency.GetExchangeRateForCurrencyFromCoid(displayCurrency, coid);

            var requiredQuantity = quantity.GetRequiredQuantity(coid, theoWeight);

            var costPerPiece = (costPerPound * requiredQuantity.TotalPounds()) * exchangeRate;
            var costPerInch = (costPerPiece / requiredQuantity.TotalInches());

            var result = new BaseCost
            {
                RequiredQuantity = requiredQuantity,
                TheoWeight = theoWeight,
                CostPerPound = costPerPound,
                CostPerKg = costPerPound * (decimal)2.20462,
                CostPerPiece = costPerPiece,
                CostPerInch = costPerInch,
                ExchangeRate = exchangeRate
            };

            return result;
        }

        public static BaseCost FromMadeUpCost(string coid, MadeUpCost madeUpCost, OrderQuantity quantity, string displayCurrency)
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRate = helperCurrency.GetExchangeRateForCurrencyFromCoid(displayCurrency, coid);

            var requiredQuantity = quantity.GetRequiredQuantity(coid,madeUpCost.TheoWeight);
            var costPerPiece = (madeUpCost.CostPerPound * requiredQuantity.TotalPounds()) * exchangeRate;
            var costPerInch = (costPerPiece / requiredQuantity.TotalInches());

            var result = new BaseCost
            {
                RequiredQuantity = requiredQuantity,
                TheoWeight = madeUpCost.TheoWeight,
                CostPerKg = madeUpCost.CostPerKilogram,
                CostPerPound = madeUpCost.CostPerPound,
                CostPerPiece = costPerPiece,
                CostPerInch = costPerInch,
                IsCostMadeup = true,
                ExchangeRate = exchangeRate
            };

            return result;
        }

        public static (BaseCost BaseCost, RequiredQuantity RequiredQuantity) FromStockItems(string coid, decimal costPerPound, decimal theoWeight, OrderQuantity quantity, string tagNumber, string displayCurrency)
        {

            var helperCurrency = new HelperCurrencyForIMetal();
            var exchangeRate = helperCurrency.GetExchangeRateForCurrencyFromCoid(displayCurrency, coid);

            var requiredQuantity = quantity.GetRequiredQuantity(coid, theoWeight);
            var totalPounds = requiredQuantity.TotalPounds();
            var totalInches = requiredQuantity.TotalInches();
            var costPerPiece = (costPerPound * totalPounds) * exchangeRate;
            var costPerInch = (costPerPiece / totalInches);

            var baseCost = new BaseCost
            {
                RequiredQuantity = requiredQuantity,
                TheoWeight = theoWeight,
                CostPerKg = costPerPound * (decimal)2.20462,
                CostPerPound = costPerPound,
                CostPerPiece = costPerPiece,
                CostPerInch = costPerInch,
                TagNumber = tagNumber,
                ExchangeRate = exchangeRate
            };

            return (baseCost,requiredQuantity);

        }

        public void ChangeCostPerInch(decimal costPerInch)
        {
            var totalInches = TotalInches;
            CostPerInch = costPerInch * ExchangeRate;
            CostPerPiece = RequiredQuantity.PieceLength.Inches * CostPerInch;
            CostPerPound = (CostPerInch * RequiredQuantity.TotalInches()) / RequiredQuantity.TotalPounds(); //CostPerInch * TheoWeight * ExchangeRate;
            CostPerKg = CostPerPound * (decimal)2.20462;
        }

        public void ChangeTheoWeight(decimal theoWeight)
        {
            TheoWeight = theoWeight;
            ChangeCostPerPound(CostPerPound);

            //var totalInches = TotalInches;
            //CostPerPound = totalInches * TheoWeight;
            //CostPerKg = CostPerPound * (decimal)2.20462;
            //CostPerPiece = RequiredQuantity.PieceLength.Inches * CostPerInch;
        }

        public void ChangeCostPerPound(decimal costPerPound)
        {
            CostPerPound = costPerPound * ExchangeRate;
            CostPerKg = costPerPound * (decimal)2.20462;
            CostPerPiece = RequiredQuantity.PieceWeight.Pounds * CostPerPound;
            CostPerInch = CostPerPiece / RequiredQuantity.PieceLength.Inches;
        }

        public void ChangeCostPerKg(decimal costPerKg)
        {
            CostPerKg = costPerKg * ExchangeRate;
            CostPerPound = CostPerKg * (decimal)0.453592;
            CostPerPiece = RequiredQuantity.PieceWeight.Kilograms * CostPerKg;
            CostPerInch = CostPerPiece / RequiredQuantity.PieceLength.Inches;
        }

        public static BaseCost Clone(BaseCost baseCost)
        {
            return new BaseCost()
            {
                TheoWeight = baseCost.TheoWeight,
                CostPerPound = baseCost.CostPerPound,
                CostPerInch = baseCost.CostPerInch,
                CostPerKg = baseCost.CostPerKg,
                CostPerPiece = baseCost.CostPerPiece,
                RequiredQuantity = baseCost.RequiredQuantity,
                ExchangeRate = baseCost.ExchangeRate
            };
        }
    }
}