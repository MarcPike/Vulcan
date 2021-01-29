using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    [BsonIgnoreExtraElements]
    public class ItemSummaryViewModel
    {
        public int Index { get; set; }
        public bool IsQuickQuoteItem { get; set; }
        public bool IsMachinedPart { get; set; }
        public bool IsCrozCalcItem { get; set; }
        public bool IsLost { get; set; }
        public bool Regret { get; set; }
        public string StartingProductCode { get; set; }
        public string ProductCode { get; set; }
        public int Pieces { get; set; }
        public decimal Inches { get; set; }

        public string Units { get; set; } = string.Empty;

        public string PartNumber { get; set; }
        public string OemType { get; set; }
        public decimal Total { get; set; }

        public ItemSummaryViewModel()
        {

        }

        public ItemSummaryViewModel(QuoteItemModel model)
        {
            Index = model.Index;
            IsQuickQuoteItem = model.IsQuickQuoteItem;
            PartNumber = model.PartNumber;
            OemType = model.OemType;
            IsLost = model.IsLost;
            IsMachinedPart = model.IsMachinedPart;
            IsCrozCalcItem = model.IsCrozCalc;

            if (IsQuickQuoteItem)
            {
                Regret = model.QuickQuoteData.Regret;

                StartingProductCode = model.QuickQuoteData?.StartingProduct?.ProductCode ?? "Unknown";
                ProductCode = model.QuickQuoteData?.Label ?? model.QuickQuoteData?.FinishedProduct?.ProductCode ?? "Unknown";

                if (model.QuickQuoteData.RequiredQuantity != null)
                {
                    Inches = model.QuickQuoteData.RequiredQuantity.TotalInches();
                }
                else
                {
                    Inches = model.QuickQuoteData.OrderQuantity.GetTotalInches();
                }

                Total = model.QuickQuoteData.Price;
                Pieces = model.QuickQuoteData.OrderQuantity.Pieces;
                if (model.QuickQuoteData.OrderQuantity.QuantityType == "ea")
                {
                    Units =
                        $"piece(s)";
                }
                else
                {
                    Units =
                        $"{model.QuickQuoteData.OrderQuantity.Quantity} {model.QuickQuoteData.OrderQuantity.QuantityType}";
                }

            }
            if (model.IsCrozCalc)
            {

                var calcModel = model.CrozCalcItem;

                StartingProductCode = calcModel.StartingProductLabel;
                ProductCode = calcModel.FinishedProductLabel;
                Inches = calcModel.OrderQuantity.GetTotalInches();
                Regret = calcModel.Regret;
                Total = calcModel.TotalPrice;
                Pieces = calcModel.OrderQuantity.Pieces;
                if (calcModel.OrderQuantity.QuantityType == "ea")
                {
                    Units =
                        $"piece(s)";
                }
                else
                {
                    Units =
                        $"{calcModel.OrderQuantity.Quantity} {calcModel.OrderQuantity.QuantityType}";
                }

            }
            else if (IsMachinedPart)
            {
                StartingProductCode = model.MachinedPartModel.MachinedPartFromCacheValue.ProductCode;
                ProductCode = model.MachinedPartModel.MachinedPartFromCacheValue.ProductCode;
                Inches = 0;
                Pieces = model.MachinedPartModel.Pieces;
                Total = model.MachinedPartModel.TotalPrice;
                Units =
                    $"each";
            }
            else
            {
                StartingProductCode = model.QuotePriceModel?.StartingProduct?.ProductCode ?? "";
                ProductCode = model.QuotePriceModel?.FinishedProduct?.ProductCode ?? "";

                Inches = model.QuotePriceModel.MaterialCostValue.TotalInches;
                Pieces = (model.QuotePriceModel?.RequiredQuantity?.Pieces + model.CalculateQuotePriceModel.TestPieces?.Count) ?? 0;
                Total = (model.QuotePriceModel?.FinalPrice ?? 0);
                Units = $"{model.QuotePriceModel.MaterialCostValue.RequiredQuantity.PieceLength.Inches} inch";
            }

        }

        public ItemSummaryViewModel(CrmQuoteItem item)
        {

            Index = item.Index;
            IsQuickQuoteItem = item.IsQuickQuoteItem;
            PartNumber = item.PartNumber;
            OemType = item.OemType;
            IsLost = item.IsLost;
            IsMachinedPart = item.IsMachinedPart;
            IsCrozCalcItem = item.IsCrozCalc;

            if (IsQuickQuoteItem)
            {
                Regret = item.QuickQuoteData.Regret;

                StartingProductCode = item.QuickQuoteData?.StartingProduct?.ProductCode ?? "Unknown";
                ProductCode = item.QuickQuoteData?.Label ?? item.QuickQuoteData?.FinishedProduct?.ProductCode  ?? "Unknown";

                if (item.QuickQuoteData.RequiredQuantity != null)
                {
                    Inches = item.QuickQuoteData.RequiredQuantity.TotalInches();
                }
                else
                {
                    Inches = item.QuickQuoteData.OrderQuantity.GetTotalInches();
                }

                Total = item.QuickQuoteData.Price;
                Pieces = item.QuickQuoteData.OrderQuantity.Pieces;
                if (item.QuickQuoteData.OrderQuantity.QuantityType == "ea")
                {
                    Units =
                        $"piece(s)";
                }
                else
                {
                    Units =
                        $"{item.QuickQuoteData.OrderQuantity.Quantity} {item.QuickQuoteData.OrderQuantity.QuantityType}";
                }

            }
            else if (item.IsCrozCalc)
            {
                StartingProductCode = item.CrozCalcItem.StartingProductLabel;
                ProductCode = item.CrozCalcItem.FinishedProductLabel;
                Inches = item.CrozCalcItem.OrderQuantity.GetTotalInches();
                Regret = item.CrozCalcItem.Regret;

                Total = item.CrozCalcItem.TotalPrice;
                Pieces = item.CrozCalcItem.OrderQuantity.Pieces;
                if (item.CrozCalcItem.OrderQuantity.QuantityType == "ea")
                {
                    Units =
                        $"piece(s)";
                }
                else
                {
                    Units =
                        $"{item.CrozCalcItem.OrderQuantity.Quantity} {item.CrozCalcItem.OrderQuantity.QuantityType}";
                }

            }

            else if (IsMachinedPart)
            {
                StartingProductCode = item.MachinedPartModel.MachinedPartFromCacheValue.ProductCode;
                ProductCode = item.MachinedPartModel.MachinedPartFromCacheValue.ProductCode;
                Inches = 0;
                Pieces = item.MachinedPartModel.Pieces;
                Total = item.MachinedPartModel.TotalPrice;
                Units =
                    $"each";

            }
            else
            {
                StartingProductCode = item.QuotePrice?.StartingProduct?.ProductCode ?? "";
                ProductCode = item.QuotePrice?.FinishedProduct?.ProductCode ?? "";
                
                Inches = item.QuotePrice?.FinishQuantity.TotalInches() ?? 0;
                Pieces = (item.QuotePrice?.RequiredQuantity?.Pieces + item.QuotePrice?.TestPieces?.Count) ?? 0;
                Total = (item.QuotePrice?.FinalPrice ?? 0);
                Units = $"{item.QuotePrice?.MaterialCostValue.RequiredQuantity.PieceLength.Inches ?? 0} inch";

            }

        }
    }
}