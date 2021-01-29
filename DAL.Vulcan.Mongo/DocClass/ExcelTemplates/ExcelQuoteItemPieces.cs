using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemPieces")]
    public class ExcelQuoteItemPieces: ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemPieces()
        {
            FieldName = "Pieces";
            Label = "Pieces";
            Id = "6C96253A-3E15-451A-AC1F-85F621BC5653";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var pieces = 0;
            var item = GetQuoteItemModel(model, index);
            if (item.IsQuickQuoteItem)
            {
                pieces = item.QuickQuoteData.OrderQuantity?.Pieces ?? 0;
            } else if (item.IsMachinedPart)
            {
                pieces = item.MachinedPartModel.Pieces;
            }
            else
            {
                pieces = item.QuotePriceModel.RequiredQuantity.Pieces;
            }

            return pieces.ToString();
        }
    }
}