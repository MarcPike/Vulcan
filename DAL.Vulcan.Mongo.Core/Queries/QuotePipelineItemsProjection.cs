using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Queries
{
    public struct QuotePipelineItemsProjection
    {
        public string Id { get; set; }
        public int Pieces { get; set; }
        public decimal Inches { get; set; }
        public string StartingProductCode { get; set; }
        public string FinishedProductCode { get; set; }
        public bool IsLost { get; set; }
        public bool Regret { get; set; }
        public string PartNumber { get; set; }
        public string PartSpecification { get; set; }
        public string OemType { get; set; }

        public QuotePipelineItemsProjection(CrmQuoteItem crmQuoteItem)
        {
            StartingProductCode = string.Empty;
            FinishedProductCode = string.Empty;
            Inches = 0;



            Id = crmQuoteItem.Id.ToString();
            if (crmQuoteItem.IsQuickQuoteItem)
            {
                FinishedProductCode = crmQuoteItem.QuickQuoteData.FinishedProduct?.ProductCode ?? crmQuoteItem.QuickQuoteData.Label ?? string.Empty;
                StartingProductCode = StartingProductCode ?? string.Empty;
                Pieces = crmQuoteItem.QuickQuoteData.OrderQuantity.Pieces;
                Regret = crmQuoteItem.QuickQuoteData.Regret;

            }
            else if (crmQuoteItem.IsMachinedPart)
            {
                FinishedProductCode = crmQuoteItem.MachinedPartModel.ProductMaster.ProductCode ?? string.Empty;
                StartingProductCode = FinishedProductCode;
                Pieces = crmQuoteItem.MachinedPartModel.Pieces;
                Regret = false;
            }
            else if (crmQuoteItem.IsCrozCalc)
            {
                FinishedProductCode = crmQuoteItem.CrozCalcItem.FinishedProductLabel;
                StartingProductCode = crmQuoteItem.CrozCalcItem.StartingProductLabel;
                Pieces = crmQuoteItem.CrozCalcItem.OrderQuantity.Pieces;
                Inches = 0;
                Regret = false;

            }
            else
            {
                FinishedProductCode = crmQuoteItem.QuotePrice?.FinishedProduct?.ProductCode ?? string.Empty;
                StartingProductCode = crmQuoteItem.QuotePrice?.StartingProduct?.ProductCode ?? string.Empty;
                Pieces = crmQuoteItem.QuotePrice.RequiredQuantity.Pieces + crmQuoteItem.QuotePrice.TestPieces.Count;
                Inches = crmQuoteItem.QuotePrice.RequiredQuantity.PieceLength.Inches;
                Regret = false;
            }

            OemType = crmQuoteItem.OemType;
            IsLost = crmQuoteItem.IsLost;
            PartNumber = crmQuoteItem.PartNumber;
            PartSpecification = crmQuoteItem.PartSpecification;
        }
    }
}