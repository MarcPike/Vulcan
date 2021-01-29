using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class MaterialCostValue
    {
        public OrderQuantity OrderQuantity { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }
        public List<QuoteTestPiece> TestPieces { get; set; }
        public ProductMaster StartingProduct { get; set; }
        public BaseCost BaseCost { get; set; }
        public decimal CutCostPerPiece { get; set; }


        public string Currency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; } = (decimal) 1;

        public int MaterialPieces => RequiredQuantity.Pieces;
        public decimal MaterialInches => (RequiredQuantity.Pieces * RequiredQuantity.PieceLength.Inches);
        public decimal MaterialPounds => (RequiredQuantity.Pieces * RequiredQuantity.PieceWeight.Pounds);

        public decimal MaterialFeet => MaterialInches / 12;
        public decimal MaterialKilograms => MaterialPounds * (decimal)0.453592;

        public decimal MaterialTotalCost => MaterialPounds * (BaseCost.CostPerPound * BaseCost.ExchangeRate);
        //public decimal MaterialTotalCost => (+MaterialPounds + KerfTotalPounds) * (BaseCost.CostPerPound * BaseCost.ExchangeRate);
        public decimal MaterialCostPerInch => BaseCost.CostPerInch;
        public decimal MaterialCostPerFoot => BaseCost.CostPerInch * 12;
        public decimal MaterialCostPerPound => BaseCost.CostPerPound * BaseCost.ExchangeRate;
        public decimal MaterialCostPerKg => BaseCost.CostPerKg * BaseCost.ExchangeRate;

        public decimal MaterialOnlyCost => MaterialTotalCost;

        public decimal KurfInchesPerCut { get; set; }
        public decimal KerfTotalInches => TotalPieces * KurfInchesPerCut;
        public decimal KerfPoundsPerPiece => (KurfInchesPerCut * BaseCost.TheoWeight);
        public decimal KerfTotalPounds => (KerfTotalInches) * BaseCost.TheoWeight;
        public decimal KerfTotalCost => (KerfTotalInches * BaseCost.CostPerInch);

        public decimal TestPieceInches => TestPieces.Sum(x => x.RequiredQuantity.TotalInches());
        public decimal TestPiecePounds => TestPieces.Sum(x=>x.RequiredQuantity.TotalPounds());
        public decimal TestPieceKilograms => TestPieces.Sum(x => x.RequiredQuantity.TotalKilograms());
        public decimal TestPiecesTotalCost => TestPiecePounds * (BaseCost.CostPerPound * BaseCost.ExchangeRate);

        public int TotalPieces => MaterialPieces + TestPieces.Sum(x => x.RequiredQuantity.Pieces);
        public decimal TotalInches => MaterialInches + KerfTotalInches + TestPieceInches;
        public decimal TotalPounds => MaterialPounds + KerfTotalPounds + TestPiecePounds;
        public decimal TotalKilograms => TotalPounds * (decimal) 0.453592;
        public decimal TotalFeet => TotalInches / 12;
        public decimal TotalCutCost => TotalPieces * CutCostPerPiece;
        public decimal TotalCost => MaterialTotalCost + TotalCutCost;

        public MaterialCostValue(ProductMaster startingProduct, OrderQuantity orderQuantity, BaseCost baseCost,
            List<QuoteTestPiece> testPieces, decimal kurfInchesPerCut, List<CostOverride> costOverrides, decimal cutCostPerPiece, string displayCurrency)
        {

            Currency = displayCurrency;
            ExchangeRate = baseCost.ExchangeRate;

            BaseCost = baseCost;
            ApplyOverrides(costOverrides);
            StartingProduct = startingProduct;
            OrderQuantity = orderQuantity;
            RequiredQuantity = OrderQuantity.GetRequiredQuantity(StartingProduct.Coid, StartingProduct.TheoWeight);
            TestPieces = testPieces;
            KurfInchesPerCut = kurfInchesPerCut;
            CutCostPerPiece = cutCostPerPiece;

            //if (cutCostPerPiece != 0)
            //{
            //    CutCostPerPiece = cutCostPerPiece;
            //}
            //else
            //{
            //    CutCostPerPiece = CalculateCutCharge();
            //}
        }

        public void ApplyOverrides(List<CostOverride> costOverrides)
        {
            foreach (var costOverride in costOverrides)
            {
                switch (costOverride.OverrideType)
                {
                    case OverrideType.CostPerInch:
                    {
                        BaseCost.ExchangeRate = 1;
                        BaseCost.ChangeCostPerInch(costOverride.Value);
                        break;
                    }
                    case OverrideType.TheoWeight:
                    {
                        BaseCost.ExchangeRate = 1;
                        BaseCost.ChangeTheoWeight(costOverride.Value);
                        break;
                    }
                    case OverrideType.CostPerKg:
                    {
                        BaseCost.ExchangeRate = 1;
                        BaseCost.ChangeCostPerKg(costOverride.Value);
                        break;
                    }
                    case OverrideType.CostPerPound:
                    {
                        BaseCost.ExchangeRate = 1;
                        BaseCost.ChangeCostPerPound(costOverride.Value);
                        break;
                    }

                }
            }
        }

        public MaterialCostValue()
        {

        }


    }
}