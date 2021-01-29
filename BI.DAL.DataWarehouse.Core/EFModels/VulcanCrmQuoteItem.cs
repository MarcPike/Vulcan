using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VulcanCrmQuoteItem
    {
        public VulcanCrmQuoteItem()
        {
            VulcanCrmQuoteItemProductionCost = new HashSet<VulcanCrmQuoteItemProductionCost>();
            VulcanCrmQuoteItemTestPieces = new HashSet<VulcanCrmQuoteItemTestPieces>();
        }

        public int Id { get; set; }
        public int QuoteId { get; set; }
        public int QuoteItemId { get; set; }
        public string StartingProductCoid { get; set; }
        public int StartingProductId { get; set; }
        public decimal StartingOuterDiameter { get; set; }
        public decimal StartingInsideDiameter { get; set; }
        public string StartingProductCode { get; set; }
        public string StartingProductType { get; set; }
        public string StartingStockGrade { get; set; }
        public string StartingMetalCategory { get; set; }
        public string StartingProductCondition { get; set; }
        public string TagNumber { get; set; }
        public string FinishProductCoid { get; set; }
        public int FinishProductId { get; set; }
        public decimal FinishOuterDiameter { get; set; }
        public decimal FinishInsideDiameter { get; set; }
        public string FinishProductCode { get; set; }
        public string FinishProductType { get; set; }
        public string FinishStockGrade { get; set; }
        public string FinishMetalCategory { get; set; }
        public string FinishProductCondition { get; set; }
        public decimal CostPerInch { get; set; }
        public decimal CostPerPound { get; set; }
        public decimal CostPerKg { get; set; }
        public decimal CostPerPiece { get; set; }
        public int Pieces { get; set; }
        public decimal TotalPounds { get; set; }
        public decimal TotalKilograms { get; set; }
        public decimal KerfInchesPerCut { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal TotalFeet { get; set; }
        public decimal TotalInches { get; set; }
        public bool IsCostMadeup { get; set; }
        public decimal TheoWeight { get; set; }
        public decimal KerfTotalCost { get; set; }
        public decimal KerfTotalPrice { get; set; }
        public decimal TotalCutCost { get; set; }
        public decimal ProductionCostTotal { get; set; }
        public decimal ProductionPriceTotal { get; set; }
        public decimal TotalCost { get; set; }
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
        public string Currency { get; set; }
        public decimal TestPieceInches { get; set; }
        public decimal TestPiecesTotalPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginOverride { get; set; }
        public string OemType { get; set; }
        public string PartSpecification { get; set; }
        public string PartNumber { get; set; }
        public string PoNumber { get; set; }
        public string LeadTime { get; set; }
        public string CustomerNotes { get; set; }
        public string SalesPersonNotes { get; set; }
        public string LostReason { get; set; }
        public string LostProductCode { get; set; }
        public bool IsLost { get; set; }
        public string QuoteSource { get; set; }
        public bool IsQuickQuoteItem { get; set; }
        public string QuickQuoteItemLabel { get; set; }
        public decimal QuickQuoteItemCost { get; set; }
        public decimal QuickQuoteItemPrice { get; set; }
        public string QuickQuoteItemFinishedProductCode { get; set; }
        public int QuickQuoteItemPieces { get; set; }
        public decimal QuickQuoteItemQuantity { get; set; }
        public string QuickQuoteItemQuantityType { get; set; }
        public bool QuickQuoteItemRegret { get; set; }
        public string RequestedProductCode { get; set; }
        public decimal CutCostPerPiece { get; set; }
        public decimal CutCostPerPieceOverride { get; set; }
        public string FinalPriceOverrideType { get; set; }
        public decimal FinalPriceOverrideValue { get; set; }
        public string DisplayCurrency { get; set; }
        public decimal StartingPounds { get; set; }
        public decimal FinishPounds { get; set; }
        public decimal StartingKilograms { get; set; }
        public decimal FinishKilograms { get; set; }
        public decimal? FinalPrice { get; set; }
        public decimal FinalMargin { get; set; }
        public decimal PricePerEach { get; set; }
        public decimal MaterialOnlyCost { get; set; }
        public DateTime CreateDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
        public DateTime? ImportDateTimeUtc { get; set; }
        public DateTime? LostDateTimeUtc { get; set; }
        public string StartingProductSize { get; set; }
        public string FinishProductSize { get; set; }
        public string StartingProductCategory { get; set; }
        public string FinishedProductCategory { get; set; }
        public string HeatNumber { get; set; }
        public string BaseCurrency { get; set; }
        public decimal BaseCostPerInch { get; set; }
        public decimal BaseCostPerKg { get; set; }
        public decimal BaseCostPerPiece { get; set; }
        public decimal BaseCostPerPound { get; set; }
        public decimal BaseCutCostPerPiece { get; set; }
        public decimal BaseCutCostPerPieceOverride { get; set; }
        public decimal BaseFinalMargin { get; set; }
        public decimal BaseFinalPrice { get; set; }
        public decimal BaseFinalPriceOverrideValue { get; set; }
        public decimal BaseMaterialOnlyCost { get; set; }
        public decimal BaseMaterialTotalPrice { get; set; }
        public decimal BaseMaterialTotalPriceOverride { get; set; }
        public decimal BasePricePerEach { get; set; }
        public decimal BasePricePerFoot { get; set; }
        public decimal BasePricePerFootOverride { get; set; }
        public decimal BasePricePerInch { get; set; }
        public decimal BasePricePerInchOverride { get; set; }
        public decimal BasePricePerKilogram { get; set; }
        public decimal BasePricePerKilogramOverride { get; set; }
        public decimal BasePricePerPound { get; set; }
        public decimal BasePricePerPoundOverride { get; set; }
        public decimal BaseProductionCostTotal { get; set; }
        public decimal BaseProductionPriceTotal { get; set; }
        public decimal BaseTotalCost { get; set; }
        public decimal BaseTotalCutCost { get; set; }
        public decimal BaseKerfTotalCost { get; set; }
        public decimal BaseKerfTotalPrice { get; set; }
        public decimal BaseTestPiecesTotalPrice { get; set; }
        public decimal BaseQuickQuoteItemCost { get; set; }
        public decimal BaseQuickQuoteItemPrice { get; set; }
        public string LostComments { get; set; }

        public virtual ICollection<VulcanCrmQuoteItemProductionCost> VulcanCrmQuoteItemProductionCost { get; set; }
        public virtual ICollection<VulcanCrmQuoteItemTestPieces> VulcanCrmQuoteItemTestPieces { get; set; }
    }
}
