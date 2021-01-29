using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemUnitPrice")]
    public class ExcelQuoteItemUnitPrice: ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemUnitPrice()
        {
            FieldName = "UnitPrice";
            Label = "Unit Price";
            Id = "94301AC1-2D52-4BB2-BD8B-3D751FB157CF";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            decimal unitPrice = 0;
            if (item.IsQuickQuoteItem)
            {
                unitPrice = item.QuickQuoteData.UnitPrice.RoundAndNormalize(3);
            }
            else if (item.IsMachinedPart)
            {
                unitPrice = item.MachinedPartModel.PiecePrice.RoundAndNormalize(3);
            }
            else if (item.IsCrozCalc)
            {
                unitPrice = item.CrozCalcItem.UnitPrice;
            }
            else
            {
                unitPrice = item.QuotePriceModel.FinalPriceOverride.FinalUnitCost.RoundAndNormalize(3);
                //unitPrice = item.CalculateQuotePriceModel.MaterialPriceValue.TotalPrice /
                //                item.CalculateQuotePriceModel.RequiredQuantity.Pieces;
            }

            return unitPrice.ToString("0.000");

        }
    }
}