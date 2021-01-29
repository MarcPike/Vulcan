using System;
using System.Linq;
using DAL.iMetal.Core.Helpers;
using DAL.iMetal.Core.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{

    [BsonIgnoreExtraElements]
    public class MachinedPartCostPriceValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Coid { get; set; }
        public ProductMastersQuery ProductMaster { get; set; }
        public StockItemsQuery StockItem { get; set; }
        public decimal PieceWeightLbs => (StockItem?.AvailablePieces > 0) ? StockItem.AvailableWeightLbs / StockItem.AvailablePieces : 0;
        public decimal PieceWeightKilos => (StockItem?.AvailablePieces > 0) ? StockItem.AvailableWeightKgs / StockItem.AvailablePieces : 0;
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
                var value = StockItem?.TotalCost ?? 0;
                if (StockItem.AvailablePieces > 0)
                {
                    value = value / StockItem.AvailablePieces;
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
            var stockItemResult = StockItemsQuery.GetForId(coid, stockItemId).FirstOrDefault();
            var productMasterResult = ProductMastersQuery.GetForId(coid, stockItemId);
            Coid = coid;
            StockItem = stockItemResult;
            ProductMaster = productMasterResult;
            Label = ProductMaster.ProductCode;
            Pieces = pieces;
            DisplayCurrency = displayCurrency;
            if (!productMasterResult.IsMachineComponent)
            {
                throw new Exception("This product is not a Machined Part");
            }
        }
    }
}