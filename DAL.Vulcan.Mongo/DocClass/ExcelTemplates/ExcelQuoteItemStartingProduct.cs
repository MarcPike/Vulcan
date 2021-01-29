using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemStartingProduct")]
    public class ExcelQuoteItemStartingProduct : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemStartingProduct()
        {
            FieldName = "StartingProductCode";
            Label = "Starting Product";
            Id = "3FB6CFF8-25C2-4801-90FB-2BA5854CA78E";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            if (item.IsQuickQuoteItem)
            {
                return item.QuickQuoteData.StartingProduct?.ProductCode ?? string.Empty;
            } else if (item.IsMachinedPart)
            {
                return item.MachinedPartModel.MachinedPart?.ProductCode ?? string.Empty;
            }

            return item.QuotePriceModel.StartingProduct?.ProductCode ?? string.Empty;
        }
    }
}