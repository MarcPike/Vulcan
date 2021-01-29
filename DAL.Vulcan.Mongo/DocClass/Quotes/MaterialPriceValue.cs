using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Team_Settings;
using DAL.Vulcan.Mongo.TeamSettings;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class MaterialPriceValue
    {
        public MaterialCostValue MaterialCostValue { get; set; }
        public decimal MaterialTotalCost => MaterialCostValue.MaterialTotalCost;
        public decimal MaterialCostPerInch => MaterialCostValue.MaterialCostPerInch;
        public decimal MaterialCostPerPound => MaterialCostValue.MaterialCostPerPound;
        public decimal MaterialCostPerKilogram => MaterialCostValue.MaterialCostPerKg;
        public decimal MaterialCostPerFoot => MaterialCostValue.MaterialCostPerFoot;
        public decimal MaterialTotalPrice { get; set; }
        public decimal MaterialTotalPriceOverride { get; set; }
        public decimal PricePerPound { get; set; }
        public decimal PricePerPoundOverride { get; set; }
        public decimal PricePerInch { get; set; }
        public decimal PricePerInchOverride { get; set; }
        public decimal PricePerKilogram { get; set; }
        public decimal PricePerKilogramOverride { get; set; }
        public decimal PricePerFoot { get; set; }
        public decimal PricePerFootOverride { get; set; }

        public decimal ExchangeRate { get; set; } = (decimal) 1;
        public string Currency { get; set; } = string.Empty;

        public decimal KerfTotalPrice => MaterialCostValue.KerfTotalInches * PricePerInch;
        public decimal TestPiecesTotalPrice => MaterialCostValue.TestPieceInches * PricePerInch;

        public decimal TotalPrice => MaterialTotalPrice;

        public decimal Margin { get; set; } 
        public decimal MarginOverride { get; set; }

        public MaterialPriceValue(MaterialCostValue cost, TeamPriceTier teamPriceTier)
        {
            UpdateCost(cost,teamPriceTier);
        }

        public MaterialPriceValue()
        {

        }

        public void UpdateCost(MaterialCostValue cost, TeamPriceTier teamPriceTier)
        {
            ExchangeRate = cost.ExchangeRate;
            Currency = cost.Currency;
            MaterialCostValue = cost;
            MaterialTotalPrice = cost.MaterialTotalCost;
            PricePerFoot = cost.MaterialCostPerFoot;
            PricePerInch = cost.MaterialCostPerInch;
            PricePerKilogram = cost.MaterialCostPerKg;
            PricePerPound = cost.MaterialCostPerPound;

            OverrideMargin();
            OverrideTotalPrice();
            OverridePricePerInch();
            OverridePricePerPound();
            OverridePricePerFoot();
            OverridePricePerKilogram();
            //BackoutAndApplyTestCosts();

            var helperTeamPriceTier = new HelperTeamPriceTier();

            Margin = QuoteCalculations.GetMargin(MaterialTotalCost, MaterialTotalPrice);

            if ((Margin == 0))
            {
                if (teamPriceTier.WeightType == WeightType.Kg)
                {
                    var price = helperTeamPriceTier.GetPrice(teamPriceTier.Team.Id, MaterialCostValue.StartingProduct,
                        MaterialCostValue.RequiredQuantity.TotalKilograms(), cost.Currency);


                    if (price != 0)
                    {
                        var existingOverrides = MarginOverride + PricePerFootOverride + PricePerInchOverride +
                                                PricePerKilogramOverride + PricePerPoundOverride;

                        if (existingOverrides == 0)
                        {
                            PricePerKilogramOverride = price;
                            OverridePricePerKilogram();
                        }


                    }
                }

                if (teamPriceTier.WeightType == WeightType.Lbs)
                {
                    var price = helperTeamPriceTier.GetPrice(teamPriceTier.Team.Id, MaterialCostValue.StartingProduct,
                        MaterialCostValue.RequiredQuantity.TotalPounds(), cost.Currency);
                    if (price != 0)
                    {
                        var existingOverrides = MarginOverride + PricePerFootOverride + PricePerInchOverride +
                                                PricePerKilogramOverride + PricePerPoundOverride;

                        if (existingOverrides == 0)
                        {
                            PricePerPoundOverride = price;
                            OverridePricePerPound();
                        }
                    }
                }
            }

            Margin = QuoteCalculations.GetMargin(MaterialTotalCost, MaterialTotalPrice);

        }

        private void BackoutAndApplyTestCosts()
        {
            //MaterialTotalPrice = MaterialTotalPrice + TestPiecesTotalPrice;
        }

        private void OverridePricePerKilogram()
        {
            if (PricePerKilogramOverride == 0) return;

            var newTotalPrice = PricePerKilogramOverride * MaterialCostValue.MaterialKilograms;

            var newMargin = QuoteCalculations.GetMargin(MaterialTotalCost, newTotalPrice);
            Margin = newMargin;
            PricePerPound = newTotalPrice / MaterialCostValue.MaterialPounds;
            PricePerInch = newTotalPrice / MaterialCostValue.MaterialInches;
            PricePerFoot = newTotalPrice / MaterialCostValue.MaterialFeet; 
            PricePerKilogram = PricePerKilogramOverride;
            MaterialTotalPrice = newTotalPrice;
        }

        private void OverridePricePerFoot()
        {
            if (PricePerFootOverride == 0) return;

            var newTotalPrice = PricePerFootOverride * MaterialCostValue.MaterialFeet;

            var newMargin = QuoteCalculations.GetMargin(MaterialTotalCost, newTotalPrice);
            Margin = newMargin;
            PricePerPound = newTotalPrice / MaterialCostValue.MaterialPounds;
            PricePerInch = newTotalPrice / MaterialCostValue.MaterialInches;
            PricePerFoot = PricePerFootOverride;
            PricePerKilogram = newTotalPrice / MaterialCostValue.MaterialKilograms;
            MaterialTotalPrice = newTotalPrice;
        }

        private void OverridePricePerPound()
        {
            if (PricePerPoundOverride == 0) return;

            var newTotalPrice = PricePerPoundOverride * MaterialCostValue.MaterialPounds;

            var newMargin = QuoteCalculations.GetMargin(MaterialTotalCost, newTotalPrice);
            Margin = newMargin;
            PricePerPound = PricePerPoundOverride;
            PricePerInch = newTotalPrice / MaterialCostValue.MaterialInches;
            PricePerFoot = newTotalPrice / MaterialCostValue.MaterialFeet;
            PricePerKilogram = newTotalPrice / MaterialCostValue.MaterialKilograms;
            MaterialTotalPrice = newTotalPrice;
        }

        private void OverridePricePerInch()
        {
            if (PricePerInchOverride == 0) return;

            var newTotalPrice = PricePerInchOverride * MaterialCostValue.MaterialInches;

            var newMargin = QuoteCalculations.GetMargin(MaterialTotalCost, newTotalPrice);
            Margin = newMargin;
            PricePerPound = newTotalPrice / MaterialCostValue.MaterialPounds;
            PricePerInch = PricePerInchOverride;
            PricePerFoot = newTotalPrice / MaterialCostValue.MaterialFeet;
            PricePerKilogram = newTotalPrice / MaterialCostValue.MaterialKilograms;
            MaterialTotalPrice = newTotalPrice;
        }

        private void OverrideTotalPrice()
        {
            if (MaterialTotalPriceOverride == 0) return;

            var newTotalPrice = MaterialTotalPriceOverride;

            var newMargin = QuoteCalculations.GetMargin(MaterialTotalCost, newTotalPrice);
            Margin = newMargin;
            PricePerPound = newTotalPrice / MaterialCostValue.MaterialPounds;
            PricePerInch = newTotalPrice / MaterialCostValue.MaterialInches;
            PricePerFoot = newTotalPrice / MaterialCostValue.MaterialFeet;
            PricePerKilogram = newTotalPrice / MaterialCostValue.MaterialKilograms;
            MaterialTotalPrice = newTotalPrice;
        }

        private void OverrideMargin()
        {
            if (MarginOverride == 0) return;

            if (MarginOverride >= 1)
            {
                MarginOverride = MarginOverride * (decimal).01;
            }

            var newTotalPrice = QuoteCalculations.GetSalePriceFromMargin(MaterialTotalPrice, MarginOverride);
            if (newTotalPrice == 0)
            {
                Margin = 0;
                return;
            }
            Margin = MarginOverride;
            PricePerPound = newTotalPrice / MaterialCostValue.MaterialPounds;
            PricePerInch = newTotalPrice / MaterialCostValue.MaterialInches;
            PricePerFoot = newTotalPrice / MaterialCostValue.MaterialFeet;
            PricePerKilogram = newTotalPrice / MaterialCostValue.MaterialKilograms;
            MaterialTotalPrice = newTotalPrice;

        }

        public void ConvertAllOverridesForExchangeRate(decimal rateForCurrencyChange)
        {


            PricePerPoundOverride = PricePerPoundOverride * rateForCurrencyChange;
            PricePerInchOverride = PricePerInchOverride * rateForCurrencyChange;
            PricePerFootOverride = PricePerFootOverride * rateForCurrencyChange;
            PricePerKilogramOverride = PricePerKilogramOverride * rateForCurrencyChange;
            MaterialTotalPriceOverride = MaterialTotalPriceOverride * rateForCurrencyChange;
        }
    }
}