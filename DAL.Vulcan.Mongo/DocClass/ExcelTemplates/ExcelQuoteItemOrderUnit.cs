using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemOrderUnit")]
    public class ExcelQuoteItemOrderUnit : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemOrderUnit()
        {
            FieldName = "OrderUnit";
            Label = "Unit";
            Id = "20B37CA9-8438-4722-A9BE-6EC932ABED54";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            if (item.IsQuickQuoteItem)
            {
                return item.QuickQuoteData.OrderQuantity.QuantityType;
            }
            else if (item.IsMachinedPart)
            {
                return "each";
            }
            else
            {
                return item.CalculateQuotePriceModel.OrderQuantity.QuantityType;
            }

        }
    }
}