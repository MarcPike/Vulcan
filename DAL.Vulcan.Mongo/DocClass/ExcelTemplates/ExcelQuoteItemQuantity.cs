using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemQuantity")]
    public class ExcelQuoteItemQuantity: ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemQuantity()
        {
            FieldName = "Quantity";
            Label = "Quantity";
            Id = "5E94724C-7A40-4196-AE79-652E4D5B13E7";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            decimal quantity = 0;
            var item = GetQuoteItemModel(model, index);
            if (item.IsQuickQuoteItem)
            {
                if (item.QuickQuoteData.OrderQuantity.EachBased)
                {
                    return "piece(s)";
                }
                else
                {
                    return item.QuickQuoteData.QuantityValueForPdf().ToString("0.000");
                }
            }
            else if (item.IsMachinedPart)
            {
                quantity = item.MachinedPartModel.PieceWeightLbs;
            }
            else
            {
                quantity = item.CalculateQuotePriceModel.OrderQuantity.Quantity;
            }

            return quantity.ToString();
        }
    }

}
