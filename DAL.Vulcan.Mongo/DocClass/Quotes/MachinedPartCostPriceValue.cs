using System;
using System.Collections.Generic;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{

    public class MachinedPartCostPriceValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Coid { get; set; }
        public ProductMaster MachinedPart { get; set; }
        public StockItemsAdvancedQuery MachinedPartFromCacheValue { get; set; }
        public decimal PieceWeightLbs => (MachinedPartFromCacheValue?.AvailablePieces > 0) ? MachinedPartFromCacheValue.AvailableWeightLbs / MachinedPartFromCacheValue.AvailablePieces : 0;
        public decimal PieceWeightKilos => (MachinedPartFromCacheValue?.AvailablePieces > 0) ? MachinedPartFromCacheValue.AvailableWeightKgs / MachinedPartFromCacheValue.AvailablePieces : 0;
        public string DisplayCurrency { get; set; } = "USD";
        public decimal ExchangeRate {
            get
            {
                var helperCurrency = new HelperCurrencyForIMetal();
                return helperCurrency.GetExchangeRateForCurrencyFromCoid(DisplayCurrency, Coid);
            }
        }
        public int Pieces { get; set; }

        public decimal PieceCost
        {
            get
            {
                var value = MachinedPartFromCacheValue?.TotalCost ?? 0;
                if (MachinedPartFromCacheValue.AvailablePieces > 0)
                {
                    value = value / MachinedPartFromCacheValue.AvailablePieces;
                }
                return value * ExchangeRate;
            }
        }

        private decimal _piecePrice = 0;

        public decimal PiecePrice
        {
            get => ((_piecePrice == 0) && (PieceCost > 0)) ? PieceCost : _piecePrice;
            set
            {
                if (_piecePrice != value)
                {
                    _piecePrice = value;
                }
            }
        }

        public decimal PiecePriceOverride { get; set; } = 0;

        public decimal Margin => QuoteCalculations.GetMargin(PieceCost, PiecePrice);

        public decimal MarginOverride { get; set; } = 0;

        public decimal TotalCost => PieceCost * Pieces;
        public decimal TotalPrice => PiecePrice * Pieces;

        public string Label { get; set; } = string.Empty;
            

        public MachinedPartCostPriceValue()
        {

        }

        public void Calculate()
        {

            if (MarginOverride > 0)
            {
                if (MarginOverride > 1)
                    MarginOverride = MarginOverride / 100;

                PiecePrice = QuoteCalculations.GetSalePriceFromMargin(PieceCost, MarginOverride);
                PiecePriceOverride = 0;
            }
            else if (PiecePriceOverride > 0)
            {
                PiecePrice = PiecePriceOverride;
                MarginOverride = 0;
            }
            
        }

        public MachinedPartCostPriceValue(string coid, int stockItemId, int pieces, string displayCurrency)
        {
            var productMasterResult = ProductMaster.FromStockId(coid, stockItemId);
            Coid = coid;
            MachinedPartFromCacheValue = productMasterResult.StockItem;
            MachinedPart = productMasterResult.ProductMaster;
            Label = MachinedPart.ProductCode;
            Pieces = pieces;
            DisplayCurrency = displayCurrency;
            if (!productMasterResult.ProductMaster.IsMachineComponent)
            {
                throw new Exception("This product is not a Machined Part");
            }
        }
    }
}